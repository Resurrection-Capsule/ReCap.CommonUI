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


        static bool TryConvertInternal(IList<object> values, CaptionButtonClusterPosition position, out IEnumerable<CaptionButtonRole> enumerable)
        {
            WindowChrome windowChrome = null;
            bool isValid = position.Evaluate(out bool left, out bool right);
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
            else if (left)
                enumerable = windowChrome.LeftCaptionButtonRoles;
            else if (right)
                enumerable = windowChrome.RightCaptionButtonRoles;
            else
                return false;

            return enumerable != null;
        }
    }
}