using System;
using System.Runtime.InteropServices;

namespace ReCap.CommonUI.Util.OperatingSystem.Win32
{
    internal static partial class NtDll
    {
        const string _NTDLL = "ntdll.dll";




        [DllImport(_NTDLL)]
        public static extern int RtlGetVersion(ref RTL_OSVERSIONINFOEX lpVersionInformation);
    }
}