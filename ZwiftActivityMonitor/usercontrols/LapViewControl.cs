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
    public partial class LapViewControl : UserControlBase
    {
        #region public class LapDetailItem
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
                double lapDistanceMi;

                this.LapNumber = lapNumber;
                this.LapTime = lapTime;
                this.LapDistanceKm = Math.Round(lapDistanceKm, 1);
                this.LapAvgWatts = lapAvgWatts;
                this.TotalTime = totalTime;

                lapDistanceMi = lapDistanceKm / 1.609;
                this.LapDistanceMi = Math.Round(lapDistanceMi, 1);

                if (lapTime.TotalSeconds > 0)
                {
                    this.LapSpeedKph = Math.Round((lapDistanceKm / lapTime.TotalSeconds) * 3600, 1);
                    this.LapSpeedMph = Math.Round((lapDistanceMi / lapTime.TotalSeconds) * 3600, 1);
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
        #endregion

        #region public class LapListViewColumnSorter and LapListViewItem

        public class LapListViewColumnSorter : IComparer
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


        public class LapListViewItem : ListViewItem
        {
            public LapDetailItem LapItem { get; set; }

            public LapListViewItem(LapViewControl.LapDetailItem item, MeasurementSystemType roadUom, MeasurementSystemType powerUom) : base(SubItemStrings(item, roadUom, powerUom))
            {
                this.LapItem = item;
                this.Name = item.LapNumber.ToString(); // this is the Key in the listview.items collection
            }

            private static string[] SubItemStrings(LapViewControl.LapDetailItem item, MeasurementSystemType roadUom, MeasurementSystemType powerUom)
            {
                return (new string[]
                {
                    "", // dummy first column
                    item.LapNumber.ToString(),
                    $"{item.LapTime.Minutes:0#}:{item.LapTime.Seconds:0#}",
                    roadUom == MeasurementSystemType.Metric ? $"{item.LapSpeedKph:0.0}" : $"{item.LapSpeedMph:0.0}",
                    roadUom == MeasurementSystemType.Metric ? $"{item.LapDistanceKm:0.0}" : $"{item.LapDistanceMi:0.0}",
                    powerUom == MeasurementSystemType.Metric ? $"{item.LapAvgWkg:0.00}" : $"{item.LapAvgWatts}",
                    $"{item.TotalTime.Hours:0#}:{item.TotalTime.Minutes:0#}:{item.TotalTime.Seconds:0#}"
                });
            }

            public static void RefreshAll(ListView listView, MeasurementSystemType roadUom, MeasurementSystemType powerUom)
            {
                foreach (ListViewItem item in listView.Items)
                {
                    (item as LapListViewItem).Refresh(roadUom, powerUom);
                }
            }

            public void Refresh(MeasurementSystemType roadUom, MeasurementSystemType powerUom)
            {
                string[] text = SubItemStrings(LapItem, roadUom, powerUom);

                // Update the speed column header text accordingly
                this.ListView.Columns[3].Text = roadUom == MeasurementSystemType.Metric ? "km/h" : "mi/h";
                this.ListView.Columns[4].Text = roadUom == MeasurementSystemType.Metric ? "km" : "mi";
                this.ListView.Columns[5].Text = powerUom == MeasurementSystemType.Metric ? "w/kg" : "Avg";

                for (int i = 0; i < text.Length; i++)
                    this.SubItems[i].Text = text[i];
            }
        }
        #endregion


        private readonly LapsManager m_LapsManager;
        private Dispatcher m_dispatcher;

        private MeasurementSystemType CurrentPowerUom { get; set; }
        private int TimerTicks { get; set; }
        private int StatusLabelSeconds { get; set; }
        private Timer CountdownTimer { get; }


        public LapViewControl()
        {
            InitializeComponent();

            m_LapsManager = new LapsManager();
            m_LapsManager.LapUpdatedEvent += this.LapEventHandler;
            m_LapsManager.LapStartedEvent += this.LapStartedEventHandler;


            this.CountdownTimer = new();
            this.CountdownTimer.Interval = 1000;
            this.CountdownTimer.Tick += this.CountdownTimer_Tick;

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

            CountdownTimer.Enabled = true;

            //lblGoalSpeed.Text = m_LapsManager.GoalText;
        }

        public void StopCollection()
        {
            m_LapsManager.EndLapMonitoring();

            CountdownTimer.Enabled = false;
        }

        private void OnCollectionStatusChanged()
        {

        }

        public void ClearListView()
        {
            this.lvLaps.Items.Clear();
            //lblGoalSpeed.Text = "";
        }

        public event EventHandler<LapsManager.LapEventArgs> LapCompletedEvent
        {
            add
            {
                m_LapsManager.LapCompletedEvent += value;
            }
            remove
            {
                m_LapsManager.LapCompletedEvent -= value;
            }
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
                item.Refresh(ZAMsettings.Settings.Laps.MeasurementSystemSetting, this.CurrentPowerUom);
            }
            else
            {
                LapListViewItem item = new LapListViewItem(LapItem, ZAMsettings.Settings.Laps.MeasurementSystemSetting, this.CurrentPowerUom);
                lvLaps.Items.Add(item);
                lvLaps.Sort();

                // On first item added make sure the column headings match the data fields 
                if (lvLaps.Items.Count == 1)
                {
                    item.Refresh(ZAMsettings.Settings.Laps.MeasurementSystemSetting, this.CurrentPowerUom);
                }
            }

            Logger.LogInformation($"LapEventHandler {LapItem.LapNumber}, {LapItem.LapTime}, {LapItem.LapSpeedKph}km/h, {LapItem.LapDistanceKm}km, {LapItem.TotalTime}");
        }


        /// <summary>
        /// A delegate used solely by the LapStartedEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void LapStartedEventHandlerDelegate(object sender, LapsManager.LapStartedEventArgs e);

        /// <summary>
        /// Occurs each time a lap gets updated or completes.  Allows for UI update by marshalling the call accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LapStartedEventHandler(object sender, LapsManager.LapStartedEventArgs e)
        {
            if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                m_dispatcher.BeginInvoke(new LapStartedEventHandlerDelegate(LapStartedEventHandler), new object[] { sender, e });
                return;
            }

            SetStatusLabel(e.StatusMsg, 5);
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            this.TimerTicks++;

            if (this.TimerTicks % 5 == 0)
            {
                this.CurrentPowerUom = this.CurrentPowerUom == MeasurementSystemType.Metric ? MeasurementSystemType.Imperial : MeasurementSystemType.Metric;

                LapListViewItem.RefreshAll(lvLaps, ZAMsettings.Settings.Laps.MeasurementSystemSetting, this.CurrentPowerUom);
            }

            if (this.StatusLabelSeconds > 0)
            {
                if (--this.StatusLabelSeconds == 0)
                    this.toolStripStatusLabel.Text = "";
            }
        }

        private void SetStatusLabel(string text, int seconds)
        {
            this.toolStripStatusLabel.Text = text;
            this.StatusLabelSeconds = seconds;
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
