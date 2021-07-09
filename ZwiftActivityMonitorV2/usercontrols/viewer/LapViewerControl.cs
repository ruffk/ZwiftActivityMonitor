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
    public partial class LapViewerControl : ViewerUserControlEx
    {
        /// <summary>
        /// Extension renderer class to avoid the pesky border under a ToolStrip control
        /// </summary>
        public class ToolStripProfessionalRendererEx : ToolStripProfessionalRenderer
        {
            protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
            {
                if (!(e.ToolStrip is ToolStrip))
                    base.OnRenderToolStripBorder(e);
            }
        }

        private enum DetailColumn
        {
            LapNumber = 0,
            LapTime,
            LapSpeed,
            LapDistance,
            LapAP,
            TotalTime,
            Blank
        }

        //private enum SummaryColumn
        //{
        //    Blank
        //}

        #region DataGridViewManager class

        public class DataGridViewManager
        {
            public SortableBindingList<DetailRow> DetailRows { get; } = new();         // Sortable binding list collection
            private SyncBindingSource DetailBindingSource { get; } = new();

            private readonly ILogger<DataGridViewManager> Logger;


            private SpeedDisplayType mAutoToggleSpeedDisplayType;
            private DistanceDisplayType mAutoToggleDistanceDisplayType;
            private PowerDisplayType mAutoTogglePowerDisplayType;
            private DataGridViewEx DetailGrid;
            //private DataGridViewEx SummaryGrid;

            public SpeedDisplayType LapSpeed_PreferredDisplayType { get; internal set; }
            public DistanceDisplayType LapDistance_PreferredDisplayType { get; internal set; }
            public PowerDisplayType LapAP_PreferredDisplayType { get; internal set; }

            public event EventHandler<SpeedDisplayTypeChangedEventArgs> SpeedDisplayTypeChangedEvent;
            public event EventHandler<DistanceDisplayTypeChangedEventArgs> DistanceDisplayTypeChangedEvent;
            public event EventHandler<PowerDisplayTypeChangedEventArgs> PowerDisplayTypeChangedEvent;

            public DataGridViewManager(DataGridViewEx detailGrid)
            {
                this.DetailGrid = detailGrid;
                //this.SummaryGrid = summaryGrid;

                if (ZAMsettings.LoggerFactory == null)
                    return;

                Logger = ZAMsettings.LoggerFactory.CreateLogger<DataGridViewManager>();


                // Note: anytime grid is sorted, the BindingSource will reset itself and things like cell colors and row visibility will be lost
                this.DetailBindingSource.DataSource = this.DetailRows;
                //this.SummaryBindingSource.DataSource = this.SummaryRows;
            }

            /// <summary>
            /// Call this after enrolling in the TypeChanged events to get header's properly named
            /// </summary>
            public void Initialize()
            {
                this.InitializeDetailDataGrid();

                //this.InitializeSummaryDataGrid();

                // Initialize auto-toggle and force an initial column header update that will observe user selections.
                this.SetAutoToggleMeasurementSystem(MeasurementSystemType.Imperial, true);
            }

            #region Detail Grid initialization

            private void InitializeDetailDataGrid()
            {
                this.DetailGrid.DataSource = this.DetailBindingSource;

                // Allow column headers to wrap to a second line
                this.DetailGrid.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;

                this.DetailGrid.Columns[(int)DetailColumn.LapNumber].Width = 36;
                this.DetailGrid.Columns[(int)DetailColumn.LapNumber].HeaderText = LapViewMetricEnum.Instance.GetColumnHeaderText(LapViewMetricType.DetailLapNumber);
                this.DetailGrid.Columns[(int)DetailColumn.LapNumber].Tag = LapViewMetricType.DetailLapNumber;

                this.DetailGrid.Columns[(int)DetailColumn.LapTime].Width = 51;
                this.DetailGrid.Columns[(int)DetailColumn.LapTime].HeaderText = LapViewMetricEnum.Instance.GetColumnHeaderText(LapViewMetricType.DetailLapTime);
                this.DetailGrid.Columns[(int)DetailColumn.LapTime].DefaultCellStyle.Format = "mm\\:ss";
                this.DetailGrid.Columns[(int)DetailColumn.LapTime].Tag = LapViewMetricType.DetailLapTime;

                this.DetailGrid.Columns[(int)DetailColumn.LapSpeed].Width = 48;
                this.DetailGrid.Columns[(int)DetailColumn.LapSpeed].HeaderText = LapViewMetricEnum.Instance.GetColumnHeaderText(LapViewMetricType.DetailLapSpeed);
                this.DetailGrid.Columns[(int)DetailColumn.LapSpeed].Tag = LapViewMetricType.DetailLapSpeed;

                this.DetailGrid.Columns[(int)DetailColumn.LapDistance].Width = 50;
                this.DetailGrid.Columns[(int)DetailColumn.LapDistance].HeaderText = LapViewMetricEnum.Instance.GetColumnHeaderText(LapViewMetricType.DetailLapDistance);
                this.DetailGrid.Columns[(int)DetailColumn.LapDistance].Tag = LapViewMetricType.DetailLapDistance;

                this.DetailGrid.Columns[(int)DetailColumn.LapAP].Width = 50;
                this.DetailGrid.Columns[(int)DetailColumn.LapAP].HeaderText = LapViewMetricEnum.Instance.GetColumnHeaderText(LapViewMetricType.DetailLapAP);
                this.DetailGrid.Columns[(int)DetailColumn.LapAP].Tag = LapViewMetricType.DetailLapAP;

                this.DetailGrid.Columns[(int)DetailColumn.TotalTime].Width = 74;
                this.DetailGrid.Columns[(int)DetailColumn.TotalTime].HeaderText = LapViewMetricEnum.Instance.GetColumnHeaderText(LapViewMetricType.DetailTotalTime);
                this.DetailGrid.Columns[(int)DetailColumn.TotalTime].DefaultCellStyle.Format = "hh\\:mm\\:ss";
                this.DetailGrid.Columns[(int)DetailColumn.TotalTime].Tag = LapViewMetricType.DetailTotalTime;

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

            //private void InitializeSummaryDataGrid()
            //{
            //    this.SummaryGrid.DataSource = this.SummaryBindingSource;

            //    // Use the last blank column to fill the gap if user resizes
            //    this.SummaryGrid.Columns[(int)SummaryColumn.Blank].Width = 5; // Five seems to be minimum size, even if set to zero
            //    this.SummaryGrid.Columns[(int)SummaryColumn.Blank].HeaderText = "";
            //    this.SummaryGrid.Columns[(int)SummaryColumn.Blank].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            //    foreach (DataGridViewColumn c in this.SummaryGrid.Columns)
            //    {
            //        c.MinimumWidth = c.Width;
            //        c.SortMode = DataGridViewColumnSortMode.NotSortable;
            //    }

            //    // Set the row default cell font to the grid's default cell font.  Then clear the grid default so the row default is all that has to change. 
            //    this.SummaryGrid.RowsDefaultCellStyle.Font = this.SummaryGrid.DefaultCellStyle.Font;
            //    this.SummaryGrid.DefaultCellStyle.Font = null;

            //    // These must be set here not in designer otherwise column widths change. not sure why
            //    SummaryGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            //    SummaryGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            //    this.SummaryGrid.ShowFocus = false;

            //    SummaryRow r = new(this);
            //    this.SummaryRows.Add(r);
            //}

            #endregion

            #region Speed and Distance column handling

            public SpeedDisplayType LapSpeed_SelectedDisplayType
            {
                get { return ZAMsettings.Settings.CurrentUser.LapViewColumnSettings.SpeedValues[LapViewMetricType.DetailLapSpeed].Key; }
                set
                {
                    ZAMsettings.Settings.CurrentUser.LapViewColumnSettings.SpeedValues[LapViewMetricType.DetailLapSpeed] = SpeedDisplayEnum.Instance.GetItem(value);

                    this.LapSpeed_PreferredDisplayType = value == SpeedDisplayType.Both ? mAutoToggleSpeedDisplayType : value;

                    OnSpeedDisplayTypeChangedEvent(new SpeedDisplayTypeChangedEventArgs(DetailColumn.LapSpeed.ToString(), this.LapSpeed_PreferredDisplayType, this.DetailGrid));

                    foreach (var item in this.DetailRows)
                    {
                        item.UpdateLapSpeed(value);
                    }
                }
            }

            public DistanceDisplayType LapDistance_SelectedDisplayType
            {
                get { return ZAMsettings.Settings.CurrentUser.LapViewColumnSettings.DistanceValues[LapViewMetricType.DetailLapDistance].Key; }
                set
                {
                    ZAMsettings.Settings.CurrentUser.LapViewColumnSettings.DistanceValues[LapViewMetricType.DetailLapDistance] = DistanceDisplayEnum.Instance.GetItem(value);

                    this.LapDistance_PreferredDisplayType = value == DistanceDisplayType.Both ? mAutoToggleDistanceDisplayType : value;

                    OnDistanceDisplayTypeChangedEvent(new DistanceDisplayTypeChangedEventArgs(DetailColumn.LapDistance.ToString(), this.LapDistance_PreferredDisplayType, this.DetailGrid));

                    foreach (var item in this.DetailRows)
                    {
                        item.UpdateLapDistance(value);
                    }
                }
            }
            public PowerDisplayType LapAP_SelectedDisplayType
            {
                get { return ZAMsettings.Settings.CurrentUser.LapViewColumnSettings.PowerValues[LapViewMetricType.DetailLapAP].Key; }
                set
                {
                    ZAMsettings.Settings.CurrentUser.LapViewColumnSettings.PowerValues[LapViewMetricType.DetailLapAP] = PowerDisplayEnum.Instance.GetItem(value);

                    this.LapAP_PreferredDisplayType = value == PowerDisplayType.Both ? mAutoTogglePowerDisplayType : value;

                    OnPowerDisplayTypeChangedEvent(new PowerDisplayTypeChangedEventArgs(DetailColumn.LapAP.ToString(), this.LapAP_PreferredDisplayType, this.DetailGrid));

                    foreach (var item in this.DetailRows)
                    {
                        item.UpdateLapAP(value);
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
                PowerDisplayType powerDisplayType;

                if (type == MeasurementSystemType.Imperial)
                {
                    speedDisplayType = SpeedDisplayType.MilesPerHour;
                    distanceDisplayType = DistanceDisplayType.Miles;
                    powerDisplayType = PowerDisplayType.Watts;
                }
                else
                {
                    speedDisplayType = SpeedDisplayType.KilometersPerHour;
                    distanceDisplayType = DistanceDisplayType.Kilometers;
                    powerDisplayType = PowerDisplayType.WattsPerKg;
                }

                this.mAutoToggleDistanceDisplayType = distanceDisplayType;
                this.mAutoToggleSpeedDisplayType = speedDisplayType;
                this.mAutoTogglePowerDisplayType = powerDisplayType;

                this.LapSpeed_PreferredDisplayType = this.LapSpeed_SelectedDisplayType == SpeedDisplayType.Both ? mAutoToggleSpeedDisplayType : this.LapSpeed_SelectedDisplayType;
                this.LapDistance_PreferredDisplayType = this.LapDistance_SelectedDisplayType == DistanceDisplayType.Both ? mAutoToggleDistanceDisplayType : this.LapDistance_SelectedDisplayType;
                this.LapAP_PreferredDisplayType = this.LapAP_SelectedDisplayType == PowerDisplayType.Both ? mAutoTogglePowerDisplayType : this.LapAP_SelectedDisplayType;

                if (this.DetailRows.Count > 0 || forceUpdate)
                {
                    OnSpeedDisplayTypeChangedEvent(new SpeedDisplayTypeChangedEventArgs(DetailColumn.LapSpeed.ToString(), this.LapSpeed_PreferredDisplayType, this.DetailGrid));
                    OnDistanceDisplayTypeChangedEvent(new DistanceDisplayTypeChangedEventArgs(DetailColumn.LapDistance.ToString(), this.LapDistance_PreferredDisplayType, this.DetailGrid));
                    OnPowerDisplayTypeChangedEvent(new PowerDisplayTypeChangedEventArgs(DetailColumn.LapAP.ToString(), this.LapAP_PreferredDisplayType, this.DetailGrid));
                }

                foreach (var item in this.DetailRows)
                {
                    item.UpdateLapDistance(distanceDisplayType);
                    item.UpdateLapSpeed(speedDisplayType);
                    item.UpdateLapAP(powerDisplayType);
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
            private void OnPowerDisplayTypeChangedEvent(PowerDisplayTypeChangedEventArgs e)
            {
                EventHandler<PowerDisplayTypeChangedEventArgs> handler = PowerDisplayTypeChangedEvent;

                if (handler != null)
                {
                    try
                    {
                        handler(null, e);
                    }
                    catch (Exception ex)
                    {
                        // Don't let downstream exceptions bubble up
                        Logger.LogError(ex, $"Caught in {this.GetType()} (OnPowerDisplayTypeChangedEvent)");
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

            public int LapNumber { get; }
            public TimeSpan LapTime { get { return this.mLapTime; } set { this.SetProperty<TimeSpan>(ref this.mLapTime, value); } }
            public string LapSpeed { get { return this.mLapSpeed; } set { this.SetProperty<string>(ref this.mLapSpeed, value); } }
            public string LapDistance { get { return this.mLapDistance; } set { this.SetProperty<string>(ref this.mLapDistance, value); } }
            public string LapAP { get { return this.mLapAP; } set { this.SetProperty<string>(ref this.mLapAP, value); } }
            public TimeSpan TotalTime { get { return this.mTotalTime; } set { this.SetProperty<TimeSpan>(ref this.mTotalTime, value); } }
            public string Blank { get; set; }

            private TimeSpan mLapTime;
            private string mLapSpeed;
            private double mLapSpeedKph;
            private double mLapSpeedMph;
            private string mLapDistance;
            private double mLapDistanceMi;
            private double mLapDistanceKm;
            private string mLapAP;
            private int mLapAPwatts;
            private double? mLapAPwattsPerKg;
            private TimeSpan mTotalTime;
            private readonly DataGridViewManager mViewManager; 

            public DetailRow(int lapNumber, DataGridViewManager viewManager)
            {
                this.LapNumber = lapNumber;
                this.mViewManager = viewManager;
            }

            /// <summary>
            /// Updates the displayed column appropriately
            /// </summary>
            /// <param name="updatedType"></param>
            public void UpdateLapSpeed(SpeedDisplayType updatedType)
            {
                SpeedDisplayType preferredType = this.mViewManager.LapSpeed_PreferredDisplayType;

                if (preferredType == updatedType)
                {
                    switch (updatedType)
                    {
                        case SpeedDisplayType.KilometersPerHour:
                            this.LapSpeed = this.LapSpeedKph >= 0 ? this.LapSpeedKph.ToString("0.0") : "";
                            break;

                        case SpeedDisplayType.MilesPerHour:
                            this.LapSpeed = this.LapSpeedMph >= 0 ? this.LapSpeedMph.ToString("0.0") : "";
                            break;
                    }
                }
                else if (preferredType == SpeedDisplayType.None)
                {
                    this.LapSpeed = "";
                }
            }

            /// <summary>
            /// Updates the displayed column appropriately
            /// </summary>
            /// <param name="updatedType"></param>
            public void UpdateLapDistance(DistanceDisplayType updatedType)
            {
                DistanceDisplayType preferredType = this.mViewManager.LapDistance_PreferredDisplayType;

                if (preferredType == updatedType)
                {
                    switch (updatedType)
                    {
                        case DistanceDisplayType.Kilometers:
                            this.LapDistance = this.LapDistanceKm >= 0 ? this.LapDistanceKm.ToString("0.0") : "";
                            break;

                        case DistanceDisplayType.Miles:
                            this.LapDistance = this.LapDistanceMi >= 0 ? this.LapDistanceMi.ToString("0.0") : "";
                            break;
                    }
                }
                else if (preferredType == DistanceDisplayType.None)
                {
                    this.LapDistance = "";
                }
            }

            /// <summary>
            /// Updates the displayed column appropriately
            /// </summary>
            /// <param name="updatedType"></param>
            public void UpdateLapAP(PowerDisplayType updatedType)
            {
                PowerDisplayType preferredType = this.mViewManager.LapAP_PreferredDisplayType;

                if (preferredType == updatedType)
                {
                    switch (updatedType)
                    {
                        case PowerDisplayType.Watts:
                            this.LapAP = this.mLapAPwatts > 0 ? this.mLapAPwatts.ToString() : "";
                            break;

                        case PowerDisplayType.WattsPerKg:
                            this.LapAP = this.mLapAPwattsPerKg.HasValue ? this.mLapAPwattsPerKg.Value.ToString("#.00") : "";
                            break;
                    }
                }
                else if (preferredType == PowerDisplayType.None)
                {
                    this.LapAP = "";
                }
            }

            public int CompareTo(int other)
            {
                return LapNumber.CompareTo(other);
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double LapSpeedKph
            {
                get { return this.mLapSpeedKph; }
                set
                {
                    this.mLapSpeedKph = value;
                    this.UpdateLapSpeed(SpeedDisplayType.KilometersPerHour);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double LapSpeedMph
            {
                get { return this.mLapSpeedMph; }
                set
                {
                    this.mLapSpeedMph = value;
                    this.UpdateLapSpeed(SpeedDisplayType.MilesPerHour);
                }
            }
            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double LapDistanceKm
            {
                get { return this.mLapDistanceKm; }
                set
                {
                    this.mLapDistanceKm = value;
                    this.UpdateLapDistance(DistanceDisplayType.Kilometers);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double LapDistanceMi
            {
                get { return this.mLapDistanceMi; }
                set
                {
                    this.mLapDistanceMi = value;
                    this.UpdateLapDistance(DistanceDisplayType.Miles);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public int LapAPwatts
            {
                get { return this.mLapAPwatts; }
                set
                {
                    this.mLapAPwatts = value;
                    this.UpdateLapAP(PowerDisplayType.Watts);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double? LapAPwattsPerKg
            {
                get { return this.mLapAPwattsPerKg; }
                set
                {
                    this.mLapAPwattsPerKg = value;
                    this.UpdateLapAP(PowerDisplayType.WattsPerKg);
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

            public string Blank { get; set; }

            private readonly DataGridViewManager mViewManager;

            public SummaryRow(DataGridViewManager viewManager)
            {
                this.mViewManager = viewManager;
            }
        }

        #endregion

        private Dispatcher UIdispatcher;
        private bool mInitialControlGainedFocus;

        private DataGridViewManager ViewManager;
        private LapsManager LapsManager;
        private int mStatusLabelSeconds;
        private readonly ILogger<LapViewerControl> Logger;

        public LapViewerControl()
        {
            InitializeComponent();

            if (DesignMode)
                return;

            this.toolStrip.Renderer = new ToolStripProfessionalRendererEx();

            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<LapViewerControl>();
        }

        #region LapViewerControl events

        private void ViewControl_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;

            Logger.LogDebug($"{this.GetType()}.ViewControl_Load");

            // for handling UI events
            this.UIdispatcher = Dispatcher.CurrentDispatcher;

            this.ViewManager = new(dgDetail);
            ViewManager.DistanceDisplayTypeChangedEvent += DataGridViewManager_DistanceDisplayTypeChangedEvent;
            ViewManager.SpeedDisplayTypeChangedEvent += DataGridViewManager_SpeedDisplayTypeChangedEvent;
            //ViewManager.PowerDisplayTypeChangedEvent += DataGridViewManager_PowerDisplayTypeChangedEvent; // Un-comment to allow AP column header to change
            ViewManager.Initialize();

            this.LapsManager = new();
            LapsManager.LapUpdatedEvent += this.LapEventHandler;
            LapsManager.LapStartedEvent += this.LapStartedEventHandler;

            // Subscribe to any SystemConfig or SplitsConfig changes
            ZAMsettings.SystemConfigChanged += ZAMsettings_SystemConfigChanged;  // get notified if current user possibly changes
            ZAMsettings.ZPMonitorService.CollectionStatusChanged += ZPMonitorService_CollectionStatusChanged;

            this.SetupDisplayForCurrentUserProfile();

            if (this.ParentForm is MainForm parentForm)
            {
                parentForm.FormSyncOneSecondTimerTickEvent += MainForm_FormSyncOneSecondTimerTickEvent;
                parentForm.FormSyncFiveSecondTimerTickEvent += MainForm_FormSyncFiveSecondTimerTickEvent;
            }

            // Trigger a resize so the the last column (Blank) can be shown/hidden if necessary
            this.OnResize(new EventArgs());
        }

        public override void ControlGainingFocus(object sender, EventArgs e)
        {
            Logger.LogDebug($"{this.GetType()}.ControlGainingFocus");

            if (!mInitialControlGainedFocus)
            {
                //Logger.LogDebug($"{this.GetType()}.ControlGainingFocus - Performing initializations");

                //int sumWidth = 0;
                //foreach (DataGridViewColumn c in this.dgDetail.Columns)
                //{
                //    sumWidth += c.Width;
                //    Logger.LogDebug($"{this.GetType()}.ControlGainingFocus - Column: {c.Name}, Width: {c.Width} ({sumWidth})");
                //}

                mInitialControlGainedFocus = true;
            }
        }

        private void ViewControl_Resize(object sender, EventArgs e)
        {
        }

        #endregion

        #region LapsManager event handling

        /// <summary>
        /// A delegate used solely by the LapEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void LapEventHandlerDelegate(object sender, LapEventArgs e);

        /// <summary>
        /// Occurs each time a lap gets updated or completes.  Allows for UI update by marshalling the call accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LapEventHandler(object sender, LapEventArgs e)
        {
            // Dispatcher.BeginInvoke IS required as the DataGridView is sorted when a new row is added.

            if (!UIdispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                UIdispatcher.BeginInvoke(new LapEventHandlerDelegate(LapEventHandler), new object[] { sender, e });
                return;
            }

            //Logger.LogDebug($"{this.GetType()} (LapEventHandler)");

            DetailRow detailRow = ViewManager.DetailRows.FirstOrDefault(r => r.LapNumber == e.LapNumber);

            if (detailRow == null)
            {
                // New lap row
                detailRow = new(e.LapNumber, ViewManager)
                {
                    LapTime = e.LapTime,
                    LapDistanceKm = e.LapDistanceKm,
                    LapDistanceMi = e.LapDistanceMi,
                    LapSpeedMph = e.LapSpeedMph,
                    LapSpeedKph = e.LapSpeedKph,
                    LapAPwatts = e.LapAPwatts,
                    LapAPwattsPerKg = e.LapAPwattsPerKg,
                    TotalTime = e.TotalTime,
                };
                ViewManager.DetailRows.Add(detailRow);

                dgDetail.Sort(dgDetail.Columns[0], ListSortDirection.Descending);
            }
            else
            {
                // Update lap in progress
                detailRow.LapTime = e.LapTime;
                detailRow.LapDistanceKm = e.LapDistanceKm;
                detailRow.LapDistanceMi = e.LapDistanceMi;
                detailRow.LapSpeedMph = e.LapSpeedMph;
                detailRow.LapSpeedKph = e.LapSpeedKph;
                detailRow.LapAPwatts = e.LapAPwatts;
                detailRow.LapAPwattsPerKg = e.LapAPwattsPerKg;
                detailRow.TotalTime = e.TotalTime;
            }
            //Logger.LogDebug($"LapEventHandler {LapItem.LapNumber}, {LapItem.LapTime}, {LapItem.LapSpeedKph}km/h, {LapItem.LapDistanceKm}km, {LapItem.TotalTime}");
        }


        /// <summary>
        /// A delegate used solely by the LapStartedEventHandler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private delegate void LapStartedEventHandlerDelegate(object sender, LapStartedEventArgs e);

        /// <summary>
        /// Occurs each time a lap gets updated or completes.  Allows for UI update by marshalling the call accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LapStartedEventHandler(object sender, LapStartedEventArgs e)
        {
            if (!UIdispatcher.CheckAccess()) // are we currently on the UI thread?
            {
                // We're not in the UI thread, ask the dispatcher to call this same method in the UI thread, then exit
                UIdispatcher.BeginInvoke(new LapStartedEventHandlerDelegate(LapStartedEventHandler), new object[] { sender, e });
                return;
            }

            SetStatusLabel(e.StatusMsg, 5);
        }

        private void SetStatusLabel(string msg, int seconds)
        {
            this.lblStatus.Text = msg;
            this.mStatusLabelSeconds = seconds;
        }


        public event EventHandler<LapEventArgs> LapCompletedEvent
        {
            add
            {
                this.LapsManager.LapCompletedEvent += value;
            }
            remove
            {
                this.LapsManager.LapCompletedEvent -= value;
            }
        }

        #endregion

        #region Supporting Event Handlers
        /// <summary>
        /// A timer event generated by MainForm to allow UserControls to syncronize data updates
        /// Occurs every second.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormSyncOneSecondTimerTickEvent(object sender, FormSyncTimerTickEventArgs e)
        {
            if (this.mStatusLabelSeconds > 0)
            {
                if (--this.mStatusLabelSeconds == 0)
                    this.lblStatus.Text = "";
            }
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

        private void ZPMonitorService_CollectionStatusChanged(object sender, CollectionStatusChangedEventArgs e)
        {
            Logger.LogDebug($"{this.GetType()}.ZPMonitorService_CollectionStatusChanged - {e.Action}");

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
            Logger.LogDebug($"{this.GetType()}.ZAMsettings_SystemConfigChanged");

            this.SetupDisplayForCurrentUserProfile();
        }
        private void tsbReset_Click(object sender, EventArgs e)
        {
            LapsManager.Reset();
            this.ClearDisplayValues();
        }

        private void tsbLap_Click(object sender, EventArgs e)
        {
            LapsManager.BeginNewLap();
        }

        #endregion

        #region Supporting Methods

        private void SetupDisplayForCurrentUserProfile()
        {
            this.dgDetail.Columns[(int)DetailColumn.LapDistance].Visible = CurrentUserProfile.LapViewColumnSettings.Visibility[LapViewMetricType.DetailLapDistance];
            this.dgDetail.Columns[(int)DetailColumn.LapNumber].Visible = CurrentUserProfile.LapViewColumnSettings.Visibility[LapViewMetricType.DetailLapNumber];
            this.dgDetail.Columns[(int)DetailColumn.LapSpeed].Visible = CurrentUserProfile.LapViewColumnSettings.Visibility[LapViewMetricType.DetailLapSpeed];
            this.dgDetail.Columns[(int)DetailColumn.LapTime].Visible = CurrentUserProfile.LapViewColumnSettings.Visibility[LapViewMetricType.DetailLapTime];
            this.dgDetail.Columns[(int)DetailColumn.LapAP].Visible = CurrentUserProfile.LapViewColumnSettings.Visibility[LapViewMetricType.DetailLapAP];
            this.dgDetail.Columns[(int)DetailColumn.TotalTime].Visible = CurrentUserProfile.LapViewColumnSettings.Visibility[LapViewMetricType.DetailTotalTime];
        }

        private void ClearDisplayValues()
        {
            ViewManager.DetailRows.Clear();
        }

        #endregion

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

        private void DataGridViewManager_PowerDisplayTypeChangedEvent(object sender, PowerDisplayTypeChangedEventArgs e)
        {
            //Logger.LogDebug($"{this.GetType()}.DataGridViewManager_PowerDisplayTypeChangedEvent - {e.ColumnName}, {e.DisplayType}");

            if (e.Tag is DataGridView dataGridView)
            {
                dataGridView.Columns[e.ColumnName].HeaderText = PowerDisplayEnum.Instance.GetColumnHeaderText(e.DisplayType);
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
                    case (int)DetailColumn.LapDistance:
                        // the LapViewMetricType is stored in the column header tag
                        metricType = (int)dataGridView.Columns[e.ColumnIndex].Tag;

                        foreach (var kvp in DistanceDisplayEnum.Instance.GetItems())
                        {
                            var mi = new ToolStripMenuItem(DistanceDisplayEnum.Instance.GetMenuItemText(kvp.Key))
                            {
                                CheckOnClick = true,
                                Tag = new object[] { kvp.Key, metricType, dataGridView }, // pass required values to the handler event
                                Checked = ViewManager.LapDistance_SelectedDisplayType == kvp.Key,
                            };
                            mi.CheckedChanged += UOM_ContextMenu_CheckChanged;
                            menuStrip.Items.Add(mi);
                        }

                        menuStrip.Show(Cursor.Position);
                        break;

                    case (int)DetailColumn.LapSpeed:
                        // the LapViewMetricType is stored in the column header tag
                        metricType = (int)dataGridView.Columns[e.ColumnIndex].Tag;

                        foreach (var kvp in SpeedDisplayEnum.Instance.GetItems())
                        {
                            var mi = new ToolStripMenuItem(SpeedDisplayEnum.Instance.GetMenuItemText(kvp.Key))
                            {
                                CheckOnClick = true,
                                Tag = new object[] { kvp.Key, metricType, dataGridView }, // pass required values to the handler event
                                Checked = ViewManager.LapSpeed_SelectedDisplayType == kvp.Key,
                            };
                            mi.CheckedChanged += UOM_ContextMenu_CheckChanged;
                            menuStrip.Items.Add(mi);
                        }

                        menuStrip.Show(Cursor.Position);
                        break;

                    case (int)DetailColumn.LapAP:
                        // the LapViewMetricType is stored in the column header tag
                        metricType = (int)dataGridView.Columns[e.ColumnIndex].Tag;

                        foreach (var kvp in PowerDisplayEnum.Instance.GetItems())
                        {
                            var mi = new ToolStripMenuItem(PowerDisplayEnum.Instance.GetMenuItemText(kvp.Key))
                            {
                                CheckOnClick = true,
                                Tag = new object[] { kvp.Key, metricType, dataGridView }, // pass required values to the handler event
                                Checked = ViewManager.LapAP_SelectedDisplayType == kvp.Key,
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
            LapViewMetricType metricType = (LapViewMetricType)tag[1]; // column to set
            DataGridView dataGridView = (DataGridView)tag[2];

            if (item.Checked)
            {
                if (displayType is DistanceDisplayType distanceDisplayType)
                {
                    if (CurrentUserProfile.LapViewColumnSettings.DistanceValues.ContainsKey(metricType))
                    {
                        ZAMsettings.BeginCachedConfiguration();
                        if (dataGridView == dgDetail)
                            ViewManager.LapDistance_SelectedDisplayType = distanceDisplayType;
                        ZAMsettings.CommitCachedConfiguration();
                    }
                }
                else if (displayType is SpeedDisplayType speedDisplayType)
                {
                    if (CurrentUserProfile.LapViewColumnSettings.SpeedValues.ContainsKey(metricType))
                    {
                        ZAMsettings.BeginCachedConfiguration();
                        if (dataGridView == dgDetail)
                            ViewManager.LapSpeed_SelectedDisplayType = speedDisplayType;
                        ZAMsettings.CommitCachedConfiguration();
                    }
                }
                else if (displayType is PowerDisplayType powerDisplayType)
                {
                    if (CurrentUserProfile.LapViewColumnSettings.PowerValues.ContainsKey(metricType))
                    {
                        ZAMsettings.BeginCachedConfiguration();
                        if (dataGridView == dgDetail)
                            ViewManager.LapAP_SelectedDisplayType = powerDisplayType;
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
                if (c.HeaderText != "" && c.Tag is LapViewMetricType metricType) // exclude filler column
                {
                    var mi = new ToolStripMenuItem(LapViewMetricEnum.Instance.GetMenuItemText(metricType))
                    {
                        Tag = new object[] { c.Index, metricType, dataGridView },
                        Checked = c.Visible,
                        CheckOnClick = true,
                        Enabled = (dataGridView != dgDetail || c.Index != (int)DetailColumn.LapNumber),
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
            LapViewMetricType metricType = (LapViewMetricType)tag[1];
            DataGridView dataGridView = (DataGridView)tag[2];

            foreach (DataGridViewColumn c in dataGridView.Columns)
                if (c.Tag != null && c.Visible)
                    visibleCount++;

            if (item.Checked || visibleCount > 1)
            {
                dataGridView.Columns[colIndex].Visible = item.Checked;

                if (CurrentUserProfile.LapViewColumnSettings.Visibility.ContainsKey(metricType))
                {
                    ZAMsettings.BeginCachedConfiguration();
                    CurrentUserProfile.LapViewColumnSettings.Visibility[metricType] = item.Checked;
                    ZAMsettings.CommitCachedConfiguration();
                }
            }
        }

        #endregion

        #region Base Class Overrides
        protected override void HeaderGradientEndColorChanged()
        {
            this.tlPanel.BackColor = this.HeaderGradientEndColor; // set to match column headers, don't want it to be transparent
        }

        protected override void HeaderForeColorChanged()
        {
            base.HeaderForeColorChanged();

            // change the text color on the data grid headers
            this.dgDetail.ColumnHeadersDefaultCellStyle.ForeColor = this.HeaderForeColor;
            //this.dgSummary.ColumnHeadersDefaultCellStyle.ForeColor = this.HeaderForeColor;

            this.lblStatus.ForeColor = this.HeaderForeColor; // set to match column headers
        }

        protected override void RowFontChanged()
        {
            base.RowFontChanged();

            // change the font on the data grid rows
            this.dgDetail.RowsDefaultCellStyle.Font = this.RowFont;
            this.dgDetail.DefaultCellStyle.Font = this.RowFont;

            // Trigger a resize so that dgSummary can size itself appropriately
            this.OnResize(new EventArgs());
        }

        protected override void RowBackColorChanged()
        {
            base.RowBackColorChanged();

            // change the back color on the data grid rows
            this.dgDetail.RowsDefaultCellStyle.BackColor = this.RowBackColor;
            //this.dgSummary.RowsDefaultCellStyle.BackColor = this.RowBackColor;

            this.dgDetail.RowsDefaultCellStyle.SelectionBackColor = this.RowBackColor;  // this hides the Detail cell selection box by making it the same as row back color
            //this.dgSummary.RowsDefaultCellStyle.SelectionBackColor = value; // this hides the Summary cell selection box by making it the same as row back color 

            this.dgDetail.BackgroundColor = this.RowBackColor;
            //this.dgSummary.BackgroundColor = this.RowBackColor;
        }

        protected override void RowForeColorChanged()
        {
            base.RowForeColorChanged();

            // change the fore color on the data grid rows
            this.dgDetail.RowsDefaultCellStyle.ForeColor = this.RowForeColor;
            //this.dgSummary.RowsDefaultCellStyle.ForeColor = this.RowForeColor;

            this.dgDetail.RowsDefaultCellStyle.SelectionForeColor = this.RowForeColor;  // this blends the Detail cell selection text by making it the same as row fore color 
            //this.dgSummary.RowsDefaultCellStyle.SelectionForeColor = this.RowForeColor; // this blends the Summary cell selection text by making it the same as row fore color 
        }

        #endregion
    }
}
