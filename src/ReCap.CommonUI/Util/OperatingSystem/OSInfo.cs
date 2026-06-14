using System;

namespace ReCap.CommonUI.Util.OperatingSystem
{
    public enum OSType
        : byte
    {
        Windows = 0,
        MacOS = 1,
        Linux = 2,
    }




    public static class OSInfo
    {
        public static readonly OSType CurrentOS = PlatformUtils.GetCurrentOSType(out IsWindows, out IsLinux, out IsMacOS);


        public static readonly bool IsWindows;
        public static readonly bool IsLinux;
        public static readonly bool IsMacOS;




        public static Version Version
        {
            get => _IMPL.Version;
        }


        public static bool IsVersionDefinitelyAccurate
        {
            get => _IMPL.IsVersionDefinitelyAccurate;
        }




        static readonly PlatformDetailsBase _IMPL
            = PlatformUtils.GetForPlatformByCreateFunc<PlatformDetailsBase>(
                () => new Win32.Win32Details(),
                () => new Linux.LinuxDetails(),
                () => new MacOS.MacOSDetails()
            );




        public static T GetOSDetails<T>()
            where T
                : class
            => TryGetOSDetails(out T details)
                ? details
                : throw new InvalidCastException($"Cannot assign details of type '{_IMPL.GetType().Name}' to requested type '{typeof(T).FullName}'.")
            ;


        public static bool TryGetOSDetails<T>(out T details)
            where T
                : class
        {
            if (_IMPL is T osDetails)
            {
                details = osDetails;
                return details != null;
            }
            else
            {
                details = default;
                return false;
            }
        }
    }
}