using System;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitor
{

        /// <summary>
        /// rolling_average = 30 second rolling average
        /// rolling_avg_powered = rolling_average ^ 4
        /// avg_powered_values = average of rolling_avg_powered
        /// NP = avg_powered_values ^ 0.25
        /// </summary>
    public class NormalizedPower
    {
        private readonly ILogger<NormalizedPower> Logger;
        private readonly MovingAverage m_movingAvg;

        private ulong m_sumMovingAvgPow4;
        private int m_countMovingAvgPow4;

        private int m_curNormalizedPower;
        private double? m_curIntensityFactor;
        private int? m_curTotalSufferScore;
        private bool m_started;

        private double m_curAvgKph;
        private double m_curAvgMph;
        private int m_curOverallPower;
        private DateTime m_collectionStartTime; // Time when collection started

        private UserProfile CurrentUser { get; set; }

        #region Public EventArgs classes

        public class NormalizedPowerChangedEventArgs : EventArgs
        {
            public int NormalizedPower { get; }
            public double? IntensityFactor { get; }
            public int? TotalSufferScore { get; }

            public NormalizedPowerChangedEventArgs(int normalizedPower, double? intensityFactor, int? totalSufferScore)
            {
                NormalizedPower = normalizedPower;
                IntensityFactor = intensityFactor;
                TotalSufferScore = totalSufferScore;
            }

        }
        public class MetricsChangedEventArgs : EventArgs
        {
            public double AverageKph { get; }
            public double AverageMph { get; }
            public int OverallPower { get; }

            public MetricsChangedEventArgs(double averageKph, double averageMph, int overallPower)
            {
                AverageKph = averageKph;
                AverageMph = averageMph;
                OverallPower = overallPower;
            }
        }

        #endregion

        public event EventHandler<NormalizedPowerChangedEventArgs> NormalizedPowerChangedEvent;
        public event EventHandler<MetricsChangedEventArgs> MetricsChangedEvent;


        public NormalizedPower()
        {
            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<NormalizedPower>();

            // Create a new 30 seconds moving average class, zero power reading numbers are INCLUDED (I asked support at TrainingPeaks about this).
            m_movingAvg = new MovingAverage(DurationType.ThirtySeconds, false);
            m_movingAvg.MovingAverageCalculatedEvent += MovingAverageCalculatedEventHandler;
            m_movingAvg.MetricsCalculatedEvent += MetricsCalculatedEventHandler;
        }

        public void Start()
        {
            if (!m_started)
            {
                this.CurrentUser = ZAMsettings.Settings.CurrentUser;

                m_countMovingAvgPow4 = 0;
                m_curNormalizedPower = 0;
                m_curIntensityFactor = null;
                m_curTotalSufferScore = null;
                m_sumMovingAvgPow4 = 0;
                m_curAvgKph = 0;
                m_curAvgMph = 0;
                m_curOverallPower = 0;

                m_collectionStartTime = DateTime.Now;

                m_started = true;

                m_movingAvg.Start();
            }
        }

        public void Stop()
        {
            if (m_started)
            {
                m_started = false;

                m_movingAvg.Stop();
            }
        }

        private void MovingAverageCalculatedEventHandler(object sender, MovingAverage.MovingAverageCalculatedEventArgs e)
        {
            if (!m_started)
                return;

            double? intensityFactor = null;
            int? totalSufferScore = null;

            ulong movingAvgPow4 = (ulong)Math.Pow(e.AveragePower, 4);

            m_sumMovingAvgPow4 += movingAvgPow4;
            m_countMovingAvgPow4 += 1;

            double avgMovingAvgPow4 = m_sumMovingAvgPow4 / (double)m_countMovingAvgPow4;

            int normalizedPower = (int)Math.Round(Math.Pow(avgMovingAvgPow4, 0.25), 0);

            if (CurrentUser.PowerThreshold > 0)
            {
                // Calculate Intensity Factor
                intensityFactor = Math.Round(normalizedPower / (double)CurrentUser.PowerThreshold, 2);

                // Calculate TSS
                TimeSpan runningTime = DateTime.Now - m_collectionStartTime;
                totalSufferScore = (int)Math.Round((runningTime.TotalSeconds * normalizedPower * (double)intensityFactor) / (CurrentUser.PowerThreshold * 3600) * 100, 0);
            }


            // when NP changes, send it and the current overall average power through
            if (normalizedPower != m_curNormalizedPower || intensityFactor != m_curIntensityFactor || totalSufferScore != m_curTotalSufferScore)
            {
                m_curNormalizedPower = normalizedPower;
                m_curTotalSufferScore = totalSufferScore;
                m_curIntensityFactor = intensityFactor;

                OnNormalizedPowerChangedEvent(new NormalizedPowerChangedEventArgs(normalizedPower, intensityFactor, totalSufferScore));
            }
        }
        private void MetricsCalculatedEventHandler(object sender, MovingAverage.MetricsCalculatedEventArgs e)
        {
            if (!m_started)
                return;

            if (e.AverageKph != m_curAvgKph || e.AverageMph != m_curAvgMph || e.OverallPower != m_curOverallPower)
            {
                m_curAvgKph = e.AverageKph;
                m_curAvgMph = e.AverageMph;
                m_curOverallPower = e.OverallPower;

                OnMetricsChangedEvent(new MetricsChangedEventArgs(e.AverageKph, e.AverageMph, e.OverallPower));
            }
        }

        private void OnNormalizedPowerChangedEvent(NormalizedPowerChangedEventArgs e)
        {
            EventHandler<NormalizedPowerChangedEventArgs> handler = NormalizedPowerChangedEvent;

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
        private void OnMetricsChangedEvent(MetricsChangedEventArgs e)
        {
            EventHandler<MetricsChangedEventArgs> handler = MetricsChangedEvent;

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
