using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    [PseudoClasses(
        //_STATE_MINIMIZED, _STATE_NORMAL, _STATE_MAXIMIZED, _STATE_FULLSCREEN,
        _LEFT_SIDE_BUTTONS)]
    public sealed class TitleBar2
        : TitleBar
    {
        const string _STATE_MINIMIZED = ":minimized";
        const string _STATE_NORMAL = ":normal";
        const string _STATE_MAXIMIZED = ":maximized";
        const string _STATE_FULLSCREEN = ":fullscreen";
        const string _LEFT_SIDE_BUTTONS = ":left-side-buttons";




#nullable enable
        CompositeDisposable? _disposables;
#if TITLEBAR2_CAPTIONBUTTONS
        CaptionButtons? _captionButtons;
#endif
#nullable restore

        void UpdateSize(Window window)
        {
            Margin = new Thickness(
                window.OffScreenMargin.Left,
                window.OffScreenMargin.Top,
                window.OffScreenMargin.Right,
                window.OffScreenMargin.Bottom);

            if (window.WindowState != WindowState.FullScreen)
            {
                Height = Math.Max(Math.Max(0, MinHeight), window.WindowDecorationMargin.Top);

#if TITLEBAR2_CAPTIONBUTTONS
                if (_captionButtons != null)
                {
                    _captionButtons.Height = Height;
                }
#endif
            }

            IsVisible = WindowChromeAddon.GetIsUsingManagedChrome(window);
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

#if TITLEBAR2_CAPTIONBUTTONS
            _captionButtons?.Detach();

            _captionButtons = e.NameScope.Get<CaptionButtons>("PART_CaptionButtons");
#endif

            if (VisualRoot is Window window)
            {
#if TITLEBAR2_CAPTIONBUTTONS
                _captionButtons?.Attach(window);
#endif

                UpdateSize(window);
            }
        }

        /// <inheritdoc />
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            if (VisualRoot is Window window)
            {
                _disposables = new CompositeDisposable(6)
                {
                    window.GetObservable(Window.WindowDecorationMarginProperty)
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

                    window.GetObservable(WindowChromeAddon.LeftSideButtonsProperty)
                        .Subscribe(x => PseudoClasses.Set(_LEFT_SIDE_BUTTONS, x)),
                };
            }
        }

        /// <inheritdoc />
        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);

            _disposables?.Dispose();

#if TITLEBAR2_CAPTIONBUTTONS
            _captionButtons?.Detach();
            _captionButtons = null;
#endif
        }


        protected override Type StyleKeyOverride
            => typeof(TitleBar);
    }
}