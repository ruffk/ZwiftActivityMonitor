using System;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ZwiftActivityMonitorV2
{
    public class RideRecapMetrics
    {
        public TimeSpan Duration { get; set; }
        public double DistanceKm { get; set; }
        public double DistanceMi { get; set; }
        public double AverageKph { get; set; }
        public double AverageMph { get; set; }
        public int OverallPower { get; set; }
        public int NormalizedPower { get; set; }
        public double? IntensityFactor { get; set; } // null if FTP not set
        public int? TotalSufferScore { get; set; } // null if FTP not set

        public RideRecapMetrics()
        {
        }
    }


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

        private ulong mSumMovingAvgPow4;
        private int mCountMovingAvgPow4;

        private int mCurNormalizedPower;
        private double? mCurIntensityFactor;
        private int? mCurTotalSufferScore;
        private bool mStarted;

        private double mCurAvgKph;
        private double mCurAvgMph;
        private int mCurOverallPower;
        private TimeSpan mCurDuration;
        private double mCurDistanceKm;
        private double mCurDistanceMi;

        //private DateTime m_collectionStartTime; // Time when collection started

        private UserProfile CurrentUser { get; set; }

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
            mMovingAvg = new MovingAverage(DurationEnum.Keys.ThirtySeconds, false, false);
            mMovingAvg.MovingAverageCalculatedEvent += MovingAverageCalculatedEventHandler;
            mMovingAvg.MetricsCalculatedEvent += MetricsCalculatedEventHandler;

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
            if (!mStarted)
            {
                this.CurrentUser = ZAMsettings.Settings.CurrentUser;

                mCountMovingAvgPow4 = 0;
                mCurNormalizedPower = 0;
                mCurIntensityFactor = null;
                mCurTotalSufferScore = null;
                mSumMovingAvgPow4 = 0;
                mCurAvgKph = 0;
                mCurAvgMph = 0;
                mCurOverallPower = 0;
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

        public RideRecapMetrics RideRecap
        {
            get 
            {
                return new RideRecapMetrics()
                {
                    Duration = mCurDuration,
                    DistanceKm = mCurDistanceKm,
                    DistanceMi = mCurDistanceMi,
                    AverageKph = mCurAvgKph,
                    AverageMph = mCurAvgMph,
                    OverallPower = mCurOverallPower,
                    TotalSufferScore = mCurTotalSufferScore,
                    IntensityFactor = mCurIntensityFactor,
                    NormalizedPower = mCurNormalizedPower,
                };
            }
        }

        private void MovingAverageCalculatedEventHandler(object sender, MovingAverageCalculatedEventArgs e)
        {
            if (!mStarted)
                return;

            double? intensityFactor = null;
            int? totalSufferScore = null;
            double? npWattsPerKg = null;

            ulong movingAvgPow4 = (ulong)Math.Pow(e.APwatts, 4);

            mSumMovingAvgPow4 += movingAvgPow4;
            mCountMovingAvgPow4 += 1;

            double avgMovingAvgPow4 = mSumMovingAvgPow4 / (double)mCountMovingAvgPow4;

            int npWatts = (int)Math.Round(Math.Pow(avgMovingAvgPow4, 0.25), 0);

            // calculate average w/kg
            if (CurrentUser.WeightAsKgs > 0)
                npWattsPerKg = Math.Round(npWatts / CurrentUser.WeightAsKgs, 2);


            if (CurrentUser.PowerThreshold > 0)
            {
                // Calculate Intensity Factor
                intensityFactor = Math.Round(npWatts / (double)CurrentUser.PowerThreshold, 2);

                // Calculate TSS
                //TimeSpan runningTime = DateTime.Now - m_collectionStartTime;
                totalSufferScore = (int)Math.Round((e.ElapsedTime.TotalSeconds * npWatts * (double)intensityFactor) / (CurrentUser.PowerThreshold * 3600) * 100, 0);
            }

            // when NP changes, send it and the current overall average power through
            if (npWatts != mCurNormalizedPower || intensityFactor != mCurIntensityFactor || totalSufferScore != mCurTotalSufferScore)
            {
                mCurNormalizedPower = npWatts;
                mCurTotalSufferScore = totalSufferScore;
                mCurIntensityFactor = intensityFactor;

                OnNormalizedPowerChangedEvent(new NormalizedPowerChangedEventArgs(npWatts, npWattsPerKg, intensityFactor, totalSufferScore));
            }
        }
        private void MetricsCalculatedEventHandler(object sender, MetricsCalculatedEventArgs e)
        {
            if (!mStarted)
                return;

            // just saving these most recent values for ride recap
            mCurDuration = e.ElapsedTime;
            mCurDistanceKm = e.DistanceKm;
            mCurDistanceMi = e.DistanceMi;

            if (e.SpeedKph != mCurAvgKph || e.SpeedMph != mCurAvgMph || e.APwatts != mCurOverallPower)
            {
                mCurAvgKph = e.SpeedKph;
                mCurAvgMph = e.SpeedMph;
                mCurOverallPower = e.APwatts;

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
                    Logger.LogWarning(ex, ex.ToString());
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
                    Logger.LogWarning(ex, ex.ToString());
                }
            }
        }
    }
}
