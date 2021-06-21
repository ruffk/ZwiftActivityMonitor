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

        private long mSumPower;
        private long mSumHR;
        private int  mCurAvgPower;
        private int  mCurAvgHR;
        private int mMaxAvgPower;
        private double mMaxAvgWkg;
        private int  mMaxAvgHR;
        private int  mDuration; // how long to store recorded readings
        private bool mStarted;
        private long mSumOverallPower;
        private int  mCountOverallPowerSamples;
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
                mSumPower = 0;
                mSumHR = 0;
                mCurAvgPower = 0;
                mCurAvgHR = 0;
                mMaxAvgPower = 0;
                mMaxAvgWkg = 0;
                mMaxAvgHR = 0;
                mSumOverallPower = 0;
                mCountOverallPowerSamples = 0;
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
            int curAvgPower;
            int curAvgHR;
            int maxAvgPower;
            int maxAvgHR;
            bool calculateMax = false;
            bool triggerMax = false;
            double curAvgWkg = 0;

            if (!mStarted)
                return;

            // the Statistics class captures the values we want to measure
            var stats = new Statistics(e.Power, e.Heartrate);

            if (mCountOverallPowerSamples == 0)
            {
                // Capture the current distance traveled value to use as an offset to each successive distance value.
                mDistanceSeedValue = e.Distance;
            }

            // To keep track of overall average power.  Performing here as zeros count.
            mSumOverallPower += (long)stats.Power;
            mCountOverallPowerSamples += 1;

            if (MetricsCalculatedEvent != null)
            {
                int apWatts = (int)Math.Round(mSumOverallPower / (double)mCountOverallPowerSamples, 0);

                double apWattsPerKg = 0;

                // calculate average w/kg
                if (CurrentUser.WeightAsKgs > 0)
                    apWattsPerKg = Math.Round(apWatts / CurrentUser.WeightAsKgs, 2);

                // Calculate average speed, distance is given in meters.
                //TimeSpan runningTime = (DateTime.Now - mCollectionStart);
                double distanceKm = (e.Distance - mDistanceSeedValue) / 1000.0;
                double distanceMi = distanceKm / 1.609;
                double speedKph = Math.Round((distanceKm / e.ElapsedTime.Value.TotalSeconds) * 3600, 1);
                double speedMph = Math.Round((distanceMi / e.ElapsedTime.Value.TotalSeconds) * 3600, 1);

                OnMetricsCalculatedEvent(new MetricsCalculatedEventArgs(apWatts, apWattsPerKg, speedKph, speedMph, e.ElapsedTime.Value, distanceKm, distanceMi));
            }

            // If power is zero and excluding values, exit
            if (mExcludeZeroPowerValues && stats.Power == 0)
                return;
            
            // Initialize with current max values.  This is so we know if a new max has occurred.
            maxAvgPower = mMaxAvgPower;
            maxAvgHR = mMaxAvgHR;

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
                mSumPower -= (long)peekStats.Power;
                mSumHR -= (long)peekStats.HeartRate;
                mStatsQueue.Dequeue();

                calculateMax = true;  // we have a full sample, calculate maximums
            }

            // add this new item to the queue
            mStatsQueue.Enqueue(stats);
            mSumPower += (long)stats.Power;
            mSumHR += (long)stats.HeartRate;

            // calculate averages
            curAvgPower = (int)Math.Round(mSumPower / (double)mStatsQueue.Count, 0);
            curAvgHR = (int)Math.Round(mSumHR / (double)mStatsQueue.Count, 0);

            // calculate average w/kg
            if (CurrentUser.WeightAsKgs > 0)
                curAvgWkg = Math.Round(curAvgPower / CurrentUser.WeightAsKgs, 2);

            // if queue was full, check to see if we have any new max values
            if (calculateMax)
            {
                if (curAvgPower > mMaxAvgPower)
                {
                    mMaxAvgPower = curAvgPower;
                    mMaxAvgWkg = curAvgWkg;
                    triggerMax = true;
                }
                if (curAvgHR > mMaxAvgHR)
                {
                    mMaxAvgHR = curAvgHR;
                    triggerMax = true;
                }

                // Since buffer was full trigger an event so consumer can use this new average (without waiting for a change).  Used by NormalizedPower
                OnMovingAverageCalculatedEvent(new MovingAverageCalculatedEventArgs(curAvgPower, mDurationType, e.ElapsedTime.Value));
            }

            // if either average power or average HR changed, trigger event
            if (curAvgPower != mCurAvgPower || curAvgHR != mCurAvgHR)
            {
                mCurAvgPower = curAvgPower;
                mCurAvgHR = curAvgHR;

                OnMovingAverageChangedEvent(new MovingAverageChangedEventArgs(curAvgPower, curAvgHR, mDurationType, curAvgWkg));
            }

            // if either max average power or max HR changed, trigger event
            if (triggerMax)
            {
                OnMovingAverageMaxChangedEvent(new MovingAverageMaxChangedEventArgs(mMaxAvgPower, mMaxAvgHR, mDurationType, mMaxAvgWkg));
            }

            //Logger.LogInformation($"id: {e.PlayerState.Id} watch: {e.PlayerState.WatchingRiderId} power: {stats.Power} HR: {stats.HeartRate} Count: {m_statsQueue.Count} Sum: {m_sumTotal} Avg: {PowerAvg} Oldest: {oldest.TotalSeconds} TTP: {(DateTime.Now - start).TotalMilliseconds} WorldTime: {e.PlayerState.WorldTime} ");
            //Logger.LogInformation($"id: {e.PlayerState.Id} power: {stats.Power} HR: {stats.HeartRate} Count: {m_statsQueue.Count} PowerAvg: {curAvgPower} HRAvg: {curAvgHR} PowerMax: {m_maxAvgPower} HRMax: {m_maxAvgHR} Oldest: {oldest.TotalSeconds} TTP: {(DateTime.Now - start).TotalMilliseconds} WorldTime: {e.PlayerState.WorldTime} ");
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
