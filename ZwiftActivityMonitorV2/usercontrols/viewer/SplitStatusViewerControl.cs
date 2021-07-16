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
    public partial class SplitStatusViewerControl : StatusViewerControlEx
    {
        //private Dispatcher UIdispatcher;
        private readonly ILogger<SplitStatusViewerControl> Logger;
        private SplitEventArgs mSplitEventArgs;

        public SplitStatusViewerControl(SplitEventArgs splitEventArgs)
        {
            InitializeComponent();

            if (DesignMode)
                return;

            if (ZAMsettings.LoggerFactory == null)
                return;

            Logger = ZAMsettings.LoggerFactory.CreateLogger<SplitStatusViewerControl>();

            this.mSplitEventArgs = splitEventArgs;
            
            //DeltaTime;
            //SplitNumber;
            //SplitSpeedKph;
            //SplitSpeedMph;
            //SplitTime;
            //TotalKmTravelled;
            //TotalMiTravelled;
            //TotalTime;

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

            string splitStatus = Properties.Resources.SplitStatusFreeform;
            string delta = "";

            if (this.mSplitEventArgs.DeltaTime.HasValue)
            {
                TimeSpan std = (TimeSpan)this.mSplitEventArgs.DeltaTime;
                bool negated = false;

                if (std.TotalSeconds < 0)
                {
                    std = std.Negate();
                    negated = true;
                }

                delta = $"{(negated ? "-" : "+")}{(std.Minutes > 0 ? std.ToString("m'@QT's'\"'").Replace("@QT", "\'") : std.ToString("s'\"'"))}";

                splitStatus = splitStatus.Replace("DeltaColorCode", this.mSplitEventArgs.AheadOfGoalTime.Value ? "00C000" : "C00000");
            }
            else
            {
                splitStatus = splitStatus.Replace("DeltaColorCode", "00C000");
                //splitStatus = splitStatus.Replace("DeltaColorCode", $"{colorTable.FormBackground.R:X2}{ colorTable.FormBackground.G:X2}{ colorTable.FormBackground.B:X2}");
                delta = "No Goal";
            }

            splitStatus = splitStatus.Replace("DeltaTime", delta);
            splitStatus = splitStatus.Replace("SplitNumber", this.mSplitEventArgs.SplitNumber.ToString());
            splitStatus = splitStatus.Replace("SplitSpeedKph", this.mSplitEventArgs.SplitSpeedKph.ToString("0.0"));
            splitStatus = splitStatus.Replace("SplitSpeedMph", this.mSplitEventArgs.SplitSpeedMph.ToString("0.0"));
            splitStatus = splitStatus.Replace("SplitTime", this.mSplitEventArgs.SplitTime.ToString("hh\\:mm\\:ss"));
            splitStatus = splitStatus.Replace("TotalKmTravelled", this.mSplitEventArgs.TotalKmTravelled.ToString("0.0"));
            splitStatus = splitStatus.Replace("TotalMiTravelled", this.mSplitEventArgs.TotalMiTravelled.ToString("0.0"));
            splitStatus = splitStatus.Replace("TotalTime", this.mSplitEventArgs.TotalTime.ToString("hh\\:mm\\:ss"));

            Logger.LogDebug($"{this.GetType()}::CreateDocumentText - \n{splitStatus}");
            this.DocumentText = styleSheet + splitStatus;
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
