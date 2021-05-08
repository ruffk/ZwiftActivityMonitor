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

        public class SplitListViewColumnSorter : IComparer
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


                // Not using ColumnToSort because we're sorting numeric data
                compareResult = X.SplitNumberVal.CompareTo(Y.SplitNumberVal);

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
        //public class SplitDetailItem
        //{
        //    public int SplitNumber { get; }
        //    public TimeSpan SplitTime { get; } = TimeSpan.Zero;
        //    public double SplitSpeedKph { get; }
        //    public double TotalDistanceKm { get; }
        //    public TimeSpan TotalTime { get; } = TimeSpan.Zero;
        //    public TimeSpan Delta { get; } = TimeSpan.Zero;
        //    public bool? AheadOfGoalTime { get; set; }



        //    public SplitDetailItem(int splitNumber, TimeSpan splitTime, double splitSpeedKph, double totalDistanceKm, TimeSpan totalTime, TimeSpan delta, bool? aheadOfGoalTime)
        //    {
        //        this.SplitNumber = splitNumber;
        //        this.SplitTime = splitTime;
        //        this.SplitSpeedKph = splitSpeedKph;
        //        this.TotalDistanceKm = Math.Round(totalDistanceKm, 1);
        //        this.TotalTime = totalTime;
        //        this.Delta = delta;
        //        this.AheadOfGoalTime = aheadOfGoalTime;

        //        //lapDistanceMi = lapDistanceKm / 1.609;
        //        //this.LapDistanceMi = Math.Round(lapDistanceMi, 1);

        //        //if (lapTime.TotalSeconds > 0)
        //        //{
        //        //    this.LapSpeedKph = Math.Round((lapDistanceKm / lapTime.TotalSeconds) * 3600, 1);
        //        //    this.LapSpeedMph = Math.Round((lapDistanceMi / lapTime.TotalSeconds) * 3600, 1);
        //        //}

        //        //if (ZAMsettings.Settings.CurrentUser.WeightAsKgs > 0)
        //        //{
        //        //    this.LapAvgWkg = Math.Round(lapAvgWatts / ZAMsettings.Settings.CurrentUser.WeightAsKgs, 2);
        //        //}
        //    }

        //    public SplitDetailItem()
        //    {
        //    }
        //}


        public class SplitItem
        {
            public int SplitNumberVal { get; set; }
            public string SplitNumber { get; set; }
            public string Time { get; set; } = "";
            public string Speed { get; set; } = "";
            public string TotalDistance { get; set; } = "";
            public string TotalTime { get; set; } = "";
            public string Delta { get; set; } = "";
            public bool SplitsInKm { get; set; }
            public bool? AheadOfGoalTime { get; set; }

            //public SplitItem(int splitNumber)
            //{
            //    this.SplitNumberVal = splitNumber;
            //}
            public SplitItem(int splitNumber, string time, string speed, string totalDistance, string totalTime, string delta, bool splitsInKm, bool? aheadOfGoalTime)
            {
                this.SplitNumberVal = splitNumber;
                this.SplitNumber = splitNumber.ToString();
                this.Time = time;
                this.Speed = speed;
                this.TotalDistance = totalDistance;
                this.TotalTime = totalTime;
                this.Delta = delta;
                this.SplitsInKm = splitsInKm;
                this.AheadOfGoalTime = aheadOfGoalTime;
            }

            //public void ClearDataFields()
            //{
            //    this.SplitNumber = "";
            //    this.Time = "";
            //    this.Speed = "";
            //    this.TotalDistance = "";
            //    this.TotalTime = "";
            //    this.Delta = "";
            //}
        }

        public class SplitListViewItem : ListViewItem
        {
            public SplitItem SplitItem { get; set; }

            public SplitListViewItem(SplitItem item) : base(SubItemStrings(item))
            {
                this.SplitItem = item;
                this.Name = item.SplitNumber; // this is the Key in the listview.items collection
            }

            private static string[] SubItemStrings(SplitItem item)
            {
                return (new string[]
                {
                    "", // dummy first column
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

                // Update the speed and distance header text accordingly
                this.ListView.Columns[3].Text = SplitItem.SplitsInKm ? "km/h" : "mi/h";
                this.ListView.Columns[4].Text = SplitItem.SplitsInKm ? "km" : "mi";

                for (int i = 0; i < text.Length; i++)
                    this.SubItems[i].Text = text[i];

                if (SplitItem.AheadOfGoalTime.HasValue)
                {
                    this.SubItems[6].BackColor = SplitItem.AheadOfGoalTime.Value ? GREEN : RED;

                    if (this.UseItemStyleForSubItems)
                        this.UseItemStyleForSubItems = false;
                }
            }

            public void ClearDeltaBackground()
            {
                if (!this.UseItemStyleForSubItems)
                    this.UseItemStyleForSubItems = true;
            }
        }
        #endregion


        private SplitsManager m_splitsManager;
        private Dispatcher m_dispatcher;

        private Timer backcolorTimer = new Timer();

        public static Color RED = Color.FromArgb(255, 192, 0, 0); // red 
        public static Color GREEN = Color.FromArgb(255, 0, 192, 0); // green
        public static Color TRANSPARENCY = Color.FromArgb(255, 17, 146, 204); // ZAM transparency key

        public SplitsViewControl()
        {
            InitializeComponent();

            // When goal splits complete, either Red or Green will show in background for this period
            backcolorTimer.Interval = 2000;
            backcolorTimer.Tick += backcolorTimer_Tick;

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

        public override void ControlGainingFocus(object sender, CancelEventArgs e)
        {
            base.ControlGainingFocus(sender, e);
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
        public event EventHandler<SplitsManager.SplitEventArgs> SplitGoalCompletedEvent
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
        /// Occurs each time a split gets updated or completes.  Allows for UI update by marshalling the call accordingly.
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


            SplitItem splitItem = new SplitItem(e.SplitNumber, e.SplitTimeStr, e.SplitSpeedStr, e.TotalDistanceStr, e.TotalTimeStr, e.DeltaTimeStr, e.SplitsInKm, e.AheadOfGoalTime);

            //SplitItem splitItem = new SplitItem(e.SplitNumber)
            //{
            //    SplitNumber = e.SplitNumberStr,
            //    Time = e.SplitTimeStr,
            //    Speed = e.SplitSpeedStr,
            //    TotalDistance = e.TotalDistanceStr,
            //    TotalTime = e.TotalTimeStr,
            //    Delta = e.DeltaTimeStr,  // will return empty string if not a goal based split
            //    SplitsInKm = e.SplitsInKm,
            //    AheadOfGoalTime = e.AheadOfGoalTime // null if not a goal based split
            //};

            if (lvSplits.Items.ContainsKey(splitItem.SplitNumber))
            {
                SplitListViewItem item = (SplitListViewItem)lvSplits.Items[splitItem.SplitNumber];
                item.SplitItem = splitItem; // Replace with current splitItem object and refresh
                item.Refresh();
            }
            else
            {
                if (e.SplitNumber > 1)
                {
                    // remove any color coding from previous split
                    string prevSplit = (e.SplitNumber - 1).ToString();
                    if (lvSplits.Items.ContainsKey(prevSplit))
                    {
                        SplitListViewItem item = (SplitListViewItem)lvSplits.Items[prevSplit];
                        item.ClearDeltaBackground();
                    }
                }

                lvSplits.Items.Add(new SplitListViewItem(splitItem));
                lvSplits.Sort();
            }

            //Logger.LogInformation($"SplitEventHandler {splitItem.SplitNumber}, {splitItem.Time}, {splitItem.Speed}, {splitItem.TotalDistance}, {splitItem.TotalTime}");
        }

        
        /// <summary>
        /// A delegate used solely by the SplitGoalCompletedEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void SplitGoalCompletedEventHandlerDelegate(object sender, SplitsManager.SplitEventArgs e);

        /// <summary>
        /// Occurs each time a goal split gets completed.  Allows for UI update by marshalling the call accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitGoalCompletedEventHandler(object sender, SplitsManager.SplitEventArgs e)
        {
            if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                m_dispatcher.BeginInvoke(new SplitGoalCompletedEventHandlerDelegate(SplitGoalCompletedEventHandler), new object[] { sender, e });
                return;
            }

            // do normal update to split window
            this.SplitEventHandler(sender, e);

            if (!e.DeltaTime.HasValue)  // should always have a value as it's a goal split
                return;

            // Positive number is bad, negative good
            //lvSplits.BackColor = e.DeltaTime.Value.TotalSeconds >= 1 ? RED : GREEN;

            // when timer expires color will revert to normal
            //this.backcolorTimer.Enabled = true;

            //Logger.LogInformation($"SplitGoalCompletedEventHandler {splitItem.SplitNumber}, {splitItem.Time}, {splitItem.Speed}, {splitItem.TotalDistance}, {splitItem.TotalTime}");
        }

        /// <summary>
        /// When timer fires revert backcolor to normal.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backcolorTimer_Tick(object sender, EventArgs e)
        {
            this.backcolorTimer.Enabled = false;
            lvSplits.BackColor = Color.FromArgb(255, 17, 146, 204); // transparent (based upon key)
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
