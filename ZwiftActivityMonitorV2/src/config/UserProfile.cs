using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace ZwiftActivityMonitorV2
{
    public enum PowerDisplayType
    {
        Watts,
        WattsPerKg,
        Both,
        None,
    }
    public enum SpeedDisplayType
    {
        KilometersPerHour,
        MilesPerHour,
        Both,
        None,
    }

    public class EnumManager
    {
        public class DurationTypeEnum
        {
            static private Dictionary<DurationType, string> _List = new();

            static DurationTypeEnum()
            {
                _List.Add(DurationType.FiveSeconds, "5 sec");
                _List.Add(DurationType.ThirtySeconds, "30 sec");
                _List.Add(DurationType.OneMinute, "1 min");
                _List.Add(DurationType.FiveMinutes, "5 min");
                _List.Add(DurationType.SixMinutes, "6 min");
                _List.Add(DurationType.TenMinutes, "10 min");
                _List.Add(DurationType.TwentyMinutes, "20 min");
                _List.Add(DurationType.ThirtyMinutes, "30 min");
                _List.Add(DurationType.SixtyMinutes, "60 min");
                _List.Add(DurationType.NinetyMinutes, "90 min");
            }

            static public List<KeyValuePair<DurationType, string>> ToList()
            {
                List<KeyValuePair<DurationType, string>> list = new();

                foreach (var key in _List.Keys)
                    list.Add(new KeyValuePair<DurationType, string>(key, _List[key]));
                
                return list;
            }

            static public List<string> Values
            {
                get { return _List.Values.ToList<string>(); }
            }

            static public string GetItem(DurationType key)
            {
                return _List[key];
            }
        }

        public class PowerDisplayTypeEnum
        {
            static private Dictionary<PowerDisplayType, string> _List = new();

            static PowerDisplayTypeEnum()
            {
                _List.Add(PowerDisplayType.Watts, "Watts");
                _List.Add(PowerDisplayType.WattsPerKg, "W/Kg");
                _List.Add(PowerDisplayType.Both, "Both Watts and W/Kg");
                _List.Add(PowerDisplayType.None, "None");
            }

            static public List<KeyValuePair<PowerDisplayType, string>> ToList()
            {
                List<KeyValuePair<PowerDisplayType, string>> list = new();

                foreach (var key in _List.Keys)
                    list.Add(new KeyValuePair<PowerDisplayType, string>(key, _List[key]));

                return list;
            }
            static public List<string> Values
            {
                get { return _List.Values.ToList<string>(); }
            }

            static public string GetItem(PowerDisplayType key)
            {
                return _List[key];
            }
        }
        public class SpeedDisplayTypeEnum
        {
            static private Dictionary<SpeedDisplayType, string> _List = new();

            static SpeedDisplayTypeEnum()
            {
                _List.Add(SpeedDisplayType.KilometersPerHour, "km/h");
                _List.Add(SpeedDisplayType.MilesPerHour, "mi/h");
                _List.Add(SpeedDisplayType.Both, "Both km/h and mi/h");
                _List.Add(SpeedDisplayType.None, "None");
            }

            static public List<KeyValuePair<SpeedDisplayType, string>> ToList()
            {
                List<KeyValuePair<SpeedDisplayType, string>> list = new();

                foreach (var key in _List.Keys)
                    list.Add(new KeyValuePair<SpeedDisplayType, string>(key, _List[key]));

                return list;
            }
            static public List<string> Values
            {
                get { return _List.Values.ToList<string>(); }
            }

            static public string GetItem(SpeedDisplayType key)
            {
                return _List[key];
            }
        }
    }

    public class UserCollector
    {
        // FYI - The setters here should just be "internal set" but then the json deserializer doesn't work properly.
        public KeyValuePair<DurationType, string>? Duration { get; set; }
        public KeyValuePair<PowerDisplayType, string>? AP_PowerDisplay { get; set; }
        public KeyValuePair<PowerDisplayType, string>? APmax_PowerDisplay { get; set; }
        public KeyValuePair<PowerDisplayType, string>? FTP_PowerDisplay { get; set; }


    }

    public class UserProfile : ConfigItemBase, ICloneable
    {
        public string UniqueId { get; set; } = "";
        public int PowerThreshold { get; set; }
        public SortedList<string, bool> DefaultCollectors { get; } = new SortedList<string, bool>();

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
