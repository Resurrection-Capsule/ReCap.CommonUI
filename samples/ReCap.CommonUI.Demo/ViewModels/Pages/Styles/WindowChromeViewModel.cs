using System;
using System.Collections.Generic;
using System.Linq;
using ReCap.CommonUI;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    public partial class WindowChromeViewModel
        : ViewModelBase
    {
        ManagedChromeMode _currentChromeMode = ManagedChromeMode.Auto;
        public ManagedChromeMode CurrentChromeMode
        {
            get => _currentChromeMode;
            set
            {
                RASIC(ref _currentChromeMode, value);
                UpdateChromeMode(value);
            }
        }


        bool _leftSideButtons = false;
        public bool LeftSideButtons
        {
            get => _leftSideButtons;
            set
            {
                RASIC(ref _leftSideButtons, value);
                UpdateLeftSideButtons(value);
            }
        }


        bool _maxBeforeMin = false;
        public bool MaxBeforeMin
        {
            get => _maxBeforeMin;
            set
            {
                RASIC(ref _maxBeforeMin, value);
                UpdateMaxBeforeMin(value);
            }
        }



        void UpdateChromeMode(ManagedChromeMode mode)
            => App.Current.ManagedChromeHint = mode;

        void UpdateLeftSideButtons(bool leftSide)
            => App.Current.LeftSideButtons = leftSide;

        void UpdateMaxBeforeMin(bool maxBeforeMin)
            => App.Current.ButtonsOrder = maxBeforeMin
                ? CaptionButtonsOrder.MaxMinClose
                : CaptionButtonsOrder.MinMaxClose
            ;
    }
}