using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Threading;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
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

        bool GetDesiredManagedChrome(Window window, ManagedChromeMode chromeMode);
        void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, ref bool useManagedChrome);
    }
}