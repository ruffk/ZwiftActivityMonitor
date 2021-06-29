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
    public class SplitsV2 : ConfigItemBase, ICloneable
    {
        // FYI - The setters here should just be "internal set" but then the json deserializer doesn't work properly.
        public KeyStringPair<DistanceUomType> SplitDistanceUom { get; set; }

        public bool ShowSplits { get; set; }
        public bool CalculateGoal { get; set; }
        public bool Customized { get; set; }
        public double GoalSpeed { get; set; }

        private int m_splitDistance = 5;
        private double m_goalDistance = 25;
        private TimeSpan m_goalTime = new TimeSpan(0, 45, 0);
        private readonly Dictionary<DistanceUomType, KeyStringPair<DistanceUomType>> m_uomItemList = new();

        private ILogger<SplitsV2> Logger { get; }

        public List<SplitV2> Splits { get; } = new();


        public SplitsV2()
        {
            this.Logger = ZAMsettings.LoggerFactory.CreateLogger<SplitsV2>();

            // ComboBox will display these items
            m_uomItemList.Add(DistanceUomType.Kilometers, new(DistanceUomType.Kilometers, "km"));
            m_uomItemList.Add(DistanceUomType.Miles, new(DistanceUomType.Miles, "mi"));
        }

        public override int InitializeDefaultValues()
        {
            int count = 0;

            // The KeyStringPair classes need to be initialized with defaults here as they depend on values in the lists.
            // FYI: They can't be initialized in the constructor as they will always show null during json deserialization,
            // even if the json being parsed has values in it.

            if (SplitDistanceUom == null)
            {
                Logger.LogInformation($"Initializing SplitDistanceUom");
                SplitDistanceUom = m_uomItemList[DistanceUomType.Kilometers]; // default
                count++;
            }

            foreach(SplitV2 split in Splits)
            {
                count += split.InitializeDefaultValues();
            }

            return count;
        }

        /// <summary>
        /// ComboBox items
        /// </summary>
        [JsonIgnore]
        public KeyStringPair<DistanceUomType>[] SplitDistanceUomItems
        {
            get
            {
                return m_uomItemList.Values.ToArray();
            }
        }

        /// <summary>
        /// DistanceUom is a ComboBox control.  The full KeyStringPair is stored in the item array for display.
        /// During validation, just the Key is checked for validity.
        /// </summary>
        [JsonIgnore]
        public DistanceUomType SplitDistanceUomSetting
        {
            get { return SplitDistanceUom.Key; }
            set
            {
                if (!m_uomItemList.ContainsKey(value))
                    throw new FormatException("DistanceUomType key not found.");

                SplitDistanceUom = m_uomItemList[value];
            }
        }

        public int SplitDistance
        {
            get { return m_splitDistance; }

            set
            {
                if (value < 1 || value > 999)
                    throw new FormatException("Split distance value must be between 1 and 999.");

                m_splitDistance = value;
            }
        }

        [JsonIgnore]
        public bool SplitsInKm
        {
            get { return SplitDistanceUom.Value == "km"; }
        }

        [JsonIgnore]
        public double SplitDistanceAsKm
        {
            get { return SplitsInKm ? m_splitDistance : m_splitDistance * 1.609; }
        }

        [JsonIgnore]
        public int SplitDistanceAsMeters
        {
            get { return (int)Math.Round(SplitDistanceAsKm * 1000, 0); }
        }

        public TimeSpan GoalTime
        {
            get
            {
                return m_goalTime;
            }
            set
            {
                if (value.TotalMinutes < 5)
                    throw new FormatException("Goal time must be at least 5 minutes.");

                m_goalTime = value;
            }
        }

        public double GoalDistance
        {
            get { return m_goalDistance; }

            set
            {
                if (value < 1 || value > 999)
                    throw new FormatException("Goal distance value must be between 1 and 999.");

                m_goalDistance = Math.Round(value, 1);
            }
        }

        [JsonIgnore]
        public string GoalSpeedStr
        {
            get
            {
                return $"{GoalSpeed:#.0} {(SplitsInKm ? "km/h" : "mi/h")}";
            }
        }

        [JsonIgnore]
        public string GoalDistanceStr
        {
            get
            {
                return $"{GoalDistance:#.0} {(SplitsInKm ? "km" : "mi")}";
            }
        }

        [JsonIgnore]
        public string GoalTimeStr
        {
            get
            {
                return GoalTime.Hours > 0 ? Math.Floor(GoalTime.TotalHours) + GoalTime.ToString("'h 'm'm 's's'") : GoalTime.ToString("m'm 's's'");
            }
        }


        public void CalculateDefaultSplits()
        {
            this.Splits.Clear();
            this.Customized = false;
            this.GoalSpeed = 0;

            if (!this.ShowSplits || !this.CalculateGoal)
                return;

            double numSplits = this.GoalDistance / this.SplitDistance;
            //if (numSplits < 1)
            //    return;

            if (this.GoalTime.TotalSeconds < 1)
                return;

            this.GoalSpeed = Math.Round((this.GoalDistance / this.GoalTime.TotalSeconds) * 3600, 1);

            TimeSpan splitTime = new TimeSpan(0, 0, (int)Math.Round(this.GoalTime.TotalSeconds / numSplits, 0));

            int curDistance = 0;
            TimeSpan curTime = new TimeSpan();

            for (int i = 0; i < (int)numSplits; i++)
            {
                int totalDistance = curDistance + this.SplitDistance;
                TimeSpan totalTime = curTime.Add(splitTime);

                double splitSpeed = Math.Round((this.SplitDistance / splitTime.TotalSeconds) * 3600, 1);
                double averageSpeed = Math.Round((totalDistance / totalTime.TotalSeconds) * 3600, 1);

                SplitV2 item = new SplitV2(this.SplitDistance, splitTime, splitSpeed, totalDistance, totalTime, averageSpeed, SplitDistanceUom.Key);
                this.Splits.Add(item);

                curDistance = totalDistance;
                curTime = totalTime;
            }

            if (numSplits != (int)numSplits)
            {
                double lastSplitDistance = Math.Round(this.GoalDistance - curDistance, 1);
                TimeSpan lastSplitTime = this.GoalTime.Subtract(curTime);

                double lastSplitSpeed = Math.Round((lastSplitDistance / lastSplitTime.TotalSeconds) * 3600, 1);

                SplitV2 item = new SplitV2(lastSplitDistance, lastSplitTime, lastSplitSpeed, this.GoalDistance, this.GoalTime, this.GoalSpeed, SplitDistanceUom.Key);
                this.Splits.Add(item);
            }
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }

    public class SplitV2 : ConfigItemBase
    {
        // FYI - The setters here should just be "internal set" but then the json deserializer doesn't work properly.
        public double SplitDistance { get; set; }
        public TimeSpan SplitTime { get; set; }
        public double SplitSpeed { get; set; }
        public double TotalDistance { get; set; }
        public TimeSpan TotalTime { get; set; }
        public double AverageSpeed { get; set; }
        public DistanceUomType SplitDistanceUom { get; set; } = DistanceUomType.Kilometers;

        [JsonConstructor]
        public SplitV2()
        {
        }

        public SplitV2(double splitDistance, TimeSpan splitTime, double splitSpeed, double totalDistance, TimeSpan totalTime, double averageSpeed, DistanceUomType distanceUomType)
        {
            this.SplitDistance = splitDistance;
            this.SplitTime = splitTime;
            this.SplitSpeed = splitSpeed;
            this.TotalDistance = totalDistance;
            this.TotalTime = totalTime;
            this.AverageSpeed = averageSpeed;
            this.SplitDistanceUom = distanceUomType;
        }

        public override int InitializeDefaultValues()
        {
            int count = 0;

            // The KeyStringPair classes need to be initialized with defaults here as they depend on values in the lists.
            // FYI: They can't be initialized in the constructor as they will always show null during json deserialization,
            // even if the json being parsed has values in it.

            return count;
        }


        [JsonIgnore]
        public double SplitDistanceAsKm
        {
            get { return this.SplitDistanceUom == DistanceUomType.Kilometers ? this.SplitDistance : this.SplitDistance * 1.609; }
        }

        [JsonIgnore]
        public int SplitDistanceAsMeters
        {
            get { return (int)Math.Round(this.SplitDistanceAsKm * 1000, 0); }
        }

        [JsonIgnore]
        public double TotalDistanceAsKm
        {
            get { return this.SplitDistanceUom == DistanceUomType.Kilometers ? this.TotalDistance : this.TotalDistance * 1.609; }
        }

        [JsonIgnore]
        public int TotalDistanceAsMeters
        {
            get { return (int)Math.Round(this.TotalDistanceAsKm * 1000, 0); }
        }

    }
}
