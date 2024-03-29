﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitorV2
{
    public class SplitsManagerV2
    {
        public event EventHandler<SplitEventArgs> SplitUpdatedEvent;
        public event EventHandler<SplitEventArgs> SplitGoalCompletedEvent;
        public event EventHandler<SplitEventArgs> SplitCompletedEvent;

        private readonly ILogger<SplitsManagerV2> Logger;

        private bool m_started;
        private int m_eventCount;
        private int m_splitCount;
        private int m_distanceSeedValue; // the PlayerState.Distance value when first started
        private DateTime m_splitStartTime;
        private DateTime m_startTime;
        private int m_lastSplitMeters;


        public SplitsManagerV2()
        {
            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<SplitsManagerV2>();

            ZAMsettings.ZPMonitorService.RiderStateEvent += RiderStateEventHandler;
            ZAMsettings.ZPMonitorService.CollectionStatusChanged += ZPMonitorService_CollectionStatusChanged;
        }

        private SplitsV2 ConfiguredSplits
        {
            get { return ZAMsettings.Settings.SplitsV2; }
        }

        private void ZPMonitorService_CollectionStatusChanged(object sender, CollectionStatusChangedEventArgs e)
        {
            Logger.LogDebug($"{this.GetType()}.ZPMonitorService_CollectionStatusChanged - {e.Action}");

            switch (e.Action)
            {
                case CollectionStatusChangedEventArgs.ActionType.Started:
                    this.Start();
                    break;

                case CollectionStatusChangedEventArgs.ActionType.Stopped:
                    this.Stop();
                    break;
            }
        }

        private void Start()
        {
            if (!m_started)
            {
                DateTime now = DateTime.Now;

                m_eventCount = 0;
                m_splitCount = 0;
                m_startTime = now;
                m_splitStartTime = now;
                m_lastSplitMeters = 0;

                m_started = true;
            }
        }

        public TimeSpan? GoalTime
        {
            get
            {
                if (this.GetLastSplit() is SplitV2 last)
                {
                    return last.TotalTime;
                }
                else
                {
                    return null;
                }
            }
        }

        public double? GoalDistanceKm
        {
            get
            {
                if (this.GetLastSplit() is SplitV2 last)
                {
                    return last.TotalDistanceAsKm;
                }
                else
                {
                    return null;
                }
            }
        }
        public double? GoalDistanceMi
        {
            get
            {
                if (this.GetLastSplit() is SplitV2 last)
                {
                    return last.TotalDistanceAsMi;
                }
                else
                {
                    return null;
                }
            }
        }
        public double? GoalSpeedKph
        {
            get
            {
                if (this.GetLastSplit() is SplitV2 last)
                {
                    return last.TotalSpeedKph;
                }
                else
                {
                    return null;
                }
            }
        }
        public double? GoalSpeedMph
        {
            get
            {
                if (this.GetLastSplit() is SplitV2 last)
                {
                    return last.TotalSpeedMph;
                }
                else
                {
                    return null;
                }
            }
        }

        public bool HasSplitGoals
        {
            get { return this.GetLastSplit() != null; }
        }

        private SplitV2 GetLastSplit()
        {
            if (this.ConfiguredSplits.ShowSplits && this.ConfiguredSplits.CalculateGoal && this.ConfiguredSplits.Splits.Count > 0)
            {
                return this.ConfiguredSplits.Splits[this.ConfiguredSplits.Splits.Count - 1];
            }
            return null;
        }

        //public string GoalText
        //{
        //    get
        //    {
        //        return m_splits.CalculateGoal == false ? "None" : $"{m_splits.GoalDistanceStr} @ {m_splits.GoalSpeedStr} in {m_splits.GoalTimeStr}";
        //    }
        //}

        private void Stop()
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
            if (!m_started || !ConfiguredSplits.ShowSplits || e.CollectionTime == null)
                return;

            SplitV2 split = null;

            if (ConfiguredSplits.CalculateGoal)
            {
                if (m_splitCount >= ConfiguredSplits.Splits.Count)
                    return;

                // get the in-progress split
                split = ConfiguredSplits.Splits[m_splitCount];
            }

            DateTime now = DateTime.Now;

            TimeSpan runningTime = (now - m_startTime);
            TimeSpan splitTime = (now - m_splitStartTime);

            if (m_eventCount++ == 0)
            {
                // Capture the current distance traveled value to use as an offset to each successive distance value.
                m_distanceSeedValue = e.Distance;
            }

            // Calculate total distance travelled
            int totalMeters = e.Distance - m_distanceSeedValue;

            double totalKmTravelled = totalMeters / 1000.0;
            double totalMiTravelled = totalKmTravelled / 1.609;

            //double totalDistance = Math.Round(m_splits.SplitsInKm ? kmsTravelled : milesTravelled, 1);

            // Calculate how deep into the current split the rider is.
            int splitMetersTravelled = totalMeters - m_lastSplitMeters;

            // How long is current split?  
            int splitLengthMeters = split == null ? ConfiguredSplits.SplitDistanceAsMeters : split.SplitDistanceAsMeters;

            // How much of the split is completed (expressed as percentage)
            double splitCompletedPct = splitMetersTravelled / (double)splitLengthMeters;

            // Compute distance, leave unrounded
            double splitKmTravelled = splitMetersTravelled / 1000.0;
            double splitMiTravelled = splitKmTravelled / 1.609;

            //double splitDistance = m_splits.SplitsInKm ? splitKmTravelled : splitMiTravelled;
            //double splitSpeed = Math.Round((splitDistance / splitTime.TotalSeconds) * 3600, 1);
            
            double splitSpeedMph = Math.Round((splitMiTravelled / splitTime.TotalSeconds) * 3600, 1);
            double splitSpeedKph = Math.Round((splitKmTravelled / splitTime.TotalSeconds) * 3600, 1);

            // Now round the distance
            splitMiTravelled = Math.Round(splitMiTravelled, 1);
            splitKmTravelled = Math.Round(splitKmTravelled, 1);

            //splitDistance = Math.Round(splitDistance, 1);

            if (split != null)
            {
                if (splitMetersTravelled >= splitLengthMeters)
                {
                    // Calculate the deltaTime, positive number is bad, negative good.
                    TimeSpan deltaTime = new TimeSpan(0, 0, (int)Math.Round(runningTime.Subtract(split.TotalTime).TotalSeconds, 0));

                    // This completes the split.  TotalDistance travelled and Delta is included.
                    SplitEventArgs args = new SplitEventArgs(m_splitCount + 1, splitTime, splitSpeedMph, splitSpeedKph, totalMiTravelled, totalKmTravelled, runningTime, ConfiguredSplits.SplitsInKm, deltaTime);
                    OnSplitGoalCompletedEvent(args);

                    // Reset time and begin next split
                    m_splitStartTime = now;
                    m_splitCount++;

                    m_lastSplitMeters = split.TotalDistanceAsMeters;
                }
                else
                {
                    // Goal time of split start
                    TimeSpan splitStartTime = split.TotalTime.Subtract(split.SplitTime);

                    // Goal time to get to this point in the split
                    TimeSpan splitWaypointTime = splitStartTime.Add(split.SplitTime.Multiply(splitCompletedPct));

                    // Calculate the deltaTime, positive number is bad, negative good.
                    TimeSpan deltaTime = new TimeSpan(0, 0, (int)Math.Round(runningTime.Subtract(splitWaypointTime).TotalSeconds, 0));

                    // This is an update to the split in-progress.  SplitDistance travelled is included.
                    SplitEventArgs args = new SplitEventArgs(m_splitCount + 1, splitTime, splitSpeedMph, splitSpeedKph, splitMiTravelled, splitKmTravelled, runningTime, ConfiguredSplits.SplitsInKm, deltaTime);
                    OnSplitUpdatedEvent(args);

                    Logger.LogDebug($"%Complete: {splitCompletedPct} Start: {splitStartTime.ToString("m'm 's's'")} Waypoint: {splitWaypointTime.ToString("m'm 's's'")} Delta: {deltaTime.ToString("m'm 's's'")}");
                }
            }
            else
            {
                if (splitMetersTravelled >= splitLengthMeters)
                {
                    // This completes the split.  TotalDistance traveled is included.
                    SplitEventArgs args = new SplitEventArgs(m_splitCount + 1, splitTime, splitSpeedMph, splitSpeedKph, totalMiTravelled, totalKmTravelled, runningTime, ConfiguredSplits.SplitsInKm);
                    OnSplitCompletedEvent(args);

                    // Reset time and begin next split
                    m_splitStartTime = now;
                    m_splitCount++;

                    m_lastSplitMeters = totalMeters;
                }
                else
                {
                    // This is an update to the split in-progress.  SplitDistance traveled is included.
                    SplitEventArgs args = new SplitEventArgs(m_splitCount + 1, splitTime, splitSpeedMph, splitSpeedKph, splitMiTravelled, splitKmTravelled, runningTime, ConfiguredSplits.SplitsInKm);
                    OnSplitUpdatedEvent(args);
                }
            }
        }

        private void OnSplitUpdatedEvent(SplitEventArgs e)
        {
            EventHandler<SplitEventArgs> handler = SplitUpdatedEvent;

            if (handler != null)
            {
                try
                {
                    handler(this, e);
                }
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnSplitUpdatedEvent)");
                }
            }
        }

        private void OnSplitCompletedEvent(SplitEventArgs e)
        {
            EventHandler<SplitEventArgs> handler = SplitCompletedEvent;

            if (handler != null)
            {
                try
                {
                    handler(this, e);
                }
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnSplitCompletedEvent)");
                }
            }
        }
        private void OnSplitGoalCompletedEvent(SplitEventArgs e)
        {
            EventHandler<SplitEventArgs> handler = SplitGoalCompletedEvent;

            if (handler != null)
            {
                try
                {
                    handler(this, e);
                }
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnSplitGoalCompletedEvent)");
                }
            }
        }
    }
}
