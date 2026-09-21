using System;
using ReCap.CommonUI.Util.OperatingSystem.Linux;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal abstract class LinuxWindowChromeSubImplBase
        : WindowChromeImplBaseBase
    {
        readonly LinuxDetails _details;
        protected LinuxDetails Details
        {
            get => _details;
            private init => _details = value;
        }




        public LinuxWindowChromeSubImplBase(LinuxDetails details)
        {
            Details = details;
        }




        public sealed override CaptionButtonRolesPair DefaultCaptionButtonRoles
        {
            get => throw new NotSupportedException($"Use {nameof(LinuxWindowChromeImpl)}.{nameof(LinuxWindowChromeImpl.DefaultCaptionButtonRoles)} instead");
        }
    }
}