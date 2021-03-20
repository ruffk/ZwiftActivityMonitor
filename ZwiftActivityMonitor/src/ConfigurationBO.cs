using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpPcap;
using SharpPcap.Npcap;

namespace ZwiftActivityMonitor
{
    public class ConfigurationBO_dep
    {
        private readonly ILogger<ConfigurationBO_dep> m_logger;
        private readonly IConfiguration m_configuration;

        private JObject m_configCache;
        private int m_configUserProfileIndex = -1;

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

        public class UserListItem
        {
            private int m_index;
            private string m_name;
            private bool m_default;
            private decimal m_weight;
            private string m_weightUom;
            private int m_powerThreshold;

            private UserListItem m_savedItem;

            public UserListItem(string name)
            {
                m_name = name;
                m_default = false;
                m_weight = 165;
                m_weightUom = "lbs";
                m_powerThreshold = 200;
                m_index = -1;
            }

            public UserListItem(int index, string name, string isDefault, string weight, string weightUom, string powerThreshold)
            {
                m_index = index;
                m_name = name;

                bool.TryParse(isDefault, out m_default);
                if (!decimal.TryParse(weight, out m_weight))
                    m_weight = 150;

                m_weightUom = weightUom.ToLower();

                if (m_weightUom != "kgs" && m_weightUom != "lbs")
                    m_weightUom = "lbs";
                
                if (!int.TryParse(powerThreshold, out m_powerThreshold))
                    m_powerThreshold = 200;
            }

            public void BeginEdit()
            {
                this.m_savedItem = (UserListItem)this.MemberwiseClone();
            }

            public void RollbackEdit()
            {
                if (m_savedItem != null)
                {
                    this.m_index = m_savedItem.m_index;
                    this.m_name = m_savedItem.m_name;
                    this.m_default = m_savedItem.m_default;
                    this.m_weight = m_savedItem.m_weight;
                    this.m_weightUom = m_savedItem.m_weightUom;
                    this.m_powerThreshold = m_savedItem.m_powerThreshold;

                    this.m_savedItem = null;
                }
            }

            public int Index { get { return m_index; } }
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
            public bool Default { get { return m_default; } set { m_default = value; } }
            public decimal Weight 
            { 
                get 
                {
                    switch (m_weightUom)
                    {
                        case "kgs":
                            return Math.Round(m_weight, 1);

                        default:
                            return Math.Round(m_weight, 0);
                    }
                } 

                set 
                {
                    if (value < 20)
                        throw new FormatException("Weight must be >= 20 (lbs/kgs)");

                    m_weight = value; 
                } 
            }
            public string WeightUom { get { return m_weightUom; } set { m_weightUom = value; } }
            public bool WeightIsLbs { get { return m_weightUom == "lbs"; } }
            public bool WeightIsKgs { get { return m_weightUom == "kgs"; } }
            public int PowerThreshold { get { return m_powerThreshold; } set { m_powerThreshold = value; } }
        }


        public ConfigurationBO_dep(ILogger<ConfigurationBO_dep> logger, IConfiguration configuration)
        {
            m_logger = logger;
            m_configuration = configuration;
        }

        public List<NetworkListItem> Networks
        {
            get
            {
                List<NetworkListItem> list = new List<NetworkListItem>();

                foreach (var device in NpcapDeviceList.Instance)
                {
                    m_logger.LogInformation($"{device.Interface.FriendlyName}");
                    foreach (var a in device.Interface.Addresses)
                    {
                        if (a.Addr.ipAddress != null && a.Addr.ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            m_logger.LogInformation($"{a.Addr.ipAddress}");
                            list.Add(new NetworkListItem(device.Interface.FriendlyName, a.Addr.ipAddress.ToString()));
                            break; // only use one IP
                        }
                    }
                }

                return list;
            }
        }

        public List<UserListItem> Users
        {
            get
            {
                List<UserListItem> list = new List<UserListItem>();
                int userIndex = 0;

                foreach (var user in m_configuration.GetSection("UserProfiles:User").GetChildren())
                {
                    list.Add(new UserListItem(userIndex++, user["Name"], user["Default"], user["Weight"], user["WeightUom"], user["PowerThreshold"]));

                    m_logger.LogInformation($"UserProfiles:User Name: {user["Name"]} Default: {user["Default"]} Weight: {user["Weight"]} WeightUom: {user["WeightUom"]} PowerThreshold: {user["PowerThreshold"]}");
                }

                return list;
            }
        }

        public void BeginCachedConfiguration()
        {
            string fileName = AppSettings;

            if (m_configCache != null)
            {
                throw new ApplicationException("Configuration already in a cached state.  It must be rolled back or committed before reloading.");
            }

            try
            {
                var json = new StringReader(File.ReadAllText(fileName));

                using (JsonTextReader reader = new JsonTextReader(json))
                {
                    m_configCache = (JObject)JToken.ReadFrom(reader);
                    m_logger.LogInformation($"Configuration cached from file: {fileName}");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Exception occurred trying to load cached configuration from file: {fileName}", ex);
            }

        }
        public void CommitCachedConfiguration()
        {
            string fileName = "kevin_" + AppSettings; // temporary

            try
            {
                File.WriteAllText(fileName, m_configCache.ToString());

                m_configuration["ZwiftPacketMonitor:Network"] = m_configCache["ZwiftPacketMonitor"]["Network"].ToString();
                m_configuration["ZwiftPacketMonitor:AutoStart"] = m_configCache["ZwiftPacketMonitor"]["AutoStart"].ToString();

                m_configuration["ZwiftActivityMonitor:WindowStartupPosition:X"] = m_configCache["ZwiftActivityMonitor"]["WindowStartupPosition"]["X"].ToString();
                m_configuration["ZwiftActivityMonitor:WindowStartupPosition:Y"] = m_configCache["ZwiftActivityMonitor"]["WindowStartupPosition"]["Y"].ToString();

                if (m_configUserProfileIndex != -1)
                {
                    var c = m_configCache["UserProfiles"]["User"][m_configUserProfileIndex];
                    var users = m_configuration.GetSection("UserProfiles:User").GetChildren().ToArray();

                    users[m_configUserProfileIndex]["Name"] = (string)c["Name"];
                    users[m_configUserProfileIndex]["Default"] = (string)c["Default"];
                    users[m_configUserProfileIndex]["Weight"] = (string)c["Weight"];
                    users[m_configUserProfileIndex]["WeightUom"] = (string)c["WeightUom"];
                    users[m_configUserProfileIndex]["PowerThreshold"] = (string)c["PowerThreshold"];

                    //m_logger.LogInformation($"Testing - User Name0: {users[0]["Name"]} User Name1: {users[1]["Name"]}");
                }
                m_logger.LogInformation($"Cached configuration saved to file: {fileName}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Exception occurred trying to save cached configuration to file: {fileName}", ex);
            }
            finally
            {
                m_configCache = null;
                m_configUserProfileIndex = -1;
            }
        }

        public void RollbackCachedConfiguration()
        {
            m_configCache = null;
            m_configUserProfileIndex = -1;
            m_logger.LogInformation($"Cached configuration rolled back.");
        }

        public string Environment { get { return m_configuration["Environment"]; } }
        public string AppSettings { get { return "appsettings." + Environment + ".json"; } }

        public void UpsertUserProfile(UserListItem item)
        {
            if (item.Index != -1)
            {
                JObject user = (JObject)m_configCache["UserProfiles"]["User"][item.Index];

                user["Name"] = item.Name;
                user["Default"] = item.Default.ToString();
                user["Weight"] = item.Weight.ToString();
                user["WeightUom"] = item.WeightUom;
                user["PowerThreshold"] = item.PowerThreshold.ToString();

                m_configUserProfileIndex = item.Index;
            }
            else
            {
                JArray a = (JArray)m_configCache["UserProfiles"]["User"];
                JObject c = new JObject(new JProperty("Name", "aName"), new JProperty("Default", "false"), new JProperty("Weight", "99"), new JProperty("WeightUom", "lbs"), new JProperty("PowerThreshold", "201"));
                a.Add(c);
            }
        }
        public UserListItem GetUserProfile(int index)
        {
            return this.Users[index];
        }

        public string Network
        {
            get
            {
                return m_configuration["ZwiftPacketMonitor:Network"];
            }

            set 
            {
                m_configCache["ZwiftPacketMonitor"]["Network"] = value;
            }
        }
        public bool AutoStart
        {
            get
            {
                if (bool.TryParse(m_configuration["ZwiftPacketMonitor:AutoStart"], out bool isAutoStart))
                    return isAutoStart;

                return false;
            }

            set
            {
                m_configCache["ZwiftPacketMonitor"]["AutoStart"] = value.ToString();
            }
        }

        public int WindowPositionX
        {
            get
            {
                if (int.TryParse(m_configuration["ZwiftActivityMonitor:WindowStartupPosition:X"], out int xPos))
                    return xPos;

                return 0;
            }

            set
            {
                if (value < 0 || value > 9999)
                    throw new ArgumentOutOfRangeException("WindowPositionX", "Window X position must be in the range 0..9999");

                m_configCache["ZwiftActivityMonitor"]["WindowStartupPosition"]["X"] = value.ToString();
            }
        }
        public int WindowPositionY
        {
            get
            {
                if (int.TryParse(m_configuration["ZwiftActivityMonitor:WindowStartupPosition:Y"], out int yPos))
                    return yPos;

                return 0;
            }

            set
            {
                if (value < 0 || value > 9999)
                    throw new ArgumentOutOfRangeException("WindowPositionY", "Window Y position must be in the range 0..9999");

                m_configCache["ZwiftActivityMonitor"]["WindowStartupPosition"]["Y"] = value.ToString();
            }
        }

    }
}
