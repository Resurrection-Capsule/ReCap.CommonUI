using System;

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
    }
}
