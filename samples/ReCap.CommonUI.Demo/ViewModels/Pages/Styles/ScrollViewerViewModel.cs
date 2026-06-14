using System;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    public class ScrollViewerViewModel
        : PageViewModelBase
    {
        bool _autoHideScrollBars = false;
        public bool AutoHideScrollBars
        {
            get => _autoHideScrollBars;
            set => RASIC(ref _autoHideScrollBars, value);
        }
    }
}