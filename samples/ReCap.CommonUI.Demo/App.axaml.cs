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




        public static readonly StyledProperty<ManagedChromeHint> ManagedChromeHintProperty =
            AvaloniaProperty.Register<App, ManagedChromeHint>(nameof(ManagedChromeHint));
        public ManagedChromeHint ManagedChromeHint
        {
            get => GetValue(ManagedChromeHintProperty);
            set => SetValue(ManagedChromeHintProperty, value);
        }


        public static readonly StyledProperty<CaptionButtonRoles> LeftCaptionButtonRolesProperty =
            AvaloniaProperty.Register<App, CaptionButtonRoles>(nameof(LeftCaptionButtonRoles));
        public CaptionButtonRoles LeftCaptionButtonRoles
        {
            get => GetValue(LeftCaptionButtonRolesProperty);
            set => SetValue(LeftCaptionButtonRolesProperty, value);
        }


        public static readonly StyledProperty<CaptionButtonRoles> RightCaptionButtonRolesProperty =
            AvaloniaProperty.Register<App, CaptionButtonRoles>(nameof(RightCaptionButtonRoles));
        public CaptionButtonRoles RightCaptionButtonRoles
        {
            get => GetValue(RightCaptionButtonRolesProperty);
            set => SetValue(RightCaptionButtonRolesProperty, value);
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
            ManagedChromeHint = ManagedChromeHint.Auto;
            var roles = ManagedWindowChrome.PlatformDefaultCaptionButtonRoles;
            LeftCaptionButtonRoles = roles.Left;
            RightCaptionButtonRoles = roles.Right;
            MainVM = new UITestViewModel();

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.ShutdownMode = ShutdownMode.OnMainWindowClose;
                MainWindow mainWindow = new()
                {
                    DataContext = MainVM,
                };
                desktop.MainWindow = mainWindow;
                PopOutWindowManager.Instance.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}