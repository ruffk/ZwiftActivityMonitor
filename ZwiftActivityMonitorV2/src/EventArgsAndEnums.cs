using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using Newtonsoft.Json;

namespace ZwiftActivityMonitorV2
{
    public enum DistanceUomType
    {
        Kilometers,
        Miles
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




    #region EnumBase<T>
    public class EnumListItem
    {
        public string Text { get; }
        public string MenuItemText { get; }
        public string ColumnHeaderText { get; }

        public EnumListItem(string text, string menuItemText = "", string columnHeaderText = "")
        {
            this.Text = text;

            this.MenuItemText = String.IsNullOrEmpty(menuItemText) ? text : menuItemText;
            this.ColumnHeaderText = String.IsNullOrEmpty(columnHeaderText) ? text : columnHeaderText;
        }
    }

    public class EnumBase<T> where T : System.Enum
    {
        protected dynamic EnumList { get; set; }

        /// <summary>
        /// Returns a List of KeyStringPairs containing the EnumValue, StringValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<KeyStringPair<T>> GetItems() //where T : System.Enum
        {
            List<KeyStringPair<T>> list = new();

            foreach (T key in EnumList.Keys)
                list.Add(new KeyStringPair<T>(key, (EnumList[key] as EnumListItem).Text));

            return list;
        }

        /// <summary>
        /// Returns a single KeyStringPair for a key containing the EnumValue, StringValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public KeyStringPair<T> GetItem(T key) 
        {
            return new KeyStringPair<T>(key, (EnumList[key] as EnumListItem).Text);
        }

        /// <summary>
        /// Returns a List of all string values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<string> GetValues() 
        {
            List<KeyStringPair<T>> itemList = this.GetItems();
            List<string> list = new();

            foreach (var item in itemList)
                list.Add(item.Value);
            
            return list;
        }

        /// <summary>
        /// Returns Text value for a key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetText(T key)
        {
            return (EnumList[key] as EnumListItem).Text;
        }

        /// <summary>
        /// Returns the MenuItemText value for a key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetMenuItemText(T key)
        {
            return (EnumList[key] as EnumListItem).MenuItemText;
        }

        /// <summary>
        /// Returns the ColumnHeaderText value for a key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetColumnHeaderText(T key)
        {
            return (EnumList[key] as EnumListItem).ColumnHeaderText;
        }
    }
    #endregion

    #region ActivityViewMetricEnum
    public enum ActivityViewMetricType
    {
        DetailAP,
        DetailAPmax,
        DetailFTP,
        DetailHR,
        SummaryAS,
        SummaryAP,
        SummaryNP,
        SummaryIF,
        SummaryTSS,
    }

    public sealed class ActivityViewMetricEnum : EnumBase<ActivityViewMetricType> // sealed which ensures that the class cannot be inherited and object instantiation is restricted
    {

        private static readonly Lazy<ActivityViewMetricEnum> _InstanceLock = new Lazy<ActivityViewMetricEnum>(() => new ActivityViewMetricEnum());

        private ActivityViewMetricEnum() // private constructor will ensure that the class is not going to be instantiated from outside the class
        {
            EnumList = new Dictionary<ActivityViewMetricType, EnumListItem>();

            EnumList.Add(ActivityViewMetricType.DetailAP, new EnumListItem("AP"));
            EnumList.Add(ActivityViewMetricType.DetailAPmax, new EnumListItem("AP (Max)"));
            EnumList.Add(ActivityViewMetricType.DetailFTP, new EnumListItem("95%"));
            EnumList.Add(ActivityViewMetricType.DetailHR, new EnumListItem("HR"));
            EnumList.Add(ActivityViewMetricType.SummaryAP, new EnumListItem("AP"));
            EnumList.Add(ActivityViewMetricType.SummaryAS, new EnumListItem("AS"));
            EnumList.Add(ActivityViewMetricType.SummaryNP, new EnumListItem("NP"));
            EnumList.Add(ActivityViewMetricType.SummaryIF, new EnumListItem("IF"));
            EnumList.Add(ActivityViewMetricType.SummaryTSS, new EnumListItem("TSS"));
        }

        public static ActivityViewMetricEnum Instance { get { return _InstanceLock.Value; } }
    }

    #endregion

    #region DurationEnum

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

    /// <summary>
    /// Defines available collectors.  Each enum has the matching number of seconds as it's value.
    /// </summary>
    public sealed class DurationEnum : EnumBase<DurationType> // sealed which ensures that the class cannot be inherited and object instantiation is restricted
    {

        private static readonly Lazy<DurationEnum> _InstanceLock = new Lazy<DurationEnum>(() => new DurationEnum());
        
        private DurationEnum() // private constructor will ensure that the class is not going to be instantiated from outside the class
        {
            EnumList = new Dictionary<DurationType, EnumListItem>();

            EnumList.Add(DurationType.FiveSeconds, new EnumListItem("5 sec"));
            EnumList.Add(DurationType.ThirtySeconds, new EnumListItem("30 sec"));
            EnumList.Add(DurationType.OneMinute, new EnumListItem("1 min"));
            EnumList.Add(DurationType.FiveMinutes, new EnumListItem("5 min"));
            EnumList.Add(DurationType.SixMinutes, new EnumListItem("6 min"));
            EnumList.Add(DurationType.TenMinutes, new EnumListItem("10 min"));
            EnumList.Add(DurationType.TwentyMinutes, new EnumListItem("20 min"));
            EnumList.Add(DurationType.ThirtyMinutes, new EnumListItem("30 min"));
            EnumList.Add(DurationType.SixtyMinutes, new EnumListItem("60 min"));
            EnumList.Add(DurationType.NinetyMinutes, new EnumListItem("90 min"));
        }

        public static DurationEnum Instance { get { return _InstanceLock.Value; } }
    }
    #endregion

    #region LapViewMetricEnum
    public enum LapViewMetricType
    {
        DetailLapNumber,
        DetailLapTime,
        DetailLapSpeed,
        DetailLapDistance,
        DetailLapAP,
        DetailTotalTime,
    }

    public sealed class LapViewMetricEnum : EnumBase<LapViewMetricType> // sealed which ensures that the class cannot be inherited and object instantiation is restricted
    {

        private static readonly Lazy<LapViewMetricEnum> _InstanceLock = new Lazy<LapViewMetricEnum>(() => new LapViewMetricEnum());

        private LapViewMetricEnum() // private constructor will ensure that the class is not going to be instantiated from outside the class
        {
            EnumList = new Dictionary<LapViewMetricType, EnumListItem>();

            EnumList.Add(LapViewMetricType.DetailLapNumber, new EnumListItem("Lap Number", columnHeaderText: "#"));
            EnumList.Add(LapViewMetricType.DetailLapTime, new EnumListItem("Lap Time", columnHeaderText: "Lap\nTime"));
            EnumList.Add(LapViewMetricType.DetailLapSpeed, new EnumListItem("Lap Speed", columnHeaderText: "Mi/h"));
            EnumList.Add(LapViewMetricType.DetailLapDistance, new EnumListItem("Lap Distance", columnHeaderText: "Mi"));
            EnumList.Add(LapViewMetricType.DetailLapAP, new EnumListItem("Lap Power", columnHeaderText: "AP"));
            EnumList.Add(LapViewMetricType.DetailTotalTime, new EnumListItem("Total Time", columnHeaderText: "Total\nTime"));
        }

        public static LapViewMetricEnum Instance { get { return _InstanceLock.Value; } }
    }

    #endregion

    #region TemplateEnum

    //public sealed class TemplateEnum : EnumBase<TemplateEnum.Keys> // sealed which ensures that the class cannot be inherited and object instantiation is restricted
    //{
    //    public enum Keys
    //    {
    //        YourKeysHere
    //    }

    //    private static readonly Lazy<TemplateEnum> _InstanceLock = new Lazy<TemplateEnum>(() => new TemplateEnum());

    //    private TemplateEnum() // private constructor will ensure that the class is not going to be instantiated from outside the class
    //    {
    //        EnumList = new Dictionary<TemplateEnum.Keys, string>();

    //        EnumList.Add(TemplateEnum.Keys.YourKeysHere, "SomeStringValue");
    //    }

    //    public static TemplateEnum Instance { get { return _InstanceLock.Value; } }
    //}

    #endregion

    #region PowerDisplayEnum
    public enum PowerDisplayType
    {
        Watts,
        WattsPerKg,
        Both,
        None,
    }

    public sealed class PowerDisplayEnum : EnumBase<PowerDisplayType> // sealed which ensures that the class cannot be inherited and object instantiation is restricted
    {

        private static readonly Lazy<PowerDisplayEnum> _InstanceLock = new Lazy<PowerDisplayEnum>(() => new PowerDisplayEnum());

        private PowerDisplayEnum() // private constructor will ensure that the class is not going to be instantiated from outside the class
        {
            EnumList = new Dictionary<PowerDisplayType, EnumListItem>();

            EnumList.Add(PowerDisplayType.Watts, new EnumListItem("Watts"));
            EnumList.Add(PowerDisplayType.WattsPerKg, new EnumListItem("W/Kg"));
            EnumList.Add(PowerDisplayType.Both, new EnumListItem("Both Watts and W/Kg"));
            EnumList.Add(PowerDisplayType.None, new EnumListItem("None"));
        }

        public static PowerDisplayEnum Instance { get { return _InstanceLock.Value; } }
    }

    #endregion

    #region SpeedDisplayEnum
    public enum SpeedDisplayType
    {
        KilometersPerHour,
        MilesPerHour,
        Both,
        None,
    }

    public sealed class SpeedDisplayEnum : EnumBase<SpeedDisplayType> // sealed which ensures that the class cannot be inherited and object instantiation is restricted
    {

        private static readonly Lazy<SpeedDisplayEnum> _InstanceLock = new Lazy<SpeedDisplayEnum>(() => new SpeedDisplayEnum());

        private SpeedDisplayEnum() // private constructor will ensure that the class is not going to be instantiated from outside the class
        {
            EnumList = new Dictionary<SpeedDisplayType, EnumListItem>();

            EnumList.Add(SpeedDisplayType.KilometersPerHour, new EnumListItem("Kilometers per Hour", columnHeaderText: "KPH"));
            EnumList.Add(SpeedDisplayType.MilesPerHour, new EnumListItem("Miles per Hour", columnHeaderText: "MPH"));
            EnumList.Add(SpeedDisplayType.Both, new EnumListItem("Both KPH and MPH"));
            EnumList.Add(SpeedDisplayType.None, new EnumListItem("None"));
        }

        public static SpeedDisplayEnum Instance { get { return _InstanceLock.Value; } }
    }

    #endregion

    #region DistanceDisplayEnum
    public enum DistanceDisplayType
    {
        Kilometers,
        Miles,
        Both,
        None,
    }

    public sealed class DistanceDisplayEnum : EnumBase<DistanceDisplayType> // sealed which ensures that the class cannot be inherited and object instantiation is restricted
    {

        private static readonly Lazy<DistanceDisplayEnum> _InstanceLock = new Lazy<DistanceDisplayEnum>(() => new DistanceDisplayEnum());

        private DistanceDisplayEnum() // private constructor will ensure that the class is not going to be instantiated from outside the class
        {
            EnumList = new Dictionary<DistanceDisplayType, EnumListItem>();

            EnumList.Add(DistanceDisplayType.Kilometers, new EnumListItem("Kilometers", columnHeaderText: "Km"));
            EnumList.Add(DistanceDisplayType.Miles, new EnumListItem("Miles", columnHeaderText: "Mi"));
            EnumList.Add(DistanceDisplayType.Both, new EnumListItem("Both Kilometers and Miles"));
            EnumList.Add(DistanceDisplayType.None, new EnumListItem("None"));
        }

        public static DistanceDisplayEnum Instance { get { return _InstanceLock.Value; } }
    }

    #endregion

    #region SplitViewMetricEnum
    public enum SplitViewMetricType
    {
        DetailSplitNumber,
        DetailSplitTime,
        DetailSplitSpeed,
        DetailSplitDistance,
        DetailTotalTime,
        DetailDeltaTime,
        SummaryGoalSpeed,
        SummaryGoalDistance,
        SummaryGoalTime,
    }

    public sealed class SplitViewMetricEnum : EnumBase<SplitViewMetricType> // sealed which ensures that the class cannot be inherited and object instantiation is restricted
    {

        private static readonly Lazy<SplitViewMetricEnum> _InstanceLock = new Lazy<SplitViewMetricEnum>(() => new SplitViewMetricEnum());

        private SplitViewMetricEnum() // private constructor will ensure that the class is not going to be instantiated from outside the class
        {
            EnumList = new Dictionary<SplitViewMetricType, EnumListItem>();

            EnumList.Add(SplitViewMetricType.DetailSplitNumber, new EnumListItem("Split Number", columnHeaderText: "#"));
            EnumList.Add(SplitViewMetricType.DetailSplitTime, new EnumListItem("Split Time", columnHeaderText: "Split\nTime"));
            EnumList.Add(SplitViewMetricType.DetailSplitSpeed, new EnumListItem("Split Speed", columnHeaderText: "Mi/h"));
            EnumList.Add(SplitViewMetricType.DetailSplitDistance, new EnumListItem("Split Distance", columnHeaderText: "Mi"));
            EnumList.Add(SplitViewMetricType.DetailTotalTime, new EnumListItem("Total Time", columnHeaderText: "Total\nTime"));
            EnumList.Add(SplitViewMetricType.DetailDeltaTime, new EnumListItem("Delta Time", columnHeaderText: "Time\n+/-"));
            EnumList.Add(SplitViewMetricType.SummaryGoalSpeed, new EnumListItem("Goal Speed", columnHeaderText: "Mi/h"));
            EnumList.Add(SplitViewMetricType.SummaryGoalDistance, new EnumListItem("Goal Distance", columnHeaderText: "Mi"));
            EnumList.Add(SplitViewMetricType.SummaryGoalTime, new EnumListItem("Goal Time", columnHeaderText: "Time"));
        }

        public static SplitViewMetricEnum Instance { get { return _InstanceLock.Value; } }
    }

    #endregion


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
    public class CountdownTimerTickEventArgs : EventArgs
    {
        public TimeSpan TimeRemaining { get; }
        public bool IsCompleted { get; set; }
        public bool IsCanceled { get; set; }
        public bool StartWithEventTimer { get; set; }

        public CountdownTimerTickEventArgs(TimeSpan timeRemaining)
        {
            this.TimeRemaining = timeRemaining;
        }
        public CountdownTimerTickEventArgs()
        {
        }
    }
    public class LapEventArgs : EventArgs
    {
        public int LapNumber { get; }
        public TimeSpan LapTime { get; }
        public double LapDistanceKm { get; }
        public double LapDistanceMi { get; }
        public int LapAPwatts { get; }
        public double? LapAPwattsPerKg { get; }
        public TimeSpan TotalTime { get; }
        public double LapSpeedKph { get; }
        public double LapSpeedMph { get; }

        public LapEventArgs(int lapNumber, TimeSpan lapTime, double lapDistanceKm, double lapDistanceMi, int lapAPwatts, double? lapAPwattsPerKg, TimeSpan totalTime, double lapSpeedKph, double lapSpeedMph)
        {
            this.LapNumber = lapNumber;
            this.LapTime = lapTime;
            this.LapDistanceKm = lapDistanceKm;
            this.LapDistanceMi = lapDistanceMi;
            this.LapAPwatts = lapAPwatts;
            this.LapAPwattsPerKg = lapAPwattsPerKg;
            this.TotalTime = totalTime;
            this.LapSpeedKph = lapSpeedKph;
            this.LapSpeedMph = lapSpeedMph;
        }
    }
    public class LapStartedEventArgs : EventArgs
    {
        public int LapNumber { get; }
        public string StatusMsg { get; }

        public LapStartedEventArgs(int lapNumber, string statusMsg)
        {
            this.LapNumber = lapNumber;
            this.StatusMsg = statusMsg;
        }
    }

    public class MovingAverageChangedEventArgs : EventArgs
    {
        public int APwatts { get; }
        public double? APwattsPerKg { get; }
        public int HRbpm { get; }
        public DurationType DurationType { get; }
        public int FTPwatts { get; }
        public double? FTPwattsPerKg { get; }
        public bool ignoreFTP { get; }

        public MovingAverageChangedEventArgs(int apWatts, double? apWattsPerKg, int hrBpm, DurationType durationType, int ftpWatts, double? ftpWattsPerKg, bool ignoreFTP)
        {
            this.APwatts = apWatts;
            this.APwattsPerKg = apWattsPerKg;
            this.HRbpm = hrBpm;
            this.DurationType = durationType;
            this.FTPwatts = ftpWatts;
            this.FTPwattsPerKg = ftpWattsPerKg;
            this.ignoreFTP = ignoreFTP;
        }
    }
    public class MovingAverageMaxChangedEventArgs : EventArgs
    {
        public int APwattsMax { get; }
        public double? APwattsPerKgMax { get; }
        public int HRbpmMax { get; }
        public DurationType DurationType { get; }
        public int FTPwattsMax { get; }
        public double? FTPwattsPerKgMax { get; }

        public MovingAverageMaxChangedEventArgs(int apWattsMax, double? apWattsPerKgMax, int hrBpmMax, DurationType durationType, int ftpWattsMax, double? ftpWattsPerKgMax)
        {
            this.APwattsMax = apWattsMax;
            this.APwattsPerKgMax = apWattsPerKgMax;
            this.HRbpmMax = hrBpmMax;
            this.DurationType = durationType;
            this.FTPwattsMax = ftpWattsMax;
            this.FTPwattsPerKgMax = ftpWattsPerKgMax;
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
        public double? APwattsPerKg { get; }

        public TimeSpan ElapsedTime { get; }
        public double DistanceKm { get; }
        public double DistanceMi { get; }

        public MetricsCalculatedEventArgs(int apWatts, double? apWattsPerKg, double speedKph, double speedMph, TimeSpan elapsedTime, double distanceKm, double distanceMi)
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
        public double? APwattsPerKg { get; }

        public MetricsChangedEventArgs(double speedKph, double speedMph, int apWatts, double? apWattsPerKg)
        {
            this.SpeedKph = speedKph;
            this.SpeedMph = speedMph;
            this.APwatts = apWatts;
            this.APwattsPerKg = apWattsPerKg;
        }
    }


    public class RiderStateEventArgs : EventArgs, ICloneable
    {
        public int Id { get; set; }
        public int Power { get; set; }
        public int Heartrate { get; set; }
        public int Distance { get; set; }
        public int ElapsedTimeSecs { get; set; }
        public TimeSpan ElapsedTime
        {
            get { return new TimeSpan(0, 0, this.ElapsedTimeSecs); }
        }
        public int RoadId { get; set; }
        public bool IsForward { get; set; }
        public int Course { get; set; }
        public bool IsPaused { get; set; }
        public TimeSpan PauseDuration { get; set; }

        /// <summary>
        /// The value from PlayerState.RoadTime
        /// </summary>
        public int RoadLocation { get; set; }

        /// <summary>
        /// The time captured when the collectors began running
        /// </summary>
        public DateTime? CollectionStartTime { get; }

        /// <summary>
        /// Elapsed time since collection started.
        /// </summary>
        public TimeSpan? CollectionTime { get; }

        /// <summary>
        /// Elapsed time since collection started, minus time spent paused.
        /// </summary>
        public TimeSpan? AdjustedCollectionTime { get; }


        //public long WorldTime { get; set; }
        //public int WatchingRiderId { get; set; }

        public RiderStateEventArgs(ZwiftPacketMonitor.PlayerStateEventArgs e, DateTime? collectionStart, bool isPaused, TimeSpan pauseDuration)
        {
            this.Id = e.PlayerState.Id;
            this.Power = e.PlayerState.Power;
            this.Heartrate = e.PlayerState.Heartrate;
            this.Distance = e.PlayerState.Distance;
            this.ElapsedTimeSecs = e.PlayerState.Time;
            //this.WorldTime = e.PlayerState.WorldTime;
            this.RoadLocation = e.PlayerState.RoadTime;
            //this.WatchingRiderId = e.PlayerState.WatchingRiderId;

            // credit for decoding these goes to zoffline/zwift-offline/standalone.py
            this.RoadId = (e.PlayerState.F20 & 0xFF00) >> 8;
            this.IsForward = (e.PlayerState.F19 & 0x04) != 0;
            this.Course = (e.PlayerState.F19 & 0xFF0000) >> 16;
            
            this.IsPaused = isPaused;
            this.PauseDuration = pauseDuration;

            this.CollectionStartTime = collectionStart;
            if (collectionStart != null)
            {
                this.CollectionTime = DateTime.Now - collectionStart;
                this.AdjustedCollectionTime = this.CollectionTime - this.PauseDuration;
            }
        }

        /// <summary>
        /// Constructor used when simulating power.
        /// </summary>
        /// <param name="collectionStart"></param>
        public RiderStateEventArgs(DateTime? collectionStart, bool isPaused, TimeSpan pauseDuration)
        {
            this.IsPaused = isPaused;
            this.PauseDuration = pauseDuration;

            this.CollectionStartTime = collectionStart;
            if (collectionStart != null)
            {
                this.CollectionTime = DateTime.Now - collectionStart;
                this.AdjustedCollectionTime = this.CollectionTime - this.PauseDuration;
                this.ElapsedTimeSecs = (int)this.AdjustedCollectionTime.Value.TotalSeconds;
            }
        }

        public override string ToString()
        {
            string str = "";
            str += $"Id: {this.Id}, ";
            str += $"Power: {this.Power}, ";
            str += $"Heartrate: {this.Heartrate}, ";
            str += $"Distance: {this.Distance}, ";
            str += $"ElapsedTime: {this.ElapsedTime}, ";
            //str += $"WorldTime: {this.WorldTime}, ";
            str += $"RoadLocation: {this.RoadLocation}, ";
            //str += $"WatchingRiderId: {this.WatchingRiderId}, ";
            str += $"RoadId: {this.RoadId}, ";
            str += $"IsForward: {this.IsForward}, ";
            str += $"Course: {this.Course}, ";
            str += $"CollectionStartTime: {(this.CollectionStartTime ?? DateTime.MinValue)}, ";
            str += $"CollectionTime: {(this.CollectionTime ?? TimeSpan.Zero)}, ";
            str += $"IsPaused: {this.IsPaused}, ";
            str += $"PauseDuration: {this.PauseDuration}, ";
            str += $"AdjustedCollectionTime: {(this.AdjustedCollectionTime ?? TimeSpan.Zero)}, ";

            return str;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class SpeedDisplayTypeChangedEventArgs : EventArgs
    {
        public string ColumnName { get; }
        public SpeedDisplayType DisplayType { get; }
        public object Tag { get; }

        public SpeedDisplayTypeChangedEventArgs(string columnName, SpeedDisplayType speedDisplayType, object tag)
        {
            this.ColumnName = columnName;
            this.DisplayType = speedDisplayType;
            this.Tag = tag;
        }
    }

    public class DistanceDisplayTypeChangedEventArgs : EventArgs
    {
        public string ColumnName { get; }
        public DistanceDisplayType DisplayType { get; }
        public object Tag { get; }

        public DistanceDisplayTypeChangedEventArgs(string columnName, DistanceDisplayType distanceDisplayType, object tag)
        {
            this.ColumnName = columnName;
            this.DisplayType = distanceDisplayType;
            this.Tag = tag;
        }
    }
    public class PowerDisplayTypeChangedEventArgs : EventArgs
    {
        public string ColumnName { get; }
        public PowerDisplayType DisplayType { get; }
        public object Tag { get; }

        public PowerDisplayTypeChangedEventArgs(string columnName, PowerDisplayType powerDisplayType, object tag)
        {
            this.ColumnName = columnName;
            this.DisplayType = powerDisplayType;
            this.Tag = tag;
        }
    }

    public class SplitEventArgs : EventArgs
    {
        public int SplitNumber { get; }
        public TimeSpan SplitTime { get; }
        public double SplitSpeedMph { get; }
        public double SplitSpeedKph { get; }
        public double TotalMiTravelled { get; }
        public double TotalKmTravelled { get; }
        public TimeSpan TotalTime { get; }
        public bool SplitsInKm { get; }
        public TimeSpan? DeltaTime { get; }

        public SplitEventArgs(int splitNumber, TimeSpan splitTime, double splitSpeedMph, double splitSpeedKph, double totalMiTravelled, double totalKmTravelled, TimeSpan totalTime, bool splitsInKm, TimeSpan? deltaTime = null)
        {
            this.SplitNumber = splitNumber;
            this.SplitTime = splitTime;
            this.SplitSpeedKph = splitSpeedKph;
            this.SplitSpeedMph = splitSpeedMph;
            this.TotalKmTravelled = totalKmTravelled;
            this.TotalMiTravelled = totalMiTravelled;
            this.TotalTime = totalTime;
            this.SplitsInKm = splitsInKm;
            this.DeltaTime = deltaTime;
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
            Cancelled,
            Paused,
            Resumed,
        }
        public ActionType Action { get; }
        /// <summary>
        /// For ActionType.Resumed, elapsed time between Paused and Resumed events
        /// </summary>
        public TimeSpan? PauseDuration { get; }

        public CollectionStatusChangedEventArgs(ActionType action)
        {
            this.Action = action;

        }
        public CollectionStatusChangedEventArgs(ActionType action, TimeSpan pauseDuration)
        {
            this.Action = action;
            this.PauseDuration = pauseDuration;
        }
    }

}
