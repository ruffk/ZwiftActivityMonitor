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

    public enum MeasurementSystemType 
    { 
        Imperial, 
        Metric 
    }


    #region ConfigItemBase
    public class ConfigItemBase
    {
        public ConfigItemBase()
        {

        }

        /// <summary>
        /// This method is called during ZAMsettings initialization.
        /// It is only called once and allows a Config class to perform more advanced initialization, like creating internal objects, etc.
        /// 
        /// Returns true if any initialization occurred.
        /// </summary>
        public virtual bool InitializeDefaultValues()
        {
            return false;
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

    #region UserProfile

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

    #endregion

    #region Collector

    public class Collector : ConfigItemBase, ICloneable
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
        
    #region Laps
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
            m_uomItemList.Add(DistanceUomType.Kilometers, new (DistanceUomType.Kilometers, "km"));
            m_uomItemList.Add(DistanceUomType.Miles, new (DistanceUomType.Miles, "mi"));

            // ComboBox will display these items
            m_positionItemList.Add(TriggerPositionType.StartAndLapButton, new (TriggerPositionType.StartAndLapButton, "Start and Lap Button"));
            m_positionItemList.Add(TriggerPositionType.LapButtonOnly, new (TriggerPositionType.LapButtonOnly, "Lap Button Only"));

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

        public override bool InitializeDefaultValues()
        {
            bool isInitialized = false;

            // The KeyStringPair classes need to be initialized with defaults here as they depend on values in the lists.
            // FYI: They can't be initialized in the constructor as they will always show null during json deserialization,
            // even if the json being parsed has values in it.

            if (TriggerDistanceUom == null)
            {
                Logger.LogInformation($"Initializing TriggerDistanceUom");
                TriggerDistanceUom = m_uomItemList[DistanceUomType.Kilometers]; // default
                isInitialized = true;
            }

            if (TriggerPosition == null)
            {
                Logger.LogInformation($"Initializing TriggerPosition");
                TriggerPosition = m_positionItemList[TriggerPositionType.StartAndLapButton]; // default
                isInitialized = true;
            }

            if (MeasurementSystem == null)
            {
                Logger.LogInformation($"Initializing MeasurementSystem");
                MeasurementSystem = m_measurementSystemList[MeasurementSystemType.Metric]; // default
                isInitialized = true;
            }

            if (LapStyle == null)
            {
                Logger.LogInformation($"Initializing LapStyle");
                LapStyle = m_lapStyleList[LapStyleType.Automatic]; // default
                isInitialized = true;
            }

            if (LapTrigger == null)
            {
                Logger.LogInformation($"Initializing LapTrigger");
                LapTrigger = m_lapTriggerList[LapTriggerType.Position]; // default
                isInitialized = true;
            }

            return isInitialized;
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


    #endregion

    #region Splits

    public class Splits : ConfigItemBase, ICloneable
    {
        public bool ShowSplits { get; set; }
        public bool CalculateGoal { get; set; }

        private int m_splitDistance = 5;
        private string m_splitUom = "km";
        private int m_goalHours;
        private int m_goalMinutes = 45;
        private int m_goalSeconds;
        private double m_goalDistance = 25;

        [JsonIgnore]
        public TimeSpan GoalTime { get { return new TimeSpan(m_goalHours, m_goalMinutes, m_goalSeconds); } }

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

        [JsonIgnore]
        public bool SplitsInKm
        {
            get { return m_splitUom == "km"; }
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

        public double GoalDistance
        {
            get { return m_goalDistance; }

            set
            {
                if (value < 1 || value > 999)
                    throw new FormatException("Goal distance value must be between 1 and 999.");

                m_goalDistance = value;
            }
        }


        public object Clone()
        {
            return this.MemberwiseClone();
        }

    }

    #endregion

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
        public Lap Laps { get; }

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


        private ZAMsettings()
        {
            UserProfiles    = new SortedList<string, UserProfile>();
            Collectors      = new SortedList<string, Collector>();
            Splits          = new Splits();
            Laps            = new Lap();
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
                
                bool isInitialized = Settings.Laps.InitializeDefaultValues();
                isInitialized = isInitialized || Settings.Splits.InitializeDefaultValues();

                // If a Collector or UserProfile needs to be added manually for some reason, that will need to be coded separately
                foreach (Collector c in Settings.Collectors.Values)
                    isInitialized = isInitialized || c.InitializeDefaultValues();
                foreach (UserProfile p in Settings.UserProfiles.Values)
                    isInitialized = isInitialized || p.InitializeDefaultValues();
                
                if (isInitialized)
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
