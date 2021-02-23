using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using ZwiftPacketMonitor;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.Windows;

namespace ZwiftActivityMonitor
{
    public class MovingAverage
    {
        private readonly ZPMonitorService m_zpMonitorService;
        private readonly ILogger<MovingAverage> m_logger;
        private readonly Queue<Statistics> m_statsQueue;

        private long m_sumPower;
        private long m_sumHR;
        private int m_curAvgPower;
        private int m_curAvgHR;
        private int m_maxAvgPower;
        private int m_maxAvgHR;
        private int m_duration; // how long to store recorded readings
        private bool m_started;
        private DurationTypes m_durationType = DurationTypes.not_set;

        public class MovingAverageChangedEventArgs : EventArgs
        {
            private int m_avgPower;
            private int m_avgHR;
            private MovingAverage.DurationTypes m_durationType;

            public MovingAverageChangedEventArgs(int avgPower, int avgHR, MovingAverage.DurationTypes durationType)
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

            public MovingAverage.DurationTypes DurationType { get { return m_durationType; } }
        }
        public class MovingAverageMaxChangedEventArgs : EventArgs
        {
            private int m_avgPower;
            private int m_avgHR;
            private MovingAverage.DurationTypes m_durationType;

            public MovingAverageMaxChangedEventArgs(int avgPower, int avgHR, MovingAverage.DurationTypes durationType)
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
            public MovingAverage.DurationTypes DurationType { get { return m_durationType; } }
        }

        public event EventHandler<MovingAverageChangedEventArgs> MovingAverageChangedEvent;
        public event EventHandler<MovingAverageMaxChangedEventArgs> MovingAverageMaxChangedEvent;

        public enum DurationTypes
        {
            five_second,
            one_minute,
            five_minute,
            ten_minute,
            twenty_minute,
            sixty_minute,
            ninety_minute,
            not_set
        }

        //static private int[] DurationSeconds = { 5, 60, 300, 600, 1200, 3600, 5400 };
        static private List<DurationDetail> _durationDetails;

        #region Internal Classes

        internal class DurationDetail
        {
            private DurationTypes m_type;
            private string m_label;
            private int m_seconds;

            public DurationDetail(DurationTypes type, string label, int seconds)
            {
                m_type = type;
                m_label = label;
                m_seconds = seconds;
            }

            public DurationTypes Type { get { return m_type; } }
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


        public MovingAverage(ZPMonitorService zpMonitorService, ILogger<MovingAverage> logger)
        {
            m_zpMonitorService = zpMonitorService;
            m_logger = logger;

            m_statsQueue = new Queue<Statistics>();
        }

        static MovingAverage()
        {
            _durationDetails = new List<DurationDetail>();

            _durationDetails.Add(new DurationDetail(DurationTypes.five_second, "5 sec", 5));
            _durationDetails.Add(new DurationDetail(DurationTypes.one_minute, "1 min", 60));
            _durationDetails.Add(new DurationDetail(DurationTypes.five_minute, "5 min", 300));
            _durationDetails.Add(new DurationDetail(DurationTypes.ten_minute, "10 min", 600));
            _durationDetails.Add(new DurationDetail(DurationTypes.twenty_minute, "20 min", 1200));
            _durationDetails.Add(new DurationDetail(DurationTypes.sixty_minute, "60 min", 3600));
            _durationDetails.Add(new DurationDetail(DurationTypes.ninety_minute, "90 min", 5400));
        }

        public static int GetDuration(DurationTypes type)
        {
            return _durationDetails[(int)type].Seconds;

        }
        public static DurationTypes GetType(string label)
        {
            foreach (var item in _durationDetails)
                if (item.Label == label)
                    return item.Type;

            throw new ArgumentException($"Label {label} not found in Duration Details collection.");
        }

        public MovingAverage.DurationTypes DurationType { set { m_durationType = value; } }

        public void Start()
        {
            if (m_durationType == DurationTypes.not_set)
            {
                m_logger.LogWarning("A duration type must be set before starting collection.");
                return;
            }

            if (!m_started)
            {
                //m_duration = DurationSeconds[(int)m_durationType];
                m_duration = MovingAverage.GetDuration(m_durationType);

                m_zpMonitorService.PlayerStateEvent += PlayerStateEventHandler;

                m_started = true;
            }
        }

        public void Stop()
        {
            if (m_started)
            {
                m_zpMonitorService.PlayerStateEvent -= PlayerStateEventHandler;
                m_started = false;
            }
        }


        private void PlayerStateEventHandler(object sender, PlayerStateEventArgs e)
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

            var stats = new Statistics(e.PlayerState.Power, e.PlayerState.Heartrate);
            
            maxAvgPower = m_maxAvgPower;
            maxAvgHR = m_maxAvgHR;


            while (m_statsQueue.Count > 0)
            {
                var peekStats = m_statsQueue.Peek();

                oldest = stats.Timestamp - peekStats.Timestamp;

                // is queue at capacity?
                if (oldest.TotalSeconds <= m_duration)
                    break;

                // subtract oldest entry from values and dequeue
                m_sumPower -= (long)peekStats.Power;
                m_sumHR -= (long)peekStats.HeartRate;
                m_statsQueue.Dequeue();

                calculateMax = true;  // we have a full sample, calculate maximums
            }

            // add this item to the queue
            m_statsQueue.Enqueue(stats);
            m_sumPower += (long)stats.Power;
            m_sumHR += (long)stats.HeartRate;


            curAvgPower = (int)(m_sumPower / (long)m_statsQueue.Count);
            curAvgHR = (int)(m_sumHR / (long)m_statsQueue.Count);

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
            }

            // if either average power or HR changed , trigger event
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

            //m_logger.LogInformation($"id: {e.PlayerState.Id} watch: {e.PlayerState.WatchingRiderId} power: {stats.Power} HR: {stats.HeartRate} Count: {m_statsQueue.Count} Sum: {m_sumTotal} Avg: {PowerAvg} Oldest: {oldest.TotalSeconds} TTP: {(DateTime.Now - start).TotalMilliseconds} WorldTime: {e.PlayerState.WorldTime} ");
            //m_logger.LogInformation($"id: {e.PlayerState.Id} power: {stats.Power} HR: {stats.HeartRate} Count: {m_statsQueue.Count} PowerAvg: {curAvgPower} HRAvg: {curAvgHR} PowerMax: {m_maxAvgPower} HRMax: {m_maxAvgHR} Oldest: {oldest.TotalSeconds} TTP: {(DateTime.Now - start).TotalMilliseconds} WorldTime: {e.PlayerState.WorldTime} ");
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
                catch
                {
                    // Don't let downstream exceptions bubble up
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
                catch
                {
                    // Don't let downstream exceptions bubble up
                }
            }
        }

    }
}
