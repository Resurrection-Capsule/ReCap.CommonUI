using System;
using System.Collections.Generic;
using System.Globalization;
using Avalonia.Data.Converters;
using ReCap.CommonUI.Attached.WindowChrome;
using ReCap.CommonUI.Controls.AppearanceHacks;

namespace ReCap.CommonUI.Converters
{
    internal sealed class WindowChromeToCaptionButtonRolesConverter
        : IMultiValueConverter
    {
        public static IMultiValueConverter Instance = new WindowChromeToCaptionButtonRolesConverter();




        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is not CaptionButtonClusterPosition position)
                position = CaptionButtonClusterPosition.Unknown;

            if (TryConvertInternal(values, position, out IEnumerable<CaptionButtonRole> roles))
                return roles;
            else
                return Array.Empty<CaptionButtonRole>();
        }


        //delegate bool ConvertInternal_EvaluateValue(object value, ref IEnumerable<CaptionButtonRole> enumerable);
        static bool TryConvertInternal(IList<object> values, CaptionButtonClusterPosition position, out IEnumerable<CaptionButtonRole> enumerable)
        {
            WindowChrome windowChrome = null;
            bool isLeft = position.IsLeft(out bool isValid);
            /*
            static bool EvaluateValue(object value, ref IEnumerable<CaptionButtonRole> enumerable)
            {
                if (value is IEnumerable<CaptionButtonRole> roles)
                {
                    enumerable = roles;
                    return true;
                }
                else
                {
                    return false;
                }
            }

            bool EvaluateValue_ValidPosition(object value, ref IEnumerable<CaptionButtonRole> enumerable)
            {
                if (EvaluateValue(value, ref enumerable))
                    return true;

                if (value is WindowChrome chrome)
                    windowChrome ??= chrome;
                else
                    return false;

                enumerable = isLeft
                    ? windowChrome.LeftCaptionButtonRoles
                    : windowChrome.RightCaptionButtonRoles
                ;
                return true;
            }


            ConvertInternal_EvaluateValue evaluateFunc = isValid
                ? EvaluateValue_ValidPosition
                : EvaluateValue
            ;

            */
            enumerable = null;
            foreach (object value in values)
            {
                if (enumerable != null)
                {
                    if (!isValid)
                        return true;
                    else if (windowChrome != null)
                        return true;
                }
                else if (value is IEnumerable<CaptionButtonRole> roles)
                {
                    enumerable = roles;
                    if (windowChrome != null)
                        return true;
                }
                else if (isValid && (value is WindowChrome chrome))
                {
                    windowChrome ??= chrome;
                }
            }

            if (enumerable != null)
                return true;
            else if (windowChrome == null)
                return false;
            else
                return (enumerable = isLeft
                    ? windowChrome.LeftCaptionButtonRoles
                    : windowChrome.RightCaptionButtonRoles
                ) != null;
        }
    }
}