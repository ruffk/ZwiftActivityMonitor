using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;


namespace ZwiftActivityMonitorV2
{
    public class FontComboBox : ComboBox
    {
        public FontComboBox() : base()
        {
            this.DrawMode = DrawMode.OwnerDrawVariable;
            this.DrawItem += this.FontComboBox_DrawItem;
            this.SelectionChangeCommitted += this.FontComboBox_SelectionChangeCommitted;
            this.MeasureItem += this.FontComboBox_MeasureItem;
        }

        public void Fill()
        {
            List<string> ignoreList = new();

            ignoreList.Add("Bookshelf Symbol 7");
            ignoreList.Add("Consolas");
            ignoreList.Add("Courier New");
            ignoreList.Add("HoloLens MDL2 Assets");
            ignoreList.Add("Lucida Console");
            ignoreList.Add("Lucida Sans Typewriter");
            ignoreList.Add("Marlett");
            ignoreList.Add("MingLiU-ExtB");
            ignoreList.Add("MingLiU_HKSCS-ExtB");
            ignoreList.Add("MS Gothic");
            ignoreList.Add("MS Outlook");
            ignoreList.Add("MS Reference Specialty");
            ignoreList.Add("MT Extra");
            ignoreList.Add("NSimSun");
            ignoreList.Add("OCR A Extended");
            ignoreList.Add("OCR B MT");
            ignoreList.Add("OCR-A II");
            ignoreList.Add("QuickType II Mono");
            ignoreList.Add("QuickType II Pi");
            ignoreList.Add("Segoe MDL2 Assets");
            ignoreList.Add("SimSun");
            ignoreList.Add("SimSun-ExtB");
            ignoreList.Add("Symbol");
            ignoreList.Add("Viner Hand ITC");
            ignoreList.Add("Vivaldi");
            ignoreList.Add("Vladimir Script");
            ignoreList.Add("Webdings");
            ignoreList.Add("Wingdings");
            ignoreList.Add("Wingdings 2");
            ignoreList.Add("Wingdings 3");

            InstalledFontCollection ifc = new InstalledFontCollection();

            BeginUpdate();

            this.Items.Clear();

            foreach (FontFamily family in ifc.Families)
            {
                if (!family.IsStyleAvailable(FontStyle.Regular) || ignoreList.Contains(family.Name))
                    continue;

                this.Items.Add(family.Name);
            }

            EndUpdate();
        }

        private void FontComboBox_MeasureItem(object sender,
              System.Windows.Forms.MeasureItemEventArgs e)
        {
            if (e.Index == -1)
                return;

            string fontFamily = (string)this.Items[e.Index];

            using (Font f = new Font(fontFamily, this.Font.Size, FontStyle.Regular))
            {
                using (Graphics gr = this.CreateGraphics())
                {
                    e.ItemWidth = (int)gr.MeasureString(fontFamily, f).Width;
                    e.ItemHeight = (int)gr.MeasureString(fontFamily, f).Height;
                }
            }
        }

        private void FontComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.Font = new Font((string)this.SelectedItem, this.Font.Size, FontStyle.Regular);
            //Logger.LogDebug($"FontComboBox_SelectionChangeCommitted {(string)this.SelectedItem}");
        }

        public Color DropDownBorderColor { get; set; } = SystemColors.ActiveBorder;

        public Color DropDownBackColor { get; set; } = SystemColors.Window;

        private void FontComboBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            if ((e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit)
                return;

            // Draw the background of the item
            if (
                ((e.State & DrawItemState.Focus) == DrawItemState.Focus) ||
                ((e.State & DrawItemState.Selected) == DrawItemState.Selected) ||
                ((e.State & DrawItemState.HotLight) == DrawItemState.HotLight)
               )
            {
                e.DrawBackground();
            }
            else
            {
                using (Brush backgroundBrush = new SolidBrush(DropDownBackColor))
                {
                    e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
                }
            }

            using (Font f = new Font(Items[e.Index].ToString(), this.Font.Size, FontStyle.Regular))
            {
                // Draw item text
                e.Graphics.DrawString(Items[e.Index].ToString(), f, Brushes.Black, new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height));
            }

            // Draw the focus rectangle if the mouse hovers over an item
            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                e.DrawFocusRectangle();

            //Draw the border around the whole DropDown area
            using (Pen borderPen = new Pen(DropDownBorderColor, 1))
            {
                Point start;
                Point end;

                if (e.Index == 0)
                {
                    //Draw top border
                    start = new Point(e.Bounds.Left, e.Bounds.Top);
                    end = new Point(e.Bounds.Left + e.Bounds.Width - 1, e.Bounds.Top);
                    e.Graphics.DrawLine(borderPen, start, end);
                }

                //Draw left border
                start = new Point(e.Bounds.Left, e.Bounds.Top);
                end = new Point(e.Bounds.Left, e.Bounds.Top + e.Bounds.Height - 1);
                e.Graphics.DrawLine(borderPen, start, end);

                //Draw Right border
                start = new Point(e.Bounds.Left + e.Bounds.Width - 1, e.Bounds.Top);
                end = new Point(e.Bounds.Left + e.Bounds.Width - 1,
                                e.Bounds.Top + e.Bounds.Height - 1);
                e.Graphics.DrawLine(borderPen, start, end);

                if (e.Index == Items.Count - 1)
                {
                    //Draw bottom border
                    start = new Point(e.Bounds.Left, e.Bounds.Top + e.Bounds.Height - 1);
                    end = new Point(e.Bounds.Left + e.Bounds.Width - 1,
                                    e.Bounds.Top + e.Bounds.Height - 1);
                    e.Graphics.DrawLine(borderPen, start, end);
                }
            }
        }
    }
}
