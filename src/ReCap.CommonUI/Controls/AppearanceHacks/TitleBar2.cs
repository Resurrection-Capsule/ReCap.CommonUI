using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using ReCap.CommonUI.Attached.WindowChrome;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    [PseudoClasses(_LEFT_SIDE_BUTTONS)]
    public sealed class TitleBar2
        : TitleBar
    {
        const string _PART_CAPTIONBUTTONS = "PART_CaptionButtons";

        const string _STATE_MINIMIZED = ":minimized";
        const string _STATE_NORMAL = ":normal";
        const string _STATE_MAXIMIZED = ":maximized";
        const string _STATE_FULLSCREEN = ":fullscreen";
        const string _LEFT_SIDE_BUTTONS = ":left-side-buttons";




#nullable enable
        CompositeDisposable? _disposables;
        CaptionButtons2? _captionButtons;
#nullable restore
        CaptionButton _windowIconContainer;
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

            IsVisible = WindowChromeAddon.GetIsUsingManagedChrome(window);
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            _captionButtons?.Detach();
            _captionButtons = e.NameScope.Get<CaptionButtons2>(_PART_CAPTIONBUTTONS);


            if (VisualRoot is Window window)
            {
                _captionButtons?.Attach(window);
                UpdateSize(window);
            }
            else
            {
                window = null;
            }


            if (_windowIconContainer != null)
            {
                /*
                _windowIconContainer.Click -= WindowIconContainer_Click;
                _windowIconContainer.DoubleTapped -= WindowIconContainer_DoubleTapped;
                */
                _windowIconContainer.Executed -= WindowIconContainer_Executed;
                _windowIconContainer = null;
            }

            _windowIconContainer = e.NameScope.Get<CaptionButton>("PART_WindowIconContainer");
            var role = CaptionButtonRole.Menu;
            _windowIconContainer.Role = role;
            WindowChromeAddon.SetNonClienHitTestResult(_windowIconContainer, CaptionButton.ROLE_TO_NCHITTEST[role]);
            /*
            _windowIconContainer.Click += WindowIconContainer_Click;
            _windowIconContainer.DoubleTapped += WindowIconContainer_DoubleTapped;
            */
            _windowIconContainer.Executed += WindowIconContainer_Executed;
        }

        void WindowIconContainer_Executed(object sender, CaptionButtonClickEventArgs e)
            => _captionButtons?.ExecuteCaptionButton(_windowIconContainer, e);

        /*
        void WindowIconContainer_Click(object sender, RoutedEventArgs e)
            => _captionButtons?.ExecuteCaptionButton(_windowIconContainer, CaptionButtonInputAction.LeftClick);
        void WindowIconContainer_DoubleTapped(object sender, TappedEventArgs e)
        {
            CaptionButton button = (CaptionButton)sender;
            CaptionButtonClickEventArgs args = new(button, button.Role, true, e);
            _captionButtons?.ExecuteCaptionButton(_windowIconContainer, args);
        }
        */


        /// <inheritdoc />
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            if (VisualRoot is not Window window)
                return;

            _disposables = new CompositeDisposable(6)
            {
                window.GetObservable(Window.WindowDecorationMarginProperty)
                    .Subscribe(_ => UpdateSize(window)),
                window.GetObservable(WindowChromeAddon.IsUsingManagedChromeProperty)
                    .Subscribe(_ => UpdateSize(window)),
                window.GetObservable(Window.SystemDecorationsProperty)
                    .Subscribe(_ => UpdateSize(window)),
                window.GetObservable(Window.ExtendClientAreaTitleBarHeightHintProperty)
                    .Subscribe(_ => UpdateSize(window)),
                window.GetObservable(Window.OffScreenMarginProperty)
                    .Subscribe(_ => UpdateSize(window)),
                window.GetObservable(Window.ExtendClientAreaChromeHintsProperty)
                    .Subscribe(_ => UpdateSize(window)),
                window.GetObservable(Window.WindowStateProperty)
                    .Subscribe(x =>
                    {
                        PseudoClasses.Set(_STATE_MINIMIZED, x == WindowState.Minimized);
                        PseudoClasses.Set(_STATE_NORMAL, x == WindowState.Normal);
                        PseudoClasses.Set(_STATE_MAXIMIZED, x == WindowState.Maximized);
                        PseudoClasses.Set(_STATE_FULLSCREEN, x == WindowState.FullScreen);
                    }),
                window.GetObservable(Window.IsExtendedIntoWindowDecorationsProperty)
                    .Subscribe(_ => UpdateSize(window)),
            };

            UpdateSize(window);
        }

        /// <inheritdoc />
        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            _disposables?.Dispose();

            _captionButtons?.Detach();
            _captionButtons = null;
        }
    }
}