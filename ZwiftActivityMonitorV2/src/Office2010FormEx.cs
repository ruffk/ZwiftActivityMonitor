using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

using Syncfusion.Windows.Forms;

namespace ZwiftActivityMonitorV2
{
    public class Office2010FormEx : Office2010Form
    {
        /// <summary>
        /// This method allows access to the protected property ColorTable from outside the instance.
        /// </summary>
        public Office2010Colors GetColorTable()
        {
            return this.ColorTable;
        }
    }
}
