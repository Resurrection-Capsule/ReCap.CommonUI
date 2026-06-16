using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public partial class WindowChromeAddon
        : AvaloniaObject
    {
        static readonly IWindowChromeAddonImpl _IMPL = PlatformUtils.GetForPlatformByNameMatch<IWindowChromeAddonImpl>();
        static WindowChromeAddon()
        {
            EnableHackHintProperty.Changed.AddClassHandler<Window>(EnableHackHintProperty_Changed);
            ManagedChromeHintProperty.Changed.AddClassHandler<Window>(ManagedChromeHintProperty_Changed);
            _IMPL.Init();

            Dbg.DoChangedDebugOutput<Window>(EnableHackHintProperty, ManagedChromeHintProperty, ManagedShowTitleProperty, Window.ExtendClientAreaTitleBarHeightHintProperty);
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




        static void EnableHackHintProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
            => UpdateManagedChrome(window);


        static void ManagedChromeHintProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
            => UpdateManagedChrome(window, e.GetNewValue<ManagedChromeMode>());





        internal static void UpdateManagedChrome(Window window)
            => UpdateManagedChrome(window, GetManagedChromeHint(window));
        internal static void UpdateManagedChrome(Window window, ManagedChromeMode chromeMode)
        {
            bool useManagedChrome = _IMPL.GetDesiredManagedChrome(window, chromeMode);
            SetDesiresManagedChrome(window, useManagedChrome);
        }




        static readonly Dictionary<Window, bool> _desiresManagedChrome = new();
        static bool GetDesiresManagedChrome(Window window)
            => _desiresManagedChrome[window];


        static bool TryGetDesiresManagedChrome(Window window, out bool desiresManagedChrome)
            => _desiresManagedChrome.TryGetValue(window, out desiresManagedChrome);


        static void SetDesiresManagedChrome(Window window, bool desiresManagedChrome)
        {
            if (!_desiresManagedChrome.ContainsKey(window))
                window.Closed += Window_Closed;

            _desiresManagedChrome[window] = desiresManagedChrome;

            _IMPL.ApplyDesiredManagedChrome(window, desiresManagedChrome
                , v => SetIsUsingManagedChrome(window, v)
            );
        }


        static void ClearChromeDesire(Window window)
            => _desiresManagedChrome.Remove(window);


        static void Window_Closed(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Closed -= Window_Closed;
            ClearChromeDesire(window);
        }
    }
}
