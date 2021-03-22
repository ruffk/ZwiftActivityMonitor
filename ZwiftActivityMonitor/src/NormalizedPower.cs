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

        public class NormalizedPowerChangedEventArgs : EventArgs
        {
            private int m_normalizedPower;
            private int m_overallPower;

            public NormalizedPowerChangedEventArgs(int normalizedPower, int overallPower)
            {
                m_normalizedPower = normalizedPower;
                m_overallPower = overallPower;
            }

            public int NormalizedPower
            {
                get { return m_normalizedPower; }
            }

            public int OverallPower
            {
                get { return m_overallPower; }
            }
        }

        public event EventHandler<NormalizedPowerChangedEventArgs> NormalizedPowerChangedEvent;


        public NormalizedPower(ZPMonitorService zpMonitorService, ILoggerFactory loggerFactory)
        {
            m_zpMonitorService = zpMonitorService;
            Logger = loggerFactory.CreateLogger<NormalizedPower>();

            m_movingAvg = new MovingAverage(m_zpMonitorService, loggerFactory, DurationType.ThirtySeconds, true);
            m_movingAvg.MovingAverageCalculatedEvent += MovingAverageCalculatedEventHandler;
        }

        public void Start()
        {
            if (!m_started)
            {
                m_countMovingAvgPow4 = 0;
                m_curNormalizedPower = 0;
                m_sumMovingAvgPow4 = 0;

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

            ulong avgMovingAvgPow4 = m_sumMovingAvgPow4 / (ulong)m_countMovingAvgPow4;

            int normalizedPower = (int)Math.Pow(avgMovingAvgPow4, 0.25); 

            // when NP changes, send it and the current overall average power through
            if (normalizedPower != m_curNormalizedPower)
            {
                m_curNormalizedPower = normalizedPower;

                OnNormalizedPowerChangedEvent(new NormalizedPowerChangedEventArgs(normalizedPower, e.OverallPower));
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

    }
}
