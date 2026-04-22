using System;
using Avalonia;
using Avalonia.Controls;
using ReCap.CommonUI.Util;
using ReCap.CommonUI.Util.Win32;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public partial class WindowChromeAddon
        : AvaloniaObject
    {
        static readonly IWindowChromeAddonImpl _IMPL = PlatformUtils.GetForPlatform<IWindowChromeAddonImpl>();
        static WindowChromeAddon()
        {
            EnableHackHintProperty.Changed.AddClassHandler<Window>(EnableHackHintProperty_Changed);
            ManagedChromeHintProperty.Changed.AddClassHandler<Window>(ManagedChromeHintProperty_Changed);
            DesiredManagedChromeProperty.Changed.AddClassHandler<Window>(DesiredManagedChromeProperty_Changed);

            _IMPL.Init();

#if WINDOWCHROMEADDON_PRINT_PROPERTY_CHANGES
            EnableHackHintProperty.Changed.AddClassHandler<Window>(WindowChromeCosmeticProperty_Changed);
            ManagedChromeHintProperty.Changed.AddClassHandler<Window>(WindowChromeCosmeticProperty_Changed);
            DesiredManagedChromeProperty.Changed.AddClassHandler<Window>(WindowChromeCosmeticProperty_Changed);

            ManagedShowTitleProperty.Changed.AddClassHandler<Window>(WindowChromeCosmeticProperty_Changed);
#endif
            CaptionButtonsInit();
        }




#region Decorations Customization
        public static readonly AttachedProperty<bool> ManagedShowTitleProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("ManagedShowTitle", true);
        public static bool GetManagedShowTitle(Window control)
            => control.GetValue(ManagedShowTitleProperty);
        public static void SetManagedShowTitle(Window control, bool value)
            => control.SetValue(ManagedShowTitleProperty, value);


        public static readonly AttachedProperty<bool> ManagedShowIconProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("ManagedShowIcon", _IMPL.DefaultIconInTitleBar);
        public static bool GetManagedShowIcon(Window control)
            => control.GetValue(ManagedShowIconProperty);
        public static void SetManagedShowIcon(Window control, bool value)
            => control.SetValue(ManagedShowIconProperty, value);


        public static readonly AttachedProperty<bool> ReserveCaptionAreaProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("ReserveCaptionArea", true);
        public static bool GetReserveCaptionArea(Window control)
            => control.GetValue(ReserveCaptionAreaProperty);
        public static void SetReserveCaptionArea(Window control, bool value)
            => control.SetValue(ReserveCaptionAreaProperty, value);


        public static readonly AttachedProperty<double> DefaultTitleBarHeightProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, double>("DefaultTitleBarHeight", 1d);
        public static double GetDefaultTitleBarHeight(Window control)
            => control.GetValue(DefaultTitleBarHeightProperty);
        public static void SetDefaultTitleBarHeight(Window control, double value)
            => control.SetValue(DefaultTitleBarHeightProperty, value);
#endregion




#region Managed chrome hack
        public static readonly AttachedProperty<bool> EnableHackHintProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("EnableHackHint", false);
        internal static bool GetEnableHackHint(Window control)
            => control.GetValue(EnableHackHintProperty);
        internal static void SetEnableHackHint(Window control, bool value)
            => control.SetValue(EnableHackHintProperty, value);


        public static readonly AttachedProperty<ManagedChromeMode> ManagedChromeHintProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, ManagedChromeMode>("ManagedChromeHint", ManagedChromeMode.Auto);
        public static ManagedChromeMode GetManagedChromeHint(Window control)
            => control.GetValue(ManagedChromeHintProperty);
        public static void SetManagedChromeHint(Window control, ManagedChromeMode value)
            => control.SetValue(ManagedChromeHintProperty, value);




        internal static readonly AttachedProperty<bool> DesiredManagedChromeProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("DesiredManagedChrome", false);
        internal static bool GetDesiredManagedChrome(Window control)
            => control.GetValue(DesiredManagedChromeProperty);
        internal static void SetDesiredManagedChrome(Window control, bool value)
            => control.SetValue(DesiredManagedChromeProperty, value);




        public static readonly AttachedProperty<bool> IsUsingManagedChromeProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("IsUsingManagedChrome", false);
        public static bool GetIsUsingManagedChrome(Window control)
            => control.GetValue(IsUsingManagedChromeProperty);
        internal static void SetIsUsingManagedChrome(Window control, bool value)
            => control.SetValue(IsUsingManagedChromeProperty, value);
#endregion




        public static readonly AttachedProperty<NCHitTestResult> NonClienHitTestResultProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Visual, NCHitTestResult>("NonClienHitTestResult", NCHitTestResult.CLIENT, inherits: true);
        public static NCHitTestResult GetNonClienHitTestResult(Visual control)
            => control.GetValue(NonClienHitTestResultProperty);
        public static void SetNonClienHitTestResult(Visual control, NCHitTestResult value)
            => control.SetValue(NonClienHitTestResultProperty, value);


        
        public static bool PlatformCanUseManagedWindowChrome
        {
            get => _IMPL.CanUseManagedWindowChrome;
        }


        public static bool PlatformPrefersManagedWindowChrome
        {
            get => PlatformCanUseManagedWindowChrome && _IMPL.PrefersManagedWindowChrome;
        }




        static void WindowChromeCosmeticProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
        {
#if WINDOWCHROMEADDON_PRINT_PROPERTY_CHANGES
            Console.WriteLine($"WINDOW '{window.Title}' PROPERTY '{e.Property.Name}' CHANGED:");
            Console.WriteLine($"    '{e.OldValue}' ==> '{e.NewValue}'");
#endif
        }


        static void EnableHackHintProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
            => UpdateManagedChrome(window);


        static void ManagedChromeHintProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
            => UpdateManagedChrome(window, e.GetNewValue<ManagedChromeMode>());


        static void DesiredManagedChromeProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
        {
            bool newValue = e.GetNewValue<bool>();
            bool isUsingManagedChrome = newValue;
            _IMPL.ApplyDesiredManagedChrome(window, newValue, ref isUsingManagedChrome);

            SetIsUsingManagedChrome(window, newValue);
        }





        internal static void UpdateManagedChrome(Window window)
            => UpdateManagedChrome(window, GetManagedChromeHint(window));
        internal static void UpdateManagedChrome(Window window, ManagedChromeMode chromeMode)
        {
            bool useManagedChrome = _IMPL.GetDesiredManagedChrome(window, chromeMode);
            SetDesiredManagedChrome(window, useManagedChrome);
        }
    }
}
