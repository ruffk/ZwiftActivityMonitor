using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;

namespace ZwiftActivityMonitorV2
{
    public partial class StatisticsControl : UserControlWithStatusBase
    {
        #region Internal classes
        internal class CollectorListViewColumnSorter : IComparer
        {
            /// <summary>
            /// Specifies the column to be sorted
            /// </summary>
            public int ColumnToSort { get; set; }

            /// <summary>
            /// Specifies the order in which to sort (i.e. 'Ascending').
            /// </summary>
            private SortOrder OrderOfSort { get; set; }

            public CollectorListViewColumnSorter(SortOrder sortOrder)
            {
                // Initialize the column to '0'
                ColumnToSort = 0;

                OrderOfSort = sortOrder;

                // Initialize the CaseInsensitiveComparer object
                //ObjectCompare = new CaseInsensitiveComparer();
            }

            /// <summary>
            /// This method is inherited from the IComparer interface.
            /// </summary>
            /// <param name="x">First object to be compared</param>
            /// <param name="y">Second object to be compared</param>
            /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
            public int Compare(object x, object y)
            {
                int compareResult;
                Collector collectorX, collectorY;

                // Cast the objects to be compared to ListViewItem objects
                collectorX = (x as CollectorListViewItem).Collector;
                collectorY = (y as CollectorListViewItem).Collector; ;


                // Not using ColumnToSort because we're sorting on a numeric column not in the ListView
                compareResult = collectorX.DurationSecs.CompareTo(collectorY.DurationSecs);

                // Compare the two items
                //compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

                // Calculate correct return value based on object comparison
                if (OrderOfSort == SortOrder.Ascending)
                {
                    // Ascending sort is selected, return normal result of compare operation
                    return compareResult;
                }
                else if (OrderOfSort == SortOrder.Descending)
                {
                    // Descending sort is selected, return negative result of compare operation
                    return (-compareResult);
                }
                else
                {
                    // Return '0' to indicate they are equal
                    return 0;
                }
            }
        }

        internal class CollectorListViewItem : ListViewItem
        {
            private Collector m_collector;

            //public CollectorListViewItem() : base(new string[] { "*** New Collector ***" })
            //{
            //    m_collector = new Collector();
            //}
            
            public CollectorListViewItem(Collector collector) : base(SubItemStrings(collector))
            {
                m_collector = (Collector)collector.Clone();
            }

            private static string[] SubItemStrings(Collector collector)
            {
                return (new string[]
                {
                    collector.Name,
                    collector.FieldAvgDesc,
                    collector.FieldAvgMaxDesc,
                    collector.FieldFtpDesc
                });
            }

            public void Refresh()
            {
                // Set each SubItem individually.  Cannot Clear and AddRange as it messes up.
                string[] text = SubItemStrings(m_collector);

                for (int i = 0; i < 4; i++)
                    base.SubItems[i].Text = text[i];

                //for (int i = 0; i < base.SubItems.Count; i++)
                //    base.SubItems[i] = new ListViewSubItem(this, text[i]);
            }

            public Collector Collector
            {
                get
                {
                    return m_collector;
                }
                set
                {
                    m_collector = (Collector)value.Clone();
                }
            }
        }
        #endregion

        private bool m_editMode;

        public StatisticsControl()
        {
            InitializeComponent();
            if (DesignMode)
                return;

            UserControlBase.SetListViewHeaderColor(ref this.lvCollectors, SystemColors.Control, SystemColors.ControlText);
        }

        protected override void UserControlBase_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            base.UserControlBase_Load(sender, e);

            rbAvgHide.Tag = FieldUomType.Hidden.ToString();
            rbAvgWatts.Tag = FieldUomType.Watts.ToString();
            rbAvgWkg.Tag = FieldUomType.Wkg.ToString(); ;

            rbAvgMaxHide.Tag = FieldUomType.Hidden.ToString();
            rbAvgMaxWatts.Tag = FieldUomType.Watts.ToString();
            rbAvgMaxWkg.Tag = FieldUomType.Wkg.ToString(); ;

            rbFtpHide.Tag = FieldUomType.Hidden.ToString();
            rbFtpWatts.Tag = FieldUomType.Watts.ToString();
            rbFtpWkg.Tag = FieldUomType.Wkg.ToString(); ;


            lvCollectors.ListViewItemSorter = new CollectorListViewColumnSorter(lvCollectors.Sorting);

            List<Collector> collectors = ZAMsettings.Settings.GetCollectors;

            lvCollectors.Items.Clear();
            collectors.ForEach(collector =>
            {
                lvCollectors.Items.Add(new CollectorListViewItem(collector));
            });
            
            lvCollectors.Sort();

            if (lvCollectors.Items.Count > 0)
            {
                lvCollectors.Items[0].Selected = true;
            }

            EditingCollectors = false; // initialize
        }

        //protected override void SkipControl_Enter(object sender, EventArgs e)
        //{
        //    base.SkipControl_Enter(sender, e);
        //}

        public override void ControlLosingFocus(object sender, Syncfusion.Windows.Forms.Tools.SelectedIndexChangingEventArgs e)
        {
            base.ControlLosingFocus(sender, e);

            if (EditingCollectors)
            {
                MessageBox.Show("Please either Save or Cancel current work before proceeding.", "Pending Changes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }
        public override void ControlGainingFocus(object sender, EventArgs e)
        {
            base.ControlGainingFocus(sender, e);

            //ActiveControl = this;
            btnStatsEdit.Focus();
        }

        private bool EditingCollectors
        {
            set
            {
                //btnAdd.Enabled = !value;
                btnStatsCancel.Enabled = value;
                btnStatsSave.Enabled = value;

                // some things you can only do if an item is selected
                if (lvCollectors.SelectedItems.Count > 0)
                {
                    btnStatsEdit.Enabled = !value;
                    //btnRemove.Enabled = !value;
                }
                else
                {
                    btnStatsEdit.Enabled = false;
                    //btnRemove.Enabled = false;
                }

                // when editing, you can't change the selection
                lvCollectors.Enabled = !value;

                tbDuration.Enabled = value;
                pAvgGroup.Enabled = value;
                pAvgMaxGroup.Enabled = value;
                pFtpGroup.Enabled = value;

                m_editMode = value;

                if (Logger != null)
                    Logger.LogDebug($"EditingCollectors: {value}, SelectedItemsCount: {lvCollectors.SelectedItems.Count}");
            }

            get { return m_editMode; }
        }
        private void lvCollectors_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvCollectors.SelectedItems.Count > 0)
            {
                Collector collector = ((CollectorListViewItem)lvCollectors.SelectedItems[0]).Collector;

                Collectors_LoadFields(collector);

                Logger.LogDebug($"SelectedIndexChanged {collector.Name} selected.");
            }
            else
            {
                Logger.LogDebug($"SelectedIndexChanged nothing selected.");
            }

            EditingCollectors = false;
        }

        /// <summary>
        /// Used to update display fields when selection changes
        /// </summary>
        /// <param name="user"></param>
        private void Collectors_LoadFields(Collector collector)
        {
            if (collector != null)
            {
                tbDuration.Text = collector.Name;

                RadioButton b;

                // Loop through the radiobuttons on each panel and check the appropriate button.
                // This relies on the button's Tag being set to the FieldUomType string
                foreach (Control c in pAvgGroup.Controls)
                    if ((b = (c as RadioButton)) != null && (string)b.Tag == collector.FieldAvgDesc)
                        b.Checked = true;

                foreach (Control c in pAvgMaxGroup.Controls)
                    if ((b = (c as RadioButton)) != null && (string)b.Tag == collector.FieldAvgMaxDesc)
                        b.Checked = true;

                foreach (Control c in pFtpGroup.Controls)
                    if ((b = (c as RadioButton)) != null && (string)b.Tag == collector.FieldFtpDesc)
                        b.Checked = true;

                //rbAvgHide.Checked = collector.FieldAvgType == FieldUomType.Hidden;
                //rbAvgWatts.Checked = collector.FieldAvgType == FieldUomType.Watts;
                //rbAvgWkg.Checked = collector.FieldAvgType == FieldUomType.Wkg;

                //rbAvgMaxHide.Checked = collector.FieldAvgMaxType == FieldUomType.Hidden;
                //rbAvgMaxWatts.Checked = collector.FieldAvgMaxType == FieldUomType.Watts;
                //rbAvgMaxWkg.Checked = collector.FieldAvgMaxType == FieldUomType.Wkg;

                //rbFtpHide.Checked = collector.FieldFtpType == FieldUomType.Hidden;
                //rbFtpWatts.Checked = collector.FieldFtpType == FieldUomType.Watts;
                //rbFtpWkg.Checked = collector.FieldFtpType == FieldUomType.Wkg;
            }
            else
            {
                tbDuration.Text = "";
            }
        }

        private void btnStatsEdit_Click(object sender, EventArgs e)
        {
            ZAMsettings.BeginCachedConfiguration();
            EditingCollectors = true;

        }

        private void btnStatsSave_Click(object sender, EventArgs e)
        {
            bool errorOccurred = false;

            // this should not happen
            if (lvCollectors.SelectedItems.Count < 1)
            {
                ZAMsettings.RollbackCachedConfiguration();
                EditingCollectors = false;
                return;
            }

            CollectorListViewItem itemBeingEdited = (CollectorListViewItem)lvCollectors.SelectedItems[0];
            Collector collectorBeingEdited = itemBeingEdited.Collector;

            errorOccurred = (errorOccurred || ValidateStatistics(rbAvgHide));
            errorOccurred = (errorOccurred || ValidateStatistics(rbAvgWatts));
            errorOccurred = (errorOccurred || ValidateStatistics(rbAvgWkg));

            errorOccurred = (errorOccurred || ValidateStatistics(rbAvgMaxHide));
            errorOccurred = (errorOccurred || ValidateStatistics(rbAvgMaxWatts));
            errorOccurred = (errorOccurred || ValidateStatistics(rbAvgMaxWkg));

            errorOccurred = (errorOccurred || ValidateStatistics(rbFtpHide));
            errorOccurred = (errorOccurred || ValidateStatistics(rbFtpWatts));
            errorOccurred = (errorOccurred || ValidateStatistics(rbFtpWkg));

            if (!errorOccurred)
            {
                try
                {

                    // Add or Update the Collector dictionary in the configuration.  
                    ZAMsettings.Settings.UpsertCollector(collectorBeingEdited);

                    ZAMsettings.CommitCachedConfiguration();

                    lvCollectors.BeginUpdate();

                    // Refresh the fields and the list view
                    Collectors_LoadFields(collectorBeingEdited);
                    itemBeingEdited.Refresh();

                    //CollectorListViewItem lvi = new CollectorListViewItem(collectorBeingEdited);
                    //lvCollectors.Items.Add(lvi);
                    //lvi.Selected = true;

                    //lvCollectors.Items.Remove(itemBeingEdited);
                    
                    lvCollectors.EndUpdate();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception occurred: " + ex.ToString(), "Error saving Collector", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    errorOccurred = true;
                }
                finally
                {
                    EditingCollectors = false;
                }
            }

        }

        private void btnStatsCancel_Click(object sender, EventArgs e)
        {
            ZAMsettings.RollbackCachedConfiguration();

            errorProvider.Clear();

            if (lvCollectors.SelectedItems.Count > 0)
            {
                CollectorListViewItem itemBeingEdited = (CollectorListViewItem)lvCollectors.SelectedItems[0];
                Collector userBeingEdited = itemBeingEdited.Collector;

                // If a newly added row, remove it and select the first one in list
                if (1 == 0)
                {
                    //lvCollectors.Items.Remove(itemBeingEdited);
                    //if (lvCollectors.Items.Count > 0)
                    //{
                    //    lvCollectors.Items[0].Selected = true;
                    //}
                }
                else
                {
                    // Refresh user from rolled back configuration
                    Collector refreshCollector = ZAMsettings.Settings.Collectors[userBeingEdited.Name];
                    Collectors_LoadFields(refreshCollector);

                    // This will clone the Collector so that the listview doesn't have a direct link to the configuration
                    itemBeingEdited.Collector = refreshCollector;
                }
            }
            EditingCollectors = false;
        }


        private void Statistics_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = ValidateStatistics(sender as Control);
        }

        private bool ValidateStatistics(Control control)
        {
            bool errorOccurred = false;

            Debug.Assert(errorProvider != null, "ValidateStatistics - errorProvider not set.");

            if (lvCollectors.SelectedItems.Count < 1)
                return false;

            errorProvider.SetError(control, "");

            Collector collector = ((CollectorListViewItem)lvCollectors.SelectedItems[0]).Collector;

            switch (control.Name)
            {
                case "rbAvgHide":
                case "rbAvgWatts":
                case "rbAvgWkg":
                    try
                    {
                        RadioButton b = (RadioButton)control;

                        if (b.Checked)
                            collector.FieldAvgDesc = (string)b.Tag;
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "rbAvgMaxHide":
                case "rbAvgMaxWatts":
                case "rbAvgMaxWkg":
                    try
                    {
                        RadioButton b = (RadioButton)control;

                        if (b.Checked)
                            collector.FieldAvgMaxDesc = (string)b.Tag;
                    }
                    catch (Exception ex)
                    {
                        errorProvider.SetError(control, ex.Message);
                        errorOccurred = true;
                    }
                    break;

                case "rbFtpHide":
                case "rbFtpWatts":
                case "rbFtpWkg":
                    try
                    {
                        RadioButton b = (RadioButton)control;

                        if (b.Checked)
                            collector.FieldFtpDesc = (string)b.Tag;
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
        public void Statistics_TooltipOnEnter(object sender, EventArgs e)
        {
            HandleTooltipsStatistics(sender as Control, true);
        }
        public void Statistics_TooltipOnLeave(object sender, EventArgs e)
        {
            HandleTooltipsStatistics(sender as Control, false);
        }

        public void HandleTooltipsStatistics(Control control, bool isEntering)
        {
            Debug.Assert(toolStripStatusLabel != null, "ValidateStatistics - StatusStrip not set.");

            if (!isEntering)
            {
                toolStripStatusLabel.Text = "";
                return;
            }

            switch (control.Name)
            {
                case "rbAvgHide":
                case "rbAvgWatts":
                case "rbAvgWkg":
                case "rbAvgMaxHide":
                case "rbAvgMaxWatts":
                case "rbAvgMaxWkg":
                case "rbFtpHide":
                case "rbFtpWatts":
                case "rbFtpWkg":
                    toolStripStatusLabel.Text = "Select how to display units on monitor window.";
                    break;
            }
        }

        private void ListView_Resize(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            UserControlBase.HideHorizontalScrollBar(sender as ListView);
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

            this.tbDescStatistics.BackColor = this.BackColor;
            //this.tbDuration.BackColor = this.BackColor;
            //this.lvCollectors.BackColor = this.BackColor;
            //this.btnStatsEdit.BackColor = this.BackColor;

            //UserControlBase.SetListViewHeaderColor(ref this.lvCollectors, this.BackColor, this.ForeColor);

        }

        protected override void Parent_ForeColorChanged(object sender, EventArgs e)
        {
            base.Parent_ForeColorChanged(sender, e);

            this.tbDescStatistics.ForeColor = this.ForeColor;
            this.gbCollectors.ForeColor = this.ForeColor;
            //this.tbDuration.ForeColor = this.ForeColor;
            //this.lvCollectors.ForeColor = this.ForeColor;
            //this.btnStatsEdit.ForeColor = this.ForeColor;

            //UserControlBase.SetListViewHeaderColor(ref this.lvCollectors, this.BackColor, this.ForeColor);
        }

    }
}
