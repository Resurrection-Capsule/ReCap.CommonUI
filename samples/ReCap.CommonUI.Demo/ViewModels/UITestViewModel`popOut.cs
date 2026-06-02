using System;
using ReCap.CommonUI.Demo.ViewModels.Pages;

namespace ReCap.CommonUI.Demo.ViewModels
{
    partial class UITestViewModel
    {
        public void PopOutCurrentTabCommand(object _)
        {
            bool ret = PopOutCurrentTab();
#if DEBUG
            if (!ret)
                throw new Exception($"{nameof(PopOutCurrentTab)}() returned false!");
#endif
        }


        public bool PopOutCurrentTab()
        {
            if (!TryGetTabContentAt(SelectedIndex, out ViewModelBase tabContentVM))
                return false;

            UITestSubViewModel tabContent = (UITestSubViewModel)tabContentVM;
            return tabContent.PopOutCurrentTab();
        }
    }
}