using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ZwiftActivityMonitorV2
{
    public class SplitsManagerV2
    {
        public event EventHandler<SplitEventArgs> SplitUpdatedEvent;
        public event EventHandler<SplitEventArgs> SplitGoalCompletedEvent;
        public event EventHandler<SplitEventArgs> SplitCompletedEvent;

        //private readonly ILogger<SplitsManagerV2> Logger;

        private bool m_started;
        private SplitsV2 m_splits;
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

            //Logger = ZAMsettings.LoggerFactory.CreateLogger<SplitsManagerV2>();

            ZAMsettings.ZPMonitorService.RiderStateEvent += RiderStateEventHandler;
            ZAMsettings.ZPMonitorService.CollectionStatusChanged += ZPMonitorService_CollectionStatusChanged;
        }

        private void ZPMonitorService_CollectionStatusChanged(object sender, CollectionStatusChangedEventArgs e)
        {
            Debug.WriteLine($"{this.GetType()}.ZPMonitorService_CollectionStatusChanged - {e.Action}");

            if (e.Action == CollectionStatusChangedEventArgs.ActionType.Started)
                this.Start();
            else if (e.Action == CollectionStatusChangedEventArgs.ActionType.Stopped)
                this.Stop();
        }

        private void Start()
        {
            if (!m_started)
            {
                m_splits = ZAMsettings.Settings.SplitsV2;

                m_eventCount = 0;
                m_splitCount = 0;
                m_startTime = DateTime.Now;
                m_splitStartTime = m_startTime;
                m_lastSplitMeters = 0;

                m_started = true;
            }
        }

        public string GoalText
        {
            get
            {
                return m_splits.CalculateGoal == false ? "None" : $"{m_splits.GoalDistanceStr} @ {m_splits.GoalSpeedStr} in {m_splits.GoalTimeStr}";
            }
        }

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
            if (!m_started || !m_splits.ShowSplits)
                return;

            SplitV2 split = null;

            if (m_splits.CalculateGoal)
            {
                if (m_splitCount >= m_splits.Splits.Count)
                    return;

                // get the in-progress split
                split = m_splits.Splits[m_splitCount];
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

            double kmsTravelled = totalMeters / 1000.0;
            double milesTravelled = kmsTravelled / 1.609;

            double totalDistance = Math.Round(m_splits.SplitsInKm ? kmsTravelled : milesTravelled, 1);

            // Calculate how deep into the current split the rider is.
            int splitMetersTravelled = totalMeters - m_lastSplitMeters;

            // How long is current split?  
            int splitLengthMeters = split == null ? m_splits.SplitDistanceAsMeters : split.SplitDistanceAsMeters;

            // How much of the split is completed (expressed as percentage)
            double splitCompletedPct = splitMetersTravelled / (double)splitLengthMeters;

            // Compute distance, leave unrounded
            double splitKmTravelled = splitMetersTravelled / 1000.0;
            double splitMiTravelled = splitKmTravelled / 1.609;

            double splitDistance = m_splits.SplitsInKm ? splitKmTravelled : splitMiTravelled;
            double splitSpeed = Math.Round((splitDistance / splitTime.TotalSeconds) * 3600, 1);

            // Now round the distance
            splitDistance = Math.Round(splitDistance, 1);

            if (split != null)
            {
                if (splitMetersTravelled >= splitLengthMeters)
                {
                    // Calculate the deltaTime, positive number is bad, negative good.
                    TimeSpan deltaTime = new TimeSpan(0, 0, (int)Math.Round(runningTime.Subtract(split.TotalTime).TotalSeconds, 0));

                    // This completes the split.  TotalDistance travelled and Delta is included.
                    SplitEventArgs args = new SplitEventArgs(m_splitCount + 1, splitTime, splitSpeed, totalDistance, runningTime, m_splits.SplitsInKm, deltaTime);
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
                    SplitEventArgs args = new SplitEventArgs(m_splitCount + 1, splitTime, splitSpeed, splitDistance, runningTime, m_splits.SplitsInKm, deltaTime);
                    OnSplitUpdatedEvent(args);

                    Debug.WriteLine($"%Complete: {splitCompletedPct} Start: {splitStartTime.ToString("m'm 's's'")} Waypoint: {splitWaypointTime.ToString("m'm 's's'")} Delta: {deltaTime.ToString("m'm 's's'")}");
                }
            }
            else
            {
                if (splitMetersTravelled >= splitLengthMeters)
                {
                    // This completes the split.  TotalDistance traveled is included.
                    SplitEventArgs args = new SplitEventArgs(m_splitCount + 1, splitTime, splitSpeed, totalDistance, runningTime, m_splits.SplitsInKm);
                    OnSplitCompletedEvent(args);

                    // Reset time and begin next split
                    m_splitStartTime = now;
                    m_splitCount++;

                    m_lastSplitMeters = totalMeters;
                }
                else
                {
                    // This is an update to the split in-progress.  SplitDistance traveled is included.
                    SplitEventArgs args = new SplitEventArgs(m_splitCount + 1, splitTime, splitSpeed, splitDistance, runningTime, m_splits.SplitsInKm);
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
                catch
                {
                    // Don't let downstream exceptions bubble up
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
                catch
                {
                    // Don't let downstream exceptions bubble up
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
                catch
                {
                    // Don't let downstream exceptions bubble up
                }
            }
        }
    }
}
