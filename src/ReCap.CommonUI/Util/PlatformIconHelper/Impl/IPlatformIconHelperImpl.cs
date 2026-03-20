using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Metadata;
using AvBitmap = Avalonia.Media.Imaging.Bitmap;

namespace ReCap.CommonUI.Util
{
    internal interface IPlatformIconHelperImpl
    {
        bool WindowIconUseAppIconAsFallback
        {
            get;
        }


        bool TryGetWindowPlatformIcon(Window window, bool fallbackToAppIcon, out IPlatformIcon icon);
        IPlatformIcon GetAppPlatformIcon();
    }


    internal abstract class NoOpPlatformIconHelperImpl
        : IPlatformIconHelperImpl
    {
        public bool WindowIconUseAppIconAsFallback
        {
            get => false;
        }


        public bool TryGetWindowPlatformIcon(Window window, bool fallbackToAppIcon, out IPlatformIcon icon)
        {
            //[TODO: implement]
            icon = null;
            return false;
        }


        public IPlatformIcon GetAppPlatformIcon()
        {
            //[TODO: implement]
            throw new NotImplementedException();
        }
    }
}