using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using ReCap.CommonUI.Attached.WindowChrome;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    [PseudoClasses(_PSEUD_FILLS_SCREEN)]
    public sealed class TitleBarUnderlay
        : TemplatedControl
    {
        const string _PSEUD_FILLS_SCREEN = ":fills_screen";

        /*
        /// <summary>
        /// Defines the <see cref="TitleBarHeight"/> property.
        /// </summary>
        public static readonly StyledProperty<double> TitleBarHeightProperty =
            AvaloniaProperty.Register<TitleBarUnderlay, double>(nameof(TitleBarHeight), double.NaN);

        /// <summary>
        /// Represents the Height of the containing Window's TitleBar.
        /// </summary>
        public double TitleBarHeight
        {
            get => GetValue(TitleBarHeightProperty);
            set => SetValue(TitleBarHeightProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="DefaultTitleBarHeight"/> property.
        /// </summary>
        public static readonly StyledProperty<double> DefaultTitleBarHeightProperty =
            AvaloniaProperty.Register<TitleBarUnderlay, double>(nameof(DefaultTitleBarHeight), 30);

        /// <summary>
        /// Gets or sets the default Height of the element.
        /// </summary>
        public double DefaultTitleBarHeight
        {
            get => GetValue(DefaultTitleBarHeightProperty);
            set => SetValue(DefaultTitleBarHeightProperty, value);
        }
        

        /// <summary>
        /// Defines the <see cref="IsTitleBarHeightValid"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsTitleBarHeightValidProperty =
            AvaloniaProperty.Register<TitleBarUnderlay, bool>(nameof(IsTitleBarHeightValid), false);

        /// <summary>
        /// Gets or sets the IsTitleBarHeightValid of the element.
        /// </summary>
        public bool IsTitleBarHeightValid
        {
            get => GetValue(IsTitleBarHeightValidProperty);
            set => SetValue(IsTitleBarHeightValidProperty, value);
        }
        

        /// <summary>
        /// Defines the <see cref="IsTitleBarVisible"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsTitleBarVisibleProperty =
            AvaloniaProperty.Register<TitleBarUnderlay, bool>(nameof(IsTitleBarVisible), false);

        /// <summary>
        /// Gets or sets the IsTitleBarVisible of the element.
        /// </summary>
        public bool IsTitleBarVisible
        {
            get => GetValue(IsTitleBarVisibleProperty);
            set => SetValue(IsTitleBarVisibleProperty, value);
        }
        */


        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (TopLevel.GetTopLevel(this) is Window window)
                window.BeginMoveDrag(e);
        }


        /*
        /// <summary>
        /// Defines the <see cref="IsWindowActive"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsWindowActiveProperty =
            AvaloniaProperty.Register<TitleBarUnderlay, bool>(nameof(IsWindowActive), false);

        /// <summary>
        /// Gets or sets the IsWindowActive of the element.
        /// </summary>
        public bool IsWindowActive
        {
            get => GetValue(IsWindowActiveProperty);
            set => SetValue(IsWindowActiveProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="IsWindowMaximizedOrFullScreen"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsWindowMaximizedOrFullScreenProperty =
            AvaloniaProperty.Register<TitleBarUnderlay, bool>(nameof(IsWindowMaximizedOrFullScreen), false);
        /// <summary>
        /// Gets or sets the IsWindowMaximizedOrFullScreen of the element.
        /// </summary>
        public bool IsWindowMaximizedOrFullScreen
        {
            get => GetValue(IsWindowMaximizedOrFullScreenProperty);
            set => SetValue(IsWindowMaximizedOrFullScreenProperty, value);
        }
        */
#if NO
        public static readonly DirectProperty<TitleBarUnderlay, bool> IsTitleBarHeightValidProperty =
            AvaloniaProperty.RegisterDirect<TitleBarUnderlay, bool>(
                nameof(IsTitleBarHeightValid)
                , s => s.IsTitleBarHeightValid
                //, (s, v) => s.IsTitleBarHeightValid = v
            );
        bool _isTitleBarHeightValid = false;
        public bool IsTitleBarHeightValid
        {
            get => _isTitleBarHeightValid;
            internal set => SetAndRaise(IsTitleBarHeightValidProperty, ref _isTitleBarHeightValid, value);
        }


#endif
        public static readonly DirectProperty<TitleBarUnderlay, bool> IsTitleBarVisibleProperty =
            AvaloniaProperty.RegisterDirect<TitleBarUnderlay, bool>(
                nameof(IsTitleBarVisible)
                , s => s.IsTitleBarVisible
                //, (s, v) => s.IsTitleBarVisible = v
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
                //, (s, v) => s.IsWindowActive = v
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
                //, (s, v) => s.IsWindowMaximizedOrFullScreen = v
            );
        bool _isWindowMaximizedOrFullScreen = false;
        public bool IsWindowMaximizedOrFullScreen
        {
            get => _isWindowMaximizedOrFullScreen;
            internal set => SetAndRaise(IsWindowMaximizedOrFullScreenProperty, ref _isWindowMaximizedOrFullScreen, value);
        }

#if NO
        public static readonly DirectProperty<TitleBarUnderlay, bool> HasLeftCaptionButtonsProperty =
            AvaloniaProperty.RegisterDirect<TitleBarUnderlay, bool>(
                nameof(HasLeftCaptionButtons)
                , s => s.HasLeftCaptionButtons
                //, (s, v) => s.HasLeftCaptionButtons = v
            );
        bool _hasLeftCaptionButtons = false;
        public bool HasLeftCaptionButtons
        {
            get => _hasLeftCaptionButtons;
            internal set => SetAndRaise(HasLeftCaptionButtonsProperty, ref _hasLeftCaptionButtons, value);
        }


        public static readonly DirectProperty<TitleBarUnderlay, bool> HasRightCaptionButtonsProperty =
            AvaloniaProperty.RegisterDirect<TitleBarUnderlay, bool>(
                nameof(HasRightCaptionButtons)
                , s => s.HasRightCaptionButtons
                //, (s, v) => s.HasRightCaptionButtons = v
            );
        bool _hasRightCaptionButtons = false;
        public bool HasRightCaptionButtons
        {
            get => _hasRightCaptionButtons;
            internal set => SetAndRaise(HasRightCaptionButtonsProperty, ref _hasRightCaptionButtons, value);
        }
#endif



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
            //this[!HeightProperty] = window[!Window.ExtendClientAreaTitleBarHeightHintProperty];
            //this[!IsVisibleProperty] = window[!Window.IsExtendedIntoWindowDecorationsProperty];

            //RefreshIsWindowMaximizedOrFullScreen(_window.WindowState);

#if NO
            var binding = 
                    _window
                        .GetObservable(WindowChromeAddon.LeftCaptionButtonsProperty)
                        .Select(x => x.Count > 0)
                        .ToBinding()
                //_window[!WindowChromeAddon.LeftCaptionButtonsProperty]
            ;
            //_window.PropertyChanged += Window_PropertyChanged;
            var leftBinding = _window
                .GetObservable(WindowChromeAddon.LeftCaptionButtonsProperty)
                .Select(x => x.Count > 0)
                .ToBinding()
            ;
            var rightBinding = _window
                .GetObservable(WindowChromeAddon.RightCaptionButtonsProperty)
                .Select(x => x.Count > 0)
                .ToBinding()
            ;
#endif
            _windowDisposable = new CompositeDisposable()
            {
                _window
                    .GetObservable(Window.WindowStateProperty)
                    .Subscribe(RefreshIsWindowMaximizedOrFullScreen)
                ,
                /*
                _window.Bind(Window.ExtendClientAreaTitleBarHeightHintProperty, this[!TitleBarHeightProperty]),
                _window.Bind(WindowBase.IsActiveProperty, this[!IsWindowActiveProperty])
                */
                _window
                    .GetObservable(Window.ExtendClientAreaTitleBarHeightHintProperty)
                    .Subscribe(_ => RefreshHeight())
                ,
                _window
                    .GetObservable(WindowBase.IsActiveProperty)
                    .Subscribe(isActive => IsWindowActive = isActive)
                ,
#if NO
                Bind(HasLeftCaptionButtonsProperty, leftBinding),
                Bind(HasRightCaptionButtonsProperty, rightBinding),
#endif
                /*
                _window
                    .GetObservable(WindowChromeAddon.LeftCaptionButtonsProperty)
                    .Subscribe(RefreshHasLeftCaptionButtons)
                ,
                _window
                    .GetObservable(WindowChromeAddon.RightCaptionButtonsProperty)
                    .Subscribe(RefreshHasRightCaptionButtons)
                ,
                */
            };
            RefreshIsWindowMaximizedOrFullScreen(_window.WindowState);
            //TitleBarHeight = _window.ExtendClientAreaTitleBarHeightHint;
            RefreshHeight();
            IsWindowActive = _window.IsActive;
            /*
            RefreshHasLeftCaptionButtons(WindowChromeAddon.GetLeftCaptionButtons(_window));
            RefreshHasRightCaptionButtons(WindowChromeAddon.GetRightCaptionButtons(_window));
            */
            //this[!TitleBarHeightProperty] = _window[!Window.ExtendClientAreaTitleBarHeightHintProperty];
            //this[!IsWindowActiveProperty] = _window[!WindowBase.IsActiveProperty];

            var chromeOverlay = ChromeOverlayLayer.GetOverlayLayer(_window.Presenter);
            if (chromeOverlay == null)
                return;
            
            var chromeChildren = chromeOverlay.Children;
            foreach (var chromeChild in chromeChildren)
            {
                /*
                if (chromeChild is TitleBar titleBar)
                {
                    _titleBar = titleBar;
                    //_titleBar.PropertyChanged += TitleBar_PropertyChanged;
                    this[!IsTitleBarVisibleProperty] = _titleBar[!TitleBar.IsVisibleProperty];
                    break;
                }
                */
                if (chromeChild is not TitleBar titleBar)
                    continue;

                _titleBar = titleBar;
                _titleBarDisposable = new CompositeDisposable()
                {
                    /*
                    _titleBar
                        .GetObservable(IsVisibleProperty)
                        .Subscribe(isVisible => IsVisible = isVisible)
                    ,
                    */
                    //Bind(IsTitleBarVisibleProperty, _titleBar[!IsVisibleProperty]),
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
                //_window.PropertyChanged -= Window_PropertyChanged;
                IsWindowMaximizedOrFullScreen = false;
                _window = null;
            }


            _titleBarDisposable?.Dispose();
            _titleBarDisposable = null;

            if (_titleBar != null)
            {
                //_titleBar.PropertyChanged -= TitleBar_PropertyChanged;
                _titleBar = null;
            }
        }
        
        /*
        void Window_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == Window.WindowStateProperty)
                RefreshIsWindowMaximizedOrFullScreen(e.GetNewValue<WindowState>());
        }
        */
#if NO
        void RefreshHasLeftCaptionButtons(CaptionButtonRoles captionButtons)
            => HasLeftCaptionButtons = captionButtons?.Count > 0;
        void RefreshHasRightCaptionButtons(CaptionButtonRoles captionButtons)
            => HasRightCaptionButtons = captionButtons?.Count > 0;
#endif
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

            double height = _window.ExtendClientAreaTitleBarHeightHint;
            if (height < 0d)
                height = _titleBar.Bounds.Height;

            if (height >= 0d)
                Height = height;
        }
        /*
        void TitleBar_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == TitleBar.IsVisibleProperty)
            {
                var isVisible = e.GetNewValue<bool>();
            }
        }

        void ValidateHeightProperties(double titleBarHeight, bool isTitleBarVisible)
        {

        }
        */




        static TitleBarUnderlay()
        {
            Extensions.MakeControlTypeNonInteractive<TitleBarUnderlay>();

            /*
            AffectsMeasure<TitleBarUnderlay>(TitleBarHeightProperty);

            TitleBarHeightProperty.Changed.AddClassHandler<TitleBarUnderlay>(TitleBarHeightProperty_Changed);
            */
            IsWindowMaximizedOrFullScreenProperty.Changed.AddClassHandler<TitleBarUnderlay>(IsWindowMaximizedOrFullScreenProperty_Changed);
        }

        static void IsWindowMaximizedOrFullScreenProperty_Changed(TitleBarUnderlay underlay, AvaloniaPropertyChangedEventArgs args)
        {
            underlay.PseudoClasses.Set(_PSEUD_FILLS_SCREEN, args.GetNewValue<bool>());
        }

        /*
        static void TitleBarHeightProperty_Changed(TitleBarUnderlay underlay, AvaloniaPropertyChangedEventArgs args)
        {
            var newHeight = args.GetNewValue<double>();
            underlay.IsTitleBarHeightValid = newHeight >= 0;
        }
        */

        protected override Type StyleKeyOverride => typeof(TitleBarUnderlay);
        static void WrLine(string line)
        {
            Console.WriteLine(line);
        }
    }
}
