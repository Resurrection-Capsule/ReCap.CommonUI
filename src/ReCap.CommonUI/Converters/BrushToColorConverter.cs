using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace ReCap.CommonUI.Converters
{
    public sealed class BrushToColorConverter
        : IValueConverter
    {
        public static readonly BrushToColorConverter Instance = new();
        private BrushToColorConverter()
        {}




        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color color)
                return color;
            else if (value is ISolidColorBrush scb)
                return scb.Color;
            else if (value is IGradientBrush gb)
            {
                var stops = gb.GradientStops;
                if ((stops != null) && (stops.Count > 0))
                    return stops[0];
            }

            return null;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}