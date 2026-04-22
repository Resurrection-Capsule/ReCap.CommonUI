using System;
using Avalonia.Data.Converters;

namespace ReCap.CommonUI.Converters
{
    internal static class NumberConvUtils
    {
        public static double ObjectToDouble(object value, double fallbackValue = 1d)
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


        public static int ObjectToInt(object value, int fallbackValue = 1)
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
    }
}
