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
                //Debug.WriteLine($"Initializing Theme");
                Theme = new KeyValuePair<ThemeType, string>(ThemeType.ZwiftyOrange, m_themeList[ThemeType.ZwiftyOrange]); // default
                count++;
            }

            if (Transparency == null)
            {
                //Debug.WriteLine($"Initializing Transparency");
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

        public static void ApplyColorScheme(Office2010Form form)
        {
            form.UseOffice2010SchemeBackColor = true;

            ZAMappearance settings = ZAMsettings.Settings.Appearance;

            if (settings.ThemeSetting != ThemeType.Custom)
            {
                form.ColorScheme = settings.GetOfficeColorScheme(settings.ThemeSetting, out Color? managedColor);

                if (form.ColorScheme == Office2010Theme.Managed)
                {
                    Office2010Colors.ApplyManagedColors(form, managedColor.Value);
                }
            }
            else
            {
                form.ColorScheme = Office2010Theme.Managed;
                Office2010Colors.ApplyManagedColors(form, settings.ManagedColor);
            }

        }
    }
    #endregion
}
