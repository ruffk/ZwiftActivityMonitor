using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace ZwiftActivityMonitorV2
{
    public class ViewerUserControlEx : UserControl
    {
        public Color HeaderGradientBeginColor { get; set; } = SystemColors.Control;
        public Color HeaderGradientEndColor { get; set; } = SystemColors.ControlDark;

        // A height of 19 is minimum when using Segoe UI 9pt font
        protected const int DataGridRowMinimumHeight = 19;

        private Color m_headerForeColor;
        private Color m_rowBackColor;
        private Color m_rowForeColor;
        private Font m_rowFont;

        public Color HeaderForeColor
        {
            get
            {
                return m_headerForeColor;
            }
            set
            {
                // change the text color on the data grid headers
                m_headerForeColor = value;

                if (value != Color.Empty)
                    this.HeaderForeColorChanged(); // value will be empty on initialization so don't invoke changed method
            }
        }

        protected virtual void HeaderForeColorChanged()
        {

        }

        public Font RowFont
        {
            get
            {
                return m_rowFont;
            }
            set
            {
                // change the font on the data grid rows
                m_rowFont = value;

                if (value != null)
                    this.RowFontChanged(); // value will be empty on initialization so don't invoke changed method
            }
        }

        protected virtual void RowFontChanged()
        {

        }

        public Color RowBackColor
        {
            get
            {
                return m_rowBackColor;
            }
            set
            {
                // change the back color on the data grid rows
                m_rowBackColor = value;

                if (value != Color.Empty)
                    this.RowBackColorChanged(); // value will be empty on initialization so don't invoke changed method
            }
        }

        protected virtual void RowBackColorChanged()
        {

        }

        public Color RowForeColor
        {
            get
            {
                return m_rowForeColor;
            }
            set
            {
                // change the fore color on the data grid rows
                m_rowForeColor = value;
                
                if (value != Color.Empty)
                    this.RowForeColorChanged(); // value will be empty on initialization so don't invoke changed method
            }
        }

        protected virtual void RowForeColorChanged()
        {

        }

        protected void dataGridViewGradientHeader_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                LinearGradientBrush br = new LinearGradientBrush(e.CellBounds, this.HeaderGradientBeginColor, this.HeaderGradientEndColor, 90, true);

                e.Graphics.FillRectangle(br, e.CellBounds);

                // draw the 3d header
                // 
                Rectangle r = e.CellBounds;

                r.Width -= 1;
                r.Height -= 1;

                // draw the dark border around the whole thing 
                // 
                e.Graphics.DrawRectangle(SystemPens.ControlDarkDark, r);

                r.Width -= 1;
                r.Height -= 1;

                // draw the light 3D border 
                //
                e.Graphics.DrawLine(SystemPens.ControlLightLight, r.X, r.Y, r.Right, r.Y);
                e.Graphics.DrawLine(SystemPens.ControlLightLight, r.X, r.Y, r.X, r.Bottom);

                // draw the dark 3D Border 
                //
                e.Graphics.DrawLine(SystemPens.ControlDark, r.X + 1, r.Bottom, r.Right, r.Bottom);
                e.Graphics.DrawLine(SystemPens.ControlDark, r.Right, r.Y + 1, r.Right, r.Bottom);

                e.PaintContent(e.ClipBounds);
                e.Handled = true;
            }
        }

        public virtual void ControlLosingFocus(object sender, Syncfusion.Windows.Forms.Tools.SelectedIndexChangingEventArgs e)
        {
            //Debug.WriteLine($"ControlLosingFocus - {this.Name}");
        }
        public virtual void ControlGainingFocus(object sender, EventArgs e)
        {
            //Debug.WriteLine($"ControlGainingFocus - {this.Name}");
        }

        protected UserProfile CurrentUserProfile
        {
            get { return ZAMsettings.Settings.CurrentUser; }
        }

    }
}
