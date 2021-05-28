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
    public partial class ListViewControl : UserControlBase
    {
        public ListViewControl()
        {
            InitializeComponent();

            //this.CountdownTimer = new();
            //this.CountdownTimer.Interval = 1000;
            //this.CountdownTimer.Tick += this.CountdownTimer_Tick;



            UserControlBase.SetListViewHeaderColor(ref this.lvHeader, SystemColors.Control, Color.Black);
            UserControlBase.SetListViewHeaderColor(ref this.lvDetail, SystemColors.Control, Color.Black);

            //this.lvHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(108)))), ((int)(((byte)(61)))));
            //this.lvHeader.ForeColor = System.Drawing.Color.White;
            //this.lvHeader.Font = new System.Drawing.Font("Franklin Gothic Heavy", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //UserControlBase.SetListViewHeaderColor(ref this.lvHeader, Color.FromArgb(255, 243, 108, 61), Color.White); // Orange ListView headers

            //this.lvDetail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(108)))), ((int)(((byte)(61)))));
            //this.lvDetail.ForeColor = System.Drawing.Color.White;
            //this.lvDetail.Font = new System.Drawing.Font("Franklin Gothic Demi Cond", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            //UserControlBase.SetListViewHeaderColor(ref this.lvDetail, Color.FromArgb(255, 243, 108, 61), Color.White); // Orange ListView headers
        }

        //public void InitLapEventCompletion(LapViewControl.LapDetailItem item)
        //{
        //    this.lvHeader.Columns.Clear();
        //    this.lvDetail.Columns.Clear();
        //    this.lvDetail.Items.Clear();

        //    this.lvHeader.Columns.AddRange(new ColumnHeader[]
        //    {
        //        new ColumnHeader() { Text = "", Width = 0 },
        //        new ColumnHeader() { Text = "Lap", TextAlign = HorizontalAlignment.Center, Width = 242 },
        //        new ColumnHeader() { Text = "Total", TextAlign = HorizontalAlignment.Center, Width = 72 },
        //    });

        //    this.lvDetail.Columns.AddRange(new ColumnHeader[]
        //    {
        //        new ColumnHeader() { Text = "", Width = 0 },
        //        new ColumnHeader() { Text = "#", TextAlign = HorizontalAlignment.Center, Width = 28 },
        //        new ColumnHeader() { Text = "Time", TextAlign = HorizontalAlignment.Center, Width = 66 },
        //        new ColumnHeader() { Text = "km/h", TextAlign = HorizontalAlignment.Center, Width = 48 }, 
        //        new ColumnHeader() { Text = "km", TextAlign = HorizontalAlignment.Center, Width = 50 }, 
        //        new ColumnHeader() { Text = "Avg", TextAlign = HorizontalAlignment.Center, Width = 50 },
        //        new ColumnHeader() { Text = "Time", TextAlign = HorizontalAlignment.Center, Width = 72 },
        //    });

        //    this.CurrentPowerUom = MeasurementSystemType.Imperial;
        //    this.lvDetail.Items.Add(new LapViewControl.LapListViewItem(item, ZAMsettings.Settings.Laps.MeasurementSystemSetting, this.CurrentPowerUom));

        //    LapViewControl.LapListViewItem.RefreshAll(lvDetail, ZAMsettings.Settings.Laps.MeasurementSystemSetting, this.CurrentPowerUom);

        //    this.TimerTicks = 0;
        //    this.CountdownTimer.Enabled = true;
        //}

        //public void InitSplitEventCompletion(SplitsViewControl.SplitItem item)
        //{
        //    this.lvHeader.Columns.Clear();
        //    this.lvDetail.Columns.Clear();
        //    this.lvDetail.Items.Clear();

        //    this.lvHeader.Columns.AddRange(new ColumnHeader[]
        //    {
        //        new ColumnHeader() { Text = "", Width = 0 },
        //        new ColumnHeader() { Text = "Split", TextAlign = HorizontalAlignment.Center, Width = 182 },
        //        new ColumnHeader() { Text = "Total", TextAlign = HorizontalAlignment.Center, Width = 132 },
        //    });

        //    this.lvDetail.Columns.AddRange(new ColumnHeader[]
        //    {
        //        new ColumnHeader() { Text = "", Width = 0 },
        //        new ColumnHeader() { Text = "#", TextAlign = HorizontalAlignment.Center, Width = 36 },
        //        new ColumnHeader() { Text = "Time", TextAlign = HorizontalAlignment.Center, Width = 48 },
        //        new ColumnHeader() { Text = "km/h", TextAlign = HorizontalAlignment.Center, Width = 48 }, 
        //        new ColumnHeader() { Text = "km", TextAlign = HorizontalAlignment.Center, Width = 50 }, 
        //        new ColumnHeader() { Text = "Time", TextAlign = HorizontalAlignment.Center, Width = 72 },
        //        new ColumnHeader() { Text = "+/-", TextAlign = HorizontalAlignment.Center, Width = 60 },
        //    });

        //    this.lvDetail.Items.Add(new SplitsViewControl.SplitListViewItem(item));

        //    (this.lvDetail.Items[0] as SplitsViewControl.SplitListViewItem).Refresh();
        //}

        //private void CountdownTimer_Tick(object sender, EventArgs e)
        //{
        //    this.TimerTicks++;

        //    if (this.TimerTicks % 3 == 0)
        //    {
        //        this.CurrentPowerUom = this.CurrentPowerUom == MeasurementSystemType.Metric ? MeasurementSystemType.Imperial : MeasurementSystemType.Metric;

        //        (this.lvDetail.Items[0] as LapViewControl.LapListViewItem).Refresh(ZAMsettings.Settings.Laps.MeasurementSystemSetting, this.CurrentPowerUom);
        //    }
        //}


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

        private void lvDetail_DoubleClick(object sender, EventArgs e)
        {

        }

        private void lvDetail_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
