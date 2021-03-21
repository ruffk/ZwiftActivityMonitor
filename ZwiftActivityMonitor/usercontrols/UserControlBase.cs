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
        public ILogger Logger { get; set; }

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
        protected virtual void UserControlBase_Load(object sender, EventArgs e)
        {
            if (Logger != null)
                Logger.LogInformation($"UserControlBase_Load");
        }

        protected virtual void SkipControl_Enter(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c != null && c.Parent != null)
            {
                c.Parent.SelectNextControl(c, true, true, true, true);
                Logger.LogInformation($"SkipControl_Enter, Control: {c.Name}");
            }
        }

        /// <summary>
        /// This method allows closing forms and changing tabpages to query a user control to see if the action should be canceled.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void ControlLosingFocus(object sender, CancelEventArgs e)
        {
            Logger.LogInformation($"ControlLosingFocus");
        }

        public virtual void ControlGainingFocus(object sender, CancelEventArgs e)
        {
            Logger.LogInformation($"ControlGainingFocus");
        }


    }
}
