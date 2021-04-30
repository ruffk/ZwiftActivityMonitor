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
        #region Internal LapItem and LapListViewItem classes

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
                LapItem X, Y;

                // Cast the objects to be compared to ListViewItem objects
                X = (x as LapListViewItem).LapItem;
                Y = (y as LapListViewItem).LapItem;


                // Not using ColumnToSort because we're sorting numeric data
                compareResult = X.LapNumberVal.CompareTo(Y.LapNumberVal);

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


        internal class LapItem
        {
            public int LapNumberVal { get; set; }
            public string LapNumber { get; set; }
            public string Time { get; set; } = "";
            public string Speed { get; set; } = "";
            public string TotalDistance { get; set; } = "";
            public string TotalTime { get; set; } = "";
            public string Delta { get; set; } = "";
            public bool LapsInKm { get; set; }

            public LapItem(int LapNumber)
            {
                this.LapNumberVal = LapNumber;
            }

            public void ClearDataFields()
            {
                this.LapNumber = "";
                this.Time = "";
                this.Speed = "";
                this.TotalDistance = "";
                this.TotalTime = "";
                this.Delta = "";
            }
        }

        internal class LapListViewItem : ListViewItem
        {
            public LapItem LapItem { get; set; }

            //public enum RefreshUom
            //{
            //    RefreshImperial,
            //    RefreshMetric
            //}

            public LapListViewItem(LapItem item) : base(SubItemStrings(item))
            {
                this.LapItem = item;
                this.Name = item.LapNumber; // this is the Key in the listview.items collection
            }

            private static string[] SubItemStrings(LapItem item)
            {
                return (new string[]
                {
                    "", // dummy first column
                    item.LapNumber,
                    item.Time,
                    item.Speed,
                    item.TotalDistance,
                    item.TotalTime,
                    item.Delta
                });
            }

            public void Refresh()
            {
                string[] text = SubItemStrings(LapItem);

                // Update the speed column header text accordingly
                this.ListView.Columns[3].Text = LapItem.LapsInKm ? "km/h" : "mi/h";
                this.ListView.Columns[4].Text = LapItem.LapsInKm ? "km" : "mi";

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

            m_LapsManager.Start();

            //lblGoalSpeed.Text = m_LapsManager.GoalText;
        }

        public void StopCollection()
        {
            m_LapsManager.Stop();
        }

        private void OnCollectionStatusChanged()
        {

        }

        public void ClearListView()
        {
            this.lvLaps.Items.Clear();
            //lblGoalSpeed.Text = "";
        }

        
        ///// <summary>
        ///// A delegate used solely by the LapEventHandler
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private delegate void LapEventHandlerDelegate(object sender, LapsManager.LapEventArgs e);

        ///// <summary>
        ///// Occurs each time a lap gets updated or completes.  Allows for UI update by marshalling the call accordingly.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void LapEventHandler(object sender, LapsManager.LapEventArgs e)
        //{
        //    if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
        //    {
        //        // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
        //        m_dispatcher.BeginInvoke(new LapEventHandlerDelegate(LapEventHandler), new object[] { sender, e });
        //        return;
        //    }

        //    LapItem LapItem = new LapItem(e.LapNumber)
        //    {
        //        LapNumber = e.LapNumberStr,
        //        Time = e.LapTimeStr,
        //        Speed = e.LapSpeedStr,
        //        TotalDistance = e.TotalDistanceStr,
        //        TotalTime = e.TotalTimeStr,
        //        Delta = e.DeltaTimeStr,  // will return empty string if not a goal based Lap
        //        LapsInKm = e.LapsInKm
        //    };

        //    if (lvLaps.Items.ContainsKey(LapItem.LapNumber))
        //    {
        //        LapListViewItem item = (LapListViewItem)lvLaps.Items[LapItem.LapNumber];
        //        item.LapItem = LapItem; // Replace with current LapItem object and refresh
        //        item.Refresh();
        //    }
        //    else
        //    {
        //        lvLaps.Items.Add(new LapListViewItem(LapItem));
        //        lvLaps.Sort();
        //    }

        //    Logger.LogInformation($"LapEventHandler {LapItem.LapNumber}, {LapItem.Time}, {LapItem.Speed}, {LapItem.TotalDistance}, {LapItem.TotalTime}");
        //}


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

    }
}
