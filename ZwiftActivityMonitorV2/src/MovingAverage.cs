using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ZwiftActivityMonitorV2
{
    public class MovingAverage
    {
        private readonly ILogger<MovingAverage> Logger;
        private UserProfile CurrentUser;

        private readonly Queue<Statistics> mStatsQueue;
        private readonly DurationType mDurationType;
        private readonly bool mExcludeZeroPowerValues;

        private long mWattsSum;
        private long mHRbpmSum;
        private int  mAPwatts;
        private int  mHRbpm;
        private int mAPwattsMax;
        private double? mAPwattsPerKgMax;
        private int  mHRbpmMax;
        private int  mDuration; // how long to store recorded readings
        private bool mStarted;
        private long mSampleWattsSumAll;
        private int  mSampleCountAll;
        private int  mDistanceSeedValue; // the PlayerState.Distance value when first started

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
            Logger = ZAMsettings.LoggerFactory.CreateLogger<MovingAverage>();

            mDurationType = durationType;
            mDuration = (int)durationType; 
            mExcludeZeroPowerValues = excludeZeroPowerValues;

            mStatsQueue = new Queue<Statistics>();

            if (mDuration <= 30 && allowHighRes)
            {
                ZAMsettings.ZPMonitorService.HighResRiderStateEvent += RiderStateEventHandler;
                Logger.LogInformation($"{mDuration} seconds moving average collector using high-res packets.");
            }
            else
            {
                ZAMsettings.ZPMonitorService.RiderStateEvent += RiderStateEventHandler;
            }

            ZAMsettings.ZPMonitorService.CollectionStatusChanged += ZPMonitorService_CollectionStatusChanged;
        }

        private void ZPMonitorService_CollectionStatusChanged(object sender, CollectionStatusChangedEventArgs e)
        {
            Debug.WriteLine($"{this.GetType()}.ZPMonitorService_CollectionStatusChanged - {e.Action}");

            if (e.Action == CollectionStatusChangedEventArgs.ActionType.Started)
                this.Start();
            else if (e.Action == CollectionStatusChangedEventArgs.ActionType.Stopped)
                this.Stop();
        }

        private void Start()
        {
            if (!mStarted)
            {
                mWattsSum = 0;
                mHRbpmSum = 0;
                mAPwatts = 0;
                mHRbpm = 0;
                mAPwattsMax = 0;
                mAPwattsPerKgMax = 0;
                mHRbpmMax = 0;
                mSampleWattsSumAll = 0;
                mSampleCountAll = 0;
                mStatsQueue.Clear();
                this.CurrentUser = ZAMsettings.Settings.CurrentUser;

                mStarted = true;
            }
        }

        private void Stop()
        {
            if (mStarted)
            {
                mStarted = false;
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
            //int curAvgPower;
            //int curAvgHR;
            bool calculateMax = false;
            bool triggerMax = false;

            if (!mStarted)
                return;

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
                int apSampleWatts = (int)Math.Round(mSampleWattsSumAll / (double)mSampleCountAll, 0);
                double? apSampleWattsPerKg = this.CalculateUserWattsPerKg(apSampleWatts);

                // Calculate average speed, distance is given in meters.
                double distanceKm = (e.Distance - mDistanceSeedValue) / 1000.0;
                double distanceMi = distanceKm / 1.609;
                double speedKph = Math.Round((distanceKm / e.ElapsedTime.Value.TotalSeconds) * 3600, 1);
                double speedMph = Math.Round((distanceMi / e.ElapsedTime.Value.TotalSeconds) * 3600, 1);

                OnMetricsCalculatedEvent(new MetricsCalculatedEventArgs(apSampleWatts, apSampleWattsPerKg, speedKph, speedMph, e.ElapsedTime.Value, distanceKm, distanceMi));
            }

            // If power is zero and excluding values, exit
            if (mExcludeZeroPowerValues && stats.Power == 0)
                return;
            
            // Remove any queue items which are older than the set time duration
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

            // calculate averages
            int curAvgPower = (int)Math.Round(mWattsSum / (double)mStatsQueue.Count, 0);
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
                OnMovingAverageCalculatedEvent(new MovingAverageCalculatedEventArgs(curAvgPower, mDurationType, e.ElapsedTime.Value));
            }

            // if either average power or average HR changed, trigger event
            if (curAvgPower != mAPwatts || curAvgHR != mHRbpm)
            {
                mAPwatts = curAvgPower;
                mHRbpm = curAvgHR;

                // The FTP column will track the current average power until the time duration is satisfied (mAPwattsMax set).
                // This enables the rider to see what his FTP would be real-time.

                // calculate FTP watts and w/kg based on current average power
                int ftpWatts = (int)Math.Round(curAvgPower * 0.95, 0);
                double? ftpWattsPerKg = this.CalculateUserWattsPerKg(ftpWatts);

                bool ignoreFTP = mAPwatts > 0;

                OnMovingAverageChangedEvent(new MovingAverageChangedEventArgs(curAvgPower, curAvgWkg, curAvgHR, mDurationType, ftpWatts, ftpWattsPerKg, ignoreFTP));
            }

            // if either max average power or max HR changed, trigger event
            if (triggerMax)
            {
                // Once the time duration is satisfied, FTP will no longer use current average power, it will use the maximum average power.

                // calculate FTP watts and w/kg based on max average power
                int ftpWattsMax = (int)Math.Round(mAPwattsMax * 0.95, 0);
                double? ftpWattsPerKgMax = this.CalculateUserWattsPerKg(ftpWattsMax);

                OnMovingAverageMaxChangedEvent(new MovingAverageMaxChangedEventArgs(mAPwattsMax, mAPwattsPerKgMax, mHRbpmMax, mDurationType, ftpWattsMax, ftpWattsPerKgMax));
            }

            //Logger.LogInformation($"id: {e.PlayerState.Id} watch: {e.PlayerState.WatchingRiderId} power: {stats.Power} HR: {stats.HeartRate} Count: {m_statsQueue.Count} Sum: {m_sumTotal} Avg: {PowerAvg} Oldest: {oldest.TotalSeconds} TTP: {(DateTime.Now - start).TotalMilliseconds} WorldTime: {e.PlayerState.WorldTime} ");
            //Logger.LogInformation($"id: {e.PlayerState.Id} power: {stats.Power} HR: {stats.HeartRate} Count: {m_statsQueue.Count} PowerAvg: {curAvgPower} HRAvg: {curAvgHR} PowerMax: {m_maxAvgPower} HRMax: {m_maxAvgHR} Oldest: {oldest.TotalSeconds} TTP: {(DateTime.Now - start).TotalMilliseconds} WorldTime: {e.PlayerState.WorldTime} ");
        }

        private double? CalculateUserWattsPerKg(int watts)
        {
            return CurrentUser.WeightAsKgs > 0 ? Math.Round(watts / CurrentUser.WeightAsKgs, 2) : null;
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
                    Logger.LogWarning(ex, ex.ToString());
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
                    Logger.LogWarning(ex, ex.ToString());
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
                    Logger.LogWarning(ex, ex.ToString());
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
                    Logger.LogWarning(ex, ex.ToString());
                }
            }
        }
    }
}
