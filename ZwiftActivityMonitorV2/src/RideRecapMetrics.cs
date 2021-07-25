using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZwiftActivityMonitorV2
{
    public class RideRecapMetrics
    {
        public TimeSpan Duration { get; set; }
        public double DistanceKm { get; set; }
        public double DistanceMi { get; set; }
        public double AverageKph { get; set; }
        public double AverageMph { get; set; }
        public int APwatts { get; set; }
        public double? APwattsPerKg { get; set; }
        public int NPwatts { get; set; }
        public double? NPwattsPerKg { get; set; }
        public double? IntensityFactor { get; set; } // null if FTP not set
        public int? TrainingStressScore { get; set; } // null if FTP not set

        public RideRecapLap[] Laps { get; set; }
        public RideRecapSplit[] Splits { get; set; }
        public RideRecapPower[] Power { get; set; }

        public RideRecapMetrics()
        {
        }
    }

    public class RideRecapPower
    {
        public DurationType DurationType { get; }
        public int APwattsMax { get; }
        public double? APwattsKgMax { get; }
        public RideRecapPower(DurationType durationType, int apWattsMax, double? apWattsKgMax)
        {
            this.DurationType = durationType;
            this.APwattsMax = apWattsMax;
            this.APwattsKgMax = apWattsKgMax;
        }
    }

    public class RideRecapLap
    {
        public int LapNumber { get; set; }
        public TimeSpan LapTime { get; set; }
        public double LapSpeedKph { get; set; }
        public double LapSpeedMph { get; set; }
        public double LapDistanceMi { get; set; }
        public double LapDistanceKm { get; set; }
        public int LapAPwatts { get; set; }
        public double? LapAPwattsPerKg { get; set; }
        public TimeSpan TotalTime { get; set; }

        public RideRecapLap()
        {

        }
    }
    public class RideRecapSplit
    {
        public int SplitNumber { get; set; }
        public TimeSpan SplitTime { get; set; }
        public double SplitSpeedKph { get; set; }
        public double SplitSpeedMph { get; set; }
        public double SplitDistanceMi { get; set; }
        public double SplitDistanceKm { get; set; }
        public TimeSpan TotalTime { get; set; }
        public TimeSpan? DeltaTime { get; set; }

        public RideRecapSplit()
        {

        }
    }
}
