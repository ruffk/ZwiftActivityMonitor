using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using SharpPcap.Npcap;

namespace ZwiftActivityMonitor
{

    //public enum WeightUomType
    //{
    //    lbs,
    //    kgs
    //}

    public class UserProfile : ICloneable
    {
        public string UniqueId { get; set; } = "";
        private string m_name = "";
        private decimal m_weight;
        public bool WeightInKgs { get; set; }
        public int PowerThreshold { get; set; }
        public SortedList<string, bool> DefaultCollectors { get; } = new SortedList<string, bool>();

        [JsonIgnore]
        public bool Default { get; set; }

        public UserProfile()
        {
        }

        public object Clone()
        {
            return this.MemberwiseClone();
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
        public decimal Weight
        {
            get
            {
                switch (this.WeightInKgs)
                {
                    case true:
                        return Math.Round(m_weight, 1);

                    default:
                        return Math.Round(m_weight, 0);
                }
            }

            set
            {
                if (value < 40 || value > 999)
                    throw new FormatException("Weight value must be between 40 and 999.");

                m_weight = value;
            }
        }

        public override string ToString()
        {
            return $"{this.Name}";
        }

    }

    public class Collector : ICloneable
    {
        public string Name { get; set; }
        public string DurationDesc { get; set; }
        public int DurationSecs { get; set; }
        public string FieldAvgDesc { get; set; }
        public string FieldAvgMaxDesc { get; set; }
        public string FieldFtpDesc { get; set; }

        public Collector()
        {
        }

        [JsonIgnore]
        public DurationType DurationType { get { return Enum.Parse<DurationType>(this.DurationDesc); } }

        [JsonIgnore]
        public FieldUomType FieldAvgType { get { return Enum.Parse<FieldUomType>(this.FieldAvgDesc); } }

        [JsonIgnore]
        public FieldUomType FieldAvgMaxType { get { return Enum.Parse<FieldUomType>(this.FieldAvgMaxDesc); } }

        [JsonIgnore]
        public FieldUomType FieldFtpType { get { return Enum.Parse<FieldUomType>(this.FieldFtpDesc); } }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{this.Name}";
        }
    }

    public class NetworkListItem
    {
        private string m_network;
        private string m_ip_address;

        public NetworkListItem(string network, string ip_address)
        {
            m_network = network;
            m_ip_address = ip_address;
        }

        public string Network { get { return m_network; } }

        public override string ToString()
        {
            return $"{m_network} ({m_ip_address})";
        }
    }

    public class ZAMsettings
    {
        public string Network { get; set; }
        public bool AutoStart { get; set; }
        public string DefaultUserProfile { get; set; } = "";
        public int WindowPositionX { get; set; }
        public int WindowPositionY { get; set; }

        public SortedList<string, UserProfile> UserProfiles { get; }
        public SortedList<string, Collector> Collectors { get; }

        [JsonIgnore]
        public string CurrentUserProfile { get; set; } = "";

        private bool m_readOnly; // Is the current configuration mutable


        private static ZAMsettings _cleanZAMsettings;
        private static string _cleanJsonStr;
        private static ZAMsettings _dirtyZAMsettings;

        private static ILogger<ZAMsettings> _logger;
        private static bool _initialized;


        private ZAMsettings()
        {
            UserProfiles = new SortedList<string, UserProfile>();
            Collectors = new SortedList<string, Collector>();
        }

        public void UpsertUserProfile(UserProfile user)
        {
            Debug.Assert(!this.m_readOnly, "Configuration in use is read-only.  Did you forget BeginCachedConfiguration?");

            if (user.UniqueId.Length == 0)
            {
                user.UniqueId = Guid.NewGuid().ToString();

                // Clone the user and add to the configuration's UserProfile dictionary
                UserProfiles.Add(user.UniqueId, (UserProfile)user.Clone());

                _logger.LogInformation($"User {user.Name} added.");
            }
            else
            {
                Debug.Assert(UserProfiles.ContainsKey(user.UniqueId), "User profile not found in dictionary.  Cannot update.");

                // Clone the user and update the configuration's UserProfile dictionary
                UserProfiles[user.UniqueId] = (UserProfile)user.Clone();

                _logger.LogInformation($"User {user.Name} updated.");
            }

            // The Default property is included on the profile just as a helper (it's not saved in the json).
            // What is saved is the UniqueId of the default user in the DefaultUserProfile field.
            if (user.Default)
                DefaultUserProfile = user.UniqueId;
            else if (DefaultUserProfile == user.UniqueId)
                DefaultUserProfile = "";
        }
        public void UpsertCollector(Collector collector)
        {
            Debug.Assert(!this.m_readOnly, "Configuration in use is read-only.  Did you forget BeginCachedConfiguration?");

            if (!Collectors.ContainsKey(collector.Name))
            {
                // Clone the collector and add to the configuration's Collector dictionary
                Collectors.Add(collector.Name, (Collector)collector.Clone());

                _logger.LogInformation($"Collector {collector.Name} added.");
            }
            else
            {
                Debug.Assert(Collectors.ContainsKey(collector.Name), "Collector not found in dictionary.  Cannot update.");

                // Clone the user and update the configuration's UserProfile dictionary
                Collectors[collector.Name] = (Collector)collector.Clone();

                _logger.LogInformation($"Collector {collector.Name} updated.");
            }
        }

        [JsonIgnore]
        public List<UserProfile> GetUsers
        {
            get
            {
                if (UserProfiles.ContainsKey(DefaultUserProfile))
                {
                    UserProfiles[DefaultUserProfile].Default = true;
                }

                List<UserProfile> users = UserProfiles.Values.ToList<UserProfile>();
                users.Sort(
                    delegate (UserProfile p1, UserProfile p2)
                    {
                        return p1.Name.CompareTo(p2.Name);
                    }
                );

                return users.ToList<UserProfile>();
            }
        }

        [JsonIgnore]
        public List<Collector> GetCollectors
        {
            get
            {
                List<Collector> collectors = Collectors.Values.ToList<Collector>();
                collectors.Sort(
                    delegate (Collector p1, Collector p2)
                    {
                        return p1.DurationSecs.CompareTo(p2.DurationSecs);
                    }
                );

                return collectors.ToList<Collector>();
            }
        }


        static ZAMsettings()
        {

        }

        public static ZAMsettings Settings 
        { 
            get 
            { 
                return (_dirtyZAMsettings != null ? _dirtyZAMsettings : _cleanZAMsettings); 
            } 
        }


        public static void Initialize(ILoggerFactory loggerFactory)
        {
            if (_initialized)
                return;

            _logger = loggerFactory.CreateLogger<ZAMsettings>();

            string fileNameDefault = "ZAMsettings.Default.json";
            string fileName = "ZAMsettings.Development.json";

            try
            {
                string defaultJsonStr = File.ReadAllText(fileNameDefault);
                JObject defaultJson = JObject.Parse(defaultJsonStr);

                try
                {
                    string userJsonStr = File.ReadAllText(fileName);
                    JObject userJson = JObject.Parse(userJsonStr);

                    defaultJson.Merge(userJson, new JsonMergeSettings
                    {
                        // union array values together to avoid duplicates
                        MergeArrayHandling = MergeArrayHandling.Union
                    });

                    _logger.LogInformation($"Configuration cached from default settings file {fileNameDefault} and merged with user settings file {fileName}.");
                }
                catch (FileNotFoundException)
                {
                    // this is okay as defaults will be used
                    _logger.LogInformation($"Configuration cached from default settings file {fileNameDefault}.  User settings file {fileName} not found.");
                }


                _cleanJsonStr = defaultJson.ToString();

                //_cleanJsonStr = File.ReadAllText(fileName);

                _cleanZAMsettings = JsonConvert.DeserializeObject<ZAMsettings>(_cleanJsonStr);

                _cleanZAMsettings.m_readOnly = true;

                // Set current user according to default selection.  This value is not persisted in json file.
                _cleanZAMsettings.CurrentUserProfile = _cleanZAMsettings.DefaultUserProfile;

                _initialized = true;

            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Exception occurred trying to load configuration from file: {fileName}", ex);
            }

        }

        public static List<NetworkListItem> Networks
        {
            get
            {
                List<NetworkListItem> list = new List<NetworkListItem>();

                foreach (var device in NpcapDeviceList.Instance)
                {
                    _logger.LogInformation($"{device.Interface.FriendlyName}");
                    foreach (var a in device.Interface.Addresses)
                    {
                        if (a.Addr.ipAddress != null && a.Addr.ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            _logger.LogInformation($"{a.Addr.ipAddress}");
                            list.Add(new NetworkListItem(device.Interface.FriendlyName, a.Addr.ipAddress.ToString()));
                            break; // only use one IP
                        }
                    }
                }

                return list;
            }
        }


        public static void BeginCachedConfiguration()
        {
            Debug.Assert(_initialized, "Not initialized.");
            Debug.Assert(_dirtyZAMsettings == null, "Configuration already in a cached state.  It must be rolled back or committed before calling BeginCachedConfiguration.");

            try
            {
                _logger.LogInformation($"In BeginCachedConfiguration:\n{_cleanJsonStr}");


                _dirtyZAMsettings = JsonConvert.DeserializeObject<ZAMsettings>(_cleanJsonStr);

                _dirtyZAMsettings.m_readOnly = false;

                // Because this value is not persisted in json file, set current user manually.
                _dirtyZAMsettings.CurrentUserProfile = _cleanZAMsettings.CurrentUserProfile;


                _logger.LogInformation($"Configuration cached.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Exception occurred trying to load configuration from JSON string.", ex);
            }
        }

        public static void CommitCachedConfiguration()
        {
            string fileName = "ZAMsettings.Development.json";

            Debug.Assert(_initialized, "Not initialized.");
            Debug.Assert(_dirtyZAMsettings != null, "Configuration not in a cached state.  It must be cached first using BeginCachedConfiguration.");

            try
            {
                string json = JsonConvert.SerializeObject(_dirtyZAMsettings, Formatting.Indented);

                File.WriteAllText(fileName, json);

                _logger.LogInformation($"In CommitCachedConfiguration:\n{json}");

                _cleanZAMsettings = _dirtyZAMsettings;
                _cleanJsonStr = json;
                _cleanZAMsettings.m_readOnly = true;

                _dirtyZAMsettings = null;

                _logger.LogInformation($"Cached configuration saved to file: {fileName}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Exception occurred trying to save cached configuration to file: {fileName}", ex);
            }
        }
        public static void RollbackCachedConfiguration()
        {
            Debug.Assert(_initialized, "Not initialized.");
            Debug.Assert(_dirtyZAMsettings != null, "Configuration not in a cached state.  It must be cached first using RollbackCachedConfiguration.");

            _dirtyZAMsettings = null;
            //_configUserProfileIndex = -1;

            _logger.LogInformation($"Cached configuration rolled back.");
        }


        public static void Test()
        {
            BeginCachedConfiguration();

            //Settings.Network = "Ethernet";
            //Settings.AutoStart = true;

            //UserProfile user = new UserProfile();
            //user.Name = "Kevin";
            //user.Weight = 147;
            //user.WeightInKgs = false;
            //user.PowerThreshold = 283;
            //Insert(user, true);

            Collector c = new Collector();
            c.Name = "5 sec";
            c.DurationDesc = Enum.GetName<DurationType>(DurationType.FiveSeconds);
            c.FieldAvgDesc = Enum.GetName<FieldUomType>(FieldUomType.Watts);
            c.FieldAvgMaxDesc = Enum.GetName<FieldUomType>(FieldUomType.Wkg);
            c.FieldFtpDesc = Enum.GetName<FieldUomType>(FieldUomType.Hidden);
            Settings.UpsertCollector(c);

            CommitCachedConfiguration();


            //string json = JsonConvert.SerializeObject(_ZAMsettings, Formatting.Indented);
            //logger.LogInformation($"{json.ToString()}");

            //ZAMsettings s = JsonConvert.DeserializeObject<ZAMsettings>(json);
            //logger.LogInformation($"{s.Network}");
            //logger.LogInformation($"{s.AutoStart}");
            //logger.LogInformation($"{s.DefaultUserProfile}");

            ////KeyValuePair<Guid, UserProfile> u = s.UserProfiles.First();

            //logger.LogInformation($"{s.UserProfiles.Values[0].UniqueId.ToString()}");
            //logger.LogInformation($"{s.UserProfiles.Values[0].Name}");
            ////logger.LogInformation($"{s.UserProfiles.Values[0].Default}");
            //logger.LogInformation($"{s.UserProfiles.Values[0].Weight.ToString()}");
            //logger.LogInformation($"{s.UserProfiles.Values[0].WeightInKgs}");
            //logger.LogInformation($"{s.UserProfiles.Values[0].PowerThreshold.ToString()}");
        }
    }
}
