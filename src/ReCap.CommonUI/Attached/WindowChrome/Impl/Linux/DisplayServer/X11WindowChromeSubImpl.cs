using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using ReCap.CommonUI.Util.OperatingSystem.Linux;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed class X11WindowChromeSubImpl
        : LinuxWindowChromeSubImplBase
    {
#region Properties
        public override bool CanUseManagedWindowChrome
        {
            get => true;
        }


        public override bool PrefersManagedWindowChrome
        {
            get => Details.GTKClientSideDecorations;
        }


        readonly IEnumerable<CaptionButtonRole> _validCaptionButtonRoles
            = Enum.GetValues(typeof(CaptionButtonRole))
                .Cast<CaptionButtonRole>()
                .ToArray()
            ;
        public override IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get => _validCaptionButtonRoles;
        }
#endregion





        public X11WindowChromeSubImpl(LinuxDetails details)
            : base(details)
        {}




        public override void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, Action<bool> applyUseManagedChrome)
        {
#if !WAYLAND
            if (Details.CurrentDisplayServer == KnownDisplayServer.Wayland)
            {
                WaylandWindowChromeSubImpl.ApplyDesiredManagedChrome(
                    window, desiredManagedChrome
                    , fallbackToSystemDecorationsProperty: true
                    , applyUseManagedChrome
                );
                return;
            }
#endif
            DEFAULT_IWindowChromeImpl.ApplyDesiredManagedChrome_IMPL(
                this, window, desiredManagedChrome
                , fallbackToSystemDecorationsProperty: true
                , applyUseManagedChrome
            );
        }
    }
}