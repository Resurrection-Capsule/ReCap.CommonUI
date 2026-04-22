using System;
using Avalonia.Controls;

using AvBitmap = Avalonia.Media.Imaging.Bitmap;
using IAvBitmapsEnumerable = System.Collections.Generic.IEnumerable<Avalonia.Media.Imaging.Bitmap>;

namespace ReCap.CommonUI.Util
{
    internal interface IPlatformIconHelperImpl
    {
        IAvBitmapsEnumerable GetIconVariantsForWindow(Window window, bool fallbackToAppIcon, out bool handledFallbackToAppIcon);
        //IAvBitmapsEnumerable GetIconVariantsForWindowIcon(WindowIcon windowIcon);
        IAvBitmapsEnumerable GetIconVariantsForApp();
    }




    internal abstract class NoOpPlatformIconHelperImpl
        : IPlatformIconHelperImpl
    {
        public IAvBitmapsEnumerable GetIconVariantsForWindow(Window window, bool fallbackToAppIcon, out bool handledFallbackToAppIcon)
            => throw new NotImplementedException();
        /*
        {
            handledFallbackToAppIcon = false;
            return Array.Empty<AvBitmap>();
        }
        */


        public IAvBitmapsEnumerable GetIconVariantsForWindowIcon(WindowIcon windowIcon)
            => throw new NotImplementedException();
            //=> Array.Empty<AvBitmap>();


        public IAvBitmapsEnumerable GetIconVariantsForApp()
            => throw new NotImplementedException();
            //=> Array.Empty<AvBitmap>();
    }
}