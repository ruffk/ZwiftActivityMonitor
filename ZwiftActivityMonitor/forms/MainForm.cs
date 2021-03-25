using System;
using System.Drawing;
using System.Windows.Threading;
using System.Windows.Forms;
using Dapplo.Microsoft.Extensions.Hosting.WinForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace ZwiftActivityMonitor
{
    public partial class MainForm : Form, IWinFormsShell
    {
        #region Internal Classes
        /// <summary>
        /// Wrapper class for MovingAverage and LabelHelper.
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
            public string Normalized { get; set; } = "";
            public string If { get; set; } = "";
            public string Speed { get; set; } = "";

            public SummaryItem()
            {
                this.Description = "Overall";
            }
        }
        internal class SummaryListViewItem : ListViewItem
        {
            public SummaryItem SummaryItem { get; }

            public SummaryListViewItem(SummaryItem item) : base(SubItemStrings(item))
            {
                this.SummaryItem = item;
            }

            private static string[] SubItemStrings(SummaryItem item)
            {
                return (new string[]
                {
                    item.Description,
                    item.Average,
                    item.Normalized,
                    item.If,
                    item.Speed
                });
            }

            public void Refresh()
            {
                string[] text = SubItemStrings(SummaryItem);

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

        private System.Drawing.Point m_offset; // for moving window
        private bool m_mouseDown; // for moving window

        private readonly ILogger<MainForm> m_logger;
        private readonly IServiceProvider m_serviceProvider;
        private readonly ZPMonitorService m_zpMonitorService;
        private readonly ILoggerFactory m_loggerFactory;

        //private readonly List<LabelHelper> m_labelHelpers;
        private readonly Dictionary<DurationType, MovingAverageWrapper> m_maCollection;
        //private readonly Dictionary<string, string> m_labelUnits;
        private readonly NormalizedPower m_normalizedPower;
        private readonly SummaryHelper m_summaryHelper;


        private Dispatcher m_dispatcher; // Current UI thread dispatcher, for marshalling UI calls

        private DateTime m_timerCompletion; // Time when timer countdown should complete
        private DateTime m_collectionStart; // Time when monitor run started
        private bool m_isStarted;           // Whether the collectors are currently running
        private UserProfile m_currentUser;
        //private double m_currentIf;
        //private int m_currentNp;
        //private double m_currentKph;
        //private double m_currentMph;
        //private int m_currentAp;




        public MainForm(IServiceProvider serviceProvider, IConfiguration configuration, ZPMonitorService zpMonitorService, ILoggerFactory loggerFactory)
        {
            m_logger = loggerFactory.CreateLogger<MainForm>(); ;
            m_serviceProvider = serviceProvider;
            m_zpMonitorService = zpMonitorService;
            m_loggerFactory = loggerFactory;

            m_maCollection = new Dictionary<DurationType, MovingAverageWrapper>();
            m_summaryHelper = new SummaryHelper(new SummaryListViewItem(new SummaryItem()));
            //m_labelUnits = new Dictionary<string, string>();

            //m_labelHelpers = new List<LabelHelper>();

            m_normalizedPower = new NormalizedPower(zpMonitorService, loggerFactory);
            m_normalizedPower.NormalizedPowerChangedEvent += NormalizedPowerChangedEventHandler;
            m_normalizedPower.MetricsChangedEvent += MetricsChangedEventHandler;

            InitializeComponent();

            // This rounds the edges of the borderless window
            this.Region = System.Drawing.Region.FromHrgn(ZAMsettings.CreateRoundRectRgn(0, 0, Width, Height, 15, 15));

            //MainForm.colorListViewHeader(ref lvViewer, lvViewer.BackColor, Color.White); // transparent ListView headers
            //MainForm.colorListViewHeader(ref lvOverall, lvOverall.BackColor, Color.White); // transparent ListView headers
            SetListViewHeaderColor(ref lvViewer, Color.FromArgb(255, 243, 108, 61), Color.White); // Orange ListView headers
            SetListViewHeaderColor(ref lvOverall, Color.FromArgb(255, 243, 108, 61), Color.White); // Orange ListView headers
        }

        #region Form Events

        /// <summary>
        /// On initial load, the desired collection durations are load from configuration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // for handling UI events
            m_dispatcher = Dispatcher.CurrentDispatcher;

            // Initialize the settings manager
            ZAMsettings.Initialize(m_loggerFactory);

            // Determine window position
            if (ZAMsettings.Settings.WindowPositionX > 0 && ZAMsettings.Settings.WindowPositionY > 0)
            {
                this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                this.Location = new System.Drawing.Point(ZAMsettings.Settings.WindowPositionX, ZAMsettings.Settings.WindowPositionY);
            }

            this.lvOverall.Items.Clear();
            this.lvOverall.Items.Add(m_summaryHelper.SummaryListViewItem);

            //m_labelHelpers.Add(new LabelHelper(lblMA1, lblAvgPower1, lblMaxPower1, lblFtpPower1, lblAvgHR1));
            //m_labelHelpers.Add(new LabelHelper(lblMA2, lblAvgPower2, lblMaxPower2, lblFtpPower2, lblAvgHR2));
            //m_labelHelpers.Add(new LabelHelper(lblMA3, lblAvgPower3, lblMaxPower3, lblFtpPower3, lblAvgHR3));


            // Set the environment based on the current user
            SetupCurrentUser();

            m_logger.LogInformation("MainForm Loaded");

        }

        private void SetupCurrentUser()
        {
            // Get the currently selected user profile. This will be the user marked as default at startup, but can be changed at runtime.
            m_currentUser = ZAMsettings.Settings.CurrentUser;

            SortedList<string, Collector> selectedCollectors = m_currentUser.SelectedCollectors;

            // Check the menu items for user selected collectors, uncheck the others
            foreach (ToolStripItem mi in tsmiAnalyze.DropDownItems)
            {
                ToolStripMenuItem tsmi = mi as ToolStripMenuItem;

                if (tsmi != null)
                {
                    tsmi.Checked = selectedCollectors.ContainsKey(tsmi.Text);
                }
            }

            //this.Text = $"Zwift Activity Monitor ({m_currentUser.Name})";

            // Load collectors for whatever is defined in by the checked menu items
            LoadMovingAverageCollection();

            m_logger.LogInformation("SetupCurrentUser");
        }

        /// <summary>
        /// Handle the case when form is being shown, either reopened or newly opened.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (ZAMsettings.Settings.AutoStart)
            {
                m_zpMonitorService.StartMonitor();
            }
            else
            {
                // Bring up the options dialog
                tsmiOptions.PerformClick();
            }

            // Set control statuses
            OnCollectionStatusChanged();


            m_logger.LogInformation("MainForm Shown");
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_isStarted)
            {
                if (MessageBox.Show("Are you sure you wish to stop monitoring and close the application?",
                    "Performance Monitor Running", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            Collection_OnStop();

            m_maCollection.Clear();

            m_logger.LogInformation("Closing");
        }

        #endregion

        /// <summary>
        /// Starts the data collectors and sets menu item enabled statuses accordingly.
        /// </summary>
        private void Collection_OnStart()
        {
            if (!m_isStarted)
            {
                foreach (MovingAverageWrapper maw in m_maCollection.Values)
                {
                    maw.MovingAverage.Start();
                }

                m_normalizedPower.Start();

                m_collectionStart = DateTime.Now;
                runTimer.Enabled = true;

                m_isStarted = true;

                OnCollectionStatusChanged();
            }

        }

        /// <summary>
        /// Stops the data collectors and sets menu item enabled statuses accordingly.
        /// </summary>
        private void Collection_OnStop()
        {
            if (m_isStarted)
            {
                foreach (MovingAverageWrapper maw in m_maCollection.Values)
                {
                    maw.MovingAverage.Stop();
                }

                m_normalizedPower.Stop();

                m_isStarted = false;

                runTimer.Enabled = false;

                OnCollectionStatusChanged();
            }

        }

        private void OnCollectionStatusChanged()
        {
            if (m_isStarted)
            {
                // Clear any values on the screen
                //m_labelHelpers.ForEach(helper => helper.ClearLabels(false));
                //tsslOverall.Text = "Collecting...";

                tsmiStop.Enabled = true;
                tsmiStart.Enabled = false;

                tsmi10min.Enabled = false;
                tsmi1min.Enabled = false;
                tsmi20min.Enabled = false;
                tsmi30min.Enabled = false;
                tsmi30sec.Enabled = false;
                tsmi5min.Enabled = false;
                tsmi5sec.Enabled = false;
                tsmi60min.Enabled = false;
                tsmi90min.Enabled = false;

                tsmiTimer.Enabled = false;
                tsmiOptions.Enabled = false;
                tsmiAdvanced.Enabled = false;

                tsslStatus.Text = "Running";

                if (m_zpMonitorService.IsStarted && m_zpMonitorService.IsDebugMode)
                    tsslStatus.Text += " in DEBUG/DEMO mode";
            }
            else
            {
                tsmiStop.Enabled = false;
                tsmiStart.Enabled = true;

                tsmi10min.Enabled = true;
                tsmi1min.Enabled = true;
                tsmi20min.Enabled = true;
                tsmi30min.Enabled = true;
                tsmi30sec.Enabled = true;
                tsmi5min.Enabled = true;
                tsmi5sec.Enabled = true;
                tsmi60min.Enabled = true;
                tsmi90min.Enabled = true;

                tsmiTimer.Enabled = true;
                tsmiOptions.Enabled = true;
                tsmiAdvanced.Enabled = true;

                // set Timer menu sub-items
                if (countdownTimer.Enabled)
                {
                    // Clear any values on the screen
                    //m_labelHelpers.ForEach(helper => helper.ClearLabels(false));
                    //tsslOverall.Text = "";


                    tsmiSetupTimer.Enabled = false;
                    tsmiStopTimer.Enabled = true;
                }
                else
                {
                    tsmiSetupTimer.Enabled = true;
                    tsmiStopTimer.Enabled = false;
                }

                if (m_zpMonitorService.IsStarted)
                    tsslStatus.Text = "Select Analyze->Start to begin";
                else
                    tsslStatus.Text = "ZPM Service Not Running";

            }
        }

        /// <summary>
        /// Load and initialize the moving average collection wrapper collection based upon the current menu item checked settings.
        /// 
        /// This is called on form load and also when menu items change.
        /// </summary>
        private void LoadMovingAverageCollection()
        {
            // empty the MovingAverageWrapper dictionary
            m_maCollection.Clear();

            // Clear the ListView
            lvViewer.Items.Clear();

            //m_labelHelpers.ForEach(helper => helper.ClearLabels(true));
            //tsslOverall.Text = "";

            // Loop through the menu items within the Collect menu.
            // If an item is checked, we want to create a collector for it.
            // The collector duration is determined by a match between the menu item's tag and the DurationType Enum.
            // Up to 3 items can be shown.
            // The label on the UI gets the same text as the menu item.
            foreach (ToolStripItem mi in tsmiAnalyze.DropDownItems)
            {
                ToolStripMenuItem tsmi = mi as ToolStripMenuItem;
                if (tsmi == null) continue;

                if (tsmi.Checked)
                {
                    if (m_maCollection.Count < 3)
                    {
                        DurationType result;
                        if (Enum.TryParse<DurationType>(tsmi.Tag.ToString(), true, out result))
                        {
                            MovingAverage ma = new MovingAverage(m_zpMonitorService, m_loggerFactory, result, false);

                            ma.MovingAverageChangedEvent += MovingAverageChangedEventHandler;
                            ma.MovingAverageMaxChangedEvent += MovingAverageMaxChangedEventHandler;

                            // Initialize and add item to ListView
                            ViewerListViewItem lvi = new ViewerListViewItem(new ViewerItem(tsmi.Text));
                            this.lvViewer.Items.Add(lvi);

                            Collector collector = ZAMsettings.Settings.Collectors[tsmi.Text];

                            m_maCollection.Add(result, new MovingAverageWrapper(ma, lvi, collector));

                            // Here we assign the row's id text (5 sec, 1 min, etc) and associate the matching Collector.
                            // All of this makes it easy to update the display as the MovingAverage events fire.
                            //m_labelHelpers[labelSet].MovingAvg.Text = tsmi.Text;
                            //m_labelHelpers[labelSet].Collector = ZAMsettings.Settings.Collectors[tsmi.Text];

                            if (m_maCollection.Count >= 3) // only allow up to 3 collectors
                                break;
                        }
                        else
                        {
                            throw new ApplicationException($"Bug: The menuitem tag {tsmi.Tag} for menuitem {tsmi.Text} did not match any DurationType Enums.");
                        }
                    }
                    else
                    {
                        tsmi.Checked = false;
                    }
                }
            }

        }

        #region Moving average collection event handlers

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
            if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                m_dispatcher.BeginInvoke(new MovingAverageChangedEventHandlerDelegate(MovingAverageChangedEventHandler), new object[] { sender, e });
                return;
            }

            MovingAverageWrapper wrapper = m_maCollection[e.DurationType];
            Collector collector = wrapper.Collector;
            ViewerListViewItem listViewItem = wrapper.ViewerListViewItem;
            ViewerItem viewerItem = listViewItem.ViewerItem;

            switch (collector.FieldAvgType)
            {
                case FieldUomType.Watts:
                    viewerItem.Average = e.AveragePower.ToString();
                    break;

                case FieldUomType.Wkg:
                    if (m_currentUser.WeightAsKgs > 0)
                    {
                        double wkg = e.AveragePower / m_currentUser.WeightAsKgs;
                        viewerItem.Average = wkg.ToString("#.0#");
                    }
                    break;
            }

            viewerItem.HeartRate = e.AverageHR.ToString();

            // The FTP column will track the AvgPower until the time duration is satisfied.
            // This enables the rider to see what his FTP would be real-time.
            // Once the time duration is satisfied, we no longer will update using the AvgPower.
            if (!wrapper.MaxDurationTriggered)
            {
                switch (collector.FieldFtpType)
                {
                    case FieldUomType.Watts:
                        viewerItem.Ftp = Convert.ToInt32(e.AveragePower * 0.95).ToString();
                        break;

                    case FieldUomType.Wkg:
                        if (m_currentUser.WeightAsKgs > 0)
                        {
                            double wkg = (e.AveragePower / m_currentUser.WeightAsKgs) * 0.95;
                            viewerItem.Ftp = wkg.ToString("#.00");
                        }
                        break;
                }

            }

            // Refresh the ListViewItem's columns
            listViewItem.Refresh();
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
            if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                m_dispatcher.BeginInvoke(new MovingAverageMaxChangedEventHandlerDelegate(MovingAverageMaxChangedEventHandler), new object[] { sender, e });
                return;
            }

            MovingAverageWrapper wrapper = m_maCollection[e.DurationType];
            Collector collector = wrapper.Collector;
            ViewerListViewItem listViewItem = wrapper.ViewerListViewItem;
            ViewerItem viewerItem = listViewItem.ViewerItem;

            switch (collector.FieldAvgMaxType)
            {
                case FieldUomType.Watts:
                    viewerItem.AverageMax = e.MaxAvgPower.ToString();
                    break;

                case FieldUomType.Wkg:
                    if (m_currentUser.WeightAsKgs > 0)
                    {
                        double wkg = e.MaxAvgPower / m_currentUser.WeightAsKgs;
                        viewerItem.AverageMax = wkg.ToString("#.00");
                    }
                    break;
            }

            // Save the fact that this moving average has fulfilled it's time duration
            wrapper.MaxDurationTriggered = true;

            // The FTP column will now track the MaxAvgPower now that the time duration is satisfied.
            switch (collector.FieldFtpType)
            {
                case FieldUomType.Watts:
                    viewerItem.Ftp = Convert.ToInt32(e.MaxAvgPower * 0.95).ToString();
                    break;

                case FieldUomType.Wkg:
                    if (m_currentUser.WeightAsKgs > 0)
                    {
                        double wkg = (e.MaxAvgPower / m_currentUser.WeightAsKgs) * 0.95;
                        viewerItem.Ftp = wkg.ToString("#.00");
                    }
                    break;
            }

            // Refresh the ListViewItem's columns
            listViewItem.Refresh();
        }


        /// <summary>
        /// A delegate used solely by the MovingAverageChangedEventHandler
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
            if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                m_dispatcher.BeginInvoke(new NormalizedPowerChangedEventHandlerDelegate(NormalizedPowerChangedEventHandler), new object[] { sender, e });
                return;
            }

            if (m_currentUser.PowerThreshold > 0)
            {
                m_summaryHelper.SummaryListViewItem.SummaryItem.If = Math.Round(e.NormalizedPower / (double)m_currentUser.PowerThreshold, 2).ToString("#.00");
            }
            else
            {
                m_summaryHelper.SummaryListViewItem.SummaryItem.If = ".00";
            }

            m_summaryHelper.SummaryListViewItem.SummaryItem.Normalized = e.NormalizedPower.ToString();

            m_summaryHelper.SummaryListViewItem.Refresh();
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
            if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                m_dispatcher.BeginInvoke(new MetricsChangedEventHandlerDelegate(MetricsChangedEventHandler), new object[] { sender, e });
                return;
            }

            m_summaryHelper.SummaryListViewItem.SummaryItem.Average = e.OverallPower.ToString();
            m_summaryHelper.SummaryListViewItem.SummaryItem.Speed = e.AverageKph.ToString("#.0");

            m_summaryHelper.SummaryListViewItem.Refresh();
        }

        #endregion

        #region Timer menu and tick event handling

        private void tsmiSetupTimer_Click(object sender, EventArgs e)
        {
            if (!m_zpMonitorService.IsStarted)
            {
                MessageBox.Show("Please use the Advanced Options dialog to start the service.", "ZwiftPacketMonitor Not Started", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MonitorTimer mt = m_serviceProvider.GetService<MonitorTimer>();

            DialogResult result = mt.ShowDialog(this);

            if (result == DialogResult.OK)
            {

                m_timerCompletion = DateTime.Now.AddSeconds((mt.Minutes * 60) + mt.Seconds);

                m_logger.LogInformation($"Minutes: {mt.Minutes} Seconds: {mt.Seconds} Completion Time: {m_timerCompletion.ToString()}");

                countdownTimer.Enabled = true;

                OnCollectionStatusChanged();
            }
        }

        private void tsmiStopTimer_Click(object sender, EventArgs e)
        {
            countdownTimer.Enabled = false;

            OnCollectionStatusChanged();
        }


        private void countdownTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = m_timerCompletion - DateTime.Now;

            if (ts.TotalSeconds <= 0)
            {
                countdownTimer.Enabled = false;
                m_logger.LogInformation($"Go! Go! Go!");

                Collection_OnStart();
            }
            else
            {
                tsslStatus.Text = "Time Remaining: " + ts.Minutes.ToString("0#") + ":" + ts.Seconds.ToString("0#");
            }
        }

        private void runTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - m_collectionStart;

            tsslStatus.Text = "Running time: " + ts.Hours.ToString("0#") + ":" + ts.Minutes.ToString("0#") + ":" + ts.Seconds.ToString("0#");
        }

        #endregion

        private void tsmiAdvanced_Click(object sender, EventArgs e)
        {
            var form = m_serviceProvider.GetService<AdvancedOptions>();

            DialogResult result = form.ShowDialog(this);

            // Allow menus and status bar to update according to what user just did
            OnCollectionStatusChanged();
        }

        private void tsmiCheckForUpdates_Click(object sender, EventArgs e)
        {
            //using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/myuser/myapp"))
            //{
            //    await mgr.Result.UpdateApp();
            //}
        }

        private void tsmiOptions_Click(object sender, EventArgs e)
        {
            var form = new ConfigurationOptions(m_loggerFactory, m_serviceProvider, this.Location);

            DialogResult result = form.ShowDialog(this);

            SetupCurrentUser();

            // Allow menus and status bar to update according to what user just did
            OnCollectionStatusChanged();
        }



        #region Misc Form events

        private void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            m_offset.X = e.X;
            m_offset.Y = e.Y;
            m_mouseDown = true;
        }

        private void lblTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_mouseDown)
            {
                Point currentPos = this.PointToScreen(e.Location);
                this.Location = new Point(currentPos.X - m_offset.X, currentPos.Y - m_offset.Y);
            }
        }

        private void lblTitle_MouseUp(object sender, MouseEventArgs e)
        {
            m_mouseDown = false;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Menu item events

        private void tsmiStart_Click(object sender, EventArgs e)
        {
            if (!m_zpMonitorService.IsStarted)
            {
                MessageBox.Show("Please use the Advanced Options dialog to start the service.", "ZwiftPacketMonitor Not Started", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Collection_OnStart();
        }
        private void tsmiStop_Click(object sender, EventArgs e)
        {
            Collection_OnStop();
        }

        private void anyDuration_Click(object sender, EventArgs e)
        {
            // The checked status for some item has changed.
            LoadMovingAverageCollection();
        }

        #endregion


        #region Static ListView helpers
        private void Listview_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            e.DrawDefault = true;
        }

        public static void SetListViewHeaderColor(ref ListView list, Color backColor, Color foreColor)
        {
            list.OwnerDraw = true;
            list.DrawColumnHeader +=
                new DrawListViewColumnHeaderEventHandler
                (
                    (sender, e) => ListView_DrawListViewColumnHeader(sender, e, backColor, foreColor)
                );
            
            list.DrawItem += new DrawListViewItemEventHandler(ListView_DrawListViewItem);
        }

        private static void ListView_DrawListViewColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e, Color backColor, Color foreColor)
        {
            using (SolidBrush backBrush = new SolidBrush(backColor))
            {
                e.Graphics.FillRectangle(backBrush, e.Bounds);
            }

            using (StringFormat sf = new StringFormat())
            {
                // Store the column text alignment, letting it default
                // to Left if it has not been set to Center or Right.
                switch (e.Header.TextAlign)
                {
                    case HorizontalAlignment.Center:
                        sf.Alignment = StringAlignment.Center;
                        break;
                    case HorizontalAlignment.Right:
                        sf.Alignment = StringAlignment.Far;
                        break;
                }
                sf.LineAlignment = StringAlignment.Center;

                using (SolidBrush foreBrush = new SolidBrush(foreColor))
                {
                    e.Graphics.DrawString(e.Header.Text, e.Font, foreBrush, e.Bounds, sf);
                }
            }
        }

        private static void ListView_DrawListViewItem(object sender, DrawListViewItemEventArgs e)
        {
            e.DrawDefault = true;
        }
        #endregion

    }
}
