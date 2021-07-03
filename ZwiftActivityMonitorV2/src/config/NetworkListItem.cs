using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZwiftActivityMonitorV2
{
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
}
