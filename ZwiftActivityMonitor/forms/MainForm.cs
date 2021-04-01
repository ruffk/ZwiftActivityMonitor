using System;
using System.Drawing;
using System.Windows.Threading;
using System.Windows.Forms;
using Dapplo.Microsoft.Extensions.Hosting.WinForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace ZwiftActivityMonitor
{
    public partial class MainForm : Form, IWinFormsShell
    {
        private readonly ILogger<MainForm> Logger;
        private readonly IServiceProvider m_serviceProvider;

        private System.Drawing.Point m_offset;                      // for moving window
        private bool m_mouseDown;                                   // for moving window
        private Dispatcher m_dispatcher;                            // Current UI thread dispatcher, for marshalling UI calls
        private DateTime m_timerCompletion;                         // Time when timer countdown should complete
        private DateTime m_collectionStart;                         // Time when monitor run started
        private bool m_isStarted;                                   // Whether the collectors are currently running
        private UserProfile m_currentUser;                          // Current user, selected in Options dialog
        private CancellationTokenSource m_cancellationTokenSource;  // For cancelling thread awaiting rider start

        public MainForm(IServiceProvider serviceProvider)
        {
            Logger = ZAMsettings.LoggerFactory.CreateLogger<MainForm>();
            m_serviceProvider = serviceProvider;

            InitializeComponent();

            // This rounds the edges of the borderless window
            this.Region = System.Drawing.Region.FromHrgn(ZAMsettings.CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255); //transparent
        }

        #region Form Events

        /// <summary>
        /// On initial load, the desired collection durations are load from configuration.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            // for handling UI events
            m_dispatcher = Dispatcher.CurrentDispatcher;

            // Initialize the settings manager
            //ZAMsettings.Initialize(LoggerFactory);

            // Determine window position
            if (ZAMsettings.Settings.WindowPositionX > 0 && ZAMsettings.Settings.WindowPositionY > 0)
            {
                this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
                this.Location = new System.Drawing.Point(ZAMsettings.Settings.WindowPositionX, ZAMsettings.Settings.WindowPositionY);
            }

            // Set the environment based on the current user
            SetupCurrentUser();

            Logger.LogInformation("MainForm_Load");

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

            // Load collectors for whatever is defined in by the checked menu items
            LoadMovingAverageCollection();

            Logger.LogInformation("SetupCurrentUser");
        }

        /// <summary>
        /// Handle the case when form is being shown, either reopened or newly opened.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (ZAMsettings.Settings.AutoStart)
            {
                ZAMsettings.ZPMonitorService.StartMonitor();
            }
            else
            {
                // Bring up the options dialog
                tsmiOptions.PerformClick();
            }

            // Set control statuses
            OnCollectionStatusChanged();

            Logger.LogInformation("MainForm_Shown");
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_isStarted)
            {
                if (MessageBox.Show("Are you sure you wish to stop monitoring and close the application?",
                    "Activity Monitor Running", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            Collection_OnStop();

            Logger.LogInformation("MainForm_FormClosing");
        }

        #endregion

        /// <summary>
        /// Starts the data collectors and sets menu item enabled statuses accordingly.
        /// </summary>
        private async void Collection_OnStart()
        {
            if (!m_isStarted)
            {
                m_isStarted = true;

                // update all the menu items accordingly
                OnCollectionStatusChanged();

                m_cancellationTokenSource = new CancellationTokenSource();


                // Start a thread to wait for the PlayerState.Time to become non-zero.  
                // This can be cancelled by selecting Stop from the menu.
                await WaitForRiderStartAsync(m_cancellationTokenSource.Token);

                bool cancelled = m_cancellationTokenSource.IsCancellationRequested;
                m_cancellationTokenSource = null;

                if (cancelled)
                {
                    Logger.LogInformation($"Collection_OnStart - Cancelled");
                    return;
                }

                ucMain.StartCollection();

                m_collectionStart = DateTime.Now;
                runTimer.Enabled = true;

                Logger.LogInformation($"Collection_OnStart");
            }
        }

        /// <summary>
        /// Wait for the rider clock to begin counting.
        /// This allows user to select Start, and collection won't begin until the clock begins.  
        /// On a freeride, this is when they start pedalling.
        /// In a timed event, this is when banner drops.
        /// In a non-timed event, this is when the user crosses the timing start line, which is usually shortly after the banner.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task WaitForRiderStartAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogInformation($"WaitForRiderStartAsync, Begin Waiting...");

            tsslStatus.Text = "Waiting on Event clock...";

            double currentTime = ZAMsettings.ZPMonitorService.PlayerStateTime.TotalSeconds;

            await Task.Run(() =>
            {
                while (!cancellationToken.IsCancellationRequested && ZAMsettings.ZPMonitorService.PlayerStateTime.TotalSeconds == currentTime)
                {
                    Task.Delay(250).Wait();
                }
            }, cancellationToken);
           
            Logger.LogInformation($"WaitForRiderStartAsync, Waiting completed.  Cancelled: {cancellationToken.IsCancellationRequested}");
        }


        /// <summary>
        /// Stops the data collectors and sets menu item enabled statuses accordingly.
        /// </summary>
        private void Collection_OnStop()
        {
            if (m_isStarted)
            {
                if (m_cancellationTokenSource == null)
                {
                    ucMain.StopCollection();

                    runTimer.Enabled = false;
                }
                else
                {
                    // Cancel the waiting thread
                    m_cancellationTokenSource.Cancel();
                }

                m_isStarted = false;

                OnCollectionStatusChanged();
                Logger.LogInformation($"Collection_OnStop");
            }
        }

        private void OnCollectionStatusChanged()
        {
            if (m_isStarted)
            {
                // Clear any values on the screen
                ucMain.RefreshListViews(true);

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
                    ucMain.RefreshListViews(true);

                    tsmiSetupTimer.Enabled = false;
                    tsmiStopTimer.Enabled = true;
                }
                else
                {
                    tsmiSetupTimer.Enabled = true;
                    tsmiStopTimer.Enabled = false;
                }

                if (ZAMsettings.ZPMonitorService.IsStarted)
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
            // Remove all moving average collectors and ListView items
            ucMain.ClearViewerItems();

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
                    if (ucMain.CountViewerItems < 3)
                    {
                        DurationType result;
                        if (Enum.TryParse<DurationType>(tsmi.Tag.ToString(), true, out result))
                        {
                            ucMain.AddViewerItem(result, tsmi.Text);

                            //if (ucMain.CountViewerItems >= 3) // only allow up to 3 collectors
                            //    break;
                        }
                        else
                        {
                            throw new ApplicationException($"Bug: The menuitem tag {tsmi.Tag} for menuitem {tsmi.Text} did not match any DurationType Enums.");
                        }
                    }
                    else
                    {
                        tsmi.Checked = false;
                    }
                }
            }
        }

        #region Timer menu and tick event handling

        private void tsmiSetupTimer_Click(object sender, EventArgs e)
        {
            if (!ZAMsettings.ZPMonitorService.IsStarted)
            {
                MessageBox.Show("Please use the Advanced Options dialog to start the service.", "ZwiftPacketMonitor Not Started", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MonitorTimer mt = m_serviceProvider.GetService<MonitorTimer>();

            DialogResult result = mt.ShowDialog(this);

            if (result == DialogResult.OK)
            {

                m_timerCompletion = DateTime.Now.AddSeconds((mt.Minutes * 60) + mt.Seconds);

                Logger.LogInformation($"Minutes: {mt.Minutes} Seconds: {mt.Seconds} Completion Time: {m_timerCompletion.ToString()}");

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

            if (ts.TotalSeconds <= 0)
            {
                countdownTimer.Enabled = false;
                Logger.LogInformation($"Go! Go! Go!");

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

            tsslStatus.Text = "Running time: " + ts.Hours.ToString("0#") + ":" + ts.Minutes.ToString("0#") + ":" + ts.Seconds.ToString("0#");

            ucMain.RefreshListViews();
        }

        #endregion

        #region Misc Form events

        private void lblTitle_MouseDown(object sender, MouseEventArgs e)
        {
            m_offset.X = e.X;
            m_offset.Y = e.Y;
            m_mouseDown = true;
        }

        private void lblTitle_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_mouseDown)
            {
                Point currentPos = this.PointToScreen(e.Location);
                this.Location = new Point(currentPos.X - m_offset.X, currentPos.Y - m_offset.Y);
            }
        }

        private void lblTitle_MouseUp(object sender, MouseEventArgs e)
        {
            m_mouseDown = false;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SkipControl_Enter(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (c != null && c.Parent != null)
            {
                c.Parent.SelectNextControl(c, true, true, true, true);
            }
        }


        #endregion

        #region Menu item events

        private void tsmiAdvanced_Click(object sender, EventArgs e)
        {
            var form = m_serviceProvider.GetService<AdvancedOptions>();

            DialogResult result = form.ShowDialog(this);

            // Allow menus and status bar to update according to what user just did
            OnCollectionStatusChanged();
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
            var form = new ConfigurationOptions(m_serviceProvider, this.Location);

            DialogResult result = form.ShowDialog(this);

            SetupCurrentUser();

            // Allow menus and status bar to update according to what user just did
            OnCollectionStatusChanged();
        }

        private void tsmiStart_Click(object sender, EventArgs e)
        {
            if (!ZAMsettings.ZPMonitorService.IsStarted)
            {
                MessageBox.Show("Please use the Options dialog to start the service.", "ZwiftPacketMonitor Not Started", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Collection_OnStart();
        }

        private void tsmiStop_Click(object sender, EventArgs e)
        {
            Collection_OnStop();
        }

        private void anyDuration_Click(object sender, EventArgs e)
        {
            // The checked status for some item has changed.
            LoadMovingAverageCollection();
        }

        #endregion
    }
}
