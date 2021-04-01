using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpPcap.Npcap;
using System.Runtime.InteropServices;

namespace ZwiftActivityMonitor
{
    public enum FieldUomType
    {
        Hidden,
        Watts,
        Wkg
    }


    #region UserProfile

    public class UserProfile : ICloneable
    {
        public string UniqueId { get; set; } = "";
        public int PowerThreshold { get; set; }
        public SortedList<string, bool> DefaultCollectors { get; } = new SortedList<string, bool>();

        private string m_name = "";
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

    #endregion

    #region Collector

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

    #endregion

    #region NetworkListItem

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

    #endregion

    public class Splits : ICloneable
    {
        public bool ShowSplits { get; set; }
        public bool CalculateGoal { get; set; }

        private int m_splitDistance = 5;
        private string m_splitUom = "km";
        private int m_goalHours;
        private int m_goalMinutes = 45;
        private int m_goalSeconds;

        public Splits()
        {

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
        public string SplitUom 
        {
            get { return m_splitUom; }
            set 
            { 
                if (value != "km" && value != "mi")
                    throw new FormatException("Distance UOM must be either km or mi.");

                m_splitUom = value;
            }
        }
        public int GoalHours
        {
            get { return m_goalHours; }

            set
            {
                if (value < 0 || value > 23)
                    throw new FormatException("Goal hours value must be between 0 and 23.");

                m_goalHours = value;
            }
        }
        public int GoalMinutes
        {
            get { return m_goalMinutes; }

            set
            {
                if (value < 0 || value > 59)
                    throw new FormatException("Goal minutes value must be between 0 and 59.");

                m_goalMinutes = value;
            }
        }
        public int GoalSeconds
        {
            get { return m_goalSeconds; }

            set
            {
                if (value < 0 || value > 59)
                    throw new FormatException("Goal seconds value must be between 0 and 59.");

                m_goalSeconds = value;
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }

    public class ZAMsettings
    {
        #region Public members included in .json configuration

        //public string Version { get; set; } = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        public string Network { get; set; }
        public bool AutoStart { get; set; }
        public string DefaultUserProfile { get; set; } = "";
        public int WindowPositionX { get; set; }
        public int WindowPositionY { get; set; }

        public SortedList<string, UserProfile> UserProfiles { get; }
        public SortedList<string, Collector> Collectors { get; }
        public Splits Splits { get; }

        #endregion

        [JsonIgnore]
        public string CurrentUserProfile { get; set; } = "";

        private bool m_readOnly; // Is the current configuration mutable


        private static string       _committedJsonStr;      // The clean (disk hardened) version of the .json settings
        private static ZAMsettings  _committedZAMsettings;  // The deserialized settings matching _committedJsonStr 
        private static ZAMsettings  _uncommittedZAMsettings;  // While editing, contains the dirty settings

        private static ILogger<ZAMsettings> _logger;
        public static ILoggerFactory LoggerFactory { get; set; }
        public static ZPMonitorService ZPMonitorService { get; set; }

        private static bool _initialized;


        private const string FileNameDefault = "ZAMsettings.Default.json";
        private const string FileName = "ZAMsettings.json";


        private ZAMsettings()
        {
            UserProfiles = new SortedList<string, UserProfile>();
            Collectors = new SortedList<string, Collector>();
            Splits = new Splits();
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

        public void DeleteUserProfile(UserProfile user)
        {
            Debug.Assert(!this.m_readOnly, "Configuration in use is read-only.  Did you forget BeginCachedConfiguration?");

            Debug.Assert(UserProfiles.ContainsKey(user.UniqueId), "User profile not found in dictionary.  Cannot delete.");

            UserProfiles.Remove(user.UniqueId);

            _logger.LogInformation($"User {user.Name} deleted.");
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

        /// <summary>
        /// Returns the user that has been selected as the current user
        /// </summary>
        [JsonIgnore]
        public UserProfile CurrentUser
        {
            get
            {
                if (UserProfiles.ContainsKey(CurrentUserProfile))
                    return UserProfiles[CurrentUserProfile];

                return null;
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
                Debug.Assert(_initialized, "Not initialized.");
                return (_uncommittedZAMsettings != null ? _uncommittedZAMsettings : _committedZAMsettings); 
            } 
        }


        public static void Initialize(ILoggerFactory loggerFactory, ZPMonitorService zpMonitorService)
        {
            if (_initialized)
                return;

            LoggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<ZAMsettings>();

            ZPMonitorService = zpMonitorService;

            JObject parsedJson = null;

            try
            {
                try
                {
                    // Try to load user .json file settings
                    string jsonStr = File.ReadAllText(FileName);
                    parsedJson = JObject.Parse(jsonStr);

                    _logger.LogInformation($"Configuration cached from user settings file {FileName}.");
                }
                catch (FileNotFoundException)
                {
                    // User .json file not found.  Try to load default .json file settings
                    string jsonStr = File.ReadAllText(FileNameDefault);
                    parsedJson = JObject.Parse(jsonStr);

                    _logger.LogInformation($"Configuration cached from default settings file {FileNameDefault}.  User settings file {FileName} not found.");
                }

                // Configuration has been loaded and .json is good.  Now deserialize into the settings objects.
                _committedJsonStr = parsedJson.ToString(); 

                _committedZAMsettings = JsonConvert.DeserializeObject<ZAMsettings>(_committedJsonStr); // this could throw if settings don't match the .json

                _committedZAMsettings.m_readOnly = true;

                // Set current user according to default selection.  This value is not persisted in json file.
                _committedZAMsettings.CurrentUserProfile = _committedZAMsettings.DefaultUserProfile;

                _initialized = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Exception occurred while trying to load configuration.", ex);
            }


            // This version would first read the default json settings file, and merge the user json settings file into it.
            // Problem is things like UserProfiles would suddenly have Collectors from the default showing up as selected by the user.
            //try
            //{
            //    string defaultJsonStr = File.ReadAllText(FileNameDefault);
            //    JObject defaultJson = JObject.Parse(defaultJsonStr);

            //    try
            //    {
            //        string userJsonStr = File.ReadAllText(FileName);
            //        JObject userJson = JObject.Parse(userJsonStr);

            //        defaultJson.Merge(userJson, new JsonMergeSettings
            //        {
            //            // union array values together to avoid duplicates
            //            MergeArrayHandling = MergeArrayHandling.Union
            //        });

            //        _logger.LogInformation($"Configuration cached from default settings file {FileNameDefault} and merged with user settings file {FileName}.");
            //    }
            //    catch (FileNotFoundException)
            //    {
            //        // this is okay as defaults will be used
            //        _logger.LogInformation($"Configuration cached from default settings file {FileNameDefault}.  User settings file {FileName} not found.");
            //    }

            //    _committedJsonStr = defaultJson.ToString();

            //    _committedZAMsettings = JsonConvert.DeserializeObject<ZAMsettings>(_committedJsonStr);

            //    _committedZAMsettings.m_readOnly = true;

            //    // Set current user according to default selection.  This value is not persisted in json file.
            //    _committedZAMsettings.CurrentUserProfile = _committedZAMsettings.DefaultUserProfile;

            //    _initialized = true;

            //}
            //catch (Exception ex)
            //{
            //    throw new ApplicationException($"Exception occurred trying to load configuration from file: {FileName}", ex);
            //}

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
            Debug.Assert(_uncommittedZAMsettings == null, "Configuration already in a cached state.  It must be rolled back or committed before calling BeginCachedConfiguration.");

            try
            {
                //_logger.LogInformation($"In BeginCachedConfiguration:\n{_committedJsonStr}");


                _uncommittedZAMsettings = JsonConvert.DeserializeObject<ZAMsettings>(_committedJsonStr);

                _uncommittedZAMsettings.m_readOnly = false;

                // Because this value is not persisted in json file, set current user manually.
                _uncommittedZAMsettings.CurrentUserProfile = _committedZAMsettings.CurrentUserProfile;


                _logger.LogInformation($"Configuration cached.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Exception occurred trying to load configuration from JSON string.", ex);
            }
        }

        public static void CommitCachedConfiguration()
        {
            Debug.Assert(_initialized, "Not initialized.");
            Debug.Assert(_uncommittedZAMsettings != null, "Configuration not in a cached state.  It must be cached first using BeginCachedConfiguration.");

            try
            {
                string json = JsonConvert.SerializeObject(_uncommittedZAMsettings, Formatting.Indented);

                File.WriteAllText(FileName, json);

                //_logger.LogInformation($"In CommitCachedConfiguration:\n{json}");

                _committedZAMsettings = _uncommittedZAMsettings;
                _committedJsonStr = json;
                _committedZAMsettings.m_readOnly = true;

                _uncommittedZAMsettings = null;

                _logger.LogInformation($"Cached configuration saved to file: {FileName}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Exception occurred trying to save cached configuration to file: {FileName}", ex);
            }
        }
        public static void RollbackCachedConfiguration()
        {
            Debug.Assert(_initialized, "Not initialized.");
            Debug.Assert(_uncommittedZAMsettings != null, "Configuration not in a cached state.  It must be cached first using RollbackCachedConfiguration.");

            _uncommittedZAMsettings = null;

            _logger.LogInformation($"Cached configuration rolled back.");
        }

        /// <summary>
        /// Region method to define rounded control corners
        /// </summary>
        /// <param name="nLeftRect"></param>
        /// <param name="nTopRect"></param>
        /// <param name="nRightRect"></param>
        /// <param name="nBottomRect"></param>
        /// <param name="nWidthEllipse"></param>
        /// <param name="nHeightEllipse"></param>
        /// <returns></returns>
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        /// <summary>
        /// Method for testing configuration
        /// </summary>
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

            //Collector c = new Collector();
            //c.Name = "5 sec";
            //c.DurationDesc = Enum.GetName<DurationType>(DurationType.FiveSeconds);
            //c.FieldAvgDesc = Enum.GetName<FieldUomType>(FieldUomType.Watts);
            //c.FieldAvgMaxDesc = Enum.GetName<FieldUomType>(FieldUomType.Wkg);
            //c.FieldFtpDesc = Enum.GetName<FieldUomType>(FieldUomType.Hidden);
            //Settings.UpsertCollector(c);

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
