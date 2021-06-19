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
        private int m_avgPower;
        private int m_avgHR;
        private DurationType m_durationType;

        public MovingAverageChangedEventArgs(int avgPower, int avgHR, DurationType durationType)
        {
            m_avgPower = avgPower;
            m_avgHR = avgHR;
            m_durationType = durationType;
        }

        public int AveragePower
        {
            get { return m_avgPower; }
        }
        public int AverageHR
        {
            get { return m_avgHR; }
        }

        public DurationType DurationType { get { return m_durationType; } }
    }
    public class MovingAverageMaxChangedEventArgs : EventArgs
    {
        private int m_avgPower;
        private int m_avgHR;
        private DurationType m_durationType;

        public MovingAverageMaxChangedEventArgs(int avgPower, int avgHR, DurationType durationType)
        {
            m_avgPower = avgPower;
            m_avgHR = avgHR;
            m_durationType = durationType;
        }

        public int MaxAvgPower
        {
            get { return m_avgPower; }
        }
        public int MaxAvgHR
        {
            get { return m_avgHR; }
        }
        public DurationType DurationType { get { return m_durationType; } }
    }
    public class MovingAverageCalculatedEventArgs : EventArgs
    {
        public int AveragePower { get; }
        public DurationType DurationType { get; }
        public TimeSpan ElapsedTime { get; }

        public MovingAverageCalculatedEventArgs(int avgPower, DurationType durationType, TimeSpan elapsedTime)
        {
            this.AveragePower = avgPower;
            this.DurationType = durationType;
            this.ElapsedTime = elapsedTime;
        }
    }

    public class MetricsCalculatedEventArgs : EventArgs
    {
        public int OverallPower { get; }
        public double AverageKph { get; }
        public double AverageMph { get; }

        public TimeSpan Duration { get; }
        public double DistanceKm { get; }
        public double DistanceMi { get; }

        public MetricsCalculatedEventArgs(int overallPower, double averageKph, double averageMph, TimeSpan duration, double distanceKm, double distanceMi)
        {
            OverallPower = overallPower;
            AverageKph = averageKph;
            AverageMph = averageMph;
            Duration = duration;
            DistanceKm = distanceKm;
            DistanceMi = distanceMi;
        }
    }

    public class RiderStateEventArgs : EventArgs
    {
        public int Id { get; }
        public int Power { get; }
        public int Heartrate { get; }
        public int Distance { get; }
        public int Time { get; }
        public long WorldTime { get; }
        public int RoadId { get; }
        public bool IsForward { get; }
        public int Course { get; }
        public int RoadTime { get; }
        //public int RoadPosition { get; }
        public int WatchingRiderId { get; }
        public DateTime? CollectionStartTime { get; }
        public TimeSpan? ElapsedTime { get; }

        public RiderStateEventArgs(ZwiftPacketMonitor.PlayerStateEventArgs e, DateTime? collectionStart)
        {
            this.Id = e.PlayerState.Id;
            this.Power = e.PlayerState.Power;
            this.Heartrate = e.PlayerState.Heartrate;
            this.Distance = e.PlayerState.Distance;
            this.Time = e.PlayerState.Time;
            this.WorldTime = e.PlayerState.WorldTime;
            this.RoadTime = e.PlayerState.RoadTime;
            //this.RoadPosition = e.PlayerState.RoadPosition;
            this.WatchingRiderId = e.PlayerState.WatchingRiderId;

            // credit for decoding these goes to zoffline/zwift-offline/standalone.py
            this.RoadId = (e.PlayerState.F20 & 0xFF00) >> 8;
            this.IsForward = (e.PlayerState.F19 & 0x04) != 0;
            this.Course = (e.PlayerState.F19 & 0xFF0000) >> 16;

            this.CollectionStartTime = collectionStart;
            if (collectionStart != null)
                this.ElapsedTime = DateTime.Now - collectionStart;
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
