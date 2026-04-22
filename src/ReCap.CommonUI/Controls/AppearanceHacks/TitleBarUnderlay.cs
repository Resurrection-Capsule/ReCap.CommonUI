using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    [PseudoClasses(_PSEUD_FILLS_SCREEN)]
    public sealed class TitleBarUnderlay
        : TemplatedControl
    {
        const string _PSEUD_FILLS_SCREEN = ":fills_screen";



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


        public static readonly DirectProperty<TitleBarUnderlay, bool> IsWindowActiveProperty =
            AvaloniaProperty.RegisterDirect<TitleBarUnderlay, bool>(
                nameof(IsWindowActive)
                , s => s.IsWindowActive
            );
        bool _isWindowActive = false;
        public bool IsWindowActive
        {
            get => _isWindowActive;
            internal set => SetAndRaise(IsWindowActiveProperty, ref _isWindowActive, value);
        }


        public static readonly DirectProperty<TitleBarUnderlay, bool> IsWindowMaximizedOrFullScreenProperty =
            AvaloniaProperty.RegisterDirect<TitleBarUnderlay, bool>(
                nameof(IsWindowMaximizedOrFullScreen)
                , s => s.IsWindowMaximizedOrFullScreen
            );
        bool _isWindowMaximizedOrFullScreen = false;
        public bool IsWindowMaximizedOrFullScreen
        {
            get => _isWindowMaximizedOrFullScreen;
            internal set => SetAndRaise(IsWindowMaximizedOrFullScreenProperty, ref _isWindowMaximizedOrFullScreen, value);
        }
#endregion




        Window _window = null;
        TitleBar _titleBar = null;
        IDisposable _windowDisposable = null;
        IDisposable _titleBarDisposable = null;
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            if (!(e.Root is Window window))
                return;
            
            _window = window;
            _windowDisposable = new CompositeDisposable()
            {
                _window
                    .GetObservable(Window.WindowStateProperty)
                    .Subscribe(RefreshIsWindowMaximizedOrFullScreen)
                ,
                _window
                    .GetObservable(Window.ExtendClientAreaTitleBarHeightHintProperty)
                    .Subscribe(_ => RefreshHeight())
                ,
                _window
                    .GetObservable(WindowBase.IsActiveProperty)
                    .Subscribe(isActive => IsWindowActive = isActive)
                ,
            };
            RefreshIsWindowMaximizedOrFullScreen(_window.WindowState);
            RefreshHeight();
            IsWindowActive = _window.IsActive;

            var chromeOverlay = ChromeOverlayLayer.GetOverlayLayer(_window.Presenter);
            if (chromeOverlay == null)
                return;
            
            var chromeChildren = chromeOverlay.Children;
            foreach (var chromeChild in chromeChildren)
            {
                if (chromeChild is not TitleBar titleBar)
                    continue;

                _titleBar = titleBar;
                _titleBarDisposable = new CompositeDisposable()
                {
                    _titleBar
                        .GetObservable(IsVisibleProperty)
                        .Subscribe(isVisible =>
                        {
                            IsTitleBarVisible = isVisible;
                            RefreshHeight();
                        })
                    ,
                    _titleBar
                        .GetObservable(BoundsProperty)
                        .Subscribe(_ => RefreshHeight())
                    ,
                };
                RefreshHeight();
                IsTitleBarVisible = _titleBar.IsVisible;
                break;
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            
            _windowDisposable?.Dispose();
            _windowDisposable = null;

            if (_window != null)
            {
                IsWindowMaximizedOrFullScreen = false;
                _window = null;
            }


            _titleBarDisposable?.Dispose();
            _titleBarDisposable = null;

            _titleBar = null;
        }


        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (e.ClickCount > 1)
                return;
            else if (TopLevel.GetTopLevel(this) is Window window)
                window.BeginMoveDrag(e);
        }


        void RefreshIsWindowMaximizedOrFullScreen(WindowState winState)
            => IsWindowMaximizedOrFullScreen =
                (winState == WindowState.Maximized)
                ||
                (winState == WindowState.FullScreen)
            ;
        void RefreshHeight()
        {
            if (_window == null)
                return;
            
            if (_titleBar == null)
                return;

            // [TODO: account for negative margins on TitleBarUnderlay?]
            double height = _window.ExtendClientAreaTitleBarHeightHint;
            if (height < 0d)
                height = _titleBar.Bounds.Height;

            if (height >= 0d)
                Height = height;
        }




        static TitleBarUnderlay()
        {
            Extensions.MakeControlTypeNonInteractive<TitleBarUnderlay>();
            IsWindowMaximizedOrFullScreenProperty.Changed.AddClassHandler<TitleBarUnderlay>(IsWindowMaximizedOrFullScreenProperty_Changed);
        }

        static void IsWindowMaximizedOrFullScreenProperty_Changed(TitleBarUnderlay underlay, AvaloniaPropertyChangedEventArgs args)
        {
            underlay.PseudoClasses.Set(_PSEUD_FILLS_SCREEN, args.GetNewValue<bool>());
        }

        protected override Type StyleKeyOverride
            => typeof(TitleBarUnderlay);
    }
}
