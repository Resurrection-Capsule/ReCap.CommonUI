using System;
using System.Globalization;
using Avalonia.Data.Converters;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Converters
{
    public static class ArithmeticConverters
    {
        public static readonly IValueConverter Invert = new InvertConverter();
        sealed class InvertConverter
            : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                => -ConverterHelper.ObjectToDouble(value, 0d);
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }




        protected internal abstract class ArithmeticConverterBase
            : IValueConverter
        {
            protected abstract bool Compare(double value, double param);


            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                double val = ConverterHelper.ObjectToDouble(value);
                double param = ConverterHelper.ObjectToDouble(parameter);
                return Compare(val, param);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }




        public static readonly IValueConverter LessThan = new LessThanConverter();
        sealed class LessThanConverter
            : ArithmeticConverterBase
        {
            protected override bool Compare(double value, double param)
                => value < param;
        }


        public static readonly IValueConverter LessThanOrEqual = new LessThanOrEqualConverter();
        sealed class LessThanOrEqualConverter
            : ArithmeticConverterBase
        {
            protected override bool Compare(double value, double param)
                => value <= param;
        }


        public static readonly IValueConverter GreaterThan = new GreaterThanConverter();
        sealed class GreaterThanConverter
            : ArithmeticConverterBase
        {
            protected override bool Compare(double value, double param)
                => value > param;
        }



        public static readonly IValueConverter GreaterThanOrEqual = new GreaterThanOrEqualConverter();
        sealed class GreaterThanOrEqualConverter
            : ArithmeticConverterBase
        {
            protected override bool Compare(double value, double param)
                => value >= param;
        }


        public static readonly IValueConverter Equal = new EqualConverter();
        class EqualConverter
            : ArithmeticConverterBase
        {
            // https://github.com/godotengine/godot/blob/64596092ae60e8fc10da7b61ac024d099a80a3b7/modules/mono/glue/GodotSharp/GodotSharp/Core/MathfEx.cs#L25
            const double _EPSILON = 1e-14;


            // https://github.com/godotengine/godot/blob/64596092ae60e8fc10da7b61ac024d099a80a3b7/modules/mono/glue/GodotSharp/GodotSharp/Core/Mathf.cs
            protected override bool Compare(double value, double param)
            {
                // Check for exact equality first, required to handle "infinity" values.
                if (value == param)
                {
                    return true;
                }
                // Then check for approximate equality.
                double tolerance = _EPSILON * Math.Abs(value);
                if (tolerance < _EPSILON)
                {
                    tolerance = _EPSILON;
                }
                return Math.Abs(value - param) < tolerance;
            }
        }


        public static readonly IValueConverter NotEqual = new NotEqualConverter();
        sealed class NotEqualConverter
            : EqualConverter
        {
            protected override bool Compare(double value, double param)
                => !base.Compare(value, param);
        }
    }
}