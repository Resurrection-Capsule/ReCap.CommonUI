using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    partial class WindowChrome
    {
        /// <summary>
        /// Whether a <see cref="Window"/> is currently using managed chrome.
        /// </summary>
        const bool _DEFAULT_IsChromeManaged = false;
        public static readonly DirectProperty<WindowChrome, bool> IsChromeManagedProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, bool>(nameof(IsChromeManaged)
                , getter: o => o.IsChromeManaged
                , unsetValue: _DEFAULT_IsChromeManaged
            );
        bool _isChromeManaged = _DEFAULT_IsChromeManaged;
        /// <summary>
        /// Whether a <see cref="Window"/> is currently using managed chrome.
        /// </summary>
        public bool IsChromeManaged
        {
            get => _isChromeManaged;
            internal set => SetAndRaise(IsChromeManagedProperty, ref _isChromeManaged, value);
        }


        /// <summary>
        /// Whether a <see cref="Window"/> is currently using managed chrome.
        /// </summary>
        const bool _DEFAULT_IsTitleVisible = true;
        public static readonly DirectProperty<WindowChrome, bool> IsTitleVisibleProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, bool>(nameof(IsTitleVisible)
                , getter: o => o.IsTitleVisible
                , unsetValue: _DEFAULT_IsTitleVisible
            );
        bool _isTitleVisible = _DEFAULT_IsTitleVisible;
        /// <summary>
        /// Whether a <see cref="Window"/> using managed chrome is showing its <see cref="Window.Title"/>.
        /// </summary>
        public bool IsTitleVisible
        {
            get => _isTitleVisible;
            internal set => SetAndRaise(IsTitleVisibleProperty, ref _isTitleVisible, value);
        }


        /// <summary>
        /// Whether a <see cref="Window"/> is currently using managed chrome.
        /// </summary>
        const bool _DEFAULT_IsIconVisible = true;
        public static readonly DirectProperty<WindowChrome, bool> IsIconVisibleProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, bool>(nameof(IsIconVisible)
                , getter: o => o.IsIconVisible
                , unsetValue: _DEFAULT_IsIconVisible
            );
        bool _isIconVisible = _DEFAULT_IsIconVisible;
        /// <summary>
        /// Whether a <see cref="Window"/> using managed chrome is showing its <see cref="Window.Icon"/>.
        /// </summary>
        public bool IsIconVisible
        {
            get => _isIconVisible;
            internal set => SetAndRaise(IsIconVisibleProperty, ref _isIconVisible, value);
        }




        const double _DEFAULT_BridgedCaptionButtonsWidth = 0d;
        /// <summary>
        /// Bridged width of a <see cref="Window"/>'s managed chrome's left caption buttons.
        /// </summary>
        public static readonly DirectProperty<WindowChrome, double> LeftCaptionButtonsWidthProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, double>(nameof(LeftCaptionButtonsWidth)
                , getter: o => o.LeftCaptionButtonsWidth
                , unsetValue: _DEFAULT_BridgedCaptionButtonsWidth
            );
        double _leftCaptionButtonsWidth = _DEFAULT_BridgedCaptionButtonsWidth;
        /// <summary>
        /// Whether a <see cref="Window"/> using managed chrome is showing its <see cref="Window.Icon"/>.
        /// </summary>
        public double LeftCaptionButtonsWidth
        {
            get => _leftCaptionButtonsWidth;
            internal set => SetAndRaise(LeftCaptionButtonsWidthProperty, ref _leftCaptionButtonsWidth, value);
        }


        const double _DEFAULT_CosmeticLeftCaptionButtonsWidth = 0d;
        public static readonly DirectProperty<WindowChrome, double> CosmeticLeftCaptionButtonsWidthProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, double>(nameof(CosmeticLeftCaptionButtonsWidth)
                , getter: o => o.CosmeticLeftCaptionButtonsWidth
                , unsetValue: _DEFAULT_BridgedCaptionButtonsWidth
            );
        double _cosmeticLeftCaptionButtonsWidth = _DEFAULT_CosmeticLeftCaptionButtonsWidth;
        /// <summary>
        /// Whether a <see cref="Window"/> using managed chrome is showing its <see cref="Window.Icon"/>.
        /// </summary>
        public double CosmeticLeftCaptionButtonsWidth
        {
            get => _cosmeticLeftCaptionButtonsWidth;
            internal set => SetAndRaise(CosmeticLeftCaptionButtonsWidthProperty, ref _cosmeticLeftCaptionButtonsWidth, value);
        }


        /// <summary>
        /// Bridged width of a <see cref="Window"/>'s managed chrome's right caption buttons.
        /// </summary>
        public static readonly DirectProperty<WindowChrome, double> RightCaptionButtonsWidthProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, double>(nameof(RightCaptionButtonsWidth)
                , getter: o => o.RightCaptionButtonsWidth
                , unsetValue: _DEFAULT_BridgedCaptionButtonsWidth
            );
        double _rightCaptionButtonsWidth = _DEFAULT_BridgedCaptionButtonsWidth;
        /// <summary>
        /// Whether a <see cref="Window"/> using managed chrome is showing its <see cref="Window.Icon"/>.
        /// </summary>
        public double RightCaptionButtonsWidth
        {
            get => _rightCaptionButtonsWidth;
            internal set => SetAndRaise(RightCaptionButtonsWidthProperty, ref _rightCaptionButtonsWidth, value);
        }


        const double _DEFAULT_CosmeticRightCaptionButtonsWidth = 0d;
        public static readonly DirectProperty<WindowChrome, double> CosmeticRightCaptionButtonsWidthProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, double>(nameof(CosmeticRightCaptionButtonsWidth)
                , getter: o => o.CosmeticRightCaptionButtonsWidth
                , unsetValue: _DEFAULT_BridgedCaptionButtonsWidth
            );
        double _cosmeticRightCaptionButtonsWidth = _DEFAULT_CosmeticRightCaptionButtonsWidth;
        /// <summary>
        /// Whether a <see cref="Window"/> using managed chrome is showing its <see cref="Window.Icon"/>.
        /// </summary>
        public double CosmeticRightCaptionButtonsWidth
        {
            get => _cosmeticRightCaptionButtonsWidth;
            internal set => SetAndRaise(CosmeticRightCaptionButtonsWidthProperty, ref _cosmeticRightCaptionButtonsWidth, value);
        }




        static void StateInit()
            => AffectsAll(new AvaloniaProperty[]
            {
                IsChromeManagedProperty,
                IsTitleVisibleProperty,
                IsIconVisibleProperty,
                ReserveCaptionAreaProperty,
                LeftCaptionButtonsWidthProperty,
                CosmeticLeftCaptionButtonsWidthProperty,
                RightCaptionButtonsWidthProperty,
                CosmeticRightCaptionButtonsWidthProperty,
            });


        static void AffectsPseudoClasses_Window(params AvaloniaProperty[] properties)
        {
            foreach (var property in properties)
            {
                IsChromeManagedProperty.Changed.AddClassHandler<WindowChrome>(AffectsPseudoClassProperty_Changed);
            }
        }
        static void AffectsPseudoClasses_WindowChrome(params AvaloniaProperty[] properties)
        {
            foreach (var property in properties)
            {
                IsChromeManagedProperty.Changed.AddClassHandler<WindowChrome>(AffectsPseudoClassProperty_Changed);
            }
        }
        static void AffectsPseudoClassProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
            => AffectsPseudoClassProperty_Changed(GetChrome(window), e);
        static void AffectsPseudoClassProperty_Changed(WindowChrome chrome, AvaloniaPropertyChangedEventArgs e)
            => chrome
                .Elements
                .Refresh()
        ;



        internal void RefreshShowIcon()
            => RefreshShowIcon(GetShowIconHint(HostWindow));
        internal void RefreshShowIcon(ManagedChromeElementHint hint)
            => IsIconVisible = hint.ResolveVisibility(PLATFORM_IMPL.DefaultShowCaptionIcon);


        internal void RefreshShowTitle()
            => RefreshShowTitle(GetShowTitleHint(HostWindow));
        internal void RefreshShowTitle(ManagedChromeElementHint hint)
            => IsTitleVisible = hint.ResolveVisibility(PLATFORM_IMPL.DefaultShowCaptionText);


        internal void RefreshShowIconAndTitle()
        {
            RefreshShowIcon();
            RefreshShowTitle();
        }
    }
}