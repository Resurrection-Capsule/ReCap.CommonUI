using System;
using Avalonia.Controls;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public enum CaptionButtonRole
    {
        Minimize = WindowState.Minimized,
        Maximize = WindowState.Maximized,
        FullScreen = WindowState.FullScreen,
        Close = 10,
        WindowMenu = 11,
#if CAPTIONBUTTONROLES_NYI
        ApplicationMenu = 12,
        ShowOnAllDesktops = 13,
        ContextHelp = 14,
        Shade = 15,
        KeepBelow = 16,
#endif
        KeepAbove = 17,
    }


    /// <summary>
    /// Defines hints for chrome on a <see cref="Window"/>.
    /// </summary>
    public enum ManagedChromeHint
        : sbyte
    {
        /// <summary>
        /// Use system chrome if possible.
        /// </summary>
        Never            = ManagedChromeElementHint.Hide,
        /// <summary>
        /// Use system or managed chrome, depending on platform preference/convention.
        /// </summary>
        Auto             = ManagedChromeElementHint.Auto,
        /// <summary>
        /// Use managed chrome if possible.
        /// </summary>
        WheneverPossible = ManagedChromeElementHint.Show,
    }


    /// <summary>
    /// Defines hints for visibility of the managed chrome on a <see cref="Window"/>.
    /// </summary>
    public enum ManagedChromeElementHint
        : sbyte
    {
        /// <summary>
        /// Hide the element.
        /// </summary>
        Hide = -1,
        /// <summary>
        /// Defer to platform preference/convention.
        /// </summary>
        Show = 0,
        /// <summary>
        /// Show the element.
        /// </summary>
        Auto = 1,
    }




    internal static class NCHitTestResult
    {
        /// <summary>
        /// In the border of a window that does not have a sizing border.
        /// </summary>
        public const Win32Properties.Win32HitTestValue BORDER = (Win32Properties.Win32HitTestValue)18;
        /// <summary>
        /// In the lower-horizontal border of a resizable window (the user can click the mouse to resize the window vertically).
        /// </summary>
        public const Win32Properties.Win32HitTestValue BOTTOM = Win32Properties.Win32HitTestValue.Bottom;
        /// <summary>
        /// In the lower-left corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).
        /// </summary>
        public const Win32Properties.Win32HitTestValue BOTTOMLEFT = Win32Properties.Win32HitTestValue.BottomLeft;
        /// <summary>
        /// In the lower-right corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).
        /// </summary>
        public const Win32Properties.Win32HitTestValue BOTTOMRIGHT = Win32Properties.Win32HitTestValue.BottomRight;
        /// <summary>
        /// In a title bar.
        /// </summary>
        public const Win32Properties.Win32HitTestValue CAPTION = Win32Properties.Win32HitTestValue.Caption;
        /// <summary>
        /// In a client area.
        /// </summary>
        public const Win32Properties.Win32HitTestValue CLIENT = Win32Properties.Win32HitTestValue.Client;
        /// <summary>
        /// In a Close button.
        /// </summary>
        public const Win32Properties.Win32HitTestValue CLOSE = Win32Properties.Win32HitTestValue.Close;
        /// <summary>
        /// On the screen background or on a dividing line between windows (same as <see cref="NOWHERE"/>, except that the DefWindowProc function produces a system beep to indicate an error).
        /// </summary>
        public const Win32Properties.Win32HitTestValue ERROR = (Win32Properties.Win32HitTestValue)(-2);
        /// <summary>
        /// In a size box (same as <see cref="SIZE"/>).
        /// </summary>
        public const Win32Properties.Win32HitTestValue GROWBOX = (Win32Properties.Win32HitTestValue)4;
        /// <summary>
        /// In a Help button.
        /// </summary>
        public const Win32Properties.Win32HitTestValue HELP = (Win32Properties.Win32HitTestValue)21;
        /// <summary>
        /// In a horizontal scroll bar.
        /// </summary>
        public const Win32Properties.Win32HitTestValue HSCROLL = (Win32Properties.Win32HitTestValue)6;
        /// <summary>
        /// In the left border of a resizable window (the user can click the mouse to resize the window horizontally).
        /// </summary>
        public const Win32Properties.Win32HitTestValue LEFT = Win32Properties.Win32HitTestValue.Left;
        /// <summary>
        /// In a menu.
        /// </summary>
        public const Win32Properties.Win32HitTestValue MENU = (Win32Properties.Win32HitTestValue)5;
        /// <summary>
        /// In a Maximize button.
        /// </summary>
        public const Win32Properties.Win32HitTestValue MAXBUTTON = Win32Properties.Win32HitTestValue.MaxButton;
        /// <summary>
        /// In a Minimize button.
        /// </summary>
        public const Win32Properties.Win32HitTestValue MINBUTTON = Win32Properties.Win32HitTestValue.MinButton;
        /// <summary>
        /// On the screen background or on a dividing line between windows.
        /// </summary>
        public const Win32Properties.Win32HitTestValue NOWHERE = Win32Properties.Win32HitTestValue.Nowhere;
        /// <summary>
        /// In the right border of a resizable window (the user can click the mouse to resize the window horizontally).
        /// </summary>
        public const Win32Properties.Win32HitTestValue RIGHT = Win32Properties.Win32HitTestValue.Right;
        /// <summary>
        /// In a size box (same as <see cref="GROWBOX"/>).
        /// </summary>
        public const Win32Properties.Win32HitTestValue SIZE = (Win32Properties.Win32HitTestValue)4;
        /// <summary>
        /// In a window menu or in a Close button in a child window.
        /// </summary>
        public const Win32Properties.Win32HitTestValue SYSMENU = (Win32Properties.Win32HitTestValue)3;
        /// <summary>
        /// In the upper-horizontal border of a window.
        /// </summary>
        public const Win32Properties.Win32HitTestValue TOP = Win32Properties.Win32HitTestValue.Top;
        /// <summary>
        /// In the upper-left corner of a window border.
        /// </summary>
        public const Win32Properties.Win32HitTestValue TOPLEFT = Win32Properties.Win32HitTestValue.TopLeft;
        /// <summary>
        /// In the upper-right corner of a window border.
        /// </summary>
        public const Win32Properties.Win32HitTestValue TOPRIGHT = Win32Properties.Win32HitTestValue.TopRight;
        /// <summary>
        /// In a window currently covered by another window in the same thread (the message will be sent to underlying windows in the same thread until one of them returns a code that is not <see cref="TRANSPARENT"/>).
        /// </summary>
        public const Win32Properties.Win32HitTestValue TRANSPARENT = (Win32Properties.Win32HitTestValue)(-1);
        /// <summary>
        /// In the vertical scroll bar.
        /// </summary>
        public const Win32Properties.Win32HitTestValue VSCROLL = (Win32Properties.Win32HitTestValue)7;
    }
}