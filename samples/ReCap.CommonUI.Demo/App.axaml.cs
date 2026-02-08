using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
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


        public static readonly StyledProperty<bool> LeftSideButtonsProperty =
            AvaloniaProperty.Register<App, bool>(nameof(LeftSideButtons));
        public bool LeftSideButtons
        {
            get => GetValue(LeftSideButtonsProperty);
            set => SetValue(LeftSideButtonsProperty, value);
        }


        public static readonly StyledProperty<CaptionButtonsOrder> ButtonsOrderProperty =
            AvaloniaProperty.Register<App, CaptionButtonsOrder>(nameof(ButtonsOrder));
        public CaptionButtonsOrder ButtonsOrder
        {
            get => GetValue(ButtonsOrderProperty);
            set => SetValue(ButtonsOrderProperty, value);
        }




        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }


        public override void OnFrameworkInitializationCompleted()
        {
            ManagedChromeHint = ManagedChromeMode.Auto;
            LeftSideButtons = WindowChromeAddon.PlatformPrefersLeftSideButtons;
            ButtonsOrder = WindowChromeAddon.PlatformPreferredCaptionButtonsOrder;

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow()
                {
                    DataContext = new UITestViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}