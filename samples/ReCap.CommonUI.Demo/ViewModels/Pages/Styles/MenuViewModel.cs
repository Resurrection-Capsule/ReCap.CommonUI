using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    public class MenuViewModel
        : FlyoutViewModel
    {
#region Properties
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
#endregion




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