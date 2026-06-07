using System;
using System.Collections.Generic;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    public class ComboBoxViewModel
        : PageViewModelBase
    {
        readonly IEnumerable<SampleItemViewModel> _sampleItems = SampleItemFactory.CreateSampleItems<SampleItemViewModel>(4);
        public IEnumerable<SampleItemViewModel> SampleItems
        {
            get => _sampleItems;
        }


        int _selectedIndex = 0;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => RASIC(ref _selectedIndex, value);
        }
    }
}