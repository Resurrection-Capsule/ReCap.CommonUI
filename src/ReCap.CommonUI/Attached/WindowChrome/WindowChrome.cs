using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Threading;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public sealed partial class WindowChrome
        : Layoutable
        , IDisposable
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


        public static readonly AttachedProperty<WindowChrome> ChromeProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, WindowChrome>("Chrome", null);
        public static WindowChrome GetChrome(Window window)
            => window.GetValue(ChromeProperty);
        static void SetChrome(Window window, WindowChrome value)
            => window.SetValue(ChromeProperty, value);




        WeakReference<Window> _hostWindowRef = null;
        internal Window HostWindow
        {
            get
            {
                _hostWindowRef.TryGetTarget(out Window hostWindow);
                return hostWindow;
            }
            private set
            {
                _hostWindowRef?.SetTarget(null);
                
                _hostWindowRef = value != null
                    ? new(value)
                    : null
                ;
            }
        }
#endregion




        internal static bool TryGetChrome(Window window, out WindowChrome chrome)
        {
            if (window != null)
            {
                chrome = GetChrome(window);
                return chrome != null;
            }
            else
            {
                chrome = null;
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




        static readonly IEnumerable<AvaloniaProperty> _WINDOW_OBSERVE_PROPERTIES;
        static readonly IEnumerable<AvaloniaProperty> _WINDOWCHROME_OBSERVE_PROPERTIES;
        static WindowChrome()
        {
            PLATFORM_IMPL.Init();

            AttachedInit();
            HintsInit();
            StateInit();

            MakeAvailableProperty.Changed.AddClassHandler<Window>(MakeAvailableProperty_Changed);


            _WINDOW_OBSERVE_PROPERTIES = new AvaloniaProperty[]
            {
                WindowBase.IsActiveProperty,
                Window.WindowStateProperty,
                Window.SystemDecorationsProperty,
                Window.ExtendClientAreaTitleBarHeightHintProperty,
                Window.OffScreenMarginProperty,
                Window.ExtendClientAreaChromeHintsProperty,
                Window.IsExtendedIntoWindowDecorationsProperty,
                /*
                ReserveCaptionAreaProperty,
                ReservedCaptionHeightProperty,
                */
                /*
                MakeAvailableProperty,
                ChromeProperty,
                */
            };

            _WINDOWCHROME_OBSERVE_PROPERTIES = new AvaloniaProperty[]
            {
                LeftCaptionButtonsWidthProperty,
                RightCaptionButtonsWidthProperty,
                LeftCaptionButtonRolesProperty,
                RightCaptionButtonRolesProperty,
            };
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
            if (GetChrome(window) != null)
                return;
            SetChrome(window, new(window));
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
            GetChrome(window)?.Dispose();
            SetChrome(window, null);
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
                , v => GetChrome(window).IsChromeManaged = v
            );

            //RefreshShowIconAndTitle(window);
            Dispatcher.UIThread.Post(() => RefreshShowIconAndTitle(window));
        }




        CompositeDisposable _disposables = null;
        internal WindowChrome(Window window)
            : this()
        {
            HostWindow = window;
            Dispatcher.UIThread.Post(BuildDisposables);
            RefreshOverrideCaptionButtonRoles();
            RefreshCaptionAreaProperties();
        }
        void BuildDisposables()
        {
            _disposables = new()
            /*
            {
                window
                    .GetObservable(ChromeProperty)
                    .Subscribe(windowChrome =>
                    {
                        if (windowChrome != null)
                            RefreshElements();
                    })
                ,
                window
                    .GetObservable(WindowBase.IsActiveProperty)
                    .Subscribe(HandleObserver)
                ,
                window
                    .GetObservable(Window.WindowStateProperty)
                    .Subscribe(HandleObserver)
                ,
                window
                    .GetObservable(Window.SystemDecorationsProperty)
                    .Subscribe(HandleObserver)
                ,
                window
                    .GetObservable(Window.ExtendClientAreaTitleBarHeightHintProperty)
                    .Subscribe(HandleObserver)
                ,
                window
                    .GetObservable(Window.OffScreenMarginProperty)
                    .Subscribe(HandleObserver)
                ,
                window
                    .GetObservable(Window.ExtendClientAreaChromeHintsProperty)
                    .Subscribe(HandleObserver)
                ,
                window
                    .GetObservable(Window.IsExtendedIntoWindowDecorationsProperty)
                    .Subscribe(HandleObserver)
                ,
                this
                    .GetObservable(LeftCaptionButtonRolesProperty)
                    .Subscribe(HandleObserver)
                ,
                this
                    .GetObservable(RightCaptionButtonRolesProperty)
                    .Subscribe(HandleObserver)
                ,
            }
            */
            ;

            var window = HostWindow;
            foreach (var property in _WINDOW_OBSERVE_PROPERTIES)
            {
                _disposables.Add(window
                    .GetObservable(property)
                    .Subscribe(HandleObserver)
                );
            }

            foreach (var property in _WINDOWCHROME_OBSERVE_PROPERTIES)
            {
                _disposables.Add(this
                    .GetObservable(property)
                    .Subscribe(HandleObserver)
                );
            }

            _disposables.Add(Disposable.Create(() => _disposables = null));
            RefreshElements();
        }
        private WindowChrome()
            : base()
        {}
        void HandleObserver<T>(T _)
            => RefreshElements();
    }
}