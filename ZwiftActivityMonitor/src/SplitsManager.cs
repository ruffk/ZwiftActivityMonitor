using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitor
{
    public class SplitsManager
    {
        private readonly ILogger<SplitsManager> Logger;

        private bool m_started;
        private Splits m_splits;
        private int m_splitMetersCompleted;
        private int m_eventCount;
        private int m_splitCount;
        private int m_distanceSeedValue; // the PlayerState.Distance value when first started
        private DateTime m_splitStartTime;
        private DateTime m_startTime;

        public SplitsManager()
        {
            Logger = ZAMsettings.LoggerFactory.CreateLogger<SplitsManager>();

            ZAMsettings.ZPMonitorService.RiderStateEvent += RiderStateEventHandler;
        }

        public void Start()
        {
            if (!m_started)
            {
                m_splits = ZAMsettings.Settings.Splits;

                m_eventCount = 0;
                m_splitCount = 0;
                m_splitMetersCompleted = 0;
                m_startTime = DateTime.Now;
                m_splitStartTime = m_startTime;

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
        /// Handle player state changes.
        /// Event distance is given in meters.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RiderStateEventHandler(object sender, RiderStateEventArgs e)
        {
            if (!m_started)
                return;

            if (m_eventCount++ == 0)
            {
                // Capture the current distance traveled value to use as an offset to each successive distance value.
                m_distanceSeedValue = e.Distance;
            }

            // Calculate total distance travelled and then determine how deep into the split distance the rider is.
            int totalMeters = e.Distance - m_distanceSeedValue;
            int splitMeters = totalMeters % m_splits.SplitDistanceAsMeters;

            // By using a modulus to calculate splitMeters, it will become zero each time the distance is equal to the configured distance.
            // However, since it most likely will never be exact we just check to see if the value is less than the previous check.
            if (splitMeters < m_splitMetersCompleted)
            {
                DateTime now = DateTime.Now;

                TimeSpan runningTime = (now - m_startTime);
                double kmsTravelled = totalMeters / 1000.0;
                double milesTravelled = kmsTravelled / 1.609;
                double averageKph = Math.Round((kmsTravelled / runningTime.TotalSeconds) * 3600, 1);
                double averageMph = Math.Round((milesTravelled / runningTime.TotalSeconds) * 3600, 1);

                TimeSpan splitTime = (now - m_splitStartTime);
                double splitKmTravelled = m_splits.SplitDistanceAsKm;
                double splitMiTravelled = splitKmTravelled / 1.609;
                double splitAverageKph = Math.Round((splitKmTravelled / splitTime.TotalSeconds) * 3600, 1);
                double splitAverageMph = Math.Round((splitMiTravelled / splitTime.TotalSeconds) * 3600, 1);

                m_splitMetersCompleted = 0;
                m_splitStartTime = now;
            }
            else
            {
                m_splitMetersCompleted = splitMeters;
            }


            
        }
    }
}
