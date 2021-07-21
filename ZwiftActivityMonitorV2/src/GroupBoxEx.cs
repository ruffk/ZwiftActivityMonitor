using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Diagnostics;

namespace ZwiftActivityMonitorV2
{
    public class GroupBoxEx : GroupBox
    {
        private Color borderColor = SystemColors.ControlLight;
        [DefaultValue(typeof(SystemColors), "ControlLight")]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; this.Invalidate(); }
        }

        private Color textColor = Color.Empty;
        /// <summary>
        /// A separate TextColor property allows the text color of the groupbox to not interfere with the ForeColor of the controls it contains.
        /// </summary>
        [DefaultValue(typeof(Color), "Empty")]
        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; this.Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            GroupBoxState state = base.Enabled ? GroupBoxState.Normal :
                GroupBoxState.Disabled;
            
            TextFormatFlags flags = TextFormatFlags.PreserveGraphicsTranslateTransform |
                TextFormatFlags.PreserveGraphicsClipping | TextFormatFlags.TextBoxControl |
                TextFormatFlags.WordBreak;

            Color titleColor = this.TextColor == Color.Empty ? this.ForeColor : this.TextColor;

            //Debug.WriteLine($"titleColor {titleColor}");
            
            if (!this.ShowKeyboardCues)
                flags |= TextFormatFlags.HidePrefix;
    
            if (this.RightToLeft == RightToLeft.Yes)
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            
            if (!this.Enabled)
                titleColor = SystemColors.GrayText;
      
            DrawUnthemedGroupBoxWithText(e.Graphics, new Rectangle(0, 0, base.Width,
                base.Height), this.Text, this.Font, titleColor, flags, state);
    
            RaisePaintEvent(this, e);
        }

        private void DrawUnthemedGroupBoxWithText(Graphics g, Rectangle bounds,
            string groupBoxText, Font font, Color titleColor,
            TextFormatFlags flags, GroupBoxState state)
        {
            Rectangle rectangle = bounds;
            rectangle.Width -= 8;
            Size size = TextRenderer.MeasureText(g, groupBoxText, font,
                new Size(rectangle.Width, rectangle.Height), flags);
            rectangle.Width = size.Width;
            rectangle.Height = size.Height;
            if ((flags & TextFormatFlags.Right) == TextFormatFlags.Right)
                rectangle.X = (bounds.Right - rectangle.Width) - 8;
            else
                rectangle.X += 8;
            TextRenderer.DrawText(g, groupBoxText, font, rectangle, titleColor, flags);
            if (rectangle.Width > 0)
                rectangle.Inflate(2, 0);
            using (var pen = new Pen(this.BorderColor))
            {
                int num = bounds.Top + (font.Height / 2);
                g.DrawLine(pen, bounds.Left, num - 1, bounds.Left, bounds.Height - 2);
                g.DrawLine(pen, bounds.Left, bounds.Height - 2, bounds.Width - 1,
                    bounds.Height - 2);
                g.DrawLine(pen, bounds.Left, num - 1, rectangle.X - 3, num - 1);
                g.DrawLine(pen, rectangle.X + rectangle.Width + 2, num - 1,
                    bounds.Width - 2, num - 1);
                g.DrawLine(pen, bounds.Width - 2, num - 1, bounds.Width - 2,
                   bounds.Height - 2);
            }
        }
    }
}
