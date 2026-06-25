using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public enum WindowChromeShowHint
        : byte
    {
        Hide = 0x00,
        Show = 0x01,
        Auto = 0x02,
    }


    public partial class WindowChromeOptions
        : Layoutable
    {
        static WindowChromeOptions()
        {
            AvaloniaProperty[] properties =
            {
                ManagedShowIconProperty,
                ManagedShowTitleProperty,
                ReserveCaptionAreaProperty,
                ReservedCaptionHeightProperty,
            };

            AffectsMeasure<WindowChromeOptions>(properties);
            AffectsArrange<WindowChromeOptions>(properties);
            AffectsRender<WindowChromeOptions>(properties);

            AffectsMeasure<Window>(properties);
            AffectsArrange<Window>(properties);
            AffectsRender<Window>(properties);

            foreach (var prop in properties)
            {
                prop.Changed.AddClassHandler<Window>(NeedsInvalidateProperty_Changed);
            }


            Dbg.DoChangedDebugOutput<Window>(properties);
            //Dbg.DoChangedDebugOutput<Window>(Window.ExtendClientAreaTitleBarHeightHintProperty);
            //_IMPL.DefaultIconInTitleBar
            ManagedShowIconHintProperty.Changed.AddClassHandler<Window>(ManagedShowIconHintProperty_Changed);
            ManagedShowTitleHintProperty.Changed.AddClassHandler<Window>(ManagedShowTextHintProperty_Changed);
        }


        static void ManagedShowIconHintProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs args)
            => RefreshManagedShowIcon(window, args.GetNewValue<WindowChromeShowHint>());
        internal static void RefreshManagedShowIcon(Window window, WindowChromeShowHint hint)
            => SetManagedShowIcon(window, ResolveWindowChromeShowHint(hint, WindowChromeAddon._IMPL.DefaultShowCaptionIcon));
        internal static void RefreshManagedShowTitle(Window window, WindowChromeShowHint hint)
            => SetManagedShowTitle(window, ResolveWindowChromeShowHint(hint, WindowChromeAddon._IMPL.DefaultShowCaptionText));


        static void ManagedShowTextHintProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs args)
            => RefreshManagedShowTitle(window, args.GetNewValue<WindowChromeShowHint>());


        static void NeedsInvalidateProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs args)
            => window.InvalidateArrange();




#region Decorations Customization
        public static readonly AttachedProperty<WindowChromeShowHint> ManagedShowTitleHintProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeOptions, Window, WindowChromeShowHint>("ManagedShowTitleHint", WindowChromeShowHint.Auto);
        public static WindowChromeShowHint GetManagedShowTitleHint(Window control)
            => control.GetValue(ManagedShowTitleHintProperty);
        public static void SetManagedShowTitleHint(Window control, WindowChromeShowHint value)
            => control.SetValue(ManagedShowTitleHintProperty, value);


        public static readonly AttachedProperty<bool> ManagedShowTitleProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeOptions, Window, bool>("ManagedShowTitle", true);
        public static bool GetManagedShowTitle(Window control)
            => control.GetValue(ManagedShowTitleProperty);
        public static void SetManagedShowTitle(Window control, bool value)
            => control.SetValue(ManagedShowTitleProperty, value);


        public static readonly AttachedProperty<WindowChromeShowHint> ManagedShowIconHintProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeOptions, Window, WindowChromeShowHint>("ManagedShowIconHint", WindowChromeShowHint.Auto);
        public static WindowChromeShowHint GetManagedShowIconHint(Window control)
            => control.GetValue(ManagedShowIconHintProperty);
        public static void SetManagedShowIconHint(Window control, WindowChromeShowHint value)
            => control.SetValue(ManagedShowIconHintProperty, value);


        public static readonly AttachedProperty<bool> ManagedShowIconProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeOptions, Window, bool>("ManagedShowIcon", false);
        public static bool GetManagedShowIcon(Window control)
            => control.GetValue(ManagedShowIconProperty);
        public static void SetManagedShowIcon(Window control, bool value)
            => control.SetValue(ManagedShowIconProperty, value);


        public static readonly AttachedProperty<bool> ReserveCaptionAreaProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeOptions, Window, bool>("ReserveCaptionArea", true);
        public static bool GetReserveCaptionArea(Window control)
            => control.GetValue(ReserveCaptionAreaProperty);
        public static void SetReserveCaptionArea(Window control, bool value)
            => control.SetValue(ReserveCaptionAreaProperty, value);


        public static readonly AttachedProperty<double> ReservedCaptionHeightProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeOptions, Window, double>("ReservedCaptionHeight", 1d);
        public static double GetReservedCaptionHeight(Window control)
            => control.GetValue(ReservedCaptionHeightProperty);
        public static void SetReservedCaptionHeight(Window control, double value)
            => control.SetValue(ReservedCaptionHeightProperty, value);
#endregion




        internal static bool ResolveWindowChromeShowHint(WindowChromeShowHint hint, bool auto)
            => hint switch
            {
                WindowChromeShowHint.Hide => false,
                WindowChromeShowHint.Show => true,
                _ => auto,
            };
    }
}