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
using Microsoft.Extensions.Logging;

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
        }

        private enum SummaryColumn
        {
            Reserved = 0,
            GoalSpeed,
            GoalDistance,
            GoalTime,
            Blank
        }

        #region DataGridViewManager class

        public class DataGridViewManager
        {
            public SortableBindingList<DetailRow> DetailRows { get; } = new();         // Sortable binding list collection
            public BindingList<SummaryRow> SummaryRows { get; } = new();
            private SyncBindingSource DetailBindingSource { get; } = new();
            private SyncBindingSource SummaryBindingSource { get; } = new();


            private SpeedDisplayType mAutoToggleSpeedDisplayType;
            private DistanceDisplayType mAutoToggleDistanceDisplayType;
            private DataGridViewEx DetailGrid;
            private DataGridViewEx SummaryGrid;

            private readonly ILogger<DataGridViewManager> Logger;

            public SpeedDisplayType SplitSpeed_PreferredDisplayType { get; internal set; }
            public DistanceDisplayType SplitDistance_PreferredDisplayType { get; internal set; }
            public SpeedDisplayType GoalSpeed_PreferredDisplayType { get; internal set; }
            public DistanceDisplayType GoalDistance_PreferredDisplayType { get; internal set; }

            public event EventHandler<SpeedDisplayTypeChangedEventArgs> SpeedDisplayTypeChangedEvent;
            public event EventHandler<DistanceDisplayTypeChangedEventArgs> DistanceDisplayTypeChangedEvent;

            public DataGridViewManager(DataGridViewEx detailGrid, DataGridViewEx summaryGrid)
            {
                if (ZAMsettings.LoggerFactory != null)
                    this.Logger = ZAMsettings.LoggerFactory.CreateLogger<DataGridViewManager>();

                this.DetailGrid = detailGrid;
                this.SummaryGrid = summaryGrid;

                // Note: anytime grid is sorted, the BindingSource will reset itself and things like cell colors and row visibility will be lost
                this.DetailBindingSource.DataSource = this.DetailRows;
                this.SummaryBindingSource.DataSource = this.SummaryRows;
            }

            /// <summary>
            /// Call this after enrolling in the TypeChanged events to get header's properly named
            /// </summary>
            public void Initialize()
            {
                this.InitializeDetailDataGrid();

                this.InitializeSummaryDataGrid();

                // Initialize auto-toggle and force an initial column header update that will observe user selections.
                this.SetAutoToggleMeasurementSystem(MeasurementSystemType.Imperial, true);
            }

            #region Detail and Summary Grid initialization

            private void InitializeDetailDataGrid()
            {
                this.DetailGrid.DataSource = this.DetailBindingSource;

                // Allow column headers to wrap to a second line
                this.DetailGrid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

                this.DetailGrid.Columns[(int)DetailColumn.SplitNumber].Width = 36;
                this.DetailGrid.Columns[(int)DetailColumn.SplitNumber].HeaderText = SplitViewMetricEnum.Instance.GetColumnHeaderText(SplitViewMetricType.DetailSplitNumber);
                this.DetailGrid.Columns[(int)DetailColumn.SplitNumber].Tag = SplitViewMetricType.DetailSplitNumber;

                this.DetailGrid.Columns[(int)DetailColumn.SplitTime].Width = 51;
                this.DetailGrid.Columns[(int)DetailColumn.SplitTime].HeaderText = SplitViewMetricEnum.Instance.GetColumnHeaderText(SplitViewMetricType.DetailSplitTime);
                this.DetailGrid.Columns[(int)DetailColumn.SplitTime].DefaultCellStyle.Format = "mm\\:ss";
                this.DetailGrid.Columns[(int)DetailColumn.SplitTime].Tag = SplitViewMetricType.DetailSplitTime;

                this.DetailGrid.Columns[(int)DetailColumn.SplitSpeed].Width = 48;
                this.DetailGrid.Columns[(int)DetailColumn.SplitSpeed].HeaderText = SplitViewMetricEnum.Instance.GetColumnHeaderText(SplitViewMetricType.DetailSplitSpeed);
                this.DetailGrid.Columns[(int)DetailColumn.SplitSpeed].Tag = SplitViewMetricType.DetailSplitSpeed;

                this.DetailGrid.Columns[(int)DetailColumn.SplitDistance].Width = 50;
                this.DetailGrid.Columns[(int)DetailColumn.SplitDistance].HeaderText = SplitViewMetricEnum.Instance.GetColumnHeaderText(SplitViewMetricType.DetailSplitDistance);
                this.DetailGrid.Columns[(int)DetailColumn.SplitDistance].Tag = SplitViewMetricType.DetailSplitDistance;

                this.DetailGrid.Columns[(int)DetailColumn.TotalTime].Width = 74;
                this.DetailGrid.Columns[(int)DetailColumn.TotalTime].HeaderText = SplitViewMetricEnum.Instance.GetColumnHeaderText(SplitViewMetricType.DetailTotalTime);
                this.DetailGrid.Columns[(int)DetailColumn.TotalTime].DefaultCellStyle.Format = "hh\\:mm\\:ss";
                this.DetailGrid.Columns[(int)DetailColumn.TotalTime].Tag = SplitViewMetricType.DetailTotalTime;

                this.DetailGrid.Columns[(int)DetailColumn.Delta].Width = 61;
                this.DetailGrid.Columns[(int)DetailColumn.Delta].HeaderText = SplitViewMetricEnum.Instance.GetColumnHeaderText(SplitViewMetricType.DetailDeltaTime);
                this.DetailGrid.Columns[(int)DetailColumn.Delta].Tag = SplitViewMetricType.DetailDeltaTime;

                this.DetailGrid.Columns[(int)DetailColumn.Blank].Width = 5; // Five seems to be minimum size, even if set to zero
                this.DetailGrid.Columns[(int)DetailColumn.Blank].HeaderText = "";
                this.DetailGrid.Columns[(int)DetailColumn.Blank].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


                int sumWidth = 0;
                foreach (DataGridViewColumn c in this.DetailGrid.Columns)
                {
                    sumWidth += c.Width;
                    //Logger.LogDebug($"{this.GetType()}.InitializeDetailDataGrid - Column: {c.Name}, Width: {c.Width} ({sumWidth})");
                    c.MinimumWidth = c.Width;
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                // Set the row default cell font to the grid's default cell font.  Then clear the grid default so the row default is all that has to change. 
                this.DetailGrid.RowsDefaultCellStyle.Font = this.DetailGrid.DefaultCellStyle.Font;
                this.DetailGrid.DefaultCellStyle.Font = null;

                // These must be set here not in designer otherwise column widths change. not sure why
                DetailGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                DetailGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                this.DetailGrid.ShowFocus = false;
            }

            private void InitializeSummaryDataGrid()
            {
                this.SummaryGrid.DataSource = this.SummaryBindingSource;

                this.SummaryGrid.Columns[(int)SummaryColumn.Reserved].Width = 87;
                this.SummaryGrid.Columns[(int)SummaryColumn.Reserved].HeaderText = "";

                this.SummaryGrid.Columns[(int)SummaryColumn.GoalSpeed].Width = 48;
                this.SummaryGrid.Columns[(int)SummaryColumn.GoalSpeed].HeaderText = SplitViewMetricEnum.Instance.GetColumnHeaderText(SplitViewMetricType.SummaryGoalSpeed);
                this.SummaryGrid.Columns[(int)SummaryColumn.GoalSpeed].Tag = SplitViewMetricType.SummaryGoalSpeed;

                this.SummaryGrid.Columns[(int)SummaryColumn.GoalDistance].Width = 50;
                this.SummaryGrid.Columns[(int)SummaryColumn.GoalDistance].HeaderText = SplitViewMetricEnum.Instance.GetColumnHeaderText(SplitViewMetricType.SummaryGoalDistance);
                this.SummaryGrid.Columns[(int)SummaryColumn.GoalDistance].Tag = SplitViewMetricType.SummaryGoalDistance;

                this.SummaryGrid.Columns[(int)SummaryColumn.GoalTime].Width = 74;
                this.SummaryGrid.Columns[(int)SummaryColumn.GoalTime].HeaderText = SplitViewMetricEnum.Instance.GetColumnHeaderText(SplitViewMetricType.SummaryGoalTime);
                this.SummaryGrid.Columns[(int)SummaryColumn.GoalTime].DefaultCellStyle.Format = "hh\\:mm\\:ss";
                this.SummaryGrid.Columns[(int)SummaryColumn.GoalTime].Tag = SplitViewMetricType.SummaryGoalTime;

                // Use the last blank column to fill the gap if user resizes
                this.SummaryGrid.Columns[(int)SummaryColumn.Blank].Width = 61; // Five seems to be minimum size, even if set to zero
                this.SummaryGrid.Columns[(int)SummaryColumn.Blank].HeaderText = "";
                this.SummaryGrid.Columns[(int)SummaryColumn.Blank].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                int sumWidth = 0;
                foreach (DataGridViewColumn c in this.SummaryGrid.Columns)
                {
                    sumWidth += c.Width;
                    //Logger.LogDebug($"{this.GetType()}.InitializeSummaryDataGrid - Column: {c.Name}, Width: {c.Width} ({sumWidth})");
                    c.MinimumWidth = c.Width;
                    c.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                // Set the row default cell font to the grid's default cell font.  Then clear the grid default so the row default is all that has to change. 
                this.SummaryGrid.RowsDefaultCellStyle.Font = this.SummaryGrid.DefaultCellStyle.Font;
                this.SummaryGrid.DefaultCellStyle.Font = null;

                // These must be set here not in designer otherwise column widths change. not sure why
                SummaryGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                SummaryGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

                this.SummaryGrid.ShowFocus = false;

                SummaryRow r = new(this);
                this.SummaryRows.Add(r);
            }

            #endregion

            #region Speed and Distance column handling
            public SpeedDisplayType SplitSpeed_SelectedDisplayType
            {
                get { return ZAMsettings.Settings.CurrentUser.SplitViewColumnSettings.SpeedValues[SplitViewMetricType.DetailSplitSpeed].Key; }
                set
                {
                    ZAMsettings.Settings.CurrentUser.SplitViewColumnSettings.SpeedValues[SplitViewMetricType.DetailSplitSpeed] = SpeedDisplayEnum.Instance.GetItem(value);

                    this.SplitSpeed_PreferredDisplayType = value == SpeedDisplayType.Both ? mAutoToggleSpeedDisplayType : value;

                    OnSpeedDisplayTypeChangedEvent(new SpeedDisplayTypeChangedEventArgs(DetailColumn.SplitSpeed.ToString(), this.SplitSpeed_PreferredDisplayType, this.DetailGrid));

                    foreach (var item in this.DetailRows)
                    {
                        item.UpdateSplitSpeed(value);
                    }
                }
            }

            public SpeedDisplayType GoalSpeed_SelectedDisplayType
            {
                get { return ZAMsettings.Settings.CurrentUser.SplitViewColumnSettings.SpeedValues[SplitViewMetricType.SummaryGoalSpeed].Key; }
                set
                {
                    ZAMsettings.Settings.CurrentUser.SplitViewColumnSettings.SpeedValues[SplitViewMetricType.SummaryGoalSpeed] = SpeedDisplayEnum.Instance.GetItem(value);

                    this.GoalSpeed_PreferredDisplayType = value == SpeedDisplayType.Both ? mAutoToggleSpeedDisplayType : value;

                    OnSpeedDisplayTypeChangedEvent(new SpeedDisplayTypeChangedEventArgs(SummaryColumn.GoalSpeed.ToString(), this.GoalSpeed_PreferredDisplayType, this.SummaryGrid));

                    foreach (var item in this.SummaryRows)
                    {
                        item.UpdateGoalSpeed(value);
                    }
                }
            }

            public DistanceDisplayType SplitDistance_SelectedDisplayType
            {
                get { return ZAMsettings.Settings.CurrentUser.SplitViewColumnSettings.DistanceValues[SplitViewMetricType.DetailSplitDistance].Key; }
                set
                {
                    ZAMsettings.Settings.CurrentUser.SplitViewColumnSettings.DistanceValues[SplitViewMetricType.DetailSplitDistance] = DistanceDisplayEnum.Instance.GetItem(value);

                    this.SplitDistance_PreferredDisplayType = value == DistanceDisplayType.Both ? mAutoToggleDistanceDisplayType : value;

                    OnDistanceDisplayTypeChangedEvent(new DistanceDisplayTypeChangedEventArgs(DetailColumn.SplitDistance.ToString(), this.SplitDistance_PreferredDisplayType, this.DetailGrid));

                    foreach (var item in this.DetailRows)
                    {
                        item.UpdateSplitDistance(value);
                    }
                }
            }
            public DistanceDisplayType GoalDistance_SelectedDisplayType
            {
                get { return ZAMsettings.Settings.CurrentUser.SplitViewColumnSettings.DistanceValues[SplitViewMetricType.SummaryGoalDistance].Key; }
                set
                {
                    ZAMsettings.Settings.CurrentUser.SplitViewColumnSettings.DistanceValues[SplitViewMetricType.SummaryGoalDistance] = DistanceDisplayEnum.Instance.GetItem(value);

                    this.GoalDistance_PreferredDisplayType = value == DistanceDisplayType.Both ? mAutoToggleDistanceDisplayType : value;

                    OnDistanceDisplayTypeChangedEvent(new DistanceDisplayTypeChangedEventArgs(SummaryColumn.GoalDistance.ToString(), this.GoalDistance_PreferredDisplayType, this.SummaryGrid));

                    foreach (var item in this.SummaryRows)
                    {
                        item.UpdateGoalDistance(value);
                    }
                }
            }

            /// <summary>
            /// Called when it's time to auto-switch between Metric and Imperial UOMs
            /// </summary>
            /// <param name="type"></param>
            public void SetAutoToggleMeasurementSystem(MeasurementSystemType type, bool forceUpdate = false)
            {
                SpeedDisplayType speedDisplayType;
                DistanceDisplayType distanceDisplayType;

                if (type == MeasurementSystemType.Imperial)
                {
                    speedDisplayType = SpeedDisplayType.MilesPerHour;
                    distanceDisplayType = DistanceDisplayType.Miles;
                }
                else
                {
                    speedDisplayType = SpeedDisplayType.KilometersPerHour;
                    distanceDisplayType = DistanceDisplayType.Kilometers;
                }

                this.mAutoToggleDistanceDisplayType = distanceDisplayType;
                this.mAutoToggleSpeedDisplayType = speedDisplayType;

                this.SplitSpeed_PreferredDisplayType = this.SplitSpeed_SelectedDisplayType == SpeedDisplayType.Both ? mAutoToggleSpeedDisplayType : this.SplitSpeed_SelectedDisplayType;
                this.SplitDistance_PreferredDisplayType = this.SplitDistance_SelectedDisplayType == DistanceDisplayType.Both ? mAutoToggleDistanceDisplayType : this.SplitDistance_SelectedDisplayType;

                if (this.DetailRows.Count > 0 || forceUpdate)
                {
                    OnSpeedDisplayTypeChangedEvent(new SpeedDisplayTypeChangedEventArgs(DetailColumn.SplitSpeed.ToString(), this.SplitSpeed_PreferredDisplayType, this.DetailGrid));
                    OnDistanceDisplayTypeChangedEvent(new DistanceDisplayTypeChangedEventArgs(DetailColumn.SplitDistance.ToString(), this.SplitDistance_PreferredDisplayType, this.DetailGrid));
                }

                this.GoalSpeed_PreferredDisplayType = this.GoalSpeed_SelectedDisplayType == SpeedDisplayType.Both ? mAutoToggleSpeedDisplayType : this.GoalSpeed_SelectedDisplayType;
                this.GoalDistance_PreferredDisplayType = this.GoalDistance_SelectedDisplayType == DistanceDisplayType.Both ? mAutoToggleDistanceDisplayType : this.GoalDistance_SelectedDisplayType;

                if (this.SummaryRows[0].GoalDistance != "" || forceUpdate)
                {
                    OnSpeedDisplayTypeChangedEvent(new SpeedDisplayTypeChangedEventArgs(SummaryColumn.GoalSpeed.ToString(), this.GoalSpeed_PreferredDisplayType, this.SummaryGrid));
                    OnDistanceDisplayTypeChangedEvent(new DistanceDisplayTypeChangedEventArgs(SummaryColumn.GoalDistance.ToString(), this.GoalDistance_PreferredDisplayType, this.SummaryGrid));
                }

                foreach (var item in this.DetailRows)
                {
                    item.UpdateSplitDistance(distanceDisplayType);
                    item.UpdateSplitSpeed(speedDisplayType);
                }

                foreach (var item in this.SummaryRows)
                {
                    item.UpdateGoalDistance(distanceDisplayType);
                    item.UpdateGoalSpeed(speedDisplayType);
                }
            }
            #endregion

            #region Event Handling
            private void OnSpeedDisplayTypeChangedEvent(SpeedDisplayTypeChangedEventArgs e)
            {
                EventHandler<SpeedDisplayTypeChangedEventArgs> handler = SpeedDisplayTypeChangedEvent;

                if (handler != null)
                {
                    try
                    {
                        handler(null, e);
                    }
                    catch (Exception ex)
                    {
                        // Don't let downstream exceptions bubble up
                        Logger.LogError(ex, $"Caught in {this.GetType()} (OnSpeedDisplayTypeChangedEvent)");
                    }
                }
            }

            private void OnDistanceDisplayTypeChangedEvent(DistanceDisplayTypeChangedEventArgs e)
            {
                EventHandler<DistanceDisplayTypeChangedEventArgs> handler = DistanceDisplayTypeChangedEvent;

                if (handler != null)
                {
                    try
                    {
                        handler(null, e);
                    }
                    catch (Exception ex)
                    {
                        // Don't let downstream exceptions bubble up
                        Logger.LogError(ex, $"Caught in {this.GetType()} (OnDistanceDisplayTypeChangedEvent)");
                    }
                }
            }
            #endregion
        }

        #endregion

        #region DetailRow class
        /// <summary>
        /// The class determines the columns available in the Detail DataGridView
        /// </summary>
        public class DetailRow : NotifyPropertyChangedBase, IComparable<int>
        {
            //Add the [Browsable(false)] attribute to any public properties you don't want columns created for in the DataGridView

            public int SplitNumber { get; }
            public TimeSpan SplitTime { get { return this.mSplitTime; } set { this.SetProperty<TimeSpan>(ref this.mSplitTime, value); } }
            public string SplitSpeed { get { return this.mSplitSpeed; } set { this.SetProperty<string>(ref this.mSplitSpeed, value); } }
            public string SplitDistance { get { return this.mSplitDistance; } set { this.SetProperty<string>(ref this.mSplitDistance, value); } }
            public TimeSpan TotalTime { get { return this.mTotalTime; } set { this.SetProperty<TimeSpan>(ref this.mTotalTime, value); } }
            public string Delta { get { return this.mDelta; } set { this.SetProperty<string>(ref this.mDelta, value); } }
            public string Blank { get; set; }

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
            private readonly DataGridViewManager mViewManager;

            public DetailRow(int splitNumber, DataGridViewManager viewManager)
            {
                this.SplitNumber = splitNumber;
                this.mViewManager = viewManager;
            }

            /// <summary>
            /// Updates the displayed column appropriately
            /// </summary>
            /// <param name="updatedType"></param>
            public void UpdateSplitSpeed(SpeedDisplayType updatedType)
            {
                SpeedDisplayType preferredType =  this.mViewManager.SplitSpeed_PreferredDisplayType;

                if (preferredType == updatedType)
                {
                    switch (updatedType)
                    {
                        case SpeedDisplayType.KilometersPerHour:
                            this.SplitSpeed = this.SplitSpeedKph >= 0 ? this.SplitSpeedKph.ToString("0.0") : "";
                            break;

                        case SpeedDisplayType.MilesPerHour:
                            this.SplitSpeed = this.SplitSpeedMph >= 0 ? this.SplitSpeedMph.ToString("0.0") : "";
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
            public void UpdateSplitDistance(DistanceDisplayType updatedType)
            {
                DistanceDisplayType preferredType = this.mViewManager.SplitDistance_PreferredDisplayType;

                if (preferredType == updatedType)
                {
                    switch (updatedType)
                    {
                        case DistanceDisplayType.Kilometers:
                            this.SplitDistance = this.SplitDistanceKm >= 0 ? this.SplitDistanceKm.ToString("0.0") : "";
                            break;

                        case DistanceDisplayType.Miles:
                            this.SplitDistance = this.SplitDistanceMi >= 0 ? this.SplitDistanceMi.ToString("0.0") : "";
                            break;
                    }
                }
                else if (preferredType == DistanceDisplayType.None)
                {
                    this.SplitDistance = "";
                }
            }

            public int CompareTo(int other)
            {
                return SplitNumber.CompareTo(other);
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
        public class SummaryRow : NotifyPropertyChangedBase
        {
            //Add the [Browsable(false)] attribute to any public properties you don't want columns created for in the DataGridView

            public string Reserved { get; set; }
            public string GoalSpeed { get { return this.mGoalSpeed; } set { this.SetProperty<string>(ref this.mGoalSpeed, value); } }
            public string GoalDistance { get { return this.mGoalDistance; } set { this.SetProperty<string>(ref this.mGoalDistance, value); } }
            public TimeSpan? GoalTime { get { return this.mGoalTime; } set { this.SetProperty<TimeSpan?>(ref this.mGoalTime, value); } }
            public string Blank { get; set; }

            private string mGoalDistance;
            private string mGoalSpeed;
            private TimeSpan? mGoalTime;

            private double? mGoalSpeedKph;
            private double? mGoalSpeedMph;
            private double? mGoalDistanceMi;
            private double? mGoalDistanceKm;
            private readonly DataGridViewManager mViewManager;

            public SummaryRow(DataGridViewManager viewManager)
            {
                this.mViewManager = viewManager;
            }

            /// <summary>
            /// Updates the displayed column appropriately
            /// </summary>
            /// <param name="updatedType"></param>
            public void UpdateGoalSpeed(SpeedDisplayType updatedType)
            {
                if (!this.GoalSpeedKph.HasValue || !this.GoalSpeedMph.HasValue)
                {
                    this.GoalSpeed = "";
                    return;
                }
                SpeedDisplayType preferredType = this.mViewManager.GoalSpeed_PreferredDisplayType;

                if (preferredType == updatedType)
                {
                    switch (updatedType)
                    {
                        case SpeedDisplayType.KilometersPerHour:
                            this.GoalSpeed = this.GoalSpeedKph >= 0 ? this.GoalSpeedKph.Value.ToString("0.0") : "";
                            break;

                        case SpeedDisplayType.MilesPerHour:
                            this.GoalSpeed = this.GoalSpeedMph >= 0 ? this.GoalSpeedMph.Value.ToString("0.0") : "";
                            break;
                    }
                }
                else if (preferredType == SpeedDisplayType.None)
                {
                    this.GoalSpeed = "";
                }
            }

            /// <summary>
            /// Updates the displayed column appropriately
            /// </summary>
            /// <param name="updatedType"></param>
            public void UpdateGoalDistance(DistanceDisplayType updatedType)
            {
                if (!this.GoalDistanceKm.HasValue || !this.GoalDistanceMi.HasValue)
                {
                    this.GoalDistance = "";
                    return;
                }

                DistanceDisplayType preferredType = this.mViewManager.GoalDistance_PreferredDisplayType;

                if (preferredType == updatedType)
                {
                    switch (updatedType)
                    {
                        case DistanceDisplayType.Kilometers:
                            this.GoalDistance = this.GoalDistanceKm >= 0 ? this.GoalDistanceKm.Value.ToString("0.0") : "";
                            break;

                        case DistanceDisplayType.Miles:
                            this.GoalDistance = this.GoalDistanceMi >= 0 ? this.GoalDistanceMi.Value.ToString("0.0") : "";
                            break;
                    }
                }
                else if (preferredType == DistanceDisplayType.None)
                {
                    this.GoalDistance = "";
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double? GoalSpeedKph
            {
                get { return this.mGoalSpeedKph; }
                set
                {
                    this.mGoalSpeedKph = value;
                    this.UpdateGoalSpeed(SpeedDisplayType.KilometersPerHour);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double? GoalSpeedMph
            {
                get { return this.mGoalSpeedMph; }
                set
                {
                    this.mGoalSpeedMph = value;
                    this.UpdateGoalSpeed(SpeedDisplayType.MilesPerHour);
                }
            }
            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double? GoalDistanceKm
            {
                get { return this.mGoalDistanceKm; }
                set
                {
                    this.mGoalDistanceKm = value;
                    this.UpdateGoalDistance(DistanceDisplayType.Kilometers);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double? GoalDistanceMi
            {
                get { return this.mGoalDistanceMi; }
                set
                {
                    this.mGoalDistanceMi = value;
                    this.UpdateGoalDistance(DistanceDisplayType.Miles);
                }
            }
        }

        #endregion

        private SplitsManagerV2 SplitsManager = new();
        private Dispatcher mDispatcher;
        private DataGridViewManager ViewManager;
        private bool mInitialControlGainedFocus;
        private ILogger<SplitViewerControl> Logger;

        private static Color RED = Color.FromArgb(255, 192, 0, 0); // red 
        private static Color GREEN = Color.FromArgb(255, 0, 192, 0); // green
        private static Color TRANSPARENCY = Color.FromArgb(255, 17, 146, 204); // ZAM transparency key

        public SplitViewerControl()
        {
            InitializeComponent();

            if (this.DesignMode)
                return;

            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<SplitViewerControl>();
        }


        #region SplitViewerControl events
        private void ViewControl_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;

            // for handling UI events
            mDispatcher = Dispatcher.CurrentDispatcher;

            this.ViewManager = new(dgDetail, dgSummary);
            ViewManager.DistanceDisplayTypeChangedEvent += DataGridViewManager_DistanceDisplayTypeChangedEvent;
            ViewManager.SpeedDisplayTypeChangedEvent += DataGridViewManager_SpeedDisplayTypeChangedEvent;
            ViewManager.Initialize();

            // Show goal information
            this.UpdateSummaryRow();

            // Subscribe to any SystemConfig or SplitsConfig changes
            ZAMsettings.SystemConfigChanged += ZAMsettings_SystemConfigChanged;
            ZAMsettings.SplitsConfigChanged += ZAMsettings_SplitsConfigChanged;
            ZAMsettings.ZPMonitorService.CollectionStatusChanged += ZPMonitorService_CollectionStatusChanged;

            this.SetupDisplayForCurrentUserProfile();

            SplitsManager.SplitGoalCompletedEvent += SplitsManager_SplitGoalCompletedEvent;    // Goal splits only
            SplitsManager.SplitUpdatedEvent += SplitsManager_SplitUpdatedOrCompleted;          // Goal and Non-Goal splits
            SplitsManager.SplitCompletedEvent += SplitsManager_SplitUpdatedOrCompleted;        // Non-Goal splits only

            if (this.ParentForm is MainForm parentForm)
                parentForm.FormSyncFiveSecondTimerTickEvent += MainForm_FormSyncFiveSecondTimerTickEvent;

            // Trigger a resize so that dgSummary can size itself appropriately
            this.OnResize(new EventArgs());
        }

        public override void ControlGainingFocus(object sender, EventArgs e)
        {
            //Logger.LogDebug($"{this.GetType()}.ControlGainingFocus");

            if (!mInitialControlGainedFocus)
            {
                //Logger.LogDebug($"{this.GetType()}.ControlGainingFocus - Performing initializations");

                int sumWidth = 0;
                foreach (DataGridViewColumn c in this.dgDetail.Columns)
                {
                    sumWidth += c.Width;
                    //Logger.LogDebug($"{this.GetType()}.ControlGainingFocus - Column: {c.Name}, Width: {c.Width} ({sumWidth})");
                }

                mInitialControlGainedFocus = true;
            }
        }


        private void ViewControl_Resize(object sender, EventArgs e)
        {
            //Logger.LogDebug($"ViewControl_Resize - Size: {this.Size}");

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
            //Logger.LogDebug($"{this.GetType()}.ViewControl_SizeChanged - {this.Size}");
        }
        #endregion

        private void dgDetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //if (e.ListChangedType == ListChangedType.Reset)
            //    Logger.LogDebug($"{this.GetType()}.dgDetail_DataBindingComplete - ListChangedType: {e.ListChangedType}");
        }

        /// <summary>
        /// A timer event generated by MainForm to allow UserControls to syncronize data updates
        /// Occurs every five seconds.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormSyncFiveSecondTimerTickEvent(object sender, FormSyncTimerTickEventArgs e)
        {
            // determine type of units to display, alternate every 5 seconds
            MeasurementSystemType type = e.TickCount % 2 == 0 ? MeasurementSystemType.Imperial : MeasurementSystemType.Metric;

            ViewManager.SetAutoToggleMeasurementSystem(type);
        }

        #region SplitsManager event handling
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
            // Dispatcher.BeginInvoke IS required as the DataGridView is sorted when a new row is added.

            if (!mDispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                mDispatcher.BeginInvoke(new SplitEventHandlerDelegate(SplitsManager_SplitUpdatedOrCompleted), new object[] { sender, e });
                return;
            }

            DetailRow detailRow = ViewManager.DetailRows.FirstOrDefault(r => r.SplitNumber == e.SplitNumber);

            if (detailRow == null)
            {
                // New split row
                detailRow = new(e.SplitNumber, ViewManager)
                {
                    SplitTime = e.SplitTime,
                    SplitDistanceKm = e.TotalKmTravelled,
                    SplitDistanceMi = e.TotalMiTravelled,
                    SplitSpeedMph = e.SplitSpeedMph,
                    SplitSpeedKph = e.SplitSpeedKph,
                    TotalTime = e.TotalTime,
                    DeltaTime = e.DeltaTime,
                };
                ViewManager.DetailRows.Add(detailRow);

                dgDetail.Sort(dgDetail.Columns[0], ListSortDirection.Descending);

                // After the sort, the cell attributes must be put back on the rows
                for (int r=0; r<dgDetail.Rows.Count; r++)
                {
                    DataGridViewCell cell = dgDetail.Rows[r].Cells[(int)DetailColumn.Delta];

                    if (dgDetail.Rows[r].DataBoundItem is DetailRow item)
                    {
                        if (item.DeltaTime.HasValue)
                        {
                            cell.Style.BackColor = item.DeltaTime.Value.TotalSeconds <= 0 ? GREEN : RED;
                            cell.Style.ForeColor = Color.White;
                        }
                    }
                }
            }
            else
            {
                // Update split in progress
                detailRow.SplitTime = e.SplitTime;
                detailRow.SplitDistanceKm = e.TotalKmTravelled;
                detailRow.SplitDistanceMi = e.TotalMiTravelled;
                detailRow.SplitSpeedMph = e.SplitSpeedMph;
                detailRow.SplitSpeedKph = e.SplitSpeedKph;
                detailRow.TotalTime = e.TotalTime;
                detailRow.DeltaTime = e.DeltaTime;

                if (e.AheadOfGoalTime.HasValue)
                {
                    dgDetail.Rows[0].Cells[(int)DetailColumn.Delta].Style.BackColor = e.AheadOfGoalTime.Value ? GREEN : RED;
                    dgDetail.Rows[0].Cells[(int)DetailColumn.Delta].Style.ForeColor = Color.White;
                }
            }
        }

        /// <summary>
        /// Occurs each time a goal split completes.  Allows for UI update by marshalling the call accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SplitsManager_SplitGoalCompletedEvent(object sender, SplitEventArgs e)
        {
            // Dispatcher.BeginInvoke not required as the DataGridView uses the SyncBindingSource class

            //if (!mDispatcher.CheckAccess()) // are we currently on the UI thread?
            //{
            //    // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
            //    mDispatcher.BeginInvoke(new SplitEventHandlerDelegate(SplitsManager_SplitGoalCompletedEvent), new object[] { sender, e });
            //    return;
            //}

            DetailRow detailRow = ViewManager.DetailRows.FirstOrDefault(r => r.SplitNumber == e.SplitNumber);

            if (detailRow != null)
            {
                // Update split in progress
                detailRow.SplitTime = e.SplitTime;
                detailRow.SplitDistanceKm = e.TotalKmTravelled;
                detailRow.SplitDistanceMi = e.TotalMiTravelled;
                detailRow.SplitSpeedMph = e.SplitSpeedMph;
                detailRow.SplitSpeedKph = e.SplitSpeedKph;
                detailRow.TotalTime = e.TotalTime;
                detailRow.DeltaTime = e.DeltaTime;

                if (e.AheadOfGoalTime.HasValue)
                {
                    dgDetail.Rows[0].Cells[(int)DetailColumn.Delta].Style.BackColor = e.AheadOfGoalTime.Value ? GREEN : RED;
                    dgDetail.Rows[0].Cells[(int)DetailColumn.Delta].Style.ForeColor = Color.White;
                }
            }
        }

        /// <summary>
        /// Allow owner class to tie into the SplitGoalCompletedEvent and SplitCompletedEvent.  This allows the MainForm to bring this control into focus.
        /// </summary>
        public event EventHandler<SplitEventArgs> SplitGoalCompletedEvent
        {
            add
            {
                SplitsManager.SplitGoalCompletedEvent += value;
            }
            remove
            {
                SplitsManager.SplitGoalCompletedEvent -= value;
            }
        }

        public event EventHandler<SplitEventArgs> SplitCompletedEvent
        {
            add
            {
                SplitsManager.SplitCompletedEvent += value;
            }
            remove
            {
                SplitsManager.SplitCompletedEvent -= value;
            }
        }

        #endregion

        private void ZAMsettings_SplitsConfigChanged(object sender, EventArgs e)
        {
            this.UpdateSummaryRow();
        }

        private void ZPMonitorService_CollectionStatusChanged(object sender, CollectionStatusChangedEventArgs e)
        {
            //Logger.LogDebug($"{this.GetType()}.ZPMonitorService_CollectionStatusChanged - {e.Action}");

            switch (e.Action)
            {
                case CollectionStatusChangedEventArgs.ActionType.Waiting:
                    this.ClearDisplayValues();
                    break;

                case CollectionStatusChangedEventArgs.ActionType.Started:
                    this.ClearDisplayValues();
                    break;
            }
        }


        private void ZAMsettings_SystemConfigChanged(object sender, EventArgs e)
        {
            //Logger.LogDebug($"ZAMsettings_SystemConfigChanged - {this.GetType()}");

            this.SetupDisplayForCurrentUserProfile();
        }

        /// <summary>
        /// Update the summary row with the latest values in configuration
        /// </summary>
        private void UpdateSummaryRow()
        {
            SummaryRow row = this.ViewManager.SummaryRows[0];

            row.Reserved = SplitsManager.HasSplitGoals ? "Goal" : "No Goal";
            row.GoalDistanceKm = SplitsManager.GoalDistanceKm;
            row.GoalDistanceMi = SplitsManager.GoalDistanceMi;
            row.GoalSpeedKph = SplitsManager.GoalSpeedKph;
            row.GoalSpeedMph = SplitsManager.GoalSpeedMph;
            row.GoalTime = SplitsManager.GoalTime;
        }

        private void SetupDisplayForCurrentUserProfile()
        {
            this.dgDetail.Columns[(int)DetailColumn.SplitDistance].Visible = CurrentUserProfile.SplitViewColumnSettings.Visibility[SplitViewMetricType.DetailSplitDistance];
            this.dgDetail.Columns[(int)DetailColumn.SplitNumber].Visible = CurrentUserProfile.SplitViewColumnSettings.Visibility[SplitViewMetricType.DetailSplitNumber];
            this.dgDetail.Columns[(int)DetailColumn.SplitSpeed].Visible = CurrentUserProfile.SplitViewColumnSettings.Visibility[SplitViewMetricType.DetailSplitSpeed];
            this.dgDetail.Columns[(int)DetailColumn.SplitTime].Visible = CurrentUserProfile.SplitViewColumnSettings.Visibility[SplitViewMetricType.DetailSplitTime];
            this.dgDetail.Columns[(int)DetailColumn.TotalTime].Visible = CurrentUserProfile.SplitViewColumnSettings.Visibility[SplitViewMetricType.DetailTotalTime];

            this.dgSummary.Columns[(int)SummaryColumn.GoalDistance].Visible = CurrentUserProfile.SplitViewColumnSettings.Visibility[SplitViewMetricType.SummaryGoalDistance];
            this.dgSummary.Columns[(int)SummaryColumn.GoalSpeed].Visible = CurrentUserProfile.SplitViewColumnSettings.Visibility[SplitViewMetricType.SummaryGoalSpeed];
            this.dgSummary.Columns[(int)SummaryColumn.GoalTime].Visible = CurrentUserProfile.SplitViewColumnSettings.Visibility[SplitViewMetricType.SummaryGoalTime];
        }

        private void ClearDisplayValues()
        {
            ViewManager.DetailRows.Clear();

            this.UpdateSummaryRow();
        }

        #region ViewManager event handling
        private void DataGridViewManager_SpeedDisplayTypeChangedEvent(object sender, SpeedDisplayTypeChangedEventArgs e)
        {
            //Logger.LogDebug($"{this.GetType()}.DataGridViewManager_SpeedDisplayTypeChangedEvent - {e.ColumnName}, {e.DisplayType}");

            if (e.Tag is DataGridView dataGridView)
            {
                dataGridView.Columns[e.ColumnName].HeaderText = SpeedDisplayEnum.Instance.GetColumnHeaderText(e.DisplayType);
            }
        }

        private void DataGridViewManager_DistanceDisplayTypeChangedEvent(object sender, DistanceDisplayTypeChangedEventArgs e)
        {
            //Logger.LogDebug($"{this.GetType()}.DataGridViewManager_DistanceDisplayTypeChangedEvent - {e.ColumnName}, {e.DisplayType}");

            if (e.Tag is DataGridView dataGridView)
            {
                dataGridView.Columns[e.ColumnName].HeaderText = DistanceDisplayEnum.Instance.GetColumnHeaderText(e.DisplayType);
            }
        }
        #endregion

        #region DataGridView events, Includes mouse click handlers for visibility and metric configuration

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
                switch (e.ColumnIndex)
                {
                    case (int)DetailColumn.SplitDistance:
                        // the SplitViewMetricType is stored in the column header tag
                        metricType = (int)dataGridView.Columns[e.ColumnIndex].Tag;

                        foreach (var kvp in DistanceDisplayEnum.Instance.GetItems())
                        {
                            var mi = new ToolStripMenuItem(DistanceDisplayEnum.Instance.GetMenuItemText(kvp.Key))
                            {
                                CheckOnClick = true,
                                Tag = new object[] { kvp.Key, metricType, dataGridView }, // pass required values to the handler event
                                Checked = ViewManager.SplitDistance_SelectedDisplayType == kvp.Key,
                            };
                            mi.CheckedChanged += UOM_ContextMenu_CheckChanged;
                            menuStrip.Items.Add(mi);
                        }

                        menuStrip.Show(Cursor.Position);
                        break;

                    case (int)DetailColumn.SplitSpeed:
                        // the SplitViewMetricType is stored in the column header tag
                        metricType = (int)dataGridView.Columns[e.ColumnIndex].Tag;

                        foreach (var kvp in SpeedDisplayEnum.Instance.GetItems())
                        {
                            var mi = new ToolStripMenuItem(SpeedDisplayEnum.Instance.GetMenuItemText(kvp.Key))
                            {
                                CheckOnClick = true,
                                Tag = new object[] { kvp.Key, metricType, dataGridView }, // pass required values to the handler event
                                Checked = ViewManager.SplitSpeed_SelectedDisplayType == kvp.Key,
                            };
                            mi.CheckedChanged += UOM_ContextMenu_CheckChanged;
                            menuStrip.Items.Add(mi);
                        }

                        menuStrip.Show(Cursor.Position);
                        break;
                }
            }
            else if (dataGridView == this.dgSummary)
            {
                switch (e.ColumnIndex)
                {
                    case (int)SummaryColumn.GoalDistance:
                        // the SplitViewMetricType is stored in the column header tag
                        metricType = (int)dataGridView.Columns[e.ColumnIndex].Tag;

                        foreach (var kvp in DistanceDisplayEnum.Instance.GetItems())
                        {
                            var mi = new ToolStripMenuItem(DistanceDisplayEnum.Instance.GetMenuItemText(kvp.Key))
                            {
                                CheckOnClick = true,
                                Tag = new object[] { kvp.Key, metricType, dataGridView }, // pass required values to the handler event
                                Checked = ViewManager.SplitDistance_SelectedDisplayType == kvp.Key,
                            };
                            mi.CheckedChanged += UOM_ContextMenu_CheckChanged;
                            menuStrip.Items.Add(mi);
                        }

                        menuStrip.Show(Cursor.Position);
                        break;

                    case (int)SummaryColumn.GoalSpeed:
                        // the SplitViewMetricType is stored in the column header tag
                        metricType = (int)dataGridView.Columns[e.ColumnIndex].Tag;

                        foreach (var kvp in SpeedDisplayEnum.Instance.GetItems())
                        {
                            var mi = new ToolStripMenuItem(SpeedDisplayEnum.Instance.GetMenuItemText(kvp.Key))
                            {
                                CheckOnClick = true,
                                Tag = new object[] { kvp.Key, metricType, dataGridView }, // pass required values to the handler event
                                Checked = ViewManager.SplitSpeed_SelectedDisplayType == kvp.Key,
                            };
                            mi.CheckedChanged += UOM_ContextMenu_CheckChanged;
                            menuStrip.Items.Add(mi);
                        }

                        menuStrip.Show(Cursor.Position);
                        break;
                }
            }        
        }

        /// <summary>
        /// Handle CheckedChanged event for UOM selection
        /// Handles both dgDetail and dgSummary
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UOM_ContextMenu_CheckChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            object[] tag = item.Tag as object[];

            var displayType = tag[0]; // value to set
            SplitViewMetricType metricType = (SplitViewMetricType)tag[1]; // column to set
            DataGridView dataGridView = (DataGridView)tag[2];

            if (item.Checked)
            {
                if (displayType is DistanceDisplayType distanceDisplayType)
                {
                    if (CurrentUserProfile.SplitViewColumnSettings.DistanceValues.ContainsKey(metricType))
                    {
                        ZAMsettings.BeginCachedConfiguration();
                        if (dataGridView == dgDetail)
                            ViewManager.SplitDistance_SelectedDisplayType = distanceDisplayType;
                        else
                            ViewManager.GoalDistance_SelectedDisplayType = distanceDisplayType;
                        ZAMsettings.CommitCachedConfiguration();
                    }
                }
                else if (displayType is SpeedDisplayType speedDisplayType)
                {
                    if (CurrentUserProfile.SplitViewColumnSettings.SpeedValues.ContainsKey(metricType))
                    {
                        ZAMsettings.BeginCachedConfiguration();
                        if (dataGridView == dgDetail)
                            ViewManager.SplitSpeed_SelectedDisplayType = speedDisplayType;
                        else
                            ViewManager.GoalSpeed_SelectedDisplayType = speedDisplayType;
                        ZAMsettings.CommitCachedConfiguration();
                    }
                }
            }
        }

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

        #endregion

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
            this.dgDetail.DefaultCellStyle.Font = this.RowFont;
            this.dgSummary.RowsDefaultCellStyle.Font = this.RowFont;
            this.dgSummary.DefaultCellStyle.Font = this.RowFont;

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
