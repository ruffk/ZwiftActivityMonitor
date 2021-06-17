using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitorV2
{
    public partial class UserControlBase : UserControl
    {
        internal ILogger Logger;

        public UserControlBase() : base()
        {
            InitializeComponent();
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

                if (Logger != null)
                    Logger.LogInformation($"SkipControl_Enter, Control: {c.Name}");
            }
        }

        /// <summary>
        /// This method allows closing forms and changing tabpages to query a user control to see if the action should be canceled.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void ControlLosingFocus(object sender, Syncfusion.Windows.Forms.Tools.SelectedIndexChangingEventArgs e)
        {
            if (Logger != null)
                Logger.LogInformation($"ControlLosingFocus");
        }

        public virtual void ControlGainingFocus(object sender, EventArgs e)
        {
            if (Logger != null)
                Logger.LogInformation($"ControlGainingFocus");
        }

        #region ListView helpers
        protected virtual void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        protected virtual void Listview_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        /// <summary>
        /// Call this from the ListView's Resize event handler
        /// </summary>
        /// <param name="listView"></param>
        protected static void HideHorizontalScrollBar(ListView listView)
        {
            listView.Scrollable = true;

            ZAMsettings.ShowScrollBar(listView.Handle, 0, false);
        }

        /// <summary>
        /// This method allows changing the ListView column header colors.  Call this static method from your form or listview load.
        /// Be sure to wire in the DrawItem and DrawSubItem events.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="backColor"></param>
        /// <param name="foreColor"></param>
        protected static void SetListViewHeaderColor(ref ListView list, Color backColor, Color foreColor)
        {
            //ILogger logger = ZAMsettings.LoggerFactory.CreateLogger("SetListViewHeaderColor");
            //logger.LogInformation($"ListView {list.Name}");

            list.OwnerDraw = true;
            list.DrawColumnHeader +=
                new DrawListViewColumnHeaderEventHandler
                (
                    (sender, e) => ListView_DrawListViewColumnHeader(sender, e, backColor, foreColor)
                );


            //list.DrawItem += new DrawListViewItemEventHandler(ListView_DrawListViewItem);
        }

        private static void ListView_DrawListViewColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e, Color backColor, Color foreColor)
        {
            //ILogger logger = ZAMsettings.LoggerFactory.CreateLogger("ListView_DrawListViewColumnHeader");

            using (SolidBrush backBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);

                // draw the 3d header
                // 
                Rectangle r = e.Bounds;

                r.Width -= 1;
                r.Height -= 1;

                // draw the dark border around the whole thing 
                // 
                e.Graphics.DrawRectangle(SystemPens.ControlDarkDark, r);

                r.Width -= 1;
                r.Height -= 1;

                // draw the light 3D border 
                //
                e.Graphics.DrawLine(SystemPens.ControlLightLight, r.X, r.Y, r.Right, r.Y);
                e.Graphics.DrawLine(SystemPens.ControlLightLight, r.X, r.Y, r.X, r.Bottom);

                // draw the dark 3D Border 
                //
                e.Graphics.DrawLine(SystemPens.ControlDark, r.X + 1, r.Bottom, r.Right, r.Bottom);
                e.Graphics.DrawLine(SystemPens.ControlDark, r.Right, r.Y + 1, r.Right, r.Bottom);
            }

            
            using (StringFormat sf = new StringFormat())
            {
                // Store the column text alignment, letting it default
                // to Left if it has not been set to Center or Right.
                switch (e.Header.TextAlign)
                {
                    case HorizontalAlignment.Center:
                        sf.Alignment = StringAlignment.Center;
                        break;
                    case HorizontalAlignment.Right:
                        sf.Alignment = StringAlignment.Far;
                        break;
                }

                using (SolidBrush foreBrush = new SolidBrush(foreColor))
                {
                    e.Graphics.DrawString(e.Header.Text, e.Font, foreBrush, e.Bounds, sf);
                }
            }
        }

        #endregion

        protected virtual void ListView_ItemSelectionChanged_Disable(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
                e.Item.Selected = false;
        }

        protected virtual void ListView_Resize_HideHorizontalScrollBar(object sender, EventArgs e)
        {
            ListView lv = (ListView)sender;
            UserControlBase.HideHorizontalScrollBar(lv);

            if (Logger != null)
                Logger.LogInformation($"ListView_Resize_HideHorizontalScrollBar {lv.Name}");

        }


        /// <summary>
        /// The DesignMode property does not correctly tell you if
        /// you are in design mode.  IsDesignerHosted is a corrected
        /// version of that property.
        /// (see https://connect.microsoft.com/VisualStudio/feedback/details/553305
        /// and http://stackoverflow.com/a/2693338/238419 )
        /// </summary>
        protected bool IsDesignerHosted
        {
            get
            {
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                    return true;

                Control ctrl = this;
                while (ctrl != null)
                {
                    if ((ctrl.Site != null) && ctrl.Site.DesignMode)
                        return true;
                    ctrl = ctrl.Parent;
                }
                return false;
            }
        }
        public static bool IsInDesignMode()
        {
            return System.Reflection.Assembly.GetExecutingAssembly()
                 .Location.Contains("VisualStudio");
        }
    }
}
