using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using ReCap.CommonUI.Attached.WindowChrome;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    [ChromeElementPseudoClassesPlaceholder]
    public sealed class TitleBar2
        : TitleBar
        , IPseudoClassesProvider
    {
        const string _PART_CAPTIONBUTTONS = "PART_CaptionButtons";




#nullable enable
        CompositeDisposable? _disposables;
        CaptionButtonsBroker? _captionButtons;
#nullable restore
        void UpdateSize(Window window)
        {
            Margin = new(0d);
            if (window.WindowState != WindowState.FullScreen)
            {
                Height = Math.Max(Math.Max(0, MinHeight), window.WindowDecorationMargin.Top);

                if (_captionButtons != null)
                {
                    _captionButtons.Height = Height;
                }
            }

            IsVisible = window.GetChrome().IsChromeManaged;
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

            var chrome = _hostWindow.GetChrome();
            chrome.AttachElement(this);
            _disposables = new CompositeDisposable(6)
            {
                _hostWindow
                    .GetObservable(Window.WindowDecorationMarginProperty)
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
                    .Subscribe(HostWindow_PropertyChanged)
                ,
                _hostWindow
                    .GetObservable(Window.IsExtendedIntoWindowDecorationsProperty)
                    .Subscribe(HostWindow_PropertyChanged)
                ,
                chrome
                    .GetObservable(WindowChrome.IsChromeManagedProperty)
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




#region IPseudoClassesProvider
        IPseudoClasses IPseudoClassesProvider.GetPseudoClasses()
            => PseudoClasses;
#endregion
    }
}