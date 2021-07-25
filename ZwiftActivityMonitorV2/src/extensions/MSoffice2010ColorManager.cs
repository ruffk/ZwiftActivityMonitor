using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ZwiftActivityMonitorV2
{
    public enum MSoffice2010Theme
    {
        Managed = -1, // 0xFFFFFFFF
        Blue = 0,
        Silver = 1,
        Black = 2,
    }

    public class MSoffice2010ColorManager : ICloneable
    {
        #region Declarations

        private static WeakReference s_blueColors = (WeakReference)null;
        private static WeakReference s_silverColors = (WeakReference)null;
        private static WeakReference s_blackColors = (WeakReference)null;
        private static MSoffice2010Theme s_defaultTheme = MSoffice2010Theme.Blue;
        private static Color s_managedBaseColor = Color.Empty;

        internal static WeakReference s_managedColors = (WeakReference)null;
        internal bool isManagedColorNotAlive;
        
        protected Color m_ShapeHoverGradientStartColor = Color.Empty;
        protected Color m_ShapeHoverGradientEndColor = Color.Empty;
        protected Color m_ShapeSelectionGradientStartColor = Color.Empty;
        protected Color m_ShapeSelectionGradientEndColor = Color.Empty;
        protected Color m_ShapeSelectionFillColor = Color.Empty;
        protected Color m_ShapeHoverFillColor = Color.Empty;
        protected Color m_ShapeBorderColor = Color.Empty;
        protected Color m_ShapeSelectedColor = Color.Empty;
        protected Color m_TabItemBorderColor = Color.Empty;
        protected Color m_TabItemInnerBorderColor = Color.Empty;
        protected Color m_TabItemOuterBorderColor = Color.Empty;
        protected Color m_TabItemTextColor = Color.Empty;
        protected Color m_TabItemActiveBottomColor = Color.Empty;
        protected Color m_TabItemTopGradientColor = Color.Empty;
        protected Color m_TabItemInActiveBottomColor = Color.Empty;
        protected Color m_TabItemMiddleLineColor = Color.Empty;
        protected Color m_TabPanelColor = Color.Empty;
        protected Color m_TabPanelBorderColor = Color.Empty;
        protected Color m_TabPanelBackColor = Color.Empty;
        protected Color m_DataTimePickerBorderColor = Color.Empty;
        protected Color m_DataTimePickerHighLightedBorderColor = Color.Empty;
        protected Color m_DataTimePickerSelectedBorderColor = Color.Empty;
        protected Color m_DataTimePickerDropDownArrowColor = Color.Empty;
        protected Color m_DataTimePickerDropDownLightColor = Color.Empty;
        protected Color m_DataTimePickerDropDownDarkColor = Color.Empty;
        protected Color m_DataTimePickerDropDownHighLightLightColor = Color.Empty;
        protected Color m_DataTimePickerDropDownHighLightDarkColor = Color.Empty;
        protected Color m_DataTimePickerDropDownSelectedLightColor = Color.Empty;
        protected Color m_DataTimePickerDropDownSelectedDarkColor = Color.Empty;
        protected Color m_DataTimePickerCheckBoxNormalColor = Color.Empty;
        protected Color m_DataTimePickerCheckBoxSelectedColor = Color.Empty;
        protected Color m_DataTimePickerCheckBoxBorderPushedColor = Color.Empty;
        protected Color m_DataTimePickerCheckBoxBorderNormalColor = Color.Empty;
        protected Color m_DataTimePickerCheckBoxInnerRectBorderNormalColor = Color.Empty;
        protected Color m_DataTimePickerCheckBoxInnerRectBorderSelectedColor = Color.Empty;
        protected Color m_DataTimePickerCheckBoxInnerRectBorderPushedColor = Color.Empty;
        protected Color m_DataTimePickerCheckBoxInnerRectFillNormalColor = Color.Empty;
        protected Color m_DataTimePickerCheckBoxInnerRectFillSelectedColor = Color.Empty;
        protected Color m_DataTimePickerCheckBoxInnerRectFillPushedColor = Color.Empty;
        protected Color m_DataTimePickerHighLightedForeColor = Color.Empty;
        protected Color m_MonthCalendarHeaderStartColor = Color.Empty;
        protected Color m_MonthCalendarHeaderEndColor = Color.Empty;
        protected Color m_MonthCalendarForeColor = Color.Empty;
        protected Color m_MonthCalendarBackgroundColor = Color.Empty;
        protected Color m_GroupBarBorderColor = Color.Empty;
        protected Color m_GroupBarItemColorDark = Color.Empty;
        protected Color m_GroupBarItemColorLight = Color.Empty;
        protected Color m_GroupBarHighlightColorLight = Color.Empty;
        protected Color m_GroupBarHighlightColorDark = Color.Empty;
        protected Color m_GroupBarSelectedColorLight = Color.Empty;
        protected Color m_GroupBarSelectedColorDark = Color.Empty;
        protected Color m_GroupBarSelectedTopColorLight = Color.Empty;
        protected Color m_GroupBarSelectedTopColorDark = Color.Empty;
        protected Color m_GroupBarSelectedHighlightColorLight = Color.Empty;
        protected Color m_GroupBarSelectedHighlightColorDark = Color.Empty;
        protected Color m_GroupBarHeaderColorLight = Color.Empty;
        protected Color m_GroupBarHeaderColorDark = Color.Empty;
        protected Color m_GroupBarItemTextColor = Color.Empty;
        protected Color m_GroupBarBackColor = Color.Empty;
        protected Color m_GroupBarHeaderTextColor = Color.Empty;
        protected Color m_GroupBarSplitterColorDark = Color.Empty;
        protected Color m_GroupBarSplitterColorLight = Color.Empty;
        protected Color m_GroupBarClientAreaBackground = Color.Empty;
        protected Color m_XPTaskPaneInternalBorderColor = Color.Empty;
        protected Color m_XPTaskPaneBorderColor = Color.Empty;
        protected Color m_XPTaskPageBackColor = Color.Empty;
        protected Color m_MenuBorderColor = Color.Empty;
        protected Color m_MenuSeparatorColor = Color.Empty;
        protected Color m_MenuColumnColor = Color.Empty;
        protected Color m_MenuColumnSeparatorColor = Color.Empty;
        protected Color m_MenuBackground = Color.Empty;
        protected Color m_MenuItemBorderColor = Color.Empty;
        protected Color m_MenuItemDarkColor = Color.Empty;
        protected Color m_MenuItemLightColor = Color.Empty;
        protected Color m_MenuItemArrowLightColor = Color.Empty;
        protected Color m_MenuItemArrowDarkColor = Color.Empty;
        protected Color m_MenuCheckedColor = Color.Empty;
        protected Color m_MenuCheckedFillColor = Color.Empty;
        protected Color m_MenuCheckedBorderColor = Color.Empty;
        protected Color m_MenuTextBoxBorderColor = Color.Empty;
        protected Color m_MenuTextBoxBackColor = Color.Empty;
        protected Color m_MenuComboButtonPushed1Color = Color.Empty;
        protected Color m_MenuComboButtonPushed2Color = Color.Empty;
        protected Color m_MenuComboButtonPushed3Color = Color.Empty;
        protected Color m_MenuComboButtonPushed4Color = Color.Empty;
        protected Color m_MenuComboButtonHighlightLightColor = Color.Empty;
        protected Color m_MenuComboButtonHighlightDarkColor = Color.Empty;
        protected Color m_MenuComboButtonArrowColor = Color.Empty;
        protected Color m_DropDownLightColor = Color.Empty;
        protected Color m_DropDownDarkColor = Color.Empty;
        protected Color m_CommandBarDarkColor = Color.Empty;
        protected Color m_CommandBarLightColor = Color.Empty;
        protected Color m_CommandBarBorderColor = Color.Empty;
        protected Color m_DockBarBackColor = Color.Empty;
        protected Color m_DropDownHighlightLightColor = Color.Empty;
        protected Color m_DropDownHighlightDarkColor = Color.Empty;
        protected Color m_DropDownPressedLightColor = Color.Empty;
        protected Color m_DropDownPressedDarkColor = Color.Empty;
        protected Color m_FloatHighlightButtonColor = Color.Empty;
        protected Color m_FloatHighlightButtonBorderColor = Color.Empty;
        protected Color m_FloatPressButtonColor = Color.Empty;
        protected Color m_FloatPressButtonBorderColor = Color.Empty;
        protected Color m_FloatPressCloseButtonBorderColor = Color.Empty;
        protected Color m_FloatPressCloseButtonColor = Color.Empty;
        protected Color m_FloatCommandBarLightColor = Color.Empty;
        protected Color m_FloatCommandBarDarkColor = Color.Empty;
        protected Color m_FloatLightBorderColor = Color.Empty;
        protected Color m_FloatBackgroundColor = Color.Empty;
        protected Color m_FloatBorderColor = Color.Empty;
        protected Color m_FloatCaptionColor = Color.Empty;
        protected Color m_BarItemSeparatorColor = Color.Empty;
        protected Color m_BarItemPressBorderColor = Color.Empty;
        protected Color m_BarItemHighlightBorderColor = Color.Empty;
        protected Color m_BarItemPressLightColor = Color.Empty;
        protected Color m_BarItemPressDarkColor = Color.Empty;
        protected Color m_DropDownBarItemLightColor = Color.Empty;
        protected Color m_DropDownBarItemDarkColor = Color.Empty;
        protected Color m_DropDownBarItemBorderColor = Color.Empty;
        protected Color m_BarItemCheckLightColor = Color.Empty;
        protected Color m_BarItemCheckDarkColor = Color.Empty;
        protected Color m_BarItemCheckBorderColor = Color.Empty;
        protected Color m_BarItemCheckFlashColor = Color.Empty;
        protected Color m_BarItemPressFlashColor = Color.Empty;
        protected Color m_BarItemSelectFlashColor = Color.Empty;
        protected Color m_TextBarItemBackColor = Color.Empty;
        protected Color m_TextBarItemBorderColor = Color.Empty;
        protected Color m_TextBarItemBorderHighlightColor = Color.Empty;
        protected Color m_ComboButtonLightColor = Color.Empty;
        protected Color m_ComboButtonDarkColor = Color.Empty;
        protected Color m_ComboButtonPressLightColor = Color.Empty;
        protected Color m_ComboButtonPressDarkColor = Color.Empty;
        protected Color m_ComboButtonHighlightLightColor = Color.Empty;
        protected Color m_ComboButtonHighlightDarkColor = Color.Empty;
        protected Color m_ComboButtonBorder = Color.Empty;
        protected Color m_ComboButtonPressBorder = Color.Empty;
        protected Color m_ComboButtonHighlightBorder = Color.Empty;
        protected Color m_ButtonPressedTopColor = Color.Empty;
        protected Color m_ButtonPressedBottomColor = Color.Empty;
        protected Color m_ButtonSelectedTopColor = Color.Empty;
        protected Color m_ButtonSelectedBottomColor = Color.Empty;
        protected Color m_ButtonDisabledTopColor = Color.Empty;
        protected Color m_ButtonDisabledBottomColor = Color.Empty;
        protected Color m_ButtonPressedBorderColor = Color.Empty;
        protected Color m_ButtonSelectedBorderColor = Color.Empty;
        protected Color m_ButtonDisabledBorderColor = Color.Empty;
        protected Color m_ButtonDefaultTopColor = Color.Empty;
        protected Color m_ButtonDefaultBottomColor = Color.Empty;
        protected Color m_ButtonDefaultBorderColor = Color.Empty;
        protected Color m_ButtonDefaultInternalBorderColor = Color.Empty;
        protected Color m_ButtonPressedInternalBorderColor = Color.Empty;
        protected Color m_ButtonSelectedInternalBorderColor = Color.Empty;
        protected Color m_BlueButtonDefaultTopColor = Color.Empty;
        protected Color m_BlueButtonDefaultBottomColor = Color.Empty;
        protected Color m_BlueButtonDefaultBorderColor = Color.Empty;
        protected Color m_BlueButtonDefaultInternalBorderColor = Color.Empty;
        protected Color m_BlueButtonPressedInternalBorderColor = Color.Empty;
        protected Color m_BlueButtonSelectedInternalBorderColor = Color.Empty;
        protected Color m_SilverButtonDefaultTopColor = Color.Empty;
        protected Color m_SilverButtonDefaultBottomColor = Color.Empty;
        protected Color m_SilverButtonDefaultBorderColor = Color.Empty;
        protected Color m_SilverButtonDefaultInternalBorderColor = Color.Empty;
        protected Color m_SilverButtonPressedInternalBorderColor = Color.Empty;
        protected Color m_SilverButtonSelectedInternalBorderColor = Color.Empty;
        protected Color m_BlackButtonDefaultTopColor = Color.Empty;
        protected Color m_BlackButtonDefaultBottomColor = Color.Empty;
        protected Color m_BlackButtonDefaultBorderColor = Color.Empty;
        protected Color m_BlackButtonDefaultInternalBorderColor = Color.Empty;
        protected Color m_BlackButtonPressedInternalBorderColor = Color.Empty;
        protected Color m_BlackButtonSelectedInternalBorderColor = Color.Empty;
        protected Color m_NumericUpDownBorderColor = Color.Empty;
        protected Color m_NumericUpDownHighLightedBorderColor = Color.Empty;
        protected Color m_NumericUpDownSelectedBorderColor = Color.Empty;
        protected Color m_NumericUpDownArrowLightColor = Color.Empty;
        protected Color m_NumericUpDownArrowDarkColor = Color.Empty;
        protected Color m_TabDefaultBorderColor = Color.Empty;
        protected Color m_TabHotLightBottomBorderLineColor = Color.Empty;
        protected Color m_TabHotLightGradientTopBeginColor = Color.Empty;
        protected Color m_TabHotLightGradientTopEndColor = Color.Empty;
        protected Color m_TabHotLightGradientBottomBeginColor = Color.Empty;
        protected Color m_TabHotLightGradientBottomEndColor = Color.Empty;
        protected Color m_TabHotLightGradientCircleColor = Color.Empty;
        protected Color m_TabSelectedGradientTopColor = Color.Empty;
        protected Color m_TabSelectedGradientBottomColor = Color.Empty;
        protected Color m_TabSelectedInnerBorderColor = Color.Empty;
        protected Color m_TabHighlightInnerBorderColor = Color.Empty;
        protected Color m_TabSelectedHotLightBorderColor = Color.Empty;
        protected Color m_TabSelectedHotLightInnerBorderColor = Color.Empty;
        protected Color m_TabForeColor = Color.Empty;
        protected Color m_ActiveTabForeColor = Color.Empty;
        protected Color m_TabBackgroundColor = Color.Empty;
        protected Color m_TabScrollArrowColor = Color.Empty;
        protected Color m_DockTabForeColor = Color.Empty;
        protected Color m_DockTabBackgroundColor = Color.Empty;
        protected Color m_SelectedNodeBackground = Color.Empty;
        protected Color m_TreeNodeArrowColor = Color.Empty;
        protected Color m_TreeviewBackColor = Color.Empty;
        protected Color m_TreeViewFontColor = Color.Empty;
        protected Color m_ActiveTextBoxBorderColor = Color.Empty;
        protected Color m_InactiveTextBoxBorderColor = Color.Empty;
        protected Color m_ActiveTextBoxBackColor = Color.Empty;
        protected Color m_InactiveTextBoxBackColor = Color.Empty;
        protected Color m_ActiveFormBorderColor = Color.Empty;
        protected Color m_InactiveFormBorderColor = Color.Empty;
        protected Color m_FormTextColor = Color.Empty;
        protected Color m_ActiveTitleGradientBegin = Color.Empty;
        protected Color m_ActiveTitleGradientEnd = Color.Empty;
        protected Color m_InactiveTitleGradientBegin = Color.Empty;
        protected Color m_InactiveTitleGradientEnd = Color.Empty;
        protected Color m_SystemButtonSelectedGradientBegin = Color.Empty;
        protected Color m_SystemButtonSelectedGradientEnd = Color.Empty;
        protected Color m_SystemButtonPressedGradientBegin = Color.Empty;
        protected Color m_SystemButtonPressedGradientEnd = Color.Empty;
        protected Color m_SystemButtonBorderSelected = Color.Empty;
        protected Color m_SystemButtonBorderPressed = Color.Empty;
        protected Color m_FormBackground = Color.Empty;
        protected Color m_UpDownArrowStartColor = Color.Empty;
        protected Color m_UpDownArrowEndColor = Color.Empty;
        protected Color m_UpDownBorderNormalColor = Color.Empty;
        protected Color m_UpDownBackgroundNormalColor = Color.Empty;
        protected Color m_UpDownBackgroundNormalStartColor = Color.Empty;
        protected Color m_UpDownBackgroundNormalEndColor = Color.Empty;
        protected Color m_UpDownBorderHotColor = Color.Empty;
        protected Color m_UpDownInnerBorderHotStartColor = Color.Empty;
        protected Color m_UpDownInnerBorderHotEndColor = Color.Empty;
        protected Color m_UpDownBorderPressedColor = Color.Empty;
        protected Color m_UpDownInnerBorderPressedStartColor = Color.Empty;
        protected Color m_UpDownInnerBorderPressedEndColor = Color.Empty;
        protected Color m_UpDownBackgroundDisabledStartColor = Color.Empty;
        protected Color m_UpDownBackgroundDisabledEndColor = Color.Empty;
        protected Color m_UpDownBorderDisabledColor = Color.Empty;
        protected Color m_UpDownBackgroundHotTopStartColor = Color.Empty;
        protected Color m_UpDownBackgroundHotTopEndColor = Color.Empty;
        protected Color m_UpDownBackgroundHotBottomStartColor = Color.Empty;
        protected Color m_UpDownBackgroundHotBottomEndColor = Color.Empty;
        protected Color m_UpDownBackgroundPressedTopStartColor = Color.Empty;
        protected Color m_UpDownBackgroundPressedTopEndColor = Color.Empty;
        protected Color m_UpDownBackgroundPressedBottomStartColor = Color.Empty;
        protected Color m_UpDownBackgroundPressedBottomEndColor = Color.Empty;
        protected Color m_ComboBoxAdvNormalBackColor = Color.Empty;
        protected Color m_ComboBoxAdvHotBackColor = Color.Empty;
        protected Color m_ComboBoxAdvNormalBorderColor = Color.Empty;
        protected Color m_ComboBoxAdvHotBorderColor = Color.Empty;
        protected Color m_ComboBoxAdvPushedBorderColor = Color.Empty;
        protected Color m_ComboBoxAdvButtonUpperLineColor = Color.Empty;
        protected Color m_ComboBoxAdvArrowColor = Color.Empty;
        protected Color m_ComboBoxAdvLowerArrowLineColor = Color.Empty;
        protected Color m_ComboBoxAdvHotBackgroundButtonColor1 = Color.Empty;
        protected Color m_ComboBoxAdvHotBackgroundButtonColor2 = Color.Empty;
        protected Color m_ComboBoxAdvHotBackgroundButtonColor3 = Color.Empty;
        protected Color m_ComboBoxAdvHotBackgroundButtonColor4 = Color.Empty;
        protected Color m_ComboBoxAdvNormalBackgroundButtonColor1 = Color.Empty;
        protected Color m_ComboBoxAdvNormalBackgroundButtonColor2 = Color.Empty;
        protected Color m_ComboBoxAdvNormalBackgroundButtonColor3 = Color.Empty;
        protected Color m_ComboBoxAdvNormalBackgroundButtonColor4 = Color.Empty;
        protected Color m_ComboBoxAdvPushedBackgroundButtonColor1 = Color.Empty;
        protected Color m_ComboBoxAdvPushedBackgroundButtonColor2 = Color.Empty;
        protected Color m_ComboBoxAdvPushedBackgroundButtonColor3 = Color.Empty;
        protected Color m_ComboBoxAdvPushedBackgroundButtonColor4 = Color.Empty;
        protected Color m_CheckBoxAdvNormalBackColor = Color.Empty;
        protected Color m_CheckBoxAdvSelectedBackColor = Color.Empty;
        protected Color m_CheckBoxAdvPushedBackColor = Color.Empty;
        protected Color m_CheckBoxAdvNormalBorderColor = Color.Empty;
        protected Color m_CheckBoxAdvSelectedBorderColor = Color.Empty;
        protected Color m_CheckBoxAdvPushedBorderColor = Color.Empty;
        protected Color m_CheckBoxAdvNormalInternalBorderColor = Color.Empty;
        protected Color m_CheckBoxAdvSelectedInternalBorderColor = Color.Empty;
        protected Color m_CheckBoxAdvPushedInternalBorderColor = Color.Empty;
        protected Color m_CheckBoxAdvNormalInternalRectangleBorderColor = Color.Empty;
        protected Color m_CheckBoxAdvSelectedInternalRectangleBorderColor = Color.Empty;
        protected Color m_CheckBoxAdvPushedInternalRectangleBorderColor = Color.Empty;
        protected Color m_CheckBoxAdvNormalInternalRectangleColor = Color.Empty;
        protected Color m_CheckBoxAdvSelectedInternalRectangleColor = Color.Empty;
        protected Color m_CheckBoxAdvPushedInternalRectangleColor = Color.Empty;
        protected Color m_CheckBoxAdvNormalTickColor = Color.Empty;
        protected Color m_CheckBoxAdvSelectedTickColor = Color.Empty;
        protected Color m_CheckBoxAdvPushedTickColor = Color.Empty;
        protected Color m_CheckBoxAdvDisabledTickColor = Color.Empty;
        protected Color m_CheckBoxAdvIndeterminateRectangleColor = Color.Empty;
        protected Color m_CheckBoxAdvDisabledBackColor = Color.Empty;
        protected Color m_CheckBoxAdvDisabledBorderColor = Color.Empty;
        protected Color m_CheckBoxAdvDisabledInternalBorderColor = Color.Empty;
        protected Color m_RadioButtonAdvNormalBackColor = Color.Empty;
        protected Color m_RadioButtonAdvNormalBorderColor = Color.Empty;
        protected Color m_RadioButtonAdvNormalInternalBorderColor = Color.Empty;
        protected Color m_RadioButtonAdvSelectedBackColor = Color.Empty;
        protected Color m_RadioButtonAdvSelectedBorderColor = Color.Empty;
        protected Color m_RadioButtonAdvSelectedInternalBorderColor = Color.Empty;
        protected Color m_RadioButtonAdvPushedBackColor = Color.Empty;
        protected Color m_RadioButtonAdvPushedBorderColor = Color.Empty;
        protected Color m_RadioButtonAdvPushedInternalBorderColor = Color.Empty;
        protected Color m_RadioButtonAdvCheckMarkBorderColor = Color.Empty;
        protected Color m_RadioButtonAdvCheckMarkNormalBottomColor = Color.Empty;
        protected Color m_RadioButtonAdvCheckMarkSelectedBottomColor = Color.Empty;
        protected Color m_RadioButtonAdvCheckMarkPushedBottomColor = Color.Empty;
        protected Color m_TabBarSplitterBackColor = Color.Empty;
        protected Color m_TabBarSplitterBorderColor = Color.Empty;
        protected Color m_TabBarSplitterTextColor = Color.Empty;
        protected Color m_TabBarSplitterTabStartColor = Color.Empty;
        protected Color m_TabBarSplitterTabEndColor = Color.Empty;
        protected Color m_TabBarSplitterTabBarStartColor = Color.Empty;
        protected Color m_TabBarSplitterTabBarEndColor = Color.Empty;
        protected Color m_TabBarSplitterButtonHoveredStartColor = Color.Empty;
        protected Color m_TabBarSplitterButtonHoveredEndColor = Color.Empty;
        protected Color m_TabBarSplitterButtonPushedStartColor = Color.Empty;
        protected Color m_TabBarSplitterButtonPushedEndColor = Color.Empty;
        protected Color m_TabBarSplitterSizeGripperColor = Color.Empty;
        protected Color m_TabBarSplitterSizeGripperLightColor = Color.Empty;
        protected Color m_TabBarSplitterSizeGripperDarkColor = Color.Empty;
        protected Color m_XPTaskBarBorderColor = Color.Empty;
        protected Color m_XPTaskBarBoxBackColor = Color.Empty;
        protected Color m_XPTaskBarBoxForeColor = Color.Empty;
        protected Color m_XPTaskBarBoxHeaderUpperLineColor = Color.Empty;
        protected Color m_XPTaskBarBoxHeaderLowerLineColor = Color.Empty;
        protected Color m_XPTaskBarBoxArrowColor = Color.Empty;
        protected Color m_XPTaskBarBoxActiveHighlightedItemColor = Color.Empty;
        protected Color m_XPTaskBarBoxInactiveHighlightedItemColor = Color.Empty;
        protected Color m_ColorUIAdvBackColor = Color.Empty;
        protected Color m_ColorUIAdvTextColor = Color.Empty;
        protected Color m_ColorUIAdvItemBorderColor = Color.Empty;
        protected Color m_ColorUIAdvHighlightedBorderColor = Color.Empty;
        protected Color m_ColorUIAdvSelectedBorderColor = Color.Empty;
        protected Color m_ColorUIAdvSelectedHighlightedBorderColor = Color.Empty;
        protected Color m_ColorUIAdvGroupHeaderBackColor = Color.Empty;
        protected Color m_StatusBarExtTopGradient = Color.Empty;
        protected Color m_StatusBarExtBottomGradient = Color.Empty;
        protected Color m_StatusBarExtFillColor = Color.Empty;

        #endregion

        public static MSoffice2010ColorManager Default => MSoffice2010ColorManager.GetColorTable(MSoffice2010ColorManager.s_defaultTheme);

        public static MSoffice2010Theme DefaultTheme
        {
            get => MSoffice2010ColorManager.s_defaultTheme;
            set
            {
                if (value == MSoffice2010ColorManager.s_defaultTheme)
                    return;
                MSoffice2010ColorManager.s_defaultTheme = value;
            }
        }

        internal static MSoffice2010ColorManager ManagedColors
        {
            get
            {
                if (MSoffice2010ColorManager.s_managedColors.IsAlive)
                    return MSoffice2010ColorManager.s_managedColors.Target as MSoffice2010ColorManager;

                MSoffice2010BlueColors office2010BlueColors = new MSoffice2010BlueColors();
                MSoffice2010ColorManager.s_managedColors = new WeakReference((object)office2010BlueColors);
                return (MSoffice2010ColorManager)office2010BlueColors;
            }
        }

        public static Color ManagedBaseColor => MSoffice2010ColorManager.s_managedBaseColor;

        public static event MSoffice2010ColorManager.ManagedColorsAppliedEventHandler ManagedColorsApplied;

        //public static MSoffice2010ColorManager GetColorTable(MSoffice2010Theme theme)
        //{
        //    return GetColorTable(theme, out Color baseColor);
        //}


        public static MSoffice2010ColorManager GetColorTable(MSoffice2010Theme theme)
        {
            MSoffice2010ColorManager office2010Colors;
            switch (theme)
            {
                case MSoffice2010Theme.Managed:
                    office2010Colors = MSoffice2010ColorManager.ManagedColors;
                    break;
                case MSoffice2010Theme.Blue:
                    if (MSoffice2010ColorManager.s_blueColors.IsAlive)
                    {
                        office2010Colors = MSoffice2010ColorManager.s_blueColors.Target as MSoffice2010ColorManager;
                        break;
                    }
                    office2010Colors = (MSoffice2010ColorManager)new MSoffice2010BlueColors();
                    MSoffice2010ColorManager.s_blueColors = new WeakReference((object)office2010Colors);
                    break;
                case MSoffice2010Theme.Silver:
                    if (MSoffice2010ColorManager.s_silverColors.IsAlive)
                    {
                        office2010Colors = MSoffice2010ColorManager.s_silverColors.Target as MSoffice2010ColorManager;
                        break;
                    }
                    office2010Colors = (MSoffice2010ColorManager)new MSoffice2010SilverColors();
                    MSoffice2010ColorManager.s_silverColors = new WeakReference((object)office2010Colors);
                    break;
                case MSoffice2010Theme.Black:
                    if (MSoffice2010ColorManager.s_blackColors.IsAlive)
                    {
                        office2010Colors = MSoffice2010ColorManager.s_blackColors.Target as MSoffice2010ColorManager;
                        break;
                    }
                    office2010Colors = (MSoffice2010ColorManager)new MSoffice2010BlackColors();
                    MSoffice2010ColorManager.s_blackColors = new WeakReference((object)office2010Colors);
                    break;
                default:
                    throw new ArgumentException("Unknown theme.");
            }
            return office2010Colors;
        }

        public static void ApplyManagedColors(Color baseColor)
        {
            MSoffice2010ColorManager.s_managedBaseColor = baseColor;
            MSoffice2010ColorManager.ManagedColors.UpdateColors(baseColor);
        }

        //public static void ApplyManagedColors(Form form, Color baseColor)
        //{
        //    MSoffice2010ColorManager.s_managedBaseColor = baseColor;
        //    MSoffice2010ColorManager.ManagedColors.UpdateColors(baseColor);
        //    MSoffice2010ColorManager.OnManagedColorApplied(form, baseColor);
        //    if (!form.IsHandleCreated)
        //        return;
        //    Syncfusion.Runtime.InteropServices.NativeMethods.RedrawWindow(form.Handle, IntPtr.Zero, IntPtr.Zero, 1025U);
        //    form.Invalidate(true);
        //    form.Update();
        //}

        public static void ApplyManagedScheme(MSoffice2010Theme scheme)
        {
            MSoffice2010ColorManager.s_managedBaseColor = Color.Empty;
            MSoffice2010ColorManager.ManagedColors.UpdateScheme(scheme);
        }

        //public static void ApplyManagedScheme(Form form, MSoffice2010Theme scheme)
        //{
        //    MSoffice2010ColorManager.s_managedBaseColor = Color.Empty;
        //    MSoffice2010ColorManager.ManagedColors.UpdateScheme(scheme);
        //    if (!form.IsHandleCreated)
        //        return;
        //    Syncfusion.Runtime.InteropServices.NativeMethods.RedrawWindow(form.Handle, IntPtr.Zero, IntPtr.Zero, 1025U);
        //    form.Invalidate(true);
        //}

        protected static void OnManagedColorApplied(Form form, Color baseColor)
        {
            if (MSoffice2010ColorManager.ManagedColorsApplied == null)
                return;
            MSoffice2010ColorManager.ManagedColorsApplied(new MSoffice2010ColorManager.ManagedColorsAppliedEventArgs(form, baseColor));
        }

        static MSoffice2010ColorManager()
        {
            MSoffice2010ColorManager.s_managedColors = new WeakReference((object)new MSoffice2010BlueColors());
            MSoffice2010ColorManager.s_blueColors = new WeakReference((object)new MSoffice2010BlueColors());
            MSoffice2010ColorManager.s_silverColors = new WeakReference((object)new MSoffice2010SilverColors());
            MSoffice2010ColorManager.s_blackColors = new WeakReference((object)new MSoffice2010BlackColors());
        }

        protected MSoffice2010ColorManager() => this.InitializeColors();

        #region Public Color Accessors

        public Color TabPanelBorderColor
        {
            get => this.m_TabPanelBorderColor;
            set
            {
                if (!(this.m_TabPanelBorderColor != value))
                    return;
                this.m_TabPanelBorderColor = value;
            }
        }

        public Color TabPanelBackColor
        {
            get => this.m_TabPanelBackColor;
            set
            {
                if (!(this.m_TabPanelBackColor != value))
                    return;
                this.m_TabPanelBackColor = value;
            }
        }

        public Color TabPanelColor
        {
            get => this.m_TabPanelColor;
            set
            {
                if (!(this.m_TabPanelColor != value))
                    return;
                this.m_TabPanelColor = value;
            }
        }

        public Color TabItemBorderColor
        {
            get => this.m_TabItemBorderColor;
            set
            {
                if (!(this.m_TabItemBorderColor != value))
                    return;
                this.m_TabItemBorderColor = value;
            }
        }

        public Color TabItemInnerBorderColor
        {
            get => this.m_TabItemInnerBorderColor;
            set
            {
                if (!(this.m_TabItemInnerBorderColor != value))
                    return;
                this.m_TabItemInnerBorderColor = value;
            }
        }

        public Color TabItemOuterBorderColor
        {
            get => this.m_TabItemOuterBorderColor;
            set
            {
                if (!(this.m_TabItemOuterBorderColor != value))
                    return;
                this.m_TabItemOuterBorderColor = value;
            }
        }

        public Color TabItemTextColor
        {
            get => this.m_TabItemTextColor;
            set
            {
                if (!(this.m_TabItemTextColor != value))
                    return;
                this.m_TabItemTextColor = value;
            }
        }

        public Color TabItemActiveBottomColor
        {
            get => this.m_TabItemActiveBottomColor;
            set
            {
                if (!(this.m_TabItemActiveBottomColor != value))
                    return;
                this.m_TabItemActiveBottomColor = value;
            }
        }

        [Obsolete("Use TabItemTopGradientColor property instead.")]
        public Color TopGradientColor
        {
            get => this.TabItemTopGradientColor;
            set => this.TabItemTopGradientColor = value;
        }

        public Color TabItemTopGradientColor
        {
            get => this.m_TabItemTopGradientColor;
            set
            {
                if (!(this.m_TabItemTopGradientColor != value))
                    return;
                this.m_TabItemTopGradientColor = value;
            }
        }

        public Color TabItemInActiveBottomColor
        {
            get => this.m_TabItemInActiveBottomColor;
            set
            {
                if (!(this.m_TabItemInActiveBottomColor != value))
                    return;
                this.m_TabItemInActiveBottomColor = value;
            }
        }

        public Color TabItemMiddleLineColor
        {
            get => this.m_TabItemMiddleLineColor;
            set
            {
                if (!(this.m_TabItemMiddleLineColor != value))
                    return;
                this.m_TabItemMiddleLineColor = value;
            }
        }

        public Color DataTimePickerBorderColor
        {
            get => this.m_DataTimePickerBorderColor;
            set
            {
                if (!(this.m_DataTimePickerBorderColor != value))
                    return;
                this.m_DataTimePickerBorderColor = value;
            }
        }

        public Color DataTimePickerHighLightedBorderColor
        {
            get => this.m_DataTimePickerHighLightedBorderColor;
            set
            {
                if (!(this.m_DataTimePickerHighLightedBorderColor != value))
                    return;
                this.m_DataTimePickerHighLightedBorderColor = value;
            }
        }

        public Color DataTimePickerSelectedBorderColor
        {
            get => this.m_DataTimePickerSelectedBorderColor;
            set
            {
                if (!(this.m_DataTimePickerSelectedBorderColor != value))
                    return;
                this.m_DataTimePickerSelectedBorderColor = value;
            }
        }

        public Color DataTimePickerDropDownArrowColor
        {
            get => this.m_DataTimePickerDropDownArrowColor;
            set
            {
                if (!(this.m_DataTimePickerDropDownArrowColor != value))
                    return;
                this.m_DataTimePickerDropDownArrowColor = value;
            }
        }

        public Color DataTimePickerDropDownLightColor
        {
            get => this.m_DataTimePickerDropDownLightColor;
            set
            {
                if (!(this.m_DataTimePickerDropDownLightColor != value))
                    return;
                this.m_DataTimePickerDropDownLightColor = value;
            }
        }

        public Color DataTimePickerDropDownDarkColor
        {
            get => this.m_DataTimePickerDropDownDarkColor;
            set
            {
                if (!(this.m_DataTimePickerDropDownDarkColor != value))
                    return;
                this.m_DataTimePickerDropDownDarkColor = value;
            }
        }

        public Color DataTimePickerDropDownHighLightLightColor
        {
            get => this.m_DataTimePickerDropDownHighLightLightColor;
            set
            {
                if (!(this.m_DataTimePickerDropDownHighLightLightColor != value))
                    return;
                this.m_DataTimePickerDropDownHighLightLightColor = value;
            }
        }

        public Color DataTimePickerDropDownHighLightDarkColor
        {
            get => this.m_DataTimePickerDropDownHighLightDarkColor;
            set
            {
                if (!(this.m_DataTimePickerDropDownHighLightDarkColor != value))
                    return;
                this.m_DataTimePickerDropDownHighLightDarkColor = value;
            }
        }

        public Color DataTimePickerDropDownSelectedLightColor
        {
            get => this.m_DataTimePickerDropDownSelectedLightColor;
            set
            {
                if (!(this.m_DataTimePickerDropDownSelectedLightColor != value))
                    return;
                this.m_DataTimePickerDropDownSelectedLightColor = value;
            }
        }

        public Color DataTimePickerDropDownSelectedDarkColor
        {
            get => this.m_DataTimePickerDropDownSelectedDarkColor;
            set
            {
                if (!(this.m_DataTimePickerDropDownSelectedDarkColor != value))
                    return;
                this.m_DataTimePickerDropDownSelectedDarkColor = value;
            }
        }

        public Color DataTimePickerCheckBoxNormalColor
        {
            get => this.m_DataTimePickerCheckBoxNormalColor;
            set
            {
                if (!(this.m_DataTimePickerCheckBoxNormalColor != value))
                    return;
                this.m_DataTimePickerCheckBoxNormalColor = value;
            }
        }

        public Color DataTimePickerCheckBoxSelectedColor
        {
            get => this.m_DataTimePickerCheckBoxSelectedColor;
            set
            {
                if (!(this.m_DataTimePickerCheckBoxSelectedColor != value))
                    return;
                this.m_DataTimePickerCheckBoxSelectedColor = value;
            }
        }

        public Color DataTimePickerCheckBoxBorderPushedColor
        {
            get => this.m_DataTimePickerCheckBoxBorderPushedColor;
            set
            {
                if (!(this.m_DataTimePickerCheckBoxBorderPushedColor != value))
                    return;
                this.m_DataTimePickerCheckBoxBorderPushedColor = value;
            }
        }

        public Color DataTimePickerCheckBoxBorderNormalColor
        {
            get => this.m_DataTimePickerCheckBoxBorderNormalColor;
            set
            {
                if (!(this.m_DataTimePickerCheckBoxBorderNormalColor != value))
                    return;
                this.m_DataTimePickerCheckBoxBorderNormalColor = value;
            }
        }

        public Color DataTimePickerCheckBoxInnerRectBorderNormalColor
        {
            get => this.m_DataTimePickerCheckBoxInnerRectBorderNormalColor;
            set
            {
                if (!(this.m_DataTimePickerCheckBoxInnerRectBorderNormalColor != value))
                    return;
                this.m_DataTimePickerCheckBoxInnerRectBorderNormalColor = value;
            }
        }

        public Color DataTimePickerCheckBoxInnerRectBorderSelectedColor
        {
            get => this.m_DataTimePickerCheckBoxInnerRectBorderSelectedColor;
            set
            {
                if (!(this.m_DataTimePickerCheckBoxInnerRectBorderSelectedColor != value))
                    return;
                this.m_DataTimePickerCheckBoxInnerRectBorderSelectedColor = value;
            }
        }

        public Color DataTimePickerCheckBoxInnerRectBorderPushedColor
        {
            get => this.m_DataTimePickerCheckBoxInnerRectBorderPushedColor;
            set
            {
                if (!(this.m_DataTimePickerCheckBoxInnerRectBorderPushedColor != value))
                    return;
                this.m_DataTimePickerCheckBoxInnerRectBorderPushedColor = value;
            }
        }

        public Color DataTimePickerCheckBoxInnerRectFillNormalColor
        {
            get => this.m_DataTimePickerCheckBoxInnerRectFillNormalColor;
            set
            {
                if (!(this.m_DataTimePickerCheckBoxInnerRectFillNormalColor != value))
                    return;
                this.m_DataTimePickerCheckBoxInnerRectFillNormalColor = value;
            }
        }

        public Color DataTimePickerCheckBoxInnerRectFillSelectedColor
        {
            get => this.m_DataTimePickerCheckBoxInnerRectFillSelectedColor;
            set
            {
                if (!(this.m_DataTimePickerCheckBoxInnerRectFillSelectedColor != value))
                    return;
                this.m_DataTimePickerCheckBoxInnerRectFillSelectedColor = value;
            }
        }

        public Color DataTimePickerCheckBoxInnerRectFillPushedColor
        {
            get => this.m_DataTimePickerCheckBoxInnerRectFillPushedColor;
            set
            {
                if (!(this.m_DataTimePickerCheckBoxInnerRectFillPushedColor != value))
                    return;
                this.m_DataTimePickerCheckBoxInnerRectFillPushedColor = value;
            }
        }

        public Color DataTimePickerHighLightedForeColor
        {
            get => this.m_DataTimePickerHighLightedForeColor;
            set
            {
                if (!(this.m_DataTimePickerHighLightedForeColor != value))
                    return;
                this.m_DataTimePickerHighLightedForeColor = value;
            }
        }

        public Color NumericUpDownBorderColor
        {
            get => this.m_NumericUpDownBorderColor;
            set
            {
                if (!(this.m_NumericUpDownBorderColor != value))
                    return;
                this.m_NumericUpDownBorderColor = value;
            }
        }

        public Color NumericUpDownHighLightedBorderColor
        {
            get => this.m_NumericUpDownHighLightedBorderColor;
            set
            {
                if (!(this.m_NumericUpDownHighLightedBorderColor != value))
                    return;
                this.m_NumericUpDownHighLightedBorderColor = value;
            }
        }

        public Color NumericUpDownSelectedBorderColor
        {
            get => this.m_NumericUpDownSelectedBorderColor;
            set
            {
                if (!(this.m_NumericUpDownSelectedBorderColor != value))
                    return;
                this.m_NumericUpDownSelectedBorderColor = value;
            }
        }

        public Color NumericUpDownArrowLightColor
        {
            get => this.m_NumericUpDownArrowLightColor;
            set
            {
                if (!(this.m_NumericUpDownArrowLightColor != value))
                    return;
                this.m_NumericUpDownArrowLightColor = value;
            }
        }

        public Color NumericUpDownArrowDarkColor
        {
            get => this.m_NumericUpDownArrowDarkColor;
            set
            {
                if (!(this.m_NumericUpDownArrowDarkColor != value))
                    return;
                this.m_NumericUpDownArrowDarkColor = value;
            }
        }

        public Color MonthCalendarHeaderStartColor
        {
            get => this.m_MonthCalendarHeaderStartColor;
            set
            {
                if (!(this.m_MonthCalendarHeaderStartColor != value))
                    return;
                this.m_MonthCalendarHeaderStartColor = value;
            }
        }

        public Color MonthCalendarHeaderEndColor
        {
            get => this.m_MonthCalendarHeaderEndColor;
            set
            {
                if (!(this.m_MonthCalendarHeaderEndColor != value))
                    return;
                this.m_MonthCalendarHeaderEndColor = value;
            }
        }

        public Color MonthCalendarBackgroundColor
        {
            get => this.m_MonthCalendarBackgroundColor;
            set
            {
                if (!(this.m_MonthCalendarBackgroundColor != value))
                    return;
                this.m_MonthCalendarBackgroundColor = value;
            }
        }

        public Color MonthCalendarForeColor
        {
            get => this.m_MonthCalendarForeColor;
            set
            {
                if (!(this.m_MonthCalendarForeColor != value))
                    return;
                this.m_MonthCalendarForeColor = value;
            }
        }

        public Color GroupBarBorderColor
        {
            get => this.m_GroupBarBorderColor;
            set
            {
                if (!(this.m_GroupBarBorderColor != value))
                    return;
                this.m_GroupBarBorderColor = value;
            }
        }

        public Color GroupBarItemColorDark
        {
            get => this.m_GroupBarItemColorDark;
            set
            {
                if (!(this.m_GroupBarItemColorDark != value))
                    return;
                this.m_GroupBarItemColorDark = value;
            }
        }

        public Color GroupBarItemColorLight
        {
            get => this.m_GroupBarItemColorLight;
            set
            {
                if (!(this.m_GroupBarItemColorLight != value))
                    return;
                this.m_GroupBarItemColorLight = value;
            }
        }

        public Color GroupBarHeaderColorDark
        {
            get => this.m_GroupBarHeaderColorDark;
            set
            {
                if (!(this.m_GroupBarHeaderColorDark != value))
                    return;
                this.m_GroupBarHeaderColorDark = value;
            }
        }

        public Color GroupBarHeaderColorLight
        {
            get => this.m_GroupBarHeaderColorLight;
            set
            {
                if (!(this.m_GroupBarHeaderColorLight != value))
                    return;
                this.m_GroupBarHeaderColorLight = value;
            }
        }

        public Color GroupBarHighlightColorLight
        {
            get => this.m_GroupBarHighlightColorLight;
            set
            {
                if (!(this.m_GroupBarHighlightColorLight != value))
                    return;
                this.m_GroupBarHighlightColorLight = value;
            }
        }

        public Color GroupBarHighlightColorDark
        {
            get => this.m_GroupBarHighlightColorDark;
            set
            {
                if (!(this.m_GroupBarHighlightColorDark != value))
                    return;
                this.m_GroupBarHighlightColorDark = value;
            }
        }

        public Color GroupBarSelectedColorDark
        {
            get => this.m_GroupBarSelectedColorDark;
            set
            {
                if (!(this.m_GroupBarSelectedColorDark != value))
                    return;
                this.m_GroupBarSelectedColorDark = value;
            }
        }

        public Color GroupBarSelectedColorLight
        {
            get => this.m_GroupBarSelectedColorLight;
            set
            {
                if (!(this.m_GroupBarSelectedColorLight != value))
                    return;
                this.m_GroupBarSelectedColorLight = value;
            }
        }

        public Color GroupBarSelectedTopColorDark
        {
            get => this.m_GroupBarSelectedTopColorDark;
            set
            {
                if (!(this.m_GroupBarSelectedTopColorDark != value))
                    return;
                this.m_GroupBarSelectedTopColorDark = value;
            }
        }

        public Color GroupBarSelectedTopColorLight
        {
            get => this.m_GroupBarSelectedTopColorLight;
            set
            {
                if (!(this.m_GroupBarSelectedTopColorLight != value))
                    return;
                this.m_GroupBarSelectedTopColorLight = value;
            }
        }

        public Color GroupBarSelectedHighlightColorDark
        {
            get => this.m_GroupBarSelectedHighlightColorDark;
            set
            {
                if (!(this.m_GroupBarSelectedHighlightColorDark != value))
                    return;
                this.m_GroupBarSelectedHighlightColorDark = value;
            }
        }

        public Color GroupBarSelectedHighlightColorLight
        {
            get => this.m_GroupBarSelectedHighlightColorLight;
            set
            {
                if (!(this.m_GroupBarSelectedHighlightColorLight != value))
                    return;
                this.m_GroupBarSelectedHighlightColorLight = value;
            }
        }

        public Color GroupBarItemTextColor
        {
            get => this.m_GroupBarItemTextColor;
            set
            {
                if (!(this.m_GroupBarItemTextColor != value))
                    return;
                this.m_GroupBarItemTextColor = value;
            }
        }

        public Color GroupBarBackColor
        {
            get => this.m_GroupBarBackColor;
            set
            {
                if (!(this.m_GroupBarBackColor != value))
                    return;
                this.m_GroupBarBackColor = value;
            }
        }

        public Color GroupBarHeaderTextColor
        {
            get => this.m_GroupBarHeaderTextColor;
            set
            {
                if (!(this.m_GroupBarHeaderTextColor != value))
                    return;
                this.m_GroupBarHeaderTextColor = value;
            }
        }

        public Color GroupBarSplitterColorDark
        {
            get => this.m_GroupBarSplitterColorDark;
            set
            {
                if (!(this.m_GroupBarSplitterColorDark != value))
                    return;
                this.m_GroupBarSplitterColorDark = value;
            }
        }

        public Color GroupBarSplitterColorLight
        {
            get => this.m_GroupBarSplitterColorLight;
            set
            {
                if (!(this.m_GroupBarSplitterColorLight != value))
                    return;
                this.m_GroupBarSplitterColorLight = value;
            }
        }

        public Color GroupBarClientAreaBackground
        {
            get => this.m_GroupBarClientAreaBackground;
            set => this.m_GroupBarClientAreaBackground = value;
        }

        public Color XPTaskPaneInternalBorderColor
        {
            get => this.m_XPTaskPaneInternalBorderColor;
            set
            {
                if (!(this.m_XPTaskPaneInternalBorderColor != value))
                    return;
                this.m_XPTaskPaneInternalBorderColor = value;
            }
        }

        public Color XPTaskPaneBorderColor
        {
            get => this.m_XPTaskPaneBorderColor;
            set
            {
                if (!(this.m_XPTaskPaneBorderColor != value))
                    return;
                this.m_XPTaskPaneBorderColor = value;
            }
        }

        public Color XPTaskPageBackColor
        {
            get => this.m_XPTaskPageBackColor;
            set
            {
                if (!(this.m_XPTaskPageBackColor != value))
                    return;
                this.m_XPTaskPageBackColor = value;
            }
        }

        public Color MenuBorderColor
        {
            get => this.m_MenuBorderColor;
            set
            {
                if (!(this.m_MenuBorderColor != value))
                    return;
                this.m_MenuBorderColor = value;
            }
        }

        public Color MenuSeparatorColor
        {
            get => this.m_MenuSeparatorColor;
            set
            {
                if (!(this.m_MenuSeparatorColor != value))
                    return;
                this.m_MenuSeparatorColor = value;
            }
        }

        public Color MenuItemDarkColor
        {
            get => this.m_MenuItemDarkColor;
            set
            {
                if (!(this.m_MenuItemDarkColor != value))
                    return;
                this.m_MenuItemDarkColor = value;
            }
        }

        public Color MenuItemLightColor
        {
            get => this.m_MenuItemLightColor;
            set
            {
                if (!(this.m_MenuItemLightColor != value))
                    return;
                this.m_MenuItemLightColor = value;
            }
        }

        public Color MenuItemBorderColor
        {
            get => this.m_MenuItemBorderColor;
            set
            {
                if (!(this.m_MenuItemBorderColor != value))
                    return;
                this.m_MenuItemBorderColor = value;
            }
        }

        public Color MenuColumnColor
        {
            get => this.m_MenuColumnColor;
            set
            {
                if (!(this.m_MenuColumnColor != value))
                    return;
                this.m_MenuColumnColor = value;
            }
        }

        public Color MenuColumnSeparatorColor
        {
            get => this.m_MenuColumnSeparatorColor;
            set
            {
                if (!(this.m_MenuColumnSeparatorColor != value))
                    return;
                this.m_MenuColumnSeparatorColor = value;
            }
        }

        public Color MenuItemArrowLightColor
        {
            get => this.m_MenuItemArrowLightColor;
            set
            {
                if (!(this.m_MenuItemArrowLightColor != value))
                    return;
                this.m_MenuItemArrowLightColor = value;
            }
        }

        public Color MenuItemArrowDarkColor
        {
            get => this.m_MenuItemArrowDarkColor;
            set
            {
                if (!(this.m_MenuItemArrowDarkColor != value))
                    return;
                this.m_MenuItemArrowDarkColor = value;
            }
        }

        public Color MenuCheckedColor
        {
            get => this.m_MenuCheckedColor;
            set
            {
                if (!(this.m_MenuCheckedColor != value))
                    return;
                this.m_MenuCheckedColor = value;
            }
        }

        public Color MenuCheckedFillColor
        {
            get => this.m_MenuCheckedFillColor;
            set
            {
                if (!(this.m_MenuCheckedFillColor != value))
                    return;
                this.m_MenuCheckedFillColor = value;
            }
        }

        public Color MenuCheckedBorderColor
        {
            get => this.m_MenuCheckedBorderColor;
            set
            {
                if (!(this.m_MenuCheckedBorderColor != value))
                    return;
                this.m_MenuCheckedBorderColor = value;
            }
        }

        public Color MenuTextBoxBorderColor
        {
            get => this.m_MenuTextBoxBorderColor;
            set
            {
                if (!(this.m_MenuTextBoxBorderColor != value))
                    return;
                this.m_MenuTextBoxBorderColor = value;
            }
        }

        public Color MenuTextBoxBackColor
        {
            get => this.m_MenuTextBoxBackColor;
            set
            {
                if (!(this.m_MenuTextBoxBackColor != value))
                    return;
                this.m_MenuTextBoxBackColor = value;
            }
        }

        public Color MenuComboButtonPushed1Color
        {
            get => this.m_MenuComboButtonPushed1Color;
            set
            {
                if (!(this.m_MenuComboButtonPushed1Color != value))
                    return;
                this.m_MenuComboButtonPushed1Color = value;
            }
        }

        public Color MenuComboButtonPushed2Color
        {
            get => this.m_MenuComboButtonPushed2Color;
            set
            {
                if (!(this.m_MenuComboButtonPushed2Color != value))
                    return;
                this.m_MenuComboButtonPushed2Color = value;
            }
        }

        public Color MenuComboButtonPushed3Color
        {
            get => this.m_MenuComboButtonPushed3Color;
            set
            {
                if (!(this.m_MenuComboButtonPushed3Color != value))
                    return;
                this.m_MenuComboButtonPushed3Color = value;
            }
        }

        public Color MenuComboButtonPushed4Color
        {
            get => this.m_MenuComboButtonPushed4Color;
            set
            {
                if (!(this.m_MenuComboButtonPushed4Color != value))
                    return;
                this.m_MenuComboButtonPushed4Color = value;
            }
        }

        public Color MenuComboButtonHighlightLightColor
        {
            get => this.m_MenuComboButtonHighlightLightColor;
            set
            {
                if (!(this.m_MenuComboButtonHighlightLightColor != value))
                    return;
                this.m_MenuComboButtonHighlightLightColor = value;
            }
        }

        public Color MenuComboButtonHighlightDarkColor
        {
            get => this.m_MenuComboButtonHighlightDarkColor;
            set
            {
                if (!(this.m_MenuComboButtonHighlightDarkColor != value))
                    return;
                this.m_MenuComboButtonHighlightDarkColor = value;
            }
        }

        public Color MenuComboButtonArrowColor
        {
            get => this.m_MenuComboButtonArrowColor;
            set
            {
                if (!(this.m_MenuComboButtonArrowColor != value))
                    return;
                this.m_MenuComboButtonArrowColor = value;
            }
        }

        public Color MenuBackground
        {
            get => this.m_MenuBackground;
            set
            {
                if (!(this.m_MenuBackground != value))
                    return;
                this.m_MenuBackground = value;
            }
        }

        public Color DropDownLightColor
        {
            get => this.m_DropDownLightColor;
            set
            {
                if (!(this.m_DropDownLightColor != value))
                    return;
                this.m_DropDownLightColor = value;
            }
        }

        public Color DropDownDarkColor
        {
            get => this.m_DropDownDarkColor;
            set
            {
                if (!(this.m_DropDownDarkColor != value))
                    return;
                this.m_DropDownDarkColor = value;
            }
        }

        public Color CommandBarDarkColor
        {
            get => this.m_CommandBarDarkColor;
            set
            {
                if (!(this.m_CommandBarDarkColor != value))
                    return;
                this.m_CommandBarDarkColor = value;
            }
        }

        public Color CommandBarLightColor
        {
            get => this.m_CommandBarLightColor;
            set
            {
                if (!(this.m_CommandBarLightColor != value))
                    return;
                this.m_CommandBarLightColor = value;
            }
        }

        public Color CommandBarBorderColor
        {
            get => this.m_CommandBarBorderColor;
            set
            {
                if (!(this.m_CommandBarBorderColor != value))
                    return;
                this.m_CommandBarBorderColor = value;
            }
        }

        public Color DockBarBackColor
        {
            get => this.m_DockBarBackColor;
            set
            {
                if (!(this.m_DockBarBackColor != value))
                    return;
                this.m_DockBarBackColor = value;
            }
        }

        public Color DropDownHighlightLightColor
        {
            get => this.m_DropDownHighlightLightColor;
            set
            {
                if (!(this.m_DropDownHighlightLightColor != value))
                    return;
                this.m_DropDownHighlightLightColor = value;
            }
        }

        public Color DropDownHighlightDarkColor
        {
            get => this.m_DropDownHighlightDarkColor;
            set
            {
                if (!(this.m_DropDownHighlightDarkColor != value))
                    return;
                this.m_DropDownHighlightDarkColor = value;
            }
        }

        public Color DropDownPressedLightColor
        {
            get => this.m_DropDownPressedLightColor;
            set
            {
                if (!(this.m_DropDownPressedLightColor != value))
                    return;
                this.m_DropDownPressedLightColor = value;
            }
        }

        public Color DropDownPressedDarkColor
        {
            get => this.m_DropDownPressedDarkColor;
            set
            {
                if (!(this.m_DropDownPressedDarkColor != value))
                    return;
                this.m_DropDownPressedDarkColor = value;
            }
        }

        public Color FloatHighlightButtonColor
        {
            get => this.m_FloatHighlightButtonColor;
            set
            {
                if (!(this.m_FloatHighlightButtonColor != value))
                    return;
                this.m_FloatHighlightButtonColor = value;
            }
        }

        public Color FloatHighlightButtonBorderColor
        {
            get => this.m_FloatHighlightButtonBorderColor;
            set
            {
                if (!(this.m_FloatHighlightButtonBorderColor != value))
                    return;
                this.m_FloatHighlightButtonBorderColor = value;
            }
        }

        public Color FloatPressButtonColor
        {
            get => this.m_FloatPressButtonColor;
            set
            {
                if (!(this.m_FloatPressButtonColor != value))
                    return;
                this.m_FloatPressButtonColor = value;
            }
        }

        public Color FloatPressButtonBorderColor
        {
            get => this.m_FloatPressButtonBorderColor;
            set
            {
                if (!(this.m_FloatPressButtonBorderColor != value))
                    return;
                this.m_FloatPressButtonBorderColor = value;
            }
        }

        public Color FloatPressCloseButtonBorderColor
        {
            get => this.m_FloatPressCloseButtonBorderColor;
            set
            {
                if (!(this.m_FloatPressCloseButtonBorderColor != value))
                    return;
                this.m_FloatPressCloseButtonBorderColor = value;
            }
        }

        public Color FloatPressCloseButtonColor
        {
            get => this.m_FloatPressCloseButtonColor;
            set
            {
                if (!(this.m_FloatPressCloseButtonColor != value))
                    return;
                this.m_FloatPressCloseButtonColor = value;
            }
        }

        public Color FloatCommandBarLightColor
        {
            get => this.m_FloatCommandBarLightColor;
            set
            {
                if (!(this.m_FloatCommandBarLightColor != value))
                    return;
                this.m_FloatCommandBarLightColor = value;
            }
        }

        public Color FloatCommandBarDarkColor
        {
            get => this.m_FloatCommandBarDarkColor;
            set
            {
                if (!(this.m_FloatCommandBarDarkColor != value))
                    return;
                this.m_FloatCommandBarDarkColor = value;
            }
        }

        public Color FloatLightBorderColor
        {
            get => this.m_FloatLightBorderColor;
            set
            {
                if (!(this.m_FloatLightBorderColor != value))
                    return;
                this.m_FloatLightBorderColor = value;
            }
        }

        public Color FloatBackgroundColor
        {
            get => this.m_FloatBackgroundColor;
            set
            {
                if (!(this.m_FloatBackgroundColor != value))
                    return;
                this.m_FloatBackgroundColor = value;
            }
        }

        public Color FloatBorderColor
        {
            get => this.m_FloatBorderColor;
            set
            {
                if (!(this.m_FloatBorderColor != value))
                    return;
                this.m_FloatBorderColor = value;
            }
        }

        public Color FloatCaptionColor
        {
            get => this.m_FloatCaptionColor;
            set
            {
                if (!(this.m_FloatCaptionColor != value))
                    return;
                this.m_FloatCaptionColor = value;
            }
        }

        public Color BarItemSeparatorColor
        {
            get => this.m_BarItemSeparatorColor;
            set
            {
                if (!(this.m_BarItemSeparatorColor != value))
                    return;
                this.m_BarItemSeparatorColor = value;
            }
        }

        public Color BarItemPressBorderColor
        {
            get => this.m_BarItemPressBorderColor;
            set
            {
                if (!(this.m_BarItemPressBorderColor != value))
                    return;
                this.m_BarItemPressBorderColor = value;
            }
        }

        public Color BarItemHighlightBorderColor
        {
            get => this.m_BarItemHighlightBorderColor;
            set
            {
                if (!(this.m_BarItemHighlightBorderColor != value))
                    return;
                this.m_BarItemHighlightBorderColor = value;
            }
        }

        public Color BarItemPressLightColor
        {
            get => this.m_BarItemPressLightColor;
            set
            {
                if (!(this.m_BarItemPressLightColor != value))
                    return;
                this.m_BarItemPressLightColor = value;
            }
        }

        public Color BarItemPressDarkColor
        {
            get => this.m_BarItemPressDarkColor;
            set
            {
                if (!(this.m_BarItemPressDarkColor != value))
                    return;
                this.m_BarItemPressDarkColor = value;
            }
        }

        public Color DropDownBarItemLightColor
        {
            get => this.m_DropDownBarItemLightColor;
            set
            {
                if (!(this.m_DropDownBarItemLightColor != value))
                    return;
                this.m_DropDownBarItemLightColor = value;
            }
        }

        public Color DropDownBarItemDarkColor
        {
            get => this.m_DropDownBarItemDarkColor;
            set
            {
                if (!(this.m_DropDownBarItemDarkColor != value))
                    return;
                this.m_DropDownBarItemDarkColor = value;
            }
        }

        public Color DropDownBarItemBorderColor
        {
            get => this.m_DropDownBarItemBorderColor;
            set
            {
                if (!(this.m_DropDownBarItemBorderColor != value))
                    return;
                this.m_DropDownBarItemBorderColor = value;
            }
        }

        public Color BarItemCheckLightColor
        {
            get => this.m_BarItemCheckLightColor;
            set
            {
                if (!(this.m_BarItemCheckLightColor != value))
                    return;
                this.m_BarItemCheckLightColor = value;
            }
        }

        public Color BarItemCheckDarkColor
        {
            get => this.m_BarItemCheckDarkColor;
            set
            {
                if (!(this.m_BarItemCheckDarkColor != value))
                    return;
                this.m_BarItemCheckDarkColor = value;
            }
        }

        public Color BarItemCheckBorderColor
        {
            get => this.m_BarItemCheckBorderColor;
            set
            {
                if (!(this.m_BarItemCheckBorderColor != value))
                    return;
                this.m_BarItemCheckBorderColor = value;
            }
        }

        public Color BarItemCheckFlashColor
        {
            get => this.m_BarItemCheckFlashColor;
            set
            {
                if (!(this.m_BarItemCheckFlashColor != value))
                    return;
                this.m_BarItemCheckFlashColor = value;
            }
        }

        public Color BarItemPressFlashColor
        {
            get => this.m_BarItemPressFlashColor;
            set
            {
                if (!(this.m_BarItemPressFlashColor != value))
                    return;
                this.m_BarItemPressFlashColor = value;
            }
        }

        public Color BarItemSelectFlashColor
        {
            get => this.m_BarItemSelectFlashColor;
            set
            {
                if (!(this.m_BarItemSelectFlashColor != value))
                    return;
                this.m_BarItemSelectFlashColor = value;
            }
        }

        public Color TextBarItemBackColor
        {
            get => this.m_TextBarItemBackColor;
            set
            {
                if (!(this.m_TextBarItemBackColor != value))
                    return;
                this.m_TextBarItemBackColor = value;
            }
        }

        public Color TextBarItemBorderColor
        {
            get => this.m_TextBarItemBorderColor;
            set
            {
                if (!(this.m_TextBarItemBorderColor != value))
                    return;
                this.m_TextBarItemBorderColor = value;
            }
        }

        public Color TextBarItemBorderHighlightColor
        {
            get => this.m_TextBarItemBorderHighlightColor;
            set
            {
                if (!(this.m_TextBarItemBorderHighlightColor != value))
                    return;
                this.m_TextBarItemBorderHighlightColor = value;
            }
        }

        public Color ComboButtonLightColor
        {
            get => this.m_ComboButtonLightColor;
            set
            {
                if (!(this.m_ComboButtonLightColor != value))
                    return;
                this.m_ComboButtonLightColor = value;
            }
        }

        public Color ComboButtonDarkColor
        {
            get => this.m_ComboButtonDarkColor;
            set
            {
                if (!(this.m_ComboButtonDarkColor != value))
                    return;
                this.m_ComboButtonDarkColor = value;
            }
        }

        public Color ComboButtonPressLightColor
        {
            get => this.m_ComboButtonPressLightColor;
            set
            {
                if (!(this.m_ComboButtonPressLightColor != value))
                    return;
                this.m_ComboButtonPressLightColor = value;
            }
        }

        public Color ComboButtonPressDarkColor
        {
            get => this.m_ComboButtonPressDarkColor;
            set
            {
                if (!(this.m_ComboButtonPressDarkColor != value))
                    return;
                this.m_ComboButtonPressDarkColor = value;
            }
        }

        public Color ComboButtonHighlightLightColor
        {
            get => this.m_ComboButtonHighlightLightColor;
            set
            {
                if (!(this.m_ComboButtonHighlightLightColor != value))
                    return;
                this.m_ComboButtonHighlightLightColor = value;
            }
        }

        public Color ComboButtonHighlightDarkColor
        {
            get => this.m_ComboButtonHighlightDarkColor;
            set
            {
                if (!(this.m_ComboButtonHighlightDarkColor != value))
                    return;
                this.m_ComboButtonHighlightDarkColor = value;
            }
        }

        public Color ComboButtonBorder
        {
            get => this.m_ComboButtonBorder;
            set
            {
                if (!(this.m_ComboButtonBorder != value))
                    return;
                this.m_ComboButtonBorder = value;
            }
        }

        public Color ComboButtonPressBorder
        {
            get => this.m_ComboButtonPressBorder;
            set
            {
                if (!(this.m_ComboButtonPressBorder != value))
                    return;
                this.m_ComboButtonPressBorder = value;
            }
        }

        public Color ComboButtonHighlightBorder
        {
            get => this.m_ComboButtonHighlightBorder;
            set
            {
                if (!(this.m_ComboButtonHighlightBorder != value))
                    return;
                this.m_ComboButtonHighlightBorder = value;
            }
        }

        public Color ButtonPressedTopColor => this.m_ButtonPressedTopColor;

        public Color ButtonPressedBottomColor => this.m_ButtonPressedBottomColor;

        public Color ButtonSelectedTopColor => this.m_ButtonSelectedTopColor;

        public Color ButtonSelectedBottomColor => this.m_ButtonSelectedBottomColor;

        public Color ButtonDisabledTopColor => this.m_ButtonDisabledTopColor;

        public Color ButtonDisabledBottomColor => this.m_ButtonDisabledBottomColor;

        public Color ButtonPressedBorderColor => this.m_ButtonPressedBorderColor;

        public Color ButtonSelectedBorderColor => this.m_ButtonSelectedBorderColor;

        public Color ButtonDisabledBorderColor => this.m_ButtonDisabledBorderColor;

        public Color ButtonDefaultTopColor
        {
            get
            {
                int num = this.m_ButtonDefaultTopColor != Color.Empty ? 1 : 0;
                return this.m_ButtonDefaultTopColor;
            }
        }

        public Color ButtonDefaultBottomColor
        {
            get
            {
                int num = this.m_ButtonDefaultBottomColor != Color.Empty ? 1 : 0;
                return this.m_ButtonDefaultBottomColor;
            }
        }

        public Color ButtonDefaultBorderColor
        {
            get
            {
                int num = this.m_ButtonDefaultBorderColor != Color.Empty ? 1 : 0;
                return this.m_ButtonDefaultBorderColor;
            }
        }

        public Color ButtonDefaultInternalBorderColor
        {
            get
            {
                int num = this.m_ButtonDefaultInternalBorderColor != Color.Empty ? 1 : 0;
                return this.m_ButtonDefaultInternalBorderColor;
            }
        }

        public Color ButtonPressedInternalBorderColor
        {
            get
            {
                int num = this.m_ButtonPressedInternalBorderColor != Color.Empty ? 1 : 0;
                return this.m_ButtonPressedInternalBorderColor;
            }
        }

        public Color ButtonSelectedInternalBorderColor
        {
            get
            {
                int num = this.m_ButtonSelectedInternalBorderColor != Color.Empty ? 1 : 0;
                return this.m_ButtonSelectedInternalBorderColor;
            }
        }

        [Obsolete]
        public Color BlueButtonDefaultTopColor
        {
            get
            {
                if (this.m_BlueButtonDefaultTopColor != Color.Empty)
                    return this.m_BlueButtonDefaultTopColor;
                this.m_BlueButtonDefaultTopColor = Color.FromArgb(231, 242, (int)byte.MaxValue);
                return this.m_BlueButtonDefaultTopColor;
            }
        }

        [Obsolete]
        public Color BlueButtonDefaultBottomColor
        {
            get
            {
                if (this.m_BlueButtonDefaultBottomColor != Color.Empty)
                    return this.m_BlueButtonDefaultBottomColor;
                this.m_BlueButtonDefaultBottomColor = Color.FromArgb(179, 209, 252);
                return this.m_BlueButtonDefaultBottomColor;
            }
        }

        [Obsolete]
        public Color BlueButtonDefaultBorderColor
        {
            get
            {
                if (this.m_BlueButtonDefaultBorderColor != Color.Empty)
                    return this.m_BlueButtonDefaultBorderColor;
                this.m_BlueButtonDefaultBorderColor = Color.FromArgb(176, 208, (int)byte.MaxValue);
                return this.m_BlueButtonDefaultBorderColor;
            }
        }

        [Obsolete]
        public Color BlueButtonDefaultInternalBorderColor
        {
            get
            {
                if (this.m_BlueButtonDefaultInternalBorderColor != Color.Empty)
                    return this.m_BlueButtonDefaultInternalBorderColor;
                this.m_BlueButtonDefaultInternalBorderColor = Color.FromArgb(128, Color.FromArgb(250, 251, (int)byte.MaxValue));
                return this.m_BlueButtonDefaultInternalBorderColor;
            }
        }

        [Obsolete]
        public Color BlueButtonPressedInternalBorderColor
        {
            get
            {
                if (this.m_BlueButtonPressedInternalBorderColor != Color.Empty)
                    return this.m_BlueButtonPressedInternalBorderColor;
                this.m_BlueButtonPressedInternalBorderColor = Color.FromArgb(70, Color.FromArgb(176, 132, 92));
                return this.m_BlueButtonPressedInternalBorderColor;
            }
        }

        [Obsolete]
        public Color BlueButtonSelectedInternalBorderColor
        {
            get
            {
                if (this.m_BlueButtonSelectedInternalBorderColor != Color.Empty)
                    return this.m_BlueButtonSelectedInternalBorderColor;
                this.m_BlueButtonSelectedInternalBorderColor = Color.FromArgb(128, Color.FromArgb(250, 251, (int)byte.MaxValue));
                return this.m_BlueButtonSelectedInternalBorderColor;
            }
        }

        [Obsolete]
        public Color SilverButtonDefaultTopColor
        {
            get
            {
                if (this.m_SilverButtonDefaultTopColor != Color.Empty)
                    return this.m_SilverButtonDefaultTopColor;
                this.m_SilverButtonDefaultTopColor = Color.FromArgb(223, 227, 231);
                return this.m_SilverButtonDefaultTopColor;
            }
        }

        [Obsolete]
        public Color SilverButtonDefaultBottomColor
        {
            get
            {
                if (this.m_SilverButtonDefaultBottomColor != Color.Empty)
                    return this.m_SilverButtonDefaultBottomColor;
                this.m_SilverButtonDefaultBottomColor = Color.FromArgb(208, 212, 221);
                return this.m_SilverButtonDefaultBottomColor;
            }
        }

        [Obsolete]
        public Color SilverButtonDefaultBorderColor
        {
            get
            {
                if (this.m_SilverButtonDefaultBorderColor != Color.Empty)
                    return this.m_SilverButtonDefaultBorderColor;
                this.m_SilverButtonDefaultBorderColor = Color.FromArgb(208, 212, 221);
                return this.m_SilverButtonDefaultBorderColor;
            }
        }

        [Obsolete]
        public Color SilverButtonDefaultInternalBorderColor
        {
            get
            {
                if (this.m_SilverButtonDefaultInternalBorderColor != Color.Empty)
                    return this.m_SilverButtonDefaultInternalBorderColor;
                this.m_SilverButtonDefaultInternalBorderColor = Color.FromArgb(128, Color.FromArgb(250, 251, (int)byte.MaxValue));
                return this.m_SilverButtonDefaultInternalBorderColor;
            }
        }

        [Obsolete]
        public Color SilverButtonPressedInternalBorderColor
        {
            get
            {
                if (this.m_SilverButtonPressedInternalBorderColor != Color.Empty)
                    return this.m_SilverButtonPressedInternalBorderColor;
                this.m_SilverButtonPressedInternalBorderColor = Color.FromArgb(70, Color.FromArgb(176, 132, 92));
                return this.m_SilverButtonPressedInternalBorderColor;
            }
        }

        [Obsolete]
        public Color SilverButtonSelectedInternalBorderColor
        {
            get
            {
                if (this.m_SilverButtonSelectedInternalBorderColor != Color.Empty)
                    return this.m_SilverButtonSelectedInternalBorderColor;
                this.m_SilverButtonSelectedInternalBorderColor = Color.FromArgb(128, Color.FromArgb(250, 251, (int)byte.MaxValue));
                return this.m_SilverButtonSelectedInternalBorderColor;
            }
        }

        [Obsolete]
        public Color BlackButtonDefaultTopColor
        {
            get
            {
                if (this.m_BlackButtonDefaultTopColor != Color.Empty)
                    return this.m_BlackButtonDefaultTopColor;
                this.m_BlackButtonDefaultTopColor = Color.FromArgb(146, 146, 146);
                return this.m_BlackButtonDefaultTopColor;
            }
        }

        [Obsolete]
        public Color BlackButtonDefaultBottomColor
        {
            get
            {
                if (this.m_BlackButtonDefaultBottomColor != Color.Empty)
                    return this.m_BlackButtonDefaultBottomColor;
                this.m_BlackButtonDefaultBottomColor = Color.FromArgb(83, 83, 83);
                return this.m_BlackButtonDefaultBottomColor;
            }
        }

        [Obsolete]
        public Color BlackButtonDefaultBorderColor
        {
            get
            {
                if (this.m_BlackButtonDefaultBorderColor != Color.Empty)
                    return this.m_BlackButtonDefaultBorderColor;
                this.m_BlackButtonDefaultBorderColor = Color.FromArgb(153, 153, 153);
                return this.m_BlackButtonDefaultBorderColor;
            }
        }

        [Obsolete]
        public Color BlackButtonDefaultInternalBorderColor
        {
            get
            {
                if (this.m_BlackButtonDefaultInternalBorderColor != Color.Empty)
                    return this.m_BlackButtonDefaultInternalBorderColor;
                this.m_BlackButtonDefaultInternalBorderColor = Color.FromArgb(128, Color.FromArgb(250, 251, (int)byte.MaxValue));
                return this.m_BlackButtonDefaultInternalBorderColor;
            }
        }

        [Obsolete]
        public Color BlackButtonPressedInternalBorderColor
        {
            get
            {
                if (this.m_BlackButtonPressedInternalBorderColor != Color.Empty)
                    return this.m_BlackButtonPressedInternalBorderColor;
                this.m_BlackButtonPressedInternalBorderColor = Color.FromArgb(70, Color.FromArgb(176, 132, 92));
                return this.m_BlackButtonPressedInternalBorderColor;
            }
        }

        [Obsolete]
        public Color BlackButtonSelectedInternalBorderColor
        {
            get
            {
                if (this.m_BlackButtonSelectedInternalBorderColor != Color.Empty)
                    return this.m_BlackButtonSelectedInternalBorderColor;
                this.m_BlackButtonSelectedInternalBorderColor = Color.FromArgb(128, Color.FromArgb(250, 251, (int)byte.MaxValue));
                return this.m_BlackButtonSelectedInternalBorderColor;
            }
        }

        public Color TabDefaultBorderColor => this.m_TabDefaultBorderColor;

        public Color TabHotLightBottomBorderLineColor => this.m_TabHotLightBottomBorderLineColor;

        public Color TabHotLightGradientTopBeginColor => this.m_TabHotLightGradientTopBeginColor;

        public Color TabHotLightGradientTopEndColor => this.m_TabHotLightGradientTopEndColor;

        public Color TabHotLightGradientBottomBeginColor => this.m_TabHotLightGradientBottomBeginColor;

        public Color TabHotLightGradientBottomEndColor => this.m_TabHotLightGradientBottomEndColor;

        public Color TabHotLightGradientCircleColor => this.m_TabHotLightGradientCircleColor;

        public Color TabSelectedGradientTopColor => this.m_TabSelectedGradientTopColor;

        public Color TabSelectedGradientBottomColor => this.m_TabSelectedGradientBottomColor;

        public Color TabSelectedInnerBorderColor => this.m_TabSelectedInnerBorderColor;

        public Color TabHighlightInnerBorderColor => this.m_TabHighlightInnerBorderColor;

        public Color TabSelectedHotLightBorderColor => this.m_TabSelectedHotLightBorderColor;

        public Color TabSelectedHotLightInnerBorderColor => this.m_TabSelectedHotLightInnerBorderColor;

        public Color TabForeColor => this.m_TabForeColor;

        public Color ActiveTabForeColor => this.m_ActiveTabForeColor;

        public Color TabBackgroundColor => this.m_TabBackgroundColor;

        public Color TabScrollArrowColor => this.m_TabScrollArrowColor;

        public Color DockTabForeColor => this.m_DockTabForeColor;

        public Color DockTabBackgroundColor => this.m_DockTabBackgroundColor;

        public Color ActiveFormBorderColor => this.m_ActiveFormBorderColor;

        public Color SelectedNodeBackground => this.m_SelectedNodeBackground;

        public Color TreeviewBackColor => this.m_TreeviewBackColor;

        public Color TreeNodeArrowColor => this.m_TreeNodeArrowColor;

        public Color TreeViewFontColor => this.m_TreeViewFontColor;

        public Color InactiveFormBorderColor => this.m_InactiveFormBorderColor;

        public Color ActiveTextBoxBorderColor => this.m_ActiveTextBoxBorderColor;

        public Color InactiveTextBoxBorderColor => this.m_InactiveTextBoxBorderColor;

        public Color ActiveTextBoxBackColor => this.m_ActiveTextBoxBackColor;

        public Color InactiveTextBoxBackColor => this.m_InactiveTextBoxBackColor;

        public Color FormTextColor => this.m_FormTextColor;

        public Color ActiveTitleGradientBegin => this.m_ActiveTitleGradientBegin;

        public Color ActiveTitleGradientEnd => this.m_ActiveTitleGradientEnd;

        public Color InactiveTitleGradientBegin => this.m_InactiveTitleGradientBegin;

        public Color InactiveTitleGradientEnd => this.m_InactiveTitleGradientEnd;

        public Color SystemButtonSelectedGradientBegin => this.m_SystemButtonSelectedGradientBegin;

        public Color SystemButtonSelectedGradientEnd => this.m_SystemButtonSelectedGradientEnd;

        public Color SystemButtonPressedGradientBegin => this.m_SystemButtonPressedGradientBegin;

        public Color SystemButtonPressedGradientEnd => this.m_SystemButtonPressedGradientEnd;

        public Color SystemButtonBorderSelected => this.m_SystemButtonBorderSelected;

        public Color SystemButtonBorderPressed => this.m_SystemButtonBorderPressed;

        public Color FormBackground => this.m_FormBackground;

        public Color UpDownArrowStartColor => this.m_UpDownArrowStartColor;

        public Color UpDownArrowEndColor => this.m_UpDownArrowEndColor;

        public Color UpDownBorderNormalColor => this.m_UpDownBorderNormalColor;

        public Color UpDownBackgroundNormalColor => this.m_UpDownBackgroundNormalColor;

        public Color UpDownBackgroundNormalStartColor => this.m_UpDownBackgroundNormalStartColor;

        public Color UpDownBackgroundNormalEndColor => this.m_UpDownBackgroundNormalEndColor;

        public Color UpDownBorderHotColor => this.m_UpDownBorderHotColor;

        public Color UpDownInnerBorderHotStartColor => this.m_UpDownInnerBorderHotStartColor;

        public Color UpDownInnerBorderHotEndColor => this.m_UpDownInnerBorderHotEndColor;

        public Color UpDownBorderPressedColor => this.m_UpDownBorderPressedColor;

        public Color UpDownInnerBorderPressedStartColor => this.m_UpDownInnerBorderPressedStartColor;

        public Color UpDownInnerBorderPressedEndColor => this.m_UpDownInnerBorderPressedEndColor;

        public Color UpDownBackgroundDisabledStartColor => this.m_UpDownBackgroundDisabledStartColor;

        public Color UpDownBackgroundDisabledEndColor => this.m_UpDownBackgroundDisabledEndColor;

        public Color UpDownBorderDisabledColor => this.m_UpDownBorderDisabledColor;

        public Color UpDownBackgroundHotTopStartColor => this.m_UpDownBackgroundHotTopStartColor;

        public Color UpDownBackgroundHotTopEndColor => this.m_UpDownBackgroundHotTopEndColor;

        public Color UpDownBackgroundHotBottomStartColor => this.m_UpDownBackgroundHotBottomStartColor;

        public Color UpDownBackgroundHotBottomEndColor => this.m_UpDownBackgroundHotBottomEndColor;

        public Color UpDownBackgroundPressedTopStartColor => this.m_UpDownBackgroundPressedTopStartColor;

        public Color UpDownBackgroundPressedTopEndColor => this.m_UpDownBackgroundPressedTopEndColor;

        public Color UpDownBackgroundPressedBottomStartColor => this.m_UpDownBackgroundPressedBottomStartColor;

        public Color UpDownBackgroundPressedBottomEndColor => this.m_UpDownBackgroundPressedBottomEndColor;

        public Color ComboBoxAdvNormalBackColor
        {
            get => this.m_ComboBoxAdvNormalBackColor;
            set
            {
                if (!(this.m_ComboBoxAdvNormalBackColor != value))
                    return;
                this.m_ComboBoxAdvNormalBackColor = value;
            }
        }

        public Color ComboBoxAdvHotBackColor
        {
            get => this.m_ComboBoxAdvHotBackColor;
            set
            {
                if (!(this.m_ComboBoxAdvHotBackColor != value))
                    return;
                this.m_ComboBoxAdvHotBackColor = value;
            }
        }

        public Color ComboBoxAdvNormalBorderColor
        {
            get => this.m_ComboBoxAdvNormalBorderColor;
            set
            {
                if (!(this.m_ComboBoxAdvNormalBorderColor != value))
                    return;
                this.m_ComboBoxAdvNormalBorderColor = value;
            }
        }

        public Color ComboBoxAdvHotBorderColor
        {
            get => this.m_ComboBoxAdvHotBorderColor;
            set
            {
                if (!(this.m_ComboBoxAdvHotBorderColor != value))
                    return;
                this.m_ComboBoxAdvHotBorderColor = value;
            }
        }

        public Color ComboBoxAdvPushedBorderColor
        {
            get => this.m_ComboBoxAdvPushedBorderColor;
            set
            {
                if (!(this.m_ComboBoxAdvPushedBorderColor != value))
                    return;
                this.m_ComboBoxAdvPushedBorderColor = value;
            }
        }

        public Color ComboBoxAdvButtonUpperLineColor
        {
            get => this.m_ComboBoxAdvButtonUpperLineColor;
            set
            {
                if (!(this.m_ComboBoxAdvButtonUpperLineColor != value))
                    return;
                this.m_ComboBoxAdvButtonUpperLineColor = value;
            }
        }

        public Color ComboBoxAdvArrowColor
        {
            get => this.m_ComboBoxAdvArrowColor;
            set
            {
                if (!(this.m_ComboBoxAdvArrowColor != value))
                    return;
                this.m_ComboBoxAdvArrowColor = value;
            }
        }

        public Color ComboBoxAdvLowerArrowLineColor
        {
            get => this.m_ComboBoxAdvLowerArrowLineColor;
            set
            {
                if (!(this.m_ComboBoxAdvLowerArrowLineColor != value))
                    return;
                this.m_ComboBoxAdvLowerArrowLineColor = value;
            }
        }

        public Color ComboBoxAdvHotBackgroundButtonColor1
        {
            get => this.m_ComboBoxAdvHotBackgroundButtonColor1;
            set
            {
                if (!(this.m_ComboBoxAdvHotBackgroundButtonColor1 != value))
                    return;
                this.m_ComboBoxAdvHotBackgroundButtonColor1 = value;
            }
        }

        public Color ComboBoxAdvHotBackgroundButtonColor2
        {
            get => this.m_ComboBoxAdvHotBackgroundButtonColor2;
            set
            {
                if (!(this.m_ComboBoxAdvHotBackgroundButtonColor2 != value))
                    return;
                this.m_ComboBoxAdvHotBackgroundButtonColor2 = value;
            }
        }

        public Color ComboBoxAdvHotBackgroundButtonColor3
        {
            get => this.m_ComboBoxAdvHotBackgroundButtonColor3;
            set
            {
                if (!(this.m_ComboBoxAdvHotBackgroundButtonColor3 != value))
                    return;
                this.m_ComboBoxAdvHotBackgroundButtonColor3 = value;
            }
        }

        public Color ComboBoxAdvHotBackgroundButtonColor4
        {
            get => this.m_ComboBoxAdvHotBackgroundButtonColor4;
            set
            {
                if (!(this.m_ComboBoxAdvHotBackgroundButtonColor4 != value))
                    return;
                this.m_ComboBoxAdvHotBackgroundButtonColor4 = value;
            }
        }

        public Color ComboBoxAdvNormalBackgroundButtonColor1
        {
            get => this.m_ComboBoxAdvNormalBackgroundButtonColor1;
            set
            {
                if (!(this.m_ComboBoxAdvNormalBackgroundButtonColor1 != value))
                    return;
                this.m_ComboBoxAdvNormalBackgroundButtonColor1 = value;
            }
        }

        public Color ComboBoxAdvNormalBackgroundButtonColor2
        {
            get => this.m_ComboBoxAdvNormalBackgroundButtonColor2;
            set
            {
                if (!(this.m_ComboBoxAdvNormalBackgroundButtonColor2 != value))
                    return;
                this.m_ComboBoxAdvNormalBackgroundButtonColor2 = value;
            }
        }

        public Color ComboBoxAdvNormalBackgroundButtonColor3
        {
            get => this.m_ComboBoxAdvNormalBackgroundButtonColor3;
            set
            {
                if (!(this.m_ComboBoxAdvNormalBackgroundButtonColor3 != value))
                    return;
                this.m_ComboBoxAdvNormalBackgroundButtonColor3 = value;
            }
        }

        public Color ComboBoxAdvNormalBackgroundButtonColor4
        {
            get => this.m_ComboBoxAdvNormalBackgroundButtonColor4;
            set
            {
                if (!(this.m_ComboBoxAdvNormalBackgroundButtonColor4 != value))
                    return;
                this.m_ComboBoxAdvNormalBackgroundButtonColor4 = value;
            }
        }

        public Color ComboBoxAdvPushedBackgroundButtonColor1
        {
            get => this.m_ComboBoxAdvPushedBackgroundButtonColor1;
            set
            {
                if (!(this.m_ComboBoxAdvPushedBackgroundButtonColor1 != value))
                    return;
                this.m_ComboBoxAdvPushedBackgroundButtonColor1 = value;
            }
        }

        public Color ComboBoxAdvPushedBackgroundButtonColor2
        {
            get => this.m_ComboBoxAdvPushedBackgroundButtonColor2;
            set
            {
                if (!(this.m_ComboBoxAdvPushedBackgroundButtonColor2 != value))
                    return;
                this.m_ComboBoxAdvPushedBackgroundButtonColor2 = value;
            }
        }

        public Color ComboBoxAdvPushedBackgroundButtonColor3
        {
            get => this.m_ComboBoxAdvPushedBackgroundButtonColor3;
            set
            {
                if (!(this.m_ComboBoxAdvPushedBackgroundButtonColor3 != value))
                    return;
                this.m_ComboBoxAdvPushedBackgroundButtonColor3 = value;
            }
        }

        public Color ComboBoxAdvPushedBackgroundButtonColor4
        {
            get => this.m_ComboBoxAdvPushedBackgroundButtonColor4;
            set
            {
                if (!(this.m_ComboBoxAdvPushedBackgroundButtonColor4 != value))
                    return;
                this.m_ComboBoxAdvPushedBackgroundButtonColor4 = value;
            }
        }

        public Color CheckBoxAdvNormalBackColor
        {
            get => this.m_CheckBoxAdvNormalBackColor;
            set
            {
                if (!(this.m_CheckBoxAdvNormalBackColor != value))
                    return;
                this.m_CheckBoxAdvNormalBackColor = value;
            }
        }

        public Color CheckBoxAdvSelectedBackColor
        {
            get => this.m_CheckBoxAdvSelectedBackColor;
            set
            {
                if (!(this.m_CheckBoxAdvSelectedBackColor != value))
                    return;
                this.m_CheckBoxAdvSelectedBackColor = value;
            }
        }

        public Color CheckBoxAdvPushedBackColor
        {
            get => this.m_CheckBoxAdvPushedBackColor;
            set
            {
                if (!(this.m_CheckBoxAdvPushedBackColor != value))
                    return;
                this.m_CheckBoxAdvPushedBackColor = value;
            }
        }

        public Color CheckBoxAdvNormalBorderColor
        {
            get => this.m_CheckBoxAdvNormalBorderColor;
            set
            {
                if (!(this.m_CheckBoxAdvNormalBorderColor != value))
                    return;
                this.m_CheckBoxAdvNormalBorderColor = value;
            }
        }

        public Color CheckBoxAdvSelectedBorderColor
        {
            get => this.m_CheckBoxAdvSelectedBorderColor;
            set
            {
                if (!(this.m_CheckBoxAdvSelectedBorderColor != value))
                    return;
                this.m_CheckBoxAdvSelectedBorderColor = value;
            }
        }

        public Color CheckBoxAdvPushedBorderColor
        {
            get => this.m_CheckBoxAdvPushedBorderColor;
            set
            {
                if (!(this.m_CheckBoxAdvPushedBorderColor != value))
                    return;
                this.m_CheckBoxAdvPushedBorderColor = value;
            }
        }

        public Color CheckBoxAdvNormalInternalBorderColor
        {
            get => this.m_CheckBoxAdvNormalInternalBorderColor;
            set
            {
                if (!(this.m_CheckBoxAdvNormalInternalBorderColor != value))
                    return;
                this.m_CheckBoxAdvNormalInternalBorderColor = value;
            }
        }

        public Color CheckBoxAdvSelectedInternalBorderColor
        {
            get => this.m_CheckBoxAdvSelectedInternalBorderColor;
            set
            {
                if (!(this.m_CheckBoxAdvSelectedInternalBorderColor != value))
                    return;
                this.m_CheckBoxAdvSelectedInternalBorderColor = value;
            }
        }

        public Color CheckBoxAdvPushedInternalBorderColor
        {
            get => this.m_CheckBoxAdvPushedInternalBorderColor;
            set
            {
                if (!(this.m_CheckBoxAdvPushedInternalBorderColor != value))
                    return;
                this.m_CheckBoxAdvPushedInternalBorderColor = value;
            }
        }

        public Color CheckBoxAdvNormalInternalRectangleBorderColor
        {
            get => this.m_CheckBoxAdvNormalInternalRectangleBorderColor;
            set
            {
                if (!(this.m_CheckBoxAdvNormalInternalRectangleBorderColor != value))
                    return;
                this.m_CheckBoxAdvNormalInternalRectangleBorderColor = value;
            }
        }

        public Color CheckBoxAdvSelectedInternalRectangleBorderColor
        {
            get => this.m_CheckBoxAdvSelectedInternalRectangleBorderColor;
            set
            {
                if (!(this.m_CheckBoxAdvSelectedInternalRectangleBorderColor != value))
                    return;
                this.m_CheckBoxAdvSelectedInternalRectangleBorderColor = value;
            }
        }

        public Color CheckBoxAdvPushedInternalRectangleBorderColor
        {
            get => this.m_CheckBoxAdvPushedInternalRectangleBorderColor;
            set
            {
                if (!(this.m_CheckBoxAdvPushedInternalRectangleBorderColor != value))
                    return;
                this.m_CheckBoxAdvPushedInternalRectangleBorderColor = value;
            }
        }

        public Color CheckBoxAdvNormalInternalRectangleColor
        {
            get => this.m_CheckBoxAdvNormalInternalRectangleColor;
            set
            {
                if (!(this.m_CheckBoxAdvNormalInternalRectangleColor != value))
                    return;
                this.m_CheckBoxAdvNormalInternalRectangleColor = value;
            }
        }

        public Color CheckBoxAdvSelectedInternalRectangleColor
        {
            get => this.m_CheckBoxAdvSelectedInternalRectangleColor;
            set
            {
                if (!(this.m_CheckBoxAdvSelectedInternalRectangleColor != value))
                    return;
                this.m_CheckBoxAdvSelectedInternalRectangleColor = value;
            }
        }

        public Color CheckBoxAdvPushedInternalRectangleColor
        {
            get => this.m_CheckBoxAdvPushedInternalRectangleColor;
            set
            {
                if (!(this.m_CheckBoxAdvPushedInternalRectangleColor != value))
                    return;
                this.m_CheckBoxAdvPushedInternalRectangleColor = value;
            }
        }

        public Color CheckBoxAdvNormalTickColor
        {
            get => this.m_CheckBoxAdvNormalTickColor;
            set
            {
                if (!(this.m_CheckBoxAdvNormalTickColor != value))
                    return;
                this.m_CheckBoxAdvNormalTickColor = value;
            }
        }

        public Color CheckBoxAdvSelectedTickColor
        {
            get => this.m_CheckBoxAdvSelectedTickColor;
            set
            {
                if (!(this.m_CheckBoxAdvSelectedTickColor != value))
                    return;
                this.m_CheckBoxAdvSelectedTickColor = value;
            }
        }

        public Color CheckBoxAdvPushedTickColor
        {
            get => this.m_CheckBoxAdvPushedTickColor;
            set
            {
                if (!(this.m_CheckBoxAdvPushedTickColor != value))
                    return;
                this.m_CheckBoxAdvPushedTickColor = value;
            }
        }

        public Color CheckBoxAdvDisabledTickColor
        {
            get => this.m_CheckBoxAdvDisabledTickColor;
            set
            {
                if (!(this.m_CheckBoxAdvDisabledTickColor != value))
                    return;
                this.m_CheckBoxAdvDisabledTickColor = value;
            }
        }

        public Color CheckBoxAdvIndeterminateRectangleColor
        {
            get => this.m_CheckBoxAdvIndeterminateRectangleColor;
            set
            {
                if (!(this.m_CheckBoxAdvIndeterminateRectangleColor != value))
                    return;
                this.m_CheckBoxAdvIndeterminateRectangleColor = value;
            }
        }

        public Color CheckBoxAdvDisabledBackColor
        {
            get => this.m_CheckBoxAdvDisabledBackColor;
            set
            {
                if (!(this.m_CheckBoxAdvDisabledBackColor != value))
                    return;
                this.m_CheckBoxAdvDisabledBackColor = value;
            }
        }

        public Color CheckBoxAdvDisabledBorderColor
        {
            get => this.m_CheckBoxAdvDisabledBorderColor;
            set
            {
                if (!(this.m_CheckBoxAdvDisabledBorderColor != value))
                    return;
                this.m_CheckBoxAdvDisabledBorderColor = value;
            }
        }

        public Color CheckBoxAdvDisabledInternalBorderColor
        {
            get => this.m_CheckBoxAdvDisabledInternalBorderColor;
            set
            {
                if (!(this.m_CheckBoxAdvDisabledInternalBorderColor != value))
                    return;
                this.m_CheckBoxAdvDisabledInternalBorderColor = value;
            }
        }

        public Color RadioButtonAdvNormalBackColor
        {
            get => this.m_RadioButtonAdvNormalBackColor;
            set
            {
                if (!(this.m_RadioButtonAdvNormalBackColor != value))
                    return;
                this.m_RadioButtonAdvNormalBackColor = value;
            }
        }

        public Color RadioButtonAdvNormalBorderColor
        {
            get => this.m_RadioButtonAdvNormalBorderColor;
            set
            {
                if (!(this.m_RadioButtonAdvNormalBorderColor != value))
                    return;
                this.m_RadioButtonAdvNormalBorderColor = value;
            }
        }

        public Color RadioButtonAdvNormalInternalBorderColor
        {
            get => this.m_RadioButtonAdvNormalInternalBorderColor;
            set
            {
                if (!(this.m_RadioButtonAdvNormalInternalBorderColor != value))
                    return;
                this.m_RadioButtonAdvNormalInternalBorderColor = value;
            }
        }

        public Color RadioButtonAdvSelectedBackColor
        {
            get => this.m_RadioButtonAdvSelectedBackColor;
            set
            {
                if (!(this.m_RadioButtonAdvSelectedBackColor != value))
                    return;
                this.m_RadioButtonAdvSelectedBackColor = value;
            }
        }

        public Color RadioButtonAdvSelectedBorderColor
        {
            get => this.m_RadioButtonAdvSelectedBorderColor;
            set
            {
                if (!(this.m_RadioButtonAdvSelectedBorderColor != value))
                    return;
                this.m_RadioButtonAdvSelectedBorderColor = value;
            }
        }

        public Color RadioButtonAdvSelectedInternalBorderColor
        {
            get => this.m_RadioButtonAdvSelectedInternalBorderColor;
            set
            {
                if (!(this.m_RadioButtonAdvSelectedInternalBorderColor != value))
                    return;
                this.m_RadioButtonAdvSelectedInternalBorderColor = value;
            }
        }

        public Color RadioButtonAdvPushedBackColor
        {
            get => this.m_RadioButtonAdvPushedBackColor;
            set
            {
                if (!(this.m_RadioButtonAdvPushedBackColor != value))
                    return;
                this.m_RadioButtonAdvPushedBackColor = value;
            }
        }

        public Color RadioButtonAdvPushedBorderColor
        {
            get => this.m_RadioButtonAdvPushedBorderColor;
            set
            {
                if (!(this.m_RadioButtonAdvPushedBorderColor != value))
                    return;
                this.m_RadioButtonAdvPushedBorderColor = value;
            }
        }

        public Color RadioButtonAdvPushedInternalBorderColor
        {
            get => this.m_RadioButtonAdvPushedInternalBorderColor;
            set
            {
                if (!(this.m_RadioButtonAdvPushedInternalBorderColor != value))
                    return;
                this.m_RadioButtonAdvPushedInternalBorderColor = value;
            }
        }

        public Color RadioButtonAdvCheckMarkBorderColor
        {
            get => this.m_RadioButtonAdvCheckMarkBorderColor;
            set
            {
                if (!(this.m_RadioButtonAdvCheckMarkBorderColor != value))
                    return;
                this.m_RadioButtonAdvCheckMarkBorderColor = value;
            }
        }

        public Color RadioButtonAdvCheckMarkNormalBottomColor
        {
            get => this.m_RadioButtonAdvCheckMarkNormalBottomColor;
            set
            {
                if (!(this.m_RadioButtonAdvCheckMarkNormalBottomColor != value))
                    return;
                this.m_RadioButtonAdvCheckMarkNormalBottomColor = value;
            }
        }

        public Color RadioButtonAdvCheckMarkSelectedBottomColor
        {
            get => this.m_RadioButtonAdvCheckMarkSelectedBottomColor;
            set
            {
                if (!(this.m_RadioButtonAdvCheckMarkSelectedBottomColor != value))
                    return;
                this.m_RadioButtonAdvCheckMarkSelectedBottomColor = value;
            }
        }

        public Color RadioButtonAdvCheckMarkPushedBottomColor
        {
            get => this.m_RadioButtonAdvCheckMarkPushedBottomColor;
            set
            {
                if (!(this.m_RadioButtonAdvCheckMarkPushedBottomColor != value))
                    return;
                this.m_RadioButtonAdvCheckMarkPushedBottomColor = value;
            }
        }

        public Color TabBarSplitterBackColor
        {
            get => this.m_TabBarSplitterBackColor;
            set
            {
                if (!(this.m_TabBarSplitterBackColor != value))
                    return;
                this.m_TabBarSplitterBackColor = value;
            }
        }

        public Color TabBarSplitterBorderColor
        {
            get => this.m_TabBarSplitterBorderColor;
            set
            {
                if (!(this.m_TabBarSplitterBorderColor != value))
                    return;
                this.m_TabBarSplitterBorderColor = value;
            }
        }

        public Color TabBarSplitterTextColor
        {
            get => this.m_TabBarSplitterTextColor;
            set
            {
                if (!(this.m_TabBarSplitterTextColor != value))
                    return;
                this.m_TabBarSplitterTextColor = value;
            }
        }

        public Color TabBarSplitterTabStartColor
        {
            get => this.m_TabBarSplitterTabStartColor;
            set
            {
                if (!(this.m_TabBarSplitterTabStartColor != value))
                    return;
                this.m_TabBarSplitterTabStartColor = value;
            }
        }

        public Color TabBarSplitterTabEndColor
        {
            get => this.m_TabBarSplitterTabEndColor;
            set
            {
                if (!(this.m_TabBarSplitterTabEndColor != value))
                    return;
                this.m_TabBarSplitterTabEndColor = value;
            }
        }

        public Color TabBarSplitterTabBarStartColor
        {
            get => this.m_TabBarSplitterTabBarStartColor;
            set
            {
                if (!(this.m_TabBarSplitterTabBarStartColor != value))
                    return;
                this.m_TabBarSplitterTabBarStartColor = value;
            }
        }

        public Color TabBarSplitterTabBarEndColor
        {
            get => this.m_TabBarSplitterTabBarEndColor;
            set
            {
                if (!(this.m_TabBarSplitterTabBarEndColor != value))
                    return;
                this.m_TabBarSplitterTabBarEndColor = value;
            }
        }

        public Color TabBarSplitterButtonHoveredStartColor
        {
            get => this.m_TabBarSplitterButtonHoveredStartColor;
            set
            {
                if (!(this.m_TabBarSplitterButtonHoveredStartColor != value))
                    return;
                this.m_TabBarSplitterButtonHoveredStartColor = value;
            }
        }

        public Color TabBarSplitterButtonHoveredEndColor
        {
            get => this.m_TabBarSplitterButtonHoveredEndColor;
            set
            {
                if (!(this.m_TabBarSplitterButtonHoveredEndColor != value))
                    return;
                this.m_TabBarSplitterButtonHoveredEndColor = value;
            }
        }

        public Color TabBarSplitterButtonPushedStartColor
        {
            get => this.m_TabBarSplitterButtonPushedStartColor;
            set
            {
                if (!(this.m_TabBarSplitterButtonPushedStartColor != value))
                    return;
                this.m_TabBarSplitterButtonPushedStartColor = value;
            }
        }

        public Color TabBarSplitterButtonPushedEndColor
        {
            get => this.m_TabBarSplitterButtonPushedEndColor;
            set
            {
                if (!(this.m_TabBarSplitterButtonPushedEndColor != value))
                    return;
                this.m_TabBarSplitterButtonPushedEndColor = value;
            }
        }

        public Color TabBarSplitterSizeGripperColor
        {
            get => this.m_TabBarSplitterSizeGripperColor;
            set
            {
                if (!(this.m_TabBarSplitterSizeGripperColor != value))
                    return;
                this.m_TabBarSplitterSizeGripperColor = value;
            }
        }

        public Color TabBarSplitterSizeGripperLightColor
        {
            get => this.m_TabBarSplitterSizeGripperLightColor;
            set
            {
                if (!(this.m_TabBarSplitterSizeGripperLightColor != value))
                    return;
                this.m_TabBarSplitterSizeGripperLightColor = value;
            }
        }

        public Color TabBarSplitterSizeGripperDarkColor
        {
            get => this.m_TabBarSplitterSizeGripperDarkColor;
            set
            {
                if (!(this.m_TabBarSplitterSizeGripperDarkColor != value))
                    return;
                this.m_TabBarSplitterSizeGripperDarkColor = value;
            }
        }

        public Color XPTaskBarBorderColor
        {
            get => this.m_XPTaskBarBorderColor;
            set
            {
                if (!(this.m_XPTaskBarBorderColor != value))
                    return;
                this.m_XPTaskBarBorderColor = value;
            }
        }

        public Color XPTaskBarBoxBackColor
        {
            get => this.m_XPTaskBarBoxBackColor;
            set
            {
                if (!(this.m_XPTaskBarBoxBackColor != value))
                    return;
                this.m_XPTaskBarBoxBackColor = value;
            }
        }

        public Color XPTaskBarBoxForeColor
        {
            get => this.m_XPTaskBarBoxForeColor;
            set
            {
                if (!(this.m_XPTaskBarBoxForeColor != value))
                    return;
                this.m_XPTaskBarBoxForeColor = value;
            }
        }

        public Color XPTaskBarBoxHeaderUpperLineColor
        {
            get => this.m_XPTaskBarBoxHeaderUpperLineColor;
            set
            {
                if (!(this.m_XPTaskBarBoxHeaderUpperLineColor != value))
                    return;
                this.m_XPTaskBarBoxHeaderUpperLineColor = value;
            }
        }

        public Color XPTaskBarBoxHeaderLowerLineColor
        {
            get => this.m_XPTaskBarBoxHeaderLowerLineColor;
            set
            {
                if (!(this.m_XPTaskBarBoxHeaderLowerLineColor != value))
                    return;
                this.m_XPTaskBarBoxHeaderLowerLineColor = value;
            }
        }

        public Color XPTaskBarBoxArrowColor
        {
            get => this.m_XPTaskBarBoxArrowColor;
            set
            {
                if (!(this.m_XPTaskBarBoxArrowColor != value))
                    return;
                this.m_XPTaskBarBoxArrowColor = value;
            }
        }

        public Color XPTaskBarBoxActiveHighlightedItemColor
        {
            get => this.m_XPTaskBarBoxActiveHighlightedItemColor;
            set
            {
                if (!(this.m_XPTaskBarBoxActiveHighlightedItemColor != value))
                    return;
                this.m_XPTaskBarBoxActiveHighlightedItemColor = value;
            }
        }

        public Color XPTaskBarBoxInactiveHighlightedItemColor
        {
            get => this.m_XPTaskBarBoxInactiveHighlightedItemColor;
            set
            {
                if (!(this.m_XPTaskBarBoxInactiveHighlightedItemColor != value))
                    return;
                this.m_XPTaskBarBoxInactiveHighlightedItemColor = value;
            }
        }

        public Color ColorUIAdvBackColor
        {
            get => this.m_ColorUIAdvBackColor;
            set
            {
                if (!(this.m_ColorUIAdvBackColor != value))
                    return;
                this.m_ColorUIAdvBackColor = value;
            }
        }

        public Color ColorUIAdvTextColor
        {
            get => this.m_ColorUIAdvTextColor;
            set
            {
                if (!(this.m_ColorUIAdvTextColor != value))
                    return;
                this.m_ColorUIAdvTextColor = value;
            }
        }

        public Color ColorUIAdvItemBorderColor
        {
            get => this.m_ColorUIAdvItemBorderColor;
            set
            {
                if (!(this.m_ColorUIAdvItemBorderColor != value))
                    return;
                this.m_ColorUIAdvItemBorderColor = value;
            }
        }

        public Color ColorUIAdvHighlightedBorderColor
        {
            get => this.m_ColorUIAdvHighlightedBorderColor;
            set
            {
                if (!(this.m_ColorUIAdvHighlightedBorderColor != value))
                    return;
                this.m_ColorUIAdvHighlightedBorderColor = value;
            }
        }

        public Color ColorUIAdvSelectedBorderColor
        {
            get => this.m_ColorUIAdvSelectedBorderColor;
            set
            {
                if (!(this.m_ColorUIAdvSelectedBorderColor != value))
                    return;
                this.m_ColorUIAdvSelectedBorderColor = value;
            }
        }

        public Color ColorUIAdvSelectedHighlightedBorderColor
        {
            get => this.m_ColorUIAdvSelectedHighlightedBorderColor;
            set
            {
                if (!(this.m_ColorUIAdvSelectedHighlightedBorderColor != value))
                    return;
                this.m_ColorUIAdvSelectedHighlightedBorderColor = value;
            }
        }

        public Color ColorUIAdvGroupHeaderBackColor
        {
            get => this.m_ColorUIAdvGroupHeaderBackColor;
            set
            {
                if (!(this.m_ColorUIAdvGroupHeaderBackColor != value))
                    return;
                this.m_ColorUIAdvGroupHeaderBackColor = value;
            }
        }

        public Color StatusBarExtTopGradient
        {
            get => this.m_StatusBarExtTopGradient;
            set
            {
                if (!(this.m_StatusBarExtTopGradient != value))
                    return;
                this.m_StatusBarExtTopGradient = value;
            }
        }

        public Color StatusBarExtBottomGradient
        {
            get => this.m_StatusBarExtBottomGradient;
            set
            {
                if (!(this.m_StatusBarExtBottomGradient != value))
                    return;
                this.m_StatusBarExtBottomGradient = value;
            }
        }

        public Color StatusBarExtFillColor
        {
            get => this.m_StatusBarExtFillColor;
            set
            {
                if (!(this.m_StatusBarExtFillColor != value))
                    return;
                this.m_StatusBarExtFillColor = value;
            }
        }

        public Color ShapeHoverGradientStartColor
        {
            get => this.m_ShapeHoverGradientStartColor;
            set
            {
                if (!(this.m_ShapeHoverGradientStartColor != value))
                    return;
                this.m_ShapeHoverGradientStartColor = value;
            }
        }

        public Color ShapeHoverGradientEndColor
        {
            get => this.m_ShapeHoverGradientEndColor;
            set
            {
                if (!(this.m_ShapeHoverGradientEndColor != value))
                    return;
                this.m_ShapeHoverGradientEndColor = value;
            }
        }

        public Color ShapeSelectionGradientStartColor
        {
            get => this.m_ShapeSelectionGradientStartColor;
            set
            {
                if (!(this.m_ShapeSelectionGradientStartColor != value))
                    return;
                this.m_ShapeSelectionGradientStartColor = value;
            }
        }

        public Color ShapeSelectionGradientEndColor
        {
            get => this.m_ShapeSelectionGradientEndColor;
            set
            {
                if (!(this.m_ShapeSelectionGradientEndColor != value))
                    return;
                this.m_ShapeSelectionGradientEndColor = value;
            }
        }

        public Color ShapeHoverFillColor
        {
            get => this.m_ShapeHoverFillColor;
            set
            {
                if (!(this.m_ShapeHoverFillColor != value))
                    return;
                this.m_ShapeHoverFillColor = value;
            }
        }

        public Color ShapeSelectionFillColor
        {
            get => this.m_ShapeSelectionFillColor;
            set
            {
                if (!(this.m_ShapeSelectionFillColor != value))
                    return;
                this.m_ShapeSelectionFillColor = value;
            }
        }

        public Color ShapeBorderColor
        {
            get => this.m_ShapeBorderColor;
            set
            {
                if (!(this.m_ShapeBorderColor != value))
                    return;
                this.m_ShapeBorderColor = value;
            }
        }

        public Color ShapeSelectedColor
        {
            get => this.m_ShapeSelectedColor;
            set
            {
                if (!(this.m_ShapeSelectedColor != value))
                    return;
                this.m_ShapeSelectedColor = value;
            }
        }

        #endregion

        #region InitializeColors

        protected virtual void InitializeColors()
        {
            this.m_MenuSeparatorColor = Color.FromArgb(197, 197, 197);
            this.m_MenuBorderColor = Color.FromArgb(134, 134, 134);
            this.m_MenuColumnColor = Color.FromArgb(233, 238, 238);
            this.m_MenuColumnSeparatorColor = Color.FromArgb(197, 197, 197);
            this.m_MenuItemBorderColor = Color.FromArgb(150, 175, 142, 80);
            this.m_MenuItemDarkColor = Color.FromArgb((int)byte.MaxValue, 244, 197);
            this.m_MenuItemLightColor = Color.FromArgb((int)byte.MaxValue, 232, 146);
            this.m_MenuCheckedColor = Color.FromArgb(52, 55, 124);
            this.m_MenuCheckedFillColor = Color.FromArgb((int)byte.MaxValue, 227, 149);
            this.m_MenuCheckedBorderColor = Color.FromArgb(247, 212, 110);
            this.m_MenuComboButtonPushed1Color = Color.FromArgb(234, 224, 191);
            this.m_MenuComboButtonPushed2Color = Color.FromArgb(239, 189, 119);
            this.m_MenuComboButtonPushed3Color = Color.FromArgb((int)byte.MaxValue, 168, 56);
            this.m_MenuComboButtonPushed4Color = Color.FromArgb((int)byte.MaxValue, 212, 86);
            this.m_BarItemPressBorderColor = Color.FromArgb(194, 138, 48);
            this.m_BarItemHighlightBorderColor = Color.FromArgb(236, 199, 87);
            this.m_BarItemPressLightColor = Color.FromArgb((int)byte.MaxValue, 223, 113);
            this.m_BarItemPressDarkColor = Color.FromArgb((int)byte.MaxValue, 242, 114);
            this.m_DropDownBarItemLightColor = Color.FromArgb((int)byte.MaxValue, 223, 113);
            this.m_DropDownBarItemDarkColor = Color.FromArgb((int)byte.MaxValue, 242, 114);
            this.m_DropDownBarItemBorderColor = Color.FromArgb(194, 138, 48);
            this.m_BarItemCheckLightColor = Color.FromArgb(253, 210, 168);
            this.m_BarItemCheckDarkColor = Color.FromArgb(249, 147, 47);
            this.m_BarItemCheckBorderColor = Color.FromArgb(160, 131, 85);
            this.m_BarItemCheckFlashColor = Color.FromArgb((int)byte.MaxValue, 253, 241, 176);
            this.m_BarItemPressFlashColor = Color.FromArgb((int)byte.MaxValue, (int)byte.MaxValue, 208, 134);
            this.m_BarItemSelectFlashColor = Color.FromArgb((int)byte.MaxValue, (int)byte.MaxValue, 235, 174);
            this.m_TextBarItemBackColor = SystemColors.Window;
            this.m_TextBarItemBorderColor = SystemColors.Window;
            this.m_TextBarItemBorderHighlightColor = Color.FromArgb(139, 118, 84);
            this.m_ComboButtonPressLightColor = Color.FromArgb((int)byte.MaxValue, 197, 108);
            this.m_ComboButtonPressDarkColor = Color.FromArgb(251, 138, 59);
            this.m_ComboButtonHighlightLightColor = Color.FromArgb((int)byte.MaxValue, 252, 217);
            this.m_ComboButtonHighlightDarkColor = Color.FromArgb((int)byte.MaxValue, 214, 70);
            this.m_ComboButtonPressBorder = Color.FromArgb(139, 118, 84);
            this.m_ComboButtonHighlightBorder = Color.FromArgb(128, 185, 160, 116);
            this.m_TabItemBorderColor = Color.FromArgb(153, 187, 232);
            this.m_TabItemInnerBorderColor = Color.FromArgb(239, 246, (int)byte.MaxValue);
            this.m_TabItemOuterBorderColor = Color.FromArgb(209, 229, 254);
            this.m_TabItemTextColor = Color.FromArgb(21, 66, 139);
            this.m_TabItemActiveBottomColor = Color.FromArgb(225, 210, 163);
            this.m_TabItemTopGradientColor = Color.FromArgb(196, 221, 254);
            this.m_TabItemInActiveBottomColor = Color.FromArgb(235, 243, 253);
            this.m_TabItemMiddleLineColor = Color.FromArgb(215, 226, 232);
            this.m_TabPanelColor = Color.FromArgb(199, 216, 237);
            this.m_TabPanelBorderColor = Color.FromArgb(219, 232, 249);
            this.m_TabPanelBackColor = Color.FromArgb(199, 216, 237);
            this.m_DataTimePickerHighLightedBorderColor = Color.FromArgb(222, 183, 69);
            this.m_DataTimePickerSelectedBorderColor = Color.FromArgb(194, 129, 51);
            this.m_DataTimePickerDropDownHighLightLightColor = Color.FromArgb(245, 241, 211);
            this.m_DataTimePickerDropDownHighLightDarkColor = Color.FromArgb(236, 216, 141);
            this.m_DataTimePickerDropDownSelectedLightColor = Color.FromArgb(254, 223, 130);
            this.m_DataTimePickerDropDownSelectedDarkColor = Color.FromArgb(253, 226, 135);
            this.m_DataTimePickerCheckBoxNormalColor = Color.FromArgb(74, 93, 148);
            this.m_DataTimePickerCheckBoxSelectedColor = Color.FromArgb(0, 32, 115);
            this.m_DataTimePickerCheckBoxInnerRectBorderNormalColor = Color.FromArgb(173, 178, 189);
            this.m_DataTimePickerCheckBoxInnerRectBorderSelectedColor = Color.FromArgb(253, 203, 87);
            this.m_DataTimePickerCheckBoxInnerRectBorderPushedColor = Color.FromArgb(241, 138, 35);
            this.m_DataTimePickerCheckBoxInnerRectFillNormalColor = Color.FromArgb(206, 207, 214);
            this.m_DataTimePickerCheckBoxInnerRectFillSelectedColor = Color.FromArgb(250, 221, 143);
            this.m_DataTimePickerCheckBoxInnerRectFillPushedColor = Color.FromArgb((int)byte.MaxValue, 206, 103);
            this.m_DataTimePickerHighLightedForeColor = Color.FromArgb(0, 101, 206);
            this.m_NumericUpDownBorderColor = Color.FromArgb(173, 174, 181);
            this.m_NumericUpDownHighLightedBorderColor = Color.FromArgb(57, 125, 181);
            this.m_NumericUpDownSelectedBorderColor = Color.FromArgb(41, 97, 140);
            this.m_NumericUpDownArrowLightColor = Color.FromArgb(115, 134, 214);
            this.m_NumericUpDownArrowDarkColor = Color.FromArgb(74, 85, 123);
            this.m_TabDefaultBorderColor = Color.FromArgb(141, 178, 227);
            this.m_TabHotLightBottomBorderLineColor = Color.FromArgb(208, 195, 146);
            this.m_TabHotLightGradientTopBeginColor = Color.FromArgb(205, 217, 224);
            this.m_TabHotLightGradientTopEndColor = Color.FromArgb(228, 230, 222);
            this.m_TabHotLightGradientBottomBeginColor = Color.FromArgb(221, 221, 208);
            this.m_TabHotLightGradientBottomEndColor = Color.FromArgb(223, 213, 177);
            this.m_TabHotLightGradientCircleColor = Color.FromArgb(196, 221, 254);
            this.m_TabSelectedGradientTopColor = Color.FromArgb(240, 246, 254);
            this.m_TabSelectedGradientBottomColor = Color.FromArgb(225, 235, 246);
            this.m_TabSelectedInnerBorderColor = Color.FromArgb(235, 243, 252);
            this.m_TabHighlightInnerBorderColor = Color.FromArgb(234, 237, 253);
            this.m_TabSelectedHotLightBorderColor = Color.FromArgb((int)byte.MaxValue, 208, 48);
            this.m_TabSelectedHotLightInnerBorderColor = Color.FromArgb((int)byte.MaxValue, 240, 187);
            this.m_TabForeColor = Color.FromArgb(21, 66, 139);
            this.m_ActiveTabForeColor = this.m_TabForeColor;
            this.m_TabBackgroundColor = Color.FromArgb(199, 216, 237);
            this.m_DockTabForeColor = Color.FromArgb(21, 66, 139);
            this.m_DockTabBackgroundColor = Color.FromArgb(199, 216, 237);
            this.m_UpDownBorderHotColor = Color.FromArgb(221, 204, 155);
            this.m_UpDownInnerBorderHotStartColor = Color.FromArgb(193, 180, 168);
            this.m_UpDownInnerBorderHotEndColor = Color.FromArgb((int)byte.MaxValue, 223, 141);
            this.m_UpDownBorderPressedColor = Color.FromArgb(152, 143, 103);
            this.m_UpDownInnerBorderPressedStartColor = Color.FromArgb(193, 180, 168);
            this.m_UpDownInnerBorderPressedEndColor = Color.FromArgb((int)byte.MaxValue, 223, 141);
            this.m_UpDownBackgroundDisabledStartColor = Color.FromArgb(244, 244, 244);
            this.m_UpDownBackgroundDisabledEndColor = Color.FromArgb(201, 201, 201);
            this.m_UpDownBorderDisabledColor = Color.FromArgb(200, 200, 200);
            this.m_UpDownBackgroundHotTopStartColor = Color.FromArgb((int)byte.MaxValue, 231, 114);
            this.m_UpDownBackgroundHotTopEndColor = Color.FromArgb(254, 242, 180);
            this.m_UpDownBackgroundHotBottomStartColor = Color.FromArgb((int)byte.MaxValue, 214, 119);
            this.m_UpDownBackgroundHotBottomEndColor = Color.FromArgb(254, 221, 139);
            this.m_UpDownBackgroundPressedTopStartColor = Color.FromArgb(253, 223, 131);
            this.m_UpDownBackgroundPressedTopEndColor = Color.FromArgb(194, 153, 67);
            this.m_UpDownBackgroundPressedBottomStartColor = Color.FromArgb((int)byte.MaxValue, 165, 59);
            this.m_UpDownBackgroundPressedBottomEndColor = Color.FromArgb((int)byte.MaxValue, 190, 71);
            this.m_ColorUIAdvBackColor = Color.FromArgb(250, 250, 250);
            this.m_ColorUIAdvTextColor = Color.FromArgb(2, 22, 109);
            this.m_ColorUIAdvItemBorderColor = Color.FromArgb(197, 197, 197);
            this.m_ColorUIAdvHighlightedBorderColor = Color.FromArgb(243, 148, 54);
            this.m_ColorUIAdvSelectedBorderColor = Color.FromArgb(235, 75, 13);
            this.m_ColorUIAdvSelectedHighlightedBorderColor = Color.FromArgb((int)byte.MaxValue, 226, 148);
            this.m_ColorUIAdvGroupHeaderBackColor = Color.FromArgb(235, 235, 235);
            this.m_ButtonPressedTopColor = Color.FromArgb((int)byte.MaxValue, 197, 108);
            this.m_ButtonPressedBottomColor = Color.FromArgb(251, 138, 59);
            this.m_ButtonSelectedTopColor = ColorTranslator.FromHtml("#FCF9E0");
            this.m_ButtonSelectedBottomColor = ColorTranslator.FromHtml("#FAEAA8");
            this.m_ButtonDisabledTopColor = Color.FromArgb(244, 244, 244);
            this.m_ButtonDisabledBottomColor = Color.FromArgb(201, 201, 201);
            this.m_ButtonPressedBorderColor = Color.FromArgb(139, 118, 84);
            this.m_ButtonSelectedBorderColor = Color.FromArgb(185, 160, 116);
            this.m_ButtonDisabledBorderColor = Color.FromArgb(156, 164, 173);
            this.m_StatusBarExtTopGradient = Color.FromArgb(182, 209, 245);
            this.m_StatusBarExtBottomGradient = Color.FromArgb(64, 77, 140);
            this.m_StatusBarExtFillColor = Color.FromArgb(180, 205, 240);
        }

        #endregion

        internal void UpdateColors(Color basicColor)
        {
            this.InitializeColors();
            MSoffice2010ColorManager colorTable = MSoffice2010ColorManager.GetColorTable(MSoffice2010Theme.Silver);
            System.Type type = colorTable.GetType();
            foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (property.PropertyType == typeof(Color))
                {
                    string name = "m_" + property.Name;
                    FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);
                    if (field != (FieldInfo)null)
                        field.SetValue((object)this, (object)MSoffice2010ColorManager.MergeColors((Color)property.GetValue((object)colorTable, (object[])null), basicColor));
                }
            }
        }

        internal void UpdateScheme(MSoffice2010Theme scheme)
        {
            switch (scheme)
            {
                case MSoffice2010Theme.Blue:
                    s_managedBaseColor = Color.CornflowerBlue;
                    MSoffice2010ColorManager.s_managedColors = new WeakReference((object)new MSoffice2010BlueColors());
                    break;
                case MSoffice2010Theme.Silver:
                    s_managedBaseColor = Color.Silver;
                    MSoffice2010ColorManager.s_managedColors = new WeakReference((object)new MSoffice2010SilverColors());
                    break;
                case MSoffice2010Theme.Black:
                    s_managedBaseColor = Color.Black;
                    MSoffice2010ColorManager.s_managedColors = new WeakReference((object)new MSoffice2010BlackColors());
                    break;
            }
        }

        internal static Color MergeColors(Color baseColor, Color blendColor)
        {
            int red = MSoffice2010ColorManager.MergeChannels((int)baseColor.R, (int)blendColor.R);
            int num1 = MSoffice2010ColorManager.MergeChannels((int)baseColor.G, (int)blendColor.G);
            int num2 = MSoffice2010ColorManager.MergeChannels((int)baseColor.B, (int)blendColor.B);
            int green = num1;
            int blue = num2;
            return Color.FromArgb(red, green, blue);
        }

        private static int MergeChannels(int baseChannel, int blendChannel)
        {
            int num1 = baseChannel * blendChannel / (int)byte.MaxValue;
            int num2 = (int)byte.MaxValue - ((int)byte.MaxValue - baseChannel) * ((int)byte.MaxValue - blendChannel) / (int)byte.MaxValue;
            return (int)(byte)(num1 + baseChannel * (num2 - num1) / (int)byte.MaxValue);
        }

        public virtual object Clone() => (object)(MSoffice2010ColorManager)this.MemberwiseClone();

        public class ManagedColorsAppliedEventArgs : EventArgs
        {
            public Form Form;
            public Color BaseColor;

            public ManagedColorsAppliedEventArgs(Form form, Color baseColor)
            {
                this.Form = form;
                this.BaseColor = baseColor;
            }
        }

        public delegate void ManagedColorsAppliedEventHandler(
          MSoffice2010ColorManager.ManagedColorsAppliedEventArgs args);
    }

    #region MSoffice2010BlackColors
    public class MSoffice2010BlackColors : MSoffice2010ColorManager
    {
        protected override void InitializeColors()
        {
            base.InitializeColors();
            this.m_MenuTextBoxBorderColor = Color.FromArgb(137, 137, 137);
            this.m_MenuTextBoxBackColor = Color.FromArgb(232, 232, 232);
            this.m_MenuComboButtonHighlightLightColor = Color.FromArgb(239, 242, 246);
            this.m_MenuComboButtonHighlightDarkColor = Color.FromArgb(218, 224, 231);
            this.m_MenuComboButtonArrowColor = Color.FromArgb(124, 124, 124);
            this.m_MenuItemArrowLightColor = Color.FromArgb(78, 78, 78);
            this.m_MenuItemArrowDarkColor = Color.FromArgb(40, 40, 40);
            this.m_CommandBarDarkColor = Color.FromArgb(148, 156, 166);
            this.m_CommandBarLightColor = Color.FromArgb(205, 208, 213);
            this.m_CommandBarLightColor = Color.FromArgb(205, 208, 213);
            this.m_DockBarBackColor = Color.FromArgb(193, 193, 193);
            this.m_DropDownDarkColor = Color.FromArgb(139, 139, 139);
            this.m_DropDownLightColor = Color.FromArgb(207, 207, 207);
            this.m_DropDownHighlightLightColor = Color.FromArgb((int)byte.MaxValue, 248, 211);
            this.m_DropDownHighlightDarkColor = Color.FromArgb((int)byte.MaxValue, 193, 118);
            this.m_DropDownPressedLightColor = Color.FromArgb(254, 149, 82);
            this.m_DropDownPressedDarkColor = Color.FromArgb((int)byte.MaxValue, 217, 149);
            this.m_FloatHighlightButtonColor = Color.FromArgb((int)byte.MaxValue, 231, 162);
            this.m_FloatHighlightButtonBorderColor = Color.FromArgb((int)byte.MaxValue, 189, 105);
            this.m_FloatPressButtonColor = Color.FromArgb(221, 224, 227);
            this.m_FloatPressButtonBorderColor = Color.FromArgb(145, 153, 164);
            this.m_FloatPressCloseButtonBorderColor = Color.FromArgb(251, 140, 60);
            this.m_FloatPressCloseButtonColor = Color.FromArgb(251, 140, 60);
            this.m_FloatCommandBarLightColor = Color.White;
            this.m_FloatCommandBarDarkColor = Color.White;
            this.m_FloatLightBorderColor = Color.White;
            this.m_FloatBackgroundColor = Color.FromArgb(128, 128, 128);
            this.m_FloatBorderColor = Color.FromArgb(113, 113, 113);
            this.m_FloatCaptionColor = Color.White;
            this.m_BarItemSeparatorColor = Color.FromArgb(145, 153, 164);
            this.m_ComboButtonLightColor = Color.FromArgb(225, 225, 225);
            this.m_ComboButtonDarkColor = Color.FromArgb(203, 203, 203);
            this.m_ComboButtonBorder = Color.FromArgb(145, 145, 145);
            this.m_TabItemBorderColor = Color.FromArgb(66, 65, 66);
            this.m_TabItemInnerBorderColor = Color.FromArgb(189, 190, 189);
            this.m_TabItemOuterBorderColor = Color.FromArgb(74, 73, 74);
            this.m_TabItemTextColor = Color.FromArgb((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
            this.m_TabItemActiveBottomColor = Color.FromArgb(189, 154, 57);
            this.m_TabItemTopGradientColor = Color.FromArgb(107, 105, 99);
            this.m_TabItemInActiveBottomColor = Color.FromArgb(148, 150, 148);
            this.m_TabItemMiddleLineColor = Color.FromArgb(101, 106, 102);
            this.m_TabPanelColor = Color.FromArgb(116, 116, 116);
            this.m_TabPanelBorderColor = Color.FromArgb(57, 56, 57);
            this.m_TabPanelBackColor = Color.FromArgb(116, 116, 116);
            this.m_GroupBarBorderColor = Color.FromArgb(45, 45, 45);
            this.m_GroupBarHeaderColorLight = Color.FromArgb(121, 121, 121);
            this.m_GroupBarHeaderColorDark = Color.FromArgb(92, 92, 92);
            this.m_GroupBarItemTextColor = Color.FromArgb(49, 60, 66);
            this.m_GroupBarBackColor = Color.FromArgb(138, 138, 138);
            this.m_GroupBarHeaderTextColor = Color.White;
            this.m_GroupBarItemColorLight = Color.FromArgb(91, 91, 91);
            this.m_GroupBarItemColorDark = Color.FromArgb(125, 125, 125);
            this.m_GroupBarSplitterColorDark = Color.FromArgb(89, 89, 89);
            this.m_GroupBarSplitterColorLight = Color.FromArgb(150, 150, 150);
            this.m_GroupBarClientAreaBackground = Color.FromArgb(138, 138, 138);
            this.m_GroupBarHighlightColorLight = Color.FromArgb(180, 180, 180);
            this.m_GroupBarHighlightColorDark = Color.FromArgb(100, 100, 100);
            this.m_GroupBarSelectedColorLight = Color.FromArgb(150, 150, 150);
            this.m_GroupBarSelectedColorDark = Color.FromArgb(203, 203, 203);
            this.m_GroupBarSelectedTopColorLight = Color.FromArgb(89, 89, 89);
            this.m_GroupBarSelectedTopColorDark = Color.FromArgb(150, 150, 150);
            this.m_GroupBarSelectedHighlightColorLight = Color.FromArgb(180, 180, 180);
            this.m_GroupBarSelectedHighlightColorDark = Color.FromArgb(100, 100, 100);
            this.m_DataTimePickerBorderColor = Color.FromArgb(137, 137, 137);
            this.m_DataTimePickerDropDownArrowColor = Color.FromArgb(79, 86, 96);
            this.m_DataTimePickerDropDownLightColor = Color.FromArgb(146, 146, 146);
            this.m_DataTimePickerDropDownDarkColor = Color.FromArgb(83, 83, 83);
            this.m_DataTimePickerCheckBoxBorderNormalColor = Color.FromArgb(132, 132, 132);
            this.m_DataTimePickerCheckBoxBorderPushedColor = Color.FromArgb(74, 81, 90);
            this.m_DataTimePickerHighLightedForeColor = Color.Black;
            this.m_MonthCalendarHeaderStartColor = Color.FromArgb(163, 171, 177);
            this.m_MonthCalendarHeaderEndColor = Color.FromArgb(163, 171, 177);
            this.m_MonthCalendarForeColor = Color.White;
            this.m_MonthCalendarBackgroundColor = Color.FromArgb(163, 171, 177);
            this.m_XPTaskPaneInternalBorderColor = Color.FromArgb(246, 243, 248);
            this.m_XPTaskPaneBorderColor = Color.FromArgb(170, 170, 170);
            this.m_XPTaskPageBackColor = Color.FromArgb(246, 243, 248);
            this.m_TabDefaultBorderColor = Color.FromArgb(94, 94, 94);
            this.m_TabHotLightBottomBorderLineColor = Color.Green;
            this.m_TabHotLightGradientTopBeginColor = Color.Green;
            this.m_TabHotLightGradientTopEndColor = Color.Green;
            this.m_TabHotLightGradientBottomBeginColor = Color.Green;
            this.m_TabHotLightGradientBottomEndColor = Color.Green;
            this.m_TabHotLightGradientCircleColor = Color.Yellow;
            this.m_TabSelectedGradientTopColor = Color.FromArgb(166, 166, 166);
            this.m_TabSelectedGradientBottomColor = Color.FromArgb(200, 200, 200);
            this.m_TabSelectedInnerBorderColor = Color.FromArgb(94, 94, 94);
            this.m_TabHighlightInnerBorderColor = Color.Red;
            this.m_TabSelectedHotLightBorderColor = Color.Red;
            this.m_TabSelectedHotLightInnerBorderColor = Color.Red;
            this.m_TabForeColor = Color.White;
            this.m_ActiveTabForeColor = Color.Black;
            this.m_TabBackgroundColor = Color.FromArgb(116, 116, 116);
            this.m_TabScrollArrowColor = Color.WhiteSmoke;
            this.m_DockTabForeColor = Color.FromArgb(176, 178, 176);
            this.m_DockTabBackgroundColor = Color.FromArgb(82, 81, 82);
            this.m_ActiveTextBoxBorderColor = Color.FromArgb(235, 137, 0);
            this.m_ActiveTextBoxBackColor = Color.FromArgb((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
            this.m_InactiveTextBoxBorderColor = Color.FromArgb(155, 155, 155);
            this.m_InactiveTextBoxBackColor = Color.FromArgb(198, 198, 198);
            this.m_SelectedNodeBackground = Color.FromArgb(59, 59, 59);
            this.m_TreeNodeArrowColor = Color.FromArgb((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
            this.m_TreeviewBackColor = Color.FromArgb(149, 151, 153);
            this.m_TreeViewFontColor = Color.FromArgb((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
            this.m_ActiveFormBorderColor = Color.FromArgb(106, 106, 106);
            this.m_InactiveFormBorderColor = Color.FromArgb(166, 166, 166);
            this.m_FormTextColor = Color.White;
            this.m_ActiveTitleGradientBegin = Color.FromArgb(125, 125, 125);
            this.m_ActiveTitleGradientEnd = Color.FromArgb(96, 96, 96);
            this.m_InactiveTitleGradientBegin = Color.FromArgb(177, 177, 177);
            this.m_InactiveTitleGradientEnd = Color.FromArgb(160, 160, 160);
            this.m_SystemButtonSelectedGradientBegin = Color.FromArgb(221, 217, 221);
            this.m_SystemButtonSelectedGradientEnd = Color.FromArgb(111, 120, 130);
            this.m_SystemButtonPressedGradientBegin = Color.FromArgb(133, 138, 140);
            this.m_SystemButtonPressedGradientEnd = Color.FromArgb(90, 96, 102);
            this.m_SystemButtonBorderSelected = Color.FromArgb(178, 181, 183);
            this.m_SystemButtonBorderPressed = Color.FromArgb(82, 85, 86);
            this.m_FormBackground = Color.FromArgb(137, 137, 137);
            this.m_UpDownArrowStartColor = Color.FromArgb(87, 87, 87);
            this.m_UpDownArrowEndColor = Color.FromArgb(52, 52, 52);
            this.m_UpDownBorderNormalColor = Color.FromArgb(90, 90, 90);
            this.m_UpDownBackgroundNormalColor = Color.FromArgb(198, 198, 198);
            this.m_UpDownBackgroundNormalStartColor = Color.FromArgb(218, 224, 231);
            this.m_UpDownBackgroundNormalEndColor = Color.FromArgb(238, 242, 246);
            this.m_ComboBoxAdvNormalBackColor = SystemColors.Window;
            this.m_ComboBoxAdvHotBackColor = Color.FromArgb(198, 198, 198);
            this.m_ComboBoxAdvNormalBorderColor = Color.FromArgb(160, 160, 160);
            this.m_ComboBoxAdvHotBorderColor = Color.FromArgb(235, 201, 70);
            this.m_ComboBoxAdvPushedBorderColor = Color.FromArgb(194, 130, 51);
            this.m_ComboBoxAdvButtonUpperLineColor = Color.FromArgb(192, 185, 165);
            this.m_ComboBoxAdvArrowColor = Color.FromArgb(83, 83, 83);
            this.m_ComboBoxAdvLowerArrowLineColor = Color.FromArgb((int)byte.MaxValue, 248, 203);
            this.m_ComboBoxAdvHotBackgroundButtonColor1 = Color.FromArgb(244, 225, 153);
            this.m_ComboBoxAdvHotBackgroundButtonColor2 = Color.FromArgb(245, 219, 124);
            this.m_ComboBoxAdvHotBackgroundButtonColor3 = Color.FromArgb(245, 210, 130);
            this.m_ComboBoxAdvHotBackgroundButtonColor4 = Color.FromArgb(246, 200, 138);
            this.m_ComboBoxAdvNormalBackgroundButtonColor1 = Color.FromArgb(226, 226, 226);
            this.m_ComboBoxAdvNormalBackgroundButtonColor2 = Color.FromArgb(220, 220, 220);
            this.m_ComboBoxAdvNormalBackgroundButtonColor3 = Color.FromArgb(209, 209, 209);
            this.m_ComboBoxAdvNormalBackgroundButtonColor4 = Color.FromArgb(196, 196, 196);
            this.m_ComboBoxAdvPushedBackgroundButtonColor1 = Color.FromArgb(250, 211, 115);
            this.m_ComboBoxAdvPushedBackgroundButtonColor2 = Color.FromArgb(253, 228, 124);
            this.m_ComboBoxAdvPushedBackgroundButtonColor3 = Color.FromArgb(254, 220, 130);
            this.m_ComboBoxAdvPushedBackgroundButtonColor4 = Color.FromArgb((int)byte.MaxValue, 228, 138);
            this.m_CheckBoxAdvNormalBackColor = Color.FromArgb(248, 248, 248);
            this.m_CheckBoxAdvSelectedBackColor = Color.FromArgb(254, 248, 232);
            this.m_CheckBoxAdvPushedBackColor = Color.FromArgb((int)byte.MaxValue, 244, 213);
            this.m_CheckBoxAdvNormalBorderColor = Color.FromArgb(132, 132, 132);
            this.m_CheckBoxAdvSelectedBorderColor = Color.FromArgb(132, 132, 132);
            this.m_CheckBoxAdvPushedBorderColor = Color.FromArgb(132, 132, 132);
            this.m_CheckBoxAdvNormalInternalBorderColor = Color.FromArgb(244, 244, 244);
            this.m_CheckBoxAdvSelectedInternalBorderColor = Color.FromArgb(244, 244, 244);
            this.m_CheckBoxAdvPushedInternalBorderColor = Color.FromArgb(229, 236, 247);
            this.m_CheckBoxAdvNormalInternalRectangleBorderColor = Color.FromArgb(162, 172, 185);
            this.m_CheckBoxAdvSelectedInternalRectangleBorderColor = Color.FromArgb(250, 213, 122);
            this.m_CheckBoxAdvPushedInternalRectangleBorderColor = Color.FromArgb(242, 137, 38);
            this.m_CheckBoxAdvNormalInternalRectangleColor = Color.FromArgb(202, 207, 213);
            this.m_CheckBoxAdvSelectedInternalRectangleColor = Color.FromArgb(252, 231, 175);
            this.m_CheckBoxAdvPushedInternalRectangleColor = Color.FromArgb((int)byte.MaxValue, 208, 103);
            this.m_CheckBoxAdvNormalTickColor = Color.FromArgb(74, 107, 150);
            this.m_CheckBoxAdvSelectedTickColor = Color.FromArgb(78, 108, 143);
            this.m_CheckBoxAdvPushedTickColor = Color.FromArgb(79, 105, 130);
            this.m_CheckBoxAdvDisabledTickColor = Color.FromArgb(176, 190, 208);
            this.m_CheckBoxAdvIndeterminateRectangleColor = Color.FromArgb(158, 168, 178);
            this.m_CheckBoxAdvDisabledBackColor = Color.FromArgb(238, 240, 242);
            this.m_CheckBoxAdvDisabledBorderColor = Color.FromArgb(174, 177, 181);
            this.m_CheckBoxAdvDisabledInternalBorderColor = Color.FromArgb(224, 226, 229);
            this.m_RadioButtonAdvNormalBackColor = SystemColors.Window;
            this.m_RadioButtonAdvNormalBorderColor = Color.FromArgb(132, 132, 132);
            this.m_RadioButtonAdvNormalInternalBorderColor = Color.FromArgb(162, 172, 185);
            this.m_RadioButtonAdvSelectedBackColor = Color.FromArgb(244, 244, 244);
            this.m_RadioButtonAdvSelectedBorderColor = Color.FromArgb(132, 132, 132);
            this.m_RadioButtonAdvSelectedInternalBorderColor = Color.FromArgb(203, 209, 216);
            this.m_RadioButtonAdvPushedBackColor = Color.FromArgb(244, 244, 244);
            this.m_RadioButtonAdvPushedBorderColor = Color.FromArgb(132, 132, 132);
            this.m_RadioButtonAdvPushedInternalBorderColor = Color.FromArgb(203, 209, 216);
            this.m_RadioButtonAdvCheckMarkBorderColor = Color.FromArgb(75, 75, 75);
            this.m_RadioButtonAdvCheckMarkNormalBottomColor = Color.FromArgb(118, 118, 118);
            this.m_RadioButtonAdvCheckMarkSelectedBottomColor = Color.FromArgb(155, 156, 157);
            this.m_RadioButtonAdvCheckMarkPushedBottomColor = Color.FromArgb(95, 95, 95);
            this.m_TabBarSplitterBackColor = Color.FromArgb((int)byte.MaxValue, 251, (int)byte.MaxValue);
            this.m_TabBarSplitterBorderColor = Color.FromArgb(107, 109, 107);
            this.m_TabBarSplitterTextColor = Color.FromArgb(49, 52, 49);
            this.m_TabBarSplitterTabStartColor = Color.FromArgb(222, 223, 231);
            this.m_TabBarSplitterTabEndColor = Color.FromArgb(181, 190, 206);
            this.m_TabBarSplitterTabBarStartColor = Color.FromArgb(123, 121, 123);
            this.m_TabBarSplitterTabBarEndColor = Color.FromArgb(74, 73, 74);
            this.m_TabBarSplitterButtonHoveredStartColor = Color.White;
            this.m_TabBarSplitterButtonHoveredEndColor = Color.FromArgb(222, 231, 247);
            this.m_TabBarSplitterButtonPushedStartColor = Color.FromArgb(206, 223, (int)byte.MaxValue);
            this.m_TabBarSplitterButtonPushedEndColor = Color.FromArgb(148, 182, 239);
            this.m_TabBarSplitterSizeGripperColor = Color.FromArgb(112, 112, 112);
            this.m_TabBarSplitterSizeGripperLightColor = Color.FromArgb(204, 204, 204);
            this.m_TabBarSplitterSizeGripperDarkColor = Color.FromArgb(37, 37, 37);
            this.m_XPTaskBarBorderColor = Color.FromArgb(46, 46, 46);
            this.m_XPTaskBarBoxBackColor = Color.FromArgb(83, 83, 83);
            this.m_XPTaskBarBoxForeColor = Color.White;
            this.m_XPTaskBarBoxHeaderLowerLineColor = Color.FromArgb(46, 46, 46);
            this.m_XPTaskBarBoxHeaderUpperLineColor = Color.FromArgb(65, 65, 65);
            this.m_XPTaskBarBoxArrowColor = Color.White;
            this.m_XPTaskBarBoxActiveHighlightedItemColor = Color.FromArgb((int)byte.MaxValue, 232, 156);
            this.m_XPTaskBarBoxInactiveHighlightedItemColor = Color.FromArgb(229, 229, 229);
            this.m_ButtonDefaultTopColor = Color.FromArgb(146, 146, 146);
            this.m_ButtonDefaultBottomColor = Color.FromArgb(83, 83, 83);
            this.m_ButtonDefaultBorderColor = Color.FromArgb(169, 169, 169);
            this.m_ButtonDefaultInternalBorderColor = Color.FromArgb(128, Color.FromArgb(250, 251, (int)byte.MaxValue));
            this.m_ButtonPressedInternalBorderColor = Color.FromArgb(70, Color.FromArgb(176, 132, 92));
            this.m_ButtonSelectedInternalBorderColor = Color.FromArgb(128, Color.FromArgb(250, 251, (int)byte.MaxValue));
            this.m_ColorUIAdvTextColor = Color.FromArgb(70, 70, 70);
            this.m_ShapeSelectedColor = Color.FromArgb(251, 183, 99);
            this.m_ShapeBorderColor = Color.FromArgb(213, 217, 224);
            this.m_ShapeHoverFillColor = Color.FromArgb(251, 183, 99);
            this.m_ShapeHoverGradientEndColor = Color.FromArgb(250, 242, 193);
            this.m_ShapeHoverGradientStartColor = Color.FromArgb(252, 248, 223);
            this.m_ShapeSelectionFillColor = Color.FromArgb(80, 81, 82);
            this.m_ShapeSelectionGradientEndColor = Color.FromArgb(109, 110, 110);
            this.m_ShapeSelectionGradientStartColor = Color.FromArgb(140, 140, 140);
        }
    }
    #endregion

    #region MSoffice2010BlueColors
    public class MSoffice2010BlueColors : MSoffice2010ColorManager
    {
        protected override void InitializeColors()
        {
            base.InitializeColors();
            if (MSoffice2010ColorManager.s_managedColors != null && !MSoffice2010ColorManager.s_managedColors.IsAlive)
                this.isManagedColorNotAlive = true;
            else
                this.isManagedColorNotAlive = false;
            this.m_MenuTextBoxBorderColor = Color.FromArgb(179, 199, 225);
            this.m_MenuTextBoxBackColor = Color.FromArgb(234, 242, 251);
            this.m_MenuComboButtonHighlightLightColor = Color.FromArgb(232, 241, 253);
            this.m_MenuComboButtonHighlightDarkColor = Color.FromArgb(207, 223, 243);
            this.m_MenuComboButtonArrowColor = Color.FromArgb(86, 125, 177);
            this.m_MenuItemArrowLightColor = Color.FromArgb(106, 126, 197);
            this.m_MenuItemArrowDarkColor = Color.FromArgb(64, 70, 90);
            this.m_CommandBarDarkColor = Color.FromArgb(166, 194, 225);
            this.m_CommandBarLightColor = Color.FromArgb(221, 233, 246);
            this.m_CommandBarBorderColor = Color.FromArgb(221, 233, 246);
            this.m_DockBarBackColor = Color.FromArgb(187, 209, 233);
            this.m_DropDownDarkColor = Color.FromArgb(204, 215, 233);
            this.m_DropDownLightColor = Color.FromArgb(244, 248, 251);
            this.m_DropDownHighlightLightColor = Color.FromArgb((int)byte.MaxValue, 248, 237);
            this.m_DropDownHighlightDarkColor = Color.FromArgb((int)byte.MaxValue, 193, 118);
            this.m_DropDownPressedLightColor = Color.FromArgb(254, 149, 82);
            this.m_DropDownPressedDarkColor = Color.FromArgb((int)byte.MaxValue, 217, 149);
            this.m_FloatHighlightButtonColor = Color.FromArgb((int)byte.MaxValue, 231, 162);
            this.m_FloatHighlightButtonBorderColor = Color.FromArgb((int)byte.MaxValue, 189, 105);
            this.m_FloatPressButtonColor = Color.FromArgb(214, 232, (int)byte.MaxValue);
            this.m_FloatPressButtonBorderColor = Color.FromArgb(101, 147, 207);
            this.m_FloatPressCloseButtonBorderColor = Color.FromArgb(251, 140, 60);
            this.m_FloatPressCloseButtonColor = Color.FromArgb(251, 140, 60);
            this.m_FloatCommandBarLightColor = Color.White;
            this.m_FloatCommandBarDarkColor = Color.White;
            this.m_FloatLightBorderColor = Color.FromArgb(194, 220, (int)byte.MaxValue);
            this.m_FloatBackgroundColor = Color.FromArgb(187, 206, 230);
            this.m_FloatBorderColor = Color.FromArgb(187, 206, 230);
            this.m_FloatCaptionColor = Color.FromArgb(64, 22, 157);
            this.m_BarItemSeparatorColor = Color.FromArgb(154, 198, (int)byte.MaxValue);
            this.m_ComboButtonLightColor = Color.FromArgb(201, 221, 246);
            this.m_ComboButtonDarkColor = Color.FromArgb(160, 189, 224);
            this.m_ComboButtonBorder = Color.FromArgb(138, 173, 219);
            this.m_TabItemBorderColor = Color.FromArgb(153, 187, 232);
            this.m_TabItemInnerBorderColor = Color.FromArgb(239, 246, (int)byte.MaxValue);
            this.m_TabItemOuterBorderColor = Color.FromArgb(209, 229, 254);
            this.m_TabItemTextColor = Color.FromArgb(21, 66, 139);
            this.m_TabItemActiveBottomColor = Color.FromArgb(239, 211, 156);
            this.m_TabItemTopGradientColor = Color.FromArgb(196, 221, 254);
            this.m_TabItemInActiveBottomColor = Color.FromArgb(235, 243, 253);
            this.m_TabItemMiddleLineColor = Color.FromArgb(215, 226, 232);
            this.m_TabPanelColor = Color.FromArgb(188, 208, 232);
            this.m_TabPanelBorderColor = Color.FromArgb(219, 232, 249);
            this.m_TabPanelBackColor = Color.FromArgb(187, 206, 230);
            this.m_GroupBarBorderColor = Color.FromArgb(99, 146, 206);
            this.m_GroupBarHeaderColorLight = Color.FromArgb(219, 233, 246);
            this.m_GroupBarHeaderColorDark = Color.FromArgb(186, 208, 232);
            this.m_GroupBarItemTextColor = Color.FromArgb(33, 77, 140);
            this.m_GroupBarBackColor = Color.FromArgb(198, 215, 234);
            this.m_GroupBarHeaderTextColor = Color.FromArgb(16, 65, 140);
            this.m_GroupBarItemColorLight = this.m_GroupBarHeaderColorLight;
            this.m_GroupBarItemColorDark = this.m_GroupBarHeaderColorDark;
            this.m_GroupBarSplitterColorDark = Color.FromArgb(189, 219, (int)byte.MaxValue);
            this.m_GroupBarSplitterColorLight = Color.FromArgb(239, 243, (int)byte.MaxValue);
            this.m_GroupBarClientAreaBackground = Color.FromArgb(213, 228, 242);
            this.m_GroupBarHighlightColorLight = Color.FromArgb(229, 241, 252);
            this.m_GroupBarHighlightColorDark = Color.FromArgb(195, 219, 241);
            this.m_GroupBarSelectedColorLight = Color.FromArgb(184, 206, 231);
            this.m_GroupBarSelectedColorDark = Color.FromArgb(229, 241, 252);
            this.m_GroupBarSelectedTopColorLight = Color.FromArgb(199, 219, 239);
            this.m_GroupBarSelectedTopColorDark = Color.FromArgb(184, 206, 231);
            this.m_GroupBarSelectedHighlightColorLight = Color.FromArgb(229, 241, 252);
            this.m_GroupBarSelectedHighlightColorDark = Color.FromArgb(195, 219, 241);
            this.m_DataTimePickerBorderColor = Color.FromArgb(139, 160, 188);
            this.m_DataTimePickerDropDownArrowColor = Color.FromArgb(86, 125, 177);
            this.m_DataTimePickerDropDownLightColor = Color.FromArgb(252, 253, 254);
            this.m_DataTimePickerDropDownDarkColor = Color.FromArgb(181, 203, 232);
            this.m_DataTimePickerCheckBoxBorderNormalColor = Color.FromArgb(171, 193, 222);
            this.m_DataTimePickerCheckBoxBorderPushedColor = Color.FromArgb(85, 119, 163);
            this.m_MonthCalendarHeaderStartColor = Color.FromArgb(208, 221, 238);
            this.m_MonthCalendarHeaderEndColor = Color.FromArgb(208, 221, 238);
            this.m_MonthCalendarForeColor = SystemColors.ControlText;
            this.m_MonthCalendarBackgroundColor = Color.FromArgb(208, 221, 238);
            this.m_XPTaskPaneInternalBorderColor = Color.FromArgb(221, 237, 253);
            this.m_XPTaskPaneBorderColor = Color.FromArgb(145, 183, 249);
            this.m_XPTaskPageBackColor = Color.FromArgb(221, 237, 253);
            this.m_TabDefaultBorderColor = Color.FromArgb(184, 201, 219);
            this.m_TabHotLightBottomBorderLineColor = Color.Red;
            this.m_TabHotLightGradientTopBeginColor = Color.Red;
            this.m_TabHotLightGradientTopEndColor = Color.Red;
            this.m_TabHotLightGradientBottomBeginColor = Color.Red;
            this.m_TabHotLightGradientBottomEndColor = Color.Red;
            this.m_TabHotLightGradientCircleColor = Color.FromArgb(196, 221, 254);
            this.m_TabSelectedGradientTopColor = Color.FromArgb(242, 249, (int)byte.MaxValue);
            this.m_TabSelectedGradientBottomColor = Color.FromArgb(240, 246, 253);
            this.m_TabSelectedInnerBorderColor = Color.FromArgb(184, 201, 219);
            this.m_TabHighlightInnerBorderColor = Color.FromArgb(234, 237, 253);
            this.m_TabSelectedHotLightBorderColor = Color.FromArgb((int)byte.MaxValue, 208, 48);
            this.m_TabSelectedHotLightInnerBorderColor = Color.FromArgb((int)byte.MaxValue, 240, 187);
            this.m_TabForeColor = Color.FromArgb(21, 66, 139);
            this.m_ActiveTabForeColor = this.m_TabForeColor;
            this.m_TabBackgroundColor = Color.FromArgb(224, 249, 254);
            this.m_TabScrollArrowColor = Color.FromArgb(86, 125, 177);
            this.m_DockTabForeColor = Color.FromArgb(21, 66, 139);
            this.m_DockTabBackgroundColor = Color.FromArgb(199, 216, 237);
            this.m_ActiveTextBoxBorderColor = Color.FromArgb(235, 137, 0);
            this.m_InactiveTextBoxBorderColor = Color.FromArgb(188, 202, 221);
            this.m_ActiveTextBoxBackColor = Color.FromArgb((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
            this.m_InactiveTextBoxBackColor = Color.FromArgb(237, 245, 253);
            this.m_SelectedNodeBackground = Color.FromArgb(169, 192, 223);
            this.m_TreeNodeArrowColor = Color.FromArgb(129, 129, 129);
            this.m_TreeviewBackColor = Color.FromArgb(207, 221, 238);
            this.m_TreeViewFontColor = Color.FromArgb(30, 57, 91);
            this.m_ActiveFormBorderColor = Color.FromArgb(186, 209, 229);
            this.m_InactiveFormBorderColor = Color.FromArgb(214, 227, 239);
            this.m_FormTextColor = Color.FromArgb(57, 105, 173);
            this.m_ActiveTitleGradientBegin = Color.FromArgb(220, 233, 246);
            this.m_ActiveTitleGradientEnd = Color.FromArgb(188, 207, 233);
            this.m_InactiveTitleGradientBegin = Color.FromArgb(234, 242, 250);
            this.m_InactiveTitleGradientEnd = Color.FromArgb(215, 226, 242);
            this.m_SystemButtonSelectedGradientBegin = Color.FromArgb(247, 248, 249);
            this.m_SystemButtonSelectedGradientEnd = Color.FromArgb(220, 228, 234);
            this.m_SystemButtonPressedGradientBegin = Color.FromArgb(216, 226, 234);
            this.m_SystemButtonPressedGradientEnd = Color.FromArgb(142, 173, 193);
            this.m_SystemButtonBorderSelected = Color.FromArgb(169, 195, 224);
            this.m_SystemButtonBorderPressed = Color.FromArgb(131, 161, 183);
            this.m_FormBackground = Color.FromArgb(206, 220, 237);
            this.m_UpDownArrowStartColor = Color.FromArgb(24, 82, 172);
            this.m_UpDownArrowEndColor = Color.FromArgb(13, 50, 103);
            this.m_UpDownBorderNormalColor = Color.FromArgb(177, 197, 218);
            this.m_UpDownBackgroundNormalColor = Color.FromArgb(237, 245, 253);
            this.m_UpDownBackgroundNormalStartColor = Color.FromArgb(207, 223, 243);
            this.m_UpDownBackgroundNormalEndColor = Color.FromArgb(231, 241, 253);
            this.m_ComboBoxAdvNormalBackColor = SystemColors.Window;
            this.m_ComboBoxAdvHotBackColor = Color.FromArgb(237, 245, 253);
            this.m_ComboBoxAdvNormalBorderColor = Color.FromArgb(171, 186, 208);
            this.m_ComboBoxAdvHotBorderColor = Color.FromArgb(194, 130, 51);
            this.m_ComboBoxAdvPushedBorderColor = Color.FromArgb(157, 146, 102);
            this.m_ComboBoxAdvButtonUpperLineColor = Color.FromArgb(192, 185, 165);
            this.m_ComboBoxAdvArrowColor = Color.FromArgb(21, 74, 155);
            this.m_ComboBoxAdvLowerArrowLineColor = Color.FromArgb((int)byte.MaxValue, 248, 203);
            this.m_ComboBoxAdvHotBackgroundButtonColor1 = Color.FromArgb(244, 225, 153);
            this.m_ComboBoxAdvHotBackgroundButtonColor2 = Color.FromArgb(245, 219, 124);
            this.m_ComboBoxAdvHotBackgroundButtonColor3 = Color.FromArgb(245, 210, 130);
            this.m_ComboBoxAdvHotBackgroundButtonColor4 = Color.FromArgb(246, 200, 138);
            this.m_ComboBoxAdvNormalBackgroundButtonColor1 = Color.FromArgb(226, 226, 226);
            this.m_ComboBoxAdvNormalBackgroundButtonColor2 = Color.FromArgb(220, 220, 220);
            this.m_ComboBoxAdvNormalBackgroundButtonColor3 = Color.FromArgb(209, 209, 209);
            this.m_ComboBoxAdvNormalBackgroundButtonColor4 = Color.FromArgb(196, 196, 196);
            this.m_ComboBoxAdvPushedBackgroundButtonColor1 = Color.FromArgb(250, 211, 115);
            this.m_ComboBoxAdvPushedBackgroundButtonColor2 = Color.FromArgb(253, 228, 124);
            this.m_ComboBoxAdvPushedBackgroundButtonColor3 = Color.FromArgb(254, 220, 130);
            this.m_ComboBoxAdvPushedBackgroundButtonColor4 = Color.FromArgb((int)byte.MaxValue, 228, 138);
            this.m_CheckBoxAdvNormalBackColor = Color.FromArgb(248, 248, 248);
            this.m_CheckBoxAdvSelectedBackColor = Color.FromArgb(254, 248, 232);
            this.m_CheckBoxAdvPushedBackColor = Color.FromArgb((int)byte.MaxValue, 244, 213);
            this.m_CheckBoxAdvNormalBorderColor = Color.FromArgb(171, 193, 222);
            this.m_CheckBoxAdvSelectedBorderColor = Color.FromArgb(85, 119, 163);
            this.m_CheckBoxAdvPushedBorderColor = Color.FromArgb(85, 119, 163);
            this.m_CheckBoxAdvNormalInternalBorderColor = Color.FromArgb(244, 244, 244);
            this.m_CheckBoxAdvSelectedInternalBorderColor = Color.FromArgb(222, 234, 250);
            this.m_CheckBoxAdvPushedInternalBorderColor = Color.FromArgb(193, 216, 245);
            this.m_CheckBoxAdvNormalInternalRectangleBorderColor = Color.FromArgb(162, 172, 185);
            this.m_CheckBoxAdvSelectedInternalRectangleBorderColor = Color.FromArgb(250, 213, 122);
            this.m_CheckBoxAdvPushedInternalRectangleBorderColor = Color.FromArgb(242, 137, 38);
            this.m_CheckBoxAdvNormalInternalRectangleColor = Color.FromArgb(202, 207, 213);
            this.m_CheckBoxAdvSelectedInternalRectangleColor = Color.FromArgb(252, 231, 175);
            this.m_CheckBoxAdvPushedInternalRectangleColor = Color.FromArgb((int)byte.MaxValue, 208, 103);
            this.m_CheckBoxAdvNormalTickColor = Color.FromArgb(74, 107, 150);
            this.m_CheckBoxAdvSelectedTickColor = Color.FromArgb(78, 108, 141);
            this.m_CheckBoxAdvPushedTickColor = Color.FromArgb(79, 105, 130);
            this.m_CheckBoxAdvDisabledTickColor = Color.FromArgb(176, 190, 208);
            this.m_CheckBoxAdvIndeterminateRectangleColor = Color.FromArgb(158, 168, 178);
            this.m_CheckBoxAdvDisabledBackColor = Color.FromArgb(238, 240, 242);
            this.m_CheckBoxAdvDisabledBorderColor = Color.FromArgb(174, 177, 181);
            this.m_CheckBoxAdvDisabledInternalBorderColor = Color.FromArgb(224, 226, 229);
            this.m_RadioButtonAdvNormalBackColor = SystemColors.Window;
            this.m_RadioButtonAdvNormalBorderColor = Color.FromArgb(148, 175, 214);
            this.m_RadioButtonAdvNormalInternalBorderColor = Color.FromArgb(162, 172, 185);
            this.m_RadioButtonAdvSelectedBackColor = Color.FromArgb(227, 252, (int)byte.MaxValue);
            this.m_RadioButtonAdvSelectedBorderColor = Color.FromArgb(85, 119, 163);
            this.m_RadioButtonAdvSelectedInternalBorderColor = Color.FromArgb(250, 205, 101);
            this.m_RadioButtonAdvPushedBackColor = Color.FromArgb(205, 242, (int)byte.MaxValue);
            this.m_RadioButtonAdvPushedBorderColor = Color.FromArgb(85, 119, 163);
            this.m_RadioButtonAdvPushedInternalBorderColor = Color.FromArgb(244, 171, 14);
            this.m_RadioButtonAdvCheckMarkBorderColor = Color.FromArgb(17, 69, 221);
            this.m_RadioButtonAdvCheckMarkNormalBottomColor = Color.FromArgb(11, 130, 199);
            this.m_RadioButtonAdvCheckMarkSelectedBottomColor = Color.FromArgb(13, 160, 243);
            this.m_RadioButtonAdvCheckMarkPushedBottomColor = Color.FromArgb(7, 84, 131);
            this.m_TabBarSplitterBackColor = Color.FromArgb(231, 243, (int)byte.MaxValue);
            this.m_TabBarSplitterBorderColor = Color.FromArgb(148, 166, 198);
            this.m_TabBarSplitterTextColor = Color.FromArgb(16, 65, 140);
            this.m_TabBarSplitterTabStartColor = Color.FromArgb(222, 231, (int)byte.MaxValue);
            this.m_TabBarSplitterTabEndColor = Color.FromArgb(189, 211, 247);
            this.m_TabBarSplitterTabBarStartColor = Color.FromArgb(165, 190, 231);
            this.m_TabBarSplitterTabBarEndColor = Color.FromArgb(132, 166, 214);
            this.m_TabBarSplitterButtonHoveredStartColor = Color.White;
            this.m_TabBarSplitterButtonHoveredEndColor = Color.FromArgb(222, 231, 247);
            this.m_TabBarSplitterButtonPushedStartColor = Color.FromArgb(206, 223, (int)byte.MaxValue);
            this.m_TabBarSplitterButtonPushedEndColor = Color.FromArgb(148, 182, 239);
            this.m_TabBarSplitterSizeGripperColor = Color.FromArgb((int)sbyte.MaxValue, 163, 211);
            this.m_TabBarSplitterSizeGripperLightColor = Color.FromArgb(177, 201, 232);
            this.m_TabBarSplitterSizeGripperDarkColor = Color.FromArgb(69, 93, 128);
            this.m_XPTaskBarBorderColor = Color.FromArgb(162, 184, 212);
            this.m_XPTaskBarBoxBackColor = Color.FromArgb(184, 206, 231);
            this.m_XPTaskBarBoxForeColor = Color.FromArgb(0, 0, 0);
            this.m_XPTaskBarBoxHeaderLowerLineColor = Color.FromArgb(133, 158, 191);
            this.m_XPTaskBarBoxHeaderUpperLineColor = Color.FromArgb(162, 184, 212);
            this.m_XPTaskBarBoxArrowColor = Color.FromArgb(62, 62, 71);
            this.m_XPTaskBarBoxActiveHighlightedItemColor = Color.FromArgb((int)byte.MaxValue, 232, 156);
            this.m_XPTaskBarBoxInactiveHighlightedItemColor = Color.FromArgb(229, 229, 229);
            this.m_ColorUIAdvGroupHeaderBackColor = Color.FromArgb(222, 230, 238);
            this.m_ButtonDefaultTopColor = Color.FromArgb(231, 242, (int)byte.MaxValue);
            this.m_ButtonDefaultBottomColor = ColorTranslator.FromHtml("#bed1ea");
            this.m_ButtonDefaultBorderColor = Color.FromArgb(160, 178, 200);
            this.m_ButtonDefaultInternalBorderColor = Color.FromArgb(128, Color.FromArgb(250, 251, (int)byte.MaxValue));
            this.m_ButtonPressedInternalBorderColor = Color.FromArgb(70, Color.FromArgb(176, 132, 92));
            this.m_ButtonSelectedInternalBorderColor = Color.FromArgb(128, Color.FromArgb(250, 251, (int)byte.MaxValue));
            this.m_ShapeSelectedColor = Color.FromArgb(251, 183, 99);
            this.m_ShapeBorderColor = Color.FromArgb(213, 217, 224);
            this.m_ShapeHoverFillColor = Color.FromArgb(251, 183, 99);
            this.m_ShapeHoverGradientEndColor = Color.FromArgb(250, 242, 193);
            this.m_ShapeHoverGradientStartColor = Color.FromArgb(252, 248, 223);
            this.m_ShapeSelectionFillColor = Color.FromArgb(188, 208, 233);
            this.m_ShapeSelectionGradientEndColor = Color.FromArgb(208, 224, 243);
            this.m_ShapeSelectionGradientStartColor = Color.FromArgb(231, 242, 250);
        }
    }
    #endregion

    #region MSoffice2010SilverColors
    public class MSoffice2010SilverColors : MSoffice2010ColorManager
    {
        protected override void InitializeColors()
        {
            base.InitializeColors();
            this.m_MenuTextBoxBorderColor = Color.FromArgb(169, 177, 184);
            this.m_MenuTextBoxBackColor = Color.FromArgb(232, 234, 236);
            this.m_MenuComboButtonHighlightLightColor = Color.FromArgb(239, 242, 246);
            this.m_MenuComboButtonHighlightDarkColor = Color.FromArgb(218, 224, 231);
            this.m_MenuComboButtonArrowColor = Color.FromArgb(124, 124, 124);
            this.m_MenuItemArrowLightColor = Color.FromArgb(158, 158, 158);
            this.m_MenuItemArrowDarkColor = Color.FromArgb(124, 124, 124);
            this.m_CommandBarDarkColor = Color.FromArgb((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
            this.m_CommandBarLightColor = Color.FromArgb(241, 243, 244);
            this.m_CommandBarBorderColor = Color.FromArgb(241, 243, 244);
            this.m_DockBarBackColor = Color.FromArgb(249, 250, 251);
            this.m_DropDownDarkColor = Color.FromArgb(220, 221, 221);
            this.m_DropDownLightColor = Color.FromArgb(243, 245, 248);
            this.m_DropDownHighlightLightColor = Color.FromArgb((int)byte.MaxValue, 248, 211);
            this.m_DropDownHighlightDarkColor = Color.FromArgb((int)byte.MaxValue, 193, 118);
            this.m_DropDownPressedLightColor = Color.FromArgb(254, 149, 82);
            this.m_DropDownPressedDarkColor = Color.FromArgb((int)byte.MaxValue, 217, 149);
            this.m_FloatHighlightButtonColor = Color.FromArgb((int)byte.MaxValue, 231, 162);
            this.m_FloatHighlightButtonBorderColor = Color.FromArgb((int)byte.MaxValue, 189, 105);
            this.m_FloatPressButtonColor = Color.FromArgb(208, 212, 217);
            this.m_FloatPressButtonBorderColor = Color.FromArgb(124, 124, 148);
            this.m_FloatPressCloseButtonBorderColor = Color.FromArgb(251, 140, 60);
            this.m_FloatPressCloseButtonColor = Color.FromArgb(251, 140, 60);
            this.m_FloatCommandBarLightColor = Color.White;
            this.m_FloatCommandBarDarkColor = Color.White;
            this.m_FloatLightBorderColor = Color.FromArgb(219, 218, 228);
            this.m_FloatBackgroundColor = Color.FromArgb(151, 151, 151);
            this.m_FloatBorderColor = Color.FromArgb(151, 151, 115);
            this.m_FloatCaptionColor = Color.White;
            this.m_BarItemSeparatorColor = Color.FromArgb(110, 109, 143);
            this.m_ComboButtonLightColor = Color.FromArgb(243, 244, 245);
            this.m_ComboButtonDarkColor = Color.FromArgb(251, 251, 251);
            this.m_ComboButtonBorder = Color.FromArgb(212, 214, 217);
            this.m_TabItemBorderColor = Color.FromArgb(189, 190, 198);
            this.m_TabItemInnerBorderColor = Color.FromArgb(239, 235, 239);
            this.m_TabItemOuterBorderColor = Color.FromArgb(231, 229, 239);
            this.m_TabItemTextColor = Color.FromArgb(74, 81, 90);
            this.m_TabItemActiveBottomColor = Color.FromArgb(239, 211, 156);
            this.m_TabItemTopGradientColor = Color.FromArgb(222, 219, 231);
            this.m_TabItemInActiveBottomColor = Color.FromArgb(231, 231, 239);
            this.m_TabItemMiddleLineColor = Color.FromArgb(198, 190, 198);
            this.m_TabPanelColor = Color.Red;
            this.m_TabPanelBorderColor = Color.FromArgb(189, 190, 189);
            this.m_TabPanelBackColor = Color.FromArgb(227, 230, 232);
            this.m_GroupBarBorderColor = Color.FromArgb(161, 169, 180);
            this.m_GroupBarHeaderColorLight = Color.FromArgb(237, 241, 244);
            this.m_GroupBarHeaderColorDark = Color.FromArgb(221, 227, 231);
            this.m_GroupBarItemTextColor = Color.FromArgb(74, 81, 90);
            this.m_GroupBarBackColor = Color.FromArgb(222, 226, 234);
            this.m_GroupBarHeaderTextColor = Color.FromArgb(74, 81, 90);
            this.m_GroupBarItemColorLight = Color.FromArgb(241, 237, 241);
            this.m_GroupBarItemColorDark = this.m_GroupBarHeaderColorDark;
            this.m_GroupBarSplitterColorDark = Color.FromArgb(222, 227, 228);
            this.m_GroupBarSplitterColorLight = Color.FromArgb(220, 226, 230);
            this.m_GroupBarClientAreaBackground = Color.FromArgb(238, 238, 244);
            this.m_GroupBarHighlightColorLight = Color.FromArgb(239, 242, 247);
            this.m_GroupBarHighlightColorDark = Color.FromArgb(231, 232, 237);
            this.m_GroupBarSelectedColorLight = Color.FromArgb(225, 230, 234);
            this.m_GroupBarSelectedColorDark = Color.FromArgb(237, 238, 244);
            this.m_GroupBarSelectedTopColorLight = Color.FromArgb(222, 227, 228);
            this.m_GroupBarSelectedTopColorDark = Color.FromArgb(220, 226, 230);
            this.m_GroupBarSelectedHighlightColorLight = Color.FromArgb(218, 218, 214);
            this.m_GroupBarSelectedHighlightColorDark = Color.FromArgb(231, 237, 241);
            this.m_DataTimePickerBorderColor = Color.FromArgb(169, 177, 184);
            this.m_DataTimePickerDropDownArrowColor = Color.FromArgb(124, 124, 124);
            this.m_DataTimePickerDropDownLightColor = Color.FromArgb(223, 227, 231);
            this.m_DataTimePickerDropDownDarkColor = Color.FromArgb(208, 212, 221);
            this.m_DataTimePickerCheckBoxBorderNormalColor = Color.FromArgb(155, 157, 160);
            this.m_DataTimePickerCheckBoxBorderPushedColor = Color.FromArgb(107, 113, 115);
            this.m_DataTimePickerHighLightedForeColor = Color.FromArgb(107, 113, 115);
            this.m_MonthCalendarHeaderStartColor = Color.FromArgb(233, 236, 241);
            this.m_MonthCalendarHeaderEndColor = Color.FromArgb(233, 236, 241);
            this.m_MonthCalendarForeColor = SystemColors.ControlText;
            this.m_MonthCalendarBackgroundColor = Color.FromArgb(233, 236, 241);
            this.m_XPTaskPaneInternalBorderColor = Color.FromArgb(232, 235, 248);
            this.m_XPTaskPaneBorderColor = Color.FromArgb(158, 160, 160);
            this.m_XPTaskPageBackColor = Color.Red;
            this.m_TabDefaultBorderColor = Color.FromArgb(189, 190, 189);
            this.m_TabHotLightBottomBorderLineColor = Color.FromArgb(239, 186, 116);
            this.m_TabHotLightGradientTopBeginColor = Color.FromArgb(222, 219, 222);
            this.m_TabHotLightGradientTopEndColor = Color.FromArgb(231, 227, 231);
            this.m_TabHotLightGradientBottomBeginColor = Color.FromArgb(231, 215, 173);
            this.m_TabHotLightGradientBottomEndColor = Color.FromArgb(239, 211, 140);
            this.m_TabHotLightGradientCircleColor = Color.FromArgb(222, 219, 231);
            this.m_TabSelectedGradientTopColor = Color.FromArgb(254, 254, 254);
            this.m_TabSelectedGradientBottomColor = Color.FromArgb((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
            this.m_TabSelectedInnerBorderColor = Color.FromArgb(235, 243, 252);
            this.m_TabHighlightInnerBorderColor = Color.FromArgb(234, 237, 253);
            this.m_TabSelectedHotLightBorderColor = Color.FromArgb((int)byte.MaxValue, 208, 48);
            this.m_TabSelectedHotLightInnerBorderColor = Color.FromArgb((int)byte.MaxValue, 240, 187);
            this.m_TabForeColor = Color.FromArgb(74, 81, 90);
            this.m_ActiveTabForeColor = this.m_TabForeColor;
            this.m_TabBackgroundColor = Color.FromArgb(227, 230, 232);
            this.m_TabScrollArrowColor = Color.FromArgb(109, 114, 123);
            this.m_DockTabForeColor = Color.FromArgb(74, 81, 90);
            this.m_DockTabBackgroundColor = Color.FromArgb(214, 215, 222);
            this.m_ActiveTextBoxBorderColor = Color.FromArgb(235, 137, 0);
            this.m_ActiveTextBoxBackColor = Color.FromArgb((int)byte.MaxValue, (int)byte.MaxValue, (int)byte.MaxValue);
            this.m_InactiveTextBoxBorderColor = Color.FromArgb(224, 222, 222);
            this.m_InactiveTextBoxBackColor = Color.FromArgb(250, 250, 250);
            this.m_SelectedNodeBackground = Color.FromArgb(196, 196, 196);
            this.m_TreeNodeArrowColor = Color.FromArgb(129, 129, 129);
            this.m_TreeviewBackColor = Color.FromArgb(231, 234, 239);
            this.m_TreeViewFontColor = Color.FromArgb(33, 39, 35);
            this.m_ActiveFormBorderColor = Color.FromArgb(209, 211, 212);
            this.m_InactiveFormBorderColor = Color.FromArgb(214, 217, 220);
            this.m_FormTextColor = Color.FromArgb(83, 84, 89);
            this.m_ActiveTitleGradientBegin = Color.FromArgb(246, 248, 251);
            this.m_ActiveTitleGradientEnd = Color.FromArgb(209, 210, 211);
            this.m_InactiveTitleGradientBegin = Color.FromArgb(250, 251, 253);
            this.m_InactiveTitleGradientEnd = Color.FromArgb(227, 228, 229);
            this.m_SystemButtonSelectedGradientBegin = Color.FromArgb(245, 246, 247);
            this.m_SystemButtonSelectedGradientEnd = Color.FromArgb(232, 235, 239);
            this.m_SystemButtonPressedGradientBegin = Color.FromArgb(202, 204, 206);
            this.m_SystemButtonPressedGradientEnd = Color.FromArgb(172, 177, 183);
            this.m_SystemButtonBorderSelected = Color.FromArgb(191, 192, 196);
            this.m_SystemButtonBorderPressed = Color.FromArgb(142, 143, 145);
            this.m_FormBackground = Color.FromArgb(232, 236, 240);
            this.m_UpDownArrowStartColor = Color.FromArgb(96, 104, 112);
            this.m_UpDownArrowEndColor = Color.FromArgb(58, 62, 66);
            this.m_UpDownBorderNormalColor = Color.FromArgb(169, 177, 184);
            this.m_UpDownBackgroundNormalColor = Color.FromArgb(250, 250, 250);
            this.m_UpDownBackgroundNormalStartColor = Color.FromArgb(250, 250, 250);
            this.m_UpDownBackgroundNormalEndColor = Color.FromArgb(250, 250, 250);
            this.m_ComboBoxAdvNormalBackColor = SystemColors.Window;
            this.m_ComboBoxAdvHotBackColor = Color.FromArgb(250, 250, 250);
            this.m_ComboBoxAdvNormalBorderColor = Color.FromArgb(213, 214, 218);
            this.m_ComboBoxAdvHotBorderColor = Color.FromArgb(187, 192, 196);
            this.m_ComboBoxAdvPushedBorderColor = Color.FromArgb(157, 146, 102);
            this.m_ComboBoxAdvButtonUpperLineColor = Color.FromArgb(192, 185, 165);
            this.m_ComboBoxAdvArrowColor = Color.FromArgb(83, 83, 83);
            this.m_ComboBoxAdvLowerArrowLineColor = Color.FromArgb((int)byte.MaxValue, 248, 203);
            this.m_ComboBoxAdvHotBackgroundButtonColor1 = Color.FromArgb(244, 225, 153);
            this.m_ComboBoxAdvHotBackgroundButtonColor2 = Color.FromArgb(245, 219, 124);
            this.m_ComboBoxAdvHotBackgroundButtonColor3 = Color.FromArgb(245, 210, 130);
            this.m_ComboBoxAdvHotBackgroundButtonColor4 = Color.FromArgb(246, 200, 138);
            this.m_ComboBoxAdvNormalBackgroundButtonColor1 = Color.FromArgb(226, 226, 226);
            this.m_ComboBoxAdvNormalBackgroundButtonColor2 = Color.FromArgb(220, 220, 220);
            this.m_ComboBoxAdvNormalBackgroundButtonColor3 = Color.FromArgb(209, 209, 209);
            this.m_ComboBoxAdvNormalBackgroundButtonColor4 = Color.FromArgb(196, 196, 196);
            this.m_ComboBoxAdvPushedBackgroundButtonColor1 = Color.FromArgb(250, 211, 115);
            this.m_ComboBoxAdvPushedBackgroundButtonColor2 = Color.FromArgb(253, 228, 124);
            this.m_ComboBoxAdvPushedBackgroundButtonColor3 = Color.FromArgb(254, 220, 130);
            this.m_ComboBoxAdvPushedBackgroundButtonColor4 = Color.FromArgb((int)byte.MaxValue, 228, 138);
            this.m_CheckBoxAdvNormalBackColor = Color.FromArgb(248, 248, 248);
            this.m_CheckBoxAdvSelectedBackColor = Color.FromArgb(254, 248, 232);
            this.m_CheckBoxAdvPushedBackColor = Color.FromArgb((int)byte.MaxValue, 244, 213);
            this.m_CheckBoxAdvNormalBorderColor = Color.FromArgb(155, 157, 160);
            this.m_CheckBoxAdvSelectedBorderColor = Color.FromArgb(155, 157, 160);
            this.m_CheckBoxAdvPushedBorderColor = Color.FromArgb(155, 157, 160);
            this.m_CheckBoxAdvNormalInternalBorderColor = Color.FromArgb(244, 244, 244);
            this.m_CheckBoxAdvSelectedInternalBorderColor = Color.FromArgb(244, 244, 244);
            this.m_CheckBoxAdvPushedInternalBorderColor = Color.FromArgb(229, 236, 247);
            this.m_CheckBoxAdvNormalInternalRectangleBorderColor = Color.FromArgb(162, 172, 185);
            this.m_CheckBoxAdvSelectedInternalRectangleBorderColor = Color.FromArgb(250, 213, 122);
            this.m_CheckBoxAdvPushedInternalRectangleBorderColor = Color.FromArgb(242, 137, 38);
            this.m_CheckBoxAdvNormalInternalRectangleColor = Color.FromArgb(202, 207, 213);
            this.m_CheckBoxAdvSelectedInternalRectangleColor = Color.FromArgb(252, 231, 175);
            this.m_CheckBoxAdvPushedInternalRectangleColor = Color.FromArgb((int)byte.MaxValue, 208, 103);
            this.m_CheckBoxAdvNormalTickColor = Color.FromArgb(74, 107, 150);
            this.m_CheckBoxAdvSelectedTickColor = Color.FromArgb(78, 108, 143);
            this.m_CheckBoxAdvPushedTickColor = Color.FromArgb(79, 108, 139);
            this.m_CheckBoxAdvDisabledTickColor = Color.FromArgb(176, 190, 208);
            this.m_CheckBoxAdvIndeterminateRectangleColor = Color.FromArgb(158, 168, 178);
            this.m_CheckBoxAdvDisabledBackColor = Color.FromArgb(238, 240, 242);
            this.m_CheckBoxAdvDisabledBorderColor = Color.FromArgb(174, 177, 181);
            this.m_CheckBoxAdvDisabledInternalBorderColor = Color.FromArgb(224, 226, 229);
            this.m_RadioButtonAdvNormalBackColor = SystemColors.Window;
            this.m_RadioButtonAdvNormalBorderColor = Color.FromArgb(155, 157, 160);
            this.m_RadioButtonAdvNormalInternalBorderColor = Color.FromArgb(162, 172, 185);
            this.m_RadioButtonAdvSelectedBackColor = Color.FromArgb(244, 244, 244);
            this.m_RadioButtonAdvSelectedBorderColor = Color.FromArgb(165, 167, 170);
            this.m_RadioButtonAdvSelectedInternalBorderColor = Color.FromArgb(203, 209, 216);
            this.m_RadioButtonAdvPushedBackColor = Color.FromArgb(229, 236, 247);
            this.m_RadioButtonAdvPushedBorderColor = Color.FromArgb(165, 167, 160);
            this.m_RadioButtonAdvPushedInternalBorderColor = Color.FromArgb(203, 209, 216);
            this.m_RadioButtonAdvCheckMarkBorderColor = Color.FromArgb(160, 160, 160);
            this.m_RadioButtonAdvCheckMarkNormalBottomColor = Color.FromArgb(128, 128, 128);
            this.m_RadioButtonAdvCheckMarkSelectedBottomColor = Color.FromArgb(155, 156, 167);
            this.m_RadioButtonAdvCheckMarkPushedBottomColor = Color.FromArgb(107, 107, 107);
            this.m_TabBarSplitterBackColor = Color.FromArgb(239, 243, (int)byte.MaxValue);
            this.m_TabBarSplitterBorderColor = Color.FromArgb(107, 109, 107);
            this.m_TabBarSplitterTextColor = Color.FromArgb(49, 52, 49);
            this.m_TabBarSplitterTabStartColor = Color.FromArgb(222, 223, 231);
            this.m_TabBarSplitterTabEndColor = Color.FromArgb(181, 190, 206);
            this.m_TabBarSplitterTabBarStartColor = Color.FromArgb(123, 121, 123);
            this.m_TabBarSplitterTabBarEndColor = Color.FromArgb(74, 73, 74);
            this.m_TabBarSplitterButtonHoveredStartColor = Color.White;
            this.m_TabBarSplitterButtonHoveredEndColor = Color.FromArgb(222, 231, 247);
            this.m_TabBarSplitterButtonPushedStartColor = Color.FromArgb(206, 223, (int)byte.MaxValue);
            this.m_TabBarSplitterButtonPushedEndColor = Color.FromArgb(148, 182, 239);
            this.m_TabBarSplitterSizeGripperColor = Color.FromArgb(183, 186, 194);
            this.m_TabBarSplitterSizeGripperLightColor = Color.FromArgb(205, 209, 213);
            this.m_TabBarSplitterSizeGripperDarkColor = Color.FromArgb(114, 118, 122);
            this.m_XPTaskBarBorderColor = Color.FromArgb(161, 169, 179);
            this.m_XPTaskBarBoxBackColor = Color.FromArgb(220, 226, 231);
            this.m_XPTaskBarBoxForeColor = Color.FromArgb(0, 0, 0);
            this.m_XPTaskBarBoxHeaderLowerLineColor = Color.FromArgb(161, 169, 169);
            this.m_XPTaskBarBoxHeaderUpperLineColor = Color.FromArgb(179, 186, 195);
            this.m_XPTaskBarBoxArrowColor = Color.FromArgb(66, 69, 71);
            this.m_XPTaskBarBoxActiveHighlightedItemColor = Color.FromArgb((int)byte.MaxValue, 232, 156);
            this.m_XPTaskBarBoxInactiveHighlightedItemColor = Color.FromArgb(229, 229, 229);
            this.m_ButtonDefaultTopColor = Color.FromArgb(253, 253, 253);
            this.m_ButtonDefaultBottomColor = Color.FromArgb(208, 212, 221);
            this.m_ButtonDefaultBorderColor = Color.FromArgb(182, 185, 190);
            this.m_ButtonDefaultInternalBorderColor = Color.FromArgb(128, Color.FromArgb(250, 251, (int)byte.MaxValue));
            this.m_ButtonPressedInternalBorderColor = Color.FromArgb(70, Color.FromArgb(176, 132, 92));
            this.m_ButtonSelectedInternalBorderColor = Color.FromArgb(128, Color.FromArgb(250, 251, (int)byte.MaxValue));
            this.m_ColorUIAdvTextColor = Color.FromArgb(79, 83, 87);
            this.m_StatusBarExtTopGradient = Color.FromArgb(232, 236, 240);
            this.m_StatusBarExtBottomGradient = Color.FromArgb(232, 236, 240);
            this.m_StatusBarExtFillColor = Color.FromArgb(192, 198, 207);
            this.m_ShapeSelectedColor = Color.FromArgb(251, 183, 99);
            this.m_ShapeBorderColor = Color.FromArgb(213, 217, 224);
            this.m_ShapeHoverFillColor = Color.FromArgb(251, 183, 99);
            this.m_ShapeHoverGradientEndColor = Color.FromArgb(250, 242, 193);
            this.m_ShapeHoverGradientStartColor = Color.FromArgb(252, 248, 223);
            this.m_ShapeSelectionFillColor = Color.FromArgb(206, 210, 221);
            this.m_ShapeSelectionGradientEndColor = Color.FromArgb(229, 233, 239);
            this.m_ShapeSelectionGradientStartColor = Color.FromArgb(249, 249, 250);
        }
    }
    #endregion
}
