using System;
using Avalonia.Controls;
using Avalonia.Threading;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal static class WindowChromeImplHelper
    {
        public static void ApplyDesiredManagedChrome(
            IWindowChromeImpl impl, bool desiredManagedChrome
            , bool fallbackToSystemDecorationsProperty
            , Action<bool> applyUseManagedChrome
        )
        {
            Window window = impl.GetHostWindow();
            bool oldIsExtendedIntoWindowDecorations = window.IsExtendedIntoWindowDecorations;


            Dispatcher.UIThread.Invoke(() =>
            {
                if (fallbackToSystemDecorationsProperty)
                {
                    if (desiredManagedChrome && !window.IsExtendedIntoWindowDecorations)
                        window.SystemDecorations = SystemDecorations.None;
                    else if ((!desiredManagedChrome) && !oldIsExtendedIntoWindowDecorations)
                        window.SystemDecorations = SystemDecorations.Full;
                }


                Dispatcher.UIThread.Invoke(() => applyUseManagedChrome(window.IsExtendedIntoWindowDecorations || desiredManagedChrome));
            });
        }
    }
}