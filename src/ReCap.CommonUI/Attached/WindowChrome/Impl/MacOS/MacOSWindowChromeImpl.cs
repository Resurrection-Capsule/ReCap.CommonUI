using System;
using Avalonia.Controls;
using Avalonia.Platform;
using ReCap.CommonUI.Controls.AppearanceHacks;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed class MacOSWindowChromeImpl
        : WindowChromeImplBase
    {
        public MacOSWindowChromeImpl(WindowChrome chrome)
            : base(chrome)
        {}




        public override void ApplyDesiredManagedChrome(bool desiredManagedChrome, Action<bool> applyUseManagedChrome)
        {
            Window hostWindow = this.GetHostWindow();
            hostWindow.ExtendClientAreaToDecorationsHint = desiredManagedChrome;
            bool useManagedChrome = default;
            WindowChromeImplHelper.ApplyDesiredManagedChrome(
                this, desiredManagedChrome
                , fallbackToSystemDecorationsProperty: true
                , value =>
                {
                    useManagedChrome = value;
                    applyUseManagedChrome(value);
                }
            );

            hostWindow.ExtendClientAreaChromeHints = useManagedChrome
                ? ExtendClientAreaChromeHints.NoChrome
                : ExtendClientAreaChromeHints.Default
            ;
        }


        public override void Prepare(CaptionButton button)
        {
            base.Prepare(button);
            button.MakeFruity();
        }
    }
}