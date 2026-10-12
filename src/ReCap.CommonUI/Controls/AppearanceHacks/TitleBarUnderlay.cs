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
    [ChromeElementPseudoClassesPlaceholder]
    public sealed class TitleBarUnderlay
        : TemplatedControl
        , IPseudoClassesProvider
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
                _hostWindow
                    .GetObservable(Window.ExtendClientAreaTitleBarHeightHintProperty)
                    .Subscribe(RefreshHeightHandler)
                ,
            };
            RefreshHeight();

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




#region IPseudoClassesProvider
        IPseudoClasses IPseudoClassesProvider.GetPseudoClasses()
            => PseudoClasses;
#endregion
    }
}
