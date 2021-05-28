using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitor
{
    public class SplitsManagerV2
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
                    //return GoalTime.Hours.ToString("0#") + ":" + GoalTime.Minutes.ToString("0#") + ":" + GoalTime.Seconds.ToString("0#");
                    return GoalTime.Hours > 0 ? Math.Floor(GoalTime.TotalHours) + GoalTime.ToString("'h 'm'm 's's'") : GoalTime.ToString("m'm 's's'");
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
                    //return SplitTime.Hours > 0 ? Math.Floor(SplitTime.TotalHours) + SplitTime.ToString("'h 'm'm 's's'") : SplitTime.ToString("m'm 's's'");
                }
            }
            public string TotalTimeStr
            {
                get
                {
                    return TotalTime.Hours.ToString("0#") + ":" + TotalTime.Minutes.ToString("0#") + ":" + TotalTime.Seconds.ToString("0#");
                    //return TotalTime.Hours > 0 ? Math.Floor(TotalTime.TotalHours) + TotalTime.ToString("'h 'm'm 's's'") : TotalTime.ToString("m'm 's's'");
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
            public TimeSpan? DeltaTime { get; }


            public SplitEventArgs(int splitNumber, TimeSpan splitTime, double splitSpeed, double totalDistance, TimeSpan totalTime, bool splitsInKm)
            {
                this.SplitNumber = splitNumber;
                this.SplitTime = splitTime;
                this.SplitSpeed = splitSpeed;
                this.TotalDistance = totalDistance;
                this.TotalTime = totalTime;
                this.SplitsInKm = splitsInKm;
                this.DeltaTime = null;
            }

            public SplitEventArgs(int splitNumber, TimeSpan splitTime, double splitSpeed, double totalDistance, TimeSpan totalTime, bool splitsInKm, TimeSpan deltaTime)
            {
                this.SplitNumber = splitNumber;
                this.SplitTime = splitTime;
                this.SplitSpeed = splitSpeed;
                this.TotalDistance = totalDistance;
                this.TotalTime = totalTime;
                this.SplitsInKm = splitsInKm;
                this.DeltaTime = deltaTime;
            }

            public string SplitNumberStr
            {
                get
                {
                    return SplitNumber.ToString();
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
                    //return TotalTime.Hours.ToString("0#") + ":" + TotalTime.Minutes.ToString("0#") + ":" + TotalTime.Seconds.ToString("0#");
                    return $"{(TotalTime.Hours > 0 ? TotalTime.ToString("hh':'mm':'ss") : TotalTime.ToString("mm':'ss"))}";
                }
            }

            public bool? AheadOfGoalTime 
            { 
                get 
                {
                    if (DeltaTime.HasValue)
                    {
                        TimeSpan std = (TimeSpan)DeltaTime;
                        return std.TotalSeconds <= 0;
                    }
                    else
                    {
                        return null;
                    }
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

                        //return $"{(negated ? "-" : "+")}{std.Minutes:0#}:{std.Seconds:0#}";
                        return $"{(negated ? "-" : "+")}{(std.Minutes > 0 ? std.ToString("m'@QT's'\"'").Replace("@QT", "\'") : std.ToString("s'\"'"))}";
                    }
                    else
                    {
                        return "";
                    }
                }
            }


        }

        //public class SplitGoalCompletedEventArgs : SplitEventArgs
        //{
        //    public TimeSpan DeltaTime { get; }

        //    public SplitGoalCompletedEventArgs(int splitNumber, TimeSpan splitTime, double splitSpeed, double totalDistance, TimeSpan totalTime, bool splitsInKm, TimeSpan deltaTime) :
        //        base(splitNumber, splitTime, splitSpeed, totalDistance, totalTime, splitsInKm)
        //    {
        //        this.DeltaTime = deltaTime;
        //    }

        //    public string DeltaTimeStr
        //    {
        //        get
        //        {
        //            TimeSpan std = DeltaTime;
        //            bool negated = false;

        //            if (DeltaTime.TotalSeconds < 0)
        //            {
        //                std = DeltaTime.Negate();
        //                negated = true;
        //            }

        //            return $"{(negated ? "-" : "+")}{std.Minutes:0#}:{std.Seconds:0#}";
        //        }
        //    }
        //}

        //public class SplitCompletedEventArgs : SplitUpdatedEventArgs
        //{
        //    public SplitCompletedEventArgs(int splitNumber, TimeSpan splitTime, double splitSpeed, double totalDistance, TimeSpan totalTime, bool splitsInKm) :
        //        base(splitNumber, splitTime, splitSpeed, totalDistance, totalTime, splitsInKm)
        //    {
        //    }
        //}

        #endregion

        public event EventHandler<SplitEventArgs> SplitUpdatedEvent;
        public event EventHandler<SplitEventArgs> SplitGoalCompletedEvent;
        public event EventHandler<SplitEventArgs> SplitCompletedEvent;

        private readonly ILogger<SplitsManagerV2> Logger;

        private bool m_started;
        private SplitsV2 m_splits;
        private int m_eventCount;
        private int m_splitCount;
        private int m_distanceSeedValue; // the PlayerState.Distance value when first started
        private DateTime m_splitStartTime;
        private DateTime m_startTime;
        private SplitGoals m_splitGoals;
        private int m_lastSplitMeters;

        public SplitsManagerV2()
        {
            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<SplitsManagerV2>();

            ZAMsettings.ZPMonitorService.RiderStateEvent += RiderStateEventHandler;
        }


        public void Start()
        {
            if (!m_started)
            {
                m_splits = ZAMsettings.Settings.SplitsV2;

                m_splitGoals = SplitsManagerV2.GetSplitGoals(); // returns null if no goals

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
            if (!m_started || !m_splits.ShowSplits)
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

            // How much of the split is completed (expressed as percentage)
            //double splitCompletedPct = Math.Round(splitMeters / (double)m_splits.SplitDistanceAsMeters, 4);
            double splitCompletedPct = splitMeters / (double)m_splits.SplitDistanceAsMeters;

            // Compute distance, leave unrounded
            double splitKmTravelled = splitMeters / 1000.0;
            double splitMiTravelled = splitKmTravelled / 1.609;

            double splitDistance = m_splits.SplitsInKm ? splitKmTravelled : splitMiTravelled;
            double splitSpeed = Math.Round((splitDistance / splitTime.TotalSeconds) * 3600, 1);

            // Now round the distance
            splitDistance = Math.Round(splitDistance, 1);

            //double splitAverageKph = Math.Round((splitKmTravelled / splitTime.TotalSeconds) * 3600, 1);
            //double splitAverageMph = Math.Round((splitMiTravelled / splitTime.TotalSeconds) * 3600, 1);

            if (goal != null)
            {
                if (splitKmTravelled >= goal.SplitDistanceKm)
                {
                    // Calculate the deltaTime, positive number is good, negative bad.
                    //TimeSpan deltaTime = goal.TotalTime.Subtract(runningTime);

                    // Calculate the deltaTime, positive number is bad, negative good.
                    TimeSpan deltaTime = new TimeSpan(0, 0, (int)Math.Round(runningTime.Subtract(goal.TotalTime).TotalSeconds, 0));

                    // This completes the split.  TotalDistance travelled and Delta is included.
                    SplitEventArgs args = new SplitEventArgs(m_splitCount + 1, splitTime, splitSpeed, totalDistance, runningTime, m_splits.SplitsInKm, deltaTime);
                    OnSplitGoalCompletedEvent(args);

                    // Reset time and begin next split
                    m_splitStartTime = now;
                    m_splitCount++;

                    m_lastSplitMeters = 0;
                }
                else
                {
                    if (splitMeters - m_lastSplitMeters >= 1) 
                    {
                        // Goal time of split start
                        TimeSpan splitStartTime = goal.TotalTime.Subtract(goal.SplitTime);
                        
                        // Goal time to get to this point in the split
                        TimeSpan splitWaypointTime = splitStartTime.Add(goal.SplitTime.Multiply(splitCompletedPct));

                        // Calculate the deltaTime, positive number is good, negative bad.
                        //TimeSpan deltaTime = splitWaypointTime.Subtract(runningTime);

                        // Calculate the deltaTime, positive number is bad, negative good.
                        TimeSpan deltaTime = new TimeSpan(0, 0, (int)Math.Round(runningTime.Subtract(splitWaypointTime).TotalSeconds, 0));

                        // This is an update to the split in-progress.  SplitDistance travelled is included.
                        SplitEventArgs args = new SplitEventArgs(m_splitCount + 1, splitTime, splitSpeed, splitDistance, runningTime, m_splits.SplitsInKm, deltaTime);
                        OnSplitUpdatedEvent(args);

                        m_lastSplitMeters = splitMeters;

                        Logger.LogInformation($"%Complete: {splitCompletedPct} Start: {splitStartTime.ToString("m'm 's's'")} Waypoint: {splitWaypointTime.ToString("m'm 's's'")} Delta: {deltaTime.ToString("m'm 's's'")}");

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
                    if (splitMeters - m_lastSplitMeters >= 1)
                    {
                        // This is an update to the split in-progress.  SplitDistance traveled is included.
                        SplitEventArgs args = new SplitEventArgs(m_splitCount + 1, splitTime, splitSpeed, splitDistance, runningTime, m_splits.SplitsInKm);
                        OnSplitUpdatedEvent(args);

                        m_lastSplitMeters = splitMeters;
                    }
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
            SplitsV2 splits = ZAMsettings.Settings.SplitsV2;

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
