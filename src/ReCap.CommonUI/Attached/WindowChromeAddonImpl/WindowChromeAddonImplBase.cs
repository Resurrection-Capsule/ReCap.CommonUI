using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;

namespace ReCap.CommonUI
{
    internal abstract class WindowChromeAddonImplBase
        : IWindowChromeAddonImpl
    {
        public abstract bool CanUseManagedWindowChrome
        {
            get;
        }


        public abstract bool PrefersManagedWindowChrome
        {
            get;
        }


        public abstract bool PrefersLeftSideButtons
        {
            get;
        }


        public abstract CaptionButtonsOrder PreferredCaptionButtonsOrder
        {
            get;
        }


        public virtual void Init()
        {
        }


        public virtual void OnDesiredManagedChromePropertyChanged(Window window, AvaloniaPropertyChangedEventArgs e)
        {
            bool newValue = e.GetNewValue<bool>();

            bool oldIsExtendedIntoWindowDecorations = window.IsExtendedIntoWindowDecorations;
            window.ExtendClientAreaToDecorationsHint = newValue;
            Dispatcher.UIThread.Post(() =>
            {
                if (newValue && !window.IsExtendedIntoWindowDecorations)
                    window.SystemDecorations = SystemDecorations.None;
                else if ((!newValue) && !oldIsExtendedIntoWindowDecorations)
                    window.SystemDecorations = SystemDecorations.Full;


                Dispatcher.UIThread.Post(() =>
                {
                    bool isUsingManagedChrome = window.IsExtendedIntoWindowDecorations || newValue;
                    WindowChromeAddon.SetIsUsingManagedChrome(window, isUsingManagedChrome);
                });
            });
        }
    }
}