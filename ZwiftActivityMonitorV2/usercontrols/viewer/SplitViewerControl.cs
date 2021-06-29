using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.ComponentModel;
using System.Windows.Threading;

namespace ZwiftActivityMonitorV2
{

    public partial class SplitViewerControl : ViewerUserControlEx
    {
        private enum DetailColumn
        {
            SplitNumber = 0,
            SplitTime,
            SplitSpeed,
            SplitDistance,
            TotalTime,
            Delta,
            Blank,
            Split_SpeedDisplayType,
            Split_DistanceDisplayType,
        }

        private enum SummaryColumn
        {
            Reserved = 0,
            GoalDistance,
            GoalSpeed,
            GoalTime,
            Blank
        }

        #region DetailRow class

        /// <summary>
        /// The class determines the columns available in the Detail DataGridView
        /// </summary>
        protected class DetailRow : NotifyPropertyChangedBase
        {
            //Add the [Browsable(false)] attribute to any public properties you don't want columns created for in the DataGridView

            public int SplitNumber { get; set; }
            public TimeSpan SplitTime { get { return this.mSplitTime; } set { this.SetProperty<TimeSpan>(ref this.mSplitTime, value); } }
            public string SplitSpeed { get { return this.mSplitSpeed; } set { this.SetProperty<string>(ref this.mSplitSpeed, value); } }
            public string SplitDistance { get { return this.mSplitDistance; } set { this.SetProperty<string>(ref this.mSplitDistance, value); } }
            public TimeSpan TotalTime { get { return this.mTotalTime; } set { this.SetProperty<TimeSpan>(ref this.mTotalTime, value); } }
            public string Delta { get { return this.mDelta; } set { this.SetProperty<string>(ref this.mDelta, value); } }
            public string Blank { get; set; }

            public SpeedDisplayType Split_SpeedDisplayType
            {
                get { return this.mSplit_SpeedDisplayType; }
                set
                {
                    this.SetProperty<SpeedDisplayType>(ref this.mSplit_SpeedDisplayType, value);
                    UpdateSplitSpeed(value);
                }
            }

            public DistanceDisplayType Split_DistanceDisplayType
            {
                get { return this.mSplit_DistanceDisplayType; }
                set
                {
                    this.SetProperty<DistanceDisplayType>(ref this.mSplit_DistanceDisplayType, value);
                    UpdateSplitDistance(value);
                }
            }

            private string mDelta;
            private TimeSpan mSplitTime;
            private string mSplitSpeed;
            private double mSplitSpeedKph;
            private double mSplitSpeedMph;
            private string mSplitDistance;
            private double mSplitDistanceMi;
            private double mSplitDistanceKm;
            private TimeSpan mTotalTime;
            private TimeSpan? mDeltaTime;
            private SpeedDisplayType mSplit_SpeedDisplayType;
            private DistanceDisplayType mSplit_DistanceDisplayType;

            private SpeedDisplayType mCurrentSpeedDisplayType;
            private DistanceDisplayType mCurrentDistanceDisplayType;

            public DetailRow(int splitNumber)
            {
                this.SplitNumber = splitNumber;
            }

            public void SetCurrentMeasurementSystemType(MeasurementSystemType type)
            {
                if (type == MeasurementSystemType.Imperial)
                {
                    this.mCurrentSpeedDisplayType = SpeedDisplayType.MilesPerHour;
                    this.mCurrentDistanceDisplayType = DistanceDisplayType.Miles;
                }
                else
                {
                    this.mCurrentSpeedDisplayType = SpeedDisplayType.KilometersPerHour;
                    this.mCurrentDistanceDisplayType = DistanceDisplayType.Kilometers;
                }
            }

            public SpeedDisplayType GetPreferredType(SpeedDisplayType currentType)
            {
                SpeedDisplayType preferredType = currentType == SpeedDisplayType.Both ? this.mCurrentSpeedDisplayType : currentType;

                return preferredType;
            }

            public DistanceDisplayType GetPreferredType(DistanceDisplayType currentType)
            {
                DistanceDisplayType preferredType = currentType == DistanceDisplayType.Both ? this.mCurrentDistanceDisplayType : currentType;

                return preferredType;
            }

            /// <summary>
            /// Updates the displayed column appropriately
            /// </summary>
            /// <param name="updatedType"></param>
            private void UpdateSplitSpeed(SpeedDisplayType updatedType)
            {
                SpeedDisplayType preferredType = GetPreferredType(this.Split_SpeedDisplayType);

                if (preferredType == updatedType)
                {
                    switch (updatedType)
                    {
                        case SpeedDisplayType.KilometersPerHour:
                            this.SplitSpeed = this.SplitSpeedKph > 0 ? this.SplitSpeedKph.ToString("0.0") : "";
                            break;

                        case SpeedDisplayType.MilesPerHour:
                            this.SplitSpeed = this.SplitSpeedMph > 0 ? this.SplitSpeedMph.ToString("0.0") : "";
                            break;
                    }
                }
                else if (preferredType == SpeedDisplayType.None)
                {
                    this.SplitSpeed = "";
                }
            }

            /// <summary>
            /// Updates the displayed column appropriately
            /// </summary>
            /// <param name="updatedType"></param>
            private void UpdateSplitDistance(DistanceDisplayType updatedType)
            {
                DistanceDisplayType preferredType = GetPreferredType(this.Split_DistanceDisplayType);

                if (preferredType == updatedType)
                {
                    switch (updatedType)
                    {
                        case DistanceDisplayType.Kilometers:
                            this.SplitDistance = this.SplitDistanceKm > 0 ? this.SplitDistanceKm.ToString("0.0") : "";
                            break;

                        case DistanceDisplayType.Miles:
                            this.SplitDistance = this.SplitDistanceMi > 0 ? this.SplitDistanceMi.ToString("0.0") : "";
                            break;
                    }
                }
                else if (preferredType == DistanceDisplayType.None)
                {
                    this.SplitDistance = "";
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double SplitSpeedKph
            {
                get { return this.mSplitSpeedKph; }
                set
                {
                    this.mSplitSpeedKph = value;
                    this.UpdateSplitSpeed(SpeedDisplayType.KilometersPerHour);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double SplitSpeedMph
            {
                get { return this.mSplitSpeedMph; }
                set
                {
                    this.mSplitSpeedMph = value;
                    this.UpdateSplitSpeed(SpeedDisplayType.MilesPerHour);
                }
            }
            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double SplitDistanceKm
            {
                get { return this.mSplitDistanceKm; }
                set
                {
                    this.mSplitDistanceKm = value;
                    this.UpdateSplitDistance(DistanceDisplayType.Kilometers);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double SplitDistanceMi
            {
                get { return this.mSplitDistanceMi; }
                set
                {
                    this.mSplitDistanceMi = value;
                    this.UpdateSplitDistance(DistanceDisplayType.Miles);
                }
            }

            [Browsable(false)]
            public TimeSpan? DeltaTime
            {
                get { return mDeltaTime; }
                set
                {
                    mDeltaTime = value;
                    if (mDeltaTime.HasValue)
                    {
                        TimeSpan std = (TimeSpan)mDeltaTime;
                        bool negated = false;

                        if (std.TotalSeconds < 0)
                        {
                            std = std.Negate();
                            negated = true;
                        }

                        this.Delta = $"{(negated ? "-" : "+")}{(std.Minutes > 0 ? std.ToString("m'@QT's'\"'").Replace("@QT", "\'") : std.ToString("s'\"'"))}";
                    }
                    else
                    {
                        this.Delta = "";
                    }
                }
            }
        }

        #endregion

        #region SummaryRow class

        /// <summary>
        /// The class determines the columns available in the Summary DataGridView
        /// </summary>
        protected class SummaryRow : NotifyPropertyChangedBase
        {
            //Add the [Browsable(false)] attribute to any public properties you don't want columns created for in the DataGridView

            public string Reserved { get; set; }
            public string GoalDistance { get { return this.mGoalDistance; } set { this.SetProperty<string>(ref this.mGoalDistance, value); } }
            public string GoalSpeed { get { return this.mGoalSpeed; } set { this.SetProperty<string>(ref this.mGoalSpeed, value); } }
            public string GoalTime { get { return this.mGoalTime; } set { this.SetProperty<string>(ref this.mGoalTime, value); } }
            public string Blank { get; set; }

            private string mGoalDistance;
            private string mGoalSpeed;
            private string mGoalTime;

            private SpeedDisplayType mCurrentSpeedDisplayType;

            public void SetCurrentMeasurementSystemType(MeasurementSystemType type)
            {
                if (type == MeasurementSystemType.Imperial)
                {
                    this.mCurrentSpeedDisplayType = SpeedDisplayType.MilesPerHour;
                }
                else
                {
                    this.mCurrentSpeedDisplayType = SpeedDisplayType.KilometersPerHour;
                }
            }

            public SpeedDisplayType GetPreferredType(SpeedDisplayType currentType)
            {
                SpeedDisplayType preferredType = currentType == SpeedDisplayType.Both ? this.mCurrentSpeedDisplayType : currentType;

                return preferredType;
            }
        }

        #endregion

        private BindingList<DetailRow> DetailRows = new();
        private BindingList<SummaryRow> SummaryRows = new();
        private SyncBindingSource DetailBindingSource { get; set; }
        private SyncBindingSource SummaryBindingSource { get; set; }

        private SplitsManagerV2 mSplitsManager;
        private Dispatcher mDispatcher;

        public SplitViewerControl()
        {
            InitializeComponent();
        }

        private void ViewControl_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;

            InitializeDetailDataGrid();
            //LoadDetailDataGrid();

            InitializeSummaryDataGrid();
            LoadSummaryDataGrid();

            // Subscribe to any SystemConfig changes
            ZAMsettings.SystemConfigChanged += ZAMsettings_SystemConfigChanged;
            ZAMsettings.ZPMonitorService.CollectionStatusChanged += ZPMonitorService_CollectionStatusChanged;

            this.SetupDisplayForCurrentUserProfile();


            // for handling UI events
            mDispatcher = Dispatcher.CurrentDispatcher;

            mSplitsManager = new();

            mSplitsManager.SplitGoalCompletedEvent += SplitsManager_SplitGoalCompletedEvent;
            mSplitsManager.SplitUpdatedEvent += SplitsManager_SplitUpdatedOrCompleted;
            mSplitsManager.SplitCompletedEvent += SplitsManager_SplitUpdatedOrCompleted;

            // Trigger a resize so that dgSummary can size itself appropriately
            this.OnResize(new EventArgs());
        }

        /// <summary>
        /// A delegate used solely by the SplitsManager_SplitUpdatedOrCompleted and SplitsManager_SplitGoalCompletedEvent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void SplitEventHandlerDelegate(object sender, SplitEventArgs e);

        /// <summary>
        /// Occurs each time a split gets updated or completes.  Allows for UI update by marshalling the call accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitsManager_SplitUpdatedOrCompleted(object sender, SplitEventArgs e)
        {
            if (!mDispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                mDispatcher.BeginInvoke(new SplitEventHandlerDelegate(SplitsManager_SplitUpdatedOrCompleted), new object[] { sender, e });
                return;
            }

            DetailRow detailRow = DetailRows.FirstOrDefault(r => r.SplitNumber == e.SplitNumber);

            if (detailRow == null)
            {
                detailRow = new(e.SplitNumber)
                {
                    SplitTime = e.SplitTime,
                    SplitDistanceKm = e.TotalKmTravelled,
                    SplitDistanceMi = e.TotalMiTravelled,
                    SplitSpeedMph = e.SplitSpeedMph,
                    SplitSpeedKph = e.SplitSpeedKph,
                    TotalTime = e.TotalTime,
                    DeltaTime = e.DeltaTime,
                };
                DetailRows.Add(detailRow);
                detailRow.PropertyChanged += DetailOrSummaryRow_PropertyChanged;
            }
            else
            {
                detailRow.SplitTime = e.SplitTime;
                detailRow.SplitDistanceKm = e.TotalKmTravelled;
                detailRow.SplitDistanceMi = e.TotalMiTravelled;
                detailRow.SplitSpeedMph = e.SplitSpeedMph;
                detailRow.SplitSpeedKph = e.SplitSpeedKph;
                detailRow.TotalTime = e.TotalTime;
                detailRow.DeltaTime = e.DeltaTime;
            }

            //if (lvSplits.Items.ContainsKey(splitItem.SplitNumber)) 
            //{
            //    SplitListViewItem item = (SplitListViewItem)lvSplits.Items[splitItem.SplitNumber];
            //    item.SplitItem = splitItem; // Replace with current splitItem object and refresh
            //    item.Refresh();
            //}
            //else
            //{
            //    if (e.SplitNumber > 1)
            //    {
            //        // remove any color coding from previous split
            //        string prevSplit = (e.SplitNumber - 1).ToString();
            //        if (lvSplits.Items.ContainsKey(prevSplit))
            //        {
            //            SplitListViewItem item = (SplitListViewItem)lvSplits.Items[prevSplit];
            //            item.ClearDeltaBackground();
            //        }
            //    }

            //    lvSplits.Items.Add(new SplitListViewItem(splitItem));
            //    lvSplits.Sort();
            //}

        }

        /// <summary>
        /// Occurs each time a split gets updated or completes.  Allows for UI update by marshalling the call accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitsManager_SplitGoalCompletedEvent(object sender, SplitEventArgs e)
        {
            if (!mDispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                mDispatcher.BeginInvoke(new SplitEventHandlerDelegate(SplitsManager_SplitGoalCompletedEvent), new object[] { sender, e });
                return;
            }
        }

        /// <summary>
        /// Allow owner class to tie into the SplitGoalCompletedEvent and SplitCompletedEvent.  This allows the MainForm to bring this control into focus.
        /// </summary>
        public event EventHandler<SplitEventArgs> SplitGoalCompletedEvent
        {
            add
            {
                mSplitsManager.SplitGoalCompletedEvent += value;
            }
            remove
            {
                mSplitsManager.SplitGoalCompletedEvent -= value;
            }
        }

        public event EventHandler<SplitEventArgs> SplitCompletedEvent
        {
            add
            {
                mSplitsManager.SplitCompletedEvent += value;
            }
            remove
            {
                mSplitsManager.SplitCompletedEvent -= value;
            }
        }


        private void InitializeDetailDataGrid()
        {
            // Note: anytime rows are added to the List, the BindingSource must be recreated (or maybe just a reset on the BindingSource)
            this.DetailBindingSource = new SyncBindingSource();
            this.DetailBindingSource.DataSource = this.DetailRows;

            this.dgDetail.DataSource = this.DetailBindingSource;

            // Allow column headers to wrap to a second line
            this.dgDetail.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

            this.dgDetail.Columns[(int)DetailColumn.SplitNumber].Width = 36;
            this.dgDetail.Columns[(int)DetailColumn.SplitNumber].HeaderText = SplitViewMetricEnum.Instance.GetColumnHeaderText(SplitViewMetricType.DetailSplitNumber);
            this.dgDetail.Columns[(int)DetailColumn.SplitNumber].Tag = SplitViewMetricType.DetailSplitNumber;

            this.dgDetail.Columns[(int)DetailColumn.SplitTime].Width = 48;
            this.dgDetail.Columns[(int)DetailColumn.SplitTime].HeaderText = SplitViewMetricEnum.Instance.GetColumnHeaderText(SplitViewMetricType.DetailSplitTime);
            this.dgDetail.Columns[(int)DetailColumn.SplitTime].DefaultCellStyle.Format = "mm\\:ss";
            this.dgDetail.Columns[(int)DetailColumn.SplitTime].Tag = SplitViewMetricType.DetailSplitTime;

            this.dgDetail.Columns[(int)DetailColumn.SplitSpeed].Width = 48;
            this.dgDetail.Columns[(int)DetailColumn.SplitSpeed].HeaderText = SplitViewMetricEnum.Instance.GetColumnHeaderText(SplitViewMetricType.DetailSplitSpeed);
            this.dgDetail.Columns[(int)DetailColumn.SplitSpeed].Tag = SplitViewMetricType.DetailSplitSpeed;

            this.dgDetail.Columns[(int)DetailColumn.SplitDistance].Width = 50;
            this.dgDetail.Columns[(int)DetailColumn.SplitDistance].HeaderText = SplitViewMetricEnum.Instance.GetColumnHeaderText(SplitViewMetricType.DetailSplitDistance);
            this.dgDetail.Columns[(int)DetailColumn.SplitDistance].DefaultCellStyle.Format = "F1";
            this.dgDetail.Columns[(int)DetailColumn.SplitDistance].Tag = SplitViewMetricType.DetailSplitDistance;

            this.dgDetail.Columns[(int)DetailColumn.TotalTime].Width = 72;
            this.dgDetail.Columns[(int)DetailColumn.TotalTime].HeaderText = SplitViewMetricEnum.Instance.GetColumnHeaderText(SplitViewMetricType.DetailTotalTime);
            this.dgDetail.Columns[(int)DetailColumn.TotalTime].DefaultCellStyle.Format = "hh\\:mm\\:ss";
            this.dgDetail.Columns[(int)DetailColumn.TotalTime].Tag = SplitViewMetricType.DetailTotalTime;

            this.dgDetail.Columns[(int)DetailColumn.Delta].Width = 60;
            this.dgDetail.Columns[(int)DetailColumn.Delta].HeaderText = SplitViewMetricEnum.Instance.GetColumnHeaderText(SplitViewMetricType.DetailDeltaTime);
            this.dgDetail.Columns[(int)DetailColumn.Delta].Tag = SplitViewMetricType.DetailDeltaTime;

            this.dgDetail.Columns[(int)DetailColumn.Blank].Width = 5; // Five seems to be minimum size, even if set to zero
            this.dgDetail.Columns[(int)DetailColumn.Blank].HeaderText = "";
            this.dgDetail.Columns[(int)DetailColumn.Blank].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.dgDetail.Columns[(int)DetailColumn.Split_SpeedDisplayType].Width = 5; // Five seems to be minimum size, even if set to zero
            this.dgDetail.Columns[(int)DetailColumn.Split_SpeedDisplayType].HeaderText = "";
            this.dgDetail.Columns[(int)DetailColumn.Split_SpeedDisplayType].Visible = false;

            this.dgDetail.Columns[(int)DetailColumn.Split_DistanceDisplayType].Width = 5; // Five seems to be minimum size, even if set to zero
            this.dgDetail.Columns[(int)DetailColumn.Split_DistanceDisplayType].HeaderText = "";
            this.dgDetail.Columns[(int)DetailColumn.Split_DistanceDisplayType].Visible = false;

            foreach (DataGridViewColumn c in this.dgDetail.Columns)
            {
                c.MinimumWidth = c.Width;
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Set the row default cell font to the grid's default cell font.  Then clear the grid default so the row default is all that has to change. 
            this.dgDetail.RowsDefaultCellStyle.Font = this.dgDetail.DefaultCellStyle.Font;
            this.dgDetail.DefaultCellStyle.Font = null;

            // These must be set here not in designer otherwise column widths change. not sure why
            dgDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            this.dgDetail.ShowFocus = false;
        }

        //private void LoadDetailDataGrid()
        //{
        //    DetailRow r = new(1)
        //    {
        //        SplitTime = TimeSpan.Zero,
        //        SplitSpeed = "88.8",
        //        SplitDistance = "888.8",
        //        TotalTime = "88:88:88",
        //        DeltaTime = "+88:88",
        //    };
        //    this.DetailRows.Add(r);

        //    // A height of 19 is minimum when using Segoe UI 9pt font
        //    this.dgDetail.Rows[0].MinimumHeight = DataGridRowMinimumHeight;

        //    //dgDetail.Sort(dgDetail.Columns[(int)DetailColumn.SplitNumber], ListSortDirection.Descending);

        //}

        private void InitializeSummaryDataGrid()
        {
            // Note: anytime rows are added to the List, the BindingSource must be recreated (or maybe just a reset on the BindingSource)
            this.SummaryBindingSource = new SyncBindingSource();
            this.SummaryBindingSource.DataSource = this.SummaryRows;

            this.dgSummary.DataSource = this.SummaryBindingSource;


            this.dgSummary.Columns[(int)SummaryColumn.Reserved].Width = 48;
            this.dgSummary.Columns[(int)SummaryColumn.Reserved].HeaderText = "";

            this.dgSummary.Columns[(int)SummaryColumn.GoalDistance].Width = 50;
            this.dgSummary.Columns[(int)SummaryColumn.GoalDistance].HeaderText = "Distance";

            this.dgSummary.Columns[(int)SummaryColumn.GoalSpeed].Width = 48;
            this.dgSummary.Columns[(int)SummaryColumn.GoalSpeed].HeaderText = "Speed";

            this.dgSummary.Columns[(int)SummaryColumn.GoalTime].Width = 48;
            this.dgSummary.Columns[(int)SummaryColumn.GoalTime].HeaderText = "Time";

            // Use the last blank column to fill the gap if user resizes
            this.dgSummary.Columns[(int)SummaryColumn.Blank].Width = 5; // Five seems to be minimum size, even if set to zero
            this.dgSummary.Columns[(int)SummaryColumn.Blank].HeaderText = "";
            this.dgSummary.Columns[(int)SummaryColumn.Blank].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            foreach (DataGridViewColumn c in this.dgSummary.Columns)
            {
                c.MinimumWidth = c.Width;
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // Set the row default cell font to the grid's default cell font.  Then clear the grid default so the row default is all that has to change. 
            this.dgSummary.RowsDefaultCellStyle.Font = this.dgSummary.DefaultCellStyle.Font;
            this.dgSummary.DefaultCellStyle.Font = null;

            // These must be set here not in designer otherwise column widths change. not sure why
            dgSummary.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            this.dgSummary.ShowFocus = false;
        }
        private void LoadSummaryDataGrid()
        {
            SummaryRow r = new()
            {
                Reserved = "Goal",
                GoalDistance = "888.8",
                GoalSpeed = "88.8",
                GoalTime = "88:88:88",
            };
            this.SummaryRows.Add(r);

            // A height of 19 is minimum when using Segoe UI 9pt font
            this.dgSummary.Rows[0].MinimumHeight = DataGridRowMinimumHeight;
        }

        /// <summary>
        /// Occurs whenever a property value changes.
        /// This allows changing the title to the Speed column, depending on the underlying data UOM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DetailOrSummaryRow_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is DetailRow detailRow)
            {
                if (e.PropertyName == DetailColumn.SplitSpeed.ToString())
                {
                    SpeedDisplayType preferredType = detailRow.GetPreferredType(detailRow.Split_SpeedDisplayType);

                    switch (preferredType)
                    {
                        case SpeedDisplayType.KilometersPerHour:
                        case SpeedDisplayType.MilesPerHour:
                            this.dgDetail.Columns[(int)DetailColumn.SplitSpeed].HeaderText = SpeedDisplayEnum.Instance.GetColumnHeaderText(preferredType);
                            break;

                        default:
                            this.dgDetail.Columns[(int)DetailColumn.SplitSpeed].HeaderText = "";
                            break;
                    }
                }
                else if (e.PropertyName == DetailColumn.SplitDistance.ToString())
                {
                    DistanceDisplayType preferredType = detailRow.GetPreferredType(detailRow.Split_DistanceDisplayType);

                    switch (preferredType)
                    {
                        case DistanceDisplayType.Kilometers:
                        case DistanceDisplayType.Miles:
                            this.dgDetail.Columns[(int)DetailColumn.SplitDistance].HeaderText = DistanceDisplayEnum.Instance.GetColumnHeaderText(preferredType);
                            break;

                        default:
                            this.dgDetail.Columns[(int)DetailColumn.SplitDistance].HeaderText = "";
                            break;
                    }
                }
            }
        }

        private void ZPMonitorService_CollectionStatusChanged(object sender, CollectionStatusChangedEventArgs e)
        {
            Debug.WriteLine($"{this.GetType()}.ZPMonitorService_CollectionStatusChanged - {e.Action}");
        }


        private void ZAMsettings_SystemConfigChanged(object sender, EventArgs e)
        {
            this.SetupDisplayForCurrentUserProfile();

            Debug.WriteLine($"ZAMsettings_SystemConfigChanged - {this.GetType()}");
        }


        private void SetupDisplayForCurrentUserProfile()
        {
            this.dgDetail.Columns[(int)DetailColumn.SplitDistance].Visible = CurrentUserProfile.SplitViewColumnSettings.Visibility[SplitViewMetricType.DetailSplitDistance]; ;
            this.dgDetail.Columns[(int)DetailColumn.SplitNumber].Visible = CurrentUserProfile.SplitViewColumnSettings.Visibility[SplitViewMetricType.DetailSplitNumber];
            this.dgDetail.Columns[(int)DetailColumn.SplitSpeed].Visible = CurrentUserProfile.SplitViewColumnSettings.Visibility[SplitViewMetricType.DetailSplitSpeed];
            this.dgDetail.Columns[(int)DetailColumn.SplitTime].Visible = CurrentUserProfile.SplitViewColumnSettings.Visibility[SplitViewMetricType.DetailSplitTime];
            this.dgDetail.Columns[(int)DetailColumn.TotalTime].Visible = CurrentUserProfile.SplitViewColumnSettings.Visibility[SplitViewMetricType.DetailTotalTime];

            //SummaryRows[0].GoalDistance = "";
            //SummaryRows[0].GoalSpeed= "";
            //SummaryRows[0].GoalTime = "";
            //SummaryRows[0].AP_PowerDisplayType = CurrentUserProfile.ActivityViewSummaryRowSettings.PowerValues[ActivityViewMetricType.SummaryAP].Key;
            //SummaryRows[0].NP_PowerDisplayType = CurrentUserProfile.ActivityViewSummaryRowSettings.PowerValues[ActivityViewMetricType.SummaryNP].Key;
            //SummaryRows[0].AS_SpeedDisplayType = CurrentUserProfile.ActivityViewSummaryRowSettings.SpeedValues[ActivityViewMetricType.SummaryAS].Key;
        }

        private void ViewControl_Resize(object sender, EventArgs e)
        {
            //Debug.WriteLine($"ViewControl_Resize - Size: {this.Size}");

            // TableLayoutPanel tlPanel helps keep things organized when resizing.
            //
            // tlPanel has been set up in designer with the following:
            // tlPanel.RowStyles[0].SizeType = SizeType.Percent;
            // tlPanel.RowStyles[0].Height = 100; // 100%
            // tlPanel.RowStyles[1].SizeType = SizeType.Absolute;
            // tlPanel.RowStyles[1].Height = 50; // default size


            // Calculate the height required to show all of dgSummary
            int dgSummaryHeight = dgSummary.Rows.GetRowsHeight(DataGridViewElementStates.None) + dgSummary.ColumnHeadersHeight;
            dgSummaryHeight += (dgSummary.Controls.OfType<HScrollBar>().FirstOrDefault(c => c.Visible) != null ? SystemInformation.HorizontalScrollBarHeight : 0);

            // Set the height of the lower row for dgSummary, the upper row will automatically adjust
            tlPanel.RowStyles[1].Height = dgSummaryHeight;

            // The following is not needed but just shown for completeness
            //int dgDetailHeight = dgDetail.Rows.GetRowsHeight(states) + dgDetail.ColumnHeadersHeight;
            //dgDetailHeight += (dgDetail.Controls.OfType<HScrollBar>().FirstOrDefault(c => c.Visible) != null ? SystemInformation.HorizontalScrollBarHeight : 0);
        }

        private void ViewControl_SizeChanged(object sender, EventArgs e)
        {
            Debug.WriteLine($"ViewControl_SizeChanged - Size: {this.Size}");
        }

        /// <summary>
        /// Determine action on cell mouse click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;

            if (dataGridView == null)
                return;

            if (e.Button != MouseButtons.Right)
                return;

            if (e.RowIndex == -1) // Right click on column headers?
            {
                this.dataGridView_ColumnHeaderMouseClick(dataGridView, e);
            }
            else
            {
                this.dataGridView_RowMouseClick(dataGridView, e);
            }
        }

        private void dataGridView_RowMouseClick(DataGridView dataGridView, DataGridViewCellMouseEventArgs e)
        {
            int metricType;
            ContextMenuStrip menuStrip = new ContextMenuStrip();

            if (dataGridView == this.dgDetail)
            {
                Dictionary<int, int> distanceDisplayColumnMap = new();
                distanceDisplayColumnMap.Add((int)DetailColumn.SplitDistance, (int)DetailColumn.Split_DistanceDisplayType);

                //switch (e.ColumnIndex)
                //{
                //    case (int)DetailColumn.SplitDistance:
                //        // map the right-clicked column to the column that stores the type of power display
                //        int distanceDisplayColumnIndex = distanceDisplayColumnMap[e.ColumnIndex];

                //        // the durationType is stored in the row
                //        //int durationType = (int)dataGridView[(int)DetailColumn.PeriodSecs, e.RowIndex].Value;

                //        // the SplitViewMetricType is stored in the column header tag
                //        metricType = (int)dataGridView.Columns[e.ColumnIndex].Tag;

                //        foreach (var kvp in DistanceDisplayEnum.Instance.GetItems())
                //        {
                //            var mi = new ToolStripMenuItem(DistanceDisplayEnum.Instance.GetMenuItemText(kvp.Key))
                //            {
                //                CheckOnClick = true,
                //                Tag = new object[] { distanceDisplayColumnIndex, (int)kvp.Key, metricType, dataGridView }, // pass required values to the handler event
                //                Checked = (DistanceDisplayType)dataGridView[distanceDisplayColumnIndex, e.RowIndex].Value == kvp.Key,
                //            };
                //            mi.CheckedChanged += distanceContextMenu_CheckChanged;
                //            menuStrip.Items.Add(mi);
                //        }

                //        menuStrip.Show(Cursor.Position);
                //        break;
                //}
            }
            //else if (dataGridView == this.dgSummary)
            //{
            //    Dictionary<int, int> powerDisplayColumnMap = new();
            //    powerDisplayColumnMap.Add((int)SummaryColumn.AP, (int)SummaryColumn.AP_PowerDisplayType);
            //    powerDisplayColumnMap.Add((int)SummaryColumn.NP, (int)SummaryColumn.NP_PowerDisplayType);

            //    Dictionary<int, int> speedDisplayColumnMap = new();
            //    speedDisplayColumnMap.Add((int)SummaryColumn.AS, (int)SummaryColumn.AS_SpeedDisplayType);

            //    switch (e.ColumnIndex)
            //    {
            //        case (int)SummaryColumn.AS:
            //            // map the right-clicked column to the column that stores the type of speed display
            //            int speedDisplayColumnIndex = speedDisplayColumnMap[e.ColumnIndex];

            //            // the ActivityViewMetricType is stored in the column header tag
            //            metricType = (int)dataGridView.Columns[e.ColumnIndex].Tag;

            //            foreach (var kvp in SpeedDisplayEnum.Instance.GetItems())
            //            {
            //                var mi = new ToolStripMenuItem(kvp.Value)
            //                {
            //                    CheckOnClick = true,
            //                    Tag = new object[] { e.RowIndex, speedDisplayColumnIndex, (int)kvp.Key, metricType, dataGridView }, // pass required values to the handler event
            //                    Checked = (SpeedDisplayType)dataGridView[speedDisplayColumnIndex, e.RowIndex].Value == kvp.Key,
            //                };
            //                mi.CheckedChanged += speedContextMenu_CheckChanged;
            //                menuStrip.Items.Add(mi);
            //            }

            //            menuStrip.Show(Cursor.Position);
            //            break;

            //        case (int)SummaryColumn.AP:
            //        case (int)SummaryColumn.NP:
            //            // map the right-clicked column to the column that stores the type of power display
            //            int powerDisplayColumnIndex = powerDisplayColumnMap[e.ColumnIndex];

            //            // the ActivityViewMetricType is stored in the column header tag
            //            metricType = (int)dataGridView.Columns[e.ColumnIndex].Tag;

            //            foreach (var kvp in PowerDisplayEnum.Instance.GetItems())
            //            {
            //                var mi = new ToolStripMenuItem(kvp.Value)
            //                {
            //                    CheckOnClick = true,
            //                    Tag = new object[] { e.RowIndex, powerDisplayColumnIndex, (int)kvp.Key, metricType, null, dataGridView }, // pass required values to the handler event
            //                    Checked = (PowerDisplayType)dataGridView[powerDisplayColumnIndex, e.RowIndex].Value == kvp.Key,
            //                };
            //                mi.CheckedChanged += powerContextMenu_CheckChanged;
            //                menuStrip.Items.Add(mi);
            //            }

            //            menuStrip.Show(Cursor.Position);
            //            break;
            //    }
            //}
        }

        ///// <summary>
        ///// Handle CheckedChanged event for period collector row visibility
        ///// Collectors are in dgDetail only
        ///// </summary>
        ///// <param name="dataGridView"></param>
        ///// <param name="e"></param>
        //private void periodContextMenu_CheckChanged(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem item = (ToolStripMenuItem)sender;

        //    object[] tag = item.Tag as object[];
        //    int rowIndex = (int)tag[0];
        //    DurationType durationType = (DurationType)tag[1];

        //    if (item.Checked || dgDetail.Rows.GetRowCount(DataGridViewElementStates.Visible) > 1)
        //    {
        //        dgDetail.CurrentCell = null;
        //        dgDetail.Rows[rowIndex].Visible = item.Checked;

        //        // Place the CurrentCell on a valid (visible) cell
        //        // Failing to do this will make the row reappear when it is next updated.  (BindingSource.Position being on a hidden row is a bad thing).
        //        dgDetail.CurrentCell = dgDetail.FirstDisplayedCell;

        //        if (CurrentUserProfile.ActivityViewDetailRowSettings.ContainsKey(durationType))
        //        {
        //            ZAMsettings.BeginCachedConfiguration();
        //            CurrentUserProfile.ActivityViewDetailRowSettings[durationType].IsVisible = item.Checked;
        //            ZAMsettings.CommitCachedConfiguration();
        //        }
        //    }
        //}

        ///// <summary>
        ///// Handle CheckedChanged event for power UOM selection
        ///// Handles both dgDetail and dgSummary
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void powerContextMenu_CheckChanged(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem item = (ToolStripMenuItem)sender;
        //    object[] tag = item.Tag as object[];

        //    int rowIndex = (int)tag[0], powerDisplayColumnIndex = (int)tag[1];
        //    PowerDisplayType powerDisplayType = (PowerDisplayType)tag[2];
        //    ActivityViewMetricType metricType = (ActivityViewMetricType)tag[3];
        //    DurationType? durationType = (DurationType?)tag[4]; // included in dgDetail columns only
        //    DataGridView dataGridView = (DataGridView)tag[5];

        //    if (item.Checked)
        //    {
        //        // set the PowerDisplayType in the dataGridView.
        //        dataGridView[powerDisplayColumnIndex, rowIndex].Value = powerDisplayType;

        //        if (durationType.HasValue)
        //        {
        //            // Update configuration
        //            if (CurrentUserProfile.ActivityViewDetailRowSettings.ContainsKey(durationType.Value))
        //            {
        //                if (CurrentUserProfile.ActivityViewDetailRowSettings[durationType.Value].PowerValues.ContainsKey(metricType))
        //                {
        //                    ZAMsettings.BeginCachedConfiguration();
        //                    CurrentUserProfile.ActivityViewDetailRowSettings[durationType.Value].PowerValues[metricType] = PowerDisplayEnum.Instance.GetItem(powerDisplayType);
        //                    ZAMsettings.CommitCachedConfiguration();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            // Update configuration
        //            if (CurrentUserProfile.ActivityViewSummaryRowSettings.PowerValues.ContainsKey(metricType))
        //            {
        //                ZAMsettings.BeginCachedConfiguration();
        //                CurrentUserProfile.ActivityViewSummaryRowSettings.PowerValues[metricType] = PowerDisplayEnum.Instance.GetItem(powerDisplayType);
        //                ZAMsettings.CommitCachedConfiguration();
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// Handle CheckedChanged event for speed UOM selection
        ///// Currently speed is in dgSummary only
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void speedContextMenu_CheckChanged(object sender, EventArgs e)
        //{
        //    ToolStripMenuItem item = (ToolStripMenuItem)sender;

        //    if (item.Checked)
        //    {
        //        object[] tag = item.Tag as object[];
        //        int rowIndex = (int)tag[0], speedDisplayColumnIndex = (int)tag[1];
        //        SpeedDisplayType speedDisplayType = (SpeedDisplayType)tag[2];
        //        ActivityViewMetricType metricType = (ActivityViewMetricType)tag[3];
        //        DataGridView dataGridView = (DataGridView)tag[4];

        //        dataGridView[speedDisplayColumnIndex, rowIndex].Value = (SpeedDisplayType)speedDisplayType;

        //        // Update configuration
        //        if (CurrentUserProfile.ActivityViewSummaryRowSettings.PowerValues.ContainsKey(metricType))
        //        {
        //            ZAMsettings.BeginCachedConfiguration();
        //            CurrentUserProfile.ActivityViewSummaryRowSettings.SpeedValues[metricType] = SpeedDisplayEnum.Instance.GetItem(speedDisplayType);
        //            ZAMsettings.CommitCachedConfiguration();
        //        }
        //    }
        //}

        /// <summary>
        /// Handle mouse click on column headers and allow visibility change.
        /// Handles both dgDetail and dgSummary
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="e"></param>
        private void dataGridView_ColumnHeaderMouseClick(DataGridView dataGridView, DataGridViewCellMouseEventArgs e)
        {
            ContextMenuStrip menuStrip = new ContextMenuStrip();

            //Add the columns to context menu strip
            foreach (DataGridViewColumn c in dataGridView.Columns)
            {
                if (c.HeaderText != "" && c.Tag is SplitViewMetricType metricType) // exclude filler column
                {
                    var mi = new ToolStripMenuItem(SplitViewMetricEnum.Instance.GetMenuItemText(metricType))
                    {
                        Tag = new object[] { c.Index, metricType, dataGridView },
                        Checked = c.Visible,
                        CheckOnClick = true,
                        Enabled = (dataGridView != dgDetail || c.Index != (int)DetailColumn.SplitNumber),
                    };
                    mi.CheckedChanged += headerContextMenu_CheckedChanged;
                    menuStrip.Items.Add(mi);
                }
            }

            if (menuStrip.Items.Count > 0)
            {
                menuStrip.Show(Cursor.Position);
            }
        }

        /// <summary>
        /// Handle CheckedChanged event for column header visibility
        /// Generic to both dgDetail and dgSummary
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void headerContextMenu_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            int visibleCount = 0;

            object[] tag = item.Tag as object[];
            int colIndex = (int)tag[0];
            SplitViewMetricType metricType = (SplitViewMetricType)tag[1];
            DataGridView dataGridView = (DataGridView)tag[2];

            foreach (DataGridViewColumn c in dataGridView.Columns)
                if (c.Tag != null && c.Visible)
                    visibleCount++;

            if (item.Checked || visibleCount > 1)
            {
                dataGridView.Columns[colIndex].Visible = item.Checked;

                if (CurrentUserProfile.SplitViewColumnSettings.Visibility.ContainsKey(metricType))
                {
                    ZAMsettings.BeginCachedConfiguration();
                    CurrentUserProfile.SplitViewColumnSettings.Visibility[metricType] = item.Checked;
                    ZAMsettings.CommitCachedConfiguration();
                }

                // Trigger a resize so that dgSummary can size itself appropriately
                //this.OnResize(new EventArgs());
            }
        }
        #region Base Class Overrides
        protected override void HeaderForeColorChanged()
        {
            base.HeaderForeColorChanged();

            // change the text color on the data grid headers
            this.dgDetail.ColumnHeadersDefaultCellStyle.ForeColor = this.HeaderForeColor;
            this.dgSummary.ColumnHeadersDefaultCellStyle.ForeColor = this.HeaderForeColor;
        }

        protected override void RowFontChanged()
        {
            base.RowFontChanged();

            // change the font on the data grid rows
            this.dgDetail.RowsDefaultCellStyle.Font = this.RowFont;
            this.dgSummary.RowsDefaultCellStyle.Font = this.RowFont;

            // Trigger a resize so that dgSummary can size itself appropriately
            this.OnResize(new EventArgs());
        }

        protected override void RowBackColorChanged()
        {
            base.RowBackColorChanged();

            // change the back color on the data grid rows
            this.dgDetail.RowsDefaultCellStyle.BackColor = this.RowBackColor;
            this.dgSummary.RowsDefaultCellStyle.BackColor = this.RowBackColor;

            this.dgDetail.RowsDefaultCellStyle.SelectionBackColor = this.RowBackColor;  // this hides the Detail cell selection box by making it the same as row back color
            this.dgSummary.RowsDefaultCellStyle.SelectionBackColor = this.RowBackColor; // this hides the Summary cell selection box by making it the same as row back color 

            this.dgDetail.BackgroundColor = this.RowBackColor;
            this.dgSummary.BackgroundColor = this.RowBackColor;
            this.tlPanel.BackColor = this.RowBackColor;
        }

        protected override void RowForeColorChanged()
        {
            base.RowForeColorChanged();

            // change the fore color on the data grid rows
            this.dgDetail.RowsDefaultCellStyle.ForeColor = this.RowForeColor;
            this.dgSummary.RowsDefaultCellStyle.ForeColor = this.RowForeColor;

            this.dgDetail.RowsDefaultCellStyle.SelectionForeColor = this.RowForeColor;  // this blends the Detail cell selection text by making it the same as row fore color 
            this.dgSummary.RowsDefaultCellStyle.SelectionForeColor = this.RowForeColor; // this blends the Summary cell selection text by making it the same as row fore color 
        }
        #endregion

    }
}
