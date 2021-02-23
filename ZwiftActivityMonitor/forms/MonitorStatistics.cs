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

        #region Internal Classes
        internal class MovingAverageWrapper
        {
            private MovingAverage m_movingAverage;
            private LabelHelper m_labelHelper;

            public MovingAverageWrapper(MovingAverage movingAverage, LabelHelper labelHelper)
            {
                m_movingAverage = movingAverage;
                m_labelHelper = labelHelper;
            }

            public MovingAverage MovingAverage { get { return m_movingAverage; } }
            public LabelHelper LabelHelper {  get { return m_labelHelper; } }
        }

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

        internal class Collector
        {
            private string m_label;
            private MovingAverage.DurationTypes m_type;
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
            public MovingAverage.DurationTypes Type { get { return m_type; } }
            public bool DisplayDefault { get { return m_displayDefault; } }
            public FieldUomType FieldAvg { get { return m_fieldAvg; } }
            public FieldUomType FieldAvgMax { get { return m_fieldAvgMax; } }
            public FieldUomType FieldFtp { get { return m_fieldFtp; } }
            public bool MaxDurationTriggered { get { return m_maxDurationTriggered; } set { m_maxDurationTriggered = value; } }
        }
        #endregion

        private List<LabelHelper> m_labelHelpers;

        private Dictionary<MovingAverage.DurationTypes, MovingAverageWrapper> m_maCollection;
        private Dictionary<string, string> m_labelUnits;
        private Dictionary<MovingAverage.DurationTypes, Collector> m_collectors;

        private Dispatcher m_dispatcher;

        private DateTime m_timerCompletion;
        private double m_ZwifterWeightKgs;

        public MonitorStatistics(ILogger<MonitorStatistics> logger, IServiceProvider serviceProvider, IConfiguration configuration, ZPMonitorService zpMonitorService)
        {
            m_logger = logger;
            m_serviceProvider = serviceProvider;
            m_configuration = configuration;
            m_zpMonitorService = zpMonitorService;
            m_maCollection = new Dictionary<MovingAverage.DurationTypes, MovingAverageWrapper>();
            m_labelUnits = new Dictionary<string, string>();
            m_collectors = new Dictionary<MovingAverage.DurationTypes, Collector>();

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

            m_logger.LogInformation($"Weight: {m_ZwifterWeightKgs} kgs");

            foreach (var child in m_configuration.GetSection("MovingAverage:Collector").GetChildren())
            {
                m_logger.LogInformation($"MovingAverage:Collector Duration: {child["Duration"]} Display: {child["Display"]}");

                Collector c = new Collector(child["Duration"], child["Display"], child["FieldAvgUom"], child["FieldAvgMaxUom"], child["FieldFtpUom"]);
                m_collectors.Add(c.Type, c);

                if (c.DisplayDefault)
                {
                    // check the menu items based on configuration
                    foreach (ToolStripItem mi in tsmiCollect.DropDownItems)
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
                tsmiStart.Enabled = true;
                tsmiStop.Enabled = false;

                tsmiStopTimer.Enabled = false;
                tsmiSetupTimer.Enabled = true;

                // Load collectors for whatever is defined in by the checked menu items
                LoadMovingAverageCollection();

                m_logger.LogInformation("MonitorStatistics Visible");
            }
        }

        private void MonitorStatistics_FormClosing(object sender, FormClosingEventArgs e)
        {
            Collection_OnStop();
 
            m_maCollection.Clear();

            m_logger.LogInformation("Closing");
        }

        #endregion

        #region Menu item events

        private void tsmiStart_Click(object sender, EventArgs e)
        {
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
            Collection_OnStop();

            this.Hide(); // Hide here instead of close to preserve menu choices

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
            foreach (MovingAverageWrapper maw in m_maCollection.Values)
            {
                maw.MovingAverage.Start();
            }

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
        }

        /// <summary>
        /// Stops the data collectors and sets menu item enabled statuses accordingly.
        /// </summary>
        private void Collection_OnStop()
        {
            foreach (MovingAverageWrapper maw in m_maCollection.Values)
            {
                maw.MovingAverage.Stop();
            }

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

            countdownTimer.Enabled = false;

            tsslStatus.Text = "Ready";
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
            // The collector duration is determined by a match between the menu item's tag and the DurationTypes Enum.
            // Up to 3 items can be shown.
            // The label on the UI gets the same text as the menu item.
            foreach (ToolStripItem mi in tsmiCollect.DropDownItems)
            {
                ToolStripMenuItem tsmi = mi as ToolStripMenuItem;
                if (tsmi == null) continue;

                if (tsmi.Checked)
                {
                    MovingAverage.DurationTypes result;
                    if (Enum.TryParse<MovingAverage.DurationTypes>(tsmi.Tag.ToString(), true, out result))
                    {
                        if (m_maCollection.Count >= 3) // only allow up to 3 collectors
                            break;

                        MovingAverage ma = m_serviceProvider.GetService<MovingAverage>();

                        ma.DurationType = result;
                        ma.MovingAverageChangedEvent += MovingAverageChangedEventHandler;
                        ma.MovingAverageMaxChangedEvent += MovingAverageMaxChangedEventHandler;

                        int labelSet = m_maCollection.Count;

                        if (labelSet < m_labelHelpers.Count)
                        {
                            m_maCollection.Add(result, new MovingAverageWrapper(ma, m_labelHelpers[labelSet]));

                            m_labelHelpers[labelSet].MA.Text = tsmi.Text;
                            m_labelHelpers[labelSet].Collector = m_collectors[result];
                        }
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
                    double wkg = e.AveragePower / m_ZwifterWeightKgs;
                    l.AvgPower.Text = wkg.ToString("#.0#");
                    break;
            }

            l.AvgHR.Text = e.AverageHR.ToString();       
            
            if (!c.MaxDurationTriggered)
            {
                switch (c.FieldFtp)
                {
                    case Collector.FieldUomType.Watts:
                        l.FtpPower.Text = Convert.ToInt32(e.AveragePower * 0.95).ToString();
                        break;

                    case Collector.FieldUomType.Wkg:
                        double wkg = (e.AveragePower / m_ZwifterWeightKgs) * 0.95;
                        l.FtpPower.Text = wkg.ToString("#.0#");
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
                    double wkg = e.MaxAvgPower / m_ZwifterWeightKgs;
                    l.MaxPower.Text = wkg.ToString("#.0#");
                    break;
            }

            c.MaxDurationTriggered = true;

            switch (c.FieldFtp)
            {
                case Collector.FieldUomType.Watts:
                    l.FtpPower.Text = Convert.ToInt32(e.MaxAvgPower * 0.95).ToString();
                    break;

                case Collector.FieldUomType.Wkg:
                    double wkg = (e.MaxAvgPower / m_ZwifterWeightKgs) * 0.95;
                    l.FtpPower.Text = wkg.ToString("#.0#");
                    break;
            }


            //l.AvgHR.Text = e.AverageHR.ToString();


            //maw.LabelHelper.MaxPower.Text = e.MaxAvgPower.ToString();
            //maw.LabelHelper.MaxHR.Text = e.MaxAvgHR.ToString();

        }

        #endregion


        #region Timer menu and tick event handling

        private void tsmiSetupTimer_Click(object sender, EventArgs e)
        {
            MonitorTimer mt = m_serviceProvider.GetService<MonitorTimer>();

            DialogResult result = mt.ShowDialog(this);

            if (result == DialogResult.OK)
            {

                m_timerCompletion = DateTime.Now.AddSeconds((mt.Minutes * 60) + mt.Seconds);

                m_logger.LogInformation($"Minutes: {mt.Minutes} Seconds: {mt.Seconds} Completion Time: {m_timerCompletion.ToString()}");

                countdownTimer.Enabled = true;

                tsmiStopTimer.Enabled = true;
                tsmiSetupTimer.Enabled = false;

            }
        }

        private void tsmiStopTimer_Click(object sender, EventArgs e)
        {
            countdownTimer.Enabled = false;

            tsmiStopTimer.Enabled = false;
            tsmiSetupTimer.Enabled = true;

            tsslStatus.Text = "Ready";
        }


        private void countdownTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = m_timerCompletion - DateTime.Now;

            m_logger.LogInformation($"Time remaining: {ts.Minutes}:{ts.Seconds}");

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

    }
}
