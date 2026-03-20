using System;
using System.Globalization;
using System.Linq;
using Avalonia.Data.Converters;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Converters
{
    public static class CountIEnumerableConverters
    {
        public static readonly IValueConverter Count = new CountIEnumerableConverter();
        internal class CountIEnumerableConverter
            : IValueConverter
        {
            internal CountIEnumerableConverter()
            {}

            public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                => CountIEnumerable(value);
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                => throw new NotSupportedException();


            internal static int CountIEnumerable(object value)
                => value is System.Collections.IEnumerable enumerable
                    ? enumerable.Count()
                    : 0
                ;
        }




        internal abstract class CompareConverter
            : IValueConverter
        {
            protected CompareConverter()
            {}


            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                => Convert(CountIEnumerableConverter.CountIEnumerable(value), parameter);
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                => throw new NotSupportedException();


            protected abstract bool Convert(int count, object parameter);
        }


        public static readonly IValueConverter Any = new AnyConverter();
        internal class AnyConverter
            : CompareConverter
        {
            protected override bool Convert(int count, object parameter)
                => count > 0;
        }


        public static readonly IValueConverter None = new NoneConverter();
        internal class NoneConverter
            : AnyConverter
        {
            protected override bool Convert(int count, object parameter)
                => !base.Convert(count, parameter);
        }




        internal abstract class CompareToParameterConverter
            : CompareConverter
        {
            protected override bool Convert(int count, object parameter)
                => Convert(count, (int)Math.Round(NumberConvUtils.ObjectToDouble(parameter)));
            protected abstract bool Convert(int count, int parameter);
        }


        public static readonly IValueConverter GreaterOrEqualTo = new GreaterOrEqualConverter();
        internal sealed class GreaterOrEqualConverter
            : CompareToParameterConverter
        {
            protected override bool Convert(int count, int parameter)
                => count >= parameter;
        }


        public static readonly IValueConverter Greater = new GreaterConverter();
        internal sealed class GreaterConverter
            : CompareToParameterConverter
        {
            protected override bool Convert(int count, int parameter)
                => count > parameter;
        }


        public static readonly IValueConverter LessOrEqualTo = new LessOrEqualConverter();
        internal sealed class LessOrEqualConverter
            : CompareToParameterConverter
        {
            protected override bool Convert(int count, int parameter)
                => count <= parameter;
        }


        public static readonly IValueConverter Less = new LessConverter();
        internal sealed class LessConverter
            : CompareToParameterConverter
        {
            protected override bool Convert(int count, int parameter)
                => count < parameter;
        }


        public static readonly IValueConverter Equal = new EqualConverter();
        internal sealed class EqualConverter
            : CompareToParameterConverter
        {
            protected override bool Convert(int count, int parameter)
                => count == parameter;
        }
    }
}
