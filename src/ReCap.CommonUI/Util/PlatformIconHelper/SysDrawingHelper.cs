using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Avalonia.Controls;
using ReCap.CommonUI.Util.Win32;

using AvBitmap = Avalonia.Media.Imaging.Bitmap;

namespace ReCap.CommonUI.Util
{
    internal static class SysDrawingHelper
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
            hIcon = Win32Methods.SendMessage(hWnd, WindowMessage.GETICON, (IntPtr)size, (IntPtr)Helpers.RoundToInt(renderScaling * 96));
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