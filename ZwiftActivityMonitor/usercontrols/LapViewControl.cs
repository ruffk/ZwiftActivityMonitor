using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitor
{
    public class LapDetailItem
    {
        public int LapNumber { get; }
        public TimeSpan LapTime { get; } = TimeSpan.Zero;
        public double LapDistanceKm { get; }
        public int LapAvgWatts { get; }
        public TimeSpan TotalTime { get; } = TimeSpan.Zero;

        public double LapDistanceMi { get; }
        public double LapSpeedKph { get; }
        public double LapSpeedMph { get; }
        public double LapAvgWkg { get; }



        public LapDetailItem(int lapNumber, TimeSpan lapTime, double lapDistanceKm, int lapAvgWatts, TimeSpan totalTime)
        {
            this.LapNumber = lapNumber;
            this.LapTime = lapTime;
            this.LapDistanceKm = Math.Round(lapDistanceKm, 1);
            this.LapAvgWatts = lapAvgWatts;
            this.TotalTime = totalTime;

            this.LapDistanceMi = Math.Round(lapDistanceKm / 1.609, 1);

            if (lapTime.TotalSeconds > 0)
            {
                this.LapSpeedKph = Math.Round((this.LapDistanceKm / lapTime.TotalSeconds) * 3600, 1);
                this.LapSpeedMph = Math.Round((this.LapDistanceMi / lapTime.TotalSeconds) * 3600, 1);
            }

            if (ZAMsettings.Settings.CurrentUser.WeightAsKgs > 0)
            {
                this.LapAvgWkg = Math.Round(lapAvgWatts / ZAMsettings.Settings.CurrentUser.WeightAsKgs, 2);
            }
        }

        public LapDetailItem()
        {
        }
    }

    public partial class LapViewControl : UserControlBase
    {
        #region Internal LapListViewItem Class

        internal class LapListViewColumnSorter : IComparer
        {
            /// <summary>
            /// Specifies the column to be sorted
            /// </summary>
            //public int ColumnToSort { get; set; }

            /// <summary>
            /// Specifies the order in which to sort (i.e. 'Ascending').
            /// </summary>
            private SortOrder OrderOfSort { get; set; }

            public LapListViewColumnSorter(SortOrder sortOrder)
            {
                // Initialize the column to '0'
                //ColumnToSort = 0;

                OrderOfSort = sortOrder;

                // Initialize the CaseInsensitiveComparer object
                //ObjectCompare = new CaseInsensitiveComparer();
            }

            /// <summary>
            /// This method is inherited from the IComparer interface.
            /// </summary>
            /// <param name="x">First object to be compared</param>
            /// <param name="y">Second object to be compared</param>
            /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
            public int Compare(object x, object y)
            {
                int compareResult;
                LapDetailItem X, Y;

                // Cast the objects to be compared to ListViewItem objects
                X = (x as LapListViewItem).LapItem;
                Y = (y as LapListViewItem).LapItem;


                // Not using ColumnToSort because we're sorting numeric data
                compareResult = X.LapNumber.CompareTo(Y.LapNumber);

                // Compare the two items
                //compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

                // Calculate correct return value based on object comparison
                if (OrderOfSort == SortOrder.Ascending)
                {
                    // Ascending sort is selected, return normal result of compare operation
                    return compareResult;
                }
                else if (OrderOfSort == SortOrder.Descending)
                {
                    // Descending sort is selected, return negative result of compare operation
                    return (-compareResult);
                }
                else
                {
                    // Return '0' to indicate they are equal
                    return 0;
                }
            }
        }


        internal class LapListViewItem : ListViewItem
        {
            public LapDetailItem LapItem { get; set; }

            public enum RefreshUom
            {
                RefreshImperial,
                RefreshMetric
            }

            public LapListViewItem(LapDetailItem item) : base(SubItemStrings(item, RefreshUom.RefreshMetric))
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


        private readonly LapsManager m_LapsManager;
        private Dispatcher m_dispatcher;

        //private Timer backcolorTimer = new Timer();


        public LapViewControl()
        {
            InitializeComponent();

            m_LapsManager = new LapsManager();
            m_LapsManager.LapUpdatedEvent += this.LapEventHandler;


            // When goal Laps complete, either Red or Green will show in background for this period
            //backcolorTimer.Interval = 2000;
            //backcolorTimer.Tick += backcolorTimer_Tick;

            UserControlBase.SetListViewHeaderColor(ref this.lvLaps, Color.FromArgb(255, 243, 108, 61), Color.White); // Orange ListView headers
        }

        protected override void UserControlBase_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            // for handling UI events
            m_dispatcher = Dispatcher.CurrentDispatcher;

            this.Logger = ZAMsettings.LoggerFactory.CreateLogger<LapViewControl>();

            this.lvLaps.Items.Clear();
            this.lvLaps.ListViewItemSorter = new LapListViewColumnSorter(SortOrder.Descending);

            base.UserControlBase_Load(sender, e);
        }

        public override void ControlGainingFocus(object sender, CancelEventArgs e)
        {
            base.ControlGainingFocus(sender, e);
        }


        public void StartCollection()
        {
            this.lvLaps.Items.Clear();

            m_LapsManager.BeginLapMonitoring();

            //lblGoalSpeed.Text = m_LapsManager.GoalText;
        }

        public void StopCollection()
        {
            m_LapsManager.EndLapMonitoring();
        }

        private void OnCollectionStatusChanged()
        {

        }

        public void ClearListView()
        {
            this.lvLaps.Items.Clear();
            //lblGoalSpeed.Text = "";
        }


        /// <summary>
        /// A delegate used solely by the LapEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void LapEventHandlerDelegate(object sender, LapsManager.LapEventArgs e);

        /// <summary>
        /// Occurs each time a lap gets updated or completes.  Allows for UI update by marshalling the call accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LapEventHandler(object sender, LapsManager.LapEventArgs e)
        {
            if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                m_dispatcher.BeginInvoke(new LapEventHandlerDelegate(LapEventHandler), new object[] { sender, e });
                return;
            }

            LapDetailItem LapItem = new(e.LapNumber, e.LapTime, e.LapDistanceKm, e.LapAvgWatts, e.TotalTime);

            if (lvLaps.Items.ContainsKey(LapItem.LapNumber.ToString()))
            {
                LapListViewItem item = (LapListViewItem)lvLaps.Items[LapItem.LapNumber.ToString()];
                item.LapItem = LapItem; // Replace with current LapDetailItem object and refresh
                item.Refresh(LapListViewItem.RefreshUom.RefreshMetric);
            }
            else
            {
                lvLaps.Items.Add(new LapListViewItem(LapItem));
                lvLaps.Sort();
            }

            Logger.LogInformation($"LapEventHandler {LapItem.LapNumber}, {LapItem.LapTime}, {LapItem.LapSpeedKph}km/h, {LapItem.LapDistanceKm}km, {LapItem.TotalTime}");
        }

        /// <summary>
        /// When timer fires revert backcolor to normal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void backcolorTimer_Tick(object sender, EventArgs e)
        //{
        //    this.backcolorTimer.Enabled = false;
        //    lvLaps.BackColor = Color.FromArgb(255, 17, 146, 204); // transparent (based upon key)
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

        private void tsbReset_Click(object sender, EventArgs e)
        {
            m_LapsManager.Reset();
            this.ClearListView();
        }

        private void tsbLap_Click(object sender, EventArgs e)
        {
            m_LapsManager.BeginNewLap();
        }
    }
}
