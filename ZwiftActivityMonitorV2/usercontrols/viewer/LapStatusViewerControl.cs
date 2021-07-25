using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace ZwiftActivityMonitorV2
{
    public partial class LapStatusViewerControl : StatusViewerControlEx
    {
        //private Dispatcher UIdispatcher;
        private readonly ILogger<LapStatusViewerControl> Logger;
        private LapEventArgs mLapEventArgs;

        public LapStatusViewerControl(LapEventArgs lapEventArgs)
        {
            InitializeComponent();

            if (DesignMode)
                return;

            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<LapStatusViewerControl>();

            this.mLapEventArgs = lapEventArgs;
        }

        public void CreateDocumentText()
        {
            /*
                #000001 BackColor
                #000002 ActiveFormBorderColor
                #000003 ForeColor
                #000004 ActiveTitleGradientEnd
            */
            MSoffice2010ColorManager colorTable = ZAMappearance.GetColorTable();

            string styleSheet = Properties.Resources.StyleSheet;

            styleSheet = styleSheet.Replace("000001", $"{colorTable.FormBackground.R:X2}{ colorTable.FormBackground.G:X2}{ colorTable.FormBackground.B:X2}");
            styleSheet = styleSheet.Replace("000002", $"{colorTable.ActiveFormBorderColor.R:X2}{ colorTable.ActiveFormBorderColor.G:X2}{ colorTable.ActiveFormBorderColor.B:X2}");
            styleSheet = styleSheet.Replace("000003", $"{colorTable.FormTextColor.R:X2}{ colorTable.FormTextColor.G:X2}{ colorTable.FormTextColor.B:X2}");
            styleSheet = styleSheet.Replace("000004", $"{colorTable.ActiveTitleGradientEnd.R:X2}{ colorTable.ActiveTitleGradientEnd.G:X2}{ colorTable.ActiveTitleGradientEnd.B:X2}");

            string lapStatus = Properties.Resources.LapStatusFreeform;


            lapStatus = lapStatus.Replace("LapAPwattsPerKg", this.mLapEventArgs.LapAPwattsPerKg.HasValue ? this.mLapEventArgs.LapAPwattsPerKg.Value.ToString("0.00") + "w/kg" : "");
            lapStatus = lapStatus.Replace("LapAPwatts", this.mLapEventArgs.LapAPwatts.ToString());
            lapStatus = lapStatus.Replace("LapDistanceKm", this.mLapEventArgs.LapDistanceKm.ToString("0.0"));
            lapStatus = lapStatus.Replace("LapDistanceMi", this.mLapEventArgs.LapDistanceMi.ToString("0.0"));
            lapStatus = lapStatus.Replace("LapNumber", this.mLapEventArgs.LapNumber.ToString());
            lapStatus = lapStatus.Replace("LapSpeedKph", this.mLapEventArgs.LapSpeedKph.ToString("0.0"));
            lapStatus = lapStatus.Replace("LapSpeedMph", this.mLapEventArgs.LapSpeedMph.ToString("0.0"));
            lapStatus = lapStatus.Replace("LapTime", this.mLapEventArgs.LapTime.ToString("hh\\:mm\\:ss"));
            lapStatus = lapStatus.Replace("TotalTime", this.mLapEventArgs.TotalTime.ToString("hh\\:mm\\:ss"));

            lapStatus = lapStatus.Replace("LapPower", $"{this.mLapEventArgs.LapAPwatts + " w"}{(this.mLapEventArgs.LapAPwattsPerKg.HasValue ? " (" + this.mLapEventArgs.LapAPwattsPerKg.Value.ToString("0.00") + " w/kg)" : "")}");
            lapStatus = lapStatus.Replace("LapDistance", $"{this.mLapEventArgs.LapDistanceKm.ToString("0.0") + " km"}{" (" + this.mLapEventArgs.LapDistanceMi.ToString("0.0") + " mi)"}");
            lapStatus = lapStatus.Replace("LapSpeed", $"{this.mLapEventArgs.LapSpeedKph.ToString("0.0") + " km/h"}{" (" + this.mLapEventArgs.LapSpeedMph.ToString("0.0") + " mi/h)"}");

            Logger.LogDebug($"{this.GetType()}::CreateDocumentText - \n{lapStatus}");
            this.DocumentText = styleSheet + lapStatus;
        }

        public override void InitializeStatus(Form form)
        {
            Logger.LogDebug($"{this.GetType()}::InitializeStatus");

            base.InitializeStatus(form);

            // populate browser control
            this.CreateDocumentText();
        }

    }
}
