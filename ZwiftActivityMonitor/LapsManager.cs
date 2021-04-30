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
            public double Lapspeed { get; }
            public double TotalDistance { get; }
            public TimeSpan TotalTime { get; }
            public bool LapsInKm { get; }
            public TimeSpan? DeltaTime { get; }


            public LapEventArgs(int LapNumber, TimeSpan LapTime, double Lapspeed, double totalDistance, TimeSpan totalTime, bool LapsInKm)
            {
                this.LapNumber = LapNumber;
                this.LapTime = LapTime;
                this.Lapspeed = Lapspeed;
                this.TotalDistance = totalDistance;
                this.TotalTime = totalTime;
                this.LapsInKm = LapsInKm;
                this.DeltaTime = null;
            }

            public LapEventArgs(int LapNumber, TimeSpan LapTime, double Lapspeed, double totalDistance, TimeSpan totalTime, bool LapsInKm, TimeSpan deltaTime)
            {
                this.LapNumber = LapNumber;
                this.LapTime = LapTime;
                this.Lapspeed = Lapspeed;
                this.TotalDistance = totalDistance;
                this.TotalTime = totalTime;
                this.LapsInKm = LapsInKm;
                this.DeltaTime = deltaTime;
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
            public string LapspeedStr
            {
                get
                {
                    return $"{Lapspeed:#.0}";
                }
            }
            public string TotalDistanceStr
            {
                get
                {
                    return $"{TotalDistance:0.0}";
                }
            }
            public string TotalTimeStr
            {
                get
                {
                    return TotalTime.Hours.ToString("0#") + ":" + TotalTime.Minutes.ToString("0#") + ":" + TotalTime.Seconds.ToString("0#");
                }
            }
            public string DeltaTimeStr
            {
                get
                {
                    if (DeltaTime.HasValue)
                    {
                        TimeSpan std = (TimeSpan)DeltaTime;
                        bool negated = false;

                        if (std.TotalSeconds < 0)
                        {
                            std = std.Negate();
                            negated = true;
                        }

                        return $"{(negated ? "-" : "+")}{std.Minutes:0#}:{std.Seconds:0#}";
                    }
                    else
                    {
                        return "";
                    }
                }
            }


        }

        #endregion

        public event EventHandler<LapEventArgs> LapUpdatedEvent;
        public event EventHandler<LapEventArgs> LapCompletedEvent;

        private readonly ILogger<LapsManager> Logger;

        public bool IsStarted { get; set; }

        private int m_eventCount;
        private int m_LapCount;
        private int m_distanceSeedValue; // the PlayerState.Distance value when first started
        private DateTime m_LapstartTime;
        private DateTime m_startTime;
        private int m_lastLapMeters;

        public LapsManager()
        {
            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<LapsManager>();

            ZAMsettings.ZPMonitorService.RiderStateEvent += RiderStateEventHandler;
        }


        public void Start()
        {
            if (!IsStarted)
            {
                //m_Laps = ZAMsettings.Settings.Laps;

                //m_LapGoals = LapsManager.GetLapGoals(); // returns null if no goals

                m_eventCount = 0;
                m_LapCount = 0;
                m_startTime = DateTime.Now;
                m_LapstartTime = m_startTime;
                m_lastLapMeters = 0;

                IsStarted = true;
            }
        }

        public void Stop()
        {
            if (IsStarted)
            {
                IsStarted = false;
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

            TimeSpan runningTime = (now - m_startTime);
            TimeSpan LapTime = (now - m_LapstartTime);

            if (m_eventCount++ == 0)
            {
                // Capture the current distance traveled value to use as an offset to each successive distance value.
                m_distanceSeedValue = e.Distance;
            }

            // Calculate total distance travelled
            int totalMeters = e.Distance - m_distanceSeedValue;


            double kmsTravelled = totalMeters / 1000.0;
            double milesTravelled = kmsTravelled / 1.609;
            /*
            double totalDistance = Math.Round(m_Laps.LapsInKm ? kmsTravelled : milesTravelled, 1);

            // Calculate how deep into the Lap distance the rider is.
            int LapMeters = totalMeters - (m_Laps.LapDistanceAsMeters * m_LapCount);

            double LapKmTravelled = Math.Round(LapMeters / 1000.0, 1);
            double LapMiTravelled = Math.Round(LapKmTravelled / 1.609, 1);

            double LapDistance = m_Laps.LapsInKm ? LapKmTravelled : LapMiTravelled;
            double Lapspeed = Math.Round((LapDistance / LapTime.TotalSeconds) * 3600, 1);

            if (LapKmTravelled >= m_Laps.LapDistanceAsKm)
            {
                // This completes the Lap.  TotalDistance traveled is included.
                LapEventArgs args = new LapEventArgs(m_LapCount + 1, LapTime, Lapspeed, totalDistance, runningTime, m_Laps.LapsInKm);
                OnLapCompletedEvent(args);

                // Reset time and begin next Lap
                m_LapstartTime = now;
                m_LapCount++;

                m_lastLapMeters = 0;
            }
            else
            {
                if (LapMeters - m_lastLapMeters >= 100) // only raise update event every 100 meters or so
                {
                    // This is an update to the Lap in-progress.  LapDistance traveled is included.
                    LapEventArgs args = new LapEventArgs(m_LapCount + 1, LapTime, Lapspeed, LapDistance, runningTime, m_Laps.LapsInKm);
                    OnLapUpdatedEvent(args);

                    m_lastLapMeters = LapMeters;
                }
            }
            */
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
        private void OnLapCompletedEvent(LapEventArgs e)
        {
            EventHandler<LapEventArgs> handler = LapCompletedEvent;

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
