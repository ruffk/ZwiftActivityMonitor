using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitor
{

    #region DurationType
    public enum DurationType
    {
        FiveSeconds,
        ThirtySeconds,
        OneMinute,
        FiveMinutes,
        SixMinutes,
        TenMinutes,
        TwentyMinutes,
        ThirtyMinutes,
        SixtyMinutes,
        NinetyMinutes
    }
    #endregion

    public class MovingAverage
    {
        private readonly ILogger<MovingAverage> Logger;
        private readonly Queue<Statistics> m_statsQueue;
        private readonly DurationType m_durationType;
        private readonly bool m_excludeZeroPowerValues;

        private long m_sumPower;
        private long m_sumHR;
        private int  m_curAvgPower;
        private int  m_curAvgHR;
        private int  m_maxAvgPower;
        private int  m_maxAvgHR;
        private int  m_duration; // how long to store recorded readings
        private bool m_started;
        private long m_sumOverallPower;
        private int  m_countOverallPowerSamples;
        private int  m_distanceSeedValue; // the PlayerState.Distance value when first started
        private DateTime m_collectionStart;

        #region EventArgs
        public class MovingAverageChangedEventArgs : EventArgs
        {
            private int m_avgPower;
            private int m_avgHR;
            private DurationType m_durationType;

            public MovingAverageChangedEventArgs(int avgPower, int avgHR, DurationType durationType)
            {
                m_avgPower = avgPower;
                m_avgHR = avgHR;
                m_durationType = durationType;
            }

            public int AveragePower
            {
                get { return m_avgPower; }
            }
            public int AverageHR
            {
                get { return m_avgHR; }
            }

            public DurationType DurationType { get { return m_durationType; } }
        }
        public class MovingAverageMaxChangedEventArgs : EventArgs
        {
            private int m_avgPower;
            private int m_avgHR;
            private DurationType m_durationType;

            public MovingAverageMaxChangedEventArgs(int avgPower, int avgHR, DurationType durationType)
            {
                m_avgPower = avgPower;
                m_avgHR = avgHR;
                m_durationType = durationType;
            }

            public int MaxAvgPower
            {
                get { return m_avgPower; }
            }
            public int MaxAvgHR
            {
                get { return m_avgHR; }
            }
            public DurationType DurationType { get { return m_durationType; } }
        }
        public class MovingAverageCalculatedEventArgs : EventArgs
        {
            private int m_avgPower;
            private DurationType m_durationType;

            public MovingAverageCalculatedEventArgs(int avgPower, DurationType durationType)
            {
                m_avgPower = avgPower;
                m_durationType = durationType;
            }

            public int AveragePower
            {
                get { return m_avgPower; }
            }

            public DurationType DurationType { get { return m_durationType; } }
        }

        public class MetricsCalculatedEventArgs : EventArgs
        {
            public int OverallPower { get; }
            public double AverageKph { get; }
            public double AverageMph { get; }

            public MetricsCalculatedEventArgs(int overallPower, double averageKph, double averageMph)
            {
                OverallPower = overallPower;
                AverageKph = averageKph;
                AverageMph = averageMph;
            }
        }

        public event EventHandler<MovingAverageChangedEventArgs> MovingAverageChangedEvent;
        public event EventHandler<MovingAverageMaxChangedEventArgs> MovingAverageMaxChangedEvent;
        public event EventHandler<MetricsCalculatedEventArgs> MetricsCalculatedEvent;
        public event EventHandler<MovingAverageCalculatedEventArgs> MovingAverageCalculatedEvent;
        #endregion

        static private List<DurationDetail> _durationDetails;

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


        public MovingAverage(DurationType durationType, bool excludeZeroPowerValues)
        {
            Logger = ZAMsettings.LoggerFactory.CreateLogger<MovingAverage>();

            m_durationType = durationType;
            m_duration = MovingAverage.GetDuration(durationType);
            m_excludeZeroPowerValues = excludeZeroPowerValues;

            m_statsQueue = new Queue<Statistics>();

            ZAMsettings.ZPMonitorService.RiderStateEvent += RiderStateEventHandler;
        }

        static MovingAverage()
        {
            _durationDetails = new List<DurationDetail>
            {
                new DurationDetail(DurationType.FiveSeconds, "5 sec", 5),
                new DurationDetail(DurationType.ThirtySeconds, "30 sec", 30),
                new DurationDetail(DurationType.OneMinute, "1 min", 60),
                new DurationDetail(DurationType.FiveMinutes, "5 min", 300),
                new DurationDetail(DurationType.SixMinutes, "6 min", 360),
                new DurationDetail(DurationType.TenMinutes, "10 min", 600),
                new DurationDetail(DurationType.TwentyMinutes, "20 min", 1200),
                new DurationDetail(DurationType.ThirtyMinutes, "30 min", 1800),
                new DurationDetail(DurationType.SixtyMinutes, "60 min", 3600),
                new DurationDetail(DurationType.NinetyMinutes, "90 min", 5400)
            };
        }

        public static int GetDuration(DurationType type)
        {
            return _durationDetails[(int)type].Seconds;

        }
        /// <summary>
        /// Find DurationType based upon a duration label (5 sec, 1 min, 5 min, etc.)
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static DurationType GetType(string label)
        {
            foreach (var item in _durationDetails)
                if (item.Label == label)
                    return item.Type;

            throw new ArgumentException($"Label {label} not found in Duration Details collection.");
        }

        public void Start()
        {
            if (!m_started)
            {
                m_sumPower = 0;
                m_sumHR = 0;
                m_curAvgPower = 0;
                m_curAvgHR = 0;
                m_maxAvgPower = 0;
                m_maxAvgHR = 0;
                m_sumOverallPower = 0;
                m_countOverallPowerSamples = 0;
                m_collectionStart = DateTime.Now;
                m_statsQueue.Clear();

                m_started = true;
            }
        }

        public void Stop()
        {
            if (m_started)
            {
                m_started = false;
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
            TimeSpan oldest = TimeSpan.Zero; // oldest item in queue
            DateTime start = now; // for duration timing
            int curAvgPower;
            int curAvgHR;
            int maxAvgPower;
            int maxAvgHR;
            bool calculateMax = false;
            bool triggerMax = false;

            if (!m_started)
                return;

            // the Statistics class captures the values we want to measure
            var stats = new Statistics(e.Power, e.Heartrate);

            if (m_countOverallPowerSamples == 0)
            {
                // Capture the current distance traveled value to use as an offset to each successive distance value.
                m_distanceSeedValue = e.Distance;
            }

            // To keep track of overall average power.  Performing here as zeros count.
            m_sumOverallPower += (long)stats.Power;
            m_countOverallPowerSamples += 1;

            if (MetricsCalculatedEvent != null)
            {
                int overallPower = (int)Math.Round(m_sumOverallPower / (double)m_countOverallPowerSamples, 0);

                // Calculate average speed, distance is given in meters.
                TimeSpan runningTime = (DateTime.Now - m_collectionStart);
                double kmsTravelled = (e.Distance - m_distanceSeedValue) / 1000.0;
                double milesTravelled = kmsTravelled / 1.609;
                double averageKph = Math.Round((kmsTravelled / runningTime.TotalSeconds) * 3600, 1);
                double averageMph = Math.Round((milesTravelled / runningTime.TotalSeconds) * 3600, 1);

                OnMetricsCalculatedEvent(new MetricsCalculatedEventArgs(overallPower, averageKph, averageMph));
            }

            // For calculating normalized power zeros are ignored.
            if (m_excludeZeroPowerValues && stats.Power == 0)
                return;
            
            // Initialize with current max values.  This is so we know if a new max has occurred.
            maxAvgPower = m_maxAvgPower;
            maxAvgHR = m_maxAvgHR;

            // Remove any queue items which are older than the set time duration
            while (m_statsQueue.Count > 0)
            {
                // look at front of queue
                var peekStats = m_statsQueue.Peek();

                // determine time difference between the newest item and this oldest item
                oldest = stats.Timestamp - peekStats.Timestamp;

                // if queue isn't at capacity yet, exit loop
                if (oldest.TotalSeconds <= m_duration)
                    break;

                // subtract oldest entry from values and dequeue
                m_sumPower -= (long)peekStats.Power;
                m_sumHR -= (long)peekStats.HeartRate;
                m_statsQueue.Dequeue();

                calculateMax = true;  // we have a full sample, calculate maximums
            }

            // add this new item to the queue
            m_statsQueue.Enqueue(stats);
            m_sumPower += (long)stats.Power;
            m_sumHR += (long)stats.HeartRate;

            // calculate averages
            curAvgPower = (int)Math.Round(m_sumPower / (double)m_statsQueue.Count, 0);
            curAvgHR = (int)Math.Round(m_sumHR / (double)m_statsQueue.Count, 0);

            // if queue was full, check to see if we have any new max values
            if (calculateMax)
            {
                if (curAvgPower > m_maxAvgPower)
                {
                    m_maxAvgPower = curAvgPower;
                    triggerMax = true;
                }
                if (curAvgHR > m_maxAvgHR)
                {
                    m_maxAvgHR = curAvgHR;
                    triggerMax = true;
                }

                // Since buffer was full trigger an event so consumer can use this new average (without waiting for a change).  Used by NormalizedPower
                OnMovingAverageCalculatedEvent(new MovingAverageCalculatedEventArgs(curAvgPower, m_durationType));
            }

            // if either average power or average HR changed, trigger event
            if (curAvgPower != m_curAvgPower || curAvgHR != m_curAvgHR)
            {
                m_curAvgPower = curAvgPower;
                m_curAvgHR = curAvgHR;

                OnMovingAverageChangedEvent(new MovingAverageChangedEventArgs(curAvgPower, curAvgHR, m_durationType));
            }

            // if either max average power or max HR changed, trigger event
            if (triggerMax)
            {
                OnMovingAverageMaxChangedEvent(new MovingAverageMaxChangedEventArgs(m_maxAvgPower, m_maxAvgHR, m_durationType));
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
