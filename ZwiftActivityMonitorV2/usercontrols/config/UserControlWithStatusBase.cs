using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ZwiftActivityMonitorV2
{
    public partial class UserControlWithStatusBase : UserControlBase
    {

        public UserControlWithStatusBase()
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
                //Debug.WriteLine($"UserControlWithStatusBase_ParentChanged - Initializing");

                this.Parent.BackColorChanged += Parent_BackColorChanged;
                this.Parent.ForeColorChanged += Parent_ForeColorChanged;

                //Debug.WriteLine($"Parent ForeColor at init: {Parent.ForeColor.R},{Parent.ForeColor.G},{Parent.ForeColor.B}");
                //Debug.WriteLine($"Parent BackColor at init: {Parent.BackColor.R},{Parent.BackColor.G},{Parent.BackColor.B}");

                // Initialize various control colors to current parent values. 
                this.Parent_BackColorChanged(this, new());
                this.Parent_ForeColorChanged(this, new());
            }
        }

        protected virtual void Parent_ForeColorChanged(object sender, EventArgs e)
        {
            this.ForeColor = this.Parent.ForeColor;
            //Debug.WriteLine($"Parent_ForeColorChanged - UC ForeColor Now: {this.ForeColor.R},{this.ForeColor.G},{this.ForeColor.B}");
        }

        protected virtual void Parent_BackColorChanged(object sender, EventArgs e)
        {
            this.BackColor = this.Parent.BackColor;
            //Debug.WriteLine($"Parent_BackColorChanged - UC BackColor Now: {this.BackColor.R},{this.BackColor.G},{this.BackColor.B}");
        }
    }
}
