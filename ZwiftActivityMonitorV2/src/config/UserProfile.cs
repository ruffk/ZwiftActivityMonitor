using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;


namespace ZwiftActivityMonitorV2
{
    #region UserActivityViewColumnSettings class
    /// <summary>
    /// Settings for the ActivityViewerControl's Detail and Summary column visibility
    /// </summary>
    public class UserActivityViewColumnSettings : ConfigItemBase, ICloneable
    {
        public KeyStringPair<ActivityViewMetricType> Metric { get; set; }

        public bool? IsVisible { get; set; }

        [JsonConstructor]
        public UserActivityViewColumnSettings()
        {
        }

        public UserActivityViewColumnSettings(ActivityViewMetricType metric)
        {
            this.MetricSetting = metric;
        }

        /// <summary>
        /// The full KeyValuePair is stored in the item array for display.
        /// </summary>
        [JsonIgnore]
        public ActivityViewMetricType MetricSetting
        {
            get { return Metric.Key; }
            set
            {
                Metric = ActivityViewMetricEnum.Instance.GetItem(value);
            }
        }


        public override int InitializeDefaultValues()
        {
            int count = 0;

            // The KeyStringPair classes need to be initialized with defaults here as they depend on values in the lists.
            // FYI: They can't be initialized in the constructor as they will always show null during json deserialization,
            // even if the json being parsed has values in it.

            if (this.IsVisible == null)
            {
                switch(this.MetricSetting)
                {
                    case ActivityViewMetricType.SummaryKJ:
                        this.IsVisible = false;
                        break;

                    default:
                        this.IsVisible = true;
                        break;
                }
                count++;
            }

            return count;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    #endregion

    #region UserActivityViewDetailRowSettings class
    /// <summary>
    /// Settings for the ActivityViewControl's Detail row Power columns and row visibility
    /// </summary>
    public class UserActivityViewDetailRowSettings : ConfigItemBase, ICloneable
    {
        // FYI - The setters here should just be "internal set" but then the json deserializer doesn't work properly.
        public KeyStringPair<DurationType> Duration { get; set; }

        public bool? IsVisible { get; set; }

        public SortedList<ActivityViewMetricType, KeyStringPair<PowerDisplayType>> PowerValues = new();

        [JsonConstructor]
        public UserActivityViewDetailRowSettings()
        {

        }

        public UserActivityViewDetailRowSettings(DurationType duration)
        {
            this.DurationSetting = duration;
        }

        public override int InitializeDefaultValues()
        {
            int count = 0;

            // The KeyStringPair classes need to be initialized with defaults here as they depend on values in the lists.
            // FYI: They can't be initialized in the constructor as they will always show null during json deserialization,
            // even if the json being parsed has values in it.

            if (!this.PowerValues.ContainsKey(ActivityViewMetricType.DetailAP))
            {
                this.PowerValues.Add(ActivityViewMetricType.DetailAP, PowerDisplayEnum.Instance.GetItem(PowerDisplayType.Watts));
                count++;
            }

            if (!this.PowerValues.ContainsKey(ActivityViewMetricType.DetailAPmax))
            {
                this.PowerValues.Add(ActivityViewMetricType.DetailAPmax, PowerDisplayEnum.Instance.GetItem(PowerDisplayType.WattsPerKg));
                count++;
            }

            if (!this.PowerValues.ContainsKey(ActivityViewMetricType.DetailFTP))
            {
                switch (this.DurationSetting)
                {
                    case DurationType.TwentyMinutes:
                        this.PowerValues.Add(ActivityViewMetricType.DetailFTP, PowerDisplayEnum.Instance.GetItem(PowerDisplayType.WattsPerKg));
                        break;

                    default:
                        this.PowerValues.Add(ActivityViewMetricType.DetailFTP, PowerDisplayEnum.Instance.GetItem(PowerDisplayType.None));
                        break;
                }
                count++;
            }

            if (this.IsVisible == null)
            {
                switch (this.DurationSetting)
                {
                    case DurationType.OneMinute:
                    case DurationType.FiveMinutes:
                    case DurationType.TwentyMinutes:
                        this.IsVisible = true;
                        break;

                    default:
                        this.IsVisible = false;
                        break;
                }
                count++;
            }

            return count;
        }


        /// <summary>
        /// The full KeyValuePair is stored in the item array for display.
        /// </summary>
        [JsonIgnore]
        public DurationType DurationSetting
        {
            get { return Duration.Key; }
            set
            {
                Duration = DurationEnum.Instance.GetItem(value);
            }
        }

        [JsonIgnore]
        public PowerDisplayType AP_PowerDisplaySetting
        {
            get { return this.PowerValues[ActivityViewMetricType.DetailAP].Key; }
            set
            {
                this.PowerValues[ActivityViewMetricType.DetailAP] = PowerDisplayEnum.Instance.GetItem(value);
            }
        }

        [JsonIgnore]
        public PowerDisplayType APmax_PowerDisplaySetting
        {
            get { return this.PowerValues[ActivityViewMetricType.DetailAPmax].Key; }
            set
            {
                this.PowerValues[ActivityViewMetricType.DetailAPmax] = PowerDisplayEnum.Instance.GetItem(value);
            }
        }

        [JsonIgnore]
        public PowerDisplayType FTP_PowerDisplaySetting
        {
            get { return this.PowerValues[ActivityViewMetricType.DetailFTP].Key; }
            set
            {
                this.PowerValues[ActivityViewMetricType.DetailFTP] = PowerDisplayEnum.Instance.GetItem(value);
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    #endregion

    #region UserActivityViewSummaryRowSettings class
    /// <summary>
    /// Settings for the ActivityViewerControl's Summary Power and Speed columns
    /// </summary>
    public class UserActivityViewSummaryRowSettings : ConfigItemBase, ICloneable
    {
        public SortedList<ActivityViewMetricType, KeyStringPair<PowerDisplayType>> PowerValues = new();
        public SortedList<ActivityViewMetricType, KeyStringPair<SpeedDisplayType>> SpeedValues = new();

        public override int InitializeDefaultValues()
        {
            int count = 0;

            // The KeyStringPair classes need to be initialized with defaults here as they depend on values in the lists.
            // FYI: They can't be initialized in the constructor as they will always show null during json deserialization,
            // even if the json being parsed has values in it.

            if (!this.PowerValues.ContainsKey(ActivityViewMetricType.SummaryAP))
            {
                this.PowerValues.Add(ActivityViewMetricType.SummaryAP, PowerDisplayEnum.Instance.GetItem(PowerDisplayType.Watts));
                count++;
            }

            if (!this.PowerValues.ContainsKey(ActivityViewMetricType.SummaryNP))
            {
                this.PowerValues.Add(ActivityViewMetricType.SummaryNP, PowerDisplayEnum.Instance.GetItem(PowerDisplayType.Both));
                count++;
            }

            if (!this.SpeedValues.ContainsKey(ActivityViewMetricType.SummaryAS))
            {
                this.SpeedValues.Add(ActivityViewMetricType.SummaryAS, SpeedDisplayEnum.Instance.GetItem(SpeedDisplayType.Both));
                count++;
            }

            return count;
        }

        [JsonIgnore]
        public PowerDisplayType AP_PowerDisplaySetting
        {
            get { return this.PowerValues[ActivityViewMetricType.SummaryAP].Key; }
            set
            {
                this.PowerValues[ActivityViewMetricType.SummaryAP] = PowerDisplayEnum.Instance.GetItem(value);
            }
        }

        [JsonIgnore]
        public PowerDisplayType NP_PowerDisplaySetting
        {
            get { return this.PowerValues[ActivityViewMetricType.SummaryNP].Key; }
            set
            {
                this.PowerValues[ActivityViewMetricType.SummaryNP] = PowerDisplayEnum.Instance.GetItem(value);
            }
        }

        [JsonIgnore]
        public SpeedDisplayType AS_SpeedDisplaySetting
        {
            get { return this.SpeedValues[ActivityViewMetricType.SummaryAS].Key; }
            set
            {
                this.SpeedValues[ActivityViewMetricType.SummaryAS] = SpeedDisplayEnum.Instance.GetItem(value);
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    #endregion

    #region UserSplitViewColumnSettings class
    /// <summary>
    /// Settings for the SplitViewerControl's Detail and Summary column visibility
    /// </summary>
    public class UserSplitViewColumnSettings : ConfigItemBase, ICloneable
    {
        public SortedList<SplitViewMetricType, bool> Visibility = new();
        public SortedList<SplitViewMetricType, KeyStringPair<SpeedDisplayType>> SpeedValues = new();
        public SortedList<SplitViewMetricType, KeyStringPair<DistanceDisplayType>> DistanceValues = new();

        [JsonConstructor]
        public UserSplitViewColumnSettings()
        {
        }

        public override int InitializeDefaultValues()
        {
            int count = 0;

            // The KeyStringPair classes need to be initialized with defaults here as they depend on values in the lists.
            // FYI: They can't be initialized in the constructor as they will always show null during json deserialization,
            // even if the json being parsed has values in it.

            // default all columns to visible
            foreach (var item in SplitViewMetricEnum.Instance.GetItems())
            {
                if (!this.Visibility.ContainsKey(item.Key))
                {
                    this.Visibility.Add(item.Key, true);
                    count++;
                }
            }

            if (!this.SpeedValues.ContainsKey(SplitViewMetricType.DetailSplitSpeed))
            {
                this.SpeedValues.Add(SplitViewMetricType.DetailSplitSpeed, SpeedDisplayEnum.Instance.GetItem(SpeedDisplayType.Both));
                count++;
            }

            if (!this.DistanceValues.ContainsKey(SplitViewMetricType.DetailSplitDistance))
            {
                this.DistanceValues.Add(SplitViewMetricType.DetailSplitDistance, DistanceDisplayEnum.Instance.GetItem(DistanceDisplayType.Both));
                count++;
            }

            if (!this.SpeedValues.ContainsKey(SplitViewMetricType.SummaryGoalSpeed))
            {
                this.SpeedValues.Add(SplitViewMetricType.SummaryGoalSpeed, SpeedDisplayEnum.Instance.GetItem(SpeedDisplayType.Both));
                count++;
            }

            if (!this.DistanceValues.ContainsKey(SplitViewMetricType.SummaryGoalDistance))
            {
                this.DistanceValues.Add(SplitViewMetricType.SummaryGoalDistance, DistanceDisplayEnum.Instance.GetItem(DistanceDisplayType.Both));
                count++;
            }

            return count;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    #endregion

    #region UserLapViewColumnSettings class
    /// <summary>
    /// Settings for the LapViewerControl's Detail and Summary column visibility
    /// </summary>
    public class UserLapViewColumnSettings : ConfigItemBase, ICloneable
    {
        public SortedList<LapViewMetricType, bool> Visibility = new();
        public SortedList<LapViewMetricType, KeyStringPair<SpeedDisplayType>> SpeedValues = new();
        public SortedList<LapViewMetricType, KeyStringPair<DistanceDisplayType>> DistanceValues = new();
        public SortedList<LapViewMetricType, KeyStringPair<PowerDisplayType>> PowerValues = new();

        [JsonConstructor]
        public UserLapViewColumnSettings()
        {
        }

        public override int InitializeDefaultValues()
        {
            int count = 0;

            // The KeyStringPair classes need to be initialized with defaults here as they depend on values in the lists.
            // FYI: They can't be initialized in the constructor as they will always show null during json deserialization,
            // even if the json being parsed has values in it.

            // default all columns to visible
            foreach (var item in LapViewMetricEnum.Instance.GetItems())
            {
                if (!this.Visibility.ContainsKey(item.Key))
                {
                    this.Visibility.Add(item.Key, true);
                    count++;
                }
            }

            if (!this.SpeedValues.ContainsKey(LapViewMetricType.DetailLapSpeed))
            {
                this.SpeedValues.Add(LapViewMetricType.DetailLapSpeed, SpeedDisplayEnum.Instance.GetItem(SpeedDisplayType.Both));
                count++;
            }

            if (!this.DistanceValues.ContainsKey(LapViewMetricType.DetailLapDistance))
            {
                this.DistanceValues.Add(LapViewMetricType.DetailLapDistance, DistanceDisplayEnum.Instance.GetItem(DistanceDisplayType.Both));
                count++;
            }

            if (!this.PowerValues.ContainsKey(LapViewMetricType.DetailLapAP))
            {
                this.PowerValues.Add(LapViewMetricType.DetailLapAP, PowerDisplayEnum.Instance.GetItem(PowerDisplayType.Both));
                count++;
            }

            return count;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
    #endregion

    public class UserProfile : ConfigItemBase, ICloneable
    {
        public string UniqueId { get; set; } = "";
        public int PowerThreshold { get; set; }

        public SortedList<DurationType, UserActivityViewDetailRowSettings> ActivityViewDetailRowSettings { get; set; } = new();
        public SortedList<ActivityViewMetricType, UserActivityViewColumnSettings> ActivityViewColumnSettings { get; set; } = new();

        public UserActivityViewSummaryRowSettings ActivityViewSummaryRowSettings { get; set; } = new();

        public UserSplitViewColumnSettings SplitViewColumnSettings { get; set; } = new();
        
        public UserLapViewColumnSettings LapViewColumnSettings { get; set; } = new();

        public bool AutoPause { get; set; } = true;

        private string m_name = "";
        private string m_email = "";
        private double m_weight;
        private bool m_weightInKgs;

        /// <summary>
        /// Identifies the user profile as the default user.
        /// </summary>
        [JsonIgnore]
        public bool Default { get; set; }


        public UserProfile()
        {
        }

        public override int InitializeDefaultValues()
        {
            int count = 0;

            // The KeyStringPair classes need to be initialized with defaults here as they depend on values in the lists.
            // FYI: They can't be initialized in the constructor as they will always show null during json deserialization,
            // even if the json being parsed has values in it.

            // Initialize configuration for the collector rows if missing
            foreach (var duration in DurationEnum.Instance.GetItems())
            {
                if (!ActivityViewDetailRowSettings.ContainsKey(duration.Key))
                {
                    ActivityViewDetailRowSettings.Add(duration.Key, new UserActivityViewDetailRowSettings(duration.Key));
                    count++;
                }
            }

            foreach (var item in this.ActivityViewDetailRowSettings)
                count += item.Value.InitializeDefaultValues();

            // Initialize configuration for the metric columns if missing
            foreach(var metric in ActivityViewMetricEnum.Instance.GetItems())
            {
                if (!ActivityViewColumnSettings.ContainsKey(metric.Key))
                {
                    ActivityViewColumnSettings.Add(metric.Key, new UserActivityViewColumnSettings(metric.Key));
                    count++;
                }
            }

            foreach (var item in this.ActivityViewColumnSettings)
                count += item.Value.InitializeDefaultValues();

            count += this.ActivityViewSummaryRowSettings.InitializeDefaultValues();

            count += this.SplitViewColumnSettings.InitializeDefaultValues();

            count += this.LapViewColumnSettings.InitializeDefaultValues();

            return count;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public bool WeightInKgs
        {
            get
            {
                return m_weightInKgs;
            }
            set
            {
                m_weightInKgs = value;

                m_weight = (m_weightInKgs ? Math.Round(m_weight, 1) : Math.Round(m_weight, 0));
            }
        }


        public string Name
        {
            get { return new string(m_name.Take(30).ToArray()); }
            set
            {
                if (value.Length < 1 || value.Length > 30)
                    throw new FormatException("Name must be between 1 and 30 characters.");

                m_name = value;
            }
        }
        public string EmailAddress
        {
            get { return new string(m_email.Take(100).ToArray()); }
            set
            {
                if (value.Length < 0 || value.Length > 100)
                    throw new FormatException("Email address must be between 0 and 100 characters.");

                if (value.Length > 0)
                    new System.Net.Mail.MailAddress(value); // this will throw exception if in invalid format

                m_email = value;
            }
        }

        public double Weight
        {
            get
            {
                this.WeightInKgs = m_weightInKgs; // fixup weight if necessary

                return m_weight;
            }

            set
            {
                if (value < 40 || value > 999)
                    throw new FormatException("Weight value must be between 40 and 999.");

                m_weight = value;
            }
        }

        /// <summary>
        /// Weight is needed in kgs for watts/kg calculations
        /// </summary>
        /// <returns></returns>
        [JsonIgnore]
        public double WeightAsKgs
        {
            get
            {
                return (this.m_weightInKgs ? this.m_weight : this.m_weight / 2.205);
            }
        }

        public override string ToString()
        {
            return $"{this.Name}";
        }

    }
}
