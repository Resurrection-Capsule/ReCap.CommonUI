using System;
using Avalonia.Controls;

namespace ReCap.CommonUI.Demo.ViewModels
{
    public sealed class PopOutWindowManager
        : ViewModelBase
    {
        static readonly PopOutWindowManager _instance = new();
        public static PopOutWindowManager Instance
        {
            get => _instance;
        }




        Window _mainWindow = null;
        public Window MainWindow
        {
            get => _mainWindow;
            set
            {
                if (_mainWindow != null)
                    _mainWindow.Closed -= MainWindow_Closed;

                _mainWindow = value;

                if (_mainWindow != null)
                    _mainWindow.Closed += MainWindow_Closed;
            }
        }




        public void PopOutCommand(object parameter)
        {
            if (parameter is ViewModelBase viewModel)
                PopOutViewModel(viewModel);
            else if (parameter is UserControl view)
                PopOutView(view);
        }




        Window _popOutWindow = null;
        public void ClosePopOutWindow()
            => _popOutWindow?.Close();


        public bool PopOutViewModel(ViewModelBase viewModel)
        {
            ClosePopOutWindow();
            IPopOutWindowProvider windowProvider = (IPopOutWindowProvider)viewModel;
            return PopOutWindowProvider(windowProvider);
        }


        public bool PopOutWindowProvider(IPopOutWindowProvider windowProvider)
        {
            ClosePopOutWindow();

            if (windowProvider == null)
                throw new ArgumentNullException(paramName: nameof(windowProvider));

            _popOutWindow = windowProvider.CreatePopOutWindow();
            if (_popOutWindow == null)
                throw new NullReferenceException($"{nameof(IPopOutWindowProvider.CreatePopOutWindow)}() returned null!");

            _popOutWindow.Show();
            return true;
        }


        public bool PopOutView(UserControl view)
        {
            ClosePopOutWindow();

            if (view == null)
                throw new ArgumentNullException(paramName: nameof(view));

            var dataContext = view.DataContext ?? throw new NullReferenceException($"{nameof(view)}.{nameof(Control.DataContext)}");
            ViewModelBase viewModel = (ViewModelBase)dataContext;
            return PopOutViewModel(viewModel);

        }




        void MainWindow_Closed(object sender, EventArgs e)
            => ClosePopOutWindow();


        void PopOutWindow_Closed(object sender, EventArgs e)
        {
            if (_popOutWindow != null)
            {
                _popOutWindow.Closed -= PopOutWindow_Closed;
                _popOutWindow = null;
            }
        }
    }
}