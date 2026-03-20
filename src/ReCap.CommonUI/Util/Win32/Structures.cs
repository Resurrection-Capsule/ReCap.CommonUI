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


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    internal struct WNDCLASSEX
    {
        public int cbSize;
        public int style;
        public Win32Methods.WndProcDelegate lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        public string lpszMenuName;
        public string lpszClassName;
        public IntPtr hIconSm;
    }


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

    [ComImport()]
    [Guid("46EB5926-582E-4017-9FDF-E8998DAA0950")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IImageList
    {
        [PreserveSig]
        int Add(
            IntPtr hbmImage,
            IntPtr hbmMask,
            ref int pi);

        [PreserveSig]
        int ReplaceIcon(
            int i,
            IntPtr hicon,
            ref int pi);

        [PreserveSig]
        int SetOverlayImage(
            int iImage,
            int iOverlay);

        [PreserveSig]
        int Replace(
            int i,
            IntPtr hbmImage,
            IntPtr hbmMask);

        [PreserveSig]
        int AddMasked(
            IntPtr hbmImage,
            int crMask,
            ref int pi);

        [PreserveSig]
        int Draw(
            ref IMAGELISTDRAWPARAMS pimldp);

        [PreserveSig]
        int Remove(
        int i);

        [PreserveSig]
        int GetIcon(
            int i,
            int flags,
            ref IntPtr picon);

        [PreserveSig]
        int GetImageInfo(
            int i,
            ref IMAGEINFO pImageInfo);

        [PreserveSig]
        int Copy(
            int iDst,
            IImageList punkSrc,
            int iSrc,
            int uFlags);

        [PreserveSig]
        int Merge(
            int i1,
            IImageList punk2,
            int i2,
            int dx,
            int dy,
            ref Guid riid,
            ref IntPtr ppv);

        [PreserveSig]
        int Clone(
            ref Guid riid,
            ref IntPtr ppv);

        [PreserveSig]
        int GetImageRect(
            int i,
            ref System.Drawing.Rectangle prc);

        [PreserveSig]
        int GetIconSize(
            ref int cx,
            ref int cy);

        [PreserveSig]
        int SetIconSize(
            int cx,
            int cy);

        [PreserveSig]
        int GetImageCount(
        ref int pi);

        [PreserveSig]
        int SetImageCount(
            int uNewCount);

        [PreserveSig]
        int SetBkColor(
            int clrBk,
            ref int pclr);

        [PreserveSig]
        int GetBkColor(
            ref int pclr);

        [PreserveSig]
        int BeginDrag(
            int iTrack,
            int dxHotspot,
            int dyHotspot);

        [PreserveSig]
        int EndDrag();

        [PreserveSig]
        int DragEnter(
            IntPtr hwndLock,
            int x,
            int y);

        [PreserveSig]
        int DragLeave(
            IntPtr hwndLock);

        [PreserveSig]
        int DragMove(
            int x,
            int y);

        [PreserveSig]
        int SetDragCursorImage(
            ref IImageList punk,
            int iDrag,
            int dxHotspot,
            int dyHotspot);

        [PreserveSig]
        int DragShowNolock(
            int fShow);

        [PreserveSig]
        int GetDragImage(
            ref System.Drawing.Point ppt,
            ref System.Drawing.Point pptHotspot,
            ref Guid riid,
            ref IntPtr ppv);

        [PreserveSig]
        int GetItemFlags(
            int i,
            ref int dwFlags);

        [PreserveSig]
        int GetOverlayImage(
            int iOverlay,
            ref int piIndex);
    };

    [StructLayout(LayoutKind.Sequential)]
    internal struct IMAGEINFO
    {
        public IntPtr hbmImage;
        public IntPtr hbmMask;
        public int Unused1;
        public int Unused2;
        public System.Drawing.Rectangle rcImage;
    }

    internal struct IMAGELISTDRAWPARAMS
    {
        public int cbSize;
        public IntPtr himl;
        public int i;
        public IntPtr hdcDst;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public int xBitmap;        // x offest from the upperleft of bitmap
        public int yBitmap;        // y offset from the upperleft of bitmap
        public int rgbBk;
        public int rgbFg;
        public int fStyle;
        public int dwRop;
        public int fState;
        public int Frame;
        public int crEffect;
    }

}