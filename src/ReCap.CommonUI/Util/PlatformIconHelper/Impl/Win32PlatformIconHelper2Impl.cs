using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;
using Avalonia.Controls;
using ReCap.CommonUI.Util.Win32;
using Avalonia;

using AvBitmap = Avalonia.Media.Imaging.Bitmap;
using IAvBitmapsEnumerable = System.Collections.Generic.IEnumerable<Avalonia.Media.Imaging.Bitmap>;
using AvBitmapsList = System.Collections.Generic.List<Avalonia.Media.Imaging.Bitmap>;

namespace ReCap.CommonUI.Util
{
    internal class Win32PlatformIconHelperImpl
        : IPlatformIconHelperImpl
    {
        public IAvBitmapsEnumerable GetIconVariantsForWindow(Window window, bool fallbackToAppIcon, out bool handledFallbackToAppIcon)
        {
            if (window == null)
            {
                handledFallbackToAppIcon = false;
                return Array.Empty<AvBitmap>();
            }

            AvBitmapsList bitmaps = GetFromWindow(window, fallbackToAppIcon, out handledFallbackToAppIcon);
            if (!fallbackToAppIcon)
                return bitmaps;
            else if (handledFallbackToAppIcon)
                return bitmaps;

            AvBitmapsList appBitmaps = GetIconVariantsForAppInternal();
            if (appBitmaps.Any())
            {
                handledFallbackToAppIcon = true;
                bitmaps.AddRange(appBitmaps);
            }

            return bitmaps;
        }

        public IAvBitmapsEnumerable GetIconVariantsForApp()
            => GetIconVariantsForAppInternal();


        AvBitmapsList GetIconVariantsForAppInternal()
        {
            if (!TryGetAppHIcons(out IntPtr hLargeIcon, out IntPtr hSmallIcon))
                return new();


            List<Icon> icons = new();

            Icon smallIcon = Icon.FromHandle(hSmallIcon);
            if (smallIcon != null)
                icons.Add(smallIcon);

            Icon largeIcon = Icon.FromHandle(hLargeIcon);
            if (largeIcon != null)
                icons.Add(largeIcon);

            if (!icons.Any())
                return new();

            return icons
                .Select(SysDrawingHelper.ToAvBitmap)
                .ToList()
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
            isLargeIconValid = SysDrawingHelper.IsIconValid(hLargeIcon);
            isSmallIconValid = SysDrawingHelper.IsIconValid(hSmallIcon);


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




        static AvBitmapsList GetFromWindow(Window window, bool fallbackToAppIcon, out bool handledFallbackToAppIcon)
        {
            AvBitmapsList bitmaps = new();

            IntPtr hWnd = window.GetHWnd();
            double renderScaling = window.RenderScaling;

            if (SysDrawingHelper.TryGetIconViaMessage(hWnd, WindowIconFromMessageParam.SMALL, renderScaling, out Icon smallIcon))
                bitmaps.Add(smallIcon.ToAvBitmap());

            if (SysDrawingHelper.TryGetIconViaMessage(hWnd, WindowIconFromMessageParam.BIG, renderScaling, out Icon bigIcon))
                bitmaps.Add(bigIcon.ToAvBitmap());


            if (!fallbackToAppIcon)
                goto noFallback;

            if (SysDrawingHelper.TryGetIconViaMessage(hWnd, WindowIconFromMessageParam.SMALL2, renderScaling, out Icon smallIcon2))
            {
                handledFallbackToAppIcon = true;
                bitmaps.Add(smallIcon2.ToAvBitmap());
                return bitmaps;
            }

            noFallback:
            handledFallbackToAppIcon = false;
            return bitmaps;
        }
    }
}