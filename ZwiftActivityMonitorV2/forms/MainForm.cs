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
        private bool mStartCollectionOnEventTimerStart;

        private SynchronizationContext UISyncContext;

        private readonly ILogger<MainForm> Logger;
        private readonly IServiceProvider ServiceProvider;

        private const string HomeTitle = "Activity Monitor";

        private int mSyncFormTimerTickCount;

        public event EventHandler<FormSyncTimerTickEventArgs> FormSyncOneSecondTimerTickEvent;
        public event EventHandler<FormSyncTimerTickEventArgs> FormSyncFiveSecondTimerTickEvent;


        public MainForm(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.Logger = ZAMsettings.LoggerFactory.CreateLogger<MainForm>();

            InitializeComponent();

            // Determine window position
            if (ZAMsettings.Settings.WindowPositionX != 0 && ZAMsettings.Settings.WindowPositionY != 0)
            {
                this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                this.Location = new System.Drawing.Point(ZAMsettings.Settings.WindowPositionX, ZAMsettings.Settings.WindowPositionY);
            }

            ucColorView.ColorsAndFontChanged += ucColorView_ColorsAndFontChanged;
            ZAMsettings.ZPMonitorService.CollectionStatusChanged += ZPMonitorService_CollectionStatusChanged;
            ZAMsettings.ZPMonitorService.ZPMonitorServiceStatusChanged += ZPMonitorService_ZPMonitorServiceStatusChanged;
            ZAMsettings.ZPMonitorService.RiderStateEvent += ZPMonitorService_RiderStateEvent;
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            // for handling UI events
            mDispatcher = Dispatcher.CurrentDispatcher;
            this.UISyncContext = WindowsFormsSynchronizationContext.Current;

            this.SetControlColors();


            // Determine window size
            this.Size = ZAMsettings.Settings.Appearance.WindowSize;

            // Set the environment based on the current user
            this.SetupCurrentUser();

            // toggle the tabs so the first tab gets initialized
            tabControl.SelectedIndex = 1;
            tabControl.SelectedIndex = 0;

            // start general syncronization timer
            this.formSyncTimer.Interval = 1000;
            this.formSyncTimer.Enabled = true;

            this.OnCollectionStatusChanged();
        }
        private void ucColorView_ColorsAndFontChanged(object sender, ColorsAndFontChangedEventArgs e)
        {
            SetControlColors();
        }

        private void SetControlColors()
        {
            ZAMappearance settings = ZAMsettings.Settings.Appearance;

            ZAMappearance.ApplyColorScheme(this);

            //this.UseOffice2010SchemeBackColor = true;

            //if (settings.ThemeSetting != ThemeType.Custom)
            //{
            //    this.ColorScheme = settings.GetOfficeColorScheme(settings.ThemeSetting, out Color? managedColor);

            //    if (this.ColorScheme == Office2010Theme.Managed)
            //    {
            //        Office2010Colors.ApplyManagedColors(this, managedColor.Value);
            //    }
            //}
            //else
            //{
            //    this.ColorScheme = Office2010Theme.Managed;
            //    Office2010Colors.ApplyManagedColors(this, settings.ManagedColor);
            //}

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

            tabControl.TabPanelBackColor = this.ColorTable.ActiveFormBorderColor;
            tabControl.InactiveTabColor = this.ColorTable.ActiveFormBorderColor;
            tabControl.ActiveTabColor = this.ColorTable.ActiveFormBorderColor;

            pnBottom.BackColor = this.ColorTable.ActiveFormBorderColor;
            statusStrip.BackColor = this.ColorTable.ActiveFormBorderColor;
            tssbMenu.ForeColor = this.ColorTable.FormTextColor;
            statusLabel.ForeColor = this.ColorTable.FormTextColor;

            tpActivity.ForeColor = foreColor;
            tpSplit.ForeColor = foreColor;
            tpLap.ForeColor = foreColor;
            tpColor.ForeColor = this.ColorTable.FormTextColor;

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

            FontStyle style = 0;

            style |= settings.IsFontBold ? FontStyle.Bold : 0;
            style |= settings.IsFontItalic ? FontStyle.Italic : 0;

            Font font = new Font(settings.FontFamily, settings.FontSize, style);

            ucActivityView.RowFont  = font;
            ucSplitView.RowFont     = font;
            ucLapView.RowFont       = font;
            ucColorView.RowFont     = font;

        }

        private void tabControl_SelectedIndexChanging(object sender, SelectedIndexChangingEventArgs e)
        {
            if (this.tabControl.SelectedTab == null)
                return;

            //Debug.WriteLine($"tabControl_SelectedIndexChanging - TabPageName: {this.tabControl.SelectedTab.Name}");

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
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl.SelectedTab == null)
                return;

            //Debug.WriteLine($"tabControl_SelectedIndexChanged - TabPageName: {this.tabControl.SelectedTab.Name}");

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
            }
        }

        /// <summary>
        /// Allow user to change tab alignment position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabControl_MouseClick(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine($"tabControl_MouseClick - Location: {e.Location}, Button: {e.Button}");

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
            //Debug.WriteLine($"MainForm_Resize - Size: {this.Size}");
        }

        private void MainForm_ResizeEnd(object sender, EventArgs e)
        {
            if (this.Size != ZAMsettings.Settings.Appearance.WindowSize)
            {
                ZAMsettings.BeginCachedConfiguration();
                ZAMsettings.Settings.Appearance.WindowSize = this.Size;
                ZAMsettings.CommitCachedConfiguration();

                Debug.WriteLine($"MainForm_ResizeEnd - New window size saved, Size: {this.Size}");
            }
        }


        private void SetupCurrentUser()
        {
            // Get the currently selected user profile. This will be the user marked as default at startup, but can be changed at runtime.
            //this.CurrentUserProfile = ZAMsettings.Settings.CurrentUser;

            //SortedList<string, Collector> selectedCollectors = m_currentUser.SelectedCollectors;

            //// Check the menu items for user selected collectors, uncheck the others
            //foreach (ToolStripItem mi in tsmiAnalyze.DropDownItems)
            //{
            //    ToolStripMenuItem tsmi = mi as ToolStripMenuItem;

            //    if (tsmi != null)
            //    {
            //        tsmi.Checked = selectedCollectors.ContainsKey(tsmi.Text);
            //    }
            //}

            //// Load collectors for whatever is defined in by the checked menu items
            //LoadMovingAverageCollection();

            Logger.LogInformation("SetupCurrentUser");
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
            Debug.WriteLine($"{this.GetType()}.ZPMonitorService_CollectionStatusChanged - {e.Action}");

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
            Debug.WriteLine($"{this.GetType()}.ZPMonitorService_ZPMonitorServiceStatusChanged - {e.Action}");

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
            // ElapsedTime will be null if Monitoring but not Collecting
            if (e.ElapsedTime == null)
                return;

            TimeSpan elapsedTime = e.ElapsedTime.Value;

            SynchronizationContext.SetSynchronizationContext(WindowsFormsSynchronizationContext.Current);

            //this.UISyncContext.Post(x =>
            //{
            //    statusLabel.Text = $"Running time: {(e.ElapsedTime.TotalMinutes > 60 ? e.ElapsedTime.Hours.ToString() + " hr " : "")}{(e.ElapsedTime.TotalSeconds > 60 ? e.ElapsedTime.Minutes.ToString() + " min " : "")}{e.ElapsedTime.Seconds.ToString() + " sec"}";

            //}, null);

            //if (!mDispatcher.CheckAccess()) // are we currently on the UI thread?
            //{
            //    // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
            //    mDispatcher.BeginInvoke(new ZPMonitorService_RiderStateEventDelegate(ZPMonitorService_RiderStateEvent), new object[] { sender, e });
            //    return;
            //}
            //Debug.WriteLine($"{this.GetType()}.ZPMonitorService_RiderStateEvent");

            statusLabel.Text = $"Running time: {(elapsedTime.TotalMinutes > 60 ? elapsedTime.Hours.ToString() + " hr " : "")}{(elapsedTime.TotalSeconds > 60 ? elapsedTime.Minutes.ToString() + " min " : "")}{elapsedTime.Seconds.ToString() + " sec"}";
        }

        private void OnCollectionStatusChanged()
        {
            Debug.WriteLine($"OnCollectionStatusChanged - {ZAMsettings.ZPMonitorService.IsCollectionStartWaiting}, {ZAMsettings.ZPMonitorService.IsCollectionStarted}");

            if (ZAMsettings.ZPMonitorService.IsCollectionStarted || ZAMsettings.ZPMonitorService.IsCollectionStartWaiting)
            {
                tsmiStop.Enabled = true;
                tsmiStart.Enabled = false;

                tsmiTimer.Enabled = false;
                tsmiConfiguration.Enabled = false;
                tsmiAdvanced.Enabled = false;

                if (ZAMsettings.ZPMonitorService.IsCollectionStartWaiting)
                    statusLabel.Text = "Waiting on Event clock...";
                else
                    statusLabel.Text = "Started";
            }
            else
            {
                tsmiStop.Enabled = false;
                tsmiStart.Enabled = true;

                tsmiTimer.Enabled = true;
                tsmiConfiguration.Enabled = true;
                tsmiAdvanced.Enabled = true;

                //tsmiSetupTimer.Enabled = true;
                //tsmiStopTimer.Enabled = false;

                // set Timer menu sub-items
                //if (countdownTimer.Enabled)
                //{
                //    // Clear any values on the screen
                //    MainView.RefreshListViews(true);
                //    SplitsView.ClearListView();
                //    LapView.ClearListView();

                //    tsmiSetupTimer.Enabled = false;
                //    tsmiStopTimer.Enabled = true;
                //    tsmiStart.Enabled = false;
                //    tsmiOptions.Enabled = false;
                //    tsmiAdvanced.Enabled = false;
                //}

                if (ZAMsettings.ZPMonitorService.IsZPMonitorStarted)
                    statusLabel.Text = "Select Menu->Start to begin";
                else
                    statusLabel.Text = "ZPM Service Not Running";

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

            //SetupCurrentUser();

            // Allow menus and status bar to update according to what user just did
            //OnCollectionStatusChanged();
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


        private bool postLoadCompleted;

        /// <summary>
        /// General use timer set for one second intervals
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void formSyncTimer_Tick(object sender, EventArgs e)
        {
            mSyncFormTimerTickCount++;

            // The one second timer gets the actual tick count in it's event args
            OnFormSyncOneSecondTimerTickEvent(new FormSyncTimerTickEventArgs(mSyncFormTimerTickCount));

            if (!postLoadCompleted)
            {
                ucActivityView.Control_PostLoad();
                this.postLoadCompleted = true;
            }

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
                    Logger.LogWarning(ex, ex.ToString());
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
                    Logger.LogWarning(ex, ex.ToString());
                }
            }
        }

    }
}
