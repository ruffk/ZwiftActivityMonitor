using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZwiftActivityMonitorV2
{
    public class ConfigItemBase
    {
        public ConfigItemBase()
        {

        }

        /// <summary>
        /// This method is called during ZAMsettings initialization.
        /// It is only called once and allows a Config class to perform more advanced initialization, like creating internal objects, etc.
        /// 
        /// Returns count of any initializations that occurred.
        /// </summary>
        public virtual int InitializeDefaultValues()
        {
            return 0;
        }
    }
}
