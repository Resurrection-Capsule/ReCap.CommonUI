using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;

namespace ReCap.CommonUI
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


        public override void OnDesiredManagedChromePropertyChanged(Window window, AvaloniaPropertyChangedEventArgs e)
        {
            base.OnDesiredManagedChromePropertyChanged(window, e);

            Dispatcher.UIThread.Post(() => Dispatcher.UIThread.Post(() =>
            {
#if NO
                window.InvalidateMeasure();
                window.InvalidateArrange();
                window.InvalidateVisual();
#else
                Win32WindowChromeUpdateHack.DoHack(window);
#endif
            }));
        }


        /// <summary>
        /// Workaround for improper positioning of window visual after change.
        /// </summary>
        /// <remarks>
        /// Possible Avalonia bug?
        /// </remarks>
        static class Win32WindowChromeUpdateHack
        {
            internal static void DoHack(Window window)
            {
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
}