using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitor
{
    public class LapsManager
    {
        #region Public EventArgs classes

        public class LapEventArgs : EventArgs
        {
            public int LapNumber { get; }
            public TimeSpan LapTime { get; }
            public double LapSpeed { get; }
            public double LapDistance { get; }
            public int LapAvgPower { get; }
            public TimeSpan TotalTime { get; }
            public bool LapsInKm { get; }


            public LapEventArgs(int LapNumber, TimeSpan LapTime, double LapSpeed, double LapDistance, int LapAvgPower, TimeSpan TotalTime, bool LapsInKm)
            {
                this.LapNumber = LapNumber;
                this.LapTime = LapTime;
                this.LapSpeed = LapSpeed;
                this.LapDistance = LapDistance;
                this.LapAvgPower = LapAvgPower;
                this.TotalTime = TotalTime;
                this.LapsInKm = LapsInKm;
            }

            public string LapNumberStr
            {
                get
                {
                    return LapNumber.ToString();
                }
            }
            public string LapTimeStr
            {
                get
                {
                    return LapTime.Minutes.ToString("0#") + ":" + LapTime.Seconds.ToString("0#");
                }
            }
            public string LapSpeedStr
            {
                get
                {
                    return $"{LapSpeed:#.0}";
                }
            }
            public string LapDistanceStr
            {
                get
                {
                    return $"{LapDistance:0.0}";
                }
            }
            public string LapAvgPowerStr
            {
                get
                {
                    return LapAvgPower.ToString();
                }
            }
            public string TotalTimeStr
            {
                get
                {
                    return TotalTime.Hours.ToString("0#") + ":" + TotalTime.Minutes.ToString("0#") + ":" + TotalTime.Seconds.ToString("0#");
                }
            }
        }

        #endregion

        public event EventHandler<LapEventArgs> LapUpdatedEvent;

        private readonly ILogger<LapsManager> Logger;

        public bool IsStarted { get; set; }

        private int m_eventCount;           // Count of how many RiderStateEvent's processed
        private int m_distanceSeedValue;    // The RiderStateEvent.Distance value when first started
        private DateTime m_startTime;       // Time when collection started

        private int m_lapEventCount;        // Count of how many RiderStateEvent's processed during current lap
        private int m_lapCount;             // Current lap count, zero origin
        private int m_lapSeedValue;         // The RiderStateEvent.Distance value when current lap started
        private DateTime m_lapStartTime;    // Time when lap started
        private long m_lapPowerTotal;       // The sum of all power values captured during the lap


        private int m_lastEventMeters;
        private bool m_beginNewLap;

        public LapsManager()
        {
            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<LapsManager>();

            ZAMsettings.ZPMonitorService.RiderStateEvent += RiderStateEventHandler;
        }


        public void BeginLapMonitoring()
        {
            if (!IsStarted)
            {
                IsStarted = true;
            }
        }

        public void EndLapMonitoring()
        {
            if (IsStarted)
            {
                IsStarted = false;
            }
        }

        public void Reset()
        {
            if (IsStarted)
            {
                m_eventCount = 0;
            }
        }

        public void BeginNewLap()
        {
            if (IsStarted)
            {
                m_beginNewLap = true;
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
            if (!IsStarted)
                return;

            DateTime now = DateTime.Now;
            bool lapsInKm = true;

            // Event count will be zero on startup or after a reset
            if (m_eventCount == 0)
            {
                // Capture the current distance traveled value to use as an offset to each successive distance value.
                m_distanceSeedValue = e.Distance;
                m_startTime = now;
                m_lastEventMeters = 0;
                m_lapCount = 0;

                m_beginNewLap = true;
            }

            if (m_beginNewLap)
            {
                m_lapSeedValue = e.Distance;
                m_lapStartTime = now;
                m_lapPowerTotal = 0; // to calculate average power
                m_lapEventCount = 0; // to calculate average power

                // if not the first event captured, increment lap counter
                if (m_eventCount > 0)
                    m_lapCount++;

                m_beginNewLap = false;
            }

            m_lapPowerTotal += (long)e.Power;
            m_eventCount++;
            m_lapEventCount++;


            TimeSpan totalTime = (now - m_startTime);
            TimeSpan lapTime = (now - m_lapStartTime);

            // Calculate total distance travelled
            int totalMeters = e.Distance - m_distanceSeedValue;


            double kmsTravelled = totalMeters / 1000.0;
            double milesTravelled = kmsTravelled / 1.609;

            double totalDistance = Math.Round(lapsInKm ? kmsTravelled : milesTravelled, 1);

            // Calculate how deep into the lap distance the rider is.
            int lapMeters = e.Distance - m_lapSeedValue;

            // How much of the lap is completed (expressed as percentage)
            //double lapCompletedPct = lapMeters / (double)m_laps.SplitDistanceAsMeters;

            // Compute distance, leave unrounded
            double lapKmTravelled = lapMeters / 1000.0;
            double lapMiTravelled = lapKmTravelled / 1.609;

            double lapDistance = lapsInKm ? lapKmTravelled : lapMiTravelled;
            double lapSpeed = Math.Round((lapDistance / lapTime.TotalSeconds) * 3600, 1);

            // Now round the distance
            lapDistance = Math.Round(lapDistance, 1);

            int lapAvgPower = (int)Math.Round(m_lapPowerTotal / (double)m_lapEventCount, 0);

            /*
            if (lapKmTravelled >= m_laps.SplitDistanceAsKm)
            {
                // This completes the lap.  TotalDistance traveled is included.
                SplitEventArgs args = new SplitEventArgs(m_lapCount + 1, lapTime, lapSpeed, totalDistance, runningTime, m_laps.SplitsInKm);
                OnSplitCompletedEvent(args);

                // Reset time and begin next lap
                m_lapStartTime = now;
                m_lapCount++;

                m_lastSplitMeters = 0;
            }
            else
            {
                if (lapMeters - m_lastSplitMeters >= 1)
                {
                    // This is an update to the lap in-progress.  SplitDistance traveled is included.
                    SplitEventArgs args = new SplitEventArgs(m_lapCount + 1, lapTime, lapSpeed, lapDistance, runningTime, m_laps.SplitsInKm);
                    OnSplitUpdatedEvent(args);

                    m_lastSplitMeters = lapMeters;
                }
            }
            */
            if (totalMeters - m_lastEventMeters >= 1)
            {
                // This is an update to the lap in-progress.  LapDistance traveled is included.
                LapEventArgs args = new LapEventArgs(m_lapCount + 1, lapTime, lapSpeed, lapDistance, lapAvgPower , totalTime, lapsInKm);
                OnLapUpdatedEvent(args);

                m_lastEventMeters = totalMeters;
            }
        }


        private void OnLapUpdatedEvent(LapEventArgs e)
        {
            EventHandler<LapEventArgs> handler = LapUpdatedEvent;

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

        //private void OnLapCompletedEvent(LapEventArgs e)
        //{
        //    EventHandler<LapEventArgs> handler = LapCompletedEvent;

        //    if (handler != null)
        //    {
        //        try
        //        {
        //            handler(this, e);
        //        }
        //        catch (Exception ex)
        //        {
        //            // Don't let downstream exceptions bubble up
        //            Logger.LogWarning(ex, ex.ToString());
        //        }
        //    }
        //}
    }
}
