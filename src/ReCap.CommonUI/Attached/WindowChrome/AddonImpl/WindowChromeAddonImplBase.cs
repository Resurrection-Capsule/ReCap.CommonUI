using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;

namespace ReCap.CommonUI.Attached.WindowChrome
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




        public virtual bool GetDesiredManagedChrome(Window window, ManagedChromeMode chromeMode)
            => chromeMode switch
            {
                ManagedChromeMode.WheneverPossible => CanUseManagedWindowChrome,
                ManagedChromeMode.Auto => PrefersManagedWindowChrome,
                _ => false,
            };


        public virtual void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, ref bool useManagedChrome)
        {
            bool oldIsExtendedIntoWindowDecorations = window.IsExtendedIntoWindowDecorations;
            bool isUsingManagedChrome = default;


            Dispatcher.UIThread.Invoke(() =>
            {
                if (desiredManagedChrome && !window.IsExtendedIntoWindowDecorations)
                    window.SystemDecorations = SystemDecorations.None;
                else if ((!desiredManagedChrome) && !oldIsExtendedIntoWindowDecorations)
                    window.SystemDecorations = SystemDecorations.Full;


                Dispatcher.UIThread.Invoke(() =>
                {
                    isUsingManagedChrome = window.IsExtendedIntoWindowDecorations || desiredManagedChrome;
                    Debug.WriteLine("1");
                });
            });


            Debug.WriteLine("2");
            useManagedChrome = isUsingManagedChrome;
            Debug.WriteLine("3");
        }
    }
}