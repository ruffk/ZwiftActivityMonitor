using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.ComponentModel;
using System.Threading;
using System.Runtime.CompilerServices;

namespace ZwiftActivityMonitorV2
{

    public partial class ActivityViewerControl : ViewerUserControlEx
    {
        protected enum DetailColumn
        {
            Period = 0,
            PeriodSecs,
            AP,
            APmax,
            FTP,
            HR,
            Blank,
            AP_PowerDisplayType,
            APmax_PowerDisplayType,
            FTP_PowerDisplayType,
        }

        protected enum SummaryColumn
        {
            AS = 0,
            AP,
            NP,
            IF,
            TSS,
            Blank,
            AP_PowerDisplayType,
            NP_PowerDisplayType,
            AS_SpeedDisplayType,
        }

        public class NotifyPropertyChangedBase : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }

            /// <summary>
            /// Conditionally updates a property only if the value has actually changed.  If so, NotifyPropertyChanged is also called.
            /// This is to save on updates to the DataGridView.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="property"></param>
            /// <param name="valueToSet"></param>
            /// <param name="propertyName"></param>
            protected bool SetProperty<T>(ref T property, T valueToSet, [CallerMemberName] string propertyName = "")
            {
                if (property == null || !property.Equals(valueToSet))
                {
                    //Debug.WriteLine($"SetProperty<T> NOT EQUAL - Name: {propertyName}, Type: {typeof(T)} Current: {property}, New: {valueToSet}");

                    property = valueToSet;
                    this.NotifyPropertyChanged(propertyName);
                    return true;
                }
                //else Debug.WriteLine($"SetProperty<T> EQUAL - Name: {propertyName}, Type: {typeof(T)} Current: {property}, New: {valueToSet}");
                return false;
            }

        }

        #region DetailRow class

        protected class DetailRow : NotifyPropertyChangedBase
        {
            //Add the [Browsable(false)] attribute to any public properties you don't want columns created for in the DataGridView
            public string Period { get; set; }
            public int PeriodSecs { get; set; }
            public string AP { get { return this.mAP; } set { this.SetProperty<string>(ref this.mAP, value); } }
            public string APmax { get { return this.mAPmax; } set { this.SetProperty<string>(ref this.mAPmax, value); } }
            public string FTP { get { return this.mFTP; } set { this.SetProperty<string>(ref this.mFTP, value); } }
            public string HR { get { return this.mHR; } set { this.SetProperty<string>(ref this.mHR, value); } }
            public string Blank { get; set; }
            public PowerDisplayType AP_PowerDisplayType
            {
                get { return this.mAP_PowerDisplayType; }
                set
                {
                    this.SetProperty<PowerDisplayType>(ref this.mAP_PowerDisplayType, value);
                    UpdateAP(value);
                }
            }
            public PowerDisplayType APmax_PowerDisplayType
            {
                get { return this.mAPmax_PowerDisplayType; }
                set
                {
                    this.SetProperty<PowerDisplayType>(ref this.mAPmax_PowerDisplayType, value);
                    UpdateAPmax(value);
                }
            }
            public PowerDisplayType FTP_PowerDisplayType
            {
                get { return this.mFTP_PowerDisplayType; }
                set
                {
                    this.SetProperty<PowerDisplayType>(ref this.mFTP_PowerDisplayType, value);
                    UpdateFTP(value);
                }
            }

            private string mAP;
            private PowerDisplayType mAP_PowerDisplayType;
            private PowerDisplayType mAPmax_PowerDisplayType;
            private PowerDisplayType mFTP_PowerDisplayType;
            private string mAPmax;
            private string mFTP;
            private string mHR;

            private int mAPwatts;
            private double? mAPwattsPerKg;
            private int mAPwattsMax;
            private double? mAPwattsPerKgMax;
            private int mFTPwatts;
            private double? mFTPwattsPerKg;
            private int mHRbpm;
            private MeasurementSystemType mCurrentMeasurementSystemType = MeasurementSystemType.Imperial;
            private PowerDisplayType mCurrentPowerDisplayType = PowerDisplayType.Watts;

            [Browsable(false)]
            public bool IsRowVisible { get; set; }



            public void SetCurrentMeasurementSystemType(MeasurementSystemType type)
            {
                this.mCurrentMeasurementSystemType = type;

                if (type == MeasurementSystemType.Imperial)
                {
                    this.mCurrentPowerDisplayType = PowerDisplayType.Watts;
                }
                else
                {
                    this.mCurrentPowerDisplayType = PowerDisplayType.WattsPerKg;
                }

                this.UpdateAP(this.mCurrentPowerDisplayType);
                this.UpdateAPmax(this.mCurrentPowerDisplayType);
                this.UpdateFTP(this.mCurrentPowerDisplayType);
            }

            public PowerDisplayType GetPreferredType(PowerDisplayType currentType)
            {
                PowerDisplayType preferredType = currentType == PowerDisplayType.Both ? this.mCurrentPowerDisplayType : currentType;

                return preferredType;
            }


            /// <summary>
            /// Updates the displayed column appropriately
            /// </summary>
            /// <param name="updatedType"></param>
            private void UpdateAP(PowerDisplayType updatedType)
            {
                PowerDisplayType preferredType = GetPreferredType(this.AP_PowerDisplayType);

                if (preferredType == updatedType)
                {
                    switch (updatedType)
                    {
                        case PowerDisplayType.Watts:
                            this.AP = this.APwatts > 0 ? this.APwatts.ToString() : "";
                            break;

                        case PowerDisplayType.WattsPerKg:
                            this.AP = this.APwattsPerKg.HasValue ? this.APwattsPerKg.Value.ToString("#.00") : "";
                            break;
                    }
                }
                else if (preferredType == PowerDisplayType.None)
                {
                    this.AP = "";
                }
            }

            /// <summary>
            /// Updates the displayed column appropriately
            /// </summary>
            /// <param name="updatedType"></param>
            private void UpdateAPmax(PowerDisplayType updatedType)
            {
                PowerDisplayType preferredType = GetPreferredType(this.APmax_PowerDisplayType);

                if (preferredType == updatedType)
                {
                    switch (updatedType)
                    {
                        case PowerDisplayType.Watts:
                            this.APmax = this.APwattsMax > 0 ? this.APwattsMax.ToString() : "";
                            break;

                        case PowerDisplayType.WattsPerKg:
                            this.APmax = this.APwattsPerKgMax.HasValue ? this.APwattsPerKgMax.Value.ToString("#.00") : "";
                            break;
                    }
                }
                else if (preferredType == PowerDisplayType.None)
                {
                    this.APmax = "";
                }
            }

            /// <summary>
            /// Updates the displayed column appropriately
            /// </summary>
            /// <param name="updatedType"></param>
            private void UpdateFTP(PowerDisplayType updatedType)
            {
                PowerDisplayType preferredType = GetPreferredType(this.FTP_PowerDisplayType);

                if (preferredType == updatedType)
                {
                    switch (updatedType)
                    {
                        case PowerDisplayType.Watts:
                            this.FTP = this.FTPwatts > 0 ? this.FTPwatts.ToString() : "";
                            break;

                        case PowerDisplayType.WattsPerKg:
                            this.FTP = this.FTPwattsPerKg.HasValue ? this.FTPwattsPerKg.Value.ToString("#.00") : "";
                            break;
                    }
                }
                else if (preferredType == PowerDisplayType.None)
                {
                    this.FTP = "";
                }

            }

            /// <summary>
            /// Updates the displayed column appropriately
            /// </summary>
            /// <param name="updatedType"></param>
            private void UpdateHR()
            {
                this.HR = this.HRbpm > 0 ? this.HRbpm.ToString() : "";
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public int APwatts
            {
                get { return this.mAPwatts; }
                set
                {
                    this.mAPwatts = value;
                    this.UpdateAP(PowerDisplayType.Watts);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double? APwattsPerKg
            {
                get { return this.mAPwattsPerKg; }
                set
                {
                    this.mAPwattsPerKg = value;
                    this.UpdateAP(PowerDisplayType.WattsPerKg);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public int APwattsMax
            {
                get { return this.mAPwattsMax; }
                set
                {
                    this.mAPwattsMax = value;
                    this.UpdateAPmax(PowerDisplayType.Watts);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double? APwattsPerKgMax
            {
                get { return this.mAPwattsPerKgMax; }
                set
                {
                    this.mAPwattsPerKgMax = value;
                    this.UpdateAPmax(PowerDisplayType.WattsPerKg);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public int FTPwatts
            {
                get { return this.mFTPwatts; }
                set
                {
                    this.mFTPwatts = value;
                    this.UpdateFTP(PowerDisplayType.Watts);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double? FTPwattsPerKg
            {
                get { return this.mFTPwattsPerKg; }
                set
                {
                    this.mFTPwattsPerKg = value;
                    this.UpdateFTP(PowerDisplayType.WattsPerKg);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public int HRbpm
            {
                get { return this.mHRbpm; }
                set
                {
                    this.mHRbpm = value;
                    this.UpdateHR();
                }
            }

        }

        #endregion

        private BindingList<DetailRow> DetailRows = new();

        #region SummaryRow class

        protected class SummaryRow : NotifyPropertyChangedBase
        {
            public string AS { get { return this.mAS; } set { this.SetProperty<string>(ref this.mAS, value); } }
            public string AP { get { return this.mAP; } set { this.SetProperty<string>(ref this.mAP, value); } }
            public string NP { get { return this.mNP; } set { this.SetProperty<string>(ref this.mNP, value); } }
            public string IF { get { return this.mIF; } set { this.SetProperty<string>(ref this.mIF, value); } }
            public string TSS { get { return this.mTSS; } set { this.SetProperty<string>(ref this.mTSS, value); } }
            public string Blank { get; set; }
            public PowerDisplayType AP_PowerDisplayType
            {
                get { return this.mAP_PowerDisplayType; }
                set
                {
                    this.SetProperty<PowerDisplayType>(ref this.mAP_PowerDisplayType, value);
                    UpdateAP(value);
                }
            }
            public PowerDisplayType NP_PowerDisplayType
            {
                get { return this.mNP_PowerDisplayType; }
                set
                {
                    this.SetProperty<PowerDisplayType>(ref this.mNP_PowerDisplayType, value);
                    UpdateNP(value);
                }
            }
            public SpeedDisplayType AS_SpeedDisplayType
            {
                get { return this.mAS_SpeedDisplayType; }
                set
                {
                    this.SetProperty<SpeedDisplayType>(ref this.mAS_SpeedDisplayType, value);
                    UpdateAS(value);
                }
            }

            private PowerDisplayType mAP_PowerDisplayType;
            private PowerDisplayType mNP_PowerDisplayType;
            private SpeedDisplayType mAS_SpeedDisplayType;
            private string mAS;
            private string mAP;
            private string mNP;
            private string mIF;
            private string mTSS;

            private int mAPwatts;
            private double? mAPwattsPerKg;
            private int mNPwatts;
            private double? mNPwattsPerKg;
            private double mSpeedKph;
            private double mSpeedMph;
            private MeasurementSystemType mCurrentMeasurementSystemType = MeasurementSystemType.Imperial;
            private PowerDisplayType mCurrentPowerDisplayType = PowerDisplayType.Watts;
            private SpeedDisplayType mCurrentSpeedDisplayType = SpeedDisplayType.MilesPerHour;


            public void SetCurrentMeasurementSystemType(MeasurementSystemType type)
            {
                this.mCurrentMeasurementSystemType = type;

                if (type == MeasurementSystemType.Imperial)
                {
                    this.mCurrentPowerDisplayType = PowerDisplayType.Watts;
                    this.mCurrentSpeedDisplayType = SpeedDisplayType.MilesPerHour;
                }
                else
                {
                    this.mCurrentPowerDisplayType = PowerDisplayType.WattsPerKg;
                    this.mCurrentSpeedDisplayType = SpeedDisplayType.KilometersPerHour;
                }

                this.UpdateAP(this.mCurrentPowerDisplayType);
                this.UpdateNP(this.mCurrentPowerDisplayType);
                this.UpdateAS(this.mCurrentSpeedDisplayType);
            }

            public PowerDisplayType GetPreferredType(PowerDisplayType currentType)
            {
                PowerDisplayType preferredType = currentType == PowerDisplayType.Both ? this.mCurrentPowerDisplayType : currentType;

                return preferredType;
            }
            public SpeedDisplayType GetPreferredType(SpeedDisplayType currentType)
            {
                SpeedDisplayType preferredType = currentType == SpeedDisplayType.Both ? this.mCurrentSpeedDisplayType : currentType;

                return preferredType;
            }

            /// <summary>
            /// Updates the displayed column appropriately
            /// </summary>
            /// <param name="updatedType"></param>
            private void UpdateAP(PowerDisplayType updatedType)
            {
                PowerDisplayType preferredType = GetPreferredType(this.AP_PowerDisplayType);

                if (preferredType == updatedType)
                {
                    switch (updatedType)
                    {
                        case PowerDisplayType.Watts:
                            this.AP = this.APwatts > 0 ? this.APwatts.ToString() : "";
                            break;

                        case PowerDisplayType.WattsPerKg:
                            this.AP = this.APwattsPerKg.HasValue ? this.APwattsPerKg.Value.ToString("#.00") : "";
                            break;
                    }
                }
                else if (preferredType == PowerDisplayType.None)
                {
                    this.AP = "";
                }
            }

            /// <summary>
            /// Updates the displayed column appropriately
            /// </summary>
            /// <param name="updatedType"></param>
            private void UpdateNP(PowerDisplayType updatedType)
            {
                PowerDisplayType preferredType = GetPreferredType(this.NP_PowerDisplayType);

                if (preferredType == updatedType)
                {
                    switch (updatedType)
                    {
                        case PowerDisplayType.Watts:
                            this.NP = this.NPwatts > 0 ? this.NPwatts.ToString() : "";
                            break;

                        case PowerDisplayType.WattsPerKg:
                            this.NP = this.NPwattsPerKg.HasValue ? this.NPwattsPerKg.Value.ToString("#.00") : "";
                            break;
                    }
                }
                else if (preferredType == PowerDisplayType.None)
                {
                    this.NP = "";
                }
            }

            /// <summary>
            /// Updates the displayed column appropriately
            /// </summary>
            /// <param name="updatedType"></param>
            private void UpdateAS(SpeedDisplayType updatedType)
            {
                SpeedDisplayType preferredType = GetPreferredType(this.AS_SpeedDisplayType);

                if (preferredType == updatedType)
                {
                    switch (updatedType)
                    {
                        case SpeedDisplayType.KilometersPerHour:
                            this.AS = this.SpeedKph > 0 ? this.SpeedKph.ToString("#.0") : "";
                            break;

                        case SpeedDisplayType.MilesPerHour:
                            this.AS = this.SpeedMph > 0 ? this.SpeedMph.ToString("#.0") : "";
                            break;
                    }
                }
                else if (preferredType == SpeedDisplayType.None)
                {
                    this.AS = "";
                }

            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public int APwatts
            {
                get { return this.mAPwatts; }
                set
                {
                    this.mAPwatts = value;
                    this.UpdateAP(PowerDisplayType.Watts);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double? APwattsPerKg
            {
                get { return this.mAPwattsPerKg; }
                set
                {
                    this.mAPwattsPerKg = value;
                    this.UpdateAP(PowerDisplayType.WattsPerKg);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public int NPwatts
            {
                get { return this.mNPwatts; }
                set
                {
                    this.mNPwatts = value;
                    this.UpdateNP(PowerDisplayType.Watts);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double? NPwattsPerKg
            {
                get { return this.mNPwattsPerKg; }
                set
                {
                    this.mNPwattsPerKg = value;
                    this.UpdateNP(PowerDisplayType.WattsPerKg);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double SpeedKph
            {
                get { return this.mSpeedKph; }
                set
                {
                    this.mSpeedKph = value;
                    this.UpdateAS(SpeedDisplayType.KilometersPerHour);
                }
            }

            /// <summary>
            /// Saves the value privately and updates the displayed field if the units match
            /// </summary>
            [Browsable(false)]
            public double SpeedMph
            {
                get { return this.mSpeedMph; }
                set
                {
                    this.mSpeedMph = value;
                    this.UpdateAS(SpeedDisplayType.MilesPerHour);
                }
            }
        }

        #endregion

        private BindingList<SummaryRow> SummaryRows = new();

        #region SyncBindingSource class

        /// <summary>
        /// Since the DataGridView is getting updated on non-gui threads, we're using a syncronized binding source to marshall the updates.  See link for details.
        /// https://stackoverflow.com/questions/32885552/update-elements-in-bindingsource-via-separate-task
        /// </summary>
        public class SyncBindingSource : BindingSource
        {
            private SynchronizationContext syncContext;
            public SyncBindingSource()
            {
                syncContext = SynchronizationContext.Current;
            }
            public SyncBindingSource(object dataSource, string dataMember) : base(dataSource, dataMember)
            {
                syncContext = SynchronizationContext.Current;
            }
            public SyncBindingSource(IContainer container) : base (container)
            {
                syncContext = SynchronizationContext.Current;
            }

            protected override void OnListChanged(ListChangedEventArgs e)
            {
                if (syncContext != null)
                    syncContext.Send(_ => base.OnListChanged(e), null);
                else
                    base.OnListChanged(e);
            }
        }

        #endregion


        private SyncBindingSource DetailBindingSource { get; set; }
        private SyncBindingSource SummaryBindingSource { get; set; }

        protected class MovingAverageManager
        {
            public class CollectorAttributes
            {
                public DurationType DurationType { get; }
                public string Label { get; }
                public MovingAverage MAcollector { get; }
                public DetailRow DetailDataRow { get; set; } = null;

                public CollectorAttributes(DurationType durationType, string label)
                {
                    this.DurationType = durationType;
                    this.Label = label;
                    this.MAcollector = new MovingAverage(durationType);

                    this.MAcollector.MovingAverageChangedEvent += MAcollector_MovingAverageChangedEvent;
                    this.MAcollector.MovingAverageMaxChangedEvent += MAcollector_MovingAverageMaxChangedEvent;
                }

                private void MAcollector_MovingAverageMaxChangedEvent(object sender, MovingAverageMaxChangedEventArgs e)
                {
                    this.DetailDataRow.APwattsMax = e.APwattsMax;
                    this.DetailDataRow.APwattsPerKgMax = e.APwattsPerKgMax;
                    this.DetailDataRow.FTPwatts = e.FTPwattsMax;
                    this.DetailDataRow.FTPwattsPerKg = e.FTPwattsPerKgMax;
                }

                private void MAcollector_MovingAverageChangedEvent(object sender, MovingAverageChangedEventArgs e)
                {
                    this.DetailDataRow.APwatts = e.APwatts;
                    this.DetailDataRow.APwattsPerKg = e.APwattsPerKg;
                    this.DetailDataRow.HRbpm = e.HRbpm;

                    if (!e.ignoreFTP)
                    {
                        this.DetailDataRow.FTPwatts = e.FTPwatts;
                        this.DetailDataRow.FTPwattsPerKg = e.FTPwattsPerKg;
                    }
                }
            }

            public class CollectorAttributesCollection : Dictionary<DurationType, CollectorAttributes>
            {
            }

            private ActivityViewerControl mParent;

            private CollectorAttributesCollection mCollectorAttributes = new();
            private NormalizedPower mNormalizedPower;
            private SummaryRow mSummaryRow;
            //private readonly System.Threading.Timer Timer;
            //public event EventHandler<EventArgs> SomeEvent;


            public MovingAverageManager()
            {
                // Create a CollectorAttributes class for each DurationType enum
                foreach (var kvp in DurationEnum.Instance.GetItems())
                {
                    mCollectorAttributes.Add(kvp.Key, new CollectorAttributes(kvp.Key, kvp.Value));
                }

                //Timer = new(OnTimer, null, 5000, 1000);

                mNormalizedPower = new();

                mNormalizedPower.NormalizedPowerChangedEvent += NormalizedPower_NormalizedPowerChangedEvent;
                mNormalizedPower.MetricsChangedEvent += NormalizedPower_MetricsChangedEvent;
                ZAMsettings.ZPMonitorService.CollectionStatusChanged += ZPMonitorService_CollectionStatusChanged;
            }

            public ActivityViewerControl Parent
            {
                get { return this.mParent; }
                set 
                {
                    this.mParent = value;

                    MainForm mainForm = this.Parent.ParentForm as MainForm;

                    Debug.Assert(mainForm != null, $"MovingAverageManager - Set Parent - MainForm is null");
                    
                    if (mainForm != null)
                    {
                        mainForm.FormSyncFiveSecondTimerTickEvent += MainForm_FormSyncFiveSecondTimerTickEvent;
                    }

                }
            }

            public SummaryRow SummaryDataRow 
            { 
                get { return this.mSummaryRow; }
                set
                {
                    this.mSummaryRow = value;
                    if (this.mSummaryRow != null)
                    {
                        this.mSummaryRow.PropertyChanged += SummaryRow_PropertyChanged;
                    }
                }
            }

            private void SummaryRow_PropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                SummaryRow row = sender as SummaryRow;

                if (row == null || this.Parent == null)
                    return;

                if (e.PropertyName == SummaryColumn.AS.ToString())
                {
                    SpeedDisplayType preferredType = row.GetPreferredType(row.AS_SpeedDisplayType);

                    switch(preferredType)
                    {
                        case SpeedDisplayType.KilometersPerHour:
                        case SpeedDisplayType.MilesPerHour:
                            this.Parent.SetSummaryHeaderText(SummaryColumn.AS, SpeedDisplayEnum.Instance.GetValue(preferredType)); ;
                            break;

                        default:
                            this.Parent.SetSummaryHeaderText(SummaryColumn.AS, ""); ;
                            break;
                    }
                }
            }

            private void MainForm_FormSyncFiveSecondTimerTickEvent(object sender, FormSyncTimerTickEventArgs e)
            {
                // determine type of units to display, alternate every 5 seconds
                MeasurementSystemType type = e.TickCount % 2 == 0 ? MeasurementSystemType.Imperial : MeasurementSystemType.Metric;

                foreach (var ca in this.GetCollectorAttributes())
                {
                    ca.DetailDataRow.SetCurrentMeasurementSystemType(type);
                }

                SummaryDataRow.SetCurrentMeasurementSystemType(type);
                
                //Debug.WriteLine($"MainForm_FormSyncFiveSecondTimerTickEvent - TickCount: {e.TickCount}, Type: {type}");
            }

            private void ZPMonitorService_CollectionStatusChanged(object sender, CollectionStatusChangedEventArgs e)
            {
            }

            private void NormalizedPower_MetricsChangedEvent(object sender, MetricsChangedEventArgs e)
            {
                this.SummaryDataRow.APwatts = e.APwatts;
                this.SummaryDataRow.APwattsPerKg = e.APwattsPerKg; ;
                this.SummaryDataRow.SpeedKph = e.SpeedKph;
                this.SummaryDataRow.SpeedMph = e.SpeedMph;
            }

            private void NormalizedPower_NormalizedPowerChangedEvent(object sender, NormalizedPowerChangedEventArgs e)
            {
                this.SummaryDataRow.IF = e.IFvalue.HasValue ? e.IFvalue.ToString() : null;
                this.SummaryDataRow.TSS = e.TSSvalue.HasValue ? e.TSSvalue.ToString() : null;
                this.SummaryDataRow.NPwatts = e.NPwatts;
                this.SummaryDataRow.NPwattsPerKg = e.NPwattsPerKg;
            }

            private void OnTimer(object state)
            {
                Debug.WriteLine("OnTimer");

                //foreach (var c in mCollectorAttributes)
                //{
                //    c.Value.DetailDataRow.AP = (Convert.ToInt32(c.Value.DetailDataRow.AP == "" ? "0" : c.Value.DetailDataRow.AP) + 1).ToString();
                //    c.Value.DetailDataRow.APmax = (Convert.ToInt32(c.Value.DetailDataRow.APmax == "" ? "0" : c.Value.DetailDataRow.APmax) + 2).ToString();
                //    c.Value.DetailDataRow.FTP = (Convert.ToInt32(c.Value.DetailDataRow.FTP == "" ? "0" : c.Value.DetailDataRow.FTP) + 3).ToString();
                //    c.Value.DetailDataRow.HR = (Convert.ToInt32(c.Value.DetailDataRow.HR == "" ? "0" : c.Value.DetailDataRow.HR) + 4).ToString();
                //}

                //this.SummaryDataRow.Speed = (Convert.ToInt32(this.SummaryDataRow.Speed == "" ? "0" : this.SummaryDataRow.Speed) + 1).ToString();
                //this.SummaryDataRow.AP = (Convert.ToInt32(this.SummaryDataRow.AP == "" ? "0" : this.SummaryDataRow.AP) + 2).ToString();
                //this.SummaryDataRow.NP = (Convert.ToInt32(this.SummaryDataRow.NP == "" ? "0" : this.SummaryDataRow.NP) + 3).ToString();
                //this.SummaryDataRow.IF = (Convert.ToInt32(this.SummaryDataRow.IF == "" ? "0" : this.SummaryDataRow.IF) + 4).ToString();
                //this.SummaryDataRow.TSS = (Convert.ToInt32(this.SummaryDataRow.TSS == "" ? "0" : this.SummaryDataRow.TSS) + 5).ToString();
            }

            public List<CollectorAttributes> GetCollectorAttributes()
            {
                return mCollectorAttributes.Values.ToList<CollectorAttributes>();
            }
        }

        private MovingAverageManager mMovingAverageManager;

        public ActivityViewerControl()
        {
            //Debug.WriteLine($"ActivityViewerControl_ctor started...");
            InitializeComponent();

            mMovingAverageManager = new();

            InitializeDetailDataGrid();
            InitializeSummaryDataGrid();

            // Subscribe to any SystemConfig changes
            ZAMsettings.SystemConfigChanged += ZAMsettings_SystemConfigChanged;
            ZAMsettings.ZPMonitorService.CollectionStatusChanged += ZPMonitorService_CollectionStatusChanged;

            //Debug.WriteLine($"ActivityViewerControl_ctor completed.");
        }

        private void ZPMonitorService_CollectionStatusChanged(object sender, CollectionStatusChangedEventArgs e)
        {
            Debug.WriteLine($"{this.GetType()}.ZPMonitorService_CollectionStatusChanged - {e.Action}");
        }

        private void ZAMsettings_SystemConfigChanged(object sender, EventArgs e)
        {
            //this.CurrentUserProfile = ZAMsettings.Settings.CurrentUser;
            SetRowColumnVisibilityStatusForUser();
            
            Debug.WriteLine($"ZAMsettings_SystemConfigChanged - {this.GetType()}");
        }

        private void ViewControl_Load(object sender, EventArgs e)
        {
            Debug.WriteLine($"ActivityViewerControl_Load");

            // Get the MovingAverageManager access to this control
            mMovingAverageManager.Parent = this;

            // Get the currently selected user
            //this.CurrentUserProfile = ZAMsettings.Settings.CurrentUser;
            //this.SetRowVisibilityStatus();
            SetRowColumnVisibilityStatusForUser();

            // Trigger a resize so that dgSummary can size itself appropriately
            this.OnResize(new EventArgs());

            //Debug.WriteLine($"ViewControl_Load2 - Row[0].Visible: {dgDetail.Rows[0].Visible}");
        }

        public override void Control_PostLoad()
        {
            Debug.WriteLine($"Control_PostLoad");
        }

        private UserProfile CurrentUserProfile 
        { 
            get { return ZAMsettings.Settings.CurrentUser; }
        }

        private void InitializeDetailDataGrid()
        {
            Debug.WriteLine($"InitializeDetailDataGrid1");

            // set in designer
            //dgDetail.ReadOnly = true;

            // Add all known Collectors to the view.  Later, row visibility will be set.
            foreach (var collector in mMovingAverageManager.GetCollectorAttributes())
            {
                DetailRow row = new()
                {
                    Period = collector.Label,
                    PeriodSecs = (int)collector.DurationType,
                    //AP = "",
                    //APmax = "",
                    //FTP = "",
                    //HR = "",
                    //Blank = "",
                    //AP_PowerDisplayType = CurrentUserProfile.Collectors[collector.DurationType].PowerValues[CollectorMetricType.DetailAP].Key,
                    //APmax_PowerDisplayType = CurrentUserProfile.Collectors[collector.DurationType].PowerValues[CollectorMetricType.DetailAPmax].Key,
                    //FTP_PowerDisplayType = CurrentUserProfile.Collectors[collector.DurationType].PowerValues[CollectorMetricType.DetailFTP].Key,
                };
                this.DetailRows.Add(row);

                collector.DetailDataRow = row;
            }

            // Note: anytime rows are added to the List, the BindingSource must be recreated (or maybe just a reset on the BindingSource)
            this.DetailBindingSource = new SyncBindingSource();
            this.DetailBindingSource.DataSource = this.DetailRows;

            this.dgDetail.DataSource = this.DetailBindingSource;


            for (int i = 0; i < dgDetail.Rows.Count; i++)
            {
                DataGridViewRow r = dgDetail.Rows[i];
                DataGridViewCell c = dgDetail[(int)DetailColumn.AP, i];


                // A height of 19 is minimum when using Segoe UI 9pt font
                r.MinimumHeight = DataGridRowMinimumHeight;
            }

            this.dgDetail.Columns[(int)DetailColumn.Period].Width = 76; // minimum 75

            this.dgDetail.Columns[(int)DetailColumn.PeriodSecs].Width = 5;
            this.dgDetail.Columns[(int)DetailColumn.PeriodSecs].HeaderText = "";
            this.dgDetail.Columns[(int)DetailColumn.PeriodSecs].Visible = false;

            this.dgDetail.Columns[(int)DetailColumn.AP].Width = 51; // minimum 50
            this.dgDetail.Columns[(int)DetailColumn.AP].HeaderText = CollectorMetricEnum.Instance.GetValue(CollectorMetricType.DetailAP);
            this.dgDetail.Columns[(int)DetailColumn.AP].Tag = CollectorMetricType.DetailAP;

            this.dgDetail.Columns[(int)DetailColumn.APmax].Width = 86; // minimum 85
            this.dgDetail.Columns[(int)DetailColumn.APmax].HeaderText = CollectorMetricEnum.Instance.GetValue(CollectorMetricType.DetailAPmax);
            this.dgDetail.Columns[(int)DetailColumn.APmax].Tag = CollectorMetricType.DetailAPmax;

            this.dgDetail.Columns[(int)DetailColumn.FTP].Width = 52; // minimum 50
            this.dgDetail.Columns[(int)DetailColumn.FTP].HeaderText = CollectorMetricEnum.Instance.GetValue(CollectorMetricType.DetailFTP);
            this.dgDetail.Columns[(int)DetailColumn.FTP].Tag = CollectorMetricType.DetailFTP;

            this.dgDetail.Columns[(int)DetailColumn.HR].Width = 55; // minimum 54
            this.dgDetail.Columns[(int)DetailColumn.HR].HeaderText = CollectorMetricEnum.Instance.GetValue(CollectorMetricType.DetailHR);
            this.dgDetail.Columns[(int)DetailColumn.HR].Tag = CollectorMetricType.DetailHR;

            this.dgDetail.Columns[(int)DetailColumn.Blank].Width = 5; // Five seems to be minimum size, even if set to zero
            this.dgDetail.Columns[(int)DetailColumn.Blank].HeaderText = "";
            this.dgDetail.Columns[(int)DetailColumn.Blank].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.dgDetail.Columns[(int)DetailColumn.AP_PowerDisplayType].Width = 5;
            this.dgDetail.Columns[(int)DetailColumn.AP_PowerDisplayType].HeaderText = "";
            this.dgDetail.Columns[(int)DetailColumn.AP_PowerDisplayType].Visible = false;

            this.dgDetail.Columns[(int)DetailColumn.APmax_PowerDisplayType].Width = 5;
            this.dgDetail.Columns[(int)DetailColumn.APmax_PowerDisplayType].HeaderText = "";
            this.dgDetail.Columns[(int)DetailColumn.APmax_PowerDisplayType].Visible = false;

            this.dgDetail.Columns[(int)DetailColumn.FTP_PowerDisplayType].Width = 5;
            this.dgDetail.Columns[(int)DetailColumn.FTP_PowerDisplayType].HeaderText = "";
            this.dgDetail.Columns[(int)DetailColumn.FTP_PowerDisplayType].Visible = false;

            foreach (DataGridViewColumn c in this.dgDetail.Columns)
            {
                c.MinimumWidth = c.Width;
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }


            // set in designer
            //this.dgDetail.RowHeadersVisible = false;

            // Set the row default cell font to the grid's default cell font.  Then clear the grid default so the row default is all that has to change. 
            this.dgDetail.RowsDefaultCellStyle.Font = this.dgDetail.DefaultCellStyle.Font;
            this.dgDetail.DefaultCellStyle.Font = null;

            // These must be set here not in designer otherwise column widths change. not sure why
            dgDetail.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // set in designer
            //this.dgDetail.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            //Debug.WriteLine($"ColumnHeadersHeight: {this.dgDetail.ColumnHeadersHeight}");

            this.dgDetail.ShowFocus = false;
            Debug.WriteLine($"InitializeDetailDataGrid2");
        }


        private void InitializeSummaryDataGrid()
        {
            // set in designer
            //dgSummary.ReadOnly = true;

            SummaryRow row = new()
            {
                //AP_PowerDisplayType = CurrentUserProfile.CollectorSummary.PowerValues[CollectorMetricType.SummaryAP].Key,
                //NP_PowerDisplayType = CurrentUserProfile.CollectorSummary.PowerValues[CollectorMetricType.SummaryNP].Key,
                //AS_SpeedDisplayType = CurrentUserProfile.CollectorSummary.SpeedValues[CollectorMetricType.SummaryAS].Key,
            };

            this.SummaryRows.Add(row);

            this.mMovingAverageManager.SummaryDataRow = row;

            this.SummaryBindingSource = new SyncBindingSource();
            this.SummaryBindingSource.DataSource = this.SummaryRows;
            this.dgSummary.DataSource = this.SummaryBindingSource;

            // A height of 19 is minimum when using Segoe UI 9pt font
            this.dgSummary.Rows[0].MinimumHeight = DataGridRowMinimumHeight;

            this.dgSummary.Columns[(int)SummaryColumn.AS].Width = 76;  // minimum 75
            this.dgSummary.Columns[(int)SummaryColumn.AS].HeaderText = SpeedDisplayEnum.Instance.GetValue(SpeedDisplayType.KilometersPerHour);
            this.dgSummary.Columns[(int)SummaryColumn.AS].Tag = CollectorMetricType.SummaryAS;

            this.dgSummary.Columns[(int)SummaryColumn.AP].Width = 51; // minimum 50
            this.dgSummary.Columns[(int)SummaryColumn.AP].HeaderText = CollectorMetricEnum.Instance.GetValue(CollectorMetricType.SummaryAP);
            this.dgSummary.Columns[(int)SummaryColumn.AP].Tag = CollectorMetricType.SummaryAP;

            this.dgSummary.Columns[(int)SummaryColumn.NP].Width = 86; // minimum 85
            this.dgSummary.Columns[(int)SummaryColumn.NP].HeaderText = CollectorMetricEnum.Instance.GetValue(CollectorMetricType.SummaryNP);
            this.dgSummary.Columns[(int)SummaryColumn.NP].Tag = CollectorMetricType.SummaryNP;

            this.dgSummary.Columns[(int)SummaryColumn.IF].Width = 52; // minimum 50
            this.dgSummary.Columns[(int)SummaryColumn.IF].HeaderText = CollectorMetricEnum.Instance.GetValue(CollectorMetricType.SummaryIF);
            this.dgSummary.Columns[(int)SummaryColumn.IF].Tag = CollectorMetricType.SummaryIF;

            this.dgSummary.Columns[(int)SummaryColumn.TSS].Width = 55; // minimum 54
            this.dgSummary.Columns[(int)SummaryColumn.TSS].HeaderText = CollectorMetricEnum.Instance.GetValue(CollectorMetricType.SummaryTSS);
            this.dgSummary.Columns[(int)SummaryColumn.TSS].Tag = CollectorMetricType.SummaryTSS;

            // Use the last blank column to fill the gap if user resizes
            this.dgSummary.Columns[(int)SummaryColumn.Blank].Width = 5; // Five seems to be minimum size, even if set to zero
            this.dgSummary.Columns[(int)SummaryColumn.Blank].HeaderText = "";
            this.dgSummary.Columns[(int)SummaryColumn.Blank].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            this.dgSummary.Columns[(int)SummaryColumn.AP_PowerDisplayType].Width = 5;
            this.dgSummary.Columns[(int)SummaryColumn.AP_PowerDisplayType].HeaderText = "";
            this.dgSummary.Columns[(int)SummaryColumn.AP_PowerDisplayType].Visible = false;

            this.dgSummary.Columns[(int)SummaryColumn.NP_PowerDisplayType].Width = 5;
            this.dgSummary.Columns[(int)SummaryColumn.NP_PowerDisplayType].HeaderText = "";
            this.dgSummary.Columns[(int)SummaryColumn.NP_PowerDisplayType].Visible = false;

            this.dgSummary.Columns[(int)SummaryColumn.AS_SpeedDisplayType].Width = 5;
            this.dgSummary.Columns[(int)SummaryColumn.AS_SpeedDisplayType].HeaderText = "";
            this.dgSummary.Columns[(int)SummaryColumn.AS_SpeedDisplayType].Visible = false;

            foreach (DataGridViewColumn c in this.dgSummary.Columns)
            {
                c.MinimumWidth = c.Width;
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // set in designer
            //this.dgSummary.RowHeadersVisible = false;

            // Set the row default cell font to the grid's default cell font.  Then clear the grid default so the row default is all that has to change. 
            this.dgSummary.RowsDefaultCellStyle.Font = this.dgSummary.DefaultCellStyle.Font;
            this.dgSummary.DefaultCellStyle.Font = null;

            // These must be set here not in designer otherwise column widths change. not sure why
            dgSummary.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgSummary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            // set in designer
            //this.dgSummary.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.False;

            this.dgSummary.ShowFocus = false;
        }

        protected void SetSummaryHeaderText(SummaryColumn columnIndex, string headerText)
        {
            this.dgSummary.Columns[(int)columnIndex].HeaderText = headerText;
        }

        private void dgDetail_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (e.ListChangedType == ListChangedType.Reset)
                Debug.WriteLine($"dgDetail_DataBindingComplete - ListChangedType: {e.ListChangedType}");

            //if (e.ListChangedType == ListChangedType.Reset)
            //    this.SetRowVisibilityStatus();

        }

        private void SetRowColumnVisibilityStatusForUser()
        {
            this.dgDetail.Columns[(int)DetailColumn.AP].Visible = CurrentUserProfile.CollectorMetrics[CollectorMetricType.DetailAP].IsVisible.Value;
            this.dgDetail.Columns[(int)DetailColumn.APmax].Visible = CurrentUserProfile.CollectorMetrics[CollectorMetricType.DetailAPmax].IsVisible.Value;
            this.dgDetail.Columns[(int)DetailColumn.FTP].Visible = CurrentUserProfile.CollectorMetrics[CollectorMetricType.DetailFTP].IsVisible.Value;
            this.dgDetail.Columns[(int)DetailColumn.HR].Visible = CurrentUserProfile.CollectorMetrics[CollectorMetricType.DetailHR].IsVisible.Value;

            this.dgSummary.Columns[(int)SummaryColumn.AP].Visible = CurrentUserProfile.CollectorMetrics[CollectorMetricType.SummaryAP].IsVisible.Value;
            this.dgSummary.Columns[(int)SummaryColumn.AS].Visible = CurrentUserProfile.CollectorMetrics[CollectorMetricType.SummaryAS].IsVisible.Value;
            this.dgSummary.Columns[(int)SummaryColumn.IF].Visible = CurrentUserProfile.CollectorMetrics[CollectorMetricType.SummaryIF].IsVisible.Value;
            this.dgSummary.Columns[(int)SummaryColumn.NP].Visible = CurrentUserProfile.CollectorMetrics[CollectorMetricType.SummaryNP].IsVisible.Value;
            this.dgSummary.Columns[(int)SummaryColumn.TSS].Visible = CurrentUserProfile.CollectorMetrics[CollectorMetricType.SummaryTSS].IsVisible.Value;

            SummaryRows[0].AP = "";
            SummaryRows[0].AS = "";
            SummaryRows[0].NP = "";
            SummaryRows[0].IF = "";
            SummaryRows[0].TSS = "";
            SummaryRows[0].AP_PowerDisplayType = CurrentUserProfile.CollectorSummary.PowerValues[CollectorMetricType.SummaryAP].Key;
            SummaryRows[0].NP_PowerDisplayType = CurrentUserProfile.CollectorSummary.PowerValues[CollectorMetricType.SummaryNP].Key;
            SummaryRows[0].AS_SpeedDisplayType = CurrentUserProfile.CollectorSummary.SpeedValues[CollectorMetricType.SummaryAS].Key;

            DetailBindingSource.SuspendBinding();
            dgDetail.CurrentCell = null;
            for (int rowIndex = 0; rowIndex < dgDetail.Rows.Count; rowIndex++)
            {
                DataGridViewRow r = dgDetail.Rows[rowIndex];
                DurationType durationType = (DurationType)dgDetail[(int)DetailColumn.PeriodSecs, rowIndex].Value;

                if (CurrentUserProfile.Collectors.ContainsKey(durationType))
                {
                    r.Visible = CurrentUserProfile.Collectors[durationType].IsVisible.Value;

                    // keep track of status
                    DetailRows[rowIndex].IsRowVisible = r.Visible;
                }

                DetailRows[rowIndex].AP = "";
                DetailRows[rowIndex].APmax = "";
                DetailRows[rowIndex].FTP = "";
                DetailRows[rowIndex].HR = "";

                DetailRows[rowIndex].AP_PowerDisplayType = CurrentUserProfile.Collectors[durationType].PowerValues[CollectorMetricType.DetailAP].Key;
                DetailRows[rowIndex].APmax_PowerDisplayType = CurrentUserProfile.Collectors[durationType].PowerValues[CollectorMetricType.DetailAPmax].Key;
                DetailRows[rowIndex].FTP_PowerDisplayType = CurrentUserProfile.Collectors[durationType].PowerValues[CollectorMetricType.DetailFTP].Key;
            }
            DetailBindingSource.ResumeBinding();
            dgDetail.CurrentCell = dgDetail.FirstDisplayedCell; // Needs to be set after ResumeBinding
        }


        //private void SetRowVisibilityStatus()
        //{
        //    Debug.WriteLine($"SetRowVisibilityStatus1");

        //    //int[] list = { 60, 300, 1200 };


        //    DetailBindingSource.SuspendBinding();
        //    dgDetail.CurrentCell = null;
        //    for (int i = 0; i < dgDetail.Rows.Count; i++)
        //    {
        //        DataGridViewRow r = dgDetail.Rows[i];
        //        DurationType durationType = (DurationType)r.Cells[(int)DetailColumn.PeriodSecs].Value;
                
        //        if (CurrentUserProfile.Collectors.ContainsKey(durationType))
        //        {
        //            r.Visible = CurrentUserProfile.Collectors[durationType].IsVisible.Value;

        //            // keep track of status
        //            DetailRows[i].IsRowVisible = r.Visible;
        //        }

        //        //determine whether to hide or show row
        //        //int? value = list.Cast<int?>().FirstOrDefault(n => n == (int)r.Cells[(int)DetailColumn.PeriodSecs].Value);
        //        //if (!value.HasValue)
        //        //    r.Visible = false;
        //        //Debug.WriteLine($"value: {value}, rowval: {(int)r.Cells[(int)DetailColumn.PeriodSecs].Value}");

        //    }
        //    DetailBindingSource.ResumeBinding();
        //    dgDetail.CurrentCell = dgDetail.FirstDisplayedCell; // Needs to be set after ResumeBinding

        //    Debug.WriteLine($"SetRowVisibilityStatus2");
        //}




        private void ViewControl_Resize(object sender, EventArgs e)
        {
            //Debug.WriteLine($"DetailSummaryViewControl_Resize1 - Size: {this.Size}");

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
           
            

            //Debug.WriteLine($"Row[0].Visible1: {dgDetail.Rows[0].Visible}, CurrentCell: {dgDetail.CurrentCell} BindingSource.Position: {this.DetailBindingSource.Position}");

            //DataTable table = (DataTable)((BindingSource)this.dgDetail.DataSource).DataSource;

            //dgDetail.CurrentCell = null;
            //for (int i = 0; i < table.Rows.Count; i++)
            //{
            //    DataRow r = table.Rows[i];
            //    r.SetField<string>((int)DetailColumn.AP, (Convert.ToInt32(r.Field<string>((int)DetailColumn.AP)) + 1).ToString());
            //}

            //Debug.WriteLine($"Row[0].Visible2: {dgDetail.Rows[0].Visible}");
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
            ContextMenuStrip menuStrip = new ContextMenuStrip();

            if (dataGridView == this.dgDetail)
            {
                Dictionary<int, int> powerDisplayColumnMap = new();
                powerDisplayColumnMap.Add((int)DetailColumn.AP, (int)DetailColumn.AP_PowerDisplayType);
                powerDisplayColumnMap.Add((int)DetailColumn.APmax, (int)DetailColumn.APmax_PowerDisplayType);
                powerDisplayColumnMap.Add((int)DetailColumn.FTP, (int)DetailColumn.FTP_PowerDisplayType);

                switch (e.ColumnIndex)
                {
                    case (int)DetailColumn.Period:
                        for (int rowIndex = 0; rowIndex < dataGridView.Rows.Count; rowIndex++)
                        {
                            DataGridViewRow r = dataGridView.Rows[rowIndex];

                            string period = (string)dataGridView[(int)DetailColumn.Period, rowIndex].Value;
                            int periodSecs = (int)dataGridView[(int)DetailColumn.PeriodSecs, rowIndex].Value;

                            var mi = new ToolStripMenuItem(period)
                            {
                                CheckOnClick = true,
                                Tag = new object[] { rowIndex, periodSecs },
                                Checked = r.Visible,
                            };
                            mi.CheckedChanged += this.periodContextMenu_CheckChanged;
                            menuStrip.Items.Add(mi);
                        }

                        menuStrip.Show(Cursor.Position);
                        //menuStrip.Items[e.RowIndex].Select(); // position the highlighted cursor on the item matching the row selected
                        break;

                    case (int)DetailColumn.AP:
                    case (int)DetailColumn.APmax:
                    case (int)DetailColumn.FTP:
                        // map the right-clicked column to the column that stores the type of power display
                        int powerDisplayColumnIndex = powerDisplayColumnMap[e.ColumnIndex];

                        // the durationType is stored in the row
                        int durationType = (int)dataGridView[(int)DetailColumn.PeriodSecs, e.RowIndex].Value;

                        // the CollectorMetricType is stored in the column header tag
                        int metricType = (int)dataGridView.Columns[e.ColumnIndex].Tag;

                        foreach (var kvp in PowerDisplayEnum.Instance.GetItems())
                        {
                            var mi = new ToolStripMenuItem(kvp.Value)
                            {
                                CheckOnClick = true,
                                Tag = new object[] { e.RowIndex, powerDisplayColumnIndex, (int)kvp.Key, metricType, durationType, dataGridView }, // pass required values to the handler event
                                Checked = (PowerDisplayType)dataGridView[powerDisplayColumnIndex, e.RowIndex].Value == kvp.Key,
                            };
                            mi.CheckedChanged += powerContextMenu_CheckChanged;
                            menuStrip.Items.Add(mi);
                        }

                        menuStrip.Show(Cursor.Position);
                        break;
                }
            }
            else if (dataGridView == this.dgSummary)
            {
                Dictionary<int, int> powerDisplayColumnMap = new();
                powerDisplayColumnMap.Add((int)SummaryColumn.AP, (int)SummaryColumn.AP_PowerDisplayType);
                powerDisplayColumnMap.Add((int)SummaryColumn.NP, (int)SummaryColumn.NP_PowerDisplayType);

                Dictionary<int, int> speedDisplayColumnMap = new();
                speedDisplayColumnMap.Add((int)SummaryColumn.AS, (int)SummaryColumn.AS_SpeedDisplayType);

                switch (e.ColumnIndex)
                {
                    case (int)SummaryColumn.AS:
                        // map the right-clicked column to the column that stores the type of speed display
                        int speedDisplayColumnIndex = speedDisplayColumnMap[e.ColumnIndex];

                        foreach (var kvp in SpeedDisplayEnum.Instance.GetItems())
                        {
                            var mi = new ToolStripMenuItem(kvp.Value)
                            {
                                CheckOnClick = true,
                                Tag = new object[] { e.RowIndex, speedDisplayColumnIndex, (int)kvp.Key, dataGridView }, // pass required values to the handler event
                                Checked = (SpeedDisplayType)dataGridView[speedDisplayColumnIndex, e.RowIndex].Value == kvp.Key,
                            };
                            mi.CheckedChanged += speedContextMenu_CheckChanged;
                            menuStrip.Items.Add(mi);
                        }

                        menuStrip.Show(Cursor.Position);
                        break;

                    case (int)SummaryColumn.AP:
                    case (int)SummaryColumn.NP:
                        // map the right-clicked column to the column that stores the type of power display
                        int powerDisplayColumnIndex = powerDisplayColumnMap[e.ColumnIndex];

                        foreach (var kvp in PowerDisplayEnum.Instance.GetItems())
                        {
                            var mi = new ToolStripMenuItem(kvp.Value)
                            {
                                CheckOnClick = true,
                                Tag = new object[] { e.RowIndex, powerDisplayColumnIndex, (int)kvp.Key, dataGridView }, // pass required values to the handler event
                                Checked = (PowerDisplayType)dataGridView[powerDisplayColumnIndex, e.RowIndex].Value == kvp.Key,
                            };
                            mi.CheckedChanged += powerContextMenu_CheckChanged;
                            menuStrip.Items.Add(mi);
                        }

                        menuStrip.Show(Cursor.Position);
                        break;

                }

            }

        }

        /// <summary>
        /// Hide / Show Collection period rows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void periodContextMenu_CheckChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            object[] tag = item.Tag as object[];
            int rowIndex = (int)tag[0];
            DurationType durationType = (DurationType)tag[1];

            if (item.Checked || dgDetail.Rows.GetRowCount(DataGridViewElementStates.Visible) > 1)
            {
                dgDetail.CurrentCell = null;
                dgDetail.Rows[rowIndex].Visible = item.Checked;

                // Place the CurrentCell on a valid (visible) cell
                // Failing to do this will make the row reappear when it is next updated.  (BindingSource.Position being on a hidden row is a bad thing).
                dgDetail.CurrentCell = dgDetail.FirstDisplayedCell;

                // keep track of status
                DetailRows[rowIndex].IsRowVisible = item.Checked;

                if (CurrentUserProfile.Collectors.ContainsKey(durationType))
                {
                    ZAMsettings.BeginCachedConfiguration();
                    CurrentUserProfile.Collectors[durationType].IsVisible = item.Checked;
                    ZAMsettings.CommitCachedConfiguration();
                }

            }
        }

        private void powerContextMenu_CheckChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            if (item.Checked)
            {
                object[] tag = item.Tag as object[];
                int rowIndex = (int)tag[0], powerDisplayColumnIndex = (int)tag[1];
                PowerDisplayType powerDisplayType = (PowerDisplayType)tag[2];
                CollectorMetricType metricType = (CollectorMetricType)tag[3];
                DurationType durationType = (DurationType)tag[4];
                DataGridView dataGridView = (DataGridView)tag[5];

                // set the PowerDisplayType in the dataGridView.
                dataGridView[powerDisplayColumnIndex, rowIndex].Value = powerDisplayType;

                // Update configuration
                if (CurrentUserProfile.Collectors.ContainsKey(durationType))
                {
                    if (CurrentUserProfile.Collectors[durationType].PowerValues.ContainsKey(metricType))
                    {
                        ZAMsettings.BeginCachedConfiguration();
                        CurrentUserProfile.Collectors[durationType].PowerValues[metricType] = PowerDisplayEnum.Instance.GetItem(powerDisplayType);
                        ZAMsettings.CommitCachedConfiguration();
                    }
                }
            }
        }


        private void speedContextMenu_CheckChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            if (item.Checked)
            {
                object[] tag = item.Tag as object[];
                int rowIndex = (int)tag[0], speedDisplayColumnIndex = (int)tag[1], speedDisplayType = (int)tag[2];
                DataGridView dataGridView = (DataGridView)tag[3];

                dataGridView[speedDisplayColumnIndex, rowIndex].Value = (SpeedDisplayType)speedDisplayType;
            }
        }

        /// <summary>
        /// Handle mouse click on column headers and allow visibility change
        /// </summary>
        /// <param name="dataGridView"></param>
        /// <param name="e"></param>
        private void dataGridView_ColumnHeaderMouseClick(DataGridView dataGridView, DataGridViewCellMouseEventArgs e)
        {
            ContextMenuStrip menuStrip = new ContextMenuStrip();

            //Add the columns to context menu strip
            foreach (DataGridViewColumn c in dataGridView.Columns)
            {
                if (c.HeaderText != "") // exclude filler column
                {
                    var mi = new ToolStripMenuItem(c.HeaderText)
                    {
                        Tag = new object[] { c.Index, c.Tag, dataGridView },
                        Checked = c.Visible,
                        CheckOnClick = true,
                        Enabled = (dataGridView != dgDetail || c.Index != (int)DetailColumn.Period),
                    };
                    mi.CheckedChanged += headerContextMenu_CheckedChanged;
                    menuStrip.Items.Add(mi);


                    //Handle CheckStateChanged event of context menu strip items
                    //mi.CheckStateChanged += (obj, args) =>
                    //{
                    //    dataGridView.Columns.GetColumnCount(DataGridViewElementStates.Visible);

                    //    // Determine how many columns are currently visible
                    //    int visibleCount = 0;
                    //    for (int j = 0; j < dataGridView.Columns.Count; j++)
                    //        if (dataGridView.Columns[j].Visible && dataGridView.Columns[j].HeaderText != "")
                    //            visibleCount++;

                    //    ToolStripMenuItem item = (ToolStripMenuItem)obj;
                    //    string columnName = (string)item.Tag;
                    //    int columnIndex = dataGridView.Columns[columnName].Index;

                    //    //Debug.WriteLine($"{item.GetCurrentParent().GetType()}");

                    //    // Don't allow removing of every column (empty grid)
                    //    if (item.Checked || visibleCount > 1)
                    //    {
                    //        dataGridView.Columns[columnName].Visible = item.Checked;
                    //    }

                    //    // Trigger a resize so that dgSummary can size itself appropriately
                    //    this.OnResize(new EventArgs());
                    //};
                }
            }

            if (menuStrip.Items.Count > 0)
            {
                menuStrip.Show(Cursor.Position);
            }
        }

        private void headerContextMenu_CheckedChanged(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            int visibleCount = 0;

            object[] tag = item.Tag as object[];
            int colIndex = (int)tag[0];
            CollectorMetricType metricType = (CollectorMetricType)tag[1];
            DataGridView dataGridView = (DataGridView)tag[2];

            foreach (DataGridViewColumn c in dataGridView.Columns)
                if (c.Tag != null && c.Visible)
                    visibleCount++;

            if (item.Checked || visibleCount > 1)
            {
                dataGridView.Columns[colIndex].Visible = item.Checked;

                if (CurrentUserProfile.CollectorMetrics.ContainsKey(metricType))
                {
                    ZAMsettings.BeginCachedConfiguration();
                    CurrentUserProfile.CollectorMetrics[metricType].IsVisible = item.Checked;
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
