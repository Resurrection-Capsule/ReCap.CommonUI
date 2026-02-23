using System;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed class LinuxWindowChromeAddonImpl
        : WindowChromeAddonImplBase
    {
        public override bool CanUseManagedWindowChrome
        {
            get
            {
                if (OSInfo.LinuxIsUsingX11)
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
                    return OSInfo.LinuxIsUsingGnome;
            }
        }


        public override bool PrefersLeftSideButtons
        {
            get
            {
                //[TODO: Detect e.g. Unity DE?]
                return false;
            }
        }


        public override CaptionButtonsOrder PreferredCaptionButtonsOrder
        {
            get
            {
                //[TODO: Detect e.g. Unity DE?]
                return CaptionButtonsOrder.MinMaxClose;
            }
        }
    }
}