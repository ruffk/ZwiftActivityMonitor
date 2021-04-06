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
    public partial class MainViewControl : UserControlBase
    {
        #region Internal Classes

        /// <summary>
        /// Wrapper class for MovingAverage and associated classes.
        /// </summary>
        internal class MovingAverageWrapper
        {
            public MovingAverage MovingAverage { get; } // Calculates the moving average based upon ZPMonitorService events
            public ViewerListViewItem ViewerListViewItem { get; }
            public Collector Collector { get; }
            public bool MaxDurationTriggered { get; set; }

            public MovingAverageWrapper(MovingAverage movingAverage, ViewerListViewItem viewerListViewItem, Collector collector)
            {
                MovingAverage = movingAverage;
                ViewerListViewItem = viewerListViewItem;
                Collector = collector;
            }
        }

        internal class ViewerItem
        {
            public string Description { get; set; }
            public string Average { get; set; } = "";
            public string AverageMax { get; set; } = "";
            public string Ftp { get; set; } = "";
            public string HeartRate { get; set; } = "";

            public ViewerItem(string description)
            {
                this.Description = description;
            }

            public void ClearDataFields()
            {
                this.Average = "";
                this.AverageMax = "";
                this.Ftp = "";
                this.HeartRate = "";
            }
        }

        internal class ViewerListViewItem : ListViewItem
        {
            public ViewerItem ViewerItem { get; }

            public ViewerListViewItem(ViewerItem item) : base(SubItemStrings(item))
            {
                this.ViewerItem = item;
            }

            private static string[] SubItemStrings(ViewerItem item)
            {
                return (new string[]
                {
                    item.Description,
                    item.Average,
                    item.AverageMax,
                    item.Ftp,
                    item.HeartRate
                });
            }

            public void Refresh()
            {
                string[] text = SubItemStrings(ViewerItem);

                for (int i = 0; i < 5; i++)
                    this.SubItems[i].Text = text[i];
            }
        }

        internal class SummaryItem
        {
            public string Description { get; set; }
            public string Average { get; set; } = "";
            public string AverageWkg { get; set; } = "";
            public string Normalized { get; set; } = "";
            public string NormalizedWkg { get; set; } = "";
            public string If { get; set; } = "";
            public string Speed { get; set; } = "";
            public string SpeedKph { get; set; } = "";

            public SummaryItem()
            {
                this.Description = "Overall";
            }

            public void ClearDataFields()
            {
                this.Average = "";
                this.Normalized = "";
                this.AverageWkg = "";
                this.NormalizedWkg = "";
                this.If = "";
                this.Speed = "";
                this.SpeedKph = "";
            }
        }

        internal class SummaryListViewItem : ListViewItem
        {
            public SummaryItem SummaryItem { get; }

            public enum RefreshUom
            {
                RefreshImperial,
                RefreshMetric
            }

            public SummaryListViewItem(SummaryItem item) : base(SubItemStrings(item, RefreshUom.RefreshImperial))
            {
                this.SummaryItem = item;
            }

            private static string[] SubItemStrings(SummaryItem item, RefreshUom refreshUom)
            {
                return (new string[]
                {
                    item.Description,
                    refreshUom == RefreshUom.RefreshImperial ? item.Average : item.AverageWkg,
                    refreshUom == RefreshUom.RefreshImperial ? item.Normalized : item.NormalizedWkg,
                    item.If,
                    refreshUom == RefreshUom.RefreshImperial ? item.Speed : item.SpeedKph
                });
            }

            public void Refresh(RefreshUom refreshUom)
            {
                string[] text = SubItemStrings(SummaryItem, refreshUom);

                // Update the speed column header text accordingly
                this.ListView.Columns[4].Text = refreshUom == RefreshUom.RefreshImperial ? "mi/h" : "km/h";

                for (int i = 0; i < 5; i++)
                    this.SubItems[i].Text = text[i];
            }
        }

        internal class SummaryHelper
        {
            public SummaryListViewItem SummaryListViewItem { get; }

            public SummaryHelper(SummaryListViewItem summaryListViewItem)
            {
                this.SummaryListViewItem = summaryListViewItem;
            }
        }

        #endregion

        private readonly SummaryHelper m_summaryHelper;
        private readonly Dictionary<DurationType, MovingAverageWrapper> m_maCollection;


        private NormalizedPower m_normalizedPower;
        private Dispatcher m_dispatcher;        // Current UI thread dispatcher, for marshalling UI calls
        private DateTime m_lastViewerRefresh;   // Time of last Viewer list view refresh
        private DateTime m_lastSummaryRefresh;  // Time of last Summary list view refresh
        private int m_summaryRefreshCount;      // Number of times Summary list view has been refreshed

        private Timer refreshTimer = new Timer();

        private UserProfile CurrentUser { get; set; }

        public MainViewControl()
        {
            InitializeComponent();

            this.refreshTimer.Interval = 1000;
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);

            m_summaryHelper = new SummaryHelper(new SummaryListViewItem(new SummaryItem()));
            m_maCollection = new Dictionary<DurationType, MovingAverageWrapper>();

            UserControlBase.SetListViewHeaderColor(ref this.lvViewer, Color.FromArgb(255, 243, 108, 61), Color.White); // Orange ListView headers
            UserControlBase.SetListViewHeaderColor(ref this.lvOverall, Color.FromArgb(255, 243, 108, 61), Color.White); // Orange ListView headers
        }

        public void StartCollection()
        {
            this.CurrentUser = ZAMsettings.Settings.CurrentUser;

            foreach (MovingAverageWrapper maw in m_maCollection.Values)
            {
                maw.MovingAverage.Start();
            }

            m_normalizedPower.Start();

            refreshTimer.Enabled = true;

            this.ResetCounters();
        }
        public void StopCollection()
        {
            foreach (MovingAverageWrapper maw in m_maCollection.Values)
            {
                maw.MovingAverage.Stop();
            }
            m_normalizedPower.Stop();

            refreshTimer.Enabled = false;
        }

        public void ClearViewerItems()
        {
            // empty the MovingAverageWrapper dictionary
            m_maCollection.Clear();

            // Clear the ListView
            this.lvViewer.Items.Clear();
        }

        public int CountViewerItems
        {
            get { return this.lvViewer.Items.Count; }
        }

        public void AddViewerItem(DurationType durationType, string durationDesc)
        {
            MovingAverage ma = new MovingAverage(durationType, false);

            ma.MovingAverageChangedEvent += MovingAverageChangedEventHandler;
            ma.MovingAverageMaxChangedEvent += MovingAverageMaxChangedEventHandler;

            // Initialize and add item to ListView
            ViewerListViewItem lvi = new ViewerListViewItem(new ViewerItem(durationDesc));
            this.lvViewer.Items.Add(lvi);

            Collector collector = ZAMsettings.Settings.Collectors[durationDesc];

            m_maCollection.Add(durationType, new MovingAverageWrapper(ma, lvi, collector));
        }

        /// <summary>
        /// A delegate used solely by the MovingAverageChangedEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void MovingAverageChangedEventHandlerDelegate(object sender, MovingAverage.MovingAverageChangedEventArgs e);

        /// <summary>
        /// Occurs each time a collector's moving average changes.  Allows for UI update by marshalling the call accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovingAverageChangedEventHandler(object sender, MovingAverage.MovingAverageChangedEventArgs e)
        {
            //if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            //{
            //    // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
            //    m_dispatcher.BeginInvoke(new MovingAverageChangedEventHandlerDelegate(MovingAverageChangedEventHandler), new object[] { sender, e });
            //    return;
            //}

            string average = "";
            string Ftp = null;
            string heartRate;

            MovingAverageWrapper wrapper = m_maCollection[e.DurationType];
            Collector collector = wrapper.Collector;

            switch (collector.FieldAvgType)
            {
                case FieldUomType.Watts:
                    average = e.AveragePower.ToString();
                    break;

                case FieldUomType.Wkg:
                    if (CurrentUser.WeightAsKgs > 0)
                        average = Math.Round(e.AveragePower / CurrentUser.WeightAsKgs, 2).ToString("#.00");
                    break;
            }

            heartRate = e.AverageHR.ToString();

            // The FTP column will track the AvgPower until the time duration is satisfied.
            // This enables the rider to see what his FTP would be real-time.
            // Once the time duration is satisfied, we no longer will update using the AvgPower.
            if (!wrapper.MaxDurationTriggered)
            {
                switch (collector.FieldFtpType)
                {
                    case FieldUomType.Watts:
                        Ftp = Math.Round(e.AveragePower * 0.95, 0).ToString();
                        break;

                    case FieldUomType.Wkg:
                        if (CurrentUser.WeightAsKgs > 0)
                            Ftp = Math.Round((e.AveragePower / CurrentUser.WeightAsKgs) * 0.95, 2).ToString("#.00");
                        break;
                }

            }

            this.UpdateViewerMovingAverages(wrapper, average, null, Ftp, heartRate);
        }

        /// <summary>
        /// A delegate used solely by the MovingAverageMaxChangedEventHandlerDelegate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void MovingAverageMaxChangedEventHandlerDelegate(object sender, MovingAverage.MovingAverageMaxChangedEventArgs e);

        /// <summary>
        /// Occurs each time a collector's moving average max value changes.  Allows for UI update by marshalling the call accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MovingAverageMaxChangedEventHandler(object sender, MovingAverage.MovingAverageMaxChangedEventArgs e)
        {
            //if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            //{
            //    // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
            //    m_dispatcher.BeginInvoke(new MovingAverageMaxChangedEventHandlerDelegate(MovingAverageMaxChangedEventHandler), new object[] { sender, e });
            //    return;
            //}

            string averageMax = "";
            string Ftp = "";

            MovingAverageWrapper wrapper = m_maCollection[e.DurationType];
            Collector collector = wrapper.Collector;

            switch (collector.FieldAvgMaxType)
            {
                case FieldUomType.Watts:
                    averageMax = e.MaxAvgPower.ToString();
                    break;

                case FieldUomType.Wkg:
                    if (CurrentUser.WeightAsKgs > 0)
                        averageMax = Math.Round(e.MaxAvgPower / CurrentUser.WeightAsKgs, 2).ToString("#.00");
                    break;
            }

            // Save the fact that this moving average has fulfilled it's time duration
            wrapper.MaxDurationTriggered = true;

            // The FTP column will now track the MaxAvgPower now that the time duration is satisfied.
            switch (collector.FieldFtpType)
            {
                case FieldUomType.Watts:
                    Ftp = Math.Round(e.MaxAvgPower * 0.95, 0).ToString();
                    break;

                case FieldUomType.Wkg:
                    if (CurrentUser.WeightAsKgs > 0)
                        Ftp = Math.Round((e.MaxAvgPower / CurrentUser.WeightAsKgs) * 0.95, 2).ToString("#.00");
                    break;
            }

            this.UpdateViewerMovingAverages(wrapper, null, averageMax, Ftp, null);
        }

        private void UpdateViewerMovingAverages(MovingAverageWrapper wrapper, string average, string averageMax, string Ftp, string heartRate)
        {
            lock (this.lvViewer)
            {
                ViewerListViewItem listViewItem = wrapper.ViewerListViewItem;
                ViewerItem viewerItem = listViewItem.ViewerItem;

                if (average != null)
                    viewerItem.Average = average;

                if (averageMax != null)
                    viewerItem.AverageMax = averageMax;

                if (Ftp != null)
                    viewerItem.Ftp = Ftp;

                if (heartRate != null)
                    viewerItem.HeartRate = heartRate;
            }
        }



        /// <summary>
        /// A delegate used solely by the NormalizedPowerChangedEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void NormalizedPowerChangedEventHandlerDelegate(object sender, NormalizedPower.NormalizedPowerChangedEventArgs e);

        /// <summary>
        /// Occurs each time the normalized power changes.  Allows for UI update by marshalling the call accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NormalizedPowerChangedEventHandler(object sender, NormalizedPower.NormalizedPowerChangedEventArgs e)
        {
            //if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            //{
            //    // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
            //    m_dispatcher.BeginInvoke(new NormalizedPowerChangedEventHandlerDelegate(NormalizedPowerChangedEventHandler), new object[] { sender, e });
            //    return;
            //}

            string If = "";
            string Normalized;
            string NormalizedWkg = "";

            if (CurrentUser.PowerThreshold > 0)
                If = Math.Round(e.NormalizedPower / (double)CurrentUser.PowerThreshold, 2).ToString("#.00");

            Normalized = e.NormalizedPower.ToString();

            if (CurrentUser.WeightAsKgs > 0)
                NormalizedWkg = Math.Round(e.NormalizedPower / CurrentUser.WeightAsKgs, 2).ToString("#.00");

            this.UpdateSummaryNormalizedPower(If, Normalized, NormalizedWkg);
        }

        private void UpdateSummaryNormalizedPower(string If, string Normalized, string NormalizedWkg)
        {
            lock (this.lvOverall)
            {
                SummaryListViewItem listViewItem = m_summaryHelper.SummaryListViewItem;
                SummaryItem summaryItem = listViewItem.SummaryItem;

                summaryItem.If = If;
                summaryItem.Normalized = Normalized;
                summaryItem.NormalizedWkg = NormalizedWkg;
            }
        }

        /// <summary>
        /// A delegate used solely by the MetricsChangedEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void MetricsChangedEventHandlerDelegate(object sender, NormalizedPower.MetricsChangedEventArgs e);


        /// <summary>
        /// Occurs each time the average speed changes.  Allows for UI update by marshalling the call accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MetricsChangedEventHandler(object sender, NormalizedPower.MetricsChangedEventArgs e)
        {
            //if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            //{
            //    // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
            //    m_dispatcher.BeginInvoke(new MetricsChangedEventHandlerDelegate(MetricsChangedEventHandler), new object[] { sender, e });
            //    return;
            //}

            string Average;
            string AverageWkg = "";
            string Speed;
            string SpeedKph;

            Average = e.OverallPower.ToString();

            if (CurrentUser.WeightAsKgs > 0)
                AverageWkg = Math.Round(e.OverallPower / CurrentUser.WeightAsKgs, 2).ToString("#.00");


            Speed = e.AverageMph.ToString("#.0");
            SpeedKph = e.AverageKph.ToString("#.0");

            this.UpdateSummaryMetrics(Average, AverageWkg, Speed, SpeedKph);
        }


        private void UpdateSummaryMetrics(string Average, string AverageWkg, string Speed, string SpeedKph)
        {
            lock (this.lvOverall)
            {
                SummaryListViewItem listViewItem = m_summaryHelper.SummaryListViewItem;
                SummaryItem summaryItem = listViewItem.SummaryItem;

                summaryItem.Average = Average;
                summaryItem.AverageWkg = AverageWkg;
                summaryItem.Speed = Speed;
                summaryItem.SpeedKph = SpeedKph;
            }
        }


        //public ListView Viewer { get { return this.lvViewer; } }
        //public ListView Summary { get { return this.lvOverall; } }

        public void ResetCounters()
        {
            m_summaryRefreshCount = 0;
            m_lastSummaryRefresh = DateTime.Now;
            m_lastViewerRefresh = DateTime.Now;

        }


        /// <summary>
        /// A delegate used solely by the RefreshListViews
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void RefreshListViewsDelegate(bool clearValues);

        /// <summary>
        /// Refresh the two ListViews on the screen with current information.
        /// This routine is called approx every second while collection is occuring.
        /// In order to moderate screen refreshes, we only update the lvViewer every 5 seconds, and the lvOverall every 10 seconds.
        /// </summary>
        /// <param name="clearDataFields"></param>
        public void RefreshListViews(bool clearDataFields = false)
        {
            if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                m_dispatcher.BeginInvoke(new RefreshListViewsDelegate(RefreshListViews), new object[] { clearDataFields });
                return;
            }

            if (clearDataFields || (DateTime.Now - m_lastViewerRefresh).TotalSeconds >= 5)
            {
                lock (this.lvViewer)
                {
                    this.lvViewer.BeginUpdate();
                    foreach (var wrapper in m_maCollection)
                    {
                        if (clearDataFields)
                            wrapper.Value.ViewerListViewItem.ViewerItem.ClearDataFields();

                        wrapper.Value.ViewerListViewItem.Refresh();
                    }
                    this.lvViewer.EndUpdate();

                    if (!clearDataFields)
                    {
                        m_lastViewerRefresh = DateTime.Now;
                    }
                }
            }

            if (clearDataFields || (DateTime.Now - m_lastSummaryRefresh).TotalSeconds >= 10)
            {
                lock (this.lvOverall)
                {
                    this.lvOverall.BeginUpdate();
                    if (clearDataFields)
                        m_summaryHelper.SummaryListViewItem.SummaryItem.ClearDataFields();

                    m_summaryHelper.SummaryListViewItem.Refresh(m_summaryRefreshCount % 2 == 0 ? SummaryListViewItem.RefreshUom.RefreshImperial : SummaryListViewItem.RefreshUom.RefreshMetric);
                    this.lvOverall.EndUpdate();

                    if (!clearDataFields)
                    {
                        m_summaryRefreshCount++; // for alternating between imperial and metric
                        m_lastSummaryRefresh = DateTime.Now;
                    }
                }
            }

            //m_logger.LogInformation($"RefreshListViews called.");
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            this.RefreshListViews();
        }



        #region Base class overrides for event selection
        protected override void UserControlBase_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            // for handling UI events
            m_dispatcher = Dispatcher.CurrentDispatcher;

            this.Logger = ZAMsettings.LoggerFactory.CreateLogger<MainViewControl>();

            m_normalizedPower = new NormalizedPower();
            m_normalizedPower.NormalizedPowerChangedEvent += NormalizedPowerChangedEventHandler;
            m_normalizedPower.MetricsChangedEvent += MetricsChangedEventHandler;

            this.lvOverall.Items.Clear();
            this.lvOverall.Items.Add(m_summaryHelper.SummaryListViewItem);

            base.UserControlBase_Load(sender, e);
        }

        protected override void ListView_ItemSelectionChanged_Disable(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (DesignMode)
                return;

            base.ListView_ItemSelectionChanged_Disable(sender, e);
        }

        protected override void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if (DesignMode)
                return;
            base.ListView_DrawItem(sender, e);
        }

        protected override void Listview_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (DesignMode)
                return;
            base.Listview_DrawSubItem(sender, e);
        }

        protected override void SkipControl_Enter(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            base.SkipControl_Enter(sender, e);
        }
        protected override void ListView_Resize_HideHorizontalScrollBar(object sender, EventArgs e)
        {
            base.ListView_Resize_HideHorizontalScrollBar(sender, e);
        }

        #endregion


    }
}
