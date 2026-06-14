using System;
using System.Runtime.InteropServices;

namespace ReCap.CommonUI.Util.OperatingSystem.Win32
{
    internal static partial class User32
    {
        const string _USER32 = "user32.dll";




        [DllImport(_USER32, SetLastError = true)]
        public static extern uint GetWindowLongPtr(IntPtr hWnd, int nIndex);

        [DllImport(_USER32, SetLastError = true, EntryPoint = "GetWindowLong")]
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

        [DllImport(_USER32, SetLastError = true, EntryPoint = "SetWindowLong")]
        private static extern uint SetWindowLong32b(IntPtr hWnd, int nIndex, uint value);

        [DllImport(_USER32, SetLastError = true, EntryPoint = "SetWindowLongPtr")]
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

        [DllImport(_USER32, EntryPoint = "DefWindowProcW")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport(_USER32)]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);


        [DllImport(_USER32, CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(/*IntPtr lpPrevWndFunc, */IntPtr hWnd, WindowMessage Msg, IntPtr wParam, IntPtr lParam);
        //(IntPtr hWnd, uint Msg, nuint wParam, StringBuilder lParam);


        [DllImport(_USER32)]
        public static extern IntPtr PostMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);


        [DllImport(_USER32)]
        public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);


        public static readonly IntPtr WVR_REDRAW = new IntPtr(0x0300);


        [DllImport(_USER32, SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public const uint WS_CAPTION = 0x00C00000;



#if WINDOWS_USE_SETWINDOWCOMPOSITIONATTRIBUTE
        [DllImport(_USER32)]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
#endif



        [DllImport(_USER32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport(_USER32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport(_USER32)]
        public static extern bool IsZoomed(IntPtr hWnd);




        [DllImport(_USER32)]
        public static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

        [DllImport(_USER32)]
        public static extern bool ShowWindowAsync(IntPtr hWnd, ShowWindowCommands nCmdShow);




        [DllImport(_USER32)]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport(_USER32)]
        public static extern bool EnableMenuItem(IntPtr hMenu, uint uIDEnableItem, MenuItemEnablementFlags uEnable);

        [DllImport(_USER32)]
        public static extern int TrackPopupMenu(IntPtr hMenu, TrackPopupMenuFlags uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr prcRect);


        [DllImport(_USER32)]
        public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);

        [DllImport(_USER32)]
        public static extern bool DestroyIcon(IntPtr hIcon);
    }
}