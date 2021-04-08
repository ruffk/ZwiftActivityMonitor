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
        #region Public SplitGoals and SplitGoal classes

        public class SplitGoals
        {
            public double GoalSpeed { get; }
            public double GoalDistance { get; }
            public bool SplitsInKm { get; }
            public TimeSpan GoalTime { get; }

            public List<SplitGoal> Goals { get; }
            
            public SplitGoals(double goalSpeed, double goalDistance, bool splitsInKm, TimeSpan goalTime)
            {
                this.Goals = new List<SplitGoal>();

                this.GoalSpeed = goalSpeed;
                this.GoalDistance = goalDistance;
                this.SplitsInKm = splitsInKm;
                this.GoalTime = goalTime;
            }

            public string GoalSpeedStr
            {
                get
                {
                    return $"{GoalSpeed:#.0} {(SplitsInKm ? "km/h" : "mi/h")}";
                }
            }
            public string GoalDistanceStr
            {
                get
                {
                    return $"{GoalDistance:#.0} {(SplitsInKm ? "km" : "mi")}";
                }
            }
            public string GoalTimeStr
            {
                get
                {
                    return GoalTime.Hours.ToString("0#") + ":" + GoalTime.Minutes.ToString("0#") + ":" + GoalTime.Seconds.ToString("0#");
                }
            }

        }

        public class SplitGoal
        {
            public double SplitDistance { get; }
            public TimeSpan SplitTime { get; }
            public double TotalDistance { get; }
            public TimeSpan TotalTime { get; }
            public bool GoalsInKm { get; }


            public SplitGoal(double splitDistance, TimeSpan splitTime, double totalDistance, TimeSpan totalTime, bool goalsInKm)
            {
                this.SplitDistance = splitDistance;
                this.SplitTime = splitTime;
                this.TotalDistance = totalDistance;
                this.TotalTime = totalTime;
                this.GoalsInKm = goalsInKm;
            }

            public double SplitDistanceKm
            {
                get
                {
                    return Math.Round(this.GoalsInKm ? this.SplitDistance : this.SplitDistance * 1.609, 1);
                }
            }
            public int SplitDistanceMeters
            {
                get
                {
                    return (int)Math.Round(this.SplitDistanceKm * 1000.0, 0);
                }
            }

            //public double SplitDistanceMi
            //{
            //    get
            //    {
            //        return Math.Round(this.GoalsInKm == false ? this.SplitDistance : this.SplitDistance / 1.609, 1);
            //    }
            //}

            public string SplitTimeStr
            {
                get
                {
                    return SplitTime.Hours.ToString("0#") + ":" + SplitTime.Minutes.ToString("0#") + ":" + SplitTime.Seconds.ToString("0#");
                }
            }
            public string TotalTimeStr
            {
                get
                {
                    return TotalTime.Hours.ToString("0#") + ":" + TotalTime.Minutes.ToString("0#") + ":" + TotalTime.Seconds.ToString("0#");
                }
            }

            public string SplitDistanceStr
            {
                get
                {
                    return $"{SplitDistance:#.0} {(GoalsInKm ? "km" : "mi")}";
                }
            }
            public string TotalDistanceStr
            {
                get
                {
                    return $"{TotalDistance:#.0} {(GoalsInKm ? "km" : "mi")}";
                }
            }
        }

        #endregion

        #region Public EventArgs classes

        public class SplitEventArgs : EventArgs
        {
            public int SplitNumber { get; }
            public TimeSpan SplitTime { get; }
            public double SplitSpeed { get; }
            public double TotalDistance { get; }
            public TimeSpan TotalTime { get; }
            public bool SplitsInKm { get; }


            public SplitEventArgs(int splitNumber, TimeSpan splitTime, double splitSpeed, double totalDistance, TimeSpan totalTime, bool splitsInKm)
            {
                this.SplitNumber = splitNumber;
                this.SplitTime = splitTime;
                this.SplitSpeed = splitSpeed;
                this.TotalDistance = totalDistance;
                this.TotalTime = totalTime;
                this.SplitsInKm = splitsInKm;
            }

            public string SplitNumberStr
            {
                get
                {
                    return SplitNumber.ToString("0#");
                }
            }
            public string SplitTimeStr
            {
                get
                {
                    return SplitTime.Minutes.ToString("0#") + ":" + SplitTime.Seconds.ToString("0#");
                }
            }
            public string SplitSpeedStr
            {
                get
                {
                    return $"{SplitSpeed:#.0}";
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

        }

        public class SplitGoalCompletedEventArgs : SplitEventArgs
        {
            public TimeSpan DeltaTime { get; }

            public SplitGoalCompletedEventArgs(int splitNumber, TimeSpan splitTime, double splitSpeed, double totalDistance, TimeSpan totalTime, bool splitsInKm, TimeSpan deltaTime) :
                base(splitNumber, splitTime, splitSpeed, totalDistance, totalTime, splitsInKm)
            {
                this.DeltaTime = deltaTime;
            }

            public string DeltaTimeStr
            {
                get
                {
                    return DeltaTime.Minutes.ToString("0#") + ":" + DeltaTime.Seconds.ToString("0#");
                }
            }
        }
        //public class SplitCompletedEventArgs : SplitUpdatedEventArgs
        //{
        //    public SplitCompletedEventArgs(int splitNumber, TimeSpan splitTime, double splitSpeed, double totalDistance, TimeSpan totalTime, bool splitsInKm) :
        //        base(splitNumber, splitTime, splitSpeed, totalDistance, totalTime, splitsInKm)
        //    {
        //    }
        //}

        #endregion

        public event EventHandler<SplitEventArgs> SplitUpdatedEvent;
        public event EventHandler<SplitGoalCompletedEventArgs> SplitGoalCompletedEvent;
        public event EventHandler<SplitEventArgs> SplitCompletedEvent;

        private readonly ILogger<SplitsManager> Logger;

        private bool m_started;
        private Splits m_splits;
        private int m_eventCount;
        private int m_splitCount;
        private int m_distanceSeedValue; // the PlayerState.Distance value when first started
        private DateTime m_splitStartTime;
        private DateTime m_startTime;
        private SplitGoals m_splitGoals;
        private int m_lastSplitMeters;

        public SplitsManager()
        {
            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<SplitsManager>();

            ZAMsettings.ZPMonitorService.RiderStateEvent += RiderStateEventHandler;
        }


        public void Start()
        {
            if (!m_started)
            {
                m_splits = ZAMsettings.Settings.Splits;

                m_splitGoals = SplitsManager.GetSplitGoals(); // returns null if no goals

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
                return m_splitGoals == null ? "None" : $"{m_splitGoals.GoalDistanceStr} @ {m_splitGoals.GoalSpeedStr} in {m_splitGoals.GoalTimeStr}";
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

            SplitGoal goal = null;

            if (m_splitGoals != null)
            {
                if (m_splitCount >= m_splitGoals.Goals.Count)
                    return;

                // get the in-progress goal
                goal = m_splitGoals.Goals[m_splitCount];
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

            //double averageKph = Math.Round((kmsTravelled / runningTime.TotalSeconds) * 3600, 1);
            //double averageMph = Math.Round((milesTravelled / runningTime.TotalSeconds) * 3600, 1);

            // Calculate how deep into the split distance the rider is.
            int splitMeters = totalMeters - (m_splits.SplitDistanceAsMeters * m_splitCount);

            double splitKmTravelled = Math.Round(splitMeters / 1000.0, 1);
            double splitMiTravelled = Math.Round(splitKmTravelled / 1.609, 1);

            double splitDistance = m_splits.SplitsInKm ? splitKmTravelled : splitMiTravelled;
            double splitSpeed = Math.Round((splitDistance / splitTime.TotalSeconds) * 3600, 1);

            //double splitAverageKph = Math.Round((splitKmTravelled / splitTime.TotalSeconds) * 3600, 1);
            //double splitAverageMph = Math.Round((splitMiTravelled / splitTime.TotalSeconds) * 3600, 1);

            if (goal != null)
            {
                if (splitKmTravelled >= goal.SplitDistanceKm)
                {
                    // Calculate the deltaTime, positive number is good, negative bad.
                    TimeSpan deltaTime = goal.TotalTime.Subtract(runningTime);

                    // This completes the split.  TotalDistance travelled and Delta is included.
                    SplitGoalCompletedEventArgs args = new SplitGoalCompletedEventArgs(m_splitCount + 1, splitTime, splitSpeed, totalDistance, runningTime, m_splits.SplitsInKm, deltaTime);
                    OnSplitGoalCompletedEvent(args);

                    // Reset time and begin next split
                    m_splitStartTime = now;
                    m_splitCount++;

                    m_lastSplitMeters = 0;
                }
                else
                {
                    if (splitMeters - m_lastSplitMeters >= 100) // only raise update event every 100 meters or so
                    {
                        // This is an update to the split in-progress.  SplitDistance travelled is included.
                        SplitEventArgs args = new SplitEventArgs(m_splitCount + 1, splitTime, splitSpeed, splitDistance, runningTime, m_splits.SplitsInKm);
                        OnSplitUpdatedEvent(args);

                        m_lastSplitMeters = splitMeters;
                    }
                }
            }
            else
            {
                if (splitKmTravelled >= m_splits.SplitDistanceAsKm)
                {
                    // This completes the split.  TotalDistance traveled is included.
                    SplitEventArgs args = new SplitEventArgs(m_splitCount + 1, splitTime, splitSpeed, totalDistance, runningTime, m_splits.SplitsInKm);
                    OnSplitCompletedEvent(args);

                    // Reset time and begin next split
                    m_splitStartTime = now;
                    m_splitCount++;

                    m_lastSplitMeters = 0;
                }
                else
                {
                    if (splitMeters - m_lastSplitMeters >= 100) // only raise update event every 100 meters or so
                    {
                        // This is an update to the split in-progress.  SplitDistance traveled is included.
                        SplitEventArgs args = new SplitEventArgs(m_splitCount + 1, splitTime, splitSpeed, splitDistance, runningTime, m_splits.SplitsInKm);
                        OnSplitUpdatedEvent(args);

                        m_lastSplitMeters = splitMeters;
                    }
                }
            }
        }

        //private void ProcessSplitGoals(RiderStateEventArgs e)
        //{
        //    if (m_splitCount >= m_splitGoals.Goals.Count)
        //        return;

        //    // get the in-progress goal
        //    SplitGoal goal = m_splitGoals.Goals[m_splitCount];

        //    DateTime now = DateTime.Now;

        //    TimeSpan runningTime = (now - m_startTime);
        //    TimeSpan splitTime = (now - m_splitStartTime);

        //    if (m_eventCount++ == 0)
        //    {
        //        // Capture the current distance traveled value to use as an offset to each successive distance value.
        //        m_distanceSeedValue = e.Distance;
        //    }

        //    // Calculate total distance travelled
        //    int totalMeters = e.Distance - m_distanceSeedValue;


        //    double kmsTravelled = totalMeters / 1000.0;
        //    double milesTravelled = kmsTravelled / 1.609;

        //    double totalDistance = Math.Round(m_splitGoals.SplitsInKm ? kmsTravelled : milesTravelled, 1);

        //    //double averageKph = Math.Round((kmsTravelled / runningTime.TotalSeconds) * 3600, 1);
        //    //double averageMph = Math.Round((milesTravelled / runningTime.TotalSeconds) * 3600, 1);

        //    // Calculate how deep into the split distance the rider is.
        //    int splitMeters = totalMeters - (m_splits.SplitDistanceAsMeters * m_splitCount);

        //    double splitKmTravelled = Math.Round(splitMeters / 1000.0, 1);
        //    double splitMiTravelled = Math.Round(splitKmTravelled / 1.609, 1);

        //    double splitDistance = m_splitGoals.SplitsInKm ? splitKmTravelled : splitMiTravelled;
        //    double splitSpeed = Math.Round((splitDistance / splitTime.TotalSeconds) * 3600, 1);

        //    //double splitAverageKph = Math.Round((splitKmTravelled / splitTime.TotalSeconds) * 3600, 1);
        //    //double splitAverageMph = Math.Round((splitMiTravelled / splitTime.TotalSeconds) * 3600, 1);

        //    if (splitKmTravelled >= goal.SplitDistanceKm)
        //    {
        //        // Calculate the deltaTime, positive number is good, negative bad.
        //        TimeSpan deltaTime = goal.TotalTime.Subtract(runningTime);

        //        // This completes the split.  TotalDistance travelled and Delta is included.
        //        SplitGoalCompletedEventArgs args = new SplitGoalCompletedEventArgs(m_splitCount + 1, splitTime, splitSpeed, totalDistance, runningTime, m_splitGoals.SplitsInKm, deltaTime);
        //        OnSplitGoalCompletedEvent(args);

        //        // Reset time and begin next split
        //        m_splitStartTime = now;
        //        m_splitCount++;

        //        m_lastSplitMeters = 0;
        //    }
        //    else
        //    {
        //        if (splitMeters - m_lastSplitMeters >= 100) // only raise update event every 100 meters or so
        //        {
        //            // This is an update to the split in-progress.  SplitDistance travelled is included.
        //            SplitGoalUpdatedEventArgs args = new SplitGoalUpdatedEventArgs(m_splitCount + 1, splitTime, splitSpeed, splitDistance, runningTime, m_splitGoals.SplitsInKm);
        //            OnSplitGoalUpdatedEvent(args);

        //            m_lastSplitMeters = splitMeters;
        //        }
        //    }

        //}

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
                    Logger.LogWarning(ex, ex.ToString());
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
                    Logger.LogWarning(ex, ex.ToString());
                }
            }
        }
        private void OnSplitGoalCompletedEvent(SplitGoalCompletedEventArgs e)
        {
            EventHandler<SplitGoalCompletedEventArgs> handler = SplitGoalCompletedEvent;

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

        /// <summary>
        /// Calculate split goals based upon configured values
        /// </summary>
        /// <returns>SplitGoals or null if no goals</returns>
        public static SplitGoals GetSplitGoals()
        {
            Splits splits = ZAMsettings.Settings.Splits;

            if (!splits.ShowSplits || !splits.CalculateGoal)
                return null;

            double numSplits = splits.GoalDistance / splits.SplitDistance;
            if (numSplits < 1)
                return null;

            if (splits.GoalTime.TotalSeconds < 1)
                return null;

            double goalSpeed = Math.Round((splits.GoalDistance / splits.GoalTime.TotalSeconds) * 3600, 1);
            double goalDistance = Math.Round(splits.GoalDistance, 1);

            SplitGoals splitGoals = new SplitGoals(goalSpeed, goalDistance, splits.SplitsInKm, splits.GoalTime);

            TimeSpan splitTime = new TimeSpan(0, 0, (int)Math.Round(splits.GoalTime.TotalSeconds / numSplits, 0));

            int curDistance = 0;
            TimeSpan curTime = new TimeSpan();

            for (int i = 0; i < (int)numSplits; i++)
            {
                int totalDistance = curDistance + splits.SplitDistance;
                TimeSpan totalTime = curTime.Add(splitTime);

                SplitGoal item = new SplitGoal(splits.SplitDistance, splitTime, totalDistance, totalTime, splits.SplitsInKm);
                splitGoals.Goals.Add(item);

                curDistance = totalDistance;
                curTime = totalTime;
            }

            if (numSplits != (int)numSplits)
            {
                double lastSplitDistance = Math.Round(splits.GoalDistance - curDistance, 1);
                TimeSpan lastSplitTime = splits.GoalTime.Subtract(curTime);

                SplitGoal item = new SplitGoal(lastSplitDistance, lastSplitTime, goalDistance, splits.GoalTime, splits.SplitsInKm);
                splitGoals.Goals.Add(item);
            }

            return splitGoals;
        }

    }
}
