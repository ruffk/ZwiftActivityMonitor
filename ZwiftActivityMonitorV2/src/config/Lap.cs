using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitorV2
{
    /// <summary>
    /// Uses a new technique of enums and dictionary lookups for validation of items used in ComboBoxes and RadioButtons
    /// </summary>
    public class Lap : ConfigItemBase, ICloneable
    {
        public enum DistanceUomType { Kilometers, Miles }
        public enum TriggerPositionType { StartAndLapButton, LapButtonOnly }
        public enum LapStyleType { Manual, Automatic }
        public enum LapTriggerType { Distance, Time, Position }

        // FYI - The setters here should just be "internal set" but then the json deserializer doesn't work properly.
        public KeyStringPair<LapStyleType> LapStyle { get; set; }
        public KeyStringPair<DistanceUomType> TriggerDistanceUom { get; set; }
        public KeyStringPair<TriggerPositionType> TriggerPosition { get; set; }
        public KeyStringPair<LapTriggerType> LapTrigger { get; set; }
        public KeyStringPair<MeasurementSystemType> MeasurementSystem { get; set; }

        private double m_triggerDistance = 5.0;
        private int m_triggerHours;
        private int m_triggerMinutes = 10;
        private int m_triggerSeconds;


        private readonly Dictionary<DistanceUomType, KeyStringPair<DistanceUomType>> m_uomItemList = new();
        private readonly Dictionary<TriggerPositionType, KeyStringPair<TriggerPositionType>> m_positionItemList = new();
        private readonly Dictionary<LapStyleType, KeyStringPair<LapStyleType>> m_lapStyleList = new();
        private readonly Dictionary<LapTriggerType, KeyStringPair<LapTriggerType>> m_lapTriggerList = new();
        private readonly Dictionary<MeasurementSystemType, KeyStringPair<MeasurementSystemType>> m_measurementSystemList = new();

        private ILogger<Lap> Logger { get; }


        public Lap()
        {
            this.Logger = ZAMsettings.LoggerFactory.CreateLogger<Lap>();

            //Logger.LogInformation("CTOR");

            // define all the valid choices for the UI

            // ComboBox will display these items
            m_uomItemList.Add(DistanceUomType.Kilometers, new(DistanceUomType.Kilometers, "km"));
            m_uomItemList.Add(DistanceUomType.Miles, new(DistanceUomType.Miles, "mi"));

            // ComboBox will display these items
            m_positionItemList.Add(TriggerPositionType.StartAndLapButton, new(TriggerPositionType.StartAndLapButton, "Start and Lap Button"));
            m_positionItemList.Add(TriggerPositionType.LapButtonOnly, new(TriggerPositionType.LapButtonOnly, "Lap Button Only"));

            // ComboBox will display these items
            m_measurementSystemList.Add(MeasurementSystemType.Imperial, new(MeasurementSystemType.Imperial, "Imperial"));
            m_measurementSystemList.Add(MeasurementSystemType.Metric, new(MeasurementSystemType.Metric, "Metric"));

            // RadioButtons, text is only used in configuration file
            m_lapStyleList.Add(LapStyleType.Manual, new(LapStyleType.Manual, "Manual"));
            m_lapStyleList.Add(LapStyleType.Automatic, new(LapStyleType.Automatic, "Automatic"));

            // RadioButtons, text is only used in configuration file
            m_lapTriggerList.Add(LapTriggerType.Distance, new(LapTriggerType.Distance, "Distance"));
            m_lapTriggerList.Add(LapTriggerType.Time, new(LapTriggerType.Time, "Time"));
            m_lapTriggerList.Add(LapTriggerType.Position, new(LapTriggerType.Position, "Position"));
        }

        public override int InitializeDefaultValues()
        {
            int count = 0;

            // The KeyStringPair classes need to be initialized with defaults here as they depend on values in the lists.
            // FYI: They can't be initialized in the constructor as they will always show null during json deserialization,
            // even if the json being parsed has values in it.

            if (TriggerDistanceUom == null)
            {
                Logger.LogInformation($"Initializing TriggerDistanceUom");
                TriggerDistanceUom = m_uomItemList[DistanceUomType.Kilometers]; // default
                count++;
            }

            if (TriggerPosition == null)
            {
                Logger.LogInformation($"Initializing TriggerPosition");
                TriggerPosition = m_positionItemList[TriggerPositionType.StartAndLapButton]; // default
                count++;
            }

            if (MeasurementSystem == null)
            {
                Logger.LogInformation($"Initializing MeasurementSystem");
                MeasurementSystem = m_measurementSystemList[MeasurementSystemType.Metric]; // default
                count++;
            }

            if (LapStyle == null)
            {
                Logger.LogInformation($"Initializing LapStyle");
                LapStyle = m_lapStyleList[LapStyleType.Automatic]; // default
                count++;
            }

            if (LapTrigger == null)
            {
                Logger.LogInformation($"Initializing LapTrigger");
                LapTrigger = m_lapTriggerList[LapTriggerType.Position]; // default
                count++;
            }

            return count;
        }

        /// <summary>
        /// ComboBox items
        /// </summary>
        [JsonIgnore]
        public KeyStringPair<DistanceUomType>[] TriggerDistanceUomItems
        {
            get
            {
                return m_uomItemList.Values.ToArray();
            }
        }

        /// <summary>
        /// ComboBox items
        /// </summary>
        [JsonIgnore]
        public KeyStringPair<TriggerPositionType>[] TriggerPositionItems
        {
            get
            {
                return m_positionItemList.Values.ToArray();
            }
        }

        /// <summary>
        /// ComboBox items
        /// </summary>
        [JsonIgnore]
        public KeyStringPair<MeasurementSystemType>[] MeasurementSystemItems
        {
            get
            {
                return m_measurementSystemList.Values.ToArray();
            }
        }

        public double TriggerDistance
        {
            get { return m_triggerDistance; }

            set
            {
                if (value < 1 || value > 999)
                    throw new FormatException("Trigger distance value must be between 1 and 999.");

                m_triggerDistance = Math.Round(value, 1);
            }
        }

        [JsonIgnore]
        public bool TriggerDistanceInKm
        {
            get { return TriggerDistanceUom.Key == DistanceUomType.Kilometers; }
        }

        [JsonIgnore]
        public double TriggerDistanceAsKm
        {
            get { return TriggerDistanceInKm ? m_triggerDistance : m_triggerDistance * 1.609; }
        }

        [JsonIgnore]
        public int TriggerDistanceAsMeters
        {
            get { return (int)Math.Round(TriggerDistanceAsKm * 1000, 0); }
        }

        /// <summary>
        /// TriggerDistanceUom is a ComboBox control.  The full KeyStringPair is stored in the item array for display.
        /// During validation, just the Key is checked for validity.
        /// </summary>
        [JsonIgnore]
        public DistanceUomType TriggerDistanceUomSetting
        {
            get { return TriggerDistanceUom.Key; }
            set
            {
                if (!m_uomItemList.ContainsKey(value))
                    throw new FormatException("DistanceUomType key not found.");

                TriggerDistanceUom = m_uomItemList[value];
            }
        }

        /// <summary>
        /// TriggerPosition is a ComboBox control.  The full KeyStringPair is stored in the item array for display.
        /// During validation, just the Key is checked for validity.
        /// </summary>
        [JsonIgnore]
        public TriggerPositionType TriggerPositionSetting
        {
            get { return TriggerPosition.Key; }
            set
            {
                if (!m_positionItemList.ContainsKey(value))
                    throw new FormatException("TriggerPositionType key not found.");

                TriggerPosition = m_positionItemList[value];
            }
        }

        /// <summary>
        /// MeasurementSystem is a ComboBox control.  The full KeyStringPair is stored in the item array for display.
        /// During validation, just the Key is checked for validity.
        /// </summary>
        [JsonIgnore]
        public MeasurementSystemType MeasurementSystemSetting
        {
            get { return MeasurementSystem.Key; }
            set
            {
                if (!m_measurementSystemList.ContainsKey(value))
                    throw new FormatException("MeasurementSystemType key not found.");

                MeasurementSystem = m_measurementSystemList[value];
            }
        }

        /// <summary>
        /// LapStyle is a Radio Button / Checkbox control.  The enum is stored in the control's Tag.
        /// </summary>
        [JsonIgnore]
        public LapStyleType LapStyleSetting
        {
            get { return LapStyle.Key; }
            set
            {
                if (!m_lapStyleList.ContainsKey(value))
                    throw new FormatException("LapStyleType key not found.");

                LapStyle = m_lapStyleList[value];
            }
        }

        /// <summary>
        /// LapTrigger is a Radio Button / Checkbox control.  The enum is stored in the control's Tag.
        /// </summary>
        [JsonIgnore]
        public LapTriggerType LapTriggerSetting
        {
            get { return LapTrigger.Key; }
            set
            {
                if (!m_lapTriggerList.ContainsKey(value))
                    throw new FormatException("LapTriggerType key not found.");

                LapTrigger = m_lapTriggerList[value];
            }
        }

        /// <summary>
        /// Helper method.  Returns true if auto-lapping by position
        /// </summary>
        [JsonIgnore]
        public bool AutoLapByPosition
        {
            get
            {
                return (LapStyleSetting == LapStyleType.Automatic && LapTriggerSetting == LapTriggerType.Position);
            }
        }




        public int TriggerHours
        {
            get { return m_triggerHours; }

            set
            {
                if (value < 0 || value > 23)
                    throw new FormatException("Trigger hours value must be between 0 and 23.");

                m_triggerHours = value;
            }
        }
        public int TriggerMinutes
        {
            get { return m_triggerMinutes; }

            set
            {
                if (value < 0 || value > 59)
                    throw new FormatException("Trigger minutes value must be between 0 and 59.");

                m_triggerMinutes = value;
            }
        }
        public int TriggerSeconds
        {
            get { return m_triggerSeconds; }

            set
            {
                if (value < 0 || value > 59)
                    throw new FormatException("Trigger seconds value must be between 0 and 59.");

                m_triggerSeconds = value;
            }
        }


        [JsonIgnore]
        public TimeSpan TriggerTime
        {
            get
            {
                return new TimeSpan(TriggerHours, TriggerMinutes, TriggerSeconds);
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }
}
