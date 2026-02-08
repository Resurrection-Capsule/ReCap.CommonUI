using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.Threading;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI
{
    public enum CaptionButtonsOrder
    {
        MinMaxClose,
        MaxMinClose,
    }


    public enum ManagedChromeMode
    {
        Never = 0,
        Auto,
        WheneverPossible,
    }


    public partial class WindowChromeAddon
        : AvaloniaObject
    {
#region Decorations Customization
        public static readonly AttachedProperty<bool> ManagedShowTitleProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("ManagedShowTitle", true);
        public static bool GetManagedShowTitle(Window control)
            => control.GetValue(ManagedShowTitleProperty);
        public static void SetManagedShowTitle(Window control, bool value)
            => control.SetValue(ManagedShowTitleProperty, value);


        public static readonly AttachedProperty<bool> LeftSideButtonsProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("LeftSideButtons", PlatformPrefersLeftSideButtons);
        public static bool GetLeftSideButtons(Window control)
            => control.GetValue(LeftSideButtonsProperty);
        public static void SetLeftSideButtons(Window control, bool value)
            => control.SetValue(LeftSideButtonsProperty, value);


        public static readonly AttachedProperty<CaptionButtonsOrder> ButtonsOrderProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, CaptionButtonsOrder>("ButtonsOrder", PlatformPreferredCaptionButtonsOrder);
        public static CaptionButtonsOrder GetButtonsOrder(Window control)
            => control.GetValue(ButtonsOrderProperty);
        public static void SetButtonsOrder(Window control, CaptionButtonsOrder value)
            => control.SetValue(ButtonsOrderProperty, value);
#endregion




#region Managed chrome hack
        public static readonly AttachedProperty<bool> EnableHackHintProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("EnableHackHint", false);
        internal static bool GetEnableHackHint(Window control)
            => control.GetValue(EnableHackHintProperty);
        internal static void SetEnableHackHint(Window control, bool value)
            => control.SetValue(EnableHackHintProperty, value);


        public static readonly AttachedProperty<ManagedChromeMode> ManagedChromeHintProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, ManagedChromeMode>("ManagedChromeHint", ManagedChromeMode.Auto);
        public static ManagedChromeMode GetManagedChromeHint(Window control)
            => control.GetValue(ManagedChromeHintProperty);
        internal static void SetManagedChromeHint(Window control, ManagedChromeMode value)
            => control.SetValue(ManagedChromeHintProperty, value);




        internal static readonly AttachedProperty<bool> DesiredManagedChromeProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("DesiredManagedChrome", false);
        internal static bool GetDesiredManagedChrome(Window control)
            => control.GetValue(DesiredManagedChromeProperty);
        internal static void SetDesiredManagedChrome(Window control, bool value)
            => control.SetValue(DesiredManagedChromeProperty, value);




        public static readonly AttachedProperty<bool> IsUsingManagedChromeProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, bool>("IsUsingManagedChrome", false);
        public static bool GetIsUsingManagedChrome(Window control)
            => control.GetValue(IsUsingManagedChromeProperty);
        internal static void SetIsUsingManagedChrome(Window control, bool value)
            => control.SetValue(IsUsingManagedChromeProperty, value);
#endregion



        
        public static bool PlatformCanUseManagedWindowChrome
        {
            get
            {
                if (OSInfo.IsWindows)
                    return true;
                else if (OSInfo.IsLinux)
                    return OSInfo.LinuxIsUsingX11;
                else
                    return false; //[TODO: ?????]
            }
        }


        public static bool PlatformPrefersManagedWindowChrome
        {
            get
            {
                if (!PlatformCanUseManagedWindowChrome)
                    return false;

                if (OSInfo.IsWindows)
                {
                    return true;
                }
                else if (OSInfo.IsLinux)
                {
                    if (int.TryParse(Environment.GetEnvironmentVariable("GTK_CSD"), out int gtkCSD))
                        return gtkCSD == 1;
                    else
                        return OSInfo.LinuxIsUsingGnome;
                }
                else
                {
                    return false; //[TODO: ?????]
                }
            }
        }


        public static bool PlatformPrefersLeftSideButtons
        {
            get
            {
                if (OSInfo.IsWindows)
                    return false;
                else if (OSInfo.IsMacOS)
                    return true;
                else //if (OSInfo.IsLinux)
                {
                    //[TODO: Detect e.g. Unity DE?]
                    return false;
                }
            }
        }


        public static CaptionButtonsOrder PlatformPreferredCaptionButtonsOrder
        {
            get
            {
                if (OSInfo.IsWindows)
                    return CaptionButtonsOrder.MinMaxClose;
                else if (OSInfo.IsMacOS)
                    return CaptionButtonsOrder.MaxMinClose;
                else //if (OSInfo.IsLinux)
                {
                    //[TODO: Detect e.g. Unity DE?]
                    return CaptionButtonsOrder.MinMaxClose;
                }
            }
        }




        static WindowChromeAddon()
        {
            EnableHackHintProperty.Changed.AddClassHandler<Window>(EnableHackHintProperty_Changed);
            ManagedChromeHintProperty.Changed.AddClassHandler<Window>(ManagedChromeHintProperty_Changed);
            DesiredManagedChromeProperty.Changed.AddClassHandler<Window>(DesiredManagedChromeProperty_Changed);

#if DEBUG
            ManagedShowTitleProperty.Changed.AddClassHandler<Window>(WindowChromeCosmeticProperty_Changed);
            LeftSideButtonsProperty.Changed.AddClassHandler<Window>(WindowChromeCosmeticProperty_Changed);
            ButtonsOrderProperty.Changed.AddClassHandler<Window>(WindowChromeCosmeticProperty_Changed);
#endif
        }


        static void WindowChromeCosmeticProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
        {
            Console.WriteLine($"WINDOW '{window.Title}' PROPERTY '{e.Property.Name}' CHANGED:");
            Console.WriteLine($"    '{e.OldValue}' ==> '{e.NewValue}'");
        }


        static void EnableHackHintProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
            => UpdateManagedChrome(window);


        static void ManagedChromeHintProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
            => UpdateManagedChrome(window, e.GetNewValue<ManagedChromeMode>());



        static void UpdateManagedChrome(Window window)
            => UpdateManagedChrome(window, GetManagedChromeHint(window));
        static void UpdateManagedChrome(Window window, ManagedChromeMode chromeMode)
        {
            bool useManagedChrome = chromeMode switch
            {
                ManagedChromeMode.WheneverPossible => PlatformCanUseManagedWindowChrome,
                ManagedChromeMode.Auto => PlatformPrefersManagedWindowChrome,
                _ => false,
            };

            SetDesiredManagedChrome(window, useManagedChrome);
        }


        static void DesiredManagedChromeProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
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
                    SetIsUsingManagedChrome(window, isUsingManagedChrome);

                    if (OSInfo.IsWindows)
                    {
#if NO
                        window.InvalidateMeasure();
                        window.InvalidateArrange();
                        window.InvalidateVisual();
#else
                        Win32WindowChromeUpdateHack.DoHack(window);
#endif
                    }
                });
            });
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
