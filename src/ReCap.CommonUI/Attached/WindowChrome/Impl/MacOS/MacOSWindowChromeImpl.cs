using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Platform;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed class MacOSWindowChromeImpl
        : WindowChromeImplBase
    {
        public override bool CanUseManagedWindowChrome
        {
            get => true;
        }


        public override bool PrefersManagedWindowChrome
        {
            get => true;
        }


        protected override IEnumerable<CaptionButtonRole> InitValidButtonRoles()
            => new List<CaptionButtonRole>()
            {
                CaptionButtonRole.Minimize,
                CaptionButtonRole.Maximize,
                CaptionButtonRole.FullScreen,
                CaptionButtonRole.Close,
                CaptionButtonRole.WindowMenu,
            };


        protected override CaptionButtonRolesPair InitDefaultButtonRoles()
            => new()
            {
                Left = new()
                {
                    CaptionButtonRole.Close,
                    CaptionButtonRole.Minimize,
                    CaptionButtonRole.Maximize,
                    CaptionButtonRole.WindowMenu,
                },
                Right = new()
                {
                },
            };


        public override void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, Action<bool> applyUseManagedChrome)
        {
            window.ExtendClientAreaToDecorationsHint = desiredManagedChrome;
            bool useManagedChrome = default;
            DEFAULT_IWindowChromeImpl.ApplyDesiredManagedChrome_IMPL(
                this, window, desiredManagedChrome
                , fallbackToSystemDecorationsProperty: true
                , value =>
                {
                    useManagedChrome = value;
                    applyUseManagedChrome(value);
                }
            );

            window.ExtendClientAreaChromeHints = useManagedChrome
                ? ExtendClientAreaChromeHints.NoChrome
                : ExtendClientAreaChromeHints.Default
            ;
        }
    }
}