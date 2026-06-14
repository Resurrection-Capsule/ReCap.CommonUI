using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ReCap.CommonUI.Attached.WindowChrome;
using ReCap.CommonUI.Demo.ViewModels;
using ReCap.CommonUI.Demo.Views;

namespace ReCap.CommonUI.Demo
{
    public partial class App
        : Application
    {
        public const string WINDOW_MANAGED_CHROME_HINT_KEY = nameof(WINDOW_MANAGED_CHROME_HINT_KEY);
        public const string WINDOW_LEFT_SIDE_BUTTONS_KEY = nameof(WINDOW_LEFT_SIDE_BUTTONS_KEY);
        public const string WINDOW_BUTTONS_ORDER_KEY = nameof(WINDOW_BUTTONS_ORDER_KEY);


        new public static App Current
        {
            get => (App)Application.Current;
        }




        public static readonly StyledProperty<ManagedChromeMode> ManagedChromeHintProperty =
            AvaloniaProperty.Register<App, ManagedChromeMode>(nameof(ManagedChromeHint));
        public ManagedChromeMode ManagedChromeHint
        {
            get => GetValue(ManagedChromeHintProperty);
            set => SetValue(ManagedChromeHintProperty, value);
        }


        public static readonly StyledProperty<CaptionButtonRoles> LeftCaptionButtonsProperty =
            AvaloniaProperty.Register<App, CaptionButtonRoles>(nameof(LeftCaptionButtons));
        public CaptionButtonRoles LeftCaptionButtons
        {
            get => GetValue(LeftCaptionButtonsProperty);
            set => SetValue(LeftCaptionButtonsProperty, value);
        }


        public static readonly StyledProperty<CaptionButtonRoles> RightCaptionButtonsProperty =
            AvaloniaProperty.Register<App, CaptionButtonRoles>(nameof(RightCaptionButtons));
        public CaptionButtonRoles RightCaptionButtons
        {
            get => GetValue(RightCaptionButtonsProperty);
            set => SetValue(RightCaptionButtonsProperty, value);
        }


        public static readonly DirectProperty<App, UITestViewModel> MainVMProperty
            = AvaloniaProperty.RegisterDirect<App, UITestViewModel>(nameof(MainVM)
                , getter: x => x.MainVM
            );
        UITestViewModel _mainVM = null;
        public UITestViewModel MainVM
        {
            get => _mainVM;
            private set => SetAndRaise(MainVMProperty, ref _mainVM, value);
        }




        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }


        public override void OnFrameworkInitializationCompleted()
        {
            ManagedChromeHint = ManagedChromeMode.Auto;
            var captionButtons = WindowChromeAddon.PlatformDefaultCaptionButtons;
            LeftCaptionButtons = captionButtons.Left;
            RightCaptionButtons = captionButtons.Right;
            MainVM = new UITestViewModel();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
                desktop.MainWindow = new MainWindow()
                {
                    DataContext = MainVM,
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}