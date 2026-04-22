using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;

using AvBitmap = Avalonia.Media.Imaging.Bitmap;
using IAvBitmapsEnumerable = System.Collections.Generic.IEnumerable<Avalonia.Media.Imaging.Bitmap>;

namespace ReCap.CommonUI.Util
{
    public static partial class PlatformIconHelper
    {
        static readonly IPlatformIconHelperImpl _IMPL = PlatformUtils.GetForPlatform<IPlatformIconHelperImpl>();


        public static bool TryGetIconVariants(this Window window, out IAvBitmapsEnumerable variants, bool fallbackToAppIcon = true)
        {
            bool handledFallbackToAppIcon = false;
            try
            {
                variants = _IMPL.GetIconVariantsForWindow(window, fallbackToAppIcon, out handledFallbackToAppIcon);
            }
            catch (NotImplementedException exception)
            {
                variants = null;
                HandleNotImplementedException(exception);
            }
            catch (Exception)
            {
                variants = null;
            }


            if ((variants != null) && variants.Any())
                return true;


            if (window.Icon?.TryGetVariants(out variants) ?? false)
            {
                return true;
            }
            else if (fallbackToAppIcon && !handledFallbackToAppIcon)
            {
                variants = GetAppIconVariants();
                return variants.Any();
            }
            else
            {
                variants = Array.Empty<AvBitmap>();
                return false;
            }
        }


        public static IAvBitmapsEnumerable GetAppIconVariants()
        {
            try
            {
                IAvBitmapsEnumerable variants = _IMPL.GetIconVariantsForApp();
                if (variants != null)
                    return variants.SortBySize();
            }
            catch (NotImplementedException)
            {
            }
            catch (Exception exception)
            {
                throw exception;
            }

            
            return Array.Empty<AvBitmap>();
        }

        static IAvBitmapsEnumerable SortBySize(this IAvBitmapsEnumerable variants)
            => variants
                .OrderBy(GetSizeMinAxis)
            ;
        static int GetSizeMinAxis(this AvBitmap bitmap)
            => bitmap.PixelSize.ToInts().Min();
    }
}