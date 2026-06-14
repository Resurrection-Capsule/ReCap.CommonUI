using System;
using Avalonia.Controls;
using AVHTV = Avalonia.Controls.Win32Properties.Win32HitTestValue;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public enum CaptionButtonClickState
    {
        Pressed,
        Released,
    }


    public enum CaptionButtonsOrder
    {
        MinMaxClose,
        MaxMinClose,
    }


    public enum CaptionButtonRole
    {
        Minimize = WindowState.Minimized,
        Maximize = WindowState.Maximized,
        FullScreen = WindowState.FullScreen,
        Close = 10,
        Menu = 11,
        /*
        ApplicationMenu = 12,
        ShowOnAllDesktops = 13,
        ContextHelp = 14,
        Shade = 15,
        KeepBelow = 16,
        */
        KeepAbove = 17,
    }


    public enum ManagedChromeMode
    {
        Never = 0,
        Auto,
        WheneverPossible,
    }




    public enum NCHitTestResult
    {
        /// <summary>
        /// In the border of a window that does not have a sizing border.
        /// </summary>
        BORDER = 18,
        /// <summary>
        /// In the lower-horizontal border of a resizable window (the user can click the mouse to resize the window vertically).
        /// </summary>
        BOTTOM = AVHTV.Bottom,
        /// <summary>
        /// In the lower-left corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).
        /// </summary>
        BOTTOMLEFT = AVHTV.BottomLeft,
        /// <summary>
        /// In the lower-right corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).
        /// </summary>
        BOTTOMRIGHT = AVHTV.BottomRight,
        /// <summary>
        /// In a title bar.
        /// </summary>
        CAPTION = AVHTV.Caption,
        /// <summary>
        /// In a client area.
        /// </summary>
        CLIENT = AVHTV.Client,
        /// <summary>
        /// In a Close button.
        /// </summary>
        CLOSE = AVHTV.Close,
        /// <summary>
        /// On the screen background or on a dividing line between windows (same as <see cref="NOWHERE"/>, except that the DefWindowProc function produces a system beep to indicate an error).
        /// </summary>
        ERROR = -2,
        /// <summary>
        /// In a size box (same as <see cref="SIZE"/>).
        /// </summary>
        GROWBOX = 4,
        /// <summary>
        /// In a Help button.
        /// </summary>
        HELP = 21,
        /// <summary>
        /// In a horizontal scroll bar.
        /// </summary>
        HSCROLL = 6,
        /// <summary>
        /// In the left border of a resizable window (the user can click the mouse to resize the window horizontally).
        /// </summary>
        LEFT = AVHTV.Left,
        /// <summary>
        /// In a menu.
        /// </summary>
        MENU = 5,
        /// <summary>
        /// In a Maximize button.
        /// </summary>
        MAXBUTTON = AVHTV.MaxButton,
        /// <summary>
        /// In a Minimize button.
        /// </summary>
        MINBUTTON = AVHTV.MinButton,
        /// <summary>
        /// On the screen background or on a dividing line between windows.
        /// </summary>
        NOWHERE = AVHTV.Nowhere,
        /// <summary>
        /// In a Minimize button.
        /// </summary>
        REDUCE = MINBUTTON,
        /// <summary>
        /// In the right border of a resizable window (the user can click the mouse to resize the window horizontally).
        /// </summary>
        RIGHT = AVHTV.Right,
        /// <summary>
        /// In a size box (same as <see cref="GROWBOX"/>).
        /// </summary>
        SIZE = 4,
        /// <summary>
        /// In a window menu or in a Close button in a child window.
        /// </summary>
        SYSMENU = 3,
        /// <summary>
        /// In the upper-horizontal border of a window.
        /// </summary>
        TOP = AVHTV.Top,
        /// <summary>
        /// In the upper-left corner of a window border.
        /// </summary>
        TOPLEFT = AVHTV.TopLeft,
        /// <summary>
        /// In the upper-right corner of a window border.
        /// </summary>
        TOPRIGHT = AVHTV.TopRight,
        /// <summary>
        /// In a window currently covered by another window in the same thread (the message will be sent to underlying windows in the same thread until one of them returns a code that is not <see cref="TRANSPARENT"/>).
        /// </summary>
        TRANSPARENT = -1,
        /// <summary>
        /// In the vertical scroll bar.
        /// </summary>
        VSCROLL = 7,
        /// <summary>
        /// In a Maximize button.
        /// </summary>
        ZOOM = MAXBUTTON,
    }
}