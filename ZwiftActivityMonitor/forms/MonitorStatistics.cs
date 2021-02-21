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
            private Label m_lblAvgHR;
            private Label m_lblMaxHR;
            private Label m_lblMA;

            public LabelHelper(Label lblMA, Label lblAvgPower, Label lblMaxPower, Label lblAvgHR, Label lblMaxHR)
            {
                m_lblAvgPower = lblAvgPower;
                m_lblMaxPower = lblMaxPower;
                m_lblAvgHR    = lblAvgHR;
                m_lblMaxHR    = lblMaxHR;
                m_lblMA       = lblMA;
            }

            public void ClearLabels()
            {
                m_lblAvgPower.Text = "";
                m_lblMaxPower.Text = "";
                m_lblAvgHR.Text = "";
                m_lblMaxHR.Text = "";
                m_lblMA.Text = "";
            }

            public Label AvgPower { get { return m_lblAvgPower; } }
            public Label MaxPower { get { return m_lblMaxPower; } }
            public Label AvgHR { get { return m_lblAvgHR; } }
            public Label MaxHR { get { return m_lblMaxHR; } }
            public Label MA { get { return m_lblMA; } }
        }
        #endregion

        private List<LabelHelper> m_labelHelpers;

        private Dictionary<MovingAverage.DurationTypes, MovingAverageWrapper> m_maCollection;

        private Dispatcher m_dispatcher;

        private DateTime m_timerCompletion;

        public MonitorStatistics(ILogger<MonitorStatistics> logger, IServiceProvider serviceProvider, IConfiguration configuration, ZPMonitorService zpMonitorService)
        {
            m_logger = logger;
            m_serviceProvider = serviceProvider;
            m_configuration = configuration;
            m_zpMonitorService = zpMonitorService;
            m_maCollection = new Dictionary<MovingAverage.DurationTypes, MovingAverageWrapper>();
            m_labelHelpers = new List<LabelHelper>();

            InitializeComponent();
        }
        private void MonitorStatistics_Load(object sender, EventArgs e)
        {
            m_dispatcher = Dispatcher.CurrentDispatcher;

            m_labelHelpers.Add(new LabelHelper(lblMA1, lblAvgPower1, lblMaxPower1, lblAvgHR1, lblMaxHR1));
            m_labelHelpers.Add(new LabelHelper(lblMA2, lblAvgPower2, lblMaxPower2, lblAvgHR2, lblMaxHR2));
            m_labelHelpers.Add(new LabelHelper(lblMA3, lblAvgPower3, lblMaxPower3, lblAvgHR3, lblMaxHR3));

            foreach (var child in m_configuration.GetSection("MovingAverage:Collect").GetChildren())
            {
                m_logger.LogInformation($"MovingAverage:Collect Key: {child.Key} Value: {child.Value}");

                if (child.Value == "true")
                {
                    // check the menu items based on configuration
                    foreach (ToolStripItem mi in tsmiCollect.DropDownItems)
                    {
                        ToolStripMenuItem tsmi = mi as ToolStripMenuItem;
                        if (tsmi == null) continue;

                        if (tsmi.Text == child.Key)
                            tsmi.Checked = true;
                    }
                }
            }


            m_logger.LogInformation("MonitorStatistics Loaded");

        }

        private void MonitorStatistics_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                tsmiStart.Enabled = true;
                tsmiStop.Enabled = false;

                tsmiStopTimer.Enabled = false;

                // Load collectors for whatever is defined in by the checked menu items
                LoadMovingAverageCollection();

                m_logger.LogInformation("MonitorStatistics Visible");
            }
        }


        private void tsmiStart_Click(object sender, EventArgs e)
        {
            OnStart();
        }

        private void OnStart()
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

        private void tsmiStop_Click(object sender, EventArgs e)
        {
            OnStop();
        }

        private void OnStop()
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

            tsslStatus.Text = "Ready";
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            foreach (MovingAverageWrapper maw in m_maCollection.Values)
            {
                maw.MovingAverage.Stop();
            }

            m_maCollection.Clear();
            this.Hide();

            m_logger.LogInformation("Closing");
        }


        private void tsmiCollect_DropDownOpening(object sender, EventArgs e)
        {

        }

        private void tsmiCollect_DropDownClosed(object sender, EventArgs e)
        {
        }

        private void anyDuration_Click(object sender, EventArgs e)
        {
            // The checked status for some item has changed.
            LoadMovingAverageCollection();
        }

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
                        }
                    }
                }
            }

        }

        private delegate void MovingAverageChangedEventHandlerDelegate(object sender, MovingAverage.MovingAverageChangedEventArgs e);

        private void MovingAverageChangedEventHandler(object sender, MovingAverage.MovingAverageChangedEventArgs e)
        {
            if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                m_dispatcher.BeginInvoke(new MovingAverageChangedEventHandlerDelegate(MovingAverageChangedEventHandler), new object[] { sender, e });
                return;
            }

            MovingAverageWrapper maw = m_maCollection[e.DurationType];

            maw.LabelHelper.AvgPower.Text = e.AveragePower.ToString();
            maw.LabelHelper.AvgHR.Text = e.AverageHR.ToString();
        }


        private delegate void MovingAverageMaxChangedEventHandlerDelegate(object sender, MovingAverage.MovingAverageMaxChangedEventArgs e);

        private void MovingAverageMaxChangedEventHandler(object sender, MovingAverage.MovingAverageMaxChangedEventArgs e)
        {
            if (!m_dispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                m_dispatcher.BeginInvoke(new MovingAverageMaxChangedEventHandlerDelegate(MovingAverageMaxChangedEventHandler), new object[] { sender, e });
                return;
            }

            MovingAverageWrapper maw = m_maCollection[e.DurationType];

            maw.LabelHelper.MaxPower.Text = e.MaxAvgPower.ToString();
            maw.LabelHelper.MaxHR.Text = e.MaxAvgHR.ToString();

        }

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

            }
        }

        private void countdownTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = m_timerCompletion - DateTime.Now;

            m_logger.LogInformation($"Time remaining: {ts.Minutes}:{ts.Seconds}");

            if (ts.TotalSeconds <= 0)
            {
                countdownTimer.Enabled = false;
                m_logger.LogInformation($"Go! Go! Go!");
                
                OnStart();
            }
            else
            {
                tsslStatus.Text = "Time Remaining: " + ts.Minutes.ToString("0#") + ":" + ts.Seconds.ToString("0#"); 
            }

        }

        private void tsmiStopTimer_Click(object sender, EventArgs e)
        {
            OnStopTimer();
        }

        private void OnStopTimer()
        {
            countdownTimer.Enabled = false;
            tsmiStopTimer.Enabled = false;

            tsslStatus.Text = "Ready";

        }

    }
}
