using System;

namespace ReCap.CommonUI.Util.OperatingSystem.Win32
{
    public sealed class Win32Details
        : PlatformDetailsBase
    {
#region OperatingSystemDetailsBase
        readonly Version _realVersion;
        internal override Version Version
        {
            get => _realVersion;
        }


        readonly bool _isVersionDefinitelyAccurate;
        internal override bool IsVersionDefinitelyAccurate
        {
            get => _isVersionDefinitelyAccurate;
        }



        // [TODO: DWM detection needed?]
#endregion




        internal Win32Details()
            : base()
        {
            if (TryGetRealVersion(out Version version))
            {
                _realVersion = version;
                _isVersionDefinitelyAccurate = true;
            }
            else
            {
                _realVersion = Environment.OSVersion.Version;
                _isVersionDefinitelyAccurate = false;
            }
        }




        internal static bool TryGetRealVersion(out Version osVersion)
        {
            osVersion = default;
            try
            {
                if (!SafeRtlGetVersion(out RTL_OSVERSIONINFOEX osVersionInfoEx))
                    return false;

                osVersion = new Version((int)osVersionInfoEx.dwMajorVersion, (int)osVersionInfoEx.dwMinorVersion, (int)osVersionInfoEx.dwBuildNumber);
                return true;
            }
            catch
            {
                return false;
            }
        }


        static bool SafeRtlGetVersion(out RTL_OSVERSIONINFOEX osVersionInfoEx)
        {
            osVersionInfoEx = new RTL_OSVERSIONINFOEX();
            return NtDll.RtlGetVersion(ref osVersionInfoEx) == 0;
        }
    }
}