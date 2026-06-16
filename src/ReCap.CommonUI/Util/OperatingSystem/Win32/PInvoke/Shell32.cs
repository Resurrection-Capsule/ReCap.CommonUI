using System;
using System.Runtime.InteropServices;

namespace ReCap.CommonUI.Util.OperatingSystem.Win32
{
    internal static partial class Shell32
    {
        const string _SHELL32 = "shell32.dll";




        [DllImport(_SHELL32, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint ExtractIconEx(string szFileName, int nIconIndex, out IntPtr phiconLarge, out IntPtr phiconSmall, uint nIcons);
    }
}