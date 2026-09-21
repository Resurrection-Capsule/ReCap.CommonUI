using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    partial class ManagedWindowChrome
    {
        public static readonly AttachedProperty<bool> MakeAvailableProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, bool>("MakeAvailable", false);
        internal static bool GetMakeAvailable(Window control)
            => control.GetValue(MakeAvailableProperty);
        internal static void SetMakeAvailable(Window control, bool value)
            => control.SetValue(MakeAvailableProperty, value);




#region Public hinting properties
        public static readonly AttachedProperty<ManagedChromeHint> HintProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, ManagedChromeHint>("Hint", ManagedChromeHint.Auto);
        public static ManagedChromeHint GetHint(Window control)
            => control.GetValue(HintProperty);
        public static void SetHint(Window control, ManagedChromeHint value)
            => control.SetValue(HintProperty, value);


        public static readonly AttachedProperty<ManagedChromeElementHint> ShowTitleHintProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, ManagedChromeElementHint>("ShowTitleHint", ManagedChromeElementHint.Auto);
        public static ManagedChromeElementHint GetShowTitleHint(Window control)
            => control.GetValue(ShowTitleHintProperty);
        public static void SetShowTitleHint(Window control, ManagedChromeElementHint value)
            => control.SetValue(ShowTitleHintProperty, value);


        public static readonly AttachedProperty<ManagedChromeElementHint> ShowIconHintProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, ManagedChromeElementHint>("ShowIconHint", ManagedChromeElementHint.Auto);
        public static ManagedChromeElementHint GetShowIconHint(Window control)
            => control.GetValue(ShowIconHintProperty);
        public static void SetShowIconHint(Window control, ManagedChromeElementHint value)
            => control.SetValue(ShowIconHintProperty, value);
#endregion




#region Actual state properties
        public static readonly AttachedProperty<bool> IsChromeManagedProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, bool>("IsChromeManaged", false);
        public static bool GetIsChromeManaged(Window control)
            => control.GetValue(IsChromeManagedProperty);
        internal static void SetIsChromeManaged(Window control, bool value)
            => control.SetValue(IsChromeManagedProperty, value);


        public static readonly AttachedProperty<bool> ShowTitleProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, bool>("ShowTitle", true);
        public static bool GetShowTitle(Window control)
            => control.GetValue(ShowTitleProperty);
        internal static void SetShowTitle(Window control, bool value)
            => control.SetValue(ShowTitleProperty, value);


        public static readonly AttachedProperty<bool> ShowIconProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, bool>("ShowIcon", false);
        public static bool GetShowIcon(Window control)
            => control.GetValue(ShowIconProperty);
        internal static void SetShowIcon(Window control, bool value)
            => control.SetValue(ShowIconProperty, value);
#endregion


        public static readonly AttachedProperty<bool> ReserveCaptionAreaProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, bool>("ReserveCaptionArea", true);
        public static bool GetReserveCaptionArea(Window control)
            => control.GetValue(ReserveCaptionAreaProperty);
        public static void SetReserveCaptionArea(Window control, bool value)
            => control.SetValue(ReserveCaptionAreaProperty, value);


        public static readonly AttachedProperty<double> ReservedCaptionHeightProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, double>("ReservedCaptionHeight", 1d);
        public static double GetReservedCaptionHeight(Window control)
            => control.GetValue(ReservedCaptionHeightProperty);
        public static void SetReservedCaptionHeight(Window control, double value)
            => control.SetValue(ReservedCaptionHeightProperty, value);




        public static readonly AttachedProperty<CaptionButtonRoles> LeftCaptionButtonRolesProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, CaptionButtonRoles>("LeftCaptionButtonRoles", PlatformDefaultCaptionButtonRoles.Left);
        public static CaptionButtonRoles GetLeftCaptionButtonRoles(Window control)
            => control.GetValue(LeftCaptionButtonRolesProperty);
        public static void SetLeftCaptionButtonRoles(Window control, CaptionButtonRoles value)
            => control.SetValue(LeftCaptionButtonRolesProperty, value);


        public static readonly AttachedProperty<double> LeftCaptionButtonsWidthProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, double>("LeftCaptionButtonsWidth", 0d);
        public static double GetLeftCaptionButtonsWidth(Window control)
            => control.GetValue(LeftCaptionButtonsWidthProperty);
        internal static void SetLeftCaptionButtonsWidth(Window control, double value)
            => control.SetValue(LeftCaptionButtonsWidthProperty, value);


        public static readonly AttachedProperty<CaptionButtonRoles> RightCaptionButtonRolesProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, CaptionButtonRoles>("RightCaptionButtonRoles", PlatformDefaultCaptionButtonRoles.Right);
        public static CaptionButtonRoles GetRightCaptionButtonRoles(Window control)
            => control.GetValue(RightCaptionButtonRolesProperty);
        public static void SetRightCaptionButtonRoles(Window control, CaptionButtonRoles value)
            => control.SetValue(RightCaptionButtonRolesProperty, value);


        public static readonly AttachedProperty<double> RightCaptionButtonsWidthProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, double>("RightCaptionButtonsWidth", 0d);
        public static double GetRightCaptionButtonsWidth(Window control)
            => control.GetValue(RightCaptionButtonsWidthProperty);
        internal static void SetRightCaptionButtonsWidth(Window control, double value)
            => control.SetValue(RightCaptionButtonsWidthProperty, value);
    }
}
