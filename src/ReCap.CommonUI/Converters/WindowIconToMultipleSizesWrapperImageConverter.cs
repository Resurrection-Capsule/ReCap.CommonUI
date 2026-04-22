using System;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using ReCap.CommonUI.Controls.Decorators;
using ReCap.CommonUI.Util;

using IAvBitmapsEnumerable = System.Collections.Generic.IEnumerable<Avalonia.Media.Imaging.Bitmap>;

namespace ReCap.CommonUI.Converters
{
    public sealed partial class WindowIconToMultipleSizesWrapperImageConverter
        : IValueConverter
    {
        public static readonly WindowIconToMultipleSizesWrapperImageConverter Instance = new();
        private WindowIconToMultipleSizesWrapperImageConverter()
        {}




        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => TryConvertToIAvBitmapsEnumerable(value, targetType, parameter, culture, out IAvBitmapsEnumerable variants)
                ? new MultipleSizesWrapperImage(variants)
                : null
            ;


        //MultipleSizesWrapperImage ConvertToMultipleSizesWrapperImage
        bool TryConvertToIAvBitmapsEnumerable(object value, Type targetType, object parameter, CultureInfo culture, out IAvBitmapsEnumerable variants)
        {
            if (value is Window window)
            {
                if (window.TryGetIconVariants(out variants, fallbackToAppIcon: true))
                    return true;
            }
            else if (value is WindowIcon windowIcon)
            {
                if (windowIcon.TryGetVariants(out variants))
                    return true;
            }

            variants = PlatformIconHelper.GetAppIconVariants();
            return variants?.Any() ?? false;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}