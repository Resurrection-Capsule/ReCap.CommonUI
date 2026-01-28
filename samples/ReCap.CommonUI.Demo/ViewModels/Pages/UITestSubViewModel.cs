using System.Collections.Generic;

namespace ReCap.CommonUI.Demo.ViewModels.Pages
{
    class UITestSubViewModel
        : TabsViewModel
    {
        public UITestSubViewModel(params PageTabViewModel[] tabs)
            : base((IEnumerable<PageTabViewModel>)tabs)
        {}
        public UITestSubViewModel(IEnumerable<PageTabViewModel> tabs)
            : base(tabs)
        {}
    }
}