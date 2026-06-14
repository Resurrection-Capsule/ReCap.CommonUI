using System;
using System.Diagnostics;
using Avalonia;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Controls
{
    public class TitleBarContentContainerViewModel
        : ViewModelBase
    {
        double _contentHeight = -1d;
        public double ContentHeight
        {
            get => _contentHeight;
            set => RASIC(ref _contentHeight, value);
        }


        bool _useReservedCaptionArea = true;
        public bool UseReservedCaptionArea
        {
            get => _useReservedCaptionArea;
            set
            {
                RASIC(ref _useReservedCaptionArea, value);
                App.Current.MainVM.UseReservedCaptionArea = UseReservedCaptionArea;
            }
        }


        string _paddingString = string.Empty;
        public string PaddingString
        {
            get => _paddingString;
            set
            {
                RASIC(ref _paddingString, value);
                try
                {
                    Thickness parsed = Thickness.Parse(value);
                    ParsedPadding = parsed;
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{nameof(PaddingString)}: couldn't parse {typeof(Thickness)} from string '{value}'.");
                    Console.WriteLine(exception);
                }
            }
        }


        Thickness _parsedPadding = new(0d);
        public Thickness ParsedPadding
        {
            get => _parsedPadding;
            set => RASIC(ref _parsedPadding, value);
        }
    }
}