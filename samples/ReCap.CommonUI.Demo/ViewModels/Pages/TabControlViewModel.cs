using System.Collections.ObjectModel;

namespace ReCap.CommonUI.Demo.ViewModels.Pages
{
    public class TabControlViewModel
        : TabsViewModelBase
    {
        protected override ObservableCollection<PageTabViewModel> CreateTabs()
            => new()
            {
                new PageTabViewModel("Tab 0", null),
                new PageTabViewModel("Tab 1", null),
                new PageTabViewModel("Tab 2", null),
                new PageTabViewModel("Tab 3", null),
            };
    }
}