using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitor
{
    public partial class SplitsViewControl : UserControlBase
    {

        internal class SplitItem
        {
            public string SplitNumber { get; set; }
            public string Time { get; set; } = "";
            public string Speed { get; set; } = "";
            public string Distance { get; set; } = "";
            public string TotalTime { get; set; } = "";
            public string Delta { get; set; } = "";

            public SplitItem()
            {
            }

            public void ClearDataFields()
            {
                this.SplitNumber = "";
                this.Time = "";
                this.Speed = "";
                this.Distance = "";
                this.TotalTime = "";
                this.Delta = "";
            }
        }

        internal class SplitListViewItem : ListViewItem
        {
            public SplitItem SplitItem { get; }

            public enum RefreshUom
            {
                RefreshImperial,
                RefreshMetric
            }

            public SplitListViewItem(SplitItem item) : base(SubItemStrings(item, RefreshUom.RefreshImperial))
            {
                this.SplitItem = item;
            }

            private static string[] SubItemStrings(SplitItem item, RefreshUom refreshUom)
            {
                return (new string[]
                {
                    item.SplitNumber,
                    item.Time,
                    refreshUom == RefreshUom.RefreshImperial ? item.Speed : item.Speed,
                    refreshUom == RefreshUom.RefreshImperial ? item.Distance : item.Distance,
                    item.TotalTime,
                    item.Delta
                });
            }

            public void Refresh(RefreshUom refreshUom)
            {
                string[] text = SubItemStrings(SplitItem, refreshUom);

                // Update the speed column header text accordingly
                this.ListView.Columns[2].Text = refreshUom == RefreshUom.RefreshImperial ? "mph" : "km/h";
                this.ListView.Columns[3].Text = refreshUom == RefreshUom.RefreshImperial ? "mi" : "km";

                for (int i = 0; i < text.Length; i++)
                    this.SubItems[i].Text = text[i];
            }
        }

        public SplitsViewControl()
        {
            InitializeComponent();
            UserControlBase.SetListViewHeaderColor(ref this.lvSplits, Color.FromArgb(255, 243, 108, 61), Color.White); // Orange ListView headers
        }

        #region Base class overrides for event selection
        protected override void UserControlBase_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            // for handling UI events
            //m_dispatcher = Dispatcher.CurrentDispatcher;

            this.Logger = ZAMsettings.LoggerFactory.CreateLogger<SplitsViewControl>();

            //this.lvOverall.Items.Clear();
            //this.lvOverall.Items.Add(m_summaryHelper.SummaryListViewItem);

            base.UserControlBase_Load(sender, e);
        }

        protected override void ListView_ItemSelectionChanged_Disable(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            base.ListView_ItemSelectionChanged_Disable(sender, e);
        }

        protected override void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            base.ListView_DrawItem(sender, e);
        }

        protected override void Listview_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            base.Listview_DrawSubItem(sender, e);
        }

        protected override void SkipControl_Enter(object sender, EventArgs e)
        {
            base.SkipControl_Enter(sender, e);
        }
        protected override void ListView_Resize_HideHorizontalScrollBar(object sender, EventArgs e)
        {
            base.ListView_Resize_HideHorizontalScrollBar(sender, e);
        }

        #endregion

    }
}
