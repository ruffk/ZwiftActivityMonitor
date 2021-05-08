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
        #region LapListViewItem Class
        //internal class LapListViewItem : ListViewItem
        //{
        //    public LapViewControl.LapDetailItem LapItem { get; set; }

        //    public LapListViewItem(LapViewControl.LapDetailItem item, MeasurementSystemType roadUom, MeasurementSystemType powerUom) : base(SubItemStrings(item, roadUom, powerUom))
        //    {
        //        this.LapItem = item;
        //        this.Name = item.LapNumber.ToString(); // this is the Key in the listview.items collection
        //    }

        //    private static string[] SubItemStrings(LapViewControl.LapDetailItem item, MeasurementSystemType roadUom, MeasurementSystemType powerUom)
        //    {
        //        return (new string[]
        //        {
        //            "", // dummy first column
        //            item.LapNumber.ToString(),
        //            $"{item.LapTime.Minutes:0#}:{item.LapTime.Seconds:0#}",
        //            roadUom == MeasurementSystemType.Metric ? $"{item.LapSpeedKph:0.0}" : $"{item.LapSpeedMph:0.0}",
        //            roadUom == MeasurementSystemType.Metric ? $"{item.LapDistanceKm:0.0}" : $"{item.LapDistanceMi:0.0}",
        //            powerUom == MeasurementSystemType.Metric ? $"{item.LapAvgWkg:0.00}" : $"{item.LapAvgWatts}",
        //            $"{item.TotalTime.Hours:0#}:{item.TotalTime.Minutes:0#}:{item.TotalTime.Seconds:0#}"
        //        });
        //    }

        //    public static void RefreshAll(ListView listView, MeasurementSystemType roadUom, MeasurementSystemType powerUom)
        //    {
        //        foreach(ListViewItem item in listView.Items)
        //        {
        //            (item as LapListViewItem).Refresh(roadUom, powerUom);
        //        }
        //    }

        //    public void Refresh(MeasurementSystemType roadUom, MeasurementSystemType powerUom)
        //    {
        //        string[] text = SubItemStrings(LapItem, roadUom, powerUom);

        //        // Update the speed column header text accordingly
        //        this.ListView.Columns[3].Text = roadUom == MeasurementSystemType.Metric ? "km/h" : "mi/h";
        //        this.ListView.Columns[4].Text = roadUom == MeasurementSystemType.Metric ? "km" : "mi";
        //        this.ListView.Columns[5].Text = powerUom == MeasurementSystemType.Metric ? "w/kg" : "Avg";

        //        for (int i = 0; i < text.Length; i++)
        //            this.SubItems[i].Text = text[i];
        //    }
        //}
        #endregion

        private MeasurementSystemType CurrentPowerUom { get; set; }
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

        public void InitLapEventCompletion(LapViewControl.LapDetailItem item)
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
                new ColumnHeader() { Text = "km/h", TextAlign = HorizontalAlignment.Center, Width = 48 }, 
                new ColumnHeader() { Text = "km", TextAlign = HorizontalAlignment.Center, Width = 50 }, 
                new ColumnHeader() { Text = "Avg", TextAlign = HorizontalAlignment.Center, Width = 50 },
                new ColumnHeader() { Text = "Time", TextAlign = HorizontalAlignment.Center, Width = 72 },
            });

            this.CurrentPowerUom = MeasurementSystemType.Imperial;
            this.lvDetail.Items.Add(new LapViewControl.LapListViewItem(item, ZAMsettings.Settings.Laps.MeasurementSystemSetting, this.CurrentPowerUom));

            LapViewControl.LapListViewItem.RefreshAll(lvDetail, ZAMsettings.Settings.Laps.MeasurementSystemSetting, this.CurrentPowerUom);

            this.TimerTicks = 0;
            this.CountdownTimer.Enabled = true;
        }

        public void InitSplitEventCompletion(SplitsViewControl.SplitItem item)
        {
            this.lvHeader.Columns.Clear();
            this.lvDetail.Columns.Clear();
            this.lvDetail.Items.Clear();

            this.lvHeader.Columns.AddRange(new ColumnHeader[]
            {
                new ColumnHeader() { Text = "", Width = 0 },
                new ColumnHeader() { Text = "Split", TextAlign = HorizontalAlignment.Center, Width = 182 },
                new ColumnHeader() { Text = "Total", TextAlign = HorizontalAlignment.Center, Width = 132 },
            });

            this.lvDetail.Columns.AddRange(new ColumnHeader[]
            {
                new ColumnHeader() { Text = "", Width = 0 },
                new ColumnHeader() { Text = "#", TextAlign = HorizontalAlignment.Center, Width = 36 },
                new ColumnHeader() { Text = "Time", TextAlign = HorizontalAlignment.Center, Width = 48 },
                new ColumnHeader() { Text = "km/h", TextAlign = HorizontalAlignment.Center, Width = 48 }, 
                new ColumnHeader() { Text = "km", TextAlign = HorizontalAlignment.Center, Width = 50 }, 
                new ColumnHeader() { Text = "Time", TextAlign = HorizontalAlignment.Center, Width = 72 },
                new ColumnHeader() { Text = "+/-", TextAlign = HorizontalAlignment.Center, Width = 60 },
            });

            this.lvDetail.Items.Add(new SplitsViewControl.SplitListViewItem(item));

            (this.lvDetail.Items[0] as SplitsViewControl.SplitListViewItem).Refresh();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            this.TimerTicks++;

            if (this.TimerTicks % 3 == 0)
            {
                this.CurrentPowerUom = this.CurrentPowerUom == MeasurementSystemType.Metric ? MeasurementSystemType.Imperial : MeasurementSystemType.Metric;

                (this.lvDetail.Items[0] as LapViewControl.LapListViewItem).Refresh(ZAMsettings.Settings.Laps.MeasurementSystemSetting, this.CurrentPowerUom);
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
