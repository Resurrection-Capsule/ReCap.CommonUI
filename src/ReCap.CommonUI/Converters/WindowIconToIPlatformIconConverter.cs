using System;
using System.Diagnostics;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using ReCap.CommonUI.Util;
using ReCap.CommonUI.Util.Reflection;

namespace ReCap.CommonUI.Converters
{
    public sealed partial class WindowIconToIPlatformIconConverter
        : IValueConverter
    {
        public static readonly WindowIconToIPlatformIconConverter Instance = new();
        private WindowIconToIPlatformIconConverter()
        {}


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!TryConvertToIPlatformIcon(value, targetType, parameter, culture, out IPlatformIcon platformIcon))
                return null;


#if NO
            if (targetType == null)
                return platformIcon;
            else if (targetType.IsAssignableTo<IBrush>())
            {
                /*
                if (!TryGetPixelSize(parameter, out PixelSize pxSize))
                    return platformIcon;
                / *
                else if (!platformIcon.TryFindNearestSize(pxSize, out PixelSize optimalSize))
                    return platformIcon;
                * /
                else if (!platformIcon.TryGetAtSize(pxSize, out Bitmap bitmap, roundToNearest: true))
                    return platformIcon;
                else
                    return new ImageBrush(bitmap);
                */
                if (TryGetBitmap(platformIcon, parameter, out Bitmap bitmap))
                    return new ImageBrush(bitmap);
            }
            else if (targetType.IsAssignableTo<IImage>() || targetType.IsAssignableTo<Bitmap>())
            {
                if (TryGetBitmap(platformIcon, parameter, out Bitmap bitmap))
                    return bitmap;
            }
            /*
            if (!TryGetPixelSize(parameter, out PixelSize pxSize))
                goto fail;
            */
#else
            if (targetType == null)
                return platformIcon;
            else if (targetType.IsAssignableTo<IBrush>() && TryGetBitmap(platformIcon, parameter, out Bitmap bitmap))
                return new ImageBrush(bitmap);
            else if ((targetType.IsAssignableTo<IImage>() || targetType.IsAssignableTo<Bitmap>()) && TryGetBitmap(platformIcon, parameter, out bitmap))
                return bitmap;
            else
                return platformIcon;
#endif
        }


        static bool TryGetBitmap(IPlatformIcon platformIcon, object parameter, out Bitmap bitmap)
        {
            if (!TryGetPixelSize(parameter, out PixelSize pxSize))
                goto fail;
            /*
            else if (!platformIcon.TryFindNearestSize(pxSize, out PixelSize optimalSize))
                goto fail;
            */
            else
                return platformIcon.TryGetAtSize(pxSize, out bitmap, roundToNearest: true);


            fail:
            bitmap = null;
            return false;
        }


        bool TryConvertToIPlatformIcon(object value, Type targetType, object parameter, CultureInfo culture, out IPlatformIcon platformIcon)
        {
            if (value is IPlatformIcon icon)
            {
                platformIcon = icon;
                return true;
            }
            else if ((value is Window window) && window.TryGetPlatformIcon(out platformIcon))
            {
                return true;
            }
            else if (PlatformIconHelper.TryGetAppPlatformIcon(out platformIcon))
            {
                return true;
            }
            /*
            if (!TryGetPixelSize(parameter, out PixelSize pxSize))
                pxSize = PlatformIconHelper.MAX_SMALL_ICON_SIZE;

            Bitmap bitmap = GetWindowIcon(value, pxSize).ToBitmap(pxSize);
            if (bitmap == null)
            {
                if (!PlatformIconHelper.TryGetAppIcon(pxSize, out IPlatformIcon icon))
                    bitmap = icon[pxSize];
                else
                    return null;
            }

            return FinishResult(bitmap, targetType);
            */
            platformIcon = default;
            return false;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();




        static bool TryGetPixelSize(object parameter, out PixelSize pxSize)
        {
            if (parameter is PixelSize pixelSize)
            {
                pxSize = pixelSize;
                return true;
            }
            else if (parameter is Visual recipient)
            {
                pxSize = PlatformIconHelper.MAX_SMALL_ICON_SIZE;
                return TryGetPixelSizeFromVisual(recipient, ref pxSize);

            }
            else if (PixelSize.TryParse($"{parameter}", out pxSize))
            {}
            else
            {
                goto fail;
            }

            return true;

            fail:
            pxSize = PlatformIconHelper.MAX_SMALL_ICON_SIZE;
            return false;
        }


        static bool TryGetPixelSizeFromVisual(Visual recipient, ref PixelSize pxSize)
        {
            if (recipient == null)
                return false;

            var root = recipient.GetVisualRoot();
            if (root == null)
                return false;

            pxSize = PixelSize.FromSize(recipient.Bounds.Size, root.RenderScaling);
            return true;
        }


        static WindowIcon GetWindowIcon(object value, PixelSize pxSize)
        {
            if (value is WindowIcon icon)
                return icon;
            else if (value is Window window)
                return window.Icon;
            else
                return null;
        }


        static object FinishResult(Bitmap bitmap, Type targetType)
        {
            if (targetType == null)
                return bitmap;
            else if (targetType.IsAssignableTo(typeof(IBrush)))
                return new ImageBrush(bitmap);
            else
                return bitmap;
        }


        /*
        static object AutoGetFromBitmap(this Bitmap bitmap, Type targetType)
        {
            if (targetType == null)
                return bitmap;
            else if (targetType.IsAssignableTo(typeof(Avalonia.Media.IBrush)))
                return new Avalonia.Media.ImageBrush(bitmap);
            else
                return bitmap;
        }
        */
    }
}