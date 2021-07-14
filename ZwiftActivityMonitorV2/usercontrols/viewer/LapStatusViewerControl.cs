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
        private Dispatcher UIdispatcher;
        private readonly ILogger<LapStatusViewerControl> Logger;

        public LapStatusViewerControl()
        {
            InitializeComponent();

            if (DesignMode)
                return;

            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<LapStatusViewerControl>();
        }

        private void LapStatusViewerControl_Load(object sender, EventArgs e)
        {
            if (this.DesignMode)
                return;

            Logger.LogDebug($"{this.GetType()}::ViewControl_Load");

            // for handling UI events
            this.UIdispatcher = Dispatcher.CurrentDispatcher;
        }

        public void CreateDocumentText(LapEventArgs e)
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

            string lapStatus = Properties.Resources.LapStatus;

            lapStatus = lapStatus.Replace("LapAPwattsPerKg", e.LapAPwattsPerKg.HasValue ? e.LapAPwattsPerKg.Value.ToString("0.00") : "");
            lapStatus = lapStatus.Replace("LapAPwatts", e.LapAPwatts.ToString());
            lapStatus = lapStatus.Replace("LapDistanceKm", e.LapDistanceKm.ToString("0.0"));
            lapStatus = lapStatus.Replace("LapDistanceMi", e.LapDistanceMi.ToString("0.0"));
            lapStatus = lapStatus.Replace("LapNumber", e.LapNumber.ToString());
            lapStatus = lapStatus.Replace("LapSpeedKph", e.LapSpeedKph.ToString("0.0"));
            lapStatus = lapStatus.Replace("LapSpeedMph", e.LapSpeedMph.ToString("0.0"));
            lapStatus = lapStatus.Replace("LapTime", e.LapTime.ToString());
            lapStatus = lapStatus.Replace("TotalTime", e.TotalTime.ToString());

            Logger.LogDebug($"{this.GetType()}::CreateDocumentText - \n{lapStatus}");
            this.DocumentText = styleSheet + lapStatus;
        }
    }
}
