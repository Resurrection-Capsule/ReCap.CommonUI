using System.Collections.Generic;

namespace ReCap.CommonUI.Demo.ViewModels
{
    public class TabsViewModel
        : TabsViewModelBase
    {
        public TabsViewModel()
            : base()
        {}


        public TabsViewModel(params PageTabViewModel[] tabs)
            : this((IEnumerable<PageTabViewModel>)tabs)
        {}
        public TabsViewModel(IEnumerable<PageTabViewModel> tabs)
            : this()
        {
            AddTabs(tabs);
        }
    }
}