using System;
using Avalonia.Controls;
using Avalonia.Threading;

namespace ReCap.CommonUI.Attached.WindowChrome
{
#if WAYLAND
    internal sealed partial class WaylandWindowChromeSubImpl
        : LinuxWindowChromeSubImplBase
#else
    internal static class WaylandWindowChromeSubImpl
#endif
    {
#if WAYLAND
#region Properties
        public override bool CanUseManagedWindowChrome
        {
            get
            {
                //[TODO: implement]
                throw new NotImplementedException("HELP WANTED");
            }
        }


        public override bool PrefersManagedWindowChrome
        {
            get
            {
                //[TODO: implement]
                throw new NotImplementedException("HELP WANTED");
            }
        }


        public override IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get
            {
                //[TODO: implement]
                throw new NotImplementedException("HELP WANTED");
            }
        }
#endregion





        public WaylandWindowChromeSubImpl(LinuxDetails details)
            : base(details)
        {}




        public override void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, Action<bool> applyUseManagedChrome)
        {
            System.Diagnostics.Debug.WriteLine($"{nameof(window)}: {window}, {nameof(desiredManagedChrome)}: {desiredManagedChrome}, {nameof(applyUseManagedChrome)}: {applyUseManagedChrome}");
            ApplyDesiredManagedChrome(
                window, desiredManagedChrome
                , fallbackToSystemDecorationsProperty: true
                , applyUseManagedChrome
            );
        }
#endif


        public static void ApplyDesiredManagedChrome(
            Window window, bool desiredManagedChrome
            , bool fallbackToSystemDecorationsProperty
            , Action<bool> applyUseManagedChrome
        )
        {
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


                Dispatcher.UIThread.Invoke(() =>
                {
                    applyUseManagedChrome(window.IsExtendedIntoWindowDecorations || desiredManagedChrome);
                });
            });
        }
    }
}