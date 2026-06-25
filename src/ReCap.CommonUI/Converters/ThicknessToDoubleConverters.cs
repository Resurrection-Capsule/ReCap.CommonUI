using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace ReCap.CommonUI.Converters
{
    public static class ThicknessToDoubleConverters
    {
        abstract class ThicknessToDoubleConverterBase
            : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                => Convert((Thickness)value);

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                => throw new NotSupportedException();


            protected abstract double Convert(Thickness thickness);
        }




        public static readonly IValueConverter Left = new ThicknessLeftToDoubleConverter();
        sealed class ThicknessLeftToDoubleConverter
            : ThicknessToDoubleConverterBase
        {
            protected override double Convert(Thickness thickness)
                => thickness.Left;
        }




        public static readonly IValueConverter Top = new ThicknessTopToDoubleConverter();
        sealed class ThicknessTopToDoubleConverter
            : ThicknessToDoubleConverterBase
        {
            protected override double Convert(Thickness thickness)
                => thickness.Top;
        }




        public static readonly IValueConverter Right = new ThicknessRightToDoubleConverter();
        sealed class ThicknessRightToDoubleConverter
            : ThicknessToDoubleConverterBase
        {
            protected override double Convert(Thickness thickness)
                => thickness.Right;
        }




        public static readonly IValueConverter Bottom = new ThicknessBottomToDoubleConverter();
        sealed class ThicknessBottomToDoubleConverter
            : ThicknessToDoubleConverterBase
        {
            protected override double Convert(Thickness thickness)
                => thickness.Bottom;
        }
    }
}