using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;
using ReCap.CommonUI.Util.Win32;
using Avalonia;

//using SDBitmap = System.Drawing.Bitmap;
using AvBitmap = Avalonia.Media.Imaging.Bitmap;
using Avalonia.Controls;
using System.Collections.Generic;
using System.Linq;

namespace ReCap.CommonUI.Util
{
    internal class Win32PlatformIconHelperImpl
        : IPlatformIconHelperImpl
    {
        public bool WindowIconUseAppIconAsFallback
        {
            get => true;
        }


        public bool TryGetWindowPlatformIcon(Window window, bool fallbackToAppIcon, out IPlatformIcon icon)
        {
            if (window == null)
                goto fail;

            /*
            var windowIcon = window.Icon;
            if (windowIcon == null)
                goto fail;
            */

            icon = new Win32PlatformIcon(window, fallbackToAppIcon);
            return icon.PixelSizes.Any();

            fail:
            icon = null;
            return false;
        }


        public IPlatformIcon GetAppPlatformIcon()
        {
            if (!TryGetAppHIcons(out IntPtr hLargeIcon, out IntPtr hSmallIcon))
                return null;


            List<Icon> icons = new();

            Icon smallIcon = Icon.FromHandle(hSmallIcon);
            if (smallIcon != null)
                icons.Add(smallIcon);

            Icon largeIcon = Icon.FromHandle(hLargeIcon);
            if (largeIcon != null)
                icons.Add(largeIcon);

            var icon = new Win32PlatformIcon(icons);
            return icon.PixelSizes.Any()
                ? icon
                : null
            ;
        }


        static bool TryGetAppHIcons(out IntPtr hLargeIcon, out IntPtr hSmallIcon)
        {
            string exePath = Process.GetCurrentProcess().MainModule.FileName;
#if DEBUG
            if (Path.GetFileNameWithoutExtension(exePath).Equals("dotnet", StringComparison.InvariantCultureIgnoreCase))
                throw new Exception(exePath);

            if (!File.Exists(exePath))
                throw new FileNotFoundException("Executable not found!", fileName: exePath);
#endif

            uint extractIconExResult = Win32Methods.ExtractIconEx(exePath, 0, out hLargeIcon, out hSmallIcon, 1);
            return extractIconExResult > 0; // ?????
        }
        /*
        Bitmap GetAppIconAsSystemDrawingBitmap(PixelSize pxSize)
        {
            string exePath = Process.GetCurrentProcess().MainModule.FileName;
            if (Path.GetFileNameWithoutExtension(exePath).Equals("dotnet", StringComparison.InvariantCultureIgnoreCase))
                throw new Exception(exePath);

            if (!File.Exists(exePath))
                throw new FileNotFoundException("Executable not found!", fileName: exePath);

            uint extractIconExResult = Win32Methods.ExtractIconEx(exePath, 0, out IntPtr hLargeIcon, out IntPtr hSmallIcon, 1);
            IntPtr hIcon = GetHIconForSize(pxSize, hLargeIcon, hSmallIcon, out bool isLargeIconValid, out bool isSmallIconValid);


            Bitmap bitmap = HIconToBitmap(hIcon, extractIconExResult);

            if (isLargeIconValid)
                Win32Methods.DestroyIcon(hLargeIcon);

            if (isSmallIconValid)
                Win32Methods.DestroyIcon(hSmallIcon);


            return bitmap;
        }
        */




        static IntPtr GetHIconForSize(PixelSize pxSize, IntPtr hLargeIcon, IntPtr hSmallIcon, out bool isLargeIconValid, out bool isSmallIconValid)
            => GetHIconForSize(PlatformIconHelper.PreferLargeIcon(pxSize), hLargeIcon, hSmallIcon, out isLargeIconValid, out isSmallIconValid);
        static IntPtr GetHIconForSize(bool preferLargeIcon, IntPtr hLargeIcon, IntPtr hSmallIcon, out bool isLargeIconValid, out bool isSmallIconValid)
        {
            isLargeIconValid = Win32PlatformIconExtensions.IsIconValid(hLargeIcon);
            isSmallIconValid = Win32PlatformIconExtensions.IsIconValid(hSmallIcon);


            var largeIcon = (hLargeIcon, isLargeIconValid);
            var smallIcon = (hSmallIcon, isSmallIconValid);

            /*
            if (preferLargeIcon)
                return GetPreferredHIcon(hLargeIcon, isLargeIconValid, hSmallIcon, isSmallIconValid);
            else
                return GetPreferredHIcon(hSmallIcon, isSmallIconValid, hLargeIcon, isLargeIconValid);
            */
            if (preferLargeIcon)
                return GetPreferredHIcon(largeIcon, smallIcon);
            else
                return GetPreferredHIcon(smallIcon, largeIcon);
        }
        /*
        static IntPtr GetPreferredHIcon(IntPtr hPrimaryIcon, bool isPrimaryIconValid, IntPtr hSecondaryIcon, bool isSecondaryIconValid)
        {
            if (isPrimaryIconValid)
                return hPrimaryIcon;
            else if (isSecondaryIconValid)
                return hSecondaryIcon;
            else
                return IntPtr.Zero;
        }
        */
        static IntPtr GetPreferredHIcon(
            (IntPtr HIcon, bool IsValid) primary,
            (IntPtr HIcon, bool IsValid) secondary
        )
        {
            if (primary.IsValid)
                return primary.HIcon;
            else if (secondary.IsValid)
                return secondary.HIcon;
            else
                return IntPtr.Zero;
        }


        /*
        static Stream HIconToStream(IntPtr hIcon, uint extractIconExResult)
        {
            bitmap.Save(iconStream, ImageFormat.Png);

            iconStream.Position = 0;
            return iconStream;
        }
        */


        static Bitmap HIconToBitmap(IntPtr hIcon, uint extractIconExResult)
        {
            if (hIcon == IntPtr.Zero)
            {
                int lastWin32Error = Marshal.GetLastWin32Error();
                string message = string.Join("\n", new[]
                {
                    $"{nameof(Win32Methods.ExtractIconEx)} => {extractIconExResult}",
                    $"{nameof(hIcon)} == {nameof(IntPtr)}.{nameof(IntPtr.Zero)}",
                    $"{nameof(Marshal)}.{nameof(Marshal.GetLastWin32Error)}() => {lastWin32Error}",
                });
                throw new NullReferenceException(message);
            }

            using var icon = Icon.FromHandle(hIcon);
            return icon.ToBitmap();
        }
    }


    internal sealed class Win32PlatformIcon
        : PlatformIconBase
    {
        public Win32PlatformIcon(params Bitmap[] bitmaps)
            : this((IEnumerable<Bitmap>)bitmaps)
        {}
        public Win32PlatformIcon(IEnumerable<Bitmap> bitmaps)
            : base(bitmaps.Select(b => b.ToAvBitmap()))
        {}


        public Win32PlatformIcon(params Icon[] icons)
            : this((IEnumerable<Icon>)icons)
        {}
        public Win32PlatformIcon(IEnumerable<Icon> icons)
            : base(icons.Select(i => i.ToAvBitmap()))
        {}


        public Win32PlatformIcon(WindowIcon windowIcon)
            : this(windowIcon.ToIcon())
        {}
        public Win32PlatformIcon(Window window, bool fallbackToAppIcon)
            : base(GetFromWindow(window, fallbackToAppIcon))
        {}
        /*
        static void A(Icon icon)
        {

            UInt32 flags = (0x000000000 | 0x100);
            ShFileInfo shInfo = new ShFileInfo();
            Win32Methods.SHGetFileInfo(ItemPath, 0, ref shInfo, (UInt32)Marshal.SizeOf(shInfo), flags);
            Icon result = (Icon)(Icon.FromHandle(shInfo.hIcon).Clone());
            Win32Methods.DestroyIcon(shInfo.hIcon);

            Guid guid = Win32Structures.IID_IMAGE_LIST;
            var hres = Win32Methods.SHGetImageList(0x4, ref guid, out IImageList list);
            IntPtr resultHandle = IntPtr.Zero;
            list.GetIcon(shInfo.iIcon, 1, ref resultHandle);
            Icon finalResult = (Icon)(Icon.FromHandle(resultHandle).Clone());
            Win32Methods.DestroyIcon(resultHandle);
            return finalResult;
        }
        */
        static readonly IEnumerable<AvBitmap> _EMPTY = Array.Empty<AvBitmap>();
        static IEnumerable<AvBitmap> GetFromWindow(Window window, bool fallbackToAppIcon)
            => window.TryGetHWnd(out IntPtr hWnd)
                ? GetFromHWnd(hWnd, window.RenderScaling, fallbackToAppIcon)
                : _EMPTY
            ;


        static IEnumerable<AvBitmap> GetFromHWnd(IntPtr hWnd, double renderScaling, bool fallbackToAppIcon)
        {
            if (!Win32Methods.IsWindow(hWnd))
                return _EMPTY;


            List<AvBitmap> bitmaps = new();

            if (Win32PlatformIconExtensions.TryGetIconViaMessage(hWnd, WindowIconFromMessageParam.SMALL, renderScaling, out Icon smallIcon))
                bitmaps.Add(smallIcon.ToAvBitmap());

            if (Win32PlatformIconExtensions.TryGetIconViaMessage(hWnd, WindowIconFromMessageParam.BIG, renderScaling, out Icon bigIcon))
                bitmaps.Add(bigIcon.ToAvBitmap());

            if (fallbackToAppIcon && (bitmaps.Count <= 0) && Win32PlatformIconExtensions.TryGetIconViaMessage(hWnd, WindowIconFromMessageParam.SMALL2, renderScaling, out Icon smallIcon2))
                bitmaps.Add(smallIcon2.ToAvBitmap());


            return bitmaps;
        }


    }


    internal static class Win32PlatformIconExtensions
    {
        public static AvBitmap ToAvBitmap(this Icon icon)
        {
            using var bitmap = icon.ToBitmap();
            return bitmap.ToAvBitmap();
        }


        public static AvBitmap ToAvBitmap(this Bitmap bitmap)
        {
            if (bitmap == null)
                return null;

            using (bitmap)
            {
                using MemoryStream iconStream = new();
                bitmap.Save(iconStream, ImageFormat.Png);
                iconStream.Position = 0;
                return new AvBitmap(iconStream);
            }
        }


        public static Icon ToIcon(this WindowIcon windowIcon)
        {
            if (windowIcon == null)
                return null;

            using MemoryStream iconStream = new();
            windowIcon.Save(iconStream);
            iconStream.Position = 0;
            return new(iconStream);
        }


        public static bool TryGetIconInfo(IntPtr hIcon, out ICONINFO iconInfo)
        {
            if ((hIcon != IntPtr.Zero) && Win32Methods.GetIconInfo(hIcon, out iconInfo))
                return true;

            iconInfo = default;
            return false;
        }


        public static bool IsIconValid(IntPtr hIcon)
            => TryGetIconInfo(hIcon, out ICONINFO iconInfo)
            && iconInfo.IsIcon()
        ;



        public static bool TryGetHIconViaMessage(IntPtr hWnd, WindowIconFromMessageParam size, double renderScaling, out IntPtr hIcon)
        {
            hIcon = Win32Methods.SendMessage(hWnd, WindowMessage.GETICON, (IntPtr)size, (IntPtr)Extensions.RoundToInt(renderScaling * 96));
            return IsIconValid(hIcon);
        }


        public static bool TryGetIconViaMessage(IntPtr hWnd, WindowIconFromMessageParam size, double renderScaling, out Icon icon)
        {
            if (!TryGetHIconViaMessage(hWnd, size, renderScaling, out IntPtr hIcon))
            {
                icon = null;
                return false;
            }

            icon = Icon.FromHandle(hIcon);
            return icon != null;
        }
    }
}