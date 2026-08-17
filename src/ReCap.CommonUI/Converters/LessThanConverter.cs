using System;
using System.Globalization;
using Avalonia.Data.Converters;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Converters
{
    public class LessThanConverter
        : IValueConverter
    {
        public static readonly LessThanConverter Instance = new();
        private LessThanConverter()
            : base()
        {}


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = ConverterHelper.ObjectToDouble(value);
            double param = ConverterHelper.ObjectToDouble(parameter);
            return val < param;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
