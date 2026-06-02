using Avalonia;
using Avalonia.Controls;
using ReCap.CommonUI.Demo.Views.Pages;

namespace ReCap.CommonUI.Demo.ViewModels.Pages
{
    public abstract class PageViewModelBase
        : ViewModelBase
        , IPopOutWindowProvider
    {
        public Window CreatePopOutWindow()
        {
            Window window = new PopOutWindow(GetPopOutDataContext());
            CustomizePopOutWindow(ref window);
            return window;
        }


        protected virtual PageViewModelBase GetPopOutDataContext()
            => this;


        protected virtual void CustomizePopOutWindow(ref Window window)
        {}
    }
}