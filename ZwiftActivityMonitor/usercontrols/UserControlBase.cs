using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitor
{
    public partial class UserControlBase : UserControl
    {
        public static ILogger Logger { get; set; }
        public static ErrorProvider ErrorProvider { get; set; }
        public static ToolStripStatusLabel StatusLabel { get; set; }

        public UserControlBase()
        {
            InitializeComponent();
        }

        static UserControlBase()
        {
        }

        /// <summary>
        /// Occurs when user control is initially loaded.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal virtual void UserControlBase_Load(object sender, EventArgs e)
        {
            if (Logger != null)
                Logger.LogInformation($"UserControlBase_Load");
        }

        internal virtual void SkipControl_Enter(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c != null && c.Parent != null)
            {
                c.Parent.SelectNextControl(c, true, true, true, true);
                Logger.LogInformation($"SkipControl_Enter, Control: {c.Name}");
            }
        }
        public virtual void ParentWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logger.LogInformation($"ParentWindow_FormClosing");
        }


    }
}
