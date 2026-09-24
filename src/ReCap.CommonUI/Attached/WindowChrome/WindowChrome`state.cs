using System;
using Avalonia;
using Avalonia.Controls;
using ReCap.CommonUI.Util;

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




        /// <summary>
        /// Bridged width of a <see cref="Window"/>'s managed chrome's left caption buttons.
        /// </summary>
        const double _DEFAULT_LeftCaptionButtonsWidth = 0d;
        public static readonly DirectProperty<WindowChrome, double> LeftCaptionButtonsWidthProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, double>(nameof(LeftCaptionButtonsWidth)
                , getter: o => o.LeftCaptionButtonsWidth
                , unsetValue: _DEFAULT_LeftCaptionButtonsWidth
            );
        double _LeftCaptionButtonsWidth = _DEFAULT_LeftCaptionButtonsWidth;
        /// <summary>
        /// Whether a <see cref="Window"/> using managed chrome is showing its <see cref="Window.Icon"/>.
        /// </summary>
        public double LeftCaptionButtonsWidth
        {
            get => _LeftCaptionButtonsWidth;
            internal set => SetAndRaise(LeftCaptionButtonsWidthProperty, ref _LeftCaptionButtonsWidth, value);
        }


        /// <summary>
        /// Bridged width of a <see cref="Window"/>'s managed chrome's right caption buttons.
        /// </summary>
        const double _DEFAULT_RightCaptionButtonsWidth = 0d;
        public static readonly DirectProperty<WindowChrome, double> RightCaptionButtonsWidthProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, double>(nameof(RightCaptionButtonsWidth)
                , getter: o => o.RightCaptionButtonsWidth
                , unsetValue: _DEFAULT_RightCaptionButtonsWidth
            );
        double _rightCaptionButtonsWidth = _DEFAULT_RightCaptionButtonsWidth;
        /// <summary>
        /// Whether a <see cref="Window"/> using managed chrome is showing its <see cref="Window.Icon"/>.
        /// </summary>
        public double RightCaptionButtonsWidth
        {
            get => _rightCaptionButtonsWidth;
            internal set => SetAndRaise(RightCaptionButtonsWidthProperty, ref _rightCaptionButtonsWidth, value);
        }




        static void StateInit()
        {
            IsChromeManagedProperty.Changed.AddClassHandler<WindowChrome>((s, e) => s.OnPropertyWithClassChanged(e, "managedchrome"));
            IsTitleVisibleProperty.Changed.AddClassHandler<WindowChrome>((s, e) => s.OnPropertyWithClassChanged(e, "titlevisible"));
            IsIconVisibleProperty.Changed.AddClassHandler<WindowChrome>((s, e) => s.OnPropertyWithClassChanged(e, "iconvisible"));
        }




        void OnPropertyWithClassChanged(AvaloniaPropertyChangedEventArgs args, string className)
        {
            if (_windowRef.TryGetTarget(out Window window))
                PropertySetClass(window, className, args.GetNewValue<bool>());
        }
        static void PropertySetClass(Window window, string className, bool value)
        {
#if PSEUDOCLASS_HACK
            if (window.TryGetPseudoClasses(out IPseudoClasses pseudoClasses))
                pseudoClasses.Set($":{className}", value);
#else
            window.Classes.Set(className, value);
#endif
        }


        WeakReference<Window> _windowRef = null;
        internal WindowChrome(Window window)
            : this()
        {
            _windowRef = new(window);
        }
        private WindowChrome()
            : base()
        {}




        internal void RefreshShowIcon()
        {
            if (_windowRef.TryGetTarget(out Window window))
                RefreshShowIcon(GetShowIconHint(window));
        }
        internal void RefreshShowIcon(ManagedChromeElementHint hint)
            => IsIconVisible = hint.ResolveVisibility(PLATFORM_IMPL.DefaultShowCaptionIcon);


        internal void RefreshShowTitle()
        {
            if (_windowRef.TryGetTarget(out Window window))
                RefreshShowTitle(GetShowTitleHint(window));
        }
        internal void RefreshShowTitle(ManagedChromeElementHint hint)
            => IsTitleVisible = hint.ResolveVisibility(PLATFORM_IMPL.DefaultShowCaptionText);

        internal void RefreshShowIconAndTitle()
        {
            RefreshShowIcon();
            RefreshShowTitle();
        }


        public void PoorMansDispose()
        {
            _windowRef.SetTarget(null);
            _windowRef = null;
        }
    }
}