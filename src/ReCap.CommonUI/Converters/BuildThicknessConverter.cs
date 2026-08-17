using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data;
using Avalonia.Data.Converters;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Converters
{
    public sealed class BuildThicknessConverter
        : IMultiValueConverter
    {
        public static readonly BuildThicknessConverter Instance = new();
        private BuildThicknessConverter()
        {}




        static readonly char[] _SEPARATORS = new[]
        {
            ',',
            ' ',
        };
        const int _MIN_SIDES = 2;
        const int _MAX_SIDES = 4;
        const string _USE_BINDING = "_";




        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                goto fail;

            if (!TryParseParameter(parameter
                , out double? left, out double? top, out double? right, out double? bottom
                , out bool axisSidesShared
            ))
                goto fail;


            int index = 0;
            double horizontal = GetSide(left, values, ref index);
            double vertical = GetSide(top, values, ref index);

            if (axisSidesShared)
                return new Thickness(horizontal, vertical);

            return new Thickness(
                horizontal
                , vertical
                , GetSide(right, values, ref index)
                , GetSide(bottom, values, ref index)
            );


            fail:
            return BindingOperations.DoNothing;
        }


        static double GetSide(double? value, IList<object> values, ref int idx)
        {
            if ((value != null) && value.HasValue)
                return value.Value;
            else
            {
                double result = ConverterHelper.ObjectToDouble(values[idx]);
                idx++;
                return result;
            }
        }


        static bool TryParseParameter(object parameter
            , out double? left, out double? top, out double? right, out double? bottom
            , out bool axisSidesShared
        )
        {
            if (!ConverterHelper.TryGetString(parameter, out string data))
                goto fail;
            else if (string.IsNullOrWhiteSpace(data))
                goto fail;
            else if (!_SEPARATORS.Any(data.Contains))
                goto fail;

            string[] segments = data.Split(_SEPARATORS);
            int count = segments.Length;
            if (count < _MIN_SIDES)
                goto fail;
            else if (count > _MAX_SIDES)
                goto fail;
            else if (count == 3)
                goto fail;
            else
            {
                left = ParseSide(segments[0]);
                top = ParseSide(segments[1]);
                if (count >= _MAX_SIDES)
                {
                    right = ParseSide(segments[2]);
                    bottom = ParseSide(segments[3]);
                    axisSidesShared = false;
                }
                else
                {
                    right = left;
                    bottom = top;
                    axisSidesShared = true;
                }
                return true;
            }


            fail:
            left = default;
            top = default;
            right = default;
            bottom = default;
            axisSidesShared = false;
            return false;
        }


        static double? ParseSide(string data)
            => data != _USE_BINDING
                ? double.Parse(data)
                : null
            ;
    }
}