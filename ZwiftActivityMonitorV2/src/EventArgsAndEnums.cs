using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ZwiftActivityMonitorV2
{
    /// <summary>
    /// Defines available collectors.  Each enum has the matching number of seconds as it's value.
    /// </summary>
    public enum DurationType
    {
        FiveSeconds = 5,
        ThirtySeconds = 30,
        OneMinute = 60,
        FiveMinutes = 300,
        SixMinutes = 360,
        TenMinutes = 600,
        TwentyMinutes = 1200,
        ThirtyMinutes = 1800,
        SixtyMinutes = 3600,
        NinetyMinutes = 5400
    }
    public enum PowerDisplayType
    {
        Watts,
        WattsPerKg,
        Both,
        None,
    }
    public enum SpeedDisplayType
    {
        KilometersPerHour,
        MilesPerHour,
        Both,
        None,
    }

    public enum FieldUomType
    {
        Hidden,
        Watts,
        Wkg
    }

    public enum MeasurementSystemType
    {
        Imperial,
        Metric
    }

    public enum ThemeType
    {
        Custom,
        Office2010Black,
        Office2010Blue,
        Office2010Silver,
        ZwiftyOrange
    }

    public enum TransparencyType
    {
        NotTransparent,
        TransparentBlackText,
        TransparentWhiteText
    }

    public class EnumManager
    {
        public class DurationTypeEnum
        {
            static private Dictionary<DurationType, string> _List = new();

            static DurationTypeEnum()
            {
                _List.Add(DurationType.FiveSeconds, "5 sec");
                _List.Add(DurationType.ThirtySeconds, "30 sec");
                _List.Add(DurationType.OneMinute, "1 min");
                _List.Add(DurationType.FiveMinutes, "5 min");
                _List.Add(DurationType.SixMinutes, "6 min");
                _List.Add(DurationType.TenMinutes, "10 min");
                _List.Add(DurationType.TwentyMinutes, "20 min");
                _List.Add(DurationType.ThirtyMinutes, "30 min");
                _List.Add(DurationType.SixtyMinutes, "60 min");
                _List.Add(DurationType.NinetyMinutes, "90 min");
            }

            static public List<KeyValuePair<DurationType, string>> ToList()
            {
                List<KeyValuePair<DurationType, string>> list = new();

                foreach (var key in _List.Keys)
                    list.Add(new KeyValuePair<DurationType, string>(key, _List[key]));

                return list;
            }

            static public List<string> Values
            {
                get { return _List.Values.ToList<string>(); }
            }

            static public string GetItem(DurationType key)
            {
                return _List[key];
            }
        }

        public class PowerDisplayTypeEnum
        {
            static private Dictionary<PowerDisplayType, string> _List = new();

            static PowerDisplayTypeEnum()
            {
                _List.Add(PowerDisplayType.Watts, "Watts");
                _List.Add(PowerDisplayType.WattsPerKg, "W/Kg");
                _List.Add(PowerDisplayType.Both, "Both Watts and W/Kg");
                _List.Add(PowerDisplayType.None, "None");
            }

            static public List<KeyValuePair<PowerDisplayType, string>> ToList()
            {
                List<KeyValuePair<PowerDisplayType, string>> list = new();

                foreach (var key in _List.Keys)
                    list.Add(new KeyValuePair<PowerDisplayType, string>(key, _List[key]));

                return list;
            }
            static public List<string> Values
            {
                get { return _List.Values.ToList<string>(); }
            }

            static public string GetItem(PowerDisplayType key)
            {
                return _List[key];
            }
        }
        public class SpeedDisplayTypeEnum
        {
            static private Dictionary<SpeedDisplayType, string> _List = new();

            static SpeedDisplayTypeEnum()
            {
                _List.Add(SpeedDisplayType.KilometersPerHour, "KM/h");
                _List.Add(SpeedDisplayType.MilesPerHour, "MI/h");
                _List.Add(SpeedDisplayType.Both, "Both KM/h and MI/h");
                _List.Add(SpeedDisplayType.None, "None");
            }

            static public List<KeyValuePair<SpeedDisplayType, string>> ToList()
            {
                List<KeyValuePair<SpeedDisplayType, string>> list = new();

                foreach (var key in _List.Keys)
                    list.Add(new KeyValuePair<SpeedDisplayType, string>(key, _List[key]));

                return list;
            }
            static public List<string> Values
            {
                get { return _List.Values.ToList<string>(); }
            }

            static public string GetItem(SpeedDisplayType key)
            {
                return _List[key];
            }
        }
    }



    public class FormSyncTimerTickEventArgs : EventArgs
    {
        public int TickCount {get;}

        public FormSyncTimerTickEventArgs(int tickCount)
        {
            this.TickCount = tickCount;
        }
    }


    public class ColorsAndFontChangedEventArgs : EventArgs
    {
        public ThemeType ThemeSetting { get; }
        public TransparencyType TransparencySetting { get; }
        public Color ManagedColor { get; }
        public string FontFamily { get; }
        public float FontSize { get; }
        public bool IsFontBold { get; }
        public bool IsFontItalic { get; }

        public ColorsAndFontChangedEventArgs(ThemeType themeSetting, TransparencyType transparencySetting, Color managedColor, string fontFamily, float fontSize, bool isFontBold, bool isFontItalic)
        {
            this.ThemeSetting = themeSetting;
            this.TransparencySetting = transparencySetting;
            this.ManagedColor = managedColor;
            this.FontFamily = fontFamily;
            this.FontSize = fontSize;
            this.IsFontBold = isFontBold;
            this.IsFontItalic = isFontItalic;
        }

    }

    public class MovingAverageChangedEventArgs : EventArgs
    {
        public int APwatts { get; }
        public int HRbpm { get; }
        public DurationType DurationType { get; }
        public double APwattsPerKg { get; }

        public MovingAverageChangedEventArgs(int apWatts, int hrBpm, DurationType durationType, double apWattsPerKg)
        {
            this.APwatts = apWatts;
            this.HRbpm = hrBpm;
            this.DurationType = durationType;
            this.APwattsPerKg = apWattsPerKg;
        }
    }
    public class MovingAverageMaxChangedEventArgs : EventArgs
    {
        public int APwattsMax { get; }
        public int HRbpmMax { get; }
        public DurationType DurationType { get; }
        public double APwattsPerKgMax { get; }

        public MovingAverageMaxChangedEventArgs(int apWattsMax, int hrBpmMax, DurationType durationType, double apWattsPerKgMax)
        {
            this.APwattsMax = apWattsMax;
            this.HRbpmMax = hrBpmMax;
            this.DurationType = durationType;
            this.APwattsPerKgMax = apWattsPerKgMax;
        }

    }
    public class MovingAverageCalculatedEventArgs : EventArgs
    {
        public int APwatts { get; }
        public DurationType DurationType { get; }
        public TimeSpan ElapsedTime { get; }

        public MovingAverageCalculatedEventArgs(int apWatts, DurationType durationType, TimeSpan elapsedTime)
        {
            this.APwatts = apWatts;
            this.DurationType = durationType;
            this.ElapsedTime = elapsedTime;
        }
    }

    public class MetricsCalculatedEventArgs : EventArgs
    {
        public double SpeedKph { get; }
        public double SpeedMph { get; }
        public int APwatts { get; }
        public double APwattsPerKg { get; }

        public TimeSpan ElapsedTime { get; }
        public double DistanceKm { get; }
        public double DistanceMi { get; }

        public MetricsCalculatedEventArgs(int apWatts, double apWattsPerKg, double speedKph, double speedMph, TimeSpan elapsedTime, double distanceKm, double distanceMi)
        {
            this.APwatts = apWatts;
            this.APwattsPerKg = apWattsPerKg;
            this.SpeedKph = speedKph;
            this.SpeedMph = speedMph;
            this.ElapsedTime = elapsedTime;
            this.DistanceKm = distanceKm;
            this.DistanceMi = distanceMi;
        }
    }

    public class NormalizedPowerChangedEventArgs : EventArgs
    {
        public int NPwatts { get; }
        public double? NPwattsPerKg { get; }
        public double? IFvalue { get; }
        public int? TSSvalue { get; }

        public NormalizedPowerChangedEventArgs(int npWatts, double? npWattsPerKg, double? ifValue, int? tssValue)
        {
            this.NPwatts = npWatts;
            this.NPwattsPerKg = npWattsPerKg;
            this.IFvalue = ifValue; ;
            this.TSSvalue = tssValue;
        }

    }
    public class MetricsChangedEventArgs : EventArgs
    {
        public double SpeedKph { get; }
        public double SpeedMph { get; }
        public int APwatts { get; }
        public double APwattsPerKg { get; }

        public MetricsChangedEventArgs(double speedKph, double speedMph, int apWatts, double apWattsPerKg)
        {
            this.SpeedKph = speedKph;
            this.SpeedMph = speedMph;
            this.APwatts = apWatts;
            this.APwattsPerKg = apWattsPerKg;
        }
    }


    public class RiderStateEventArgs : EventArgs
    {
        public int Id { get; set; }
        public int Power { get; set; }
        public int Heartrate { get; set; }
        public int Distance { get; set; }
        //public int Time { get; set; }
        //public long WorldTime { get; set; }
        public int RoadId { get; set; }
        public bool IsForward { get; set; }
        public int Course { get; set; }
        public int RoadTime { get; set; }
        //public int WatchingRiderId { get; set; }
        public DateTime? CollectionStartTime { get; }
        public TimeSpan? ElapsedTime { get; }

        public RiderStateEventArgs(ZwiftPacketMonitor.PlayerStateEventArgs e, DateTime? collectionStart)
        {
            this.Id = e.PlayerState.Id;
            this.Power = e.PlayerState.Power;
            this.Heartrate = e.PlayerState.Heartrate;
            this.Distance = e.PlayerState.Distance;
            //this.Time = e.PlayerState.Time;
            //this.WorldTime = e.PlayerState.WorldTime;
            this.RoadTime = e.PlayerState.RoadTime;
            //this.WatchingRiderId = e.PlayerState.WatchingRiderId;

            // credit for decoding these goes to zoffline/zwift-offline/standalone.py
            this.RoadId = (e.PlayerState.F20 & 0xFF00) >> 8;
            this.IsForward = (e.PlayerState.F19 & 0x04) != 0;
            this.Course = (e.PlayerState.F19 & 0xFF0000) >> 16;

            this.CollectionStartTime = collectionStart;
            if (collectionStart != null)
                this.ElapsedTime = DateTime.Now - collectionStart;
        }

        public RiderStateEventArgs(DateTime? collectionStart)
        {
            this.CollectionStartTime = collectionStart;
            if (collectionStart != null)
                this.ElapsedTime = DateTime.Now - collectionStart;
        }

        public override string ToString()
        {
            string str = "";
            str += $"Id: {this.Id}, ";
            str += $"Power: {this.Power}, ";
            str += $"Heartrate: {this.Heartrate}, ";
            str += $"Distance: {this.Distance}, ";
            //str += $"Time: {this.Time}, ";
            //str += $"WorldTime: {this.WorldTime}, ";
            str += $"RoadTime: {this.RoadTime}, ";
            //str += $"WatchingRiderId: {this.WatchingRiderId}, ";
            str += $"RoadId: {this.RoadId}, ";
            str += $"IsForward: {this.IsForward}, ";
            str += $"Course: {this.Course}, ";
            str += $"CollectionStartTime: {(this.CollectionStartTime ?? DateTime.MinValue)}, ";
            str += $"ElapsedTime: {(this.ElapsedTime ?? TimeSpan.Zero)}";

            return str;
        }

    }

    public class ZPMonitorServiceStatusChangedEventArgs : EventArgs
    {
        public enum ActionType
        {
            Started,
            Stopped
        }
        public ActionType Action { get; }

        public ZPMonitorServiceStatusChangedEventArgs(ActionType action)
        {
            this.Action = action;
        }
    }
    public class CollectionStatusChangedEventArgs : EventArgs
    {
        public enum ActionType
        {
            Started,
            Waiting,
            Stopped,
            Cancelled
        }
        public ActionType Action { get; }

        public CollectionStatusChangedEventArgs(ActionType action)
        {
            this.Action = action;

        }
    }

}
