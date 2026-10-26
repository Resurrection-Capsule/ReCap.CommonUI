using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Threading;
using ReCap.CommonUI.Controls.AppearanceHacks;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public partial class WindowChrome
        : Layoutable
        , IDisposable
    {
        internal static readonly IWindowChromeFactoryImpl FACTORY = PlatformUtils.GetForPlatformByNameMatch<IWindowChromeFactoryImpl>();
        internal static T GetFactory<T>()
            where T
                : IWindowChromeFactoryImpl
            => (T)FACTORY;
        static readonly List<Window> AttachedWindows = new();




        static readonly Type _WINDOW_TYPE = typeof(Window);
        static readonly Type _WINDOWCHROME_TYPE = typeof(WindowChrome);
        static void AffectsAll(params AvaloniaProperty[] properties)
        {
            AffectsAll<WindowChrome>(properties);
            AffectsPseudoClasses_WindowChrome(properties
                .Where(p => p.OwnerType == _WINDOWCHROME_TYPE)
                .ToArray()
            );

            AffectsPseudoClasses_Window(properties
                .Where(p => p.OwnerType == _WINDOW_TYPE)
                .ToArray()
            );
            AffectsAll<Window>(properties);
            
        }
        static void AffectsAll<T>(params AvaloniaProperty[] properties)
            where T
                : Layoutable
        {
            AffectsMeasure<T>(properties);
            AffectsArrange<T>(properties);
            AffectsRender<T>(properties);
        }



#region Internal-use-only Properties
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


        static readonly AttachedProperty<bool> UpdateHackProperty =
            AvaloniaProperty.RegisterAttached<bool, Window, bool>("UpdateHack", false);
        static bool GetUpdateHack(Window window)
            => window.GetValue(UpdateHackProperty);
        static void SetUpdateHack(Window window, bool value)
            => window.SetValue(UpdateHackProperty, value);
        internal void UpdateHack()
        {
            var window = HostWindow;
            SetUpdateHack(window, !GetUpdateHack(window));
            window.InvalidateArrange();
            window.InvalidateMeasure();
            window.InvalidateVisual();
            window.UpdateLayout();
        }




        WeakReference<Window> _hostWindowRef = null;
        internal Window HostWindow
        {
            get => WeakReferenceHelper.PropertyGet(ref _hostWindowRef);
            private set => WeakReferenceHelper.PropertySet(ref _hostWindowRef, value);
        }


        ChromeElements _elements;
        internal ChromeElements Elements
        {
            get => _elements;
            private set => _elements = value;
        }
#endregion



#region Hint Properties
        /// <summary>
        /// Whether a <see cref="Window"/> would prefer to use managed or system chrome.
        /// </summary>
        public static readonly AttachedProperty<ManagedChromeHint> HintProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, ManagedChromeHint>("Hint", ManagedChromeHint.Auto);


        /// <summary>
        /// Gets the value of the Hint attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <returns>The value of the Hint attached property.</returns>
        public static ManagedChromeHint GetHint(Window window)
            => window.GetValue(HintProperty);


        /// <summary>
        /// Sets the value of the Hint attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="control">The <see cref="Window"/>.</param>
        /// <param name="value">The value of the Hint attached property.</param>
        public static void SetHint(Window window, ManagedChromeHint value)
            => window.SetValue(HintProperty, value);




        /// <summary>
        /// Whether a <see cref="Window"/>'s managed chrome should show its <see cref="Window.Title"/>.
        /// </summary>
        public static readonly AttachedProperty<ManagedChromeElementHint> ShowTitleHintProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, ManagedChromeElementHint>("ShowTitleHint", ManagedChromeElementHint.Auto);


        /// <summary>
        /// Gets the value of the ShowTitleHint attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <returns>The value of the ShowTitleHint attached property.</returns>
        public static ManagedChromeElementHint GetShowTitleHint(Window window)
            => window.GetValue(ShowTitleHintProperty);


        /// <summary>
        /// Sets the value of the ShowTitleHint attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <param name="value">The value of the ShowTitleHint attached property.</param>
        public static void SetShowTitleHint(Window window, ManagedChromeElementHint value)
            => window.SetValue(ShowTitleHintProperty, value);




        /// <summary>
        /// Whether a <see cref="Window"/>'s managed chrome should show its <see cref="Window.Icon"/>.
        /// </summary>
        /// <remarks>
        /// Only applicable if the <see cref="CaptionButtonRole.WindowMenu"/> is the last element of <see cref="LeftCaptionButtonRoles"/> and/or the first element of <see cref="RightCaptionButtonRoles"/>, since <see cref="Window.Icon"/> is displayed on a <see cref="CaptionButtonRole.WindowMenu"/> button, if at all.
        /// </remarks>
        public static readonly AttachedProperty<ManagedChromeElementHint> ShowIconHintProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, ManagedChromeElementHint>("ShowIconHint", ManagedChromeElementHint.Auto);


        /// <summary>
        /// Gets the value of the ShowIconHint attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <returns>The value of the ShowIconHint attached property.</returns>
        public static ManagedChromeElementHint GetShowIconHint(Window window)
            => window.GetValue(ShowIconHintProperty);


        /// <summary>
        /// Sets the value of the ShowIconHint attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <param name="value">The value the the ShowIconHint attached property.</param>
        public static void SetShowIconHint(Window window, ManagedChromeElementHint value)
            => window.SetValue(ShowIconHintProperty, value);
#endregion



#region Attached Properties
#if CAPTIONBUTTONROLES_WIP
        public static readonly AttachedProperty<bool> ShowOnAllDesktopsProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, bool>("ShowOnAllDesktops", false, coerce: ShowOnAllDesktopsProperty_Coerce);
        static bool ShowOnAllDesktopsProperty_Coerce(AvaloniaObject o, bool proposedValue)
        {
            if (o is not Window window)
                goto fail;
            else if (!window.TryGetChrome(out WindowChrome chrome))
                goto fail;
            else
                return chrome.PlatformImpl.CoerceShowOnAllDesktops(proposedValue);

            fail:
            return false;
        }
        public static bool GetShowOnAllDesktops(Window window)
            => window.GetValue(ShowOnAllDesktopsProperty);
        public static void SetShowOnAllDesktops(Window window, bool value)
            => window.SetValue(ShowOnAllDesktopsProperty, value);


        public static readonly AttachedProperty<bool> ShadedProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, bool>("Shaded", false, coerce: ShadedProperty_Coerce);
        static bool ShadedProperty_Coerce(AvaloniaObject o, bool proposedValue)
        {
            if (o is not Window window)
                goto fail;
            else if (!window.TryGetChrome(out WindowChrome chrome))
                goto fail;
            else
                return chrome.PlatformImpl.CoerceShaded(proposedValue);

            fail:
            return false;
        }
        public static bool GetShaded(Window window)
            => window.GetValue(ShadedProperty);
        public static void SetShaded(Window window, bool value)
            => window.SetValue(ShadedProperty, value);
#endif


        public static readonly AttachedProperty<bool> BottommostProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, bool>("Bottommost", false, coerce: BottommostProperty_Coerce);
        static bool BottommostProperty_Coerce(AvaloniaObject o, bool proposedValue)
        {
            if (o is not Window window)
                goto fail;
            else if (!window.TryGetChrome(out WindowChrome chrome))
                goto fail;
            else
                return chrome.PlatformImpl.CoerceBottommost(proposedValue);

            fail:
            return false;
        }
        public static bool GetBottommost(Window window)
            => window.GetValue(BottommostProperty);
        public static void SetBottommost(Window window, bool value)
            => window.SetValue(BottommostProperty, value);


        public static readonly AttachedProperty<CaptionButtonRolesPair?> OverrideCaptionButtonRolesProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, CaptionButtonRolesPair?>("OverrideCaptionButtonRoles", null);
        public static CaptionButtonRolesPair? GetOverrideCaptionButtonRoles(Window window)
            => window.GetValue(OverrideCaptionButtonRolesProperty);
        public static void SetOverrideCaptionButtonRoles(Window window, CaptionButtonRolesPair? value)
            => window.SetValue(OverrideCaptionButtonRolesProperty, value);


        public static readonly DirectProperty<WindowChrome, IEnumerable<CaptionButtonRole>> LeftCaptionButtonRolesProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, IEnumerable<CaptionButtonRole>>(nameof(LeftCaptionButtonRoles)
                , getter: o => o.LeftCaptionButtonRoles
            );
        IEnumerable<CaptionButtonRole> _leftCaptionButtonRoles = Array.Empty<CaptionButtonRole>();
        public IEnumerable<CaptionButtonRole> LeftCaptionButtonRoles
        {
            get => _leftCaptionButtonRoles;
            private set => SetAndRaise(LeftCaptionButtonRolesProperty, ref _leftCaptionButtonRoles, value);
        }


        public static readonly DirectProperty<WindowChrome, IEnumerable<CaptionButtonRole>> RightCaptionButtonRolesProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, IEnumerable<CaptionButtonRole>>(nameof(RightCaptionButtonRoles)
                , getter: o => o.RightCaptionButtonRoles
            );
        IEnumerable<CaptionButtonRole> _rightCaptionButtonRoles = Array.Empty<CaptionButtonRole>();
        public IEnumerable<CaptionButtonRole> RightCaptionButtonRoles
        {
            get => _rightCaptionButtonRoles;
            private set => SetAndRaise(RightCaptionButtonRolesProperty, ref _rightCaptionButtonRoles, value);
        }




        const bool _DEFAULT_ReserveCaptionArea = true;
        public static readonly StyledProperty<bool> ReserveCaptionAreaProperty
            = AvaloniaProperty.Register<WindowChrome, bool>(nameof(ReserveCaptionArea), _DEFAULT_ReserveCaptionArea);
        public bool ReserveCaptionArea
        {
            get => GetValue(ReserveCaptionAreaProperty);
            set => SetValue(ReserveCaptionAreaProperty, value);
        }


        public static readonly AttachedProperty<double> DefaultCaptionAreaHeightProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, double>("DefaultCaptionAreaHeight", 1d);
        public static double GetDefaultCaptionAreaHeight(Window window)
            => window.GetValue(DefaultCaptionAreaHeightProperty);
        public static void SetDefaultCaptionAreaHeight(Window window, double value)
            => window.SetValue(DefaultCaptionAreaHeightProperty, value);


        public static readonly StyledProperty<double?> CaptionAreaHeightHintProperty
            = AvaloniaProperty.Register<WindowChrome, double?>(nameof(CaptionAreaHeightHint), null);
        public double? CaptionAreaHeightHint
        {
            get => GetValue(CaptionAreaHeightHintProperty);
            set => SetValue(CaptionAreaHeightHintProperty, value);
        }


        const double _DEFAULT_CaptionAreaHeight = 0d;
        public static readonly DirectProperty<WindowChrome, double> CaptionAreaHeightProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, double>(nameof(CaptionAreaHeight)
                , getter: o => o.CaptionAreaHeight
                , unsetValue: _DEFAULT_CaptionAreaHeight
            );
        double _captionAreaHeight = _DEFAULT_CaptionAreaHeight;
        public double CaptionAreaHeight
        {
            get => _captionAreaHeight;
            private set => SetAndRaise(CaptionAreaHeightProperty, ref _captionAreaHeight, value);
        }
#endregion
        IWindowChromeImpl _platformImpl;
        internal IWindowChromeImpl PlatformImpl
        {
            get => _platformImpl;
            private set => _platformImpl = value;
        }




        static readonly IEnumerable<AvaloniaProperty> _WINDOW_OBSERVE_PROPERTIES;
        static readonly IEnumerable<AvaloniaProperty> _WINDOWCHROME_OBSERVE_PROPERTIES;
        static WindowChrome()
        {
            AffectsAll(new AvaloniaProperty[]
            {
                LeftCaptionButtonRolesProperty,
                RightCaptionButtonRolesProperty,
                ReserveCaptionAreaProperty,
                DefaultCaptionAreaHeightProperty,
                CaptionAreaHeightHintProperty,
                OverrideCaptionButtonRolesProperty,
            });

            ReserveCaptionAreaProperty.Changed.AddClassHandler<WindowChrome>((s, e) => s.OnCaptionAreaPropertiesChanged(e.GetNewValue<bool>(), s.CaptionAreaHeightHint, GetDefaultCaptionAreaHeight(s.HostWindow)));
            DefaultCaptionAreaHeightProperty.Changed.AddClassHandler<Window>((s, e) =>
            {
                WindowChrome chrome = GetChrome(s);
                chrome?.OnCaptionAreaPropertiesChanged(chrome.ReserveCaptionArea, chrome.CaptionAreaHeightHint, e.GetNewValue<double>());
            });
            CaptionAreaHeightHintProperty.Changed.AddClassHandler<WindowChrome>((s, e) => s.OnCaptionAreaPropertiesChanged(s.ReserveCaptionArea, e.GetNewValue<double?>(), GetDefaultCaptionAreaHeight(s.HostWindow)));

            OverrideCaptionButtonRolesProperty.Changed.AddClassHandler<Window>((s, e) =>
            {
                WindowChrome chrome = GetChrome(s);

                if (e.TryGetNewValue(out CaptionButtonRolesPair? newRoles) && newRoles.HasValue)
                    chrome.RefreshCaptionButtonRoles(newRoles.Value);
                else
                    chrome.RefreshOverrideCaptionButtonRoles();
            });




            ShowIconHintProperty.Changed.AddClassHandler<Window>((s, e) => GetChrome(s)?.RefreshShowIcon());
            ShowTitleHintProperty.Changed.AddClassHandler<Window>((s, e) => GetChrome(s)?.RefreshShowTitle());
            HintProperty.Changed.AddClassHandler<Window>((s, e) => RefreshHint(s, e.GetNewValue<ManagedChromeHint>()));

            AffectsAll(new AvaloniaProperty[]
            {
                ShowIconHintProperty,
                ShowTitleHintProperty,
                IsChromeManagedProperty,
                IsTitleVisibleProperty,
                IsIconVisibleProperty,
                ReserveCaptionAreaProperty,
                LeftCaptionButtonsWidthProperty,
                CosmeticLeftCaptionButtonsWidthProperty,
                RightCaptionButtonsWidthProperty,
                CosmeticRightCaptionButtonsWidthProperty,
            });

            AffectsAll(UpdateHackProperty);
            MakeAvailableProperty.Changed.AddClassHandler<Window>((s, e) =>
            {
                (bool oldValue, bool newValue) = e.GetOldAndNewValue<bool>();
                if (newValue == oldValue)
                    return;
                else if (newValue == AttachedWindows.Contains(s))
                    return;
                else if (newValue)
                    AttachWindows(s);
                else
                    DetachWindows(s);
            });


            _WINDOW_OBSERVE_PROPERTIES = new AvaloniaProperty[]
            {
                WindowBase.IsActiveProperty,
                Window.WindowStateProperty,
                Window.SystemDecorationsProperty,
                Window.OffScreenMarginProperty,
                Window.IsExtendedIntoWindowDecorationsProperty,
                Window.ExtendClientAreaTitleBarHeightHintProperty,
                Window.ExtendClientAreaChromeHintsProperty,
                Window.WindowDecorationMarginProperty,
                TopLevel.AutoSafeAreaPaddingProperty,
                TopLevel.FrameSizeProperty,
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
                IsChromeManagedProperty,
                LeftCaptionButtonsWidthProperty,
                CosmeticLeftCaptionButtonsWidthProperty,
                RightCaptionButtonsWidthProperty,
                CosmeticRightCaptionButtonsWidthProperty,
                LeftCaptionButtonRolesProperty,
                RightCaptionButtonRolesProperty,
                IsIconVisibleProperty,
                IsTitleVisibleProperty,
                ReserveCaptionAreaProperty,
                CaptionAreaHeightProperty,
            };
        }


        static void RefreshShowIconAndTitle(Window window)
        {
            if (!window.TryGetChrome(out WindowChrome chrome))
                return;

            chrome.RefreshShowIcon();
            chrome.RefreshShowTitle();
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
            window.Closed -= Window_Closed;
        }


        static void Window_Closed(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            DetachWindows(window);
        }


        internal static void RefreshHint(Window window, ManagedChromeHint? hint = null)
        {
            WindowChrome chrome = GetChrome(window);
            bool useManagedChrome = chrome.PlatformImpl.GetDesiredManagedChrome(hint ?? GetHint(window));
            chrome.PlatformImpl.ApplyDesiredManagedChrome(useManagedChrome
                , v => chrome.IsChromeManaged = v
            );

            //RefreshShowIconAndTitle(window);
            Dispatcher.UIThread.Post(() => RefreshShowIconAndTitle(window));
        }




        CompositeDisposable _disposables = null;
        private WindowChrome()
            : base()
        {}
        protected WindowChrome(Window window)
            : this()
        {
            HostWindow = window;
            Elements ??= new(this);
            PlatformImpl = FACTORY.CreateImpl(this);
            Dispatcher.UIThread.Post(() =>
            {
                _disposables = new(BuildDisposables(this));
                Elements.Refresh();
            });
            RefreshOverrideCaptionButtonRoles();
            RefreshCaptionAreaProperties();
        }
        static IEnumerable<IDisposable> BuildDisposables(WindowChrome chrome)
        {
            var window = chrome.HostWindow;
            foreach (var property in _WINDOW_OBSERVE_PROPERTIES)
            {
                yield return window
                    .GetObservable(property)
                    .Subscribe(chrome.HandleObserver)
                ;
            }

            foreach (var property in _WINDOWCHROME_OBSERVE_PROPERTIES)
            {
                yield return chrome
                    .GetObservable(property)
                    .Subscribe(chrome.HandleObserver)
                ;
            }
        }
        void HandleObserver<T>(T _)
            => Elements.Refresh();



        void RefreshCaptionAreaProperties()
            => OnCaptionAreaPropertiesChanged(ReserveCaptionArea, CaptionAreaHeight, GetDefaultCaptionAreaHeight(HostWindow));


        void OnCaptionAreaPropertiesChanged(bool reserve, double? heightHint, double defaultHeight)
            => CaptionAreaHeight = reserve
                ? heightHint ?? defaultHeight
                : 0d
            ;


        void RefreshOverrideCaptionButtonRoles()
            => RefreshCaptionButtonRoles(GetOverrideCaptionButtonRoles(HostWindow) ?? PlatformDefaultCaptionButtonRoles);
        void RefreshCaptionButtonRoles(CaptionButtonRolesPair roles)
        {
            LeftCaptionButtonRoles = roles.Left;
            RightCaptionButtonRoles = roles.Right;
        }



        internal void AttachElement(IPseudoClassesProvider element)
            => Elements.Attach(element);


        internal void DetachElement(IPseudoClassesProvider element)
            => Elements.Detach(element);



        public virtual void Dispose()
        {
            _disposables?.Dispose();
            _disposables = null;

            HostWindow = null;

            Elements.Dispose();
            Elements = null;

            PlatformImpl?.Dispose();
            PlatformImpl = null;
        }
    }
}