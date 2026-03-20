using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;


namespace ReCap.CommonUI.Util.Win32
{
    internal static partial class Win32Methods
    {
        public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);





        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "GetWindowLong")]
        public static extern uint GetWindowLong32b(IntPtr hWnd, int nIndex);

        public static uint GetWindowLong(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 4)
            {
                return GetWindowLong32b(hWnd, nIndex);
            }
            else
            {
                return GetWindowLongPtr(hWnd, nIndex);
            }
        }

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong32b(IntPtr hWnd, int nIndex, uint value);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLong64b(IntPtr hWnd, int nIndex, IntPtr value);

        public static uint SetWindowLong(IntPtr hWnd, int nIndex, uint value)
        {
            if (IntPtr.Size == 4)
            {
                return SetWindowLong32b(hWnd, nIndex, value);
            }
            else
            {
                return (uint)SetWindowLong64b(hWnd, nIndex, new IntPtr((uint)value)).ToInt32();
            }
        }

        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr handle)
        {
            if (IntPtr.Size == 4)
            {
                return new IntPtr(SetWindowLong32b(hWnd, nIndex, (uint)handle.ToInt32()));
            }
            else
            {
                return SetWindowLong64b(hWnd, nIndex, handle);
            }
        }

        [DllImport("user32.dll", EntryPoint = "DefWindowProcW")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(/*IntPtr lpPrevWndFunc, */IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);
        //(IntPtr hWnd, uint Msg, nuint wParam, StringBuilder lParam);


        [DllImport("user32.dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);


        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);


        public static readonly IntPtr WVR_REDRAW = new IntPtr(0x0300);


        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public const uint WS_CAPTION = 0x00C00000;


        [DllImport("ntdll.dll")]
        internal static extern int RtlGetVersion(ref RTL_OSVERSIONINFOEX lpVersionInformation);


        [DllImport("dwmapi.dll")]
        internal static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);


#if WINDOWS_USE_SETWINDOWCOMPOSITIONATTRIBUTE
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
#endif


        [DllImport("dwmapi.dll", PreserveSig = true)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, Int32 attr, ref Int32 attrValue, Int32 attrSize);




        [DllImport("kernel32.dll")]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocConsole();



        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsZoomed(IntPtr hWnd);




        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, ShowWindowCommands nCmdShow);




        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("user32.dll")]
        public static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, MenuItemEnablementFlags uEnable);

        [DllImport("user32.dll")]
        public static extern int TrackPopupMenu(IntPtr hMenu, TrackPopupMenuFlags uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);




        [DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern uint ExtractIconEx(string szFileName, int nIconIndex, out IntPtr phiconLarge, out IntPtr phiconSmall, uint nIcons);

        [DllImport("user32.dll")]
        public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr hIcon);



        [DllImport("shell32.dll", EntryPoint = "#727")]
        public extern static int SHGetImageList(int iImageList, ref Guid riid, out IImageList ppv);
    }
}