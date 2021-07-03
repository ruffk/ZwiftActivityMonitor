using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ZwiftActivityMonitorV2
{
    public partial class UserControlWithStatusBase : UserControlBase
    {

        public UserControlWithStatusBase() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Allow the UserControl to tie into changes in its parent's colors
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlWithStatusBase_ParentChanged(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                this.Parent.BackColorChanged += Parent_BackColorChanged;
                this.Parent.ForeColorChanged += Parent_ForeColorChanged;
            }
        }

        protected virtual void Parent_ForeColorChanged(object sender, EventArgs e)
        {
            this.ForeColor = this.Parent.ForeColor;
        }

        protected virtual void Parent_BackColorChanged(object sender, EventArgs e)
        {
            this.BackColor = this.Parent.BackColor;
        }
    }
}
