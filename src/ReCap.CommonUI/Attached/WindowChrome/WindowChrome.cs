using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Threading;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public sealed partial class WindowChrome
        : Layoutable
    {
        internal static readonly IWindowChromeImpl PLATFORM_IMPL = PlatformUtils.GetForPlatformByNameMatch<IWindowChromeImpl>();
        static readonly List<Window> AttachedWindows = new();




#region Properties
        public static readonly AttachedProperty<bool> MakeAvailableProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, bool>("MakeAvailable", false);
        internal static bool GetMakeAvailable(Window window)
            => window.GetValue(MakeAvailableProperty);
        internal static void SetMakeAvailable(Window window, bool value)
            => window.SetValue(MakeAvailableProperty, value);


        public static readonly AttachedProperty<WindowChrome> StateInfoProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, WindowChrome>("StateInfo", null);
        public static WindowChrome GetStateInfo(Window window)
            => window.GetValue(StateInfoProperty);
        static void SetStateInfo(Window window, WindowChrome value)
            => window.SetValue(StateInfoProperty, value);
#endregion




        internal static bool TryGetStateInfo(Window window, out WindowChrome stateInfo)
        {
            if (window != null)
            {
                stateInfo = GetStateInfo(window);
                return stateInfo != null;
            }
            else
            {
                stateInfo = null;
                return false;
            }
        }




#region Platform Info Accessors        
        public static bool PlatformCanUseManagedWindowChrome
        {
            get => PLATFORM_IMPL.CanUseManagedWindowChrome;
        }


        public static bool PlatformPrefersManagedWindowChrome
        {
            get => PlatformCanUseManagedWindowChrome && PLATFORM_IMPL.PrefersManagedWindowChrome;
        }


#if TEST_CAPTIONBUTTONROLES
        static readonly CaptionButtonRolesPair _TEST_ROLES = new()
        {
            Left = new()
            {
                CaptionButtonRole.FullScreen,
                CaptionButtonRole.WindowMenu,
            },
            Right = new()
            {
                CaptionButtonRole.WindowMenu,
                CaptionButtonRole.Minimize,
                CaptionButtonRole.Maximize,
                CaptionButtonRole.Close,
            },
        };
#endif
        public static CaptionButtonRolesPair PlatformDefaultCaptionButtonRoles
        {
#if TEST_CAPTIONBUTTONROLES
            get => _TEST_ROLES;
#else
            get => PLATFORM_IMPL.DefaultCaptionButtonRoles;
#endif
        }


        public static IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get => PLATFORM_IMPL.ValidCaptionButtonRoles;
        }
#endregion




        static WindowChrome()
        {
            PLATFORM_IMPL.Init();

            AttachedInit();
            HintsInit();
            StateInit();

            MakeAvailableProperty.Changed.AddClassHandler<Window>(MakeAvailableProperty_Changed);
        }


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
            AffectsAll<WindowChrome>(properties);
            AffectsAll<Window>(properties);
        }


        static void MakeAvailableProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(MakeAvailableProperty_Changed)}({window} ({window.Title}), {e})");
            (bool oldValue, bool newValue) = e.GetOldAndNewValue<bool>();
            if (newValue == oldValue)
                return;
            else if (newValue == AttachedWindows.Contains(window))
                return;
            else if (newValue)
                AttachWindows(window);
            else
                DetachWindows(window);
        }


        static void AttachWindows(params Window[] array)
            => AttachWindows(windows: array);
        static void AttachWindows(IEnumerable<Window> windows)
        {
            foreach (Window window in windows)
            {
                AttachWindow(window);
            }
        }
        static void AttachWindow(Window window)
        {
            if (GetStateInfo(window) != null)
                return;
            SetStateInfo(window, new(window));
            PLATFORM_IMPL.OnWindowAttached(window);
            window.Closed += Window_Closed;
            RefreshShowIconAndTitle(window);
            RefreshHint(window);
        }


        static void DetachWindows(params Window[] array)
            => DetachWindows(windows: array);
        static void DetachWindows(IEnumerable<Window> windows)
        {
            foreach (Window window in windows)
            {
                DetachWindow(window);
            }
        }
        static void DetachWindow(Window window)
        {
            GetStateInfo(window)?.PoorMansDispose();
            SetStateInfo(window, null);
            PLATFORM_IMPL.OnWindowDetached(window);
            window.Closed -= Window_Closed;
        }


        static void Window_Closed(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            DetachWindows(window);
        }


        internal static void RefreshHint(Window window, ManagedChromeHint? hint = null)
        {
            bool useManagedChrome = PLATFORM_IMPL.GetDesiredManagedChrome(window, hint ?? GetHint(window));
            PLATFORM_IMPL.ApplyDesiredManagedChrome(window
                , useManagedChrome
                , v => GetStateInfo(window).IsChromeManaged = v
            );

            //RefreshShowIconAndTitle(window);
            Dispatcher.UIThread.Post(() => RefreshShowIconAndTitle(window));
        }
    }
}