using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using ReCap.CommonUI.Demo.ViewModels;
using ReCap.CommonUI.Demo.Views;

namespace ReCap.CommonUI.Demo
{
    public partial class App
        : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }


        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                MainWindow mainWindow = new()
                {
                    DataContext = new UITestViewModel(),
                };
                desktop.MainWindow = mainWindow;
                PopOutWindowManager.Instance.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}