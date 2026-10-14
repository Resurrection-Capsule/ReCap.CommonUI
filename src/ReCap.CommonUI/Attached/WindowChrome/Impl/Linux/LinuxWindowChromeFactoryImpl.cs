using System;
using System.Collections.Generic;
using System.Linq;
using ReCap.CommonUI.Util.OperatingSystem;
using ReCap.CommonUI.Util.OperatingSystem.Linux;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed partial class LinuxWindowChromeFactoryImpl
        : WindowChromeFactoryImplBase
    {
        static readonly LinuxDetails DETAILS = OSInfo.GetOSDetails<LinuxDetails>();




#region Properties
        public override bool CanUseManagedWindowChrome
        {
            get => true;
        }


        public override bool PrefersManagedWindowChrome
        {
            get => DETAILS.GTKClientSideDecorations;
        }
#endregion




        protected override IEnumerable<CaptionButtonRole> InitValidButtonRoles()
            => Enum.GetValues(typeof(CaptionButtonRole))
                .Cast<CaptionButtonRole>()
            ;
        protected override CaptionButtonRolesPair InitDefaultButtonRoles()
        {
            if (TryImportCaptionButtons(out CaptionButtonRolesPair imported))
                return imported;

            return new(
                //[TODO: Detect e.g. Unity DE?]
                //[TODO: platform-level user settings?]
                new[]
                {
                    CaptionButtonRole.WindowMenu,
                },
                new[]
                {
                    CaptionButtonRole.Minimize,
                    CaptionButtonRole.Maximize,
                    CaptionButtonRole.Close,
                }
            );
        }




        public LinuxWindowChromeFactoryImpl()
            : base()
        {}




        public override IWindowChromeImpl CreateImpl(WindowChrome chrome)
            => new LinuxWindowChromeImpl(chrome);
    }
}