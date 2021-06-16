using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ZwiftActivityMonitorV2
{
    public enum DurationType
    {
        FiveSeconds,
        ThirtySeconds,
        OneMinute,
        FiveMinutes,
        SixMinutes,
        TenMinutes,
        TwentyMinutes,
        ThirtyMinutes,
        SixtyMinutes,
        NinetyMinutes
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

        public RiderStateEventArgs(ZwiftPacketMonitor.PlayerStateEventArgs e)
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
        }
    }

}
