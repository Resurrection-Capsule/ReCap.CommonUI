using System;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    public class TabStripViewModel
        : TabsViewModelBase
    {
        public TabStripViewModel()
            : base()
        {
            AddTabs(new PageTabViewModel[]
            {
                new("Tab 0", null),
                new("Tab 1", null),
                new("Tab 2", null),
                new("Tab 3", null),
            });
        }
    }
}