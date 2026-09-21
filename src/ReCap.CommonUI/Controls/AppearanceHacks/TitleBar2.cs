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




    [PseudoClasses(_PSEUD_MINIMIZED, _PSEUD_NORMAL, _PSEUD_MAXIMIZED, _PSEUD_FULLSCREEN, _LEFT_SIDE_BUTTONS)]
    public sealed class TitleBar2
        : TitleBar
    {
        const string _PSEUD_MINIMIZED = ":minimized";
        const string _PSEUD_NORMAL = ":normal";
        const string _PSEUD_MAXIMIZED = ":maximized";
        const string _PSEUD_FULLSCREEN = ":fullscreen";
        const string _LEFT_SIDE_BUTTONS = ":left-side-buttons";

        const string _PART_CAPTIONBUTTONS = "PART_CaptionButtons";




        const bool _DEFAULT_LeftHasWindowMenuAtMergeIndex = false;
        public static readonly DirectProperty<TitleBar2, bool> LeftHasWindowMenuAtMergeIndexProperty
            = AvaloniaProperty.RegisterDirect<TitleBar2, bool>(nameof(LeftHasWindowMenuAtMergeIndex)
                , getter: o => o.LeftHasWindowMenuAtMergeIndex
                , unsetValue: _DEFAULT_LeftHasWindowMenuAtMergeIndex
            );
        bool _leftHasWindowMenuAtMergeIndex = _DEFAULT_LeftHasWindowMenuAtMergeIndex;
        public bool LeftHasWindowMenuAtMergeIndex
        {
            get => _leftHasWindowMenuAtMergeIndex;
            private set => SetAndRaise(LeftHasWindowMenuAtMergeIndexProperty, ref _leftHasWindowMenuAtMergeIndex, value);
        }


        const bool _DEFAULT_LeftHasNonWindowMenuButtons = false;
        public static readonly DirectProperty<TitleBar2, bool> LeftHasNonWindowMenuButtonsProperty
            = AvaloniaProperty.RegisterDirect<TitleBar2, bool>(nameof(LeftHasNonWindowMenuButtons)
                , getter: o => o.LeftHasNonWindowMenuButtons
                , unsetValue: _DEFAULT_LeftHasNonWindowMenuButtons
            );
        bool _leftHasNonWindowMenuButtons = _DEFAULT_LeftHasNonWindowMenuButtons;
        public bool LeftHasNonWindowMenuButtons
        {
            get => _leftHasNonWindowMenuButtons;
            private set => SetAndRaise(LeftHasNonWindowMenuButtonsProperty, ref _leftHasNonWindowMenuButtons, value);
        }


        const int _DEFAULT_LeftButtonCount = 0;
        public static readonly DirectProperty<TitleBar2, int> LeftButtonCountProperty
            = AvaloniaProperty.RegisterDirect<TitleBar2, int>(nameof(LeftButtonCount)
                , getter: o => o.LeftButtonCount
                , unsetValue: _DEFAULT_LeftButtonCount
            );
        int _leftButtonCount = _DEFAULT_LeftButtonCount;
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


        internal const bool DEFAULT_rightHasWindowMenuAtMergeIndex = false;
        public static readonly DirectProperty<TitleBar2, bool> RightHasWindowMenuAtMergeIndexProperty
            = AvaloniaProperty.RegisterDirect<TitleBar2, bool>(nameof(RightHasWindowMenuAtMergeIndex)
                , getter: o => o.RightHasWindowMenuAtMergeIndex
                , unsetValue: DEFAULT_rightHasWindowMenuAtMergeIndex
            );
        bool _rightHasWindowMenuAtMergeIndex = DEFAULT_rightHasWindowMenuAtMergeIndex;
        public bool RightHasWindowMenuAtMergeIndex
        {
            get => _rightHasWindowMenuAtMergeIndex;
            private set => SetAndRaise(RightHasWindowMenuAtMergeIndexProperty, ref _rightHasWindowMenuAtMergeIndex, value);
        }


        const bool _DEFAULT_rightHasNonWindowMenuButtons = false;
        public static readonly DirectProperty<TitleBar2, bool> RightHasNonWindowMenuButtonsProperty
            = AvaloniaProperty.RegisterDirect<TitleBar2, bool>(nameof(RightHasNonWindowMenuButtons)
                , getter: o => o.RightHasNonWindowMenuButtons
                , unsetValue: _DEFAULT_rightHasNonWindowMenuButtons
            );
        bool _rightHasNonWindowMenuButtons = _DEFAULT_rightHasNonWindowMenuButtons;
        public bool RightHasNonWindowMenuButtons
        {
            get => _rightHasNonWindowMenuButtons;
            private set => SetAndRaise(RightHasNonWindowMenuButtonsProperty, ref _rightHasNonWindowMenuButtons, value);
        }


        const int _DEFAULT_RightButtonCount = 0;
        public static readonly DirectProperty<TitleBar2, int> RightButtonCountProperty
            = AvaloniaProperty.RegisterDirect<TitleBar2, int>(nameof(RightButtonCount)
                , getter: o => o.RightButtonCount
                , unsetValue: _DEFAULT_RightButtonCount
            );
        int _rightButtonCount = _DEFAULT_RightButtonCount;
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

            IsVisible = ManagedWindowChrome.GetIsChromeManaged(window);
        }


        WindowMenuPresence GetPresence(Window window, bool hasWindowMenuAtMergeIndex, bool hasNonWindowMenuButtons, int count)
        {
            if (count <= 0)
                goto irrelevant;
            else if (!ManagedWindowChrome.GetShowIcon(window))
                goto irrelevant;
            else if (!hasNonWindowMenuButtons)
                return WindowMenuPresence.Lone;
            else if (hasWindowMenuAtMergeIndex && hasNonWindowMenuButtons)
                return WindowMenuPresence.Innermost;

            irrelevant:
            return WindowMenuPresence.Irrelevant;
        }
        internal void UpdateLeftClusterProperties(Window window, bool hasWindowMenuAtMergeIndex, bool hasNonWindowMenuButtons, int count)
        {
            LeftHasWindowMenuAtMergeIndex = hasWindowMenuAtMergeIndex;
            LeftHasNonWindowMenuButtons = hasNonWindowMenuButtons;
            LeftButtonCount = count;
            LeftWindowMenuPresence = GetPresence(window
                , hasWindowMenuAtMergeIndex
                , hasNonWindowMenuButtons
                , count
            );
        }
        internal void UpdateRightClusterProperties(Window window, bool hasWindowMenuAtMergeIndex, bool hasNonWindowMenuButtons, int count)
        {
            RightHasWindowMenuAtMergeIndex = hasWindowMenuAtMergeIndex;
            RightHasNonWindowMenuButtons = hasNonWindowMenuButtons;
            RightButtonCount = count;
            RightWindowMenuPresence = GetPresence(window
                , hasWindowMenuAtMergeIndex
                , hasNonWindowMenuButtons
                , count
            );
        }
        internal void UpdateClustersProperties()
        {
            if (_captionButtons == null)
                return;

            var leftCluster = _captionButtons.LeftCluster;
            if (leftCluster != null)
                UpdateLeftClusterProperties(_hostWindow, leftCluster.HasWindowMenuAtMergeIndex, leftCluster.HasNonWindowMenuButtons, leftCluster.ItemCount);

            var rightCluster = _captionButtons.RightCluster;
            if (rightCluster != null)
                UpdateRightClusterProperties(_hostWindow, rightCluster.HasWindowMenuAtMergeIndex, rightCluster.HasNonWindowMenuButtons, rightCluster.ItemCount);
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
            _disposables = new CompositeDisposable(6)
            {
                _hostWindow.GetObservable(Window.WindowDecorationMarginProperty)
                    .Subscribe(HostWindow_PropertyChanged),
                _hostWindow.GetObservable(ManagedWindowChrome.IsChromeManagedProperty)
                    .Subscribe(HostWindow_PropertyChanged),
                _hostWindow.GetObservable(Window.SystemDecorationsProperty)
                    .Subscribe(_ => HostWindowRefresh()),
                _hostWindow.GetObservable(Window.ExtendClientAreaTitleBarHeightHintProperty)
                    .Subscribe(_ => HostWindowRefresh()),
                _hostWindow.GetObservable(Window.OffScreenMarginProperty)
                    .Subscribe(HostWindow_PropertyChanged),
                _hostWindow.GetObservable(Window.ExtendClientAreaChromeHintsProperty)
                    .Subscribe(_ => HostWindowRefresh()),
                _hostWindow.GetObservable(Window.WindowStateProperty)
                    .Subscribe(x =>
                    {
                        PseudoClasses.Set(_PSEUD_MINIMIZED, x == WindowState.Minimized);
                        PseudoClasses.Set(_PSEUD_NORMAL, x == WindowState.Normal);
                        PseudoClasses.Set(_PSEUD_MAXIMIZED, x == WindowState.Maximized);
                        PseudoClasses.Set(_PSEUD_FULLSCREEN, x == WindowState.FullScreen);
                    }),
                _hostWindow.GetObservable(Window.IsExtendedIntoWindowDecorationsProperty)
                    .Subscribe(HostWindow_PropertyChanged),
            };

            HostWindowRefresh();
        }
        void HostWindow_PropertyChanged<D>(D _)
            => HostWindowRefresh();
        void HostWindowRefresh()
        {
            UpdateSize(_hostWindow);
            UpdateClustersProperties();
        }


        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            _disposables?.Dispose();

            _captionButtons?.Detach();
            _captionButtons = null;
            _hostWindow = null;
        }
    }
}