using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ZwiftActivityMonitorV2
{
    public class LapsManager
    {
        #region Waypoint classes

        internal class Waypoints
        {
            private List<Waypoint> WaypointList { get; }
            private ILogger<Waypoints> Logger { get; }

            public Waypoints()
            {
                if (ZAMsettings.LoggerFactory == null)
                    return;

                Logger = ZAMsettings.LoggerFactory.CreateLogger<Waypoints>();

                WaypointList = new();
            }

            public void Clear()
            {
                WaypointList.Clear();
            }

            public void Add(Waypoint item)
            {
                WaypointList.Add(item);
            }

            public void Add(RiderStateEventArgs e)
            {
                this.Add(new Waypoint(e.RoadId, e.IsForward, e.Course, e.RoadTime));
            }

            public Waypoint CheckWaypointCrossings(RiderStateEventArgs e)
            {
                Waypoint searchWp;

                //Logger.LogDebug($"CheckWaypointCrossings - Waypoints: {WaypointList.Count}");

                if (e.IsForward) // RoadTime values are increasing
                {
                    searchWp = WaypointList.Find(item => item.Course == e.Course && item.IsForward == e.IsForward && item.RoadId == e.RoadId
                        && item.LastRiderRoadTime < item.RoadTime   // Last check was behind Waypoint line (values going up)
                        && e.RoadTime >= item.RoadTime              // Current check is at or past Waypoint line
                    );
                }
                else // RoadTime values are decreasing
                {
                    searchWp = WaypointList.Find(item => item.Course == e.Course && item.IsForward == e.IsForward && item.RoadId == e.RoadId 
                        && item.LastRiderRoadTime > item.RoadTime   // Last check was past Waypoint line (values going down)
                        && e.RoadTime <= item.RoadTime              // Current check is at or behind Waypoint line
                    );
                }

                return searchWp;
            }

            public void UpdateWaypointLastRoadTimes(int roadTime)
            {
                WaypointList.ForEach(item => item.LastRiderRoadTime = roadTime);
            }

        }

        internal class Waypoint
        {
            public int RoadId { get; }
            public bool IsForward { get; }
            public int Course { get; }
            public int RoadTime { get; }

            public int LastRiderRoadTime { get; set; }

            public Waypoint(int roadId, bool isForward, int course, int roadTime)
            {
                this.RoadId = roadId;
                this.IsForward = isForward;
                this.Course = course;
                this.RoadTime = roadTime;
                this.LastRiderRoadTime = roadTime;
            }
        }

        #endregion

        public event EventHandler<LapEventArgs> LapUpdatedEvent;
        public event EventHandler<LapEventArgs> LapCompletedEvent;
        public event EventHandler<LapStartedEventArgs> LapStartedEvent;

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

        private UserProfile CurrentUserProfile { get { return ZAMsettings.Settings.CurrentUser; } }
        private Lap ConfiguredLaps { get { return ZAMsettings.Settings.Laps; } }

        private bool m_beginNewLap;
        private bool m_userRequestedNewLap;

        private Waypoints LapWaypoints { get; } = new();

        public LapsManager()
        {
            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<LapsManager>();

            ZAMsettings.ZPMonitorService.RiderStateEvent += RiderStateEventHandler;
            ZAMsettings.ZPMonitorService.CollectionStatusChanged += ZPMonitorService_CollectionStatusChanged;

        }

        private void Start()
        {
            if (!IsStarted)
            {
                IsStarted = true;

                this.Reset();
            }
        }

        private void Stop()
        {
            if (IsStarted)
            {
                IsStarted = false;
            }
        }

        /// <summary>
        /// Occurs when user clicks reset button
        /// </summary>
        public void Reset()
        {
            if (IsStarted)
            {
                LapWaypoints.Clear();
                m_eventCount = 0;
            }
        }

        /// <summary>
        /// Occurs when user clicks lap button
        /// </summary>
        public void BeginNewLap()
        {
            if (IsStarted)
            {
                m_userRequestedNewLap = true;
                m_beginNewLap = true;
            }
        }

        private void InitLapVars(RiderStateEventArgs e, DateTime now)
        {
            m_lapSeedValue = e.Distance;
            m_lapStartTime = now;
            m_lapPowerTotal = 0; // to calculate average power
            m_lapEventCount = 0; // to calculate average power
        }

        private void ZPMonitorService_CollectionStatusChanged(object sender, CollectionStatusChangedEventArgs e)
        {
            Logger.LogDebug($"{this.GetType()}.ZPMonitorService_CollectionStatusChanged - {e.Action}");

            switch(e.Action)
            {
                case CollectionStatusChangedEventArgs.ActionType.Started:
                    this.Start();
                    break;

                case CollectionStatusChangedEventArgs.ActionType.Stopped:
                    this.Stop();
                    break;
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
            if (!IsStarted || e.ElapsedTime == null)
                return;

            DateTime now = DateTime.Now;

            // Event count will be zero on startup or after a reset
            if (m_eventCount == 0)
            {
                // Capture the current distance traveled value to use as an offset to each successive distance value.
                m_distanceSeedValue = e.Distance;
                m_startTime = now;
                m_lapCount = 0;

                string statusMsg = "Lap timer started.";

                if (ConfiguredLaps.AutoLapByPosition && ConfiguredLaps.TriggerPositionSetting == Lap.TriggerPositionType.StartAndLapButton)
                {
                    // Add a waypoint using current position
                    LapWaypoints.Add(e);

                    statusMsg = "Lap timer started.  Auto-Lap position saved.";
                }

                OnLapStartedEvent(new LapStartedEventArgs(m_lapCount + 1, statusMsg));

                this.InitLapVars(e, now);
            }

            m_lapPowerTotal += (long)e.Power;
            m_eventCount++;
            m_lapEventCount++;

            // To keep this simple, all calculations use the metric system

            TimeSpan totalTime = new TimeSpan(0, 0, (int)Math.Round((now - m_startTime).TotalSeconds, 1));
            TimeSpan lapTime = new TimeSpan(0, 0, (int)Math.Round((now - m_lapStartTime).TotalSeconds, 1));

            // Calculate total meters
            int totalMeters = e.Distance - m_distanceSeedValue;

            // Calculate total kilometers
            double kmsTravelled = totalMeters / 1000.0;

            // Calculate lap meters
            int lapMeters = e.Distance - m_lapSeedValue;

            // Calculate lap kilometers
            double lapDistanceKm = lapMeters / 1000.0;

            // Convert to miles
            double lapDistanceMi = Math.Round(lapDistanceKm / 1.609, 1);

            // Round lap kilometers
            lapDistanceKm = Math.Round(lapDistanceKm, 1);

            // Calculate Avg Watts
            double lapAPwatts = m_lapPowerTotal / (double)m_lapEventCount;

            // Convert to W/Kg
            double? lapAPwattsPerKg = this.CalculateUserWattsPerKg(lapAPwatts);

            // Round watts
            lapAPwatts = Math.Round(lapAPwatts, 0);

            bool autoLapOccurred = false;
            string autoLapStatusMsg = "";

            // Determine if any thresholds have been hit (distance or time), or if we've passed a waypoint
            switch (ConfiguredLaps.LapStyleSetting)
            {
                case Lap.LapStyleType.Manual:
                    break;

                case Lap.LapStyleType.Automatic:

                    switch (ConfiguredLaps.LapTriggerSetting)
                    {
                        case Lap.LapTriggerType.Distance:
                            if (lapMeters >= ConfiguredLaps.TriggerDistanceAsMeters)
                            {
                                autoLapOccurred = true;
                                autoLapStatusMsg = "Auto-Lap by distance triggered.";
                                m_beginNewLap = true;
                            }
                            break;

                        case Lap.LapTriggerType.Time:
                            if (lapTime >= ConfiguredLaps.TriggerTime)
                            {
                                autoLapOccurred = true;
                                autoLapStatusMsg = "Auto-Lap by time triggered.";
                                m_beginNewLap = true;
                            }
                            break;

                        case Lap.LapTriggerType.Position:
                            if (LapWaypoints.CheckWaypointCrossings(e) != null)
                            {
                                autoLapOccurred = true;
                                autoLapStatusMsg = "Auto-Lap by position triggered.";
                                m_beginNewLap = true;
                            }
                            break;
                    }
                    break;
            }

            LapEventArgs args = new LapEventArgs(m_lapCount + 1, lapTime, lapDistanceKm, lapDistanceMi, (int)lapAPwatts, lapAPwattsPerKg, totalTime);

            if (m_beginNewLap)
            {
                // This is an update to the lap in-progress.
                OnLapUpdatedEvent(args);

                // Complete the lap in-progress.
                OnLapCompletedEvent(args);

                // To keep everything syncronized, compute next lap start time
                DateTime nextLapStart = m_startTime + totalTime;

                // Initialize for a new lap
                this.InitLapVars(e, nextLapStart);

                m_lapCount++;

                if (m_userRequestedNewLap)
                {
                    string statusMsg = "Next lap started.";
                    if (ConfiguredLaps.AutoLapByPosition
                        && (ConfiguredLaps.TriggerPositionSetting == Lap.TriggerPositionType.StartAndLapButton || ConfiguredLaps.TriggerPositionSetting == Lap.TriggerPositionType.LapButtonOnly))
                    {
                        // Add a waypoint using current position
                        LapWaypoints.Add(e);

                        statusMsg = "Next lap started.  Auto-Lap position saved.";
                    }
                    m_userRequestedNewLap = false;

                    OnLapStartedEvent(new LapStartedEventArgs(m_lapCount+1, statusMsg));
                }
                else if (autoLapOccurred)
                {
                    OnLapStartedEvent(new LapStartedEventArgs(m_lapCount + 1, autoLapStatusMsg));
                }

                m_beginNewLap = false;
            }
            else
            {
                // This is an update to the lap in-progress.
                OnLapUpdatedEvent(args);
            }

            LapWaypoints.UpdateWaypointLastRoadTimes(e.RoadTime);
        }
        
        private double? CalculateUserWattsPerKg(double watts)
        {
            return CurrentUserProfile.WeightAsKgs > 0 ? Math.Round(watts / CurrentUserProfile.WeightAsKgs, 2) : null;
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
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnLapUpdatedEvent)");
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
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnLapCompletedEvent)");
                }
            }
        }
        private void OnLapStartedEvent(LapStartedEventArgs e)
        {
            EventHandler<LapStartedEventArgs> handler = LapStartedEvent;

            if (handler != null)
            {
                try
                {
                    handler(this, e);
                }
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnLapStartedEvent)");
                }
            }
        }
    }
}
