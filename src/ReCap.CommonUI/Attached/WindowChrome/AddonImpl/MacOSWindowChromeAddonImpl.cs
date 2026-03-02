using Avalonia.Controls;
using Avalonia.Platform;

namespace ReCap.CommonUI.Attached.WindowChrome
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


        public override void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, ref bool useManagedChrome)
        {
            window.ExtendClientAreaToDecorationsHint = desiredManagedChrome;
            base.ApplyDesiredManagedChrome(window, desiredManagedChrome, ref useManagedChrome);

            window.ExtendClientAreaChromeHints = useManagedChrome
                ? ExtendClientAreaChromeHints.NoChrome
                : ExtendClientAreaChromeHints.Default
            ;
        }
    }
}