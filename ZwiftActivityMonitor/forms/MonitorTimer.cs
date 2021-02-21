using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZwiftActivityMonitor
{
    public partial class MonitorTimer : Form
    {
        public MonitorTimer()
        {
            InitializeComponent();
        }

        public int Minutes 
        { 
            get { return Convert.ToInt32(this.updnMinutes.Value); } 
            set { this.updnMinutes.Value = value; }
        }
        public int Seconds 
        { 
            get { return Convert.ToInt32(this.updnSeconds.Value); } 
            set { this.updnSeconds.Value = value; }
        }
    }
}
