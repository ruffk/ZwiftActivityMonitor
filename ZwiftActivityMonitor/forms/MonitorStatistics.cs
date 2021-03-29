using System;
using System.Windows.Threading;
using System.Windows.Forms;
using Dapplo.Microsoft.Extensions.Hosting.WinForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Collections;
using ZwiftPacketMonitor;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpPcap;
using SharpPcap.Npcap;

namespace ZwiftActivityMonitor
{


    public partial class MonitorStatistics : Form, IWinFormsShell
    {
        #region Internal Classes
        /// <summary>
        /// Wrapper class for MovingAverage and LabelHelper.
        /// </summary>
        internal class MovingAverageWrapper
        {
            private MovingAverage m_movingAverage; // Calculates the moving average based upon ZPMonitorService events
            private LabelHelper m_labelHelper; // Encapsulates all the columns in the row that need to be updated 

            public MovingAverageWrapper(MovingAverage movingAverage, LabelHelper labelHelper)
            {
                m_movingAverage = movingAverage;
                m_labelHelper = labelHelper;
            }

            public MovingAverage MovingAverage { get { return m_movingAverage; } }
            public LabelHelper LabelHelper { get { return m_labelHelper; } }
        }

        /// <summary>
        /// Column labels for a moving average row.  Contains an associated Collector to get UOM types for field display.
        /// </summary>
        internal class LabelHelper
        {
            private Label m_lblAvgPower;
            private Label m_lblMaxPower;
            private Label m_lblFtpPower;
            private Label m_lblAvgHR;
            private Label m_lblMovingAvg;
            private Collector m_collector;
            private bool m_maxDurationTriggered;

            public LabelHelper(Label lblMovingAvg, Label lblAvgPower, Label lblMaxPower, Label lblFtpPower, Label lblAvgHR)
            {
                m_lblAvgPower = lblAvgPower;
                m_lblMaxPower = lblMaxPower;
                m_lblFtpPower = lblFtpPower;
                m_lblAvgHR = lblAvgHR;
                m_lblMovingAvg = lblMovingAvg;
            }

            public void ClearLabels(bool clearMovingAvgLabel = true)
            {
                m_lblAvgPower.Text = "";
                m_lblMaxPower.Text = "";
                m_lblFtpPower.Text = "";
                m_lblAvgHR.Text = "";

                if (clearMovingAvgLabel)
                    m_lblMovingAvg.Text = "";

                m_maxDurationTriggered = false;
            }

            public Label AvgPower { get { return m_lblAvgPower; } }
            public Label MaxPower { get { return m_lblMaxPower; } }
            public Label AvgHR { get { return m_lblAvgHR; } }
            public Label FtpPower { get { return m_lblFtpPower; } }
            public Label MovingAvg { get { return m_lblMovingAvg; } }
            public Collector Collector
            {
                get { return m_collector; }
                set { m_collector = value; }
            }
            public bool MaxDurationTriggered { get { return m_maxDurationTriggered; } set { m_maxDurationTriggered = value; } }

        }

        #endregion

        private readonly ILogger<MonitorStatistics> m_logger;
        private readonly IServiceProvider m_serviceProvider;
        private readonly ZPMonitorService m_zpMonitorService;
        private readonly ILoggerFactory m_loggerFactory;

        private readonly List<LabelHelper> m_labelHelpers;
        private readonly Dictionary<DurationType, MovingAverageWrapper> m_maCollection;
        private readonly Dictionary<string, string> m_labelUnits;
        private readonly NormalizedPower m_normalizedPower;


        private Dispatcher m_dispatcher; // Current UI thread dispatcher, for marshalling UI calls

        private DateTime m_timerCompletion; // Time when timer countdown should complete
        private DateTime m_collectionStart; // Time when monitor run started
        private bool m_isStarted;           // Whether the collectors are currently running
        private UserProfile m_currentUser;


        public MonitorStatistics(IServiceProvider serviceProvider, IConfiguration configuration, ZPMonitorService zpMonitorService, ILoggerFactory loggerFactory)
        {
            m_logger = loggerFactory.CreateLogger<MonitorStatistics>(); ;
            m_serviceProvider = serviceProvider;
            m_zpMonitorService = zpMonitorService;
            m_loggerFactory = loggerFactory;

            m_maCollection = new Dictionary<DurationType, MovingAverageWrapper>();
            m_labelUnits = new Dictionary<string, string>();

            m_labelHelpers = new List<LabelHelper>();

            m_normalizedPower = new NormalizedPower(zpMonitorService, loggerFactory);
            m_normalizedPower.NormalizedPowerChangedEvent += NormalizedPowerChangedEventHandler;
            m_normalizedPower.MetricsChangedEvent += MetricsChangedEventHandler;


            InitializeComponent();
        }

        #region Form Events

        /// <summary>
        /// On initial load, the desired collection durations are load from configuration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MonitorStatistics_Load(object sender, EventArgs e)
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


            m_labelHelpers.Add(new LabelHelper(lblMA1, lblAvgPower1, lblMaxPower1, lblFtpPower1, lblAvgHR1));
            m_labelHelpers.Add(new LabelHelper(lblMA2, lblAvgPower2, lblMaxPower2, lblFtpPower2, lblAvgHR2));
            m_labelHelpers.Add(new LabelHelper(lblMA3, lblAvgPower3, lblMaxPower3, lblFtpPower3, lblAvgHR3));


            // Set the environment based on the current user
            SetupCurrentUser();

            m_logger.LogInformation("MonitorStatistics Loaded");

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

            this.Text = $"Zwift Activity Monitor ({m_currentUser.Name})";

            // Load collectors for whatever is defined in by the checked menu items
            LoadMovingAverageCollection();

            m_logger.LogInformation("SetupCurrentUser");
        }

        /// <summary>
        /// Handle the case when form is being shown, either reopened or newly opened.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void MonitorStatistics_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
            }
            m_logger.LogInformation("MonitorStatistics Visible");
        }

        private void MonitorStatistics_Shown(object sender, EventArgs e)
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


            m_logger.LogInformation("MonitorStatistics Shown");
        }


        private void MonitorStatistics_FormClosing(object sender, FormClosingEventArgs e)
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

        /// <summary>
        /// Handle the File:Exit menuitem.  Exiting the form this way will preserve duration choices if re-opened.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiExit_Click(object sender, EventArgs e)
        {
            this.Close();
            //Collection_OnStop();

            //this.Hide(); // Hide here instead of close to preserve menu choices

            m_logger.LogInformation("Exiting");
        }

        private void anyDuration_Click(object sender, EventArgs e)
        {
            // The checked status for some item has changed.
            LoadMovingAverageCollection();
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
                m_labelHelpers.ForEach(helper => helper.ClearLabels(false));
                tsslOverall.Text = "Collecting...";

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
                    m_labelHelpers.ForEach(helper => helper.ClearLabels(false));
                    tsslOverall.Text = "";


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
            // empty the dictionary
            m_maCollection.Clear();

            m_labelHelpers.ForEach(helper => helper.ClearLabels(true));
            tsslOverall.Text = "";

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
                    DurationType result;
                    if (Enum.TryParse<DurationType>(tsmi.Tag.ToString(), true, out result))
                    {
                        MovingAverage ma = new MovingAverage(m_zpMonitorService, m_loggerFactory, result, false);

                        ma.MovingAverageChangedEvent += MovingAverageChangedEventHandler;
                        ma.MovingAverageMaxChangedEvent += MovingAverageMaxChangedEventHandler;

                        // A m_labelHelpers[labelSet] object represents a row on the statistics display
                        int labelSet = m_maCollection.Count;

                        if (labelSet < m_labelHelpers.Count)
                        {
                            m_maCollection.Add(result, new MovingAverageWrapper(ma, m_labelHelpers[labelSet]));

                            // Here we assign the row's id text (5 sec, 1 min, etc) and associate the matching Collector.
                            // All of this makes it easy to update the display as the MovingAverage events fire.
                            m_labelHelpers[labelSet].MovingAvg.Text = tsmi.Text;
                            m_labelHelpers[labelSet].Collector = ZAMsettings.Settings.Collectors[tsmi.Text];

                            if (m_maCollection.Count >= 3) // only allow up to 3 collectors
                                break;
                        }
                    }
                    else
                    {
                        throw new ApplicationException($"Bug: The menuitem tag {tsmi.Tag} for menuitem {tsmi.Text} did not match any DurationType Enums.");
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

            MovingAverageWrapper maw = m_maCollection[e.DurationType];

            LabelHelper l = maw.LabelHelper;
            Collector c = l.Collector;

            switch (c.FieldAvgType)
            {
                case FieldUomType.Watts:
                    l.AvgPower.Text = e.AveragePower.ToString();
                    break;

                case FieldUomType.Wkg:
                    if (m_currentUser.WeightAsKgs > 0)
                    {
                        double wkg = e.AveragePower / m_currentUser.WeightAsKgs;
                        l.AvgPower.Text = wkg.ToString("#.0#");
                    }
                    break;
            }

            l.AvgHR.Text = e.AverageHR.ToString();

            // The FTP column will track the AvgPower until the time duration is satisfied.
            // This enables the rider to see what his FTP would be real-time.
            // Once the time duration is satisfied, we no longer will update using the AvgPower.
            if (!l.MaxDurationTriggered)
            {
                switch (c.FieldFtpType)
                {
                    case FieldUomType.Watts:
                        l.FtpPower.Text = Convert.ToInt32(e.AveragePower * 0.95).ToString();
                        break;

                    case FieldUomType.Wkg:
                        if (m_currentUser.WeightAsKgs > 0)
                        {
                            double wkg = (e.AveragePower / m_currentUser.WeightAsKgs) * 0.95;
                            l.FtpPower.Text = wkg.ToString("#.00");
                        }
                        break;
                }

            }

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

            MovingAverageWrapper maw = m_maCollection[e.DurationType];

            LabelHelper l = maw.LabelHelper;
            Collector c = l.Collector;

            switch (c.FieldAvgMaxType)
            {
                case FieldUomType.Watts:
                    l.MaxPower.Text = e.MaxAvgPower.ToString();
                    break;

                case FieldUomType.Wkg:
                    if (m_currentUser.WeightAsKgs > 0)
                    {
                        double wkg = e.MaxAvgPower / m_currentUser.WeightAsKgs;
                        l.MaxPower.Text = wkg.ToString("#.00");
                    }
                    break;
            }

            // Save the fact that this moving average has fulfilled it's time duration
            l.MaxDurationTriggered = true;

            // The FTP column will now track the MaxAvgPower now that the time duration is satisfied.
            switch (c.FieldFtpType)
            {
                case FieldUomType.Watts:
                    l.FtpPower.Text = Convert.ToInt32(e.MaxAvgPower * 0.95).ToString();
                    break;

                case FieldUomType.Wkg:
                    if (m_currentUser.WeightAsKgs > 0)
                    {
                        double wkg = (e.MaxAvgPower / m_currentUser.WeightAsKgs) * 0.95;
                        l.FtpPower.Text = wkg.ToString("#.00");
                    }
                    break;
            }
        }


        /// <summary>
        /// A delegate used solely by the MovingAverageChangedEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void NormalizedPowerChangedEventHandlerDelegate(object sender, NormalizedPower.NormalizedPowerChangedEventArgs e);

        private double m_currentIf;
        private int m_currentNp;
        private double m_currentKph;
        private double m_currentMph;
        private int m_currentAp;


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
                m_currentIf = Math.Round(e.NormalizedPower / (double)m_currentUser.PowerThreshold, 2);
            }
            else
            {
                m_currentIf = 0;
            }

            m_currentNp = e.NormalizedPower;

            UpdateOverallValues();
        }

        private void UpdateOverallValues()
        {
            tsslOverall.Text = $"AP: {m_currentAp}{(m_currentNp > 0 ? ", NP: " + m_currentNp : "")}{(m_currentIf > 0 ? ", IF: " + m_currentIf.ToString("#.00") : "")}{(m_currentKph > 0 ? ", KPH: " + m_currentKph.ToString("#.0") : "")}";
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

            m_currentKph = e.AverageKph;
            m_currentMph = e.AverageMph;
            m_currentAp = e.OverallPower;

            UpdateOverallValues();

            //m_logger.LogInformation($"Average speed: KPH {e.AverageKph}, MPH {e.AverageMph}");
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

            //m_logger.LogInformation($"Time remaining: {ts.Minutes}:{ts.Seconds}");

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

            //m_logger.LogInformation($"Time running: {ts.Minutes}:{ts.Seconds}");

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

        private void MonitorStatistics_Move(object sender, EventArgs e)
        {
            m_logger.LogInformation($"Screen position {this.Location.ToString()}");
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
            //var form = m_serviceProvider.GetService<ConfigurationOptions>();

            var form = new ConfigurationOptions(m_loggerFactory, m_serviceProvider, this.Location);

            DialogResult result = form.ShowDialog(this);

            SetupCurrentUser();

            // Allow menus and status bar to update according to what user just did
            OnCollectionStatusChanged();
        }
    }
}
