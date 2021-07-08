using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Syncfusion.Windows.Forms;

using System.Drawing.Drawing2D;
using System.Linq;

namespace ZwiftActivityMonitorV2
{
    #region ZAMColorScheme

    public class ZAMappearance : ConfigItemBase
    {
        // FYI - The setters here should just be "internal set" but then the json deserializer doesn't work properly.
        public KeyValuePair<ThemeType, string>? Theme { get; set; }
        public KeyValuePair<TransparencyType, string>? Transparency { get; set; }

        public Color ManagedColor { get; set; } = SystemColors.Control;
        public Size WindowSize { get; set; } = new Size(359, 208);
        public string FontFamily { get; set; } = "Franklin Gothic Demi Cond";
        public float FontSize { get; set; } = 13;
        public bool IsFontBold { get; set; } = false;
        public bool IsFontItalic { get; set; } = false;

        //private readonly Dictionary<ThemeType, KeyValuePair<ThemeType, string>> m_themeList = new();
        private readonly Dictionary<ThemeType, string> m_themeList = new();
        private readonly Dictionary<TransparencyType, string> m_transparencyList = new();

        public static readonly Color ZwiftyOrange = Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(108)))), ((int)(((byte)(61))))); // Zwifty Orange

        public ZAMappearance()
        {
            // ComboBox will display these items
            m_themeList.Add(ThemeType.Custom,           "Custom");
            m_themeList.Add(ThemeType.Office2010Black,  "Office Black");
            m_themeList.Add(ThemeType.Office2010Blue,   "Office Blue");
            m_themeList.Add(ThemeType.Office2010Silver, "Office Silver");
            m_themeList.Add(ThemeType.ZwiftyOrange,     "Zwifty Orange");

            // ComboBox will display these items
            m_transparencyList.Add(TransparencyType.NotTransparent, "Not Transparent");
            m_transparencyList.Add(TransparencyType.TransparentBlackText, "TP w/Black Text");
            m_transparencyList.Add(TransparencyType.TransparentWhiteText, "TP w/White Text");
        }

        public override int InitializeDefaultValues()
        {
            int count = 0;

            // The KeyValuePair classes need to be initialized with defaults here as they depend on values in the lists.
            // FYI: They can't be initialized in the constructor as they will always show null during json deserialization,
            // even if the json being parsed has values in it.

            if (Theme == null)
            {
                //Logger.LogDebug($"Initializing Theme");
                Theme = new KeyValuePair<ThemeType, string>(ThemeType.ZwiftyOrange, m_themeList[ThemeType.ZwiftyOrange]); // default
                count++;
            }

            if (Transparency == null)
            {
                //Logger.LogDebug($"Initializing Transparency");
                Transparency = new KeyValuePair<TransparencyType, string>(TransparencyType.TransparentWhiteText, m_transparencyList[TransparencyType.TransparentWhiteText]); // default
                count++;
            }

            return count;
        }

        /// <summary>
        /// ComboBox items
        /// </summary>
        [JsonIgnore]
        public KeyValuePair<ThemeType, string>[] ThemeItems
        {
            get
            {
                return m_themeList.ToArray(); 
            }
        }

        [JsonIgnore]
        public KeyValuePair<TransparencyType, string>[] TransparencyItems
        {
            get
            {
                return m_transparencyList.ToArray();
            }
        }

        /// <summary>
        /// Theme is a ComboBox control.  The full KeyValuePair is stored in the item array for display.
        /// During validation, just the Key is checked for validity.
        /// </summary>
        [JsonIgnore]
        public ThemeType ThemeSetting
        {
            get { return Theme.Value.Key; }
            set
            {
                if (!m_themeList.ContainsKey(value))
                    throw new FormatException("ThemeType key not found.");

                Theme = new KeyValuePair<ThemeType, string>(value, m_themeList[value]);
            }
        }

        /// <summary>
        /// Transparency is a ComboBox control.  The full KeyValuePair is stored in the item array for display.
        /// During validation, just the Key is checked for validity.
        /// </summary>
        [JsonIgnore]
        public TransparencyType TransparencySetting
        {
            get { return Transparency.Value.Key; }
            set
            {
                if (!m_transparencyList.ContainsKey(value))
                    throw new FormatException("TransparencyType key not found.");

                Transparency = new KeyValuePair<TransparencyType, string>(value, m_transparencyList[value]);
            }
        }

        public Office2010Theme GetOfficeColorScheme(ThemeType themeType, out Color? managedColor)
        {
            managedColor = null;

            switch (themeType)
            {
                case ThemeType.Office2010Black:
                    return Office2010Theme.Black;

                case ThemeType.Office2010Blue:
                    return Office2010Theme.Blue;

                case ThemeType.Office2010Silver:
                    return Office2010Theme.Silver;

                case ThemeType.ZwiftyOrange:
                    managedColor = ZwiftyOrange;
                    return Office2010Theme.Managed;

                default:
                    managedColor = ManagedColor;
                    return Office2010Theme.Managed;
            }
        }
        public MSoffice2010Theme GetMSoffice2010Theme(ThemeType themeType, out Color? managedColor)
        {
            managedColor = null;

            switch (themeType)
            {
                case ThemeType.Office2010Black:
                    return MSoffice2010Theme.Black;

                case ThemeType.Office2010Blue:
                    return MSoffice2010Theme.Blue;

                case ThemeType.Office2010Silver:
                    return MSoffice2010Theme.Silver;

                case ThemeType.ZwiftyOrange:
                    managedColor = ZwiftyOrange;
                    return MSoffice2010Theme.Managed;

                default:
                    managedColor = ManagedColor;
                    return MSoffice2010Theme.Managed;
            }
        }

        public static void ApplyColorScheme(Office2010Form form)
        {
            ZAMappearance settings = ZAMsettings.Settings.Appearance;

            //if (settings.ThemeSetting != ThemeType.Custom)
            //{
                Office2010Colors.DefaultTheme = settings.GetOfficeColorScheme(settings.ThemeSetting, out Color? managedColor);
            form.ColorScheme = Office2010Colors.DefaultTheme;
            form.UseOffice2010SchemeBackColor = true;

            if (Office2010Colors.DefaultTheme == Office2010Theme.Managed)
                {
                    Office2010Colors.ApplyManagedColors(form, managedColor.Value);
                }
                else
                {
                //Office2010Colors.ApplyManagedScheme(form, Office2010Colors.DefaultTheme);
                }
                //form.ColorScheme = settings.GetOfficeColorScheme(settings.ThemeSetting, out Color? managedColor);
            //}
            //else
            //{
            //    form.ColorScheme = Office2010Theme.Managed;
            //    Office2010Colors.ApplyManagedColors(form, settings.ManagedColor);
            //}

        }

        /// <summary>
        /// Standard color setup for SfForm based forms
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static MSoffice2010ColorManager ApplyColorTable(Syncfusion.WinForms.Controls.SfForm form)
        {
            MSoffice2010ColorManager colorTable = ZAMappearance.GetColorTable();

            form.Style.TitleBar.BackColor = colorTable.ActiveTitleGradientEnd;
            form.Style.TitleBar.ForeColor = colorTable.FormTextColor;
            form.Style.TitleBar.CloseButtonForeColor = colorTable.FormTextColor;
            form.Style.TitleBar.CloseButtonHoverBackColor = colorTable.XPTaskBarBoxBackColor;
            form.Style.TitleBar.CloseButtonHoverForeColor = colorTable.XPTaskBarBoxForeColor;
            form.Style.TitleBar.TextHorizontalAlignment = HorizontalAlignment.Center;
            form.Style.TitleBar.CloseButtonSize = new Size(32, 32);
            form.Style.TitleBar.Font = new System.Drawing.Font("Franklin Gothic Demi Cond", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            form.Style.TitleBar.IconBackColor = System.Drawing.Color.Transparent;
            
            form.Style.Border = new Pen(colorTable.ActiveFormBorderColor, 2);
            form.Style.InactiveBorder = new Pen(colorTable.InactiveFormBorderColor, 2);
            form.Style.ShadowOpacity = 0;

            //form.Style.BackColor = colorTable.FormBackground;
            //form.Style.ForeColor = colorTable.FormTextColor;
            //Debug.WriteLine($"ForeColor before: {form.ForeColor.R},{form.ForeColor.G},{form.ForeColor.B}");
            form.ForeColor = colorTable.FormTextColor;
            //Debug.WriteLine($"ForeColor after: {form.ForeColor.R},{form.ForeColor.G},{form.ForeColor.B}");
            form.BackColor = colorTable.FormBackground;

            return colorTable;
        }

        public static MSoffice2010ColorManager GetColorTable()
        {
            ZAMappearance settings = ZAMsettings.Settings.Appearance;

            MSoffice2010ColorManager colorTable;

            if (settings.ThemeSetting != ThemeType.Custom)
            {
                MSoffice2010Theme theme = settings.GetMSoffice2010Theme(settings.ThemeSetting, out Color? managedColor);

                if (theme == MSoffice2010Theme.Managed)
                {
                    // store managed colors in the static object
                    MSoffice2010ColorManager.ApplyManagedColors(managedColor.Value);
                    colorTable = MSoffice2010ColorManager.GetColorTable(MSoffice2010Theme.Managed);
                }
                else
                {
                    colorTable = MSoffice2010ColorManager.GetColorTable(theme);
                }
            }
            else
            {
                // store managed colors in the static object
                MSoffice2010ColorManager.ApplyManagedColors(settings.ManagedColor);
                colorTable = MSoffice2010ColorManager.GetColorTable(MSoffice2010Theme.Managed);
            }

            return colorTable;
        }
    }
    #endregion
}
