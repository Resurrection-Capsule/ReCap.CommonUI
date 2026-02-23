using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public enum CaptionButtonsOrder
    {
        MinMaxClose,
        MaxMinClose,
    }


    public enum ManagedChromeMode
    {
        Never = 0,
        Auto,
        WheneverPossible,
    }


    public partial class WindowChromeAddon
        : AvaloniaObject
    {
        static readonly IWindowChromeAddonImpl _IMPL = PlatformUtils.GetForPlatform<IWindowChromeAddonImpl>();

#region Decorations Customization
        public static readonly AttachedProperty<bool> ManagedShowTitleProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("ManagedShowTitle", true);
        public static bool GetManagedShowTitle(Window control)
            => control.GetValue(ManagedShowTitleProperty);
        public static void SetManagedShowTitle(Window control, bool value)
            => control.SetValue(ManagedShowTitleProperty, value);


        public static readonly AttachedProperty<bool> LeftSideButtonsProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("LeftSideButtons", PlatformPrefersLeftSideButtons);
        public static bool GetLeftSideButtons(Window control)
            => control.GetValue(LeftSideButtonsProperty);
        public static void SetLeftSideButtons(Window control, bool value)
            => control.SetValue(LeftSideButtonsProperty, value);


        public static readonly AttachedProperty<CaptionButtonsOrder> ButtonsOrderProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, CaptionButtonsOrder>("ButtonsOrder", PlatformPreferredCaptionButtonsOrder);
        public static CaptionButtonsOrder GetButtonsOrder(Window control)
            => control.GetValue(ButtonsOrderProperty);
        public static void SetButtonsOrder(Window control, CaptionButtonsOrder value)
            => control.SetValue(ButtonsOrderProperty, value);
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
        internal static void SetManagedChromeHint(Window control, ManagedChromeMode value)
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



        
        public static bool PlatformCanUseManagedWindowChrome
        {
            get => _IMPL.CanUseManagedWindowChrome;
        }


        public static bool PlatformPrefersManagedWindowChrome
        {
            get => PlatformCanUseManagedWindowChrome && _IMPL.PrefersManagedWindowChrome;
        }


        public static bool PlatformPrefersLeftSideButtons
        {
            get => _IMPL.PrefersLeftSideButtons;
        }


        public static CaptionButtonsOrder PlatformPreferredCaptionButtonsOrder
        {
            get => _IMPL.PreferredCaptionButtonsOrder;
        }




        static WindowChromeAddon()
        {
            EnableHackHintProperty.Changed.AddClassHandler<Window>(EnableHackHintProperty_Changed);
            ManagedChromeHintProperty.Changed.AddClassHandler<Window>(ManagedChromeHintProperty_Changed);
            DesiredManagedChromeProperty.Changed.AddClassHandler<Window>(DesiredManagedChromeProperty_Changed);

            _IMPL.Init();

#if DEBUG
            EnableHackHintProperty.Changed.AddClassHandler<Window>(WindowChromeCosmeticProperty_Changed);
            ManagedChromeHintProperty.Changed.AddClassHandler<Window>(WindowChromeCosmeticProperty_Changed);
            DesiredManagedChromeProperty.Changed.AddClassHandler<Window>(WindowChromeCosmeticProperty_Changed);

            ManagedShowTitleProperty.Changed.AddClassHandler<Window>(WindowChromeCosmeticProperty_Changed);
            LeftSideButtonsProperty.Changed.AddClassHandler<Window>(WindowChromeCosmeticProperty_Changed);
            ButtonsOrderProperty.Changed.AddClassHandler<Window>(WindowChromeCosmeticProperty_Changed);
#endif
        }


        static void WindowChromeCosmeticProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
        {
            Console.WriteLine($"WINDOW '{window.Title}' PROPERTY '{e.Property.Name}' CHANGED:");
            Console.WriteLine($"    '{e.OldValue}' ==> '{e.NewValue}'");
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
