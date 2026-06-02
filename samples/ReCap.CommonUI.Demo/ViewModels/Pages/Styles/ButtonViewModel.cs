using System;
using Avalonia.Controls;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    public class ButtonViewModel
        : PageViewModelBase
    {
        const double _POPOUT_WINDOW_WIDTH = 736d;
        protected override void CustomizePopOutWindow(ref Window window)
        {
            base.CustomizePopOutWindow(ref window);
            window.MinWidth = _POPOUT_WINDOW_WIDTH;
            window.Width = _POPOUT_WINDOW_WIDTH;
        }
    }
}