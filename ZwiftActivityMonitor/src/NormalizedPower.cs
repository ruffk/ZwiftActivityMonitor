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

        /// <summary>
        /// rolling_average = 30 second rolling average
        /// rolling_avg_powered = rolling_average ^ 4
        /// avg_powered_values = average of rolling_avg_powered
        /// NP = avg_powered_values ^ 0.25
        /// </summary>
    public class NormalizedPower
    {
        private readonly ZPMonitorService m_zpMonitorService;
        private readonly ILogger<NormalizedPower> Logger;
        private readonly MovingAverage m_movingAvg;

        private ulong m_sumMovingAvgPow4;
        private int m_countMovingAvgPow4;

        private int m_curNormalizedPower;
        private bool m_started;

        private double m_curAvgKph;
        private double m_curAvgMph;
        private int m_curOverallPower;

        public class NormalizedPowerChangedEventArgs : EventArgs
        {
            private int m_normalizedPower;

            public NormalizedPowerChangedEventArgs(int normalizedPower)
            {
                m_normalizedPower = normalizedPower;
            }

            public int NormalizedPower
            {
                get { return m_normalizedPower; }
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

        public event EventHandler<NormalizedPowerChangedEventArgs> NormalizedPowerChangedEvent;
        public event EventHandler<MetricsChangedEventArgs> MetricsChangedEvent;


        public NormalizedPower(ZPMonitorService zpMonitorService, ILoggerFactory loggerFactory)
        {
            m_zpMonitorService = zpMonitorService;
            Logger = loggerFactory.CreateLogger<NormalizedPower>();

            // Create a new 30 seconds moving average class, zero power reading numbers are INCLUDED (I asked support at TrainingPeaks about this).
            m_movingAvg = new MovingAverage(m_zpMonitorService, loggerFactory, DurationType.ThirtySeconds, false);
            m_movingAvg.MovingAverageCalculatedEvent += MovingAverageCalculatedEventHandler;
            m_movingAvg.MetricsCalculatedEvent += MetricsCalculatedEventHandler;
        }

        public void Start()
        {
            if (!m_started)
            {
                m_countMovingAvgPow4 = 0;
                m_curNormalizedPower = 0;
                m_sumMovingAvgPow4 = 0;
                m_curAvgKph = 0;
                m_curAvgMph = 0;
                m_curOverallPower = 0;

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

            ulong movingAvgPow4 = (ulong)Math.Pow(e.AveragePower, 4);

            m_sumMovingAvgPow4 += movingAvgPow4;
            m_countMovingAvgPow4 += 1;

            double avgMovingAvgPow4 = m_sumMovingAvgPow4 / (double)m_countMovingAvgPow4;

            int normalizedPower = (int)Math.Round(Math.Pow(avgMovingAvgPow4, 0.25), 0);

            // when NP changes, send it and the current overall average power through
            if (normalizedPower != m_curNormalizedPower)
            {
                m_curNormalizedPower = normalizedPower;

                OnNormalizedPowerChangedEvent(new NormalizedPowerChangedEventArgs(normalizedPower));
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
