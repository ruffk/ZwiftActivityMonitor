using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;


namespace ZwiftActivityMonitorV2
{
    public partial class TimerSetupViewerControl : ViewerControlEx
    {
        private TimeSpan TimeRemaining { get; set; }
        public bool IsTimerRunning { get; set; }
        private TimeSpan TimerDefault { get; } = new TimeSpan(0, 0, 30);
        private DateTime TimerEndTime { get; set; }
        private System.Threading.Timer autoStartTimer;
        //private System.Windows.Forms.Timer autoStartFormTimer = new();

        public event EventHandler<CountdownTimerTickEventArgs> CountdownTimerTickEvent;
        //private SynchronizationContext UISyncContext;
        private ILogger<TimerSetupViewerControl> Logger;

        private Dispatcher mDispatcher;


        public TimerSetupViewerControl()
        {
            InitializeComponent();

            if (this.DesignMode)
                return;

            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<TimerSetupViewerControl>();

            this.autoStartTimer = new(OnAutoStartTimerCallback);

            //this.autoStartFormTimer.Tick += autoStartFormTimer_Tick;
            //this.autoStartFormTimer.Interval = 1000;

            this.dtpTimeRemaining.Value = new DateTime(2021, 1, 1, TimerDefault.Hours, TimerDefault.Minutes, TimerDefault.Seconds);
        }

        private void TimerSetupViewerControl_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;

            //this.UISyncContext = WindowsFormsSynchronizationContext.Current;
            mDispatcher = Dispatcher.CurrentDispatcher;

            if (this.ParentForm != null)
            {
                //(this.ParentForm as MainForm).FormSyncOneSecondTimerTickEvent += TimerSetupViewerControl_FormSyncOneSecondTimerTickEvent;
            }


            ZAMsettings.ZPMonitorService.ZPMonitorServiceStatusChanged += ZPMonitorService_ZPMonitorServiceStatusChanged;
            ZAMsettings.ZPMonitorService.CollectionStatusChanged += ZPMonitorService_CollectionStatusChanged;
        }


        public void StartTimer()
        {
            this.autoStartTimer.Change(1000, 1000);
            this.IsTimerRunning = true;
            //this.autoStartFormTimer.Start();
        }

        private void StopTimer()
        {
            this.autoStartTimer.Change(Timeout.Infinite, Timeout.Infinite);
            this.IsTimerRunning = false;
            //this.autoStartFormTimer.Stop();
        }

        private void OnAutoStartTimerCallback(object state)
        {
            bool isCompleted = false;
            
            int seconds = (int)Math.Round((this.TimerEndTime - DateTime.Now).TotalSeconds, 0);
            this.TimeRemaining = new TimeSpan(0, 0, seconds);

            //Logger.LogDebug($"OnAutoStartTimerCallback1 - ID: {Thread.CurrentThread.ManagedThreadId}, TimeRemaining: {this.TimeRemaining.Seconds}, Seconds: {seconds}");

            if (seconds <= 0)
            {
                this.StopTimer();
                isCompleted = true;

                OnCountdownTimerTickEvent(new CountdownTimerTickEventArgs() { StartWithEventTimer = rbStartWithEventTimer.Checked, IsCompleted = true }) ;
            }
            else
            {
                OnCountdownTimerTickEvent(new CountdownTimerTickEventArgs(this.TimeRemaining));
            }

            if (isCompleted)
            {
                this.SetViewDisplayStatus();
            }
            //Logger.LogDebug($"OnAutoStartTimerCallback2 - ID: {Thread.CurrentThread.ManagedThreadId}");

        }

        /// <summary>
        /// Event raised when countdown tick occurs, timer is canceled, or timer is done.
        /// </summary>
        /// <param name="e"></param>
        private void OnCountdownTimerTickEvent(CountdownTimerTickEventArgs e)
        {
            EventHandler<CountdownTimerTickEventArgs> handler = CountdownTimerTickEvent;

            if (handler != null)
            {
                try
                {
                    handler(this, e);
                }
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnCountdownTimerTickEvent)");
                }
            }
        }

        private void ZPMonitorService_CollectionStatusChanged(object sender, CollectionStatusChangedEventArgs e)
        {
            switch(e.Action)
            {
                case CollectionStatusChangedEventArgs.ActionType.Waiting:
                case CollectionStatusChangedEventArgs.ActionType.Started:
                    if (this.IsTimerRunning)
                    {
                        // make sure timer is stopped
                        this.StopTimer();
                        OnCountdownTimerTickEvent(new CountdownTimerTickEventArgs() { IsCanceled = true });
                    }
                    break;
            }

            this.SetViewDisplayStatus();
        }

        private void ZPMonitorService_ZPMonitorServiceStatusChanged(object sender, ZPMonitorServiceStatusChangedEventArgs e)
        {
            this.SetViewDisplayStatus();
        }


        /// <summary>
        /// A delegate used solely by the SetViewDisplayStatus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void SetViewDisplayStatusDelegate();

        private void SetViewDisplayStatus()
        {
            if (!mDispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                mDispatcher.BeginInvoke(new SetViewDisplayStatusDelegate(SetViewDisplayStatus), new object[] { });
                return;
            }

            if (!ZAMsettings.ZPMonitorService.IsZPMonitorStarted || ZAMsettings.ZPMonitorService.IsCollectionStarted || ZAMsettings.ZPMonitorService.IsCollectionStartWaiting)
            {
                this.dtpTimeRemaining.Enabled = false;
                this.btnStart.Enabled = false;
                this.rbStartImmediately.Enabled = false;
                this.rbStartWithEventTimer.Enabled = false;

                if (!ZAMsettings.ZPMonitorService.IsZPMonitorStarted)
                {
                    lblSettingsDisabled.Text = "Timer disabled until ZPM is started";
                }
                else if (ZAMsettings.ZPMonitorService.IsCollectionStarted || ZAMsettings.ZPMonitorService.IsCollectionStartWaiting)
                {
                    lblSettingsDisabled.Text = "Timer disabled while monitoring is in progress";
                }

                this.lblSettingsDisabled.Visible = true;
            }
            else
            {
                this.lblSettingsDisabled.Visible = false;

                this.dtpTimeRemaining.Enabled = !this.IsTimerRunning;
                this.btnStart.Text = this.IsTimerRunning ? "Stop Timer" : "Start Timer";
                this.btnStart.Enabled = true;
                this.rbStartImmediately.Enabled = !this.IsTimerRunning;
                this.rbStartWithEventTimer.Enabled = !this.IsTimerRunning;
            }
        }

        public override void ControlGainingFocus(object sender, EventArgs e)
        {
            this.Focus();
            this.SetViewDisplayStatus();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.IsTimerRunning = !this.IsTimerRunning;

            if (this.IsTimerRunning)
            {
                this.TimerEndTime = DateTime.Now + dtpTimeRemaining.Value.TimeOfDay;
                this.StartTimer();
            }
            else
            {
                this.StopTimer();
                OnCountdownTimerTickEvent(new CountdownTimerTickEventArgs() { IsCanceled=true });
            }

            this.SetViewDisplayStatus();
        }

        private void dtpTimeRemaining_Enter(object sender, EventArgs e)
        {
        }
    }
}
