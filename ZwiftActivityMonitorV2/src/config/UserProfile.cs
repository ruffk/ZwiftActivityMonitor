using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;


namespace ZwiftActivityMonitorV2
{
    public class UserCollectorSummary : ConfigItemBase, ICloneable
    {
        public SortedList<CollectorMetricType, KeyStringPair<PowerDisplayType>> PowerValues = new();
        public SortedList<CollectorMetricType, KeyStringPair<SpeedDisplayType>> SpeedValues = new();

        public override int InitializeDefaultValues()
        {
            int count = 0;

            // The KeyStringPair classes need to be initialized with defaults here as they depend on values in the lists.
            // FYI: They can't be initialized in the constructor as they will always show null during json deserialization,
            // even if the json being parsed has values in it.

            if (!this.PowerValues.ContainsKey(CollectorMetricType.SummaryAP))
            {
                this.PowerValues.Add(CollectorMetricType.SummaryAP, PowerDisplayEnum.Instance.GetItem(PowerDisplayType.Watts));
                count++;
            }

            if (!this.PowerValues.ContainsKey(CollectorMetricType.SummaryNP))
            {
                this.PowerValues.Add(CollectorMetricType.SummaryNP, PowerDisplayEnum.Instance.GetItem(PowerDisplayType.Both));
                count++;
            }

            if (!this.SpeedValues.ContainsKey(CollectorMetricType.SummaryAS))
            {
                this.SpeedValues.Add(CollectorMetricType.SummaryAS, SpeedDisplayEnum.Instance.GetItem(SpeedDisplayType.Both));
                count++;
            }

            return count;
        }

        [JsonIgnore]
        public PowerDisplayType AP_PowerDisplaySetting
        {
            get { return this.PowerValues[CollectorMetricType.SummaryAP].Key; }
            set
            {
                this.PowerValues[CollectorMetricType.SummaryAP] = PowerDisplayEnum.Instance.GetItem(value);
            }
        }

        [JsonIgnore]
        public PowerDisplayType NP_PowerDisplaySetting
        {
            get { return this.PowerValues[CollectorMetricType.SummaryNP].Key; }
            set
            {
                this.PowerValues[CollectorMetricType.SummaryNP] = PowerDisplayEnum.Instance.GetItem(value);
            }
        }

        [JsonIgnore]
        public SpeedDisplayType AS_SpeedDisplaySetting
        {
            get { return this.SpeedValues[CollectorMetricType.SummaryAS].Key; }
            set
            {
                this.SpeedValues[CollectorMetricType.SummaryAS] = SpeedDisplayEnum.Instance.GetItem(value);
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }

    public class UserCollector : ConfigItemBase, ICloneable
    {
        // FYI - The setters here should just be "internal set" but then the json deserializer doesn't work properly.
        public KeyStringPair<DurationType> Duration { get; set; }

        public bool? IsVisible { get; set; }

        public SortedList<CollectorMetricType, KeyStringPair<PowerDisplayType>> PowerValues = new();

        [JsonConstructor]
        public UserCollector()
        {

        }
        public UserCollector(DurationType duration)
        {
            this.DurationSetting = duration;
        }

        public override int InitializeDefaultValues()
        {
            int count = 0;

            // The KeyStringPair classes need to be initialized with defaults here as they depend on values in the lists.
            // FYI: They can't be initialized in the constructor as they will always show null during json deserialization,
            // even if the json being parsed has values in it.

            if (!this.PowerValues.ContainsKey(CollectorMetricType.DetailAP))
            {
                this.PowerValues.Add(CollectorMetricType.DetailAP, PowerDisplayEnum.Instance.GetItem(PowerDisplayType.Watts));
                count++;
            }

            if (!this.PowerValues.ContainsKey(CollectorMetricType.DetailAPmax))
            {
                this.PowerValues.Add(CollectorMetricType.DetailAPmax, PowerDisplayEnum.Instance.GetItem(PowerDisplayType.WattsPerKg));
                count++;
            }

            if (!this.PowerValues.ContainsKey(CollectorMetricType.DetailFTP))
            {
                switch (this.DurationSetting)
                {
                    case DurationType.TwentyMinutes:
                        this.PowerValues.Add(CollectorMetricType.DetailFTP, PowerDisplayEnum.Instance.GetItem(PowerDisplayType.WattsPerKg));
                        break;

                    default:
                        this.PowerValues.Add(CollectorMetricType.DetailFTP, PowerDisplayEnum.Instance.GetItem(PowerDisplayType.None));
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
            get { return this.PowerValues[CollectorMetricType.DetailAP].Key; }
            set
            {
                this.PowerValues[CollectorMetricType.DetailAP] = PowerDisplayEnum.Instance.GetItem(value);
            }
        }

        [JsonIgnore]
        public PowerDisplayType APmax_PowerDisplaySetting
        {
            get { return this.PowerValues[CollectorMetricType.DetailAPmax].Key; }
            set
            {
                this.PowerValues[CollectorMetricType.DetailAPmax] = PowerDisplayEnum.Instance.GetItem(value);
            }
        }

        [JsonIgnore]
        public PowerDisplayType FTP_PowerDisplaySetting
        {
            get { return this.PowerValues[CollectorMetricType.DetailFTP].Key; }
            set
            {
                this.PowerValues[CollectorMetricType.DetailFTP] = PowerDisplayEnum.Instance.GetItem(value);
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }

    public class UserCollectorMetric : ConfigItemBase, ICloneable
    {
        public KeyStringPair<CollectorMetricType> Metric { get; set; }

        public bool? IsVisible { get; set; }

        [JsonConstructor]
        public UserCollectorMetric()
        {

        }

        public UserCollectorMetric(CollectorMetricType metric)
        {
            this.MetricSetting = metric;
        }

        /// <summary>
        /// The full KeyValuePair is stored in the item array for display.
        /// </summary>
        [JsonIgnore]
        public CollectorMetricType MetricSetting
        {
            get { return Metric.Key; }
            set
            {
                Metric = CollectorMetricEnum.Instance.GetItem(value);
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
                this.IsVisible = true;
                count++;
            }

            return count;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }

    public class UserProfile : ConfigItemBase, ICloneable
    {
        public string UniqueId { get; set; } = "";
        public int PowerThreshold { get; set; }
        public SortedList<string, bool> DefaultCollectors { get; } = new SortedList<string, bool>();

        public SortedList<DurationType, UserCollector> Collectors = new();
        public SortedList<CollectorMetricType, UserCollectorMetric> CollectorMetrics = new();

        public UserCollectorSummary CollectorSummary = new();

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
                if (!Collectors.ContainsKey(duration.Key))
                {
                    Collectors.Add(duration.Key, new UserCollector(duration.Key));
                    count++;
                }
            }

            foreach (var item in this.Collectors)
                count += item.Value.InitializeDefaultValues();

            // Initialize configuration for the metric columns if missing
            foreach(var metric in CollectorMetricEnum.Instance.GetItems())
            {
                if (!CollectorMetrics.ContainsKey(metric.Key))
                {
                    CollectorMetrics.Add(metric.Key, new UserCollectorMetric(metric.Key));
                    count++;
                }
            }

            foreach (var item in this.CollectorMetrics)
                count += item.Value.InitializeDefaultValues();

            count += this.CollectorSummary.InitializeDefaultValues();

            return count;
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void ClearDefaultCollectors()
        {
            DefaultCollectors.Clear();
        }

        public void AddDefaultCollector(string name)
        {
            DefaultCollectors.Add(name, true);
        }

        [JsonIgnore]
        public SortedList<string, Collector> SelectedCollectors
        {
            get
            {
                SortedList<string, Collector> list = new SortedList<string, Collector>();

                foreach (var item in DefaultCollectors)
                {
                    if (item.Value == true)
                    {
                        if (ZAMsettings.Settings.Collectors.ContainsKey(item.Key))
                            list.Add(item.Key, ZAMsettings.Settings.Collectors[item.Key]);
                    }
                }
                return list;
            }
        }

        [JsonIgnore]
        public List<Collector> GetCollectors
        {
            get
            {
                List<Collector> collectors = new();
                foreach (var item in DefaultCollectors)
                {
                    if (item.Value == true)
                    {
                        if (ZAMsettings.Settings.Collectors.ContainsKey(item.Key))
                            collectors.Add(ZAMsettings.Settings.Collectors[item.Key]);
                    }
                }
                collectors.Sort(
                    delegate (Collector p1, Collector p2)
                    {
                        return p1.DurationSecs.CompareTo(p2.DurationSecs);
                    }
                );

                return collectors.ToList<Collector>();
            }
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
