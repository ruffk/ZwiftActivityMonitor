using System;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpPcap.LibPcap;
using System.Runtime.InteropServices;

namespace ZwiftActivityMonitorV2
{
    #region KeyStringPair
    /// <summary>
    /// This class was developed mainly for use with populating DropDownLists.  It then allows for the key value
    /// to be verified as valid before updating configuration.  I didn't use KeyValuePair struct as I needed to override ToString().
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class KeyStringPair<TKey>
    {
        public TKey Key { get; }
        public string Value { get; }

        public KeyStringPair(TKey key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public override bool Equals(object obj)
        {
            KeyStringPair<TKey> k = obj as KeyStringPair<TKey>;

            if (k != null)
            {
                if (this.Key.Equals(k.Key))
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            return this.Value;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    public class ZAMsettings
    {
        #region Public members included in .json configuration

        public string Network { get; set; }
        public bool AutoStart { get; set; }
        public string DefaultUserProfile { get; set; } = "";
        public int WindowPositionX { get; set; }
        public int WindowPositionY { get; set; }

        public SortedList<string, UserProfile> UserProfiles { get; }
        public SortedList<string, Collector> Collectors { get; }
        //public Splits Splits { get; }
        public Lap Laps { get; }
        public SplitsV2 SplitsV2 { get; }
        public ZAMappearance Appearance { get; }

        #endregion

        [JsonIgnore]
        public string CurrentUserProfile { get; set; } = "";

        private bool m_readOnly; // Is the current configuration mutable


        private static string       _committedJsonStr;      // The clean (disk hardened) version of the .json settings
        private static ZAMsettings  _committedZAMsettings;  // The deserialized settings matching _committedJsonStr 
        private static ZAMsettings  _uncommittedZAMsettings;  // While editing, contains the dirty settings

        private static ILogger<ZAMsettings> _logger;
        private static ILoggerFactory _loggerFactory;
        public static ZPMonitorService ZPMonitorService { get; set; }

        private static bool _initialized;


        private const string FileNameDefault = "ZAMsettings.Default.json";
        private const string FileName = "ZAMsettings.json";

        public static event EventHandler<EventArgs> SystemConfigChanged;
        public static event EventHandler<EventArgs> SplitsConfigChanged;



        private ZAMsettings()
        {
            UserProfiles    = new SortedList<string, UserProfile>();
            Collectors      = new SortedList<string, Collector>();
            Laps            = new Lap();
            SplitsV2        = new SplitsV2();
            Appearance      = new ZAMappearance();
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
        
        /// <summary>
        /// Get a sorted list of all known Collectors by DurationSecs asc
        /// </summary>
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

        public static ILoggerFactory LoggerFactory
        {
            get
            {
                return _loggerFactory;
            }
        }
            



        public static void Initialize(ILoggerFactory loggerFactory, ZPMonitorService zpMonitorService)
        {
            if (_initialized)
                return;

            _loggerFactory = loggerFactory;
            _logger = loggerFactory.CreateLogger<ZAMsettings>();

            ZPMonitorService = zpMonitorService;

            JObject parsedJson = null;
            //bool userFileExists = false;

            try
            {
                try
                {
                    // Try to load user .json file settings
                    string jsonStr = File.ReadAllText(FileName);
                    parsedJson = JObject.Parse(jsonStr);

                    //userFileExists = true;

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

                // Allow any configuration classes to do default initialization (if needed)
                BeginCachedConfiguration();

                int initCount = 0;
                initCount += Settings.Laps.InitializeDefaultValues();
                initCount += Settings.SplitsV2.InitializeDefaultValues();
                initCount += Settings.Appearance.InitializeDefaultValues();

                // If a Collector or UserProfile needs to be added manually for some reason, that will need to be coded separately
                foreach (Collector c in Settings.Collectors.Values)
                    initCount += c.InitializeDefaultValues();
                foreach (UserProfile p in Settings.UserProfiles.Values)
                    initCount += p.InitializeDefaultValues();
                
                if (initCount > 0)
                {
                    // Something changed
                    CommitCachedConfiguration();
                }
                else
                {
                    // Nothing changed
                    RollbackCachedConfiguration();
                }

                //if (userFileExists)
                //{
                //    BeginCachedConfiguration();

                //    if (!Settings.Collectors.ContainsKey("6 min"))
                //    {
                //        Collector c = new Collector()
                //        {
                //            Name = "6 min",
                //            DurationDesc = "SixMinute",
                //            DurationSecs = 360,
                //            FieldAvgDesc = "Watts",
                //            FieldAvgMaxDesc = "Wkg",
                //            FieldFtpDesc = "Hidden"
                //        };
                //        Settings.Collectors.Add("6 min", c);
                //    }

                //    Settings.Laps.InitializeDefaultValues();

                //    CommitCachedConfiguration();
                //}
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Exception occurred while trying to load configuration.", ex);
            }
        }

        public static List<NetworkListItem> Networks
        {
            get
            {
                List<NetworkListItem> list = new List<NetworkListItem>();

                foreach (var device in SharpPcap.LibPcap.LibPcapLiveDeviceList.Instance)
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

        public static void OnSystemConfigChanged(object sender, EventArgs e)
        {
            EventHandler<EventArgs> handler = SystemConfigChanged;
            if (handler != null)
            {
                try
                {
                    handler(sender, e);
                }
                catch
                {
                    // Don't let downstream exceptions bubble up
                }
            }
        }
        public static void OnSplitsConfigChanged(object sender, EventArgs e)
        {
            EventHandler<EventArgs> handler = SplitsConfigChanged;
            if (handler != null)
            {
                try
                {
                    handler(sender, e);
                }
                catch
                {
                    // Don't let downstream exceptions bubble up
                }
            }
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
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="wBar">SB_HORZ = 0, SB_VERT = 1, SB_BOTH = 3</param>
        /// <param name="bShow"></param>
        /// <returns></returns>
        [DllImport("user32", CallingConvention = CallingConvention.Winapi)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ShowScrollBar
        (
            IntPtr hwnd,
            int wBar,
            [MarshalAs(UnmanagedType.Bool)] bool bShow
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
