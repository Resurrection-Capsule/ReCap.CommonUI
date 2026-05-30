using System;
using System.Collections.Generic;
using Avalonia.Controls;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    public class MenuViewModel
        : ViewModelBase
    {
        readonly IEnumerable<SampleMenuItemViewModel> _sampleItems = SampleItemFactory.CreateSampleItems<SampleMenuItemViewModel>(4);
        public IEnumerable<SampleMenuItemViewModel> SampleItems
        {
            get => _sampleItems;
        }


        string _sampleText = string.Empty;
        public string SampleText
        {
            get => _sampleText;
            set => RASIC(ref _sampleText, value);
        }




        public void ExecuteCommand(object parameter)
        {
            if (parameter is SampleMenuItemViewModel vm)
                ExecuteItemVM(vm);
            else if (parameter is MenuItem item)
                ExecuteItem(item);
        }


        public void ExecuteItemVM(SampleMenuItemViewModel item)
        {
            if (item != null)
                SampleText = item.ToString(extended: true);
        }


        public void ExecuteItem(MenuItem item)
        {
            if (item == null)
                return;

            var dc = item.DataContext;
            if (dc is SampleMenuItemViewModel vm)
            {
                ExecuteItemVM(vm);
                return;
            }
            else if (dc != null)
            {
                SampleText = dc.ToString();
                return;
            }


            var header = item.Header;
            if (header != null)
            {
                SampleText = header.ToString();
            }
            else
            {
                SampleText = item.ToString();
            }
        }
    }
}