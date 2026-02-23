using System;
using Avalonia.Controls;
using Avalonia.Threading;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed class WindowsWindowChromeAddonImpl
        : WindowChromeAddonImplBase
    {
        public override bool CanUseManagedWindowChrome
        {
            get => true;
        }


        public override bool PrefersManagedWindowChrome
        {
            get => true;
        }


        public override bool PrefersLeftSideButtons
        {
            get => false;
        }


        public override CaptionButtonsOrder PreferredCaptionButtonsOrder
        {
            get => CaptionButtonsOrder.MinMaxClose;
        }


        public override void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, ref bool useManagedChrome)
        {
            window.ExtendClientAreaToDecorationsHint = desiredManagedChrome;
            base.ApplyDesiredManagedChrome(window, desiredManagedChrome, ref useManagedChrome);

            Dispatcher.UIThread.Post(() => Dispatcher.UIThread.Post(() => {
                VisualPositionHack(window);
            }));
        }


        /// <summary>
        /// Workaround for improper positioning of window visual after change (possible Avalonia bug?)
        /// </remarks>
        void VisualPositionHack(Window window)
        {
#if NO
            window.InvalidateMeasure();
            window.InvalidateArrange();
            window.InvalidateVisual();
#else
            var winState = window.WindowState;
            Dispatcher.UIThread.Post(() => 
            {
                Action after;
                if ((winState == WindowState.Maximized) || (winState == WindowState.FullScreen))
                    after = Maximized(window, winState);
                else
                    after = Restored(window, winState);

                Dispatcher.UIThread.Post(after);
            });
#endif
        }


        static Action Restored(Window window, WindowState winState)
        {
            Action after;
            double width = window.Width;
            double height = window.Height;

            if (width > window.MinWidth)
            {
                window.Width--;
                after = () => window.Width++;
            }
            else if (width < window.MaxWidth)
            {
                window.Width++;
                after = () => window.Width--;
            }
            else if (height > window.MinHeight)
            {
                window.Height--;
                after = () => window.Height++;
            }
            else if (height < window.MaxHeight)
            {
                window.Height++;
                after = () => window.Height--;
            }
            else
            {
                window.WindowState = WindowState.Maximized;
                after = () => window.WindowState = winState;
            }

            return after;
        }


        static Action Maximized(Window window, WindowState winState)
        {
            window.WindowState = WindowState.Normal;
            return () => window.WindowState = winState;
        }
    }
}