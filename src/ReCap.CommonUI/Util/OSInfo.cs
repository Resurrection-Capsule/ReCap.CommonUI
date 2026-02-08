using System;
using System.Runtime.InteropServices;

namespace ReCap.CommonUI.Util
{
    public static class OSInfo
    {
        public static readonly bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        public static readonly bool IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        public static readonly bool IsMacOS = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static readonly Version Version;
        public static readonly bool IsVersionDefinitelyAccurate;


        public static readonly bool LinuxIsUsingX11 = false;
        public static readonly bool LinuxIsUsingGnome = false;
        static OSInfo()
        {
            if (IsWindows)
            {
                if (TryGetWindowsVersion(out Version))
                {
                    IsVersionDefinitelyAccurate = true;
                }
                else
                {
                    Version = Environment.OSVersion.Version;
                    IsVersionDefinitelyAccurate = false;
                }
            }
            else
            {
                Version = Environment.OSVersion.Version;
                IsVersionDefinitelyAccurate = true;

                if (IsLinux)
                {
                    LinuxIsUsingX11 = Environment.GetEnvironmentVariable("XDG_SESSION_TYPE") == "x11";
                    LinuxIsUsingGnome = IsDesktopEnvironment("Gnome"); //[TODO: confirm]
                }
            }
        }



        static bool TryGetWindowsVersion(out Version osVersion)
        {
            osVersion = default;
            try
            {
                if (!SafeRtlGetVersion(out WinUnmanagedMethods.RTL_OSVERSIONINFOEX osVersionInfoEx))
                    return false;

                osVersion = new Version((int)osVersionInfoEx.dwMajorVersion, (int)osVersionInfoEx.dwMinorVersion, (int)osVersionInfoEx.dwBuildNumber);
                return true;
            }
            catch
            {
                return false;
            }
        }


        static bool SafeRtlGetVersion(out WinUnmanagedMethods.RTL_OSVERSIONINFOEX osVersionInfoEx)
        {
            osVersionInfoEx = new WinUnmanagedMethods.RTL_OSVERSIONINFOEX();
            return WinUnmanagedMethods.RtlGetVersion(ref osVersionInfoEx) == 0;
        }


        // https://superuser.com/questions/1074068/what-is-the-difference-between-desktop-session-xdg-session-desktop-and-xdg-cur
        // https://unix.stackexchange.com/questions/116539/how-to-detect-the-desktop-environment-in-a-bash-script/645761#645761
        static bool IsDesktopEnvironment(string desktopEnvironment)
        {
            if (!IsLinux)
                return false;


            string desktopEnv = desktopEnvironment;
            string xdgCurrDesktop = Environment.GetEnvironmentVariable("XDG_CURRENT_DESKTOP");

            //Shortcut: If desktopEnv is empty, check if empty xdgCurrDesktop
            if (string.IsNullOrEmpty(desktopEnv))
            {
                if (string.IsNullOrEmpty(xdgCurrDesktop))
                    return true;
                else
                    return false;
            }

            //Lowercase both
            desktopEnv = desktopEnv.ToLowerInvariant();
            xdgCurrDesktop = xdgCurrDesktop.ToLowerInvariant(); //${de,,}; DEs=${DEs,,}

            //Check de against each DEs component
            //IFS=:; for DE in $DEs; do if [[ "$de" == "$DE" ]]; then return; fi; done
            string[] xdgCurrDesktops = xdgCurrDesktop.Split(':');
            foreach (string xdgDesk in xdgCurrDesktops)
            {
                if (xdgDesk == desktopEnv)
                    return true;
            }

            //Not found
            return false;
        }
    }
}
