using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    public class MenuViewModel
        : PageViewModelBase
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
            if (parameter is SampleItemViewModel vm)
                ExecuteItemVM(vm);
            else if (parameter is MenuItem item)
                ExecuteItem(item);
        }


        public void ExecuteItemVM(SampleItemViewModel item)
        {
            if (item != null)
                SampleText = item.ToString(extended: true);
        }


        public void ExecuteItem(MenuItem item)
        {
            if (item == null)
                return;

            var dc = item.DataContext;
            if (dc is SampleItemViewModel vm)
            {
                ExecuteItemVM(vm);
                return;
            }
            /*
            else if (dc != null)
            {
                SampleText = dc.ToString();
                return;
            }
            */


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




        static readonly Thickness _POPOUT_PADDING = new(2d);
        protected override void CustomizePopOutWindow(ref Window window)
        {
            base.CustomizePopOutWindow(ref window);
            window.Padding = _POPOUT_PADDING;
        }
    }
}