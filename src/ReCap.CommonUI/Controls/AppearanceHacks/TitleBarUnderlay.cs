using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using ReCap.CommonUI.Attached.WindowChrome;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    public sealed class TitleBarUnderlay
        : TemplatedControl
    {
#region Properties
        public static readonly DirectProperty<TitleBarUnderlay, bool> IsTitleBarVisibleProperty =
            AvaloniaProperty.RegisterDirect<TitleBarUnderlay, bool>(
                nameof(IsTitleBarVisible)
                , s => s.IsTitleBarVisible
            );
        bool _isTitleBarVisible = false;
        public bool IsTitleBarVisible
        {
            get => _isTitleBarVisible;
            internal set => SetAndRaise(IsTitleBarVisibleProperty, ref _isTitleBarVisible, value);
        }


        public static readonly DirectProperty<TitleBarUnderlay, bool> IsHostWindowActiveProperty =
            AvaloniaProperty.RegisterDirect<TitleBarUnderlay, bool>(
                nameof(IsHostWindowActive)
                , s => s.IsHostWindowActive
            );
        bool _isHostWindowActive = false;
        public bool IsHostWindowActive
        {
            get => _isHostWindowActive;
            internal set => SetAndRaise(IsHostWindowActiveProperty, ref _isHostWindowActive, value);
        }


        public static readonly DirectProperty<TitleBarUnderlay, WindowState> HostWindowStateProperty =
            AvaloniaProperty.RegisterDirect<TitleBarUnderlay, WindowState>(
                nameof(HostWindowState)
                , s => s.HostWindowState
            );
        WindowState _hostWindowState = WindowState.Normal;
        public WindowState HostWindowState
        {
            get => _hostWindowState;
            internal set => SetAndRaise(HostWindowStateProperty, ref _hostWindowState, value);
        }


        public static readonly DirectProperty<TitleBarUnderlay, WindowMenuPresence> LeftWindowMenuPresenceProperty
            = TitleBar2.LeftWindowMenuPresenceProperty.AddOwner<TitleBarUnderlay>(
                getter: o => o.LeftWindowMenuPresence
                , unsetValue: TitleBar2.DEFAULT_LeftWindowMenuPresence
            );
        WindowMenuPresence _leftWindowMenuPresence = TitleBar2.DEFAULT_LeftWindowMenuPresence;
        public WindowMenuPresence LeftWindowMenuPresence
        {
            get => _leftWindowMenuPresence;
            private set => SetAndRaise(LeftWindowMenuPresenceProperty, ref _leftWindowMenuPresence, value);
        }


        public static readonly DirectProperty<TitleBarUnderlay, WindowMenuPresence> RightWindowMenuPresenceProperty
            = TitleBar2.RightWindowMenuPresenceProperty.AddOwner<TitleBarUnderlay>(
                o => o.RightWindowMenuPresence
                , unsetValue: TitleBar2.DEFAULT_RightWindowMenuPresence
            );
        WindowMenuPresence _rightWindowMenuPresence = TitleBar2.DEFAULT_RightWindowMenuPresence;
        public WindowMenuPresence RightWindowMenuPresence
        {
            get => _rightWindowMenuPresence;
            private set => SetAndRaise(RightWindowMenuPresenceProperty, ref _rightWindowMenuPresence, value);
        }
#endregion




        Window _hostWindow = null;
        TitleBar2 _hostTitleBar = null;
        IDisposable _windowDisposable = null;
        IDisposable _hostTitleBarDisposable = null;
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            if (e.Root is not Window window)
                return;
            
            _hostWindow = window;
            _windowDisposable = new CompositeDisposable()
            {
                _hostWindow
                    .GetObservable(Window.WindowStateProperty)
                    .Subscribe(windowState => HostWindowState = windowState)
                ,
                _hostWindow
                    .GetObservable(Window.ExtendClientAreaTitleBarHeightHintProperty)
                    .Subscribe(_ => RefreshHeight())
                ,
                _hostWindow
                    .GetObservable(WindowBase.IsActiveProperty)
                    .Subscribe(isActive => IsHostWindowActive = isActive)
                ,
            };
            RefreshHeight();
            IsHostWindowActive = _hostWindow.IsActive;

            var chromeOverlay = ChromeOverlayLayer.GetOverlayLayer(_hostWindow.Presenter);
            if (chromeOverlay == null)
                return;
            
            var chromeChildren = chromeOverlay.Children;
            foreach (var chromeChild in chromeChildren)
            {
                if (chromeChild is not TitleBar2 hostTitleBar)
                    continue;

                _hostTitleBar = hostTitleBar;
                _hostTitleBarDisposable = new CompositeDisposable()
                {
                    _hostTitleBar
                        .GetObservable(IsVisibleProperty)
                        .Subscribe(isVisible =>
                        {
                            IsTitleBarVisible = isVisible;
                            RefreshHeight();
                        })
                    ,
                    _hostTitleBar
                        .GetObservable(BoundsProperty)
                        .Subscribe(_ => RefreshHeight())
                    ,
                    _hostTitleBar
                        .GetObservable(TitleBar2.LeftWindowMenuPresenceProperty)
                        .Subscribe(presence => LeftWindowMenuPresence = presence)
                    ,
                    _hostTitleBar
                        .GetObservable(TitleBar2.RightWindowMenuPresenceProperty)
                        .Subscribe(presence => RightWindowMenuPresence = presence)
                    ,
                };
                RefreshHeight();
                IsTitleBarVisible = _hostTitleBar.IsVisible;
                break;
            }
        }




        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            
            _windowDisposable?.Dispose();
            _windowDisposable = null;

            _hostWindow = null;


            _hostTitleBarDisposable?.Dispose();
            _hostTitleBarDisposable = null;

            _hostTitleBar = null;
        }


        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (e.ClickCount > 1)
                return;
            else if (TopLevel.GetTopLevel(this) is Window window)
                window.BeginMoveDrag(e);
        }


        void RefreshHeight()
        {
            if (_hostWindow == null)
                return;
            
            if (_hostTitleBar == null)
                return;

            // [TODO: account for negative margins on TitleBarUnderlay?]
            double height = _hostWindow.ExtendClientAreaTitleBarHeightHint;

            /*
            if (!WindowChrome.GetIsChromeManaged(_window))
                height -= WindowChrome.GetReservedCaptionHeight(_window);
            else if (height < 0d)
                height = _hostTitleBar.Bounds.Height;
            */
            if (WindowChrome.GetStateInfo(_hostWindow).IsChromeManaged)
            {
                if (height < 0d)
                    height = _hostTitleBar.Bounds.Height;
            }
            else
            {
                //height -= WindowChrome.GetReservedCaptionHeight(_window);
            }


            height = Math.Max(0d, height);
#if PRINT_PROPERTY_CHANGES
            Console.WriteLine($"{nameof(TitleBarUnderlay)}.{nameof(Height)}: '{Height}' => '{height}'");
#endif
            Height = height;
        }




        static TitleBarUnderlay()
        {
            Extensions.MakeControlTypeNonInteractive<TitleBarUnderlay>();
        }

        protected override Type StyleKeyOverride
            => typeof(TitleBarUnderlay);
    }
}
