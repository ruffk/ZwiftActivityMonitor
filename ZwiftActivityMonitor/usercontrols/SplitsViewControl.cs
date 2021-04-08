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
        #region SplitItem and SplitListViewItem classes
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
            m_splitsManager.SplitGoalUpdatedEvent += SplitGoalUpdatedEventHandler;

            base.UserControlBase_Load(sender, e);
        }

        public void StartCollection()
        {
            this.lvSplits.Items.Clear();

            m_splitsManager.Start();

            lblGoalSpeed.Text = $"{m_splitsManager.GoalDistanceStr} @ {m_splitsManager.GoalSpeedStr}";
        }

        public void StopCollection()
        {
            m_splitsManager.Stop();
        }


        /// <summary>
        /// A delegate used solely by the SplitGoalUpdatedEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void SplitGoalUpdatedEventHandlerDelegate(object sender, SplitsManager.SplitGoalUpdatedEventArgs e);

        /// <summary>
        /// Occurs each time a collector's moving average changes.  Allows for UI update by marshalling the call accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitGoalUpdatedEventHandler(object sender, SplitsManager.SplitGoalUpdatedEventArgs e)
        {
            if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                m_dispatcher.BeginInvoke(new SplitGoalUpdatedEventHandlerDelegate(SplitGoalUpdatedEventHandler), new object[] { sender, e });
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

            if (lvSplits.Items.Count < e.SplitNumber)
            {
                lvSplits.Items.Add(new SplitListViewItem(splitItem));
            }
            else
            {
                SplitListViewItem lvi = (SplitListViewItem)lvSplits.Items[e.SplitNumber - 1];
                lvi.SplitItem = splitItem;
                lvi.Refresh();
            }

            Logger.LogInformation($"SplitGoalUpdatedEventHandler {e.ToString()}");
        }

        /// <summary>
        /// A delegate used solely by the SplitGoalCompletedEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void SplitGoalCompletedEventHandlerDelegate(object sender, SplitsManager.SplitGoalCompletedEventArgs e);

        /// <summary>
        /// Occurs each time a collector's moving average changes.  Allows for UI update by marshalling the call accordingly.
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

            if (lvSplits.Items.Count < e.SplitNumber)
            {
                lvSplits.Items.Add(new SplitListViewItem(splitItem));
            }
            else
            {
                SplitListViewItem lvi = (SplitListViewItem)lvSplits.Items[e.SplitNumber - 1];
                lvi.SplitItem = splitItem;
                lvi.Refresh();
            }

            Logger.LogInformation($"SplitGoalCompletedEventHandler {e.ToString()}");
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
