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

            foreach (var key in EnumList.Keys)
                list.Add(new KeyStringPair<T>(key, EnumList[key]));

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
            return new KeyStringPair<T>(key, EnumList[key]);
        }

        /// <summary>
        /// Returns a List of all string values
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<string> GetValues() 
        {
            return EnumList.Values.ToList<string>();
        }

        /// <summary>
        /// Returns a single string value for a key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(T key) 
        {
            return EnumList[key];
        }
    }
    #endregion

    #region CollectorMetricEnum
    public enum CollectorMetricType
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

    public sealed class CollectorMetricEnum : EnumBase<CollectorMetricType> // sealed which ensures that the class cannot be inherited and object instantiation is restricted
    {

        private static readonly Lazy<CollectorMetricEnum> _InstanceLock = new Lazy<CollectorMetricEnum>(() => new CollectorMetricEnum());

        private CollectorMetricEnum() // private constructor will ensure that the class is not going to be instantiated from outside the class
        {
            EnumList = new Dictionary<CollectorMetricType, string>();

            EnumList.Add(CollectorMetricType.DetailAP, "AP");
            EnumList.Add(CollectorMetricType.DetailAPmax, "AP (Max)");
            EnumList.Add(CollectorMetricType.DetailFTP, "FTP");
            EnumList.Add(CollectorMetricType.DetailHR, "HR");
            EnumList.Add(CollectorMetricType.SummaryAP, "AP");
            EnumList.Add(CollectorMetricType.SummaryAS, "AS");
            EnumList.Add(CollectorMetricType.SummaryNP, "NP");
            EnumList.Add(CollectorMetricType.SummaryIF, "IF");
            EnumList.Add(CollectorMetricType.SummaryTSS, "TSS");
        }

        public static CollectorMetricEnum Instance { get { return _InstanceLock.Value; } }

        public void GetDefaults(CollectorMetricType metric, out bool isVisible)
        {
            isVisible = true;

            // Add any metric specific logic here
            switch (metric)
            {
                case CollectorMetricType.DetailAP:
                    break;
            }
        }

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
            EnumList = new Dictionary<DurationType, string>();

            EnumList.Add(DurationType.FiveSeconds, "5 sec");
            EnumList.Add(DurationType.ThirtySeconds, "30 sec");
            EnumList.Add(DurationType.OneMinute, "1 min");
            EnumList.Add(DurationType.FiveMinutes, "5 min");
            EnumList.Add(DurationType.SixMinutes, "6 min");
            EnumList.Add(DurationType.TenMinutes, "10 min");
            EnumList.Add(DurationType.TwentyMinutes, "20 min");
            EnumList.Add(DurationType.ThirtyMinutes, "30 min");
            EnumList.Add(DurationType.SixtyMinutes, "60 min");
            EnumList.Add(DurationType.NinetyMinutes, "90 min");
        }

        public static DurationEnum Instance { get { return _InstanceLock.Value; } }

        public void GetDefaults(DurationType duration, out PowerDisplayType apPowerDisplay, out PowerDisplayType apMaxPowerDisplay, out PowerDisplayType ftpPowerDisplay, out bool isVisible)
        {
            apPowerDisplay = PowerDisplayType.Watts;
            apMaxPowerDisplay = PowerDisplayType.WattsPerKg;
            ftpPowerDisplay = PowerDisplayType.None;
            isVisible = false;

            // Add any duration specific logic here
            switch(duration)
            {
                case DurationType.OneMinute:
                    isVisible = true;
                    break;

                case DurationType.FiveMinutes:
                    isVisible = true;
                    break;

                case DurationType.TwentyMinutes:
                    ftpPowerDisplay = PowerDisplayType.WattsPerKg;
                    isVisible = true;
                    break;
            }
        }
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
            EnumList = new Dictionary<PowerDisplayType, string>();

            EnumList.Add(PowerDisplayType.Watts, "Watts");
            EnumList.Add(PowerDisplayType.WattsPerKg, "W/Kg");
            EnumList.Add(PowerDisplayType.Both, "Both Watts and W/Kg");
            EnumList.Add(PowerDisplayType.None, "None");
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
            EnumList = new Dictionary<SpeedDisplayType, string>();

            EnumList.Add(SpeedDisplayType.KilometersPerHour, "KM/h");
            EnumList.Add(SpeedDisplayType.MilesPerHour, "MI/h");
            EnumList.Add(SpeedDisplayType.Both, "Both KM/h and MI/h");
            EnumList.Add(SpeedDisplayType.None, "None");
        }

        public static SpeedDisplayEnum Instance { get { return _InstanceLock.Value; } }
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
