using System;
using AVHTV = Avalonia.Controls.Win32Properties.Win32HitTestValue;

namespace ReCap.CommonUI.Util.Win32
{
    internal enum WindowMessage
        : uint
    {
        STYLECHANGING = 0x007C,
        GETICON = 0x007F,
        NCCALCSIZE = 0x0083,
        NCHITTEST = 0x0084,
        NCPAINT = 0x0085,
        NCACTIVATE = 0x0086,
        NCMOUSEMOVE = 0x00A0,
        NCLBUTTONDOWN = 0x00A1,
        NCLBUTTONUP = 0x00A2,
        NCLBUTTONDBLCLK = 0x00A3,
        NCRBUTTONDOWN = 0x00A4,
        NCRBUTTONUP = 0x00A5,
        NCRBUTTONDBLCLK = 0x00A6,
        NCMBUTTONDOWN = 0x00A7,
        NCMBUTTONUP = 0x00A8,
        NCMBUTTONDBLCLK = 0x00A9,
        NCXBUTTONDOWN = 0x00AB,
        NCXBUTTONUP = 0x00AC,
        NCXBUTTONDBLCLK = 0x00AD,
        SYSCOMMAND = 0x0112,
        INITMENU = 0x0116,
        MOUSEMOVE = 0x0200,
        LBUTTONDOWN = 0x0201,
        LBUTTONUP = 0x0202,
    }


    [Flags]
    internal enum WindowStyles
        : uint
    {
        OVERLAPPED = 0x00000000,
        POPUP = 0x80000000,
        CHILD = 0x40000000,
        MINIMIZE = 0x20000000,
        VISIBLE = 0x10000000,
        DISABLED = 0x08000000,
        CLIPSIBLINGS = 0x04000000,
        CLIPCHILDREN = 0x02000000,
        MAXIMIZE = 0x01000000,
        BORDER = 0x00800000,
        DLGFRAME = 0x00400000,
        VSCROLL = 0x00200000,
        HSCROLL = 0x00100000,
        SYSMENU = 0x00080000,
        THICKFRAME = 0x00040000,
        GROUP = 0x00020000,
        TABSTOP = 0x00010000,
    
        MINIMIZEBOX = 0x00020000,
        MAXIMIZEBOX = 0x00010000,
    
#if WS__PRESETS
        CAPTION = BORDER | DLGFRAME,
#endif
    
        TILED = OVERLAPPED,
        ICONIC = MINIMIZE,
        SIZEBOX = THICKFRAME,
    
#if WS__PRESETS
        OVERLAPPEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX,
        POPUPWINDOW = POPUP | BORDER | SYSMENU,
        TILEDWINDOW = OVERLAPPEDWINDOW,
#endif
    
        CHILDWINDOW = CHILD,
    }


    [Flags]
    internal enum ExtendedWindowStyles
        : uint
    {
        /// <summary>
        /// Specifies a window that accepts drag-drop files.
        /// </summary>
        ACCEPTFILES = 0x00000010,

        /// <summary>
        /// Forces a top-level window onto the taskbar when the window is visible.
        /// </summary>
        APPWINDOW = 0x00040000,

        /// <summary>
        /// Specifies a window that has a border with a sunken edge.
        /// </summary>
        CLIENTEDGE = 0x00000200,

        /// <summary>
        /// Specifies a window that paints all descendants in bottom-to-top painting order using double-buffering.
        /// This cannot be used if the window has a class style of either CS_OWNDC or CS_CLASSDC. This style is not supported in Windows 2000.
        /// </summary>
        /// <remarks>
        /// With <see cref="COMPOSITED"/> set, all descendants of a window get bottom-to-top painting order using double-buffering.
        /// Bottom-to-top painting order allows a descendent window to have translucency (alpha) and transparency (color-key) effects,
        /// but only if the descendent window also has the <see cref="TRANSPARENT"/> bit set.
        /// Double-buffering allows the window and its descendents to be painted without flicker.
        /// </remarks>
        COMPOSITED = 0x02000000,

        /// <summary>
        /// Specifies a window that includes a question mark in the title bar. When the user clicks the question mark,
        /// the cursor changes to a question mark with a pointer. If the user then clicks a child window, the child receives a <see cref="WindowMessage.HELP"/> message.
        /// The child window should pass the message to the parent window procedure, which should call the WinHelp function using the <see cref="WinHelpCommand.WM_HELP"/> command.
        /// The Help application displays a pop-up window that typically contains help for the child window.
        /// <see cref="CONTEXTHELP"/> cannot be used with the <see cref="WindowStyles.MAXIMIZEBOX"/> or <see cref="WindowStyles.MINIMIZEBOX"/>  styles.-
        /// </summary>
        CONTEXTHELP = 0x00000400,

        /// <summary>
        /// Specifies a window which contains child windows that should take part in dialog box navigation.
        /// If this style is specified, the dialog manager recurses into children of this window when performing navigation operations
        /// such as handling the TAB key, an arrow key, or a keyboard mnemonic.
        /// </summary>
        CONTROLPARENT = 0x00010000,

        /// <summary>
        /// Specifies a window that has a double border.
        /// </summary>
        DLGMODALFRAME = 0x00000001,

        /// <summary>
        /// Specifies a window that is a layered window.
        /// This cannot be used for child windows or if the window has a class style of either CS_OWNDC or CS_CLASSDC.
        /// </summary>
        LAYERED = 0x00080000,

        /// <summary>
        /// Specifies a window with the horizontal origin on the right edge. Increasing horizontal values advance to the left.
        /// The shell language must support reading-order alignment for this to take effect.
        /// </summary>
        LAYOUTRTL = 0x00400000,

        /// <summary>
        /// Specifies a window that has generic left-aligned properties. This is the default.
        /// </summary>
        LEFT = 0x00000000,

        /// <summary>
        /// Specifies a window with the vertical scroll bar (if present) to the left of the client area.
        /// The shell language must support reading-order alignment for this to take effect.
        /// </summary>
        LEFTSCROLLBAR = 0x00004000,

        /// <summary>
        /// Specifies a window that displays text using left-to-right reading-order properties. This is the default.
        /// </summary>
        LTRREADING = 0x00000000,

        /// <summary>
        /// Specifies a multiple-document interface (MDI) child window.
        /// </summary>
        MDICHILD = 0x00000040,

        /// <summary>
        /// Specifies a top-level window created with this style does not become the foreground window when the user clicks it.
        /// The system does not bring this window to the foreground when the user minimizes or closes the foreground window.
        /// The window does not appear on the taskbar by default. To force the window to appear on the taskbar, use the <see cref="APPWINDOW"/> style.
        /// To activate the window, use the SetActiveWindow or SetForegroundWindow function.
        /// </summary>
        NOACTIVATE = 0x08000000,

        /// <summary>
        /// Specifies a window which does not pass its window layout to its child windows.
        /// </summary>
        NOINHERITLAYOUT = 0x00100000,

        /// <summary>
        /// Specifies that a child window created with this style does not send the WM_PARENTNOTIFY message to its parent window when it is created or destroyed.
        /// </summary>
        NOPARENTNOTIFY = 0x00000004,

#if WS_EX__PRESETS
        /// <summary>
        /// Specifies an overlapped window.
        /// </summary>
        OVERLAPPEDWINDOW = WINDOWEDGE | CLIENTEDGE,

        /// <summary>
        /// Specifies a palette window, which is a modeless dialog box that presents an array of commands.
        /// </summary>
        PALETTEWINDOW = WINDOWEDGE | TOOLWINDOW | TOPMOST,
#endif

        /// <summary>
        /// Specifies a window that has generic "right-aligned" properties. This depends on the window class.
        /// The shell language must support reading-order alignment for this to take effect.
        /// Using the <see cref="RIGHT"/> style has the same effect as using the SS_RIGHT (static), ES_RIGHT (edit), and BS_RIGHT/BS_RIGHTBUTTON (button) control styles.
        /// </summary>
        RIGHT = 0x00001000,

        /// <summary>
        /// Specifies a window with the vertical scroll bar (if present) to the right of the client area. This is the default.
        /// </summary>
        RIGHTSCROLLBAR = 0x00000000,

        /// <summary>
        /// Specifies a window that displays text using right-to-left reading-order properties.
        /// The shell language must support reading-order alignment for this to take effect.
        /// </summary>
        RTLREADING = 0x00002000,

        /// <summary>
        /// Specifies a window with a three-dimensional border style intended to be used for items that do not accept user input.
        /// </summary>
        STATICEDGE = 0x00020000,

        /// <summary>
        /// Specifies a window that is intended to be used as a floating toolbar.
        /// A tool window has a title bar that is shorter than a normal title bar, and the window title is drawn using a smaller font.
        /// A tool window does not appear in the taskbar or in the dialog that appears when the user presses ALT+TAB.
        /// If a tool window has a system menu, its icon is not displayed on the title bar.
        /// However, you can display the system menu by right-clicking or by typing ALT+SPACE.
        /// </summary>
        TOOLWINDOW = 0x00000080,

        /// <summary>
        /// Specifies a window that should be placed above all non-topmost windows and should stay above them, even when the window is deactivated.
        /// To add or remove this style, use the SetWindowPos function.
        /// </summary>
        TOPMOST = 0x00000008,

        /// <summary>
        /// Specifies a window that should not be painted until siblings beneath the window (that were created by the same thread) have been painted.
        /// The window appears transparent because the bits of underlying sibling windows have already been painted.
        /// To achieve transparency without these restrictions, use the SetWindowRgn function.
        /// </summary>
        TRANSPARENT = 0x00000020,

        /// <summary>
        /// Specifies a window that has a border with a raised edge.
        /// </summary>
        WINDOWEDGE = 0x00000100,
    }


    internal enum SystemCommand
        : int
    {
        SIZE = 0xF000,
        MOVE = 0xF010,
        MINIMIZE = 0xF020,
        MAXIMIZE = 0xF030,
        NEXTWINDOW = 0xF040,
        PREVWINDOW = 0xF050,
        CLOSE = 0xF060,
        VSCROLL = 0xF070,
        HSCROLL = 0xF080,
        MOUSEMENU = 0xF090,
        KEYMENU = 0xF100,
        ARRANGE = 0xF110,
        RESTORE = 0xF120,
        TASKLIST = 0xF130,
        SCREENSAVE = 0xF140,
        HOTKEY = 0xF150,


        DEFAULT = 0xF160,
        MONITORPOWER = 0xF170,
        CONTEXTHELP = 0xF180,
        SEPARATOR = 0xF00F,


        SCF_ISSECURE = 0x00000001,


        ICON = MINIMIZE,
        ZOOM = MAXIMIZE,
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


    internal enum WindowLongParam
        : int
    {
        GWL_WNDPROC = -4,
        GWL_HINSTANCE = -6,
        GWL_HWNDPARENT = -8,
        GWL_ID = -12,
        GWL_STYLE = -16,
        GWL_EXSTYLE = -20,
        GWL_USERDATA = -21
    }


    internal enum ClassLongIndex
        : int
    {
        GCLP_MENUNAME = -8,
        GCLP_HBRBACKGROUND = -10,
        GCLP_HCURSOR = -12,
        GCLP_HICON = -14,
        GCLP_HMODULE = -16,
        GCL_CBWNDEXTRA = -18,
        GCL_CBCLSEXTRA = -20,
        GCLP_WNDPROC = -24,
        GCL_STYLE = -26,
        GCLP_HICONSM = -34,
        GCW_ATOM = -32
    }

    /// <summary>
    /// Commands to pass to WinHelp()
    /// </summary>
    internal enum WinHelpCommand
        : uint
    {
        /// <summary>
        /// Display topic in ulTopic
        /// </summary>
        CONTEXT = 0x0001,

        /// <summary>
        /// Terminate help
        /// </summary>
        QUIT = 0x0002,

        /// <summary>
        /// Display index
        /// </summary>
        INDEX = 0x0003,
        CONTENTS = 0x0003,

        /// <summary>
        /// Display help on using help
        /// </summary>
        HELPONHELP = 0x0004,

        /// <summary>
        /// Set current Index for multi index help
        /// </summary>
        SETINDEX = 0x0005,
        SETCONTENTS = 0x0005,
        CONTEXTPOPUP = 0x0008,
        FORCEFILE = 0x0009,

        /// <summary>
        /// Display topic for keyword in offabData
        /// </summary>
        KEY = 0x0101,
        COMMAND = 0x0102,
        PARTIALKEY = 0x0105,
        MULTIKEY = 0x0201,
        SETWINPOS = 0x0203,
        CONTEXTMENU = 0x000a,
        FINDER = 0x000b,
        WM_HELP = 0x000c,
        SETPOPUP_POS = 0x000d,
        TCARD = 0x8000,
        TCARD_DATA = 0x0010,
        TCARD_OTHER_CALLER = 0x0011,
        IDH_NO_HELP = 28440,

        /// <summary>
        /// Control doesn't have matching help context
        /// </summary>
        IDH_MISSING_CONTEXT = 28441,

        /// <summary>
        /// Property sheet help button
        /// </summary>
        IDH_GENERIC_HELP_BUTTON = 28442,
        IDH_OK = 28443,
        IDH_CANCEL = 28444,
        IDH_HELP = 28445,
    }


#if WINDOWS_USE_SETWINDOWCOMPOSITIONATTRIBUTE
    internal enum WindowCompositionAttribute
    {
        WCA_ACCENT_POLICY = 19,
        WCA_USEDARKMODECOLORS = 26
    }


    internal enum AccentState
    {
        ACCENT_DISABLED = 0,
        ACCENT_ENABLE_GRADIENT = 1,
        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
        ACCENT_ENABLE_BLURBEHIND_BUT_ITS_PER_PIXEL_ALPHA_ON_WINDOWS_8 = 3,
        ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
        ACCENT_INVALID_STATE = 5,
        ACCENT_ENABLE_PER_PIXEL_ALPHA_I_GUESS = 6
    }
#endif


    [Flags]
    internal enum DwmWindowAttribute
    {
        NCRenderingEnabled = 1,
        NCRenderingPolicy,
        TransitionsForceDisabled,
        AllowNCPaint,
        CaptionButtonBounds,
        NonClientRtlLayout,
        ForceIconicRepresentation,
        Flip3DPolicy,
        ExtendedFrameBounds,
        HasIconicBitmap,
        DisallowPeek,
        ExcludedFromPeek,
        Last
    }


    [Flags]
    internal enum DwmNCRenderingPolicy
    {
        UseWindowStyle,
        Disabled,
        Enabled,
        Last
    }


    internal enum ShowWindowCommands
        : Int32
    {
        /// <summary>
        /// Hides the window and activates another window.
        /// </summary>
        Hide = 0,
        /// <summary>
        /// Activates and displays a window. If the window is minimized or
        /// maximized, the system restores it to its original size and position.
        /// An application should specify this flag when displaying the window
        /// for the first time.
        /// </summary>
        Normal = 1,
        /// <summary>
        /// Activates the window and displays it as a minimized window.
        /// </summary>
        ShowMinimized = 2,
        /// <summary>
        /// Maximizes the specified window.
        /// </summary>
        Maximize = 3, // is this the right value?
        /// <summary>
        /// Activates the window and displays it as a maximized window.
        /// </summary>      
        ShowMaximized = 3,
        /// <summary>
        /// Displays a window in its most recent size and position. This value
        /// is similar to <see cref="Win32.ShowWindowCommand.Normal"/>, except
        /// the window is not activated.
        /// </summary>
        ShowNoActivate = 4,
        /// <summary>
        /// Activates the window and displays it in its current size and position.
        /// </summary>
        Show = 5,
        /// <summary>
        /// Minimizes the specified window and activates the next top-level
        /// window in the Z order.
        /// </summary>
        Minimize = 6,
        /// <summary>
        /// Displays the window as a minimized window. This value is similar to
        /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the
        /// window is not activated.
        /// </summary>
        ShowMinNoActive = 7,
        /// <summary>
        /// Displays the window in its current size and position. This value is
        /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the
        /// window is not activated.
        /// </summary>
        ShowNA = 8,
        /// <summary>
        /// Activates and displays the window. If the window is minimized or
        /// maximized, the system restores it to its original size and position.
        /// An application should specify this flag when restoring a minimized window.
        /// </summary>
        Restore = 9,
        /// <summary>
        /// Sets the show state based on the SW_* value specified in the
        /// STARTUPINFO structure passed to the CreateProcess function by the
        /// program that started the application.
        /// </summary>
        ShowDefault = 10,
        /// <summary>
        ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread
        /// that owns the window is not responding. This flag should only be
        /// used when minimizing windows from a different thread.
        /// </summary>
        ForceMinimize = 11
    }


    [Flags]
    internal enum TrackPopupMenuFlags
        : uint
    {
        /// <summary>
        /// Positions the shortcut menu so that its left side is aligned with the coordinate specified by the x parameter.
        /// </summary>
        LEFTALIGN = 0x0000,
        /// <summary>
        /// Centers the shortcut menu horizontally relative to the coordinate specified by the x parameter.
        /// </summary>
        CENTERALIGN = 0x0004,
        /// <summary>
        /// Positions the shortcut menu so that its right side is aligned with the coordinate specified by the x parameter. 
        /// </summary>
        RIGHTALIGN = 0x0008,
        /// <summary>
        /// Positions the shortcut menu so that its top side is aligned with the coordinate specified by the y parameter.
        /// </summary>
        TOPALIGN = 0x0000,
        /// <summary>
        /// Centers the shortcut menu vertically relative to the coordinate specified by the y parameter. 
        /// </summary>
        VCENTERALIGN = 0x0010,
        /// <summary>
        /// Positions the shortcut menu so that its bottom side is aligned with the coordinate specified by the y parameter.
        /// </summary>
        BOTTOMALIGN = 0x0020,
        /// <summary>
        /// The function does not send notification messages when the user clicks a menu item.
        /// </summary>
        NONOTIFY = 0x0080,
        /// <summary>
        /// The function returns the menu item identifier of the user's selection in the return value.
        /// </summary>
        RETURNCMD = 0x0100,
        /// <summary>
        /// The user can select menu items with only the left mouse button.
        /// </summary>
        LEFTBUTTON = 0x0000,
        /// <summary>
        /// The user can select menu items with both the left and right mouse buttons. 
        /// </summary>
        RIGHTBUTTON = 0x0002,
    }


    [Flags]
    internal enum MenuItemEnablementFlags
        : uint
    {
	    /// <summary>
        /// Indicates that uIDEnableItem gives the identifier of the menu item. If neither the <see cref="BYCOMMAND"/> nor <see cref="BYPOSITION"/> flag is specified, the <see cref="BYCOMMAND"/> flag is the default flag.
        /// </summary>
        BYCOMMAND = 0x00000000,
        /// <summary>
        /// Indicates that uIDEnableItem gives the zero-based relative position of the menu item.
        /// </summary>
        BYPOSITION = 0x00000400,
        /// <summary>
        /// Indicates that the menu item is disabled, but not grayed, so it cannot be selected.
        /// </summary>
        DISABLED = 0x00000002,
        /// <summary>
        /// Indicates that the menu item is enabled and restored from a grayed state so that it can be selected.
        /// </summary>
        ENABLED = 0x00000000,
        /// <summary>
        /// Indicates that the menu item is disabled and grayed so that it cannot be selected. 
        /// </summary>
        GRAYED = 0x00000001,
    }


    public enum WindowIconFromMessageParam
        : int
    {
        SMALL = 0,
        BIG = 1,
        SMALL2 = 2,
    }
}