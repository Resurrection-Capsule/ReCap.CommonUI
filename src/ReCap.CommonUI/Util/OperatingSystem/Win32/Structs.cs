using System;
using System.Runtime.InteropServices;

namespace ReCap.CommonUI.Util.OperatingSystem.Win32
{
    internal struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }

        public POINT(IntPtr param)
        {
            var iParam = param.ToInt32();
            X = iParam & 0x0000ffff;
            Y = iParam >> 16;
        }
    }

    internal struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public readonly int Width => right - left;
        public readonly int Height => bottom - top;
        /*public RECT(Rect rect)
        {
            left = (int)rect.X;
            top = (int)rect.Y;
            right = (int)(rect.X + rect.Width);
            bottom = (int)(rect.Y + rect.Height);
        }*/

        public RECT(POINT topLeft, POINT bottomRight)
        {
            left = topLeft.X;
            top = topLeft.Y;
            right = bottomRight.X;
            bottom = bottomRight.Y;
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct WINDOWPOS
    {
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public uint flags;
    }

    
    [StructLayout(LayoutKind.Sequential)]
    internal struct NCCALCSIZE_PARAMS
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public RECT[] rgrc;
        public WINDOWPOS lppos;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct RTL_OSVERSIONINFOEX
    {
        internal uint dwOSVersionInfoSize;
        internal uint dwMajorVersion;
        internal uint dwMinorVersion;
        internal uint dwBuildNumber;
        internal uint dwPlatformId;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        internal string szCSDVersion;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }


#if WINDOWS_USE_SETWINDOWCOMPOSITIONATTRIBUTE
    [StructLayout(LayoutKind.Sequential)]
    internal struct WindowCompositionAttributeData
    {
        public WindowCompositionAttribute Attribute;
        public IntPtr Data;
        public int SizeOfData;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct AccentPolicy
    {
        public AccentState AccentState;
        public int AccentFlags;
        public int GradientColor;
        public int AnimationId;
    }
#endif


    [StructLayout(LayoutKind.Sequential)]
    struct ICONINFO
    {
        /// <summary>
        /// Specifies whether this structure defines an icon or a cursor. A value of <see langword="true"/> specifies an icon; <see langword="false"/> specifies a cursor.
        /// </summary>
        public bool fIcon;

        /// <summary>
        /// Specifies the x-coordinate of a cursor's hot spot.
        /// </summary>
        /// <remarks>
        /// Only applicable to cursors.<br/>
        /// If this structure defines an icon, the hot spot is always in the center of the icon, and this member is ignored.
        /// </remarks>
        public Int32 xHotspot;

        /// <summary>
        /// Specifies the y-coordinate of the cursor's hot spot.
        /// </summary>
        /// <remarks>
        /// Only applicable to cursors.<br/>
        /// If this structure defines an icon, the hot spot is always in the center of the icon, and this member is ignored.
        /// </remarks>
        public Int32 yHotspot;

        /// <summary>
        /// Specifies the icon bitmask bitmap. If this structure defines a black and white icon, this bitmask is formatted so that the upper half is the icon AND bitmask and the lower half is the icon XOR bitmask. Under this condition, the height should be an even multiple of two. If this structure defines a color icon, this mask only defines the AND bitmask of the icon.
        /// </summary>
        /// <remarks>
        /// Only valid for HBITMAPs?
        /// </remarks>
        public IntPtr hbmMask;

        /// <summary>
        /// Handle to the icon color bitmap. This member can be optional if this structure defines a black and white icon. The AND bitmask of hbmMask is applied with the SRCAND flag to the destination; subsequently, the color bitmap is applied (using XOR) to the destination by using the SRCINVERT flag.
        /// </summary>
        /// <remarks>
        /// Only valid for HBITMAPs?
        /// </remarks>
        public IntPtr hbmColor;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal struct IMAGEINFO
    {
        public IntPtr hbmImage;
        public IntPtr hbmMask;
        public int Unused1;
        public int Unused2;
        public System.Drawing.Rectangle rcImage;
    }
}