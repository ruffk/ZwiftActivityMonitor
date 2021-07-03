using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZwiftActivityMonitorV2
{
    public class DataGridViewEx : DataGridView
    {
        /// <summary>
        /// DataGridView extension property
        /// Set ShowFocus=false to hide cell focus rectangle
        /// </summary>
        public bool? ShowFocus { get; set; }
        
        /// <summary>
        /// Allows override of cell focus rectangle
        /// </summary>
        protected override bool ShowFocusCues
        {
            get
            {
                return this.ShowFocus.HasValue ? this.ShowFocus.Value : base.ShowFocusCues;
            }
        }
    }
}
