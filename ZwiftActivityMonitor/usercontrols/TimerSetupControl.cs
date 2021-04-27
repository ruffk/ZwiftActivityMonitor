using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitor
{
    public partial class TimerSetupControl : UserControlWithStatusBase
    {
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public bool StartImmediately { get; set; }
        public bool StartWithEventTimer { get; set; }

        public TimerSetupControl()
        {
            InitializeComponent();
        }

        #region Base class overrides for event selection
        protected override void SkipControl_Enter(object sender, EventArgs e)
        {
            base.SkipControl_Enter(sender, e);
        }

        private void SystemSettings_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = ValidateSystemSettings(sender as Control);
        }

        private bool ValidateSystemSettings(Control control)
        {
            bool errorOccurred = false;

            errorProvider.SetError(control, "");

            switch (control.Name)
            {
                case "nMins":
                    try
                    {
                        this.Minutes = Convert.ToInt32(nMins.Value);

                        if (this.Minutes < 0 || this.Minutes > 59)
                        {
                            throw new FormatException("Minutes must be in range [0..59].");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "nSecs":
                    try
                    {
                        this.Seconds = Convert.ToInt32(nSecs.Value);

                        if (this.Seconds < 0 || this.Seconds > 59)
                        {
                            throw new FormatException("Seconds must be in range [0..59].");
                        }
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "rbStartImmediately":
                    this.StartImmediately = rbStartImmediately.Checked;
                    break;

                case "rbStartWithEventTimer":
                    this.StartWithEventTimer = rbStartWithEventTimer.Checked;
                    break;

                default:
                    Debug.Assert(1 == 0, $"Unknown control {control.Name} passed to validate method.");
                    break;
            }

            return errorOccurred;
        }

        public void SystemSettings_TooltipOnEnter(object sender, EventArgs e)
        {
            HandleTooltipsSystemSettings(sender as Control, true);
        }
        public void SystemSettings_TooltipOnLeave(object sender, EventArgs e)
        {
            HandleTooltipsSystemSettings(sender as Control, false);
        }

        public void HandleTooltipsSystemSettings(Control control, bool isEntering)
        {
            if (!isEntering)
            {
                toolStripStatusLabel.Text = "";
                return;
            }

            switch (control.Name)
            {
                case "nMins":
                    toolStripStatusLabel.Text = "Enter timer duration minutes.";
                    break;

                case "nSecs":
                    toolStripStatusLabel.Text = "Enter timer duration seconds.";
                    break;
            }

        }

        protected override void UserControlBase_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            this.Logger = ZAMsettings.LoggerFactory.CreateLogger<TimerSetupControl>();

            base.UserControlBase_Load(sender, e);
        }

        #endregion

        private void btnStart_Click(object sender, EventArgs e)
        {
            bool errorOccurred = false;

            errorOccurred = (errorOccurred || ValidateSystemSettings(nMins));
            errorOccurred = (errorOccurred || ValidateSystemSettings(nSecs));
            errorOccurred = (errorOccurred || ValidateSystemSettings(rbStartImmediately));
            errorOccurred = (errorOccurred || ValidateSystemSettings(rbStartWithEventTimer));

            if (!errorOccurred)
            {
                
            }
        }
    }
}
