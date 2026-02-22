using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Reactive;

namespace ReCap.CommonUI.Converters
{
    public sealed class StringFormatConverter
        : IMultiValueConverter
    {
        public static readonly StringFormatConverter Instance = new();
        private StringFormatConverter()
        {}
        

        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            string format = (string)values[0];
            
            object[] args = values
                .Skip(1)
                .ToArray()
            ;
            
            return string.Format(format, args);
        }
    }
}
