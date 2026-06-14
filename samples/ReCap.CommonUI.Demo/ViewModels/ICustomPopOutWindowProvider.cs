using System;
using Avalonia.Controls;

namespace ReCap.CommonUI.Demo.ViewModels
{
    public interface IPopOutWindowProvider
    {
        Window CreatePopOutWindow();
    }


    /*
    public static class PopOutWindowProviderHelper
    {
        const double _POPOUT_WINDOW_WIDTH = 640d;
        const double _POPOUT_WINDOW_HEIGHT = 480d;
        static readonly Thickness _POPOUT_WINDOW_PADDING = new(8d);


        public static Window CreateDefault(object dataContext)
            => new()
            {
                DataContext = dataContext,
                Width = _POPOUT_WINDOW_WIDTH,
                Height = _POPOUT_WINDOW_HEIGHT,
                Padding = _POPOUT_WINDOW_PADDING,
            };
    }
    */
}