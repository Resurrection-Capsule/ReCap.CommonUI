using System;
using System.Collections.Generic;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    public class ComboBoxViewModel
        : PageViewModelBase
    {
        readonly IEnumerable<SampleItemViewModel> _sampleItems = SampleItemViewModel.CreateSampleItems(4);
        public IEnumerable<SampleItemViewModel> SampleItems
        {
            get => _sampleItems;
        }
    }
}