using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using ReCap.CommonUI.Demo.Views;

namespace ReCap.CommonUI.Demo.ViewModels
{
    partial class UITestViewModel
    {
        static ViewLocator _viewLocator = null;
        public static ViewLocator ViewLocator
        {
            get
            {
                if (_viewLocator == null)
                {
                    var dataTemplates = Application.Current.DataTemplates;
                    foreach (var dataTemplate in dataTemplates)
                    {
                        if (dataTemplate is not ViewLocator viewLocator)
                            continue;

                        _viewLocator = viewLocator;
                        break;
                    }
                }
                return _viewLocator;
            }
        }




        public void PopOutCommand(object parameter)
            //=> PopOutView((UserControl)parameter);
        {
            if (parameter is ViewModelBase viewModel)
                PopOutViewModel(viewModel);
            else if (parameter is UserControl view)
                PopOutView(view);
        }



        Window _mainWindow = null;
        Window _popOutWindow = null;
        public void PopOutViewModel(ViewModelBase viewModel)
        {
            if (viewModel is PageTabViewModel pageTabVM)
            {
                PopOutViewModel(pageTabVM.ContentVM);
                return;
            }
            else if (viewModel is TabsViewModel tabsVM)
            {
                PopOutViewModel(tabsVM.Tabs[tabsVM.SelectedIndex]);
                return;
            }

            ClosePopOutWindow();

            if (viewModel == null)
                return;


            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
                _mainWindow = desktopLifetime.MainWindow;
            else
                return;


            _mainWindow.Closed += MainWindow_Closed;
            if (viewModel is IPopOutWindowProvider windowProvider)
            {
                _popOutWindow = windowProvider.CreatePopOutWindow();
                var newVM = _popOutWindow.DataContext;
                if (newVM == null)
                {
                    newVM = Activator.CreateInstance(viewModel.GetType());
                    _popOutWindow.DataContext = newVM;
                };

                _popOutWindow.Content ??= ViewLocator.Build(newVM);
            }
            else
            {
                var newVM = Activator.CreateInstance(viewModel.GetType());
                _popOutWindow = new()
                {
                    DataContext = newVM,
                    //[!ContentControl.ContentProperty] = _popOutWindow[!StyledElement.DataContextProperty],
                    Content = ViewLocator.Build(newVM),
                };
            }
            _popOutWindow.Show();
        }


        public void PopOutView(UserControl view)
        {
            ClosePopOutWindow();

            if (view == null)
                return;


            if (TopLevel.GetTopLevel(view) is Window mainWindow)
                _mainWindow = mainWindow;
            else
                return;


            var dataContext = view.DataContext;
            if (dataContext == null)
                return;


            _mainWindow.Closed += MainWindow_Closed;


#if NO
            if (dataContext is IPopOutWindowProvider windowProvider)
            {
                _popOutWindow = windowProvider.CreatePopOutWindow();
                var newVM = _popOutWindow.DataContext;
                if (newVM == null)
                {
                    newVM = Activator.CreateInstance(dataContext.GetType());
                    _popOutWindow.DataContext = newVM;
                };

                _popOutWindow.Content ??= ViewLocator.Build(newVM);
            }
            else
            {
                var newVM = Activator.CreateInstance(dataContext.GetType());
                _popOutWindow = new()
                {
                    DataContext = newVM,
                    //[!ContentControl.ContentProperty] = _popOutWindow[!StyledElement.DataContextProperty],
                    Content = ViewLocator.Build(newVM),
                };
            }
#else
            if (dataContext is IPopOutWindowProvider windowProvider)
            {
                _popOutWindow = windowProvider.CreatePopOutWindow();
            }
            else
            {
                _popOutWindow = new()
                {
                    Padding = new(8d),
                };
            }


            var popOutDataContext = _popOutWindow.DataContext;
            if (popOutDataContext == null)
            {
                popOutDataContext = Activator.CreateInstance(dataContext.GetType());
                _popOutWindow.DataContext = popOutDataContext;
            }

            _popOutWindow.Content ??= ViewLocator.Build(popOutDataContext);
#endif

            _popOutWindow.Show();
        }




        void ClosePopOutWindow()
            => _popOutWindow?.Close();


        void MainWindow_Closed(object sender, EventArgs e)
            => ClosePopOutWindow();


        void PopOutWindow_Closed(object sender, EventArgs e)
        {
            if (_popOutWindow != null)
            {
                _popOutWindow.Closed -= PopOutWindow_Closed;
                _popOutWindow = null;
            }


            if (_mainWindow != null)
            {
                _mainWindow.Closed -= MainWindow_Closed;
                _mainWindow = null;
            }
        }




        public static Window BasicCreateWindow(string title, double width, double height, bool setMinSize = false)
        {
            Window window = new()
            {
                Title = title,

                Width = width,
                Height = height,
            };

            if (setMinSize)
            {
                window.MinWidth = width;
                window.MinHeight = height;
            }

            return window;
        }
    }
}