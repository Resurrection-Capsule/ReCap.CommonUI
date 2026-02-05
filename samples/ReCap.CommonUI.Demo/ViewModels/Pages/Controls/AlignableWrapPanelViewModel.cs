using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HorizontalAlignment = Avalonia.Layout.HorizontalAlignment;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Controls
{
    public partial class AlignableWrapPanelViewModel
        : ViewModelBase
    {
        HorizontalAlignment _currentAlignment = HorizontalAlignment.Left;
        public HorizontalAlignment CurrentAlignment
        {
            get => _currentAlignment;
            set => RASIC(ref _currentAlignment, value);
        }


        ObservableCollection<RectangleInfo> _rectangles = new();
        public IReadOnlyCollection<RectangleInfo> Rectangles
        {
            get => _rectangles;
        }




        public AlignableWrapPanelViewModel()
        {
            GenerateRectangles(rect => _rectangles.Add(rect));
        }
    }
}