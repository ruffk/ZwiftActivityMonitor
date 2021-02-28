using System;
using System.Windows.Forms;
using Dapplo.Microsoft.Extensions.Hosting.WinForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Threading;

namespace ZwiftActivityMonitor
{
    public partial class MainForm : Form, IWinFormsShell
    {
        private readonly ILogger<MainForm> m_logger;
        private readonly IConfiguration m_configuration;
        private readonly IServiceProvider m_serviceProvider;
        private readonly ZPMonitorService m_zpMonitorService;

        private MonitorStatistics m_monitorStatistics;

        public MainForm(ILogger<MainForm> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this.m_logger = logger;
            this.m_serviceProvider = serviceProvider;
            this.m_configuration = configuration;

            this.m_zpMonitorService = serviceProvider.GetService<ZPMonitorService>();

            InitializeComponent();

            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            btnMonitorStatistics.Enabled = false;
        }

        private void btnRunMonitor_Click(object sender, EventArgs e)
        {
            cbxDebugMode.Enabled = false;
            btnStopMonitor.Enabled = true;
            btnRunMonitor.Enabled = false;

            int targetHR = Convert.ToInt32(tbTargetHR.Text);

            m_logger.LogInformation($"UI Thread: {Thread.CurrentThread.ManagedThreadId}");

            m_zpMonitorService.StartMonitor(cbxDebugMode.Checked, targetHR, 0);

            btnMonitorStatistics.Enabled = true;
        }

        private void btnStopMonitor_Click(object sender, EventArgs e)
        {
            btnStopMonitor.Enabled = false;

            m_zpMonitorService.StopMonitor();

            btnRunMonitor.Enabled = true;
            cbxDebugMode.Enabled = true;
            btnMonitorStatistics.Enabled = false;

            if (m_monitorStatistics != null && !m_monitorStatistics.IsDisposed)
                m_monitorStatistics.Close();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
            //Application.Exit();

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_monitorStatistics != null && !m_monitorStatistics.IsDisposed)
                m_monitorStatistics.Close();
        }


        private void buttonLog_Click(object sender, EventArgs e)
        {
            m_logger.LogDebug($"Debug worked!");
            m_logger.LogInformation($"Info worked!");
            m_logger.LogWarning($"Warning also worked!");
            m_logger.LogError($"Warning also worked!");

            m_logger.LogDebug($"D-Kevin {m_configuration["Kevin"]}");
            m_logger.LogInformation($"I-Kevin {m_configuration["Kevin"]}");
            m_logger.LogWarning($"W-Kevin {m_configuration["Kevin"]}");
            m_logger.LogError($"E-Kevin {m_configuration["Kevin"]}");
        }

        private void btnMonitorStatistics_Click(object sender, EventArgs e)
        {
            if (m_monitorStatistics is null || m_monitorStatistics.IsDisposed)
            {
                m_monitorStatistics = m_serviceProvider.GetService<MonitorStatistics>();
            }

            m_monitorStatistics.Show();
            m_monitorStatistics.Focus();
        }

    }
}
