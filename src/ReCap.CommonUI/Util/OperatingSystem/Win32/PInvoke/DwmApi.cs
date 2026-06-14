using System;
using System.Runtime.InteropServices;

namespace ReCap.CommonUI.Util.OperatingSystem.Win32
{
    internal static partial class DwmApi
    {
        const string _DWMAPI = "dwmapi.dll";




        [DllImport(_DWMAPI)]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);
    }
}