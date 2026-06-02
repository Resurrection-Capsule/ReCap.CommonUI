using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace ReCap.CommonUI.Converters
{
    public static class NullComparisonConverters
    {
        abstract class NullComparisonConverterBase
            : IValueConverter
        {
            protected abstract bool Compare(object value);
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                => Compare(value);

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }




        public static readonly IValueConverter Null = new NullConverter();
        sealed class NullConverter
            : NullComparisonConverterBase
        {
            protected override bool Compare(object value)
                => value == null;
        }




        public static readonly IValueConverter NotNull = new NotNullConverter();
        sealed class NotNullConverter
            : NullComparisonConverterBase
        {
            protected override bool Compare(object value)
                => value != null;
        }
    }
}