using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed partial class LinuxWindowChromeAddonImpl
        : WindowChromeAddonImplBase
    {
        public override bool CanUseManagedWindowChrome
        {
            get
            {
                if (OSInfo.LinuxInfo.IsUsingX11)
                    return true;
                else // [TODO: Wayland?????]
                    return false;
            }
        }


        public override bool PrefersManagedWindowChrome
        {
            get
            {
                if (int.TryParse(Environment.GetEnvironmentVariable("GTK_CSD"), out int gtkCSD))
                    return gtkCSD == 1;
                else
                    return OSInfo.LinuxInfo.IsUsingGnome;
            }
        }



        protected override IEnumerable<CaptionButtonRole> GetValidCaptionButtonRoles()
            => Enum.GetValues(typeof(CaptionButtonRole))
                .Cast<CaptionButtonRole>()
                .ToArray()
            ;


        protected override CaptionButtonRolesPair CreateDefaultCaptionButtons()
        {
            if (TryImportCaptionButtons(out CaptionButtonRolesPair imported))
                return imported;

            return new()
            {
                //[TODO: Detect e.g. Unity DE?]
                //[TODO: platform-level user settings?]
                Left = new()
                {
                    CaptionButtonRole.Menu,
                },
                Right = new()
                {
                    CaptionButtonRole.Minimize,
                    CaptionButtonRole.Maximize,
                    CaptionButtonRole.Close,
                },
            };
        }


        public override void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, ref bool useManagedChrome)
            => DefaultWindowChromeAddonImpl.ApplyDesiredManagedChrome_Default(
                this, window, desiredManagedChrome, ref useManagedChrome
                , fallbackToSystemDecorationsProperty: true
            );
    }
}