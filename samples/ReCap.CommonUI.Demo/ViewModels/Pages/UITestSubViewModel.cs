using System.Collections.Generic;

namespace ReCap.CommonUI.Demo.ViewModels.Pages
{
    class UITestSubViewModel
        : TabsViewModel
    {
        public UITestSubViewModel(params PageTabViewModel[] tabs)
            : this((IEnumerable<PageTabViewModel>)tabs)
        {}
        public UITestSubViewModel(IEnumerable<PageTabViewModel> tabs)
            : base(tabs)
        {}


        public bool PopOutCurrentTab()
        {
            if (TryGetTabContentAt(SelectedIndex, out ViewModelBase tabContent))
                return PopOutWindowManager.Instance.PopOutViewModel(tabContent);
            else
                return false;
        }
    }
}