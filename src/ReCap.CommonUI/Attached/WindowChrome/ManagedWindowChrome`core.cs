using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    partial class ManagedWindowChrome
    {
        static void AffectsAll<T>(params AvaloniaProperty[] properties)
            where T
                : Layoutable
        {
            AffectsMeasure<T>(properties);
            AffectsArrange<T>(properties);
            AffectsRender<T>(properties);
        }


        static void AffectsAll(params AvaloniaProperty[] properties)
        {
            AffectsAll<ManagedWindowChrome>(properties);
            AffectsAll<ManagedWindowChrome>(properties);
            AffectsAll<Window>(properties);
        }




        internal static readonly IWindowChromeImpl PLATFORM_IMPL = PlatformUtils.GetForPlatformByNameMatch<IWindowChromeImpl>();
        static ManagedWindowChrome()
        {
            MakeAvailableProperty.Changed.AddClassHandler<Window>(MakeAvailableProperty_Changed);
            PLATFORM_IMPL.Init();

            HintProperty.Changed.AddClassHandler<Window>(HintProperty_Changed);


            AvaloniaProperty[] properties =
            {
                ShowIconProperty,
                ShowTitleProperty,
                ReserveCaptionAreaProperty,
                ReservedCaptionHeightProperty,
                ShowIconHintProperty,
                ShowTitleHintProperty,
            };
            AffectsAll(properties);


            ShowIconHintProperty.Changed.AddClassHandler<Window>(ShowIconHintProperty_Changed);
            ShowTitleHintProperty.Changed.AddClassHandler<Window>(ShowTitleHintProperty_Changed);
        }




        static void MakeAvailableProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
        {
            UpdateManagedChrome(window);

            (bool oldValue, bool newValue) = e.GetOldAndNewValue<bool>();
            if (newValue == oldValue)
                return;
            else if (newValue == PLATFORM_IMPL.AttachedWindows.Contains(window))
                return;
            else if (newValue)
            {
                PLATFORM_IMPL.AttachWindow(window);
                window.Closed += Window_Closed;
                RefreshShowIconAndTitle(window);
            }
            else
            {
                PLATFORM_IMPL.DetachWindow(window);
                window.Closed -= Window_Closed;
            }
        }


        static void HintProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
            => UpdateManagedChrome(window, e.GetNewValue<ManagedChromeHint>());





        internal static void UpdateManagedChrome(Window window)
            => UpdateManagedChrome(window, GetHint(window));
        internal static void UpdateManagedChrome(Window window, ManagedChromeHint chromeMode)
        {
            bool useManagedChrome = PLATFORM_IMPL.GetDesiredManagedChrome(window, chromeMode);
            PLATFORM_IMPL.ApplyDesiredManagedChrome(window, useManagedChrome
                , v => SetIsChromeManaged(window, v)
            );
            RefreshShowIconAndTitle(window);
        }


        static void RefreshShowIconAndTitle(Window window)
        {
            RefreshShowIcon(window);
            RefreshShowTitle(window);
        }


        static void ShowIconHintProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs args)
            => RefreshShowIcon(window, args.GetNewValue<ManagedChromeElementHint>());
        internal static void RefreshShowIcon(Window window)
            => RefreshShowIcon(window, GetShowIconHint(window));
        internal static void RefreshShowIcon(Window window, ManagedChromeElementHint hint)
            => SetShowIcon(window, hint.ResolveVisibility(PLATFORM_IMPL.DefaultShowCaptionIcon));


        static void ShowTitleHintProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs args)
            => RefreshShowTitle(window, args.GetNewValue<ManagedChromeElementHint>());
        internal static void RefreshShowTitle(Window window)
            => RefreshShowTitle(window, GetShowTitleHint(window));
        internal static void RefreshShowTitle(Window window, ManagedChromeElementHint hint)
            => SetShowTitle(window, hint.ResolveVisibility(PLATFORM_IMPL.DefaultShowCaptionText));


        static void Window_Closed(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Closed -= Window_Closed;
        }
    }
}
