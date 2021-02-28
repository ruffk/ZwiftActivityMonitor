using System;
using System.Windows.Threading;
using System.Windows.Forms;
using Dapplo.Microsoft.Extensions.Hosting.WinForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;
using ZwiftPacketMonitor;
using System.Threading;
using System.Threading.Tasks;

namespace ZwiftActivityMonitor
{
    public partial class MonitorStatistics : Form, IWinFormsShell
    {
        private readonly ILogger<MonitorStatistics> m_logger;
        private readonly IConfiguration m_configuration;
        private readonly IServiceProvider m_serviceProvider;
        private readonly ZPMonitorService m_zpMonitorService;
        private readonly ILoggerFactory m_loggerFactory;

        private readonly List<LabelHelper> m_labelHelpers;
        private readonly Dictionary<DurationType, MovingAverageWrapper> m_maCollection;
        private readonly Dictionary<string, string> m_labelUnits;
        private readonly Dictionary<DurationType, Collector> m_collectors;

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
            public LabelHelper LabelHelper {  get { return m_labelHelper; } }
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
            private Label m_lblMA;
            private Collector m_collector;

            public LabelHelper(Label lblMA, Label lblAvgPower, Label lblMaxPower, Label lblFtpPower, Label lblAvgHR)
            {
                m_lblAvgPower = lblAvgPower;
                m_lblMaxPower = lblMaxPower;
                m_lblFtpPower = lblFtpPower;
                m_lblAvgHR = lblAvgHR;
                m_lblMA = lblMA;
            }

            public void ClearLabels()
            {
                m_lblAvgPower.Text = "";
                m_lblMaxPower.Text = "";
                m_lblFtpPower.Text = "";
                m_lblAvgHR.Text = "";
                m_lblMA.Text = "";
            }

            public Label AvgPower { get { return m_lblAvgPower; } }
            public Label MaxPower { get { return m_lblMaxPower; } }
            public Label AvgHR { get { return m_lblAvgHR; } }
            public Label FtpPower { get { return m_lblFtpPower; } }
            public Label MA { get { return m_lblMA; } }
            public Collector Collector
            {
                get { return m_collector; }
                set { m_collector = value; }
            }
        }
        
        /// <summary>
        /// Represents the MovingAverage:Collector configuration section
        /// </summary>
        internal class Collector
        {
            private string m_label;
            private DurationType m_type;
            private bool m_displayDefault;
            private FieldUomType m_fieldAvg;
            private FieldUomType m_fieldAvgMax;
            private FieldUomType m_fieldFtp;
            private bool m_maxDurationTriggered;

            public enum FieldUomType
            {
                None,
                Watts,
                Wkg
            }

            public Collector(string durationLabel, string displayDefault, string fieldAvgUom, string fieldAvgMaxUom, string fieldFtpUom)
            {
                m_label = durationLabel;

                // Find DurationType based upon a duration label (5 sec, 1 min, 5 min, etc.)
                m_type = MovingAverage.GetType(durationLabel);

                if (!bool.TryParse(displayDefault, out m_displayDefault))
                    throw new ArgumentException($"Invalid display default: {displayDefault}, Use [true | false]");

                if (!Enum.TryParse<FieldUomType>(fieldAvgUom, true, out m_fieldAvg))
                    throw new ArgumentException($"Invalid FieldAvgUom: {fieldAvgUom}, Use [None | Watts | Wkg]");
                
                if (!Enum.TryParse<FieldUomType>(fieldAvgMaxUom, true, out m_fieldAvgMax))
                    throw new ArgumentException($"Invalid FieldAvgMaxUom: {fieldAvgMaxUom}, Use [None | Watts | Wkg]");

                if (!Enum.TryParse<FieldUomType>(fieldFtpUom, true, out m_fieldFtp))
                    throw new ArgumentException($"Invalid fieldFtpUom: {fieldFtpUom}, Use [None | Watts | Wkg]");
            }

            public string Label { get { return m_label; } }
            public DurationType Type { get { return m_type; } }
            public bool DisplayDefault { get { return m_displayDefault; } }
            public FieldUomType FieldAvg { get { return m_fieldAvg; } }
            public FieldUomType FieldAvgMax { get { return m_fieldAvgMax; } }
            public FieldUomType FieldFtp { get { return m_fieldFtp; } }
            public bool MaxDurationTriggered { get { return m_maxDurationTriggered; } set { m_maxDurationTriggered = value; } }
        }
        #endregion

        private Dispatcher m_dispatcher; // Current UI thread dispatcher, for marshalling UI calls

        private DateTime m_timerCompletion; // Time when timer countdown should complete
        private DateTime m_collectionStart; // Time when monitor run started
        private double m_ZwifterWeightKgs; // Zwifter weight from configuration
        private bool m_zpMonitorAutoStart; // Whether to attempt to auto start the ZwiftPacketMonitor
        private bool m_isStarted;           // Whether the collectors are currently running


        public MonitorStatistics(IServiceProvider serviceProvider, IConfiguration configuration, ZPMonitorService zpMonitorService, ILoggerFactory loggerFactory)
        {
            m_logger = loggerFactory.CreateLogger<MonitorStatistics>(); ;
            m_serviceProvider = serviceProvider;
            m_configuration = configuration;
            m_zpMonitorService = zpMonitorService;
            m_loggerFactory = loggerFactory;

            m_maCollection = new Dictionary<DurationType, MovingAverageWrapper>();
            m_labelUnits = new Dictionary<string, string>();
            m_collectors = new Dictionary<DurationType, Collector>();

            m_labelHelpers = new List<LabelHelper>();

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
            m_dispatcher = Dispatcher.CurrentDispatcher;

            // Determine AutoStart
            if (!bool.TryParse(m_configuration["ZwiftPacketMonitor:AutoStart"], out m_zpMonitorAutoStart))
            {
                m_zpMonitorAutoStart = false;
            }

            m_logger.LogInformation($"AutoStart of ZwiftPacketMonitor is {m_zpMonitorAutoStart}");

            // Determine window position
            if (!int.TryParse(m_configuration["ZwiftActivityMonitor:WindowStartupPosition:X"], out int xPos))
            {
                xPos = 0;
            }

            if (!int.TryParse(m_configuration["ZwiftActivityMonitor:WindowStartupPosition:Y"], out int yPos))
            {
                yPos = 0;
            }

            if (xPos > 0 && yPos > 0)
            {
                this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                this.Location = new System.Drawing.Point(xPos, yPos);
            }


            m_labelHelpers.Add(new LabelHelper(lblMA1, lblAvgPower1, lblMaxPower1, lblFtpPower1, lblAvgHR1));
            m_labelHelpers.Add(new LabelHelper(lblMA2, lblAvgPower2, lblMaxPower2, lblFtpPower2, lblAvgHR2));
            m_labelHelpers.Add(new LabelHelper(lblMA3, lblAvgPower3, lblMaxPower3, lblFtpPower3, lblAvgHR3));


            double weight = Convert.ToDouble(m_configuration["ZwiftActivityMonitor:Weight"]);
            string uom = m_configuration["ZwiftActivityMonitor:UnitOfMeasure"];

            switch(uom)
            {
                case "kgs":
                    m_ZwifterWeightKgs = weight;
                    break;

                case "lbs":
                    m_ZwifterWeightKgs = weight / 2.205;
                    break;
            }

            m_logger.LogInformation($"Weight: {m_ZwifterWeightKgs.ToString("#.##")} kgs");

            foreach (var child in m_configuration.GetSection("MovingAverage:Collector").GetChildren())
            {
                m_logger.LogInformation($"MovingAverage:Collector Duration: {child["Duration"]} Display: {child["Display"]}");

                // A Collector captures what is defined in configuration and provides helper methods.
                Collector c = new Collector(child["Duration"], child["Display"], child["FieldAvgUom"], child["FieldAvgMaxUom"], child["FieldFtpUom"]);
                m_collectors.Add(c.Type, c);

                if (c.DisplayDefault)
                {
                    // check the menu items based on configuration
                    foreach (ToolStripItem mi in tsmiAnalyze.DropDownItems)
                    {
                        ToolStripMenuItem tsmi = mi as ToolStripMenuItem;
                        if (tsmi == null) continue;

                        if (tsmi.Text.ToLower() == c.Label.ToLower())
                        {
                            tsmi.Checked = true;
                            break;
                        }
                    }
                }
            }


            m_logger.LogInformation("MonitorStatistics Loaded");

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
            // Set control statuses
            OnCollectionStatusChanged();

            // Load collectors for whatever is defined in by the checked menu items
            LoadMovingAverageCollection();

            // if autostart is false, bring up the AdvancedOptions window.
            if (!m_zpMonitorAutoStart)
            {
                tsmiAdvanced.PerformClick();
            }

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
                m_isStarted = false;

                runTimer.Enabled = false;

                OnCollectionStatusChanged();
            }

        }

        private void OnCollectionStatusChanged()
        {
            if (m_isStarted)
            {
                tsmiStop.Enabled = true;
                tsmiStart.Enabled = false;

                tsmi10min.Enabled = false;
                tsmi1min.Enabled = false;
                tsmi20min.Enabled = false;
                tsmi5min.Enabled = false;
                tsmi5sec.Enabled = false;
                tsmi60min.Enabled = false;
                tsmi90min.Enabled = false;

                tsmiTimer.Enabled = false;

                tsslStatus.Text = "Running";

                if (m_zpMonitorService.IsStarted && m_zpMonitorService.IsDebugMode)
                    tsslStatus.Text += " in DEBUG/DEMO mode"
            }
            else
            {
                tsmiStop.Enabled = false;
                tsmiStart.Enabled = true;

                tsmi10min.Enabled = true;
                tsmi1min.Enabled = true;
                tsmi20min.Enabled = true;
                tsmi5min.Enabled = true;
                tsmi5sec.Enabled = true;
                tsmi60min.Enabled = true;
                tsmi90min.Enabled = true;

                tsmiTimer.Enabled = true;

                // set Timer menu sub-items
                if (countdownTimer.Enabled)
                {
                    tsmiSetupTimer.Enabled = false;
                    tsmiStopTimer.Enabled = true;
                }
                else
                {
                    tsmiSetupTimer.Enabled = true;
                    tsmiStopTimer.Enabled = false;
                }

                if (m_zpMonitorService.IsStarted)
                    tsslStatus.Text = "Ready";
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

            foreach (LabelHelper lh in m_labelHelpers)
                lh.ClearLabels();

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
                        MovingAverage ma = new MovingAverage(m_zpMonitorService, m_loggerFactory, result);

                        ma.MovingAverageChangedEvent += MovingAverageChangedEventHandler;
                        ma.MovingAverageMaxChangedEvent += MovingAverageMaxChangedEventHandler;

                        // A m_labelHelpers[labelSet] object represents a row on the statistics display
                        int labelSet = m_maCollection.Count;

                        if (labelSet < m_labelHelpers.Count)
                        {
                            m_maCollection.Add(result, new MovingAverageWrapper(ma, m_labelHelpers[labelSet]));

                            // Here we row's id text (5 sec, 1 min, etc) and associate the matching Collector.
                            // All of this makes it easy to update the display as the MovingAverage events fire.
                            m_labelHelpers[labelSet].MA.Text = tsmi.Text;
                            m_labelHelpers[labelSet].Collector = m_collectors[result];

                            if (m_maCollection.Count >= 3) // only allow up to 3 collectors
                                break;
                        }
                    }
                    else
                    {
                        throw new ApplicationException($"Bug: The menuitem tag {tsmi.Tag} did not match any DurationType Enums.");
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

            switch (c.FieldAvg)
            {
                case Collector.FieldUomType.Watts:
                    l.AvgPower.Text = e.AveragePower.ToString();
                    break;

                case Collector.FieldUomType.Wkg:
                    if (m_ZwifterWeightKgs > 0)
                    {
                        double wkg = e.AveragePower / m_ZwifterWeightKgs;
                        l.AvgPower.Text = wkg.ToString("#.0#");
                    }
                    break;
            }

            l.AvgHR.Text = e.AverageHR.ToString();

            // The FTP column will track the AvgPower until the time duration is satisfied.
            // This enables the rider to see what his FTP would be real-time.
            // Once the time duration is satisfied, we no longer will update using the AvgPower.
            if (!c.MaxDurationTriggered)
            {
                switch (c.FieldFtp)
                {
                    case Collector.FieldUomType.Watts:
                        l.FtpPower.Text = Convert.ToInt32(e.AveragePower * 0.95).ToString();
                        break;

                    case Collector.FieldUomType.Wkg:
                        if (m_ZwifterWeightKgs > 0)
                        {
                            double wkg = (e.AveragePower / m_ZwifterWeightKgs) * 0.95;
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

            switch (c.FieldAvgMax)
            {
                case Collector.FieldUomType.Watts:
                    l.MaxPower.Text = e.MaxAvgPower.ToString();
                    break;

                case Collector.FieldUomType.Wkg:
                    if (m_ZwifterWeightKgs > 0)
                    {
                        double wkg = e.MaxAvgPower / m_ZwifterWeightKgs;
                        l.MaxPower.Text = wkg.ToString("#.00");
                    }
                    break;
            }

            // Save the fact that this moving average has fulfilled it's time duration
            c.MaxDurationTriggered = true;

            // The FTP column will now track the MaxAvgPower now that the time duration is satisfied.
            switch (c.FieldFtp)
            {
                case Collector.FieldUomType.Watts:
                    l.FtpPower.Text = Convert.ToInt32(e.MaxAvgPower * 0.95).ToString();
                    break;

                case Collector.FieldUomType.Wkg:
                    if (m_ZwifterWeightKgs > 0)
                    {
                        double wkg = (e.MaxAvgPower / m_ZwifterWeightKgs) * 0.95;
                        l.FtpPower.Text = wkg.ToString("#.00");
                    }
                    break;
            }
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
        #endregion

        private void runTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = DateTime.Now - m_collectionStart;

            //m_logger.LogInformation($"Time running: {ts.Minutes}:{ts.Seconds}");

            tsslStatus.Text = "Running time: " + ts.Hours.ToString("0#") + ":" + ts.Minutes.ToString("0#") + ":" + ts.Seconds.ToString("0#");
        }

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
    }
}
