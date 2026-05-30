using System;
using System.Collections.Generic;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    public class ComboBoxViewModel
        : ViewModelBase
    {
        readonly IEnumerable<SampleItemViewModel> _sampleItems = SampleItemFactory.CreateSampleItems<SampleItemViewModel>(4);
        public IEnumerable<SampleItemViewModel> SampleItems
        {
            get => _sampleItems;
        }
    }
}