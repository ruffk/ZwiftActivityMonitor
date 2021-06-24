using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;


namespace ZwiftActivityMonitorV2
{
    public class UserCollector
    {
        // FYI - The setters here should just be "internal set" but then the json deserializer doesn't work properly.
        public KeyValuePair<DurationEnum.Keys, string>? Duration { get; set; }
        public KeyValuePair<PowerDisplayEnum.Keys, string>? AP_PowerDisplay { get; set; }
        public KeyValuePair<PowerDisplayEnum.Keys, string>? APmax_PowerDisplay { get; set; }
        public KeyValuePair<PowerDisplayEnum.Keys, string>? FTP_PowerDisplay { get; set; }

        public bool IsVisible { get; set; }

        public UserCollector(DurationEnum.Keys duration, PowerDisplayEnum.Keys ap_PowerDisplay, PowerDisplayEnum.Keys apMax_PowerDisplay, PowerDisplayEnum.Keys ftp_PowerDisplay, bool isVisible)
        {
            this.DurationSetting = duration;
            this.AP_PowerDisplaySetting = ap_PowerDisplay;
            this.APmax_PowerDisplaySetting = apMax_PowerDisplay;
            this.FTP_PowerDisplaySetting = ftp_PowerDisplay;
            this.IsVisible = isVisible;
        }

        /// <summary>
        /// The full KeyValuePair is stored in the item array for display.
        /// </summary>
        [JsonIgnore]
        public DurationEnum.Keys DurationSetting
        {
            get { return Duration.Value.Key; }
            set
            {
                Duration = DurationEnum.Instance.GetItem(value);
            }
        }

        /// <summary>
        /// The full KeyValuePair is stored in the item array for display.
        /// </summary>
        [JsonIgnore]
        public PowerDisplayEnum.Keys AP_PowerDisplaySetting
        {
            get { return AP_PowerDisplay.Value.Key; }
            set
            {
                AP_PowerDisplay = PowerDisplayEnum.Instance.GetItem(value);
            }
        }

        /// <summary>
        /// The full KeyValuePair is stored in the item array for display.
        /// </summary>
        [JsonIgnore]
        public PowerDisplayEnum.Keys APmax_PowerDisplaySetting
        {
            get { return APmax_PowerDisplay.Value.Key; }
            set
            {
                APmax_PowerDisplay = PowerDisplayEnum.Instance.GetItem(value);
            }
        }

        /// <summary>
        /// The full KeyValuePair is stored in the item array for display.
        /// </summary>
        [JsonIgnore]
        public PowerDisplayEnum.Keys FTP_PowerDisplaySetting
        {
            get { return FTP_PowerDisplay.Value.Key; }
            set
            {
                FTP_PowerDisplay = PowerDisplayEnum.Instance.GetItem(value);
            }
        }

    }

    public class UserCollectorMetric //: ConfigItemBase
    {
        public KeyValuePair<CollectorMetricEnum.Keys, string>? Metric { get; set; }

        public bool IsVisible { get; set; }

        /// <summary>
        /// The full KeyValuePair is stored in the item array for display.
        /// </summary>
        //[JsonIgnore]
        //public CollectorMetricEnum.Keys MetricSetting
        //{
        //    get { return Metric.Value.Key; }
        //    set
        //    {
        //        Metric = CollectorMetricEnum.Instance.GetItem(value);
        //    }
        //}

        //public override int InitializeDefaultValues()
        //{
        //    int count = 0;

        //    // The KeyStringPair classes need to be initialized with defaults here as they depend on values in the lists.
        //    // FYI: They can't be initialized in the constructor as they will always show null during json deserialization,
        //    // even if the json being parsed has values in it.

        //    if (Metric == null)
        //    {
        //        Debug.WriteLine($"Initializing Metric");
        //        Metric = CollectorMetricEnum.Instance.GetItem(CollectorMetricEnum.Keys.AP); // default
        //        count++;
        //    }

        //    return count;
        //}

    }

    public class UserProfile : ConfigItemBase, ICloneable
    {
        public string UniqueId { get; set; } = "";
        public int PowerThreshold { get; set; }
        public SortedList<string, bool> DefaultCollectors { get; } = new SortedList<string, bool>();

        public Dictionary<DurationEnum.Keys, UserCollector> Collectors = new();

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

            foreach (var duration in DurationEnum.Instance.GetItems())
            {
                if (!Collectors.ContainsKey(duration.Key))
                {
                    DurationEnum.Instance.GetDefaults(duration.Key, out PowerDisplayEnum.Keys apPowerDisplay, out PowerDisplayEnum.Keys apMaxPowerDisplay, out PowerDisplayEnum.Keys ftpPowerDisplay, out bool isVisible);
                    Collectors.Add(duration.Key, new UserCollector(duration.Key, apPowerDisplay, apMaxPowerDisplay, ftpPowerDisplay, isVisible));
                    count++;
                }
            }
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
