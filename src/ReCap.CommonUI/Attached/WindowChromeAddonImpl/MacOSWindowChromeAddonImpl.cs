using System;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI
{
    internal sealed class MacOSWindowChromeAddonImpl
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
            get => true;
        }


        public override CaptionButtonsOrder PreferredCaptionButtonsOrder
        {
            get => CaptionButtonsOrder.MaxMinClose;
        }
    }
}