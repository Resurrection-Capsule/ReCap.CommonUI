using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Transformation;

namespace ReCap.CommonUI.Util
{
    internal static class ConverterHelper
    {
        public static bool TryGetString(object value, out string result)
            => TryGetString(value, false, out result);
        public static bool TryGetString(object value, bool acceptNull, out string result)
        {
            if (value is string val)
                result = val;
            else if (value != null)
                result = value.ToString();
            else if (acceptNull)
                result = "null";
            else
            {
                result = default;
                return false;
            }

            return true;
        }


        public static string ObjectToString(object value)
            => ObjectToString(value, string.Empty);
        public static string ObjectToString(object value, string fallbackValue)
        {
            if (value is string val)
                return val;
            else if (value != null)
                return value.ToString();
            else
                return fallbackValue;
        }




        public static bool TryGetBool(object value, out bool result)
        {
            if (value is bool val)
                result = val;
            else if (TryGetString(value, out string valStr) && bool.TryParse(valStr, out val))
                result = val;
            else
            {
                result = default;
                return false;
            }


            return true;
        }


        public static bool ObjectToBool(object value, bool fallbackValue = false)
            => TryGetBool(value, out bool result)
                ? result
                : fallbackValue
            ;




        public static bool TryGetDouble(object value, out double result)
        {
            if (value is double val)
                result = val;
            else if (TryGetString(value, out string valStr) && double.TryParse(valStr, out val))
                result = val;
            else
            {
                result = default;
                return false;
            }


            return true;
        }


        public static double ObjectToDouble(object value, double fallbackValue = 1d)
            => TryGetDouble(value, out double result)
                ? result
                : fallbackValue
            ;
        /*
        {
            double inVal = fallbackValue;
            
            if (value == null)
                return inVal;


            if (value is double val)
                inVal = val;
            else if (!double.TryParse(value.ToString(), out inVal))
                inVal = fallbackValue;

            return inVal;
        }
        */




        public static bool TryGetInt(object value, out int result)
        {
            if (value is int val)
                result = val;
            else if (TryGetString(value, out string valStr) && int.TryParse(valStr, out val))
                result = val;
            else
            {
                result = default;
                return false;
            }


            return true;
        }


        public static int ObjectToInt(object value, int fallbackValue = 1)
            => TryGetInt(value, out int result)
                ? result
                : fallbackValue
            ;
        /*
        {
            int inVal = fallbackValue;
            
            if (value == null)
                return inVal;


            if (value is int val)
                inVal = val;
            else if (!int.TryParse(value.ToString(), out inVal))
                inVal = fallbackValue;

            return inVal;
        }
        */




        static readonly IEnumerable<Func<string, Matrix>> _MATRIX_PARSERS = new Func<string, Matrix>[]
        {
            s => Matrix.Parse(s),
            s => TransformOperations.Parse(s).Value,
        };
        static readonly IEnumerable<Func<string, Transform>> _TRANSFORM_PARSERS = new Func<string, Transform>[]
        {
            s => Transform.Parse(s),
        };
        static bool TryEach<T>(string value, out T result, IEnumerable<Func<string, T>> parsers, T fallbackValue = default)
        {
            result = fallbackValue;
            foreach (var parser in parsers)
            {
                try
                {
                    result = parser(value);
                    return true;
                }
                catch
                {}
            }
            result = fallbackValue;
            return false;
        }
        public static bool TryGetTransform(object value, out Transform result)
        {
            if (value is Transform val)
                result = val;
            else if (value is ITransform iVal)
                result = new MatrixTransform(iVal.Value);
            else if (!TryGetString(value, out string valStr))
                goto fail;
            else if (TryEach(valStr, out val, _TRANSFORM_PARSERS))
                result = val;
            else if (TryEach(valStr, out Matrix matrix, _MATRIX_PARSERS))
                result = new MatrixTransform(matrix);
            else
                goto fail;

            return true;

            fail:
            result = default;
            return false;
        }


        public static Transform ObjectToTransform(object value, Transform fallbackValue = null)
            => TryGetTransform(value, out Transform result)
                ? result
                : fallbackValue
            ;
    }
}
