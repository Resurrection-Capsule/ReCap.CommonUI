using System;
using System.Collections.Generic;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed class MacOSWindowChromeFactoryImpl
        : WindowChromeFactoryImplBase
    {
        public override bool DefaultShowCaptionIcon
        {
            get => false;
        }


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
            => new(
                new[]
                {
                    CaptionButtonRole.Close,
                    CaptionButtonRole.Minimize,
                    CaptionButtonRole.Maximize,
                },
                Array.Empty<CaptionButtonRole>()
            );



        public override IWindowChromeImpl CreateImpl(WindowChrome chrome)
            => new MacOSWindowChromeImpl(chrome);
    }
}