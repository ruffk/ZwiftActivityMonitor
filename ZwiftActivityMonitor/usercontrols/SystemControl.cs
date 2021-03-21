using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitor
{
    public partial class SystemControl : UserControlBase
    {
        public SystemControl()
        {
            InitializeComponent();
        }

        internal override void UserControlBase_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            base.UserControlBase_Load(sender, e);
        }

        internal override void SkipControl_Enter(object sender, EventArgs e)
        {
            base.SkipControl_Enter(sender, e);
        }

        public override void ControlLosingFocus(object sender, CancelEventArgs e)
        {
            base.ControlLosingFocus(sender, e);

            //if (EditingUserProfiles)
            //{
            //    MessageBox.Show("Please either Save or Cancel current work before proceeding.", "Pending Changes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    e.Cancel = true;
            //}
        }
    }
}
