using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using System.Windows.Threading;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;

using Syncfusion.Windows.Forms;
using Syncfusion.WinForms.Controls;
using Syncfusion.Windows.Forms.Tools;
using Syncfusion.Windows.Forms.Grid;
using System.Drawing.Drawing2D;

namespace ZwiftActivityMonitorV2
{

    public partial class MainForm : Syncfusion.Windows.Forms.Office2010Form, Dapplo.Microsoft.Extensions.Hosting.WinForms.IWinFormsShell
    {

        private Dispatcher mDispatcher;                            // Current UI thread dispatcher, for marshalling UI calls

        private readonly ILogger<MainForm> Logger;
        private readonly IServiceProvider ServiceProvider;

        private string HomeTitle = "Zwift Activity Monitor";

        private int mSyncFormTimerTickCount;
        private bool mOneTimeInitializationsCompleted;
        private UserProfile CurrentUserProfile { get { return ZAMsettings.Settings.CurrentUser; } }


        public event EventHandler<FormSyncTimerTickEventArgs> FormSyncOneSecondTimerTickEvent;
        public event EventHandler<FormSyncTimerTickEventArgs> FormSyncFiveSecondTimerTickEvent;


        public MainForm(IServiceProvider serviceProvider)
        {

            string[] proSuperScript = { "\u1D3E", "\u1D3F", "\u1D3C" };
            this.HomeTitle += $" {proSuperScript[0]}{proSuperScript[1]}{proSuperScript[2]}";
            
            this.ServiceProvider = serviceProvider;
            this.Logger = ZAMsettings.LoggerFactory.CreateLogger<MainForm>();

            InitializeComponent();

            // Determine window position
            if (ZAMsettings.Settings.WindowPositionX != 0 && ZAMsettings.Settings.WindowPositionY != 0)
            {
                this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                this.Location = new System.Drawing.Point(ZAMsettings.Settings.WindowPositionX, ZAMsettings.Settings.WindowPositionY);
            }

            this.Icon = Properties.Resources.ZAMicon;

            ucColorView.ColorsAndFontChanged += ucColorView_ColorsAndFontChanged;
            ucTimerSetupView.CountdownTimerTickEvent += UcTimerSetupView_CountdownTimerTickEvent; 
            ZAMsettings.ZPMonitorService.CollectionStatusChanged += ZPMonitorService_CollectionStatusChanged;
            ZAMsettings.ZPMonitorService.ZPMonitorServiceStatusChanged += ZPMonitorService_ZPMonitorServiceStatusChanged;
            ZAMsettings.ZPMonitorService.RiderStateEvent += ZPMonitorService_RiderStateEvent;
            ZAMsettings.SystemConfigChanged += ZAMsettings_SystemConfigChanged;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var form = new SplashScreen();
            DialogResult result = form.ShowDialog(this);

            // for handling UI events
            mDispatcher = Dispatcher.CurrentDispatcher;

            // Determine window size
            this.Size = ZAMsettings.Settings.Appearance.WindowSize;

            // toggle the tabs so the first tab gets initialized
            for (int i = this.tabControl.TabPages.Count - 1; i >= 0; i--)
                tabControl.SelectedIndex = i;
            //tabControl.SelectedIndex = 1;
            //tabControl.SelectedIndex = 0;

            this.OnCollectionStatusChanged();  // setup menu items and status labels
            this.SetupDisplayForCurrentUserProfile();

            this.SetControlColors();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            // start general syncronization timer (needs to be done last)
            this.formSyncTimer.Interval = 1000;
            this.formSyncTimer.Enabled = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logger.LogDebug("MainForm_FormClosing");

            if (ZAMsettings.ZPMonitorService.IsCollectionStarted)
            {
                if (MessageBox.Show("Are you sure you wish to stop monitoring and close the application?",
                    "Activity Monitor Running", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            ZAMsettings.ZPMonitorService.StopCollection();
            ZAMsettings.ZPMonitorService.StopMonitor();
        }

        private void ZAMsettings_SystemConfigChanged(object sender, EventArgs e)
        {
            Logger.LogDebug($"{this.GetType()}.ZAMsettings_SystemConfigChanged");

            this.SetupDisplayForCurrentUserProfile();
        }

        private void SetupDisplayForCurrentUserProfile()
        {
            this.tsmiAutoPause.Checked = this.CurrentUserProfile.AutoPause;
        }


        private void ucColorView_ColorsAndFontChanged(object sender, ColorsAndFontChangedEventArgs e)
        {
            SetControlColors();
        }

        private void SetControlColors()
        {
            Logger.LogDebug($"MainForm - SetControlColors");
            ZAMappearance settings = ZAMsettings.Settings.Appearance;

            ZAMappearance.ApplyColorScheme(this);

            Color foreColor = this.ColorTable.FormTextColor;
            Color backColor = this.ColorTable.FormBackground;

            if (settings.TransparencySetting != TransparencyType.NotTransparent)
            {
                foreColor = (settings.TransparencySetting == TransparencyType.TransparentBlackText ? Color.Black : Color.White);
                
                backColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(146)))), ((int)(((byte)(204)))));
                this.TransparencyKey = backColor;
            }
            else
            {
                this.TransparencyKey = Color.Empty;
            }

            this.BackColor = backColor;

            tpActivity.BackColor = backColor;
            tpSplit.BackColor = backColor;
            tpLap.BackColor = backColor;
            tpColor.BackColor = this.ColorTable.FormBackground;
            tpTimer.BackColor = this.ColorTable.FormBackground;

            tabControl.TabPanelBackColor = this.ColorTable.ActiveFormBorderColor;
            tabControl.InactiveTabColor = this.ColorTable.ActiveFormBorderColor;
            tabControl.ActiveTabColor = this.ColorTable.ActiveFormBorderColor;

            pBottom.BackColor = this.ColorTable.ActiveFormBorderColor;
            statusStrip.BackColor = this.ColorTable.ActiveFormBorderColor;
            tssbMenu.ForeColor = this.ColorTable.FormTextColor;
            statusLabel.ForeColor = this.ColorTable.FormTextColor;

            tpActivity.ForeColor = foreColor;
            tpSplit.ForeColor = foreColor;
            tpLap.ForeColor = foreColor;
            tpColor.ForeColor = this.ColorTable.FormTextColor;
            tpTimer.ForeColor = this.ColorTable.FormTextColor;

            ucActivityView.HeaderGradientBeginColor = this.ColorTable.ActiveTitleGradientBegin;
            ucActivityView.HeaderGradientEndColor = this.ColorTable.ActiveTitleGradientEnd;
            ucActivityView.HeaderForeColor = this.ColorTable.FormTextColor;
            ucActivityView.RowBackColor = backColor;
            ucActivityView.RowForeColor = foreColor;

            ucSplitView.HeaderGradientBeginColor = this.ColorTable.ActiveTitleGradientBegin;
            ucSplitView.HeaderGradientEndColor = this.ColorTable.ActiveTitleGradientEnd;
            ucSplitView.HeaderForeColor = this.ColorTable.FormTextColor;
            ucSplitView.RowBackColor = backColor;
            ucSplitView.RowForeColor = foreColor;

            ucLapView.HeaderGradientBeginColor = this.ColorTable.ActiveTitleGradientBegin;
            ucLapView.HeaderGradientEndColor = this.ColorTable.ActiveTitleGradientEnd;
            ucLapView.HeaderForeColor = this.ColorTable.FormTextColor;
            ucLapView.RowBackColor = backColor;
            ucLapView.RowForeColor = foreColor;

            ucColorView.HeaderGradientBeginColor = this.ColorTable.ActiveTitleGradientBegin;
            ucColorView.HeaderGradientEndColor = this.ColorTable.ActiveTitleGradientEnd;
            ucColorView.HeaderForeColor = this.ColorTable.FormTextColor;
            ucColorView.RowBackColor = backColor;
            ucColorView.RowForeColor = foreColor;
            
            ucTimerSetupView.HeaderGradientBeginColor = this.ColorTable.ActiveTitleGradientBegin;
            ucTimerSetupView.HeaderGradientEndColor = this.ColorTable.ActiveTitleGradientEnd;
            ucTimerSetupView.HeaderForeColor = this.ColorTable.FormTextColor;
            ucTimerSetupView.RowBackColor = backColor;
            ucTimerSetupView.RowForeColor = foreColor;

            FontStyle style = 0;

            style |= settings.IsFontBold ? FontStyle.Bold : 0;
            style |= settings.IsFontItalic ? FontStyle.Italic : 0;

            Font font = new Font(settings.FontFamily, settings.FontSize, style);

            ucActivityView.RowFont  = font;
            ucSplitView.RowFont     = font;
            ucLapView.RowFont       = font;
            ucColorView.RowFont = font;
            ucTimerSetupView.RowFont = font;

        }

        private void tabControl_SelectedIndexChanging(object sender, SelectedIndexChangingEventArgs e)
        {
            if (this.tabControl.SelectedTab == null)
                return;
            
            //Logger.LogDebug($"tabControl_SelectedIndexChanging - TabPageName: {this.tabControl.SelectedTab.Name}");

            switch (this.tabControl.SelectedTab.Name)
            {
                case "tpActivity":
                    this.ucActivityView.ControlLosingFocus(sender, e);
                    break;

                case "tpSplit":
                    this.ucSplitView.ControlLosingFocus(sender, e);
                    break;

                case "tpLap":
                    this.ucLapView.ControlLosingFocus(sender, e);
                    break;

                case "tpColor":
                    // When leaving the tpColor tab, show the transparent background if set.
                    if (ZAMsettings.Settings.Appearance.TransparencySetting != TransparencyType.NotTransparent)
                    {
                        this.TransparencyKey = this.BackColor;
                    }
                    this.ucColorView.ControlLosingFocus(sender, e);
                    break;

                case "tpTimer":
                    this.ucTimerSetupView.ControlLosingFocus(sender, e);
                    break;
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTab == null)
                return;

            //Logger.LogDebug($"tabControl_SelectedIndexChanged - TabPageName: {this.tabControl.SelectedTab.Name}");

            switch (this.tabControl.SelectedTab.Name)
            {
                case "tpActivity":
                    this.Text = HomeTitle + "";
                    this.ucActivityView.ControlGainingFocus(sender, e);
                    break;

                case "tpSplit":
                    this.Text = HomeTitle + " (Splits)";
                    this.ucSplitView.ControlGainingFocus(sender, e);
                    break;

                case "tpLap":
                    this.Text = HomeTitle + " (Laps)";
                    this.ucLapView.ControlGainingFocus(sender, e);
                    break;

                case "tpColor":
                    this.Text = HomeTitle + " (Colors)";

                    // When changing to the tpColor tab, eliminate the transparent background if showing.
                    if (ZAMsettings.Settings.Appearance.TransparencySetting != TransparencyType.NotTransparent)
                    {
                        this.TransparencyKey = Color.Empty;
                    }
                    this.ucColorView.ControlGainingFocus(sender, e);
                    break;

                case "tpTimer":
                    this.Text = HomeTitle + " (Timer)";
                    this.ucTimerSetupView.ControlGainingFocus(sender, e);
                    break;
            }
        }

        /// <summary>
        /// Allow user to change tab alignment position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_MouseClick(object sender, MouseEventArgs e)
        {
            //Logger.LogDebug($"tabControl_MouseClick - Location: {e.Location}, Button: {e.Button}");

            if (e.Button != MouseButtons.Right)
                return;

            ContextMenuStrip menuStrip = new ContextMenuStrip();
            ToolStripMenuItem item;

            item = (ToolStripMenuItem)menuStrip.Items.Add(TabAlignment.Top.ToString());
            item = (ToolStripMenuItem)menuStrip.Items.Add(TabAlignment.Bottom.ToString());
            item = (ToolStripMenuItem)menuStrip.Items.Add(TabAlignment.Left.ToString());
            item = (ToolStripMenuItem)menuStrip.Items.Add(TabAlignment.Right.ToString());

            foreach (ToolStripMenuItem mi in menuStrip.Items)
            {
                TabAlignment itemAlignment = Enum.Parse<TabAlignment>(mi.Text);
                mi.CheckOnClick = true;
                mi.Checked = (tabControl.Alignment == itemAlignment);
                mi.Tag = new KeyValuePair<string, TabAlignment>("TabAlignment", itemAlignment);
                mi.ToolTipText = item.Text;
                mi.CheckedChanged += tabContextMenu_CheckStateChanged;
            }
            menuStrip.Show(Cursor.Position);


        }

        /// <summary>
        /// Set the user selected tab alignment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabContextMenu_CheckStateChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            KeyValuePair<string, TabAlignment> itemTag = (KeyValuePair<string, TabAlignment>)item.Tag;

            tabControl.Alignment = itemTag.Value;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            //Logger.LogDebug($"MainForm_Resize - Size: {this.Size}");
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            if (this.Size != ZAMsettings.Settings.Appearance.WindowSize)
            {
                ZAMsettings.BeginCachedConfiguration();
                ZAMsettings.Settings.Appearance.WindowSize = this.Size;
                ZAMsettings.CommitCachedConfiguration();

                Logger.LogDebug($"MainForm_ResizeEnd - New window size saved, Size: {this.Size}");
            }
        }


        /// <summary>
        /// A delegate used solely by the ZPMonitorService_CollectionStatusChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void ZPMonitorService_CollectionStatusChangedDelegate(object sender, CollectionStatusChangedEventArgs e);

        private void ZPMonitorService_CollectionStatusChanged(object sender, CollectionStatusChangedEventArgs e)
        {
            if (!mDispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                mDispatcher.BeginInvoke(new ZPMonitorService_CollectionStatusChangedDelegate(ZPMonitorService_CollectionStatusChanged), new object[] { sender, e });
                return;
            }
            Logger.LogDebug($"{this.GetType()}.ZPMonitorService_CollectionStatusChanged - {e.Action}");

            this.OnCollectionStatusChanged();
        }

        /// <summary>
        /// A delegate used solely by the ZPMonitorService_ZPMonitorServiceStatusChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void ZPMonitorService_ServiceStatusChangedDelegate(object sender, ZPMonitorServiceStatusChangedEventArgs e);

        private void ZPMonitorService_ZPMonitorServiceStatusChanged(object sender, ZPMonitorServiceStatusChangedEventArgs e)
        {
            if (!mDispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                mDispatcher.BeginInvoke(new ZPMonitorService_ServiceStatusChangedDelegate(ZPMonitorService_ZPMonitorServiceStatusChanged), new object[] { sender, e });
                return;
            }
            Logger.LogDebug($"{this.GetType()}.ZPMonitorService_ZPMonitorServiceStatusChanged - {e.Action}");

            if (e.Action == ZPMonitorServiceStatusChangedEventArgs.ActionType.Started)
            {
                this.pbStatus.Image = global::ZwiftActivityMonitorV2.Properties.Resources.Status_GreenRed;
            }
            else if (e.Action == ZPMonitorServiceStatusChangedEventArgs.ActionType.Stopped)
            {
                this.pbStatus.Image = global::ZwiftActivityMonitorV2.Properties.Resources.Status_RedRed;
            }

            this.OnCollectionStatusChanged();
        }

        /// <summary>
        /// A delegate used solely by the ZPMonitorService_RiderStateEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void ZPMonitorService_RiderStateEventDelegate(object sender, RiderStateEventArgs e);
        private void ZPMonitorService_RiderStateEvent(object sender, RiderStateEventArgs e)
        {
            if (!mDispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                mDispatcher.BeginInvoke(new ZPMonitorService_RiderStateEventDelegate(ZPMonitorService_RiderStateEvent), new object[] { sender, e });
                return;
            }

            if (ZAMsettings.ZPMonitorService.EventsProcessed % 2 == 0)
            {
                this.pbStatus.Image = global::ZwiftActivityMonitorV2.Properties.Resources.Status_GreenBlank;
            }
            else
            {
                this.pbStatus.Image = global::ZwiftActivityMonitorV2.Properties.Resources.Status_GreenGreen;
            }

            // AdjustedElapsedTime will be null if Monitoring but not Collecting
            if (e.AdjustedElapsedTime == null)
            {
                return;
            }

            if (!e.IsPaused)
            {
                TimeSpan elapsedTime = e.AdjustedElapsedTime.Value;
                statusLabel.Text = $"Running time: {(elapsedTime.TotalMinutes > 60 ? elapsedTime.Hours + " hr " : "")}{(elapsedTime.TotalSeconds > 60 ? elapsedTime.Minutes + " min " : "")}{elapsedTime.Seconds + " sec"}";
            }
        }

        private void OnCollectionStatusChanged()
        {
            Logger.LogDebug($"OnCollectionStatusChanged - waiting: {ZAMsettings.ZPMonitorService.IsCollectionStartWaiting}, started: {ZAMsettings.ZPMonitorService.IsCollectionStarted}, paused: {ZAMsettings.ZPMonitorService.IsCollectionPaused}");

            if (ZAMsettings.ZPMonitorService.IsCollectionStarted || ZAMsettings.ZPMonitorService.IsCollectionStartWaiting)
            {
                tsmiStop.Enabled = true;
                tsmiStart.Enabled = false;

                tsmiConfiguration.Enabled = false;
                tsmiAdvanced.Enabled = false;
                tsmiAutoPause.Enabled = !ZAMsettings.ZPMonitorService.IsCollectionPaused;

                if (ZAMsettings.ZPMonitorService.IsCollectionStartWaiting)
                {
                    statusLabel.Text = "Waiting on Event clock...";
                }
                else if (ZAMsettings.ZPMonitorService.IsCollectionStarted)
                {
                    statusLabel.Text = ZAMsettings.ZPMonitorService.IsCollectionPaused ? "Paused" : "Started";
                }
            }
            else
            {
                tsmiStop.Enabled = false;
                tsmiStart.Enabled = ZAMsettings.ZPMonitorService.IsZPMonitorStarted;

                if (ucTimerSetupView.IsTimerRunning)
                {
                    tsmiConfiguration.Enabled = false;
                    tsmiAdvanced.Enabled = false;
                }
                else
                {
                    tsmiConfiguration.Enabled = true;
                    tsmiAdvanced.Enabled = true;
                }

                tsmiAutoPause.Enabled = true;

                if (ZAMsettings.ZPMonitorService.IsZPMonitorStarted)
                {
                    statusLabel.Text = "Select Menu->Start to begin";
                }
                else
                {
                    statusLabel.Text = "ZPM Service Not Running";
                }
            }
        }

        private void tsmiAbout_Click(object sender, EventArgs e)
        {
        }

        private void tsmiAdvanced_Click(object sender, EventArgs e)
        {
            new AdvancedOptions().ShowDialog(this);
        }

        private void tsmiConfiguration_Click(object sender, EventArgs e)
        {
            new ConfigurationOptions(this.Location).ShowDialog(this);
        }

        private void tsmiStart_Click(object sender, EventArgs e)
        {
            if (!ZAMsettings.ZPMonitorService.IsZPMonitorStarted)
            {
                MessageBox.Show("Select Menu->Configuration and use the System tab to start the service.", "ZwiftPacketMonitor Not Started", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // When Start is selected the default is to start collection when Event timer starts.  Use timer to have option to start immediately.
            ZAMsettings.ZPMonitorService.StartCollection(true);
        }

        private void tsmiStop_Click(object sender, EventArgs e)
        {
            ZAMsettings.ZPMonitorService.StopCollection();
        }

        private void tsmiAutoPause_CheckedChanged(object sender, EventArgs e)
        {
            if (ZAMsettings.Settings.CurrentUser != null)
            {
                ZAMsettings.BeginCachedConfiguration();
                ZAMsettings.Settings.CurrentUser.AutoPause = tsmiAutoPause.Checked;
                ZAMsettings.CommitCachedConfiguration();
            }
        }


        private void UcTimerSetupView_CountdownTimerTickEvent(object sender, CountdownTimerTickEventArgs e)
        {
            //Logger.LogDebug($"UcTimerSetupView_CountdownTimerTickEvent1 - ID: {Thread.CurrentThread.ManagedThreadId}");

            SynchronizationContext.SetSynchronizationContext(WindowsFormsSynchronizationContext.Current);

            //Logger.LogDebug($"UcTimerSetupView_CountdownTimerTickEvent2 - ID: {Thread.CurrentThread.ManagedThreadId}");

            Logger.LogDebug($"UcTimerSetupView_CountdownTimerTickEvent - startWithEventTimer: {e.StartWithEventTimer}");

            if (e.IsCompleted)
            {
                ZAMsettings.ZPMonitorService.StartCollection(e.StartWithEventTimer);
            }
            else if (e.IsCanceled)
            {
                this.OnCollectionStatusChanged();
            }
            else
            {
                statusLabel.Text = $"Time Remaining: {(e.TimeRemaining.TotalMinutes > 60 ? e.TimeRemaining.Hours.ToString() + " hr " : "")}{(e.TimeRemaining.TotalSeconds > 60 ? e.TimeRemaining.Minutes.ToString() + " min " : "")}{e.TimeRemaining.Seconds.ToString() + " sec"}";
            }
        }

        /// <summary>
        /// A delegate used solely by the MainForm_OneTimeInitializations method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void MainForm_OneTimeInitializationsDelegate();

        /// <summary>
        /// This is called once after the form has shown
        /// </summary>
        private void MainForm_OneTimeInitializations()
        {
            if (ZAMsettings.Settings.AutoStart)
            {
                ZAMsettings.ZPMonitorService.StartMonitor();
            }
            else
            {
                // Bring up the Configuration dialog
                tsmiConfiguration.PerformClick();
            }
        }

        /// <summary>
        /// General use timer set for one second intervals
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formSyncTimer_Tick(object sender, EventArgs e)
        {
            mSyncFormTimerTickCount++;

            if (!this.mOneTimeInitializationsCompleted)
            {
                // We invoke a delegate so this gets posted and doesn't block
                mDispatcher.BeginInvoke(new MainForm_OneTimeInitializationsDelegate(MainForm_OneTimeInitializations), new object[] { });

                this.mOneTimeInitializationsCompleted = true;
            }

            // The one second timer gets the actual tick count in it's event args
            OnFormSyncOneSecondTimerTickEvent(new FormSyncTimerTickEventArgs(mSyncFormTimerTickCount));

            // Has five seconds elapsed?
            if (mSyncFormTimerTickCount % 5 == 0)
            {
                // The five second timer gets the tick count / 5 in it's event args
                OnFormSyncFiveSecondTimerTickEvent(new FormSyncTimerTickEventArgs(mSyncFormTimerTickCount / 5));
            }
        }

        private void OnFormSyncOneSecondTimerTickEvent(FormSyncTimerTickEventArgs e)
        {
            EventHandler<FormSyncTimerTickEventArgs> handler = FormSyncOneSecondTimerTickEvent;

            if (handler != null)
            {
                try
                {
                    handler(this, e);
                }
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnFormSyncOneSecondTimerTickEvent)");
                }
            }
        }
        private void OnFormSyncFiveSecondTimerTickEvent(FormSyncTimerTickEventArgs e)
        {
            EventHandler<FormSyncTimerTickEventArgs> handler = FormSyncFiveSecondTimerTickEvent;

            if (handler != null)
            {
                try
                {
                    handler(this, e);
                }
                catch (Exception ex)
                {
                    // Don't let downstream exceptions bubble up
                    Logger.LogError(ex, $"Caught in {this.GetType()} (OnFormSyncFiveSecondTimerTickEvent)");
                }
            }
        }

    }
}
