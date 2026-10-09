using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using ReCap.CommonUI.Attached.WindowChrome;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    [PseudoClasses(WindowChrome.PSEUD_MINIMIZED, WindowChrome.PSEUD_NORMAL, WindowChrome.PSEUD_MAXIMIZED, WindowChrome.PSEUD_FULLSCREEN)]
    public sealed class TitleBarUnderlay
        : TemplatedControl
        , IChromeElement
    {
#region Properties
        public static readonly DirectProperty<TitleBarUnderlay, bool> IsTitleBarVisibleProperty
            = AvaloniaProperty.RegisterDirect<TitleBarUnderlay, bool>(
                nameof(IsTitleBarVisible)
                , s => s.IsTitleBarVisible
            );
        bool _isTitleBarVisible = false;
        public bool IsTitleBarVisible
        {
            get => _isTitleBarVisible;
            internal set => SetAndRaise(IsTitleBarVisibleProperty, ref _isTitleBarVisible, value);
        }
#endregion




        Window _hostWindow = null;
        TitleBar2 _hostTitleBar = null;
        IDisposable _windowDisposable = null;
        IDisposable _hostTitleBarDisposable = null;
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            if (e.Root is not Window hostWindow)
                return;
            
            _hostWindow = hostWindow;


            WindowChrome
                .GetChrome(_hostWindow)
                .AttachElement(this)
            ;
            _windowDisposable = new CompositeDisposable()
            {
                /*
                _hostWindow
                    .GetObservable(Window.WindowStateProperty)
                    .Subscribe(windowState => HostWindowState = windowState)
                ,
                */
                _hostWindow
                    .GetObservable(Window.ExtendClientAreaTitleBarHeightHintProperty)
                    .Subscribe(RefreshHeightHandler)
                ,
                /*
                _hostWindow
                    .GetObservable(WindowBase.IsActiveProperty)
                    .Subscribe(isActive => IsHostWindowActive = isActive)
                ,
                */
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
                        .Subscribe(RefreshHeightHandler)
                    ,
                    /*
                    _hostTitleBar
                        .GetObservable(TitleBar2.LeftWindowMenuPresenceProperty)
                        .Subscribe(presence => LeftWindowMenuPresence = presence)
                    ,
                    _hostTitleBar
                        .GetObservable(TitleBar2.RightWindowMenuPresenceProperty)
                        .Subscribe(presence => RightWindowMenuPresence = presence)
                    ,
                    */
                };
                RefreshHeight();
                IsTitleBarVisible = _hostTitleBar.IsVisible;
                break;
            }
        }
        void RefreshHeightHandler<T>(T _)
            => RefreshHeight();




        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            WindowChrome
                .GetChrome(_hostWindow)
                .DetachElement(this)
            ;
            
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
            if (WindowChrome.GetChrome(_hostWindow).IsChromeManaged)
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




#region IChromeElement
        IPseudoClasses IChromeElement.PseudoClasses
        {
            get => PseudoClasses;
        }


        public static readonly DirectProperty<TitleBarUnderlay, bool> IsHostWindowActiveProperty
            = TitleBar2.IsHostWindowActiveProperty.AddOwner<TitleBarUnderlay>(s => s.IsHostWindowActive);
        bool _isHostWindowActive = false;
        public bool IsHostWindowActive
        {
            get => _isHostWindowActive;
            private set => SetAndRaise(IsHostWindowActiveProperty, ref _isHostWindowActive, value);
        }
        bool IChromeElement.IsHostWindowActive
        {
            get => IsHostWindowActive;
            set => IsHostWindowActive = value;
        }


        public static readonly DirectProperty<TitleBarUnderlay, WindowState> HostWindowStateProperty
            = TitleBar2.HostWindowStateProperty.AddOwner<TitleBarUnderlay>(s => s.HostWindowState);
        WindowState _hostWindowState = WindowState.Normal;
        public WindowState HostWindowState
        {
            get => _hostWindowState;
            private set => SetAndRaise(HostWindowStateProperty, ref _hostWindowState, value);
        }
        WindowState IChromeElement.HostWindowState
        {
            get => HostWindowState;
            set => HostWindowState = value;
        }


        public static readonly DirectProperty<TitleBarUnderlay, bool> LeftHasWindowMenuAtMergeIndexProperty
            = TitleBar2.LeftHasWindowMenuAtMergeIndexProperty.AddOwner<TitleBarUnderlay>(o => o.LeftHasWindowMenuAtMergeIndex);
        bool _leftHasWindowMenuAtMergeIndex = TitleBar2.DEFAULT_LeftHasWindowMenuAtMergeIndex;
        public bool LeftHasWindowMenuAtMergeIndex
        {
            get => _leftHasWindowMenuAtMergeIndex;
            private set => SetAndRaise(LeftHasWindowMenuAtMergeIndexProperty, ref _leftHasWindowMenuAtMergeIndex, value);
        }
        bool IChromeElement.LeftHasWindowMenuAtMergeIndex
        {
            get => LeftHasWindowMenuAtMergeIndex;
            set => LeftHasWindowMenuAtMergeIndex = value;
        }


        public static readonly DirectProperty<TitleBarUnderlay, bool> LeftHasNonWindowMenuButtonsProperty
            = TitleBar2.LeftHasNonWindowMenuButtonsProperty.AddOwner<TitleBarUnderlay>(o => o.LeftHasNonWindowMenuButtons);
        bool _leftHasNonWindowMenuButtons = TitleBar2.DEFAULT_LeftHasNonWindowMenuButtons;
        public bool LeftHasNonWindowMenuButtons
        {
            get => _leftHasNonWindowMenuButtons;
            private set => SetAndRaise(LeftHasNonWindowMenuButtonsProperty, ref _leftHasNonWindowMenuButtons, value);
        }
        bool IChromeElement.LeftHasNonWindowMenuButtons
        {
            get => LeftHasNonWindowMenuButtons;
            set => LeftHasNonWindowMenuButtons = value;
        }


        public static readonly DirectProperty<TitleBarUnderlay, int> LeftButtonCountProperty
            = TitleBar2.LeftButtonCountProperty.AddOwner<TitleBarUnderlay>(o => o.LeftButtonCount);
        int _leftButtonCount = TitleBar2.DEFAULT_LeftButtonCount;
        public int LeftButtonCount
        {
            get => _leftButtonCount;
            private set => SetAndRaise(LeftButtonCountProperty, ref _leftButtonCount, value);
        }
        int IChromeElement.LeftButtonCount
        {
            get => LeftButtonCount;
            set => LeftButtonCount = value;
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
        WindowMenuPresence IChromeElement.LeftWindowMenuPresence
        {
            get => LeftWindowMenuPresence;
            set => LeftWindowMenuPresence = value;
        }




        public static readonly DirectProperty<TitleBarUnderlay, bool> RightHasWindowMenuAtMergeIndexProperty
            = TitleBar2.RightHasWindowMenuAtMergeIndexProperty.AddOwner<TitleBarUnderlay>(o => o.RightHasWindowMenuAtMergeIndex);
        bool _rightHasWindowMenuAtMergeIndex = TitleBar2.DEFAULT_RightHasWindowMenuAtMergeIndex;
        public bool RightHasWindowMenuAtMergeIndex
        {
            get => _rightHasWindowMenuAtMergeIndex;
            private set => SetAndRaise(RightHasWindowMenuAtMergeIndexProperty, ref _rightHasWindowMenuAtMergeIndex, value);
        }
        bool IChromeElement.RightHasWindowMenuAtMergeIndex
        {
            get => RightHasWindowMenuAtMergeIndex;
            set => RightHasWindowMenuAtMergeIndex = value;
        }


        public static readonly DirectProperty<TitleBarUnderlay, bool> RightHasNonWindowMenuButtonsProperty
            = TitleBar2.RightHasNonWindowMenuButtonsProperty.AddOwner<TitleBarUnderlay>(o => o.RightHasNonWindowMenuButtons);
        bool _rightHasNonWindowMenuButtons = TitleBar2.DEFAULT_RightHasNonWindowMenuButtons;
        public bool RightHasNonWindowMenuButtons
        {
            get => _rightHasNonWindowMenuButtons;
            private set => SetAndRaise(RightHasNonWindowMenuButtonsProperty, ref _rightHasNonWindowMenuButtons, value);
        }
        bool IChromeElement.RightHasNonWindowMenuButtons
        {
            get => RightHasNonWindowMenuButtons;
            set => RightHasNonWindowMenuButtons = value;
        }


        public static readonly DirectProperty<TitleBarUnderlay, int> RightButtonCountProperty
            = TitleBar2.RightButtonCountProperty.AddOwner<TitleBarUnderlay>(o => o.RightButtonCount);
        int _rightButtonCount = TitleBar2.DEFAULT_RightButtonCount;
        public int RightButtonCount
        {
            get => _rightButtonCount;
            private set => SetAndRaise(RightButtonCountProperty, ref _rightButtonCount, value);
        }
        int IChromeElement.RightButtonCount
        {
            get => RightButtonCount;
            set => RightButtonCount = value;
        }


        public static readonly DirectProperty<TitleBarUnderlay, WindowMenuPresence> RightWindowMenuPresenceProperty
            = TitleBar2.RightWindowMenuPresenceProperty.AddOwner<TitleBarUnderlay>(
                getter: o => o.RightWindowMenuPresence
                , unsetValue: TitleBar2.DEFAULT_RightWindowMenuPresence
            );
        WindowMenuPresence _rightWindowMenuPresence = TitleBar2.DEFAULT_RightWindowMenuPresence;
        public WindowMenuPresence RightWindowMenuPresence
        {
            get => _rightWindowMenuPresence;
            private set => SetAndRaise(RightWindowMenuPresenceProperty, ref _rightWindowMenuPresence, value);
        }
        WindowMenuPresence IChromeElement.RightWindowMenuPresence
        {
            get => RightWindowMenuPresence;
            set => RightWindowMenuPresence = value;
        }
        /*
        void IChromeElement.Update(bool isHostWindowActive, WindowState hostWindowState
            , bool leftHasNonWindowMenuButtons, bool leftHasWindowMenuAtMergeIndex, int leftButtonCount, WindowMenuPresence leftPresence
            , bool rightHasNonWindowMenuButtons, bool rightHasWindowMenuAtMergeIndex, int rightButtonCount, WindowMenuPresence rightPresence
        )
        {
            IsHostWindowActive = isHostWindowActive;
            HostWindowState = hostWindowState;
            PseudoClasses.ApplyWindowState(hostWindowState);

            LeftHasNonWindowMenuButtons = leftHasNonWindowMenuButtons;
            LeftHasWindowMenuAtMergeIndex = leftHasWindowMenuAtMergeIndex;
            LeftButtonCount = leftButtonCount;
            LeftWindowMenuPresence = leftPresence;

            RightHasNonWindowMenuButtons = rightHasNonWindowMenuButtons;
            RightHasWindowMenuAtMergeIndex = rightHasWindowMenuAtMergeIndex;
            RightButtonCount = rightButtonCount;
            RightWindowMenuPresence = rightPresence;
        }
        */
#endregion
    }
}
