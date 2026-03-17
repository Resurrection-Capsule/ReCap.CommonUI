using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Threading;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI
{
    internal interface IWindowChromeAddonImpl
    {
        bool CanUseManagedWindowChrome
        {
            get;
        }

        bool PrefersManagedWindowChrome
        {
            get;
        }

        bool PrefersLeftSideButtons
        {
            get;
        }

        CaptionButtonsOrder PreferredCaptionButtonsOrder
        {
            get;
        }


        void Init();
        void OnDesiredManagedChromePropertyChanged(Window window, AvaloniaPropertyChangedEventArgs e);
    }
}