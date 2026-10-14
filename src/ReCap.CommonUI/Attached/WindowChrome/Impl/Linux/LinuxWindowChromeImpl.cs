using System;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed partial class LinuxWindowChromeImpl
        : WindowChromeImplBase
    {
        public LinuxWindowChromeImpl(WindowChrome chrome)
            : base(chrome)
        {}


        public override void ApplyDesiredManagedChrome(bool desiredManagedChrome, Action<bool> applyUseManagedChrome)
            => WindowChromeImplHelper.ApplyDesiredManagedChrome(
                this, desiredManagedChrome
                , fallbackToSystemDecorationsProperty: true
                , applyUseManagedChrome
            );
    }
}