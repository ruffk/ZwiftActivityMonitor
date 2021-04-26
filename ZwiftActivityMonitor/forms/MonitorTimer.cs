using System;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitor
{
    public partial class MonitorTimer : Form
    {
        private readonly ILogger<MonitorTimer> Logger;

        public MonitorTimer(ILogger<MonitorTimer> logger)
        {
            Logger = logger;

            InitializeComponent();
        }

        public int Minutes
        {
            get { return ucTimerSetup.Minutes; }
        }
        public int Seconds
        {
            get { return ucTimerSetup.Seconds; }
        }

        public bool StartWithEventTimer
        {
            get { return ucTimerSetup.StartWithEventTimer; }
        }
      


        private void btnStart_Click(object sender, EventArgs e)
        {
            if (ucTimerSetup.ValidateChildren())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
