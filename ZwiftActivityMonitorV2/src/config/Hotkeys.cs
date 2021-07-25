using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WK.Libraries.HotkeyListenerNS;

namespace ZwiftActivityMonitorV2
{
    public class Hotkeys : ConfigItemBase
    {
        public string ActivityViewHotKeySequence { get; set; }
        public string SplitViewHotkeySequence { get; set; }
        public string LapViewHotkeySequence { get; set; }
        public string NewLapHotkeySequence { get; set; }
        public string ResetLapsHotkeySequence { get; set; }

        public static Hotkey ActivityViewHotkey = new();
        public static Hotkey SplitViewHotkey = new();
        public static Hotkey LapViewHotkey = new();
        public static Hotkey NewLapHotkey = new();
        public static Hotkey ResetLapsHotkey = new();

        public Hotkeys()
        {
        }

        /// <summary>
        /// Update all hotkeys based upon their text representation.  Keys are automatically added if new.
        /// </summary>
        public void UpdateHotkeys()
        {
            this.UpdateHotkey(ref ActivityViewHotkey, new Hotkey(this.ActivityViewHotKeySequence));
            this.UpdateHotkey(ref SplitViewHotkey, new Hotkey(this.SplitViewHotkeySequence));
            this.UpdateHotkey(ref LapViewHotkey, new Hotkey(this.LapViewHotkeySequence));
            this.UpdateHotkey(ref NewLapHotkey, new Hotkey(this.NewLapHotkeySequence));
            this.UpdateHotkey(ref ResetLapsHotkey, new Hotkey(this.ResetLapsHotkeySequence));
        }

        /// <summary>
        /// The author's Update method has a bug when currentHotKey is passed by reference.
        /// This replaces that method by calling the non-ref method first, and then assigning newHotkey to curHotkey
        /// </summary>
        /// <param name="currentHotkey"></param>
        /// <param name="newHotkey"></param>
        private void UpdateHotkey(ref Hotkey currentHotkey, Hotkey newHotkey)
        {
            ZAMsettings.HotkeyListener.Update(currentHotkey, newHotkey);

            currentHotkey = newHotkey;
        }

        public void AddHotkeys()
        {
            if (ActivityViewHotkey.KeyCode != Keys.None)
                ZAMsettings.HotkeyListener.Add(ActivityViewHotkey);

            if (SplitViewHotkey.KeyCode != Keys.None)
                ZAMsettings.HotkeyListener.Add(SplitViewHotkey);

            if (LapViewHotkey.KeyCode != Keys.None)
                ZAMsettings.HotkeyListener.Add(LapViewHotkey);

            if (NewLapHotkey.KeyCode != Keys.None)
                ZAMsettings.HotkeyListener.Add(NewLapHotkey);

            if (ResetLapsHotkey.KeyCode != Keys.None)
                ZAMsettings.HotkeyListener.Add(ResetLapsHotkey);
        }


        public override int InitializeDefaultValues()
        {
            int count = 0;

            if (string.IsNullOrEmpty(this.ActivityViewHotKeySequence))
            {
                this.ActivityViewHotKeySequence = new Hotkey().ToString();
                count++;
            }

            if (string.IsNullOrEmpty(this.SplitViewHotkeySequence))
            {
                this.SplitViewHotkeySequence = new Hotkey().ToString();
                count++;
            }

            if (string.IsNullOrEmpty(this.LapViewHotkeySequence))
            {
                this.LapViewHotkeySequence = new Hotkey().ToString();
                count++;
            }

            if (string.IsNullOrEmpty(this.NewLapHotkeySequence))
            {
                this.NewLapHotkeySequence = new Hotkey().ToString();
                count++;
            }

            if (string.IsNullOrEmpty(this.ResetLapsHotkeySequence))
            {
                this.ResetLapsHotkeySequence = new Hotkey().ToString();
                count++;
            }

            ActivityViewHotkey = HotkeyListener.Convert(this.ActivityViewHotKeySequence); ;
            SplitViewHotkey = HotkeyListener.Convert(this.SplitViewHotkeySequence);
            LapViewHotkey = HotkeyListener.Convert(this.LapViewHotkeySequence);
            NewLapHotkey = HotkeyListener.Convert(this.NewLapHotkeySequence);
            ResetLapsHotkey = HotkeyListener.Convert(this.ResetLapsHotkeySequence);


            return count;
        }
    }
}
