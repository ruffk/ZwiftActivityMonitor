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
    public partial class SplitsViewControl : UserControlBase
    {
        #region Internal SplitItem and SplitListViewItem classes

        internal class SplitListViewColumnSorter : IComparer
        {
            /// <summary>
            /// Specifies the column to be sorted
            /// </summary>
            //public int ColumnToSort { get; set; }

            /// <summary>
            /// Specifies the order in which to sort (i.e. 'Ascending').
            /// </summary>
            private SortOrder OrderOfSort { get; set; }

            public SplitListViewColumnSorter(SortOrder sortOrder)
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
                SplitItem X, Y;

                // Cast the objects to be compared to ListViewItem objects
                X = (x as SplitListViewItem).SplitItem;
                Y = (y as SplitListViewItem).SplitItem;


                // Not using ColumnToSort because we're sorting on a string column with numeric data
                compareResult = X.SplitNumber.CompareTo(Y.SplitNumber);

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


        internal class SplitItem
        {
            public string SplitNumber { get; set; }
            public string Time { get; set; } = "";
            public string Speed { get; set; } = "";
            public string TotalDistance { get; set; } = "";
            public string TotalTime { get; set; } = "";
            public string Delta { get; set; } = "";
            public bool SplitsInKm { get; set; }

            public SplitItem()
            {
            }

            public void ClearDataFields()
            {
                this.SplitNumber = "";
                this.Time = "";
                this.Speed = "";
                this.TotalDistance = "";
                this.TotalTime = "";
                this.Delta = "";
            }
        }

        internal class SplitListViewItem : ListViewItem
        {
            public SplitItem SplitItem { get; set; }

            //public enum RefreshUom
            //{
            //    RefreshImperial,
            //    RefreshMetric
            //}

            public SplitListViewItem(SplitItem item) : base(SubItemStrings(item))
            {
                this.SplitItem = item;
                this.Name = item.SplitNumber; // this is the Key in the listview.items collection
            }

            private static string[] SubItemStrings(SplitItem item)
            {
                return (new string[]
                {
                    item.SplitNumber,
                    item.Time,
                    item.Speed,
                    item.TotalDistance,
                    item.TotalTime,
                    item.Delta
                });
            }

            public void Refresh()
            {
                string[] text = SubItemStrings(SplitItem);

                // Update the speed column header text accordingly
                this.ListView.Columns[2].Text = SplitItem.SplitsInKm ? "km/h" : "mi/h";
                this.ListView.Columns[3].Text = SplitItem.SplitsInKm ? "km" : "mi";

                for (int i = 0; i < text.Length; i++)
                    this.SubItems[i].Text = text[i];
            }
        }
        #endregion


        private SplitsManager m_splitsManager;
        private Dispatcher m_dispatcher;

        public SplitsViewControl()
        {
            InitializeComponent();

            UserControlBase.SetListViewHeaderColor(ref this.lvSplits, Color.FromArgb(255, 243, 108, 61), Color.White); // Orange ListView headers
        }

        protected override void UserControlBase_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            // for handling UI events
            m_dispatcher = Dispatcher.CurrentDispatcher;

            this.Logger = ZAMsettings.LoggerFactory.CreateLogger<SplitsViewControl>();

            m_splitsManager = new SplitsManager();

            m_splitsManager.SplitGoalCompletedEvent += SplitGoalCompletedEventHandler;
            m_splitsManager.SplitUpdatedEvent += SplitEventHandler;
            m_splitsManager.SplitCompletedEvent += SplitEventHandler;

            this.lvSplits.Items.Clear();

            this.lvSplits.ListViewItemSorter = new SplitListViewColumnSorter(SortOrder.Descending);

            base.UserControlBase_Load(sender, e);
        }

        public void StartCollection()
        {
            this.lvSplits.Items.Clear();

            m_splitsManager.Start();

            lblGoalSpeed.Text = m_splitsManager.GoalText;
        }

        public void StopCollection()
        {
            m_splitsManager.Stop();
        }

        /// <summary>
        /// Allow owner class to tie into the SplitGoalCompletedEvent and SplitCompletedEvent.  This allows the MainForm to bring this control into focus.
        /// </summary>
        public event EventHandler<SplitsManager.SplitGoalCompletedEventArgs> SplitGoalCompletedEvent
        {
            add
            {
                m_splitsManager.SplitGoalCompletedEvent += value;
            }
            remove
            {
                m_splitsManager.SplitGoalCompletedEvent -= value;
            }
        }
        
        public event EventHandler<SplitsManager.SplitEventArgs> SplitCompletedEvent
        {
            add
            {
                m_splitsManager.SplitCompletedEvent += value;
            }
            remove
            {
                m_splitsManager.SplitCompletedEvent -= value;
            }
        }

        public void ClearListView()
        {
            this.lvSplits.Items.Clear();
            lblGoalSpeed.Text = "";
        }


        /// <summary>
        /// A delegate used solely by the SplitGoalUpdatedEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void SplitEventHandlerDelegate(object sender, SplitsManager.SplitEventArgs e);

        /// <summary>
        /// Occurs each time split gets updated.  Allows for UI update by marshalling the call accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitEventHandler(object sender, SplitsManager.SplitEventArgs e)
        {
            if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                m_dispatcher.BeginInvoke(new SplitEventHandlerDelegate(SplitEventHandler), new object[] { sender, e });
                return;
            }

            SplitItem splitItem = new SplitItem()
            {
                SplitNumber = e.SplitNumberStr,
                Time = e.SplitTimeStr,
                Speed = e.SplitSpeedStr,
                TotalDistance = e.TotalDistanceStr,
                TotalTime = e.TotalTimeStr,
                Delta = "",
                SplitsInKm = e.SplitsInKm
            };

            if (lvSplits.Items.ContainsKey(splitItem.SplitNumber))
            {
                SplitListViewItem item = (SplitListViewItem)lvSplits.Items[splitItem.SplitNumber];
                item.SplitItem = splitItem; // Replace with current splitItem object and refresh
                item.Refresh();
            }
            else
            {
                lvSplits.Items.Add(new SplitListViewItem(splitItem));
                lvSplits.Sort();
            }

            Logger.LogInformation($"SplitEventHandler {splitItem.SplitNumber}, {splitItem.Time}, {splitItem.Speed}, {splitItem.TotalDistance}, {splitItem.TotalTime}");
        }

        /// <summary>
        /// A delegate used solely by the SplitGoalCompletedEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void SplitGoalCompletedEventHandlerDelegate(object sender, SplitsManager.SplitGoalCompletedEventArgs e);

        /// <summary>
        /// Occurs each time split gets completed.  Allows for UI update by marshalling the call accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitGoalCompletedEventHandler(object sender, SplitsManager.SplitGoalCompletedEventArgs e)
        {
            if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                m_dispatcher.BeginInvoke(new SplitGoalCompletedEventHandlerDelegate(SplitGoalCompletedEventHandler), new object[] { sender, e });
                return;
            }

            SplitItem splitItem = new SplitItem()
            {
                SplitNumber = e.SplitNumberStr,
                Time = e.SplitTimeStr,
                Speed = e.SplitSpeedStr,
                TotalDistance = e.TotalDistanceStr,
                TotalTime = e.TotalTimeStr,
                Delta = e.DeltaTimeStr,
                SplitsInKm = e.SplitsInKm
            };

            if (lvSplits.Items.ContainsKey(splitItem.SplitNumber))
            {
                SplitListViewItem item = (SplitListViewItem)lvSplits.Items[splitItem.SplitNumber];
                item.SplitItem = splitItem; // Replace with current splitItem object and refresh
                item.Refresh();
            }
            else
            {
                // This should not happen
                lvSplits.Items.Add(new SplitListViewItem(splitItem));
                lvSplits.Sort();
            }

            Logger.LogInformation($"SplitGoalCompletedEventHandler {splitItem.SplitNumber}, {splitItem.Time}, {splitItem.Speed}, {splitItem.TotalDistance}, {splitItem.TotalTime}");
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
