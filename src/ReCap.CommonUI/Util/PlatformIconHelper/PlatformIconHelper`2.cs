using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;

using AvBitmap = Avalonia.Media.Imaging.Bitmap;
using IAvBitmapsEnumerable = System.Collections.Generic.IEnumerable<Avalonia.Media.Imaging.Bitmap>;

namespace ReCap.CommonUI.Util
{
    public static partial class PlatformIconHelper
    {
        public static bool TryGetVariants(this WindowIcon windowIcon, out IAvBitmapsEnumerable variants)
        {
            if (windowIcon == null)
                goto fail;

            using (MemoryStream iconStream = new())
            {
                windowIcon.Save(iconStream);
                iconStream.Position = 0;

                try
                {
                    AvBitmap bitmap = new(iconStream);
                    variants = Helpers.LonerArray(bitmap);
                    return bitmap != null;
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception);
                }
            }


            fail:
            variants = Array.Empty<AvBitmap>();
            return false;
        }


        internal static bool PreferLargeIcon(PixelSize pxSize)
            => pxSize.ToInts().Min() > 16;


#if NO
        public static bool TryFindNearestSize(IAvBitmapsEnumerable variants, Size limits, out AvBitmap bitmap)
            => TryFindNearestSize(variants, new PixelSize(Helpers.RoundToInt(limits.Width), Helpers.RoundToInt(limits.Height)), out bitmap);


        public static bool TryFindNearestSize(IAvBitmapsEnumerable variants, PixelSize limits, out AvBitmap bitmap)
        {
            if (variants == null)
                goto fail;

            if (!variants.Any())
                goto fail;


            int limit = limits.ToInts().Min();
            foreach (var icon in variants)
            {
                int sizeMin = icon.PixelSize.ToInts().Min();
                if (limit >= sizeMin)
                {
                    bitmap = icon;
                    return true;
                }
            }


            fail:
            bitmap = default;
            return false;
        }
#endif


        public static bool TryGetAtSize(IAvBitmapsEnumerable variants, PixelSize desiredSize, out AvBitmap bitmap, bool roundToNearest = true)
        {
            if (variants == null)
                goto fail;
            if (!variants.Any())
                goto fail;

#if NO
            var exactMatch = variants.FirstOrDefault(x => x.PixelSize == desiredSize);
            if (exactMatch != null)
            {
                bitmap = exactMatch;
                return true;
            }
#else
            foreach (var variant in variants)
            {
                if (variant.PixelSize == desiredSize)
                {
                    bitmap = variant;
                    return true;
                }
            }
#endif


            if (roundToNearest && PixelSizeRounding.TryGetNearest(desiredSize, variants, AvBitmap_getSize, out bitmap))
                return true;


            fail:
            bitmap = null;
            return false;
        }
        static PixelSize AvBitmap_getSize(AvBitmap bitmap)
            => bitmap.PixelSize;


        public static bool Contains(IAvBitmapsEnumerable variants, PixelSize desiredSize)
        {
            if (variants == null)
                return false;

            return variants.Any(x => x.PixelSize == desiredSize);
        }

        static void HandleNotImplementedException(NotImplementedException exception, [CallerMemberName] string functionName = null)
        {
            Debug.WriteLine($"{nameof(PlatformIconHelper)}.{functionName} not implemented on current platform!\n{exception?.Message}");
        }
    }
}