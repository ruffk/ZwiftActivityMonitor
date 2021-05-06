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
    public partial class EventCompletionViewControl : UserControlBase
    {
        public enum RefreshUom
        {
            RefreshImperial,
            RefreshMetric
        }

        #region LapListViewItem Class
        internal class LapListViewItem : ListViewItem
        {
            public LapDetailItem LapItem { get; set; }

            public LapListViewItem(LapDetailItem item, RefreshUom refreshUom) : base(SubItemStrings(item, refreshUom))
            {
                this.LapItem = item;
                this.Name = item.LapNumber.ToString(); // this is the Key in the listview.items collection
            }

            private static string[] SubItemStrings(LapDetailItem item, RefreshUom refreshUom)
            {
                bool isMetric = refreshUom == RefreshUom.RefreshMetric;

                return (new string[]
                {
                    "", // dummy first column
                    item.LapNumber.ToString(),
                    $"{item.LapTime.Minutes:0#}:{item.LapTime.Seconds:0#}",
                    isMetric ? $"{item.LapSpeedKph:#.0}" : $"{item.LapSpeedMph:#.0}",
                    isMetric ? $"{item.LapDistanceKm:0.0}" : $"{item.LapDistanceMi:0.0}",
                    isMetric ? $"{item.LapAvgWkg:#.00}" : $"{item.LapAvgWatts}",
                    $"{item.TotalTime.Hours:0#}:{item.TotalTime.Minutes:0#}:{item.TotalTime.Seconds:0#}"
                });
            }

            public void Refresh(RefreshUom refreshUom)
            {
                bool isMetric = refreshUom == RefreshUom.RefreshMetric;

                string[] text = SubItemStrings(LapItem, refreshUom);

                // Update the speed column header text accordingly
                this.ListView.Columns[3].Text = isMetric ? "km/h" : "mi/h";
                this.ListView.Columns[4].Text = isMetric ? "km" : "mi";

                for (int i = 0; i < text.Length; i++)
                    this.SubItems[i].Text = text[i];
            }
        }
        #endregion

        private RefreshUom CurrentUom { get; set; }
        private int TimerTicks { get; set; }
        private Timer CountdownTimer { get; }

        public EventCompletionViewControl()
        {
            InitializeComponent();

            this.CountdownTimer = new();
            this.CountdownTimer.Interval = 1000;
            this.CountdownTimer.Tick += this.CountdownTimer_Tick;

            UserControlBase.SetListViewHeaderColor(ref this.lvHeader, Color.FromArgb(255, 243, 108, 61), Color.White); // Orange ListView headers
            UserControlBase.SetListViewHeaderColor(ref this.lvDetail, Color.FromArgb(255, 243, 108, 61), Color.White); // Orange ListView headers
        }

        public void InitLapEventCompletion(LapDetailItem item)
        {
            this.lvHeader.Columns.Clear();
            this.lvDetail.Columns.Clear();
            this.lvDetail.Items.Clear();

            this.lvHeader.Columns.AddRange(new ColumnHeader[]
            {
                new ColumnHeader() { Text = "", Width = 0 },
                new ColumnHeader() { Text = "Lap", TextAlign = HorizontalAlignment.Center, Width = 242 },
                new ColumnHeader() { Text = "Total", TextAlign = HorizontalAlignment.Center, Width = 72 },
            });

            this.lvDetail.Columns.AddRange(new ColumnHeader[]
            {
                new ColumnHeader() { Text = "", Width = 0 },
                new ColumnHeader() { Text = "#", TextAlign = HorizontalAlignment.Center, Width = 28 },
                new ColumnHeader() { Text = "Time", TextAlign = HorizontalAlignment.Center, Width = 66 },
                new ColumnHeader() { Text = "km/h", TextAlign = HorizontalAlignment.Center, Width = 48 }, // toggles with RefreshUom
                new ColumnHeader() { Text = "km", TextAlign = HorizontalAlignment.Center, Width = 50 }, // toggles with RefreshUom
                new ColumnHeader() { Text = "Avg", TextAlign = HorizontalAlignment.Center, Width = 50 },
                new ColumnHeader() { Text = "Time", TextAlign = HorizontalAlignment.Center, Width = 72 },
            });

            this.CurrentUom = RefreshUom.RefreshMetric;
            this.lvDetail.Items.Add(new LapListViewItem(item, this.CurrentUom));

            this.TimerTicks = 0;
            this.CountdownTimer.Enabled = true;
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            this.TimerTicks++;

            if (this.TimerTicks % 2 == 0)
            {
                this.CurrentUom = this.CurrentUom == RefreshUom.RefreshMetric ? RefreshUom.RefreshImperial : RefreshUom.RefreshMetric;
                (this.lvDetail.Items[0] as LapListViewItem).Refresh(this.CurrentUom);
            }
        }


        #region Base class overrides for event selection

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
