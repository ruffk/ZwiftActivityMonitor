using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;
using WK.Libraries.HotkeyListenerNS;

namespace ZwiftActivityMonitorV2
{
    public partial class GeneralConfigControl : UserControlWithStatusBase
    {
        private Dispatcher m_dispatcher;
        private bool m_editMode;

        // Declare an instance of the Hotkey Selector class.
        internal static HotkeySelector _HotkeySelector = new();

        public GeneralConfigControl()
        {
            InitializeComponent();

            if (DesignMode)
                return;

            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<GeneralConfigControl>();
        }

        protected override void UserControlBase_Load(object sender, EventArgs e)
        {
            base.UserControlBase_Load(sender, e);

            if (DesignMode)
                return;

            m_dispatcher = Dispatcher.CurrentDispatcher;

            // initialize
            EditingSystemSettings = false;
        }

        public override void ControlLosingFocus(object sender, Syncfusion.Windows.Forms.Tools.SelectedIndexChangingEventArgs e)
        {
            base.ControlLosingFocus(sender, e);

            if (EditingSystemSettings)
            {
                MessageBox.Show("Please either Save or Cancel current work before proceeding.", "Pending Changes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }

        public override void ControlGainingFocus(object sender, EventArgs e)
        {
            base.ControlGainingFocus(sender, e);

            // Reload each time control is shown as user profile info may have changed.
            SystemSettings_LoadFields();

            btnEditSettings.Focus();
        }

        #region SystemSettings

        private void SystemSettings_LoadFields()
        {
            // Enable the default textbox for hotkey-selection.
            _HotkeySelector.Enable(tbActivityViewKeys, HotkeyListener.Convert(ZAMsettings.Settings.Hotkeys.ActivityViewHotKeySequence));
            _HotkeySelector.Enable(tbSplitViewKeys, HotkeyListener.Convert(ZAMsettings.Settings.Hotkeys.SplitViewHotkeySequence));
            _HotkeySelector.Enable(tbLapViewKeys, HotkeyListener.Convert(ZAMsettings.Settings.Hotkeys.LapViewHotkeySequence));
            _HotkeySelector.Enable(tbNewLapKeys, HotkeyListener.Convert(ZAMsettings.Settings.Hotkeys.NewLapHotkeySequence));
            _HotkeySelector.Enable(tbResetLapsKeys, HotkeyListener.Convert(ZAMsettings.Settings.Hotkeys.ResetLapsHotkeySequence));
        }

        private void btnEditSettings_Click(object sender, EventArgs e)
        {
            ZAMsettings.BeginCachedConfiguration();
            EditingSystemSettings = true;
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            bool errorOccurred = false;

            errorOccurred = (errorOccurred || ValidateSystemSettings(tbActivityViewKeys));
            errorOccurred = (errorOccurred || ValidateSystemSettings(tbSplitViewKeys));
            errorOccurred = (errorOccurred || ValidateSystemSettings(tbLapViewKeys));
            errorOccurred = (errorOccurred || ValidateSystemSettings(tbNewLapKeys));
            errorOccurred = (errorOccurred || ValidateSystemSettings(tbResetLapsKeys));

            if (!errorOccurred)
            {
                ZAMsettings.CommitCachedConfiguration();
                EditingSystemSettings = false;

                // set the new hotkey combinations
                ZAMsettings.Settings.Hotkeys.UpdateHotkeys();

                //ZAMsettings.OnSystemConfigChanged(this, new EventArgs());
            }
        }



        private void btnCancelSettings_Click(object sender, EventArgs e)
        {
            ZAMsettings.RollbackCachedConfiguration();

            errorProvider.Clear();

            // Reload values from configuration into fields since cancel was pressed
            SystemSettings_LoadFields();

            EditingSystemSettings = false;
        }


        private bool EditingSystemSettings
        {
            set
            {
                btnEditSettings.Enabled = !value;
                btnSaveSettings.Enabled = value;
                btnCancelSettings.Enabled = value;

                tbActivityViewKeys.Enabled = value;
                tbLapViewKeys.Enabled = value;
                tbSplitViewKeys.Enabled = value;
                tbNewLapKeys.Enabled = value;
                tbResetLapsKeys.Enabled = value;

                m_editMode = value;
            }

            get { return m_editMode; }
        }

        private void SystemSettings_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = ValidateSystemSettings(sender as Control);
        }

        private bool ValidateSystemSettings(Control control)
        {
            bool errorOccurred = false;

            errorProvider.SetError(control, "");

            Hotkey k;
            
            switch (control.Name)
            {
                case "tbActivityViewKeys":
                    try
                    {
                        k = HotkeyListener.Convert(tbActivityViewKeys.Text);
                        ZAMsettings.Settings.Hotkeys.ActivityViewHotKeySequence = k.ToString();
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "tbSplitViewKeys":
                    try
                    {
                        k = HotkeyListener.Convert(tbSplitViewKeys.Text);
                        ZAMsettings.Settings.Hotkeys.SplitViewHotkeySequence = k.ToString();
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "tbLapViewKeys":
                    try
                    {
                        k = HotkeyListener.Convert(tbLapViewKeys.Text);
                        ZAMsettings.Settings.Hotkeys.LapViewHotkeySequence = k.ToString();
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "tbNewLapKeys":
                    try
                    {
                        k = HotkeyListener.Convert(tbNewLapKeys.Text);
                        ZAMsettings.Settings.Hotkeys.NewLapHotkeySequence = k.ToString();
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "tbResetLapsKeys":
                    try
                    {
                        k = HotkeyListener.Convert(tbResetLapsKeys.Text);
                        ZAMsettings.Settings.Hotkeys.ResetLapsHotkeySequence = k.ToString();
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
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
                case "tbActivityViewKeys":
                    this.toolStripStatusLabel.Text = "Press the desired key combination for Activity Viewer selection.";
                    break;

                case "tbSplitViewKeys":
                    this.toolStripStatusLabel.Text = "Press the desired key combination for Split Viewer selection.";
                    break;

                case "tbLapViewKeys":
                    this.toolStripStatusLabel.Text = "Press the desired key combination for Lap Viewer selection.";
                    break;

                case "tbNewLapKeys":
                    this.toolStripStatusLabel.Text = "Press the desired key combination for starting a new lap.";
                    break;

                case "tbResetLapsKeys":
                    this.toolStripStatusLabel.Text = "Press the desired key combination for resetting all laps.";
                    break;
            }

        }

        #endregion


        protected override void ListView_ItemSelectionChanged_Disable(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            base.ListView_ItemSelectionChanged_Disable(sender, e);
        }

        protected override void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            base.ListView_DrawItem(sender, e);
        }

        protected override void Listview_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            base.Listview_DrawSubItem(sender, e);
        }

        protected override void Parent_BackColorChanged(object sender, EventArgs e)
        {
            base.Parent_BackColorChanged(sender, e);

            //Debug.WriteLine($"SystemControl setting BackColor: {this.BackColor.R},{this.BackColor.G},{this.BackColor.B}");

            this.tbDescSystem.BackColor = this.BackColor;

            MSoffice2010ColorManager colorTable = ZAMappearance.GetColorTable();

            this.gbGeneralSettings.BorderColor = colorTable.ActiveFormBorderColor;
            this.gbHotKeys.BorderColor = colorTable.ActiveFormBorderColor;
        }

        protected override void Parent_ForeColorChanged(object sender, EventArgs e)
        {
            base.Parent_ForeColorChanged(sender, e);

            //Debug.WriteLine($"SystemControl setting ForeColor: {this.ForeColor.R},{this.ForeColor.G},{this.ForeColor.B}");

            this.tbDescSystem.ForeColor = this.ForeColor;

            this.gbGeneralSettings.ForeColor = this.ForeColor;
            this.gbHotKeys.ForeColor = this.ForeColor;
        }
    }
}
