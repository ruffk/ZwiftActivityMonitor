using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ZwiftActivityMonitorV2
{
    public class MovingAverage
    {
        private readonly ILogger<MovingAverage> Logger;
        private UserProfile CurrentUserProfile { get { return ZAMsettings.Settings.CurrentUser; } }

        private readonly Queue<Statistics> mStatsQueue;
        private readonly DurationType mDurationType;
        private readonly bool mExcludeZeroPowerValues;

        private long mWattsSum;
        private long mHRbpmSum;
        private double  mAPwatts;
        private int  mHRbpm;
        private double mAPwattsMax;
        private double? mAPwattsPerKgMax;
        private int  mHRbpmMax;
        private int  mDuration; // how long to store recorded readings
        private bool mStarted;
        private long mSampleWattsSumAll;
        private int  mSampleCountAll;
        private int  mDistanceSeedValue; // the PlayerState.Distance value when first started
        private bool mWaitingOnPauseResume; // Collection Paused status received, waiting on Resumed status

        public event EventHandler<MovingAverageChangedEventArgs> MovingAverageChangedEvent;
        public event EventHandler<MovingAverageMaxChangedEventArgs> MovingAverageMaxChangedEvent;
        public event EventHandler<MetricsCalculatedEventArgs> MetricsCalculatedEvent;
        public event EventHandler<MovingAverageCalculatedEventArgs> MovingAverageCalculatedEvent;


        #region Internal Classes

        internal class DurationDetail
        {
            private DurationType m_type;
            private string m_label;
            private int m_seconds;

            public DurationDetail(DurationType type, string label, int seconds)
            {
                m_type = type;
                m_label = label;
                m_seconds = seconds;
            }

            public DurationType Type { get { return m_type; } }
            public string Label { get { return m_label; } }
            public int Seconds { get { return m_seconds; } }
        }

        internal class Statistics
        {
            private int m_power;
            private int m_heartRate;
            private DateTime m_timestamp;

            public Statistics(int power, int heartRate)
            {
                m_power = power;
                m_heartRate = heartRate;
                m_timestamp = DateTime.Now;
            }

            public int Power
            {
                get { return m_power; }
            }

            public int HeartRate
            {
                get { return m_heartRate; }
            }

            public DateTime Timestamp
            {
                get { return m_timestamp; }
                set { m_timestamp = value; }
            }
        }
        #endregion


        /// <summary>
        /// Generic moving average collector
        /// </summary>
        /// <param name="durationType"></param>The duration of collection
        /// <param name="excludeZeroPowerValues"></param>Whether to exclude zeros when collecting
        /// <param name="allowHighRes"></param>Whether to allow use of high-res packets.  Currently only collectors under 30 seconds use these. 
        public MovingAverage(DurationType durationType, bool excludeZeroPowerValues = false, bool allowHighRes = true)
        {
            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<MovingAverage>();

            mDurationType = durationType;
            mDuration = (int)durationType; 
            mExcludeZeroPowerValues = excludeZeroPowerValues;

            mStatsQueue = new Queue<Statistics>();

            if (mDuration <= 30 && allowHighRes)
            {
                ZAMsettings.ZPMonitorService.HighResRiderStateEvent += RiderStateEventHandler;
                Logger.LogDebug($"{mDuration} seconds moving average collector using high-res packets.");
            }
            else
            {
                ZAMsettings.ZPMonitorService.RiderStateEvent += RiderStateEventHandler;
            }

            ZAMsettings.ZPMonitorService.CollectionStatusChanged += ZPMonitorService_CollectionStatusChanged;
        }


        private void ZPMonitorService_CollectionStatusChanged(object sender, CollectionStatusChangedEventArgs e)
        {
            Logger.LogDebug($"{this.GetType()}.ZPMonitorService_CollectionStatusChanged - Collector: {this.mDurationType}, Action: {e.Action}");

            switch (e.Action)
            {
                case CollectionStatusChangedEventArgs.ActionType.Waiting:
                    this.ResetValues();
                    break;

                case CollectionStatusChangedEventArgs.ActionType.Started:
                    this.Start();
                    break;

                case CollectionStatusChangedEventArgs.ActionType.Stopped:
                    this.Stop();
                    break;

                case CollectionStatusChangedEventArgs.ActionType.Paused:
                    this.mWaitingOnPauseResume = true;
                    break;

                case CollectionStatusChangedEventArgs.ActionType.Resumed:
                    // allow collector to offset current queue entry timestamps
                    this.OnPauseResumed(e.PauseDuration.Value);
                    this.mWaitingOnPauseResume = false;
                    break;
            }
        }

        private void Start()
        {
            if (!this.mStarted)
            {
                this.ResetValues();

                this.mStarted = true;
            }
        }

        private void ResetValues()
        {
            this.mWattsSum = 0;
            this.mHRbpmSum = 0;
            this.mAPwatts = 0;
            this.mHRbpm = 0;
            this.mAPwattsMax = 0;
            this.mAPwattsPerKgMax = 0;
            this.mHRbpmMax = 0;
            this.mSampleWattsSumAll = 0;
            this.mSampleCountAll = 0;
            this.mWaitingOnPauseResume = false;
            this.mStatsQueue.Clear();
        }

        private void Stop()
        {
            if (this.mStarted)
            {
                this.mStarted = false;
                this.mWaitingOnPauseResume = false;
            }
        }

        /// <summary>
        /// Handle player state changes.  Calculates moving averages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RiderStateEventHandler(object sender, RiderStateEventArgs e)
        {
            DateTime now = DateTime.Now; // fixed current time
            bool calculateMax = false;
            bool triggerMax = false;

            // AdjustedCollectionTime will be null if Monitoring but not Collecting
            if (!this.mStarted || e.AdjustedCollectionTime == null || e.IsPaused || this.mWaitingOnPauseResume)
            {
                //if (this.mDuration == 60)
                //{
                //    Debug.WriteLine($"MovingAverage not collecting - Duration: {this.mDuration}");
                //}
                return;
            }
            else
            {
                //Debug.WriteLine($"MovingAverage collecting - Duration: {this.mDuration}");
            }

            // the Statistics class captures the values we want to measure
            var stats = new Statistics(e.Power, e.Heartrate);

            if (mSampleCountAll == 0)
            {
                // Capture the current distance traveled value to use as an offset to each successive distance value.
                mDistanceSeedValue = e.Distance;
            }

            // To keep track of overall average power.  Performing here as zeros count.
            mSampleWattsSumAll += (long)stats.Power;
            mSampleCountAll += 1;

            if (MetricsCalculatedEvent != null)
            {
                double apSampleWatts = mSampleWattsSumAll / (double)mSampleCountAll;
                double? apSampleWattsPerKg = this.CalculateUserWattsPerKg(apSampleWatts);

                // Calculate average speed, distance is given in meters.
                double distanceKm = (e.Distance - mDistanceSeedValue) / 1000.0;
                double distanceMi = distanceKm / 1.609;
                double speedKph = Math.Round((distanceKm / e.AdjustedCollectionTime.Value.TotalSeconds) * 3600, 1);
                double speedMph = Math.Round((distanceMi / e.AdjustedCollectionTime.Value.TotalSeconds) * 3600, 1);
                distanceKm = Math.Round(distanceKm, 1);
                distanceMi = Math.Round(distanceMi, 1);

                OnMetricsCalculatedEvent(new MetricsCalculatedEventArgs((int)Math.Round(apSampleWatts, 0), apSampleWattsPerKg, speedKph, speedMph, e.AdjustedCollectionTime.Value, distanceKm, distanceMi));
            }

            // If power is zero and excluding values, exit
            if (mExcludeZeroPowerValues && stats.Power == 0)
                return;

            // Remove any queue items which are older than the set time duration
            lock (this)
            {
                while (mStatsQueue.Count > 0)
                {
                    // look at front of queue
                    var peekStats = mStatsQueue.Peek();

                    // determine time difference between the newest item and this oldest item
                    TimeSpan oldest = stats.Timestamp - peekStats.Timestamp;

                    // if queue isn't at capacity yet, exit loop
                    if (oldest.TotalSeconds <= mDuration)
                        break;

                    // subtract oldest entry from values and dequeue
                    mWattsSum -= (long)peekStats.Power;
                    mHRbpmSum -= (long)peekStats.HeartRate;
                    mStatsQueue.Dequeue();

                    calculateMax = true;  // we have a full sample, calculate maximums
                }

                // add this new item to the queue
                mStatsQueue.Enqueue(stats);
                mWattsSum += (long)stats.Power;
                mHRbpmSum += (long)stats.HeartRate;
            }

            // calculate averages
            double curAvgPower = mWattsSum / (double)mStatsQueue.Count;
            double? curAvgWkg = this.CalculateUserWattsPerKg(curAvgPower);

            int curAvgHR = (int)Math.Round(mHRbpmSum / (double)mStatsQueue.Count, 0);

            // if queue was full, check to see if we have any new max values
            if (calculateMax)
            {
                if (curAvgPower > mAPwattsMax)
                {
                    mAPwattsMax = curAvgPower;
                    mAPwattsPerKgMax = curAvgWkg;
                    triggerMax = true;
                }
                if (curAvgHR > mHRbpmMax)
                {
                    mHRbpmMax = curAvgHR;
                    triggerMax = true;
                }

                // Since buffer was full trigger an event so consumer can use this new average (without waiting for a change).  Used by NormalizedPower
                OnMovingAverageCalculatedEvent(new MovingAverageCalculatedEventArgs((int)Math.Round(curAvgPower, 0), mDurationType, e.AdjustedCollectionTime.Value));
            }

            // if either average power or average HR changed, trigger event
            if (curAvgPower != mAPwatts || curAvgHR != mHRbpm)
            {
                mAPwatts = curAvgPower;
                mHRbpm = curAvgHR;

                // The FTP column will track the current average power until the time duration is satisfied (mAPwattsMax set).
                // This enables the rider to see what his FTP would be real-time.

                // calculate FTP watts and w/kg based on current average power
                double ftpWatts = curAvgPower * 0.95;
                double? ftpWattsPerKg = this.CalculateUserWattsPerKg(ftpWatts);
                ftpWatts = Math.Round(ftpWatts, 0);

                bool ignoreFTP = mAPwattsMax > 0;

                OnMovingAverageChangedEvent(new MovingAverageChangedEventArgs((int)Math.Round(curAvgPower, 0), curAvgWkg, curAvgHR, mDurationType, (int)ftpWatts, ftpWattsPerKg, ignoreFTP));
            }

            // if either max average power or max HR changed, trigger event
            if (triggerMax)
            {
                // Once the time duration is satisfied, FTP will no longer use current average power, it will use the maximum average power.

                // calculate FTP watts and w/kg based on max average power
                double ftpWattsMax = mAPwattsMax * 0.95;
                double? ftpWattsPerKgMax = this.CalculateUserWattsPerKg(ftpWattsMax);
                ftpWattsMax = Math.Round(ftpWattsMax, 0);

                OnMovingAverageMaxChangedEvent(new MovingAverageMaxChangedEventArgs((int)Math.Round(mAPwattsMax, 0), mAPwattsPerKgMax, mHRbpmMax, mDurationType, (int)ftpWattsMax, ftpWattsPerKgMax));
            }

            //Logger.LogDebug($"id: {e.PlayerState.Id} watch: {e.PlayerState.WatchingRiderId} power: {stats.Power} HR: {stats.HeartRate} Count: {m_statsQueue.Count} Sum: {m_sumTotal} Avg: {PowerAvg} Oldest: {oldest.TotalSeconds} TTP: {(DateTime.Now - start).TotalMilliseconds} WorldTime: {e.PlayerState.WorldTime} ");
            //Logger.LogDebug($"id: {e.PlayerState.Id} power: {stats.Power} HR: {stats.HeartRate} Count: {m_statsQueue.Count} PowerAvg: {curAvgPower} HRAvg: {curAvgHR} PowerMax: {m_maxAvgPower} HRMax: {m_maxAvgHR} Oldest: {oldest.TotalSeconds} TTP: {(DateTime.Now - start).TotalMilliseconds} WorldTime: {e.PlayerState.WorldTime} ");
        }

        /// <summary>
        /// When resuming after a pause, the items in the queue need to have their timestamp's incremented by the pause duration.
        /// This is to keep them all from immediately expiring and being removed from the queue.
        /// </summary>
        /// <param name="pauseDuration"></param>
        private void OnPauseResumed(TimeSpan pauseDuration)
        {
            lock (this)
            {
                foreach (var item in this.mStatsQueue)
                {
                    item.Timestamp += pauseDuration;
                }
            }
        }

        private double? CalculateUserWattsPerKg(double watts)
        {
            return CurrentUserProfile.WeightAsKgs > 0 ? Math.Round(watts / CurrentUserProfile.WeightAsKgs, 2) : null;
        }

        private void OnMovingAverageChangedEvent(MovingAverageChangedEventArgs e)
        {
            EventHandler<MovingAverageChangedEventArgs> handler = MovingAverageChangedEvent;

            if (handler != null)
            {
                try
                {
                    handler(this, e);
                }
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnMovingAverageChangedEvent)");
                }
            }
        }
        private void OnMovingAverageCalculatedEvent(MovingAverageCalculatedEventArgs e)
        {
            EventHandler<MovingAverageCalculatedEventArgs> handler = MovingAverageCalculatedEvent;

            if (handler != null)
            {
                try
                {
                    handler(this, e);
                }
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnMovingAverageCalculatedEvent)");
                }
            }
        }
        private void OnMovingAverageMaxChangedEvent(MovingAverageMaxChangedEventArgs e)
        {
            EventHandler<MovingAverageMaxChangedEventArgs> handler = MovingAverageMaxChangedEvent;

            if (handler != null)
            {
                try
                {
                    handler(this, e);
                }
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnMovingAverageMaxChangedEvent)");
                }
            }
        }
        private void OnMetricsCalculatedEvent(MetricsCalculatedEventArgs e)
        {
            EventHandler<MetricsCalculatedEventArgs> handler = MetricsCalculatedEvent;

            if (handler != null)
            {
                try
                {
                    handler(this, e);
                }
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnMetricsCalculatedEvent)");
                }
            }
        }
    }
}
