using System.Collections.Generic;
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


        protected override IEnumerable<CaptionButtonRole> GetValidCaptionButtonRoles()
            => new List<CaptionButtonRole>()
            {
                CaptionButtonRole.Minimize,
                CaptionButtonRole.Maximize,
                CaptionButtonRole.FullScreen,
                CaptionButtonRole.Close,
                CaptionButtonRole.Menu,
            };


        protected override CaptionButtonRolesPair CreateDefaultCaptionButtons()
            => new()
            {
                Left = new()
                {
                    CaptionButtonRole.Close,
                    CaptionButtonRole.Minimize,
                    CaptionButtonRole.Maximize,
                    CaptionButtonRole.Menu,
                },
                Right = new()
                {
                },
            };


        public override void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, ref bool useManagedChrome)
        {
            window.ExtendClientAreaToDecorationsHint = desiredManagedChrome;
            DefaultWindowChromeAddonImpl.ApplyDesiredManagedChrome_Default(
                this, window, desiredManagedChrome, ref useManagedChrome
                , fallbackToSystemDecorationsProperty: true
            );

            window.ExtendClientAreaChromeHints = useManagedChrome
                ? ExtendClientAreaChromeHints.NoChrome
                : ExtendClientAreaChromeHints.Default
            ;
        }
    }
}