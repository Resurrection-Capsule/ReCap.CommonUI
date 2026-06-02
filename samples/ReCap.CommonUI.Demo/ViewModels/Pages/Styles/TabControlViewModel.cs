using System;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    public class TabControlViewModel
        : TabsViewModelBase
        , IPopOutWindowProvider
    {
        public TabControlViewModel()
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




        public Avalonia.Controls.Window CreatePopOutWindow()
            => new Views.Pages.PopOutWindow(this);
    }
}