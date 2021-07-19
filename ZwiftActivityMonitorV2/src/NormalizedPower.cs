using System;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ZwiftActivityMonitorV2
{
    /// <summary>
    /// rolling_average = 30 second rolling average
    /// rolling_avg_powered = rolling_average ^ 4
    /// avg_powered_values = average of rolling_avg_powered
    /// NP = avg_powered_values ^ 0.25
    /// </summary>
    public class NormalizedPower
    {
        private readonly ILogger<NormalizedPower> Logger;

        private readonly MovingAverage mMovingAvg;
        private UserProfile CurrentUserProfile { get { return ZAMsettings.Settings.CurrentUser; } }

        private ulong mSumMovingAvgPow4;
        private int mCountMovingAvgPow4;

        private int mCurNPwatts;
        private double? mCurNPwattsPerKg;
        private double? mCurIntensityFactor;
        private int? mCurTrainingStressScore;
        private bool mStarted;

        private double mCurAvgKph;
        private double mCurAvgMph;
        private int mCurAPwatts;
        private double? mCurAPwattsPerKg;
        private TimeSpan mCurDuration;
        private double mCurDistanceKm;
        private double mCurDistanceMi;

        //private DateTime m_collectionStartTime; // Time when collection started

        //private UserProfile CurrentUser { get; set; }

        #region Public EventArgs classes


        #endregion

        public event EventHandler<NormalizedPowerChangedEventArgs> NormalizedPowerChangedEvent;
        public event EventHandler<MetricsChangedEventArgs> MetricsChangedEvent;


        public NormalizedPower()
        {
            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<NormalizedPower>();

            // Create a new 30 seconds moving average class, zero power reading numbers are INCLUDED (I asked support at TrainingPeaks about this).  Don't use high-res packet collection.
            mMovingAvg = new MovingAverage(DurationType.ThirtySeconds, false, false);
            mMovingAvg.MovingAverageCalculatedEvent += MovingAverageCalculatedEventHandler;
            mMovingAvg.MetricsCalculatedEvent += MetricsCalculatedEventHandler;

            ZAMsettings.ZPMonitorService.CollectionStatusChanged += ZPMonitorService_CollectionStatusChanged;

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
            if (!mStarted)
            {
                mCountMovingAvgPow4 = 0;
                mCurNPwatts = 0;
                mCurNPwattsPerKg = null;
                mCurIntensityFactor = null;
                mCurTrainingStressScore = null;
                mSumMovingAvgPow4 = 0;
                mCurAvgKph = 0;
                mCurAvgMph = 0;
                mCurAPwatts = 0;
                mCurAPwattsPerKg = null;
                mCurDuration = TimeSpan.Zero;
                mCurDistanceKm = 0;
                mCurDistanceMi = 0;

                //m_collectionStartTime = DateTime.Now;

                mStarted = true;
            }
        }

        private void Stop()
        {
            if (mStarted)
            {
                mStarted = false;
            }
        }

        public RideRecapMetrics GetRideRecapMetrics()
        {
            return new RideRecapMetrics()
            {
                Duration = mCurDuration,
                DistanceKm = mCurDistanceKm,
                DistanceMi = mCurDistanceMi,
                AverageKph = mCurAvgKph,
                AverageMph = mCurAvgMph,
                APwatts = mCurAPwatts,
                APwattsPerKg = mCurAPwattsPerKg,
                TrainingStressScore = mCurTrainingStressScore,
                IntensityFactor = mCurIntensityFactor,
                NPwatts = mCurNPwatts,
                NPwattsPerKg = mCurNPwattsPerKg,
            };
        }

        private void MovingAverageCalculatedEventHandler(object sender, MovingAverageCalculatedEventArgs e)
        {
            if (!mStarted)
                return;

            double? intensityFactor = null;
            int? trainingStressScore = null;
            double? npWattsPerKg = null;

            ulong movingAvgPow4 = (ulong)Math.Pow(e.APwatts, 4);

            mSumMovingAvgPow4 += movingAvgPow4;
            mCountMovingAvgPow4 += 1;

            double avgMovingAvgPow4 = mSumMovingAvgPow4 / (double)mCountMovingAvgPow4;

            double npWatts = Math.Pow(avgMovingAvgPow4, 0.25);

            // calculate average w/kg
            npWattsPerKg = CalculateUserWattsPerKg(npWatts);


            if (CurrentUserProfile.PowerThreshold > 0)
            {
                // Calculate Intensity Factor
                intensityFactor = Math.Round(npWatts / (double)CurrentUserProfile.PowerThreshold, 2);

                // Calculate TSS
                //TimeSpan runningTime = DateTime.Now - m_collectionStartTime;
                trainingStressScore = (int)Math.Round((e.ElapsedTime.TotalSeconds * npWatts * (double)intensityFactor) / (CurrentUserProfile.PowerThreshold * 3600) * 100, 0);
            }

            npWatts = Math.Round(npWatts, 0);

            // when NP changes, send it and the current overall average power through
            if ((int)npWatts != this.mCurNPwatts || intensityFactor != this.mCurIntensityFactor || trainingStressScore != this.mCurTrainingStressScore)
            {
                this.mCurNPwatts = (int)npWatts;
                this.mCurNPwattsPerKg = npWattsPerKg;
                this.mCurTrainingStressScore = trainingStressScore;
                this.mCurIntensityFactor = intensityFactor;

                OnNormalizedPowerChangedEvent(new NormalizedPowerChangedEventArgs((int)npWatts, npWattsPerKg, intensityFactor, trainingStressScore));
            }
        }

        private double? CalculateUserWattsPerKg(double watts)
        {
            return CurrentUserProfile.WeightAsKgs > 0 ? Math.Round(watts / CurrentUserProfile.WeightAsKgs, 2) : null;
        }

        private void MetricsCalculatedEventHandler(object sender, MetricsCalculatedEventArgs e)
        {
            if (!mStarted)
                return;

            // just saving these most recent values for ride recap
            mCurDuration = e.ElapsedTime;
            mCurDistanceKm = e.DistanceKm;
            mCurDistanceMi = e.DistanceMi;

            if (e.SpeedKph != this.mCurAvgKph || e.SpeedMph != this.mCurAvgMph || e.APwatts != this.mCurAPwatts)
            {
                this.mCurAvgKph = e.SpeedKph;
                this.mCurAvgMph = e.SpeedMph;
                this.mCurAPwatts = e.APwatts;
                this.mCurAPwattsPerKg = e.APwattsPerKg;

                OnMetricsChangedEvent(new MetricsChangedEventArgs(e.SpeedKph, e.SpeedMph, e.APwatts, e.APwattsPerKg));
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
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnNormalizedPowerChangedEvent)");
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
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnMetricsChangedEvent)");
                }
            }
        }
    }
}
