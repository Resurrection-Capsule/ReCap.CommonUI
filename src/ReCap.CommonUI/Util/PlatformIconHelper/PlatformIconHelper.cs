using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI.Util
{
    public enum WindowIconRequestFallbackMode
    {
        NoFallback = 0,
        Auto,
        FallbackToApp,
    }


    public static class PlatformIconHelper
    {
        static readonly IPlatformIconHelperImpl _IMPL = PlatformUtils.GetForPlatform<IPlatformIconHelperImpl>();


        public static bool TryGetPlatformIcon(this Window window, out IPlatformIcon icon, WindowIconRequestFallbackMode mode = WindowIconRequestFallbackMode.Auto)
        {
            bool fallbackToAppIcon = EvaluateWindowIconRequestFallbackMode(mode);

            if (_IMPL.TryGetWindowPlatformIcon(window, fallbackToAppIcon, out icon))
                return true;
            else if (fallbackToAppIcon && TryGetAppPlatformIcon(out icon))
                return true;

            icon = null;
            return false;
        }


        static bool EvaluateWindowIconRequestFallbackMode(WindowIconRequestFallbackMode mode)
            => mode switch
            {
                WindowIconRequestFallbackMode.NoFallback => false,
                WindowIconRequestFallbackMode.FallbackToApp => true,
                _ => _IMPL.WindowIconUseAppIconAsFallback,
            };




        public static bool TryGetAppPlatformIcon(out IPlatformIcon icon)
        {
            try
            {
                icon = _IMPL.GetAppPlatformIcon();
                return icon != null;
            }
            catch (NotImplementedException)
            {
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{_IMPL.GetType().FullName}.{nameof(_IMPL.GetAppPlatformIcon)} threw exception '{ex?.GetType().FullName}'!");
                Debug.WriteLine(ex);
            }

            icon = null;
            return false;
        }


        internal const int MAX_SMALL_ICON_EXTENT = 16;
        internal static readonly PixelSize MAX_SMALL_ICON_SIZE = new(MAX_SMALL_ICON_EXTENT, MAX_SMALL_ICON_EXTENT);
        internal static bool PreferLargeIcon(PixelSize pxSize)
            => pxSize.ToInts().Min() > MAX_SMALL_ICON_EXTENT;
            //=> Math.Min(pxSize.Width, pxSize.Height) > MAX_SMALL_ICON_EXTENT;


        public static bool TryFindNearestSize(this IPlatformIcon icon, Size limits, out PixelSize optimalSize)
            => icon.TryFindNearestSize(new PixelSize(Extensions.RoundToInt(limits.Width), Extensions.RoundToInt(limits.Height)), out optimalSize);


        public static bool TryFindNearestSize(this IPlatformIcon icon, PixelSize limits, out PixelSize optimalSize)
        {
            if (icon == null)
                goto fail;

            var iconSizes = icon.PixelSizes;
            if (iconSizes == null)
                goto fail;

            if (!iconSizes.Any())
                goto fail;


            int limit = limits.ToInts().Min();
            foreach (var iconSize in iconSizes)
            {
                int sizeMin = iconSize.ToInts().Min();
                if (limit >= sizeMin)
                {
                    optimalSize = iconSize;
                    return true;
                }
            }


            fail:
            optimalSize = default;
            return false;
        }


        const bool _DEFAULT_TryGetAtSize_roundToNearest = true;
        public static bool TryGetAtSize(this IPlatformIcon icon, PixelSize desiredSize, out Avalonia.Media.Imaging.Bitmap bitmap, bool roundToNearest = true)
        {
            if (icon == null)
                goto fail;

            if (icon.Contains(desiredSize))
            {
                bitmap = icon[desiredSize];
                return true;
            }


            if (!roundToNearest)
                goto fail;


            var pxSizes = icon.PixelSizes;
            if (pxSizes == null)
                goto fail;

            if (PixelSizeRounding.TryGetNearest(desiredSize, pxSizes, out PixelSize matched))
            {
                if (icon.TryGetAtSize(matched, out bitmap, false))
                    return true;
            }


            fail:
            bitmap = null;
            return false;
        }


        public static bool Contains(this IPlatformIcon icon, PixelSize pxSize)
        {
            if (icon == null)
                return false;

            var pxSizes = icon.PixelSizes;
            if (pxSizes == null)
                return false;

            return pxSizes.Contains(pxSize);
        }

    }
        /*
        https://stackoverflow.com/questions/3498968/find-the-nearest-dot-in-a-2d-space
        https://stackoverflow.com/a/3499125
        https://stackoverflow.com/questions/3498968/find-the-nearest-dot-in-a-2d-space/3499125#3499125
        */
    internal static class PixelSizeRounding
    {
        public static bool TryGetNearest(PixelSize to, IEnumerable<PixelSize> among, out PixelSize matched)
        {
            if (among == null)
                goto fail;

            int count = among.Count();
            if (count <= 0)
                goto fail;

            var result = GetNearest(among, 0, count - 1, to);
            if (result.HasValue)
            {
                matched = result.Value;
                return true;
            }

            fail:
            matched = default;
            return false;
        }
        // Find the nearest dot in array[low...high] inclusive which is closest to point d 
        static PixelSize? GetNearest(IEnumerable<PixelSize> array, int low, int high, PixelSize d)
        {
            if (low > high)
                return null;
            if (low == high)
                return array.ElementAt(low);

            int middle = low + (high - low) >> 1;
            PixelSize? p1 = GetNearest(array, low, middle, d);
            PixelSize? p2 = GetNearest(array, middle + 1, high, d);

            if (p1 == null) return p2;
            if (p2 == null) return p1;

            return Nearer(p1, p2, d);
        }

        static PixelSize Nearer(PixelSize? p1, PixelSize? p2, PixelSize d)
        {
            bool hasP1 = (p1 != null) && p1.HasValue;
            bool hasP2 = (p2 != null) && p2.HasValue;
            if (hasP1 && hasP2)
                return Nearer(p1.Value, p2.Value, d);
            else if (hasP1)
                return p1.Value;
            else if (hasP2)
                return p2.Value;
            else
                return d;
        }
        static PixelSize Nearer(PixelSize p1, PixelSize p2, PixelSize d)
            => DistanceBetween(d, p1) <= DistanceBetween(d, p2)
                ? p1
                : p2
            ;
        static double DistanceBetween(PixelSize a, PixelSize b)
            => DistanceBetween(a.Width, a.Height, b.Width, b.Height);
        static double DistanceBetween(PixelPoint a, PixelPoint b)
            => DistanceBetween(a.X, a.Y, b.X, b.Y);
        static double DistanceBetween(double x1, double y1, double x2, double y2)
            => Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
    }
}