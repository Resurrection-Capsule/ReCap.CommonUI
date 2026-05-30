using System;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReCap.CommonUI.Demo.ViewModels.Pages.Styles;

namespace ReCap.CommonUI.Demo.Views.Pages.Styles
{
    public partial class MenuView
        : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }


        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }




        Window _separateWindow = null;
        void SeparateWindowDemoButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not InputElement inputEl)
                return;
            else if (!inputEl.IsEnabled)
                return;

            _separateWindow ??= CreateSeparateWindow(this);

            if (!_separateWindow.IsVisible)
                _separateWindow.Show();

            if (!_separateWindow.IsActive)
                _separateWindow.Activate();
        }




        const double _SEPARATE_WINDOW_WIDTH = 320d;
        const double _SEPARATE_WINDOW_HEIGHT = 240d;
        static Window CreateSeparateWindow(MenuView ownerView)
        {
            MenuViewModel newVM = new();
            MenuView newView = new()
            {
                DataContext = newVM,
            };

            Window window = new()
            {
                Title = "Menu Demo",
                Content = newView,
                [!DataContextProperty] = newView[!DataContextProperty],
                Width = _SEPARATE_WINDOW_WIDTH,
                Height = _SEPARATE_WINDOW_HEIGHT,
                MinWidth = _SEPARATE_WINDOW_WIDTH,
                MinHeight = _SEPARATE_WINDOW_HEIGHT,
                Padding = new(1),
            };
            window.Closed += ownerView.SeparateWindow_Closed;

            if (TopLevel.GetTopLevel(ownerView) is MainWindow mainWindow)
                mainWindow.Closed += ownerView.MainWindow_Closed;

            return window;
        }


        void SeparateWindow_Closed(object sender, EventArgs e)
        {
            _separateWindow.Closed -= SeparateWindow_Closed;
            _separateWindow = null;
        }


        void MainWindow_Closed(object sender, EventArgs e)
        {
            ((MainWindow)sender).Closed -= MainWindow_Closed;
            _separateWindow?.Close();
        }
    }
}
