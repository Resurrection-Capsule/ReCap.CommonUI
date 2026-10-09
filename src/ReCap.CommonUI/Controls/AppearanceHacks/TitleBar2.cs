using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using ReCap.CommonUI.Attached.WindowChrome;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    public enum WindowMenuPresence
        : byte
    {
        Irrelevant = 0x00,
        Lone = 0x01,
        Innermost = 0x02,
    }




#if TITLEBAR2_PSEUDOCLASSES
    [PseudoClasses(_PSEUD_MINIMIZED, _PSEUD_NORMAL, _PSEUD_MAXIMIZED, _PSEUD_FULLSCREEN)]
#else
    [PseudoClasses(WindowChrome.PSEUD_MINIMIZED, WindowChrome.PSEUD_NORMAL, WindowChrome.PSEUD_MAXIMIZED, WindowChrome.PSEUD_FULLSCREEN)]
#endif
    public sealed class TitleBar2
        : TitleBar
        , IChromeElement
    {
#if TITLEBAR2_PSEUDOCLASSES
        const string _PSEUD_MINIMIZED = ":minimized";
        const string _PSEUD_NORMAL = ":normal";
        const string _PSEUD_MAXIMIZED = ":maximized";
        const string _PSEUD_FULLSCREEN = ":fullscreen";
#endif

        const string _PART_CAPTIONBUTTONS = "PART_CaptionButtons";




#region Properties
        public static readonly DirectProperty<TitleBar2, bool> IsHostWindowActiveProperty =
            AvaloniaProperty.RegisterDirect<TitleBar2, bool>(
                nameof(IsHostWindowActive)
                , s => s.IsHostWindowActive
            );
        bool _isHostWindowActive = false;
        public bool IsHostWindowActive
        {
            get => _isHostWindowActive;
            internal set => SetAndRaise(IsHostWindowActiveProperty, ref _isHostWindowActive, value);
        }


        public static readonly DirectProperty<TitleBar2, WindowState> HostWindowStateProperty =
            AvaloniaProperty.RegisterDirect<TitleBar2, WindowState>(
                nameof(HostWindowState)
                , s => s.HostWindowState
            );
        WindowState _hostWindowState = WindowState.Normal;
        public WindowState HostWindowState
        {
            get => _hostWindowState;
            internal set => SetAndRaise(HostWindowStateProperty, ref _hostWindowState, value);
        }


        internal const bool DEFAULT_LeftHasWindowMenuAtMergeIndex = false;
        public static readonly DirectProperty<TitleBar2, bool> LeftHasWindowMenuAtMergeIndexProperty
            = AvaloniaProperty.RegisterDirect<TitleBar2, bool>(nameof(LeftHasWindowMenuAtMergeIndex)
                , getter: o => o.LeftHasWindowMenuAtMergeIndex
                , unsetValue: DEFAULT_LeftHasWindowMenuAtMergeIndex
            );
        bool _leftHasWindowMenuAtMergeIndex = DEFAULT_LeftHasWindowMenuAtMergeIndex;
        public bool LeftHasWindowMenuAtMergeIndex
        {
            get => _leftHasWindowMenuAtMergeIndex;
            private set => SetAndRaise(LeftHasWindowMenuAtMergeIndexProperty, ref _leftHasWindowMenuAtMergeIndex, value);
        }


        internal const bool DEFAULT_LeftHasNonWindowMenuButtons = false;
        public static readonly DirectProperty<TitleBar2, bool> LeftHasNonWindowMenuButtonsProperty
            = AvaloniaProperty.RegisterDirect<TitleBar2, bool>(nameof(LeftHasNonWindowMenuButtons)
                , getter: o => o.LeftHasNonWindowMenuButtons
                , unsetValue: DEFAULT_LeftHasNonWindowMenuButtons
            );
        bool _leftHasNonWindowMenuButtons = DEFAULT_LeftHasNonWindowMenuButtons;
        public bool LeftHasNonWindowMenuButtons
        {
            get => _leftHasNonWindowMenuButtons;
            private set => SetAndRaise(LeftHasNonWindowMenuButtonsProperty, ref _leftHasNonWindowMenuButtons, value);
        }


        internal const int DEFAULT_LeftButtonCount = 0;
        public static readonly DirectProperty<TitleBar2, int> LeftButtonCountProperty
            = AvaloniaProperty.RegisterDirect<TitleBar2, int>(nameof(LeftButtonCount)
                , getter: o => o.LeftButtonCount
                , unsetValue: DEFAULT_LeftButtonCount
            );
        int _leftButtonCount = DEFAULT_LeftButtonCount;
        public int LeftButtonCount
        {
            get => _leftButtonCount;
            private set => SetAndRaise(LeftButtonCountProperty, ref _leftButtonCount, value);
        }


        internal const WindowMenuPresence DEFAULT_LeftWindowMenuPresence = WindowMenuPresence.Irrelevant;
        public static readonly DirectProperty<TitleBar2, WindowMenuPresence> LeftWindowMenuPresenceProperty
            = AvaloniaProperty.RegisterDirect<TitleBar2, WindowMenuPresence>(nameof(LeftWindowMenuPresence)
                , getter: o => o.LeftWindowMenuPresence
                , unsetValue: DEFAULT_LeftWindowMenuPresence
            );
        WindowMenuPresence _leftWindowMenuPresence = DEFAULT_LeftWindowMenuPresence;
        public WindowMenuPresence LeftWindowMenuPresence
        {
            get => _leftWindowMenuPresence;
            private set => SetAndRaise(LeftWindowMenuPresenceProperty, ref _leftWindowMenuPresence, value);
        }


        internal const bool DEFAULT_RightHasWindowMenuAtMergeIndex = false;
        public static readonly DirectProperty<TitleBar2, bool> RightHasWindowMenuAtMergeIndexProperty
            = AvaloniaProperty.RegisterDirect<TitleBar2, bool>(nameof(RightHasWindowMenuAtMergeIndex)
                , getter: o => o.RightHasWindowMenuAtMergeIndex
                , unsetValue: DEFAULT_RightHasWindowMenuAtMergeIndex
            );
        bool _rightHasWindowMenuAtMergeIndex = DEFAULT_RightHasWindowMenuAtMergeIndex;
        public bool RightHasWindowMenuAtMergeIndex
        {
            get => _rightHasWindowMenuAtMergeIndex;
            private set => SetAndRaise(RightHasWindowMenuAtMergeIndexProperty, ref _rightHasWindowMenuAtMergeIndex, value);
        }


        internal const bool DEFAULT_RightHasNonWindowMenuButtons = false;
        public static readonly DirectProperty<TitleBar2, bool> RightHasNonWindowMenuButtonsProperty
            = AvaloniaProperty.RegisterDirect<TitleBar2, bool>(nameof(RightHasNonWindowMenuButtons)
                , getter: o => o.RightHasNonWindowMenuButtons
                , unsetValue: DEFAULT_RightHasNonWindowMenuButtons
            );
        bool _rightHasNonWindowMenuButtons = DEFAULT_RightHasNonWindowMenuButtons;
        public bool RightHasNonWindowMenuButtons
        {
            get => _rightHasNonWindowMenuButtons;
            private set => SetAndRaise(RightHasNonWindowMenuButtonsProperty, ref _rightHasNonWindowMenuButtons, value);
        }


        internal const int DEFAULT_RightButtonCount = 0;
        public static readonly DirectProperty<TitleBar2, int> RightButtonCountProperty
            = AvaloniaProperty.RegisterDirect<TitleBar2, int>(nameof(RightButtonCount)
                , getter: o => o.RightButtonCount
                , unsetValue: DEFAULT_RightButtonCount
            );
        int _rightButtonCount = DEFAULT_RightButtonCount;
        public int RightButtonCount
        {
            get => _rightButtonCount;
            private set => SetAndRaise(RightButtonCountProperty, ref _rightButtonCount, value);
        }


        internal const WindowMenuPresence DEFAULT_RightWindowMenuPresence = WindowMenuPresence.Irrelevant;
        public static readonly DirectProperty<TitleBar2, WindowMenuPresence> RightWindowMenuPresenceProperty
            = AvaloniaProperty.RegisterDirect<TitleBar2, WindowMenuPresence>(nameof(RightWindowMenuPresence)
                , getter: o => o.RightWindowMenuPresence
                , unsetValue: DEFAULT_RightWindowMenuPresence
            );
        WindowMenuPresence _rightWindowMenuPresence = DEFAULT_RightWindowMenuPresence;
        public WindowMenuPresence RightWindowMenuPresence
        {
            get => _rightWindowMenuPresence;
            private set => SetAndRaise(RightWindowMenuPresenceProperty, ref _rightWindowMenuPresence, value);
        }
#endregion




#nullable enable
        CompositeDisposable? _disposables;
        CaptionButtonsBroker? _captionButtons;
#nullable restore
        void UpdateSize(Window window)
        {
            /*
            Margin = new Thickness(
                window.OffScreenMargin.Left,
                window.OffScreenMargin.Top,
                window.OffScreenMargin.Right,
                window.OffScreenMargin.Bottom);
            */
            Margin = new(0d);
            if (window.WindowState != WindowState.FullScreen)
            {
                Height = Math.Max(Math.Max(0, MinHeight), window.WindowDecorationMargin.Top);

                if (_captionButtons != null)
                {
                    _captionButtons.Height = Height;
                }
            }

            IsVisible = WindowChrome.GetChrome(window).IsChromeManaged;
        }


        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            _captionButtons?.Detach();
            _captionButtons = e.NameScope.Get<CaptionButtonsBroker>(_PART_CAPTIONBUTTONS);


            if (VisualRoot is not Window window)
                return;

            _captionButtons?.Attach(window);
            UpdateSize(window);
        }


        Window _hostWindow = null;
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            if (VisualRoot is not Window hostWindow)
                return;

            _hostWindow = hostWindow;

            var chrome = WindowChrome.GetChrome(_hostWindow);
            chrome.AttachElement(this);
            _disposables = new CompositeDisposable(6)
            {
                _hostWindow
                    .GetObservable(Window.WindowDecorationMarginProperty)
                    .Subscribe(HostWindow_PropertyChanged)
                ,
                chrome
                    .GetObservable(WindowChrome.IsChromeManagedProperty)
                    .Subscribe(HostWindow_PropertyChanged)
                ,
                /*
                _hostWindow
                    .GetObservable(Window.SystemDecorationsProperty)
                    .Subscribe(HostWindow_PropertyChanged)
                ,
                _hostWindow
                    .GetObservable(Window.ExtendClientAreaTitleBarHeightHintProperty)
                    .Subscribe(HostWindow_PropertyChanged)
                ,
                _hostWindow
                    .GetObservable(Window.OffScreenMarginProperty)
                    .Subscribe(HostWindow_PropertyChanged)
                ,
                _hostWindow
                    .GetObservable(Window.ExtendClientAreaChromeHintsProperty)
                    .Subscribe(HostWindow_PropertyChanged)
                ,
                _hostWindow
                    .GetObservable(Window.WindowStateProperty)
#if TITLEBAR2_PSEUDOCLASSES
                    .Subscribe(x =>
                    {
                        PseudoClasses.Set(_PSEUD_MINIMIZED, x == WindowState.Minimized);
                        PseudoClasses.Set(_PSEUD_NORMAL, x == WindowState.Normal);
                        PseudoClasses.Set(_PSEUD_MAXIMIZED, x == WindowState.Maximized);
                        PseudoClasses.Set(_PSEUD_FULLSCREEN, x == WindowState.FullScreen);
                        HostWindowRefresh();
                    })
#else
                    .Subscribe(HostWindow_PropertyChanged)
#endif
                ,
                */
                _hostWindow
                    .GetObservable(Window.IsExtendedIntoWindowDecorationsProperty)
                    .Subscribe(HostWindow_PropertyChanged)
                ,
            };

            HostWindowRefresh();
        }
        void HostWindow_PropertyChanged<D>(D _)
            => HostWindowRefresh();
        void HostWindowRefresh()
            => UpdateSize(_hostWindow);


        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            WindowChrome
                .GetChrome(_hostWindow)
                .DetachElement(this)
            ;

            _disposables?.Dispose();

            _captionButtons?.Detach();
            _captionButtons = null;
            _hostWindow = null;
        }




#region IChromeElement
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
        bool IChromeElement.IsHostWindowActive
        {
            get => IsHostWindowActive;
            set => IsHostWindowActive = value;
        }
        WindowState IChromeElement.HostWindowState
        {
            get => HostWindowState;
            set => HostWindowState = value;
        }


        bool IChromeElement.LeftHasWindowMenuAtMergeIndex
        {
            get => LeftHasWindowMenuAtMergeIndex;
            set => LeftHasWindowMenuAtMergeIndex = value;
        }
        bool IChromeElement.LeftHasNonWindowMenuButtons
        {
            get => LeftHasNonWindowMenuButtons;
            set => LeftHasNonWindowMenuButtons = value;
        }
        int IChromeElement.LeftButtonCount
        {
            get => LeftButtonCount;
            set => LeftButtonCount = value;
        }
        WindowMenuPresence IChromeElement.LeftWindowMenuPresence
        {
            get => LeftWindowMenuPresence;
            set => LeftWindowMenuPresence = value;
        }


        bool IChromeElement.RightHasWindowMenuAtMergeIndex
        {
            get => RightHasWindowMenuAtMergeIndex;
            set => RightHasWindowMenuAtMergeIndex = value;
        }
        bool IChromeElement.RightHasNonWindowMenuButtons
        {
            get => RightHasNonWindowMenuButtons;
            set => RightHasNonWindowMenuButtons = value;
        }
        int IChromeElement.RightButtonCount
        {
            get => RightButtonCount;
            set => RightButtonCount = value;
        }
        WindowMenuPresence IChromeElement.RightWindowMenuPresence
        {
            get => RightWindowMenuPresence;
            set => RightWindowMenuPresence = value;
        }




        IPseudoClasses IChromeElement.PseudoClasses
        {
            get => PseudoClasses;
        }
#endregion
    }
}