using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public partial class WindowChromeAddon
        : Layoutable
    {
        internal static readonly IWindowChromeAddonImpl _IMPL = PlatformUtils.GetForPlatformByNameMatch<IWindowChromeAddonImpl>();
        static WindowChromeAddon()
        {
            AffectsMeasure<WindowChromeAddon>(ManagedChromeHintProperty);
            AffectsArrange<WindowChromeAddon>(ManagedChromeHintProperty);
            AffectsRender<WindowChromeAddon>(ManagedChromeHintProperty);

            AffectsMeasure<Window>(ManagedChromeHintProperty);
            AffectsArrange<Window>(ManagedChromeHintProperty);
            AffectsRender<Window>(ManagedChromeHintProperty);


            UseAddonProperty.Changed.AddClassHandler<Window>(UseAddonProperty_Changed);
            ManagedChromeHintProperty.Changed.AddClassHandler<Window>(ManagedChromeHintProperty_Changed);
            _IMPL.Init();

            Dbg.DoChangedDebugOutput<Window>(UseAddonProperty, ManagedChromeHintProperty);
            CaptionButtonsInit();
        }




#region Managed chrome hack
        public static readonly AttachedProperty<bool> UseAddonProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("UseAddon", false);
        internal static bool GetUseAddon(Window control)
            => control.GetValue(UseAddonProperty);
        internal static void SetUseAddon(Window control, bool value)
            => control.SetValue(UseAddonProperty, value);


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




        static void UseAddonProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
            => UpdateManagedChrome(window);


        static void ManagedChromeHintProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
            => UpdateManagedChrome(window, e.GetNewValue<ManagedChromeMode>());





        internal static void UpdateManagedChrome(Window window)
            => UpdateManagedChrome(window, GetManagedChromeHint(window));
        internal static void UpdateManagedChrome(Window window, ManagedChromeMode chromeMode)
        {
            bool useManagedChrome = _IMPL.GetDesiredManagedChrome(window, chromeMode);
            SetDesiresManagedChrome(window, useManagedChrome);

            WindowChromeOptions.RefreshManagedShowIcon(window, WindowChromeOptions.GetManagedShowIconHint(window));
            WindowChromeOptions.RefreshManagedShowTitle(window, WindowChromeOptions.GetManagedShowTitleHint(window));
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
