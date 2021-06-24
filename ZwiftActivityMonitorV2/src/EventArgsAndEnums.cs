using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

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
    public class EnumBase<T>
    {
        protected dynamic EnumList { get; set; }

        /// <summary>
        /// Returns a List of KeyValuePairs containing the EnumValue, StringValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<KeyValuePair<T, string>> GetItems() //where T : System.Enum
        {
            List<KeyValuePair<T, string>> list = new();

            foreach (var key in EnumList.Keys)
                list.Add(new KeyValuePair<T, string>(key, EnumList[key]));

            return list;
        }

        /// <summary>
        /// Returns a single KeyValuePair for a key containing the EnumValue, StringValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public KeyValuePair<T, string> GetItem(T key) 
        {
            return new KeyValuePair<T, string>(key, EnumList[key]);
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

    public sealed class CollectorMetricEnum : EnumBase<CollectorMetricEnum.Keys> // sealed which ensures that the class cannot be inherited and object instantiation is restricted
    {
        public enum Keys
        {
            AP,
            APmax,
            FTP,
            HR
        }

        private static readonly Lazy<CollectorMetricEnum> _InstanceLock = new Lazy<CollectorMetricEnum>(() => new CollectorMetricEnum());

        private CollectorMetricEnum() // private constructor will ensure that the class is not going to be instantiated from outside the class
        {
            EnumList = new Dictionary<CollectorMetricEnum.Keys, string>();

            EnumList.Add(CollectorMetricEnum.Keys.AP, "AP");
            EnumList.Add(CollectorMetricEnum.Keys.APmax, "AP (Max)");
            EnumList.Add(CollectorMetricEnum.Keys.FTP, "FTP");
            EnumList.Add(CollectorMetricEnum.Keys.HR, "HR");
        }

        public static CollectorMetricEnum Instance { get { return _InstanceLock.Value; } }
    }

    #endregion

    #region DurationEnum

    /// <summary>
    /// Defines available collectors.  Each enum has the matching number of seconds as it's value.
    /// </summary>
    public sealed class DurationEnum : EnumBase<DurationEnum.Keys> // sealed which ensures that the class cannot be inherited and object instantiation is restricted
    {
        public enum Keys
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

        private static readonly Lazy<DurationEnum> _InstanceLock = new Lazy<DurationEnum>(() => new DurationEnum());
        
        private DurationEnum() // private constructor will ensure that the class is not going to be instantiated from outside the class
        {
            EnumList = new Dictionary<DurationEnum.Keys, string>();

            EnumList.Add(DurationEnum.Keys.FiveSeconds, "5 sec");
            EnumList.Add(DurationEnum.Keys.ThirtySeconds, "30 sec");
            EnumList.Add(DurationEnum.Keys.OneMinute, "1 min");
            EnumList.Add(DurationEnum.Keys.FiveMinutes, "5 min");
            EnumList.Add(DurationEnum.Keys.SixMinutes, "6 min");
            EnumList.Add(DurationEnum.Keys.TenMinutes, "10 min");
            EnumList.Add(DurationEnum.Keys.TwentyMinutes, "20 min");
            EnumList.Add(DurationEnum.Keys.ThirtyMinutes, "30 min");
            EnumList.Add(DurationEnum.Keys.SixtyMinutes, "60 min");
            EnumList.Add(DurationEnum.Keys.NinetyMinutes, "90 min");
        }

        public static DurationEnum Instance { get { return _InstanceLock.Value; } }

        public void GetDefaults(DurationEnum.Keys duration, out PowerDisplayEnum.Keys apPowerDisplay, out PowerDisplayEnum.Keys apMaxPowerDisplay, out PowerDisplayEnum.Keys ftpPowerDisplay, out bool isVisible)
        {
            apPowerDisplay = PowerDisplayEnum.Keys.Watts;
            apMaxPowerDisplay = PowerDisplayEnum.Keys.WattsPerKg;
            ftpPowerDisplay = PowerDisplayEnum.Keys.None;
            isVisible = false;

            switch(duration)
            {
                case DurationEnum.Keys.OneMinute:
                    isVisible = true;
                    break;

                case DurationEnum.Keys.FiveMinutes:
                    isVisible = true;
                    break;

                case DurationEnum.Keys.TwentyMinutes:
                    ftpPowerDisplay = PowerDisplayEnum.Keys.WattsPerKg;
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

    public sealed class PowerDisplayEnum : EnumBase<PowerDisplayEnum.Keys> // sealed which ensures that the class cannot be inherited and object instantiation is restricted
    {
        public enum Keys
        {
            Watts,
            WattsPerKg,
            Both,
            None,
        }

        private static readonly Lazy<PowerDisplayEnum> _InstanceLock = new Lazy<PowerDisplayEnum>(() => new PowerDisplayEnum());

        private PowerDisplayEnum() // private constructor will ensure that the class is not going to be instantiated from outside the class
        {
            EnumList = new Dictionary<PowerDisplayEnum.Keys, string>();

            EnumList.Add(PowerDisplayEnum.Keys.Watts, "Watts");
            EnumList.Add(PowerDisplayEnum.Keys.WattsPerKg, "W/Kg");
            EnumList.Add(PowerDisplayEnum.Keys.Both, "Both Watts and W/Kg");
            EnumList.Add(PowerDisplayEnum.Keys.None, "None");
        }

        public static PowerDisplayEnum Instance { get { return _InstanceLock.Value; } }
    }

    #endregion

    #region SpeedDisplayEnum

    public sealed class SpeedDisplayEnum : EnumBase<SpeedDisplayEnum.Keys> // sealed which ensures that the class cannot be inherited and object instantiation is restricted
    {
        public enum Keys
        {
            KilometersPerHour,
            MilesPerHour,
            Both,
            None,
        }

        private static readonly Lazy<SpeedDisplayEnum> _InstanceLock = new Lazy<SpeedDisplayEnum>(() => new SpeedDisplayEnum());

        private SpeedDisplayEnum() // private constructor will ensure that the class is not going to be instantiated from outside the class
        {
            EnumList = new Dictionary<SpeedDisplayEnum.Keys, string>();

            EnumList.Add(SpeedDisplayEnum.Keys.KilometersPerHour, "KM/h");
            EnumList.Add(SpeedDisplayEnum.Keys.MilesPerHour, "MI/h");
            EnumList.Add(SpeedDisplayEnum.Keys.Both, "Both KM/h and MI/h");
            EnumList.Add(SpeedDisplayEnum.Keys.None, "None");
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

    public class MovingAverageChangedEventArgs : EventArgs
    {
        public int APwatts { get; }
        public double? APwattsPerKg { get; }
        public int HRbpm { get; }
        public DurationEnum.Keys DurationType { get; }
        public int FTPwatts { get; }
        public double? FTPwattsPerKg { get; }
        public bool ignoreFTP { get; }

        public MovingAverageChangedEventArgs(int apWatts, double? apWattsPerKg, int hrBpm, DurationEnum.Keys durationType, int ftpWatts, double? ftpWattsPerKg, bool ignoreFTP)
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
        public DurationEnum.Keys DurationType { get; }
        public int FTPwattsMax { get; }
        public double? FTPwattsPerKgMax { get; }

        public MovingAverageMaxChangedEventArgs(int apWattsMax, double? apWattsPerKgMax, int hrBpmMax, DurationEnum.Keys durationType, int ftpWattsMax, double? ftpWattsPerKgMax)
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
        public DurationEnum.Keys DurationType { get; }
        public TimeSpan ElapsedTime { get; }

        public MovingAverageCalculatedEventArgs(int apWatts, DurationEnum.Keys durationType, TimeSpan elapsedTime)
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
