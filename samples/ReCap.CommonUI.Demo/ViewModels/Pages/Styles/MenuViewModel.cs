using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    public class MenuViewModel
        : PageViewModelBase
    {
        bool _showPlentyOfItems = true;
        public bool ShowPlentyOfItems
        {
            get => _showPlentyOfItems;
            set => RASIC(ref _showPlentyOfItems, value);
        }



        int _howManyItemsIsPlenty = 100;
        public int HowManyItemsIsPlenty
        {
            get => _howManyItemsIsPlenty;
            private set => RASIC(ref _howManyItemsIsPlenty, value);
        }


        IEnumerable<SampleMenuItemViewModel> _samplePlentyOfItems = Array.Empty<SampleMenuItemViewModel>();
        public IEnumerable<SampleMenuItemViewModel> SamplePlentyOfItems
        {
            get => _samplePlentyOfItems;
            private set => RASIC(ref _samplePlentyOfItems, value);
        }



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




        public MenuViewModel()
            : base()
        {
            OnSamplePlentyOfItemsChanged();
            PropertyChanged += This_PropertyChanged;
        }


        private void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(HowManyItemsIsPlenty))
                OnSamplePlentyOfItemsChanged();
        }
        void OnSamplePlentyOfItemsChanged()
            => SamplePlentyOfItems = SampleItemFactory.CreateSampleItems<SampleMenuItemViewModel>(HowManyItemsIsPlenty, disabledCount: 0);




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




        static readonly double _POPOUT_WIDTH = 384d;
        static readonly double _POPOUT_HEIGHT = 240d;
        static readonly Thickness _POPOUT_PADDING = new(2d);
        protected override void CustomizePopOutWindow(ref Window window)
        {
            base.CustomizePopOutWindow(ref window);

            window.Padding = _POPOUT_PADDING;

            window.MinWidth = _POPOUT_WIDTH;
            window.Width = _POPOUT_WIDTH;

            window.MinHeight = _POPOUT_HEIGHT;
            window.Height = _POPOUT_HEIGHT;
        }
    }
}