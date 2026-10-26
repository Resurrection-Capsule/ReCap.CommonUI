using System;

namespace ReCap.CommonUI.Util.OperatingSystem.Linux
{
    internal partial class X11Atoms
    {
#if NO
        // Our atoms
        public readonly IntPtr AnyPropertyType = (IntPtr)0;
        public readonly IntPtr XA_PRIMARY = (IntPtr)1;
        public readonly IntPtr XA_SECONDARY = (IntPtr)2;
        public readonly IntPtr XA_ARC = (IntPtr)3;
        public readonly IntPtr XA_ATOM = (IntPtr)4;
        public readonly IntPtr XA_BITMAP = (IntPtr)5;
        public readonly IntPtr XA_CARDINAL = (IntPtr)6;
        public readonly IntPtr XA_COLORMAP = (IntPtr)7;
        public readonly IntPtr XA_CURSOR = (IntPtr)8;
        public readonly IntPtr XA_CUT_BUFFER0 = (IntPtr)9;
        public readonly IntPtr XA_CUT_BUFFER1 = (IntPtr)10;
        public readonly IntPtr XA_CUT_BUFFER2 = (IntPtr)11;
        public readonly IntPtr XA_CUT_BUFFER3 = (IntPtr)12;
        public readonly IntPtr XA_CUT_BUFFER4 = (IntPtr)13;
        public readonly IntPtr XA_CUT_BUFFER5 = (IntPtr)14;
        public readonly IntPtr XA_CUT_BUFFER6 = (IntPtr)15;
        public readonly IntPtr XA_CUT_BUFFER7 = (IntPtr)16;
        public readonly IntPtr XA_DRAWABLE = (IntPtr)17;
        public readonly IntPtr XA_FONT = (IntPtr)18;
        public readonly IntPtr XA_INTEGER = (IntPtr)19;
        public readonly IntPtr XA_PIXMAP = (IntPtr)20;
        public readonly IntPtr XA_POINT = (IntPtr)21;
        public readonly IntPtr XA_RECTANGLE = (IntPtr)22;
        public readonly IntPtr XA_RESOURCE_MANAGER = (IntPtr)23;
        public readonly IntPtr XA_RGB_COLOR_MAP = (IntPtr)24;
        public readonly IntPtr XA_RGB_BEST_MAP = (IntPtr)25;
        public readonly IntPtr XA_RGB_BLUE_MAP = (IntPtr)26;
        public readonly IntPtr XA_RGB_DEFAULT_MAP = (IntPtr)27;
        public readonly IntPtr XA_RGB_GRAY_MAP = (IntPtr)28;
        public readonly IntPtr XA_RGB_GREEN_MAP = (IntPtr)29;
        public readonly IntPtr XA_RGB_RED_MAP = (IntPtr)30;
        public readonly IntPtr XA_STRING = (IntPtr)31;
        public readonly IntPtr XA_VISUALID = (IntPtr)32;
        public readonly IntPtr XA_WINDOW = (IntPtr)33;
        public readonly IntPtr XA_WM_COMMAND = (IntPtr)34;
        public readonly IntPtr XA_WM_HINTS = (IntPtr)35;
        public readonly IntPtr XA_WM_CLIENT_MACHINE = (IntPtr)36;
        public readonly IntPtr XA_WM_ICON_NAME = (IntPtr)37;
        public readonly IntPtr XA_WM_ICON_SIZE = (IntPtr)38;
        public readonly IntPtr XA_WM_NAME = (IntPtr)39;
        public readonly IntPtr XA_WM_NORMAL_HINTS = (IntPtr)40;
        public readonly IntPtr XA_WM_SIZE_HINTS = (IntPtr)41;
        public readonly IntPtr XA_WM_ZOOM_HINTS = (IntPtr)42;
        public readonly IntPtr XA_MIN_SPACE = (IntPtr)43;
        public readonly IntPtr XA_NORM_SPACE = (IntPtr)44;
        public readonly IntPtr XA_MAX_SPACE = (IntPtr)45;
        public readonly IntPtr XA_END_SPACE = (IntPtr)46;
        public readonly IntPtr XA_SUPERSCRIPT_X = (IntPtr)47;
        public readonly IntPtr XA_SUPERSCRIPT_Y = (IntPtr)48;
        public readonly IntPtr XA_SUBSCRIPT_X = (IntPtr)49;
        public readonly IntPtr XA_SUBSCRIPT_Y = (IntPtr)50;
        public readonly IntPtr XA_UNDERLINE_POSITION = (IntPtr)51;
        public readonly IntPtr XA_UNDERLINE_THICKNESS = (IntPtr)52;
        public readonly IntPtr XA_STRIKEOUT_ASCENT = (IntPtr)53;
        public readonly IntPtr XA_STRIKEOUT_DESCENT = (IntPtr)54;
        public readonly IntPtr XA_ITALIC_ANGLE = (IntPtr)55;
        public readonly IntPtr XA_X_HEIGHT = (IntPtr)56;
        public readonly IntPtr XA_QUAD_WIDTH = (IntPtr)57;
        public readonly IntPtr XA_WEIGHT = (IntPtr)58;
        public readonly IntPtr XA_POINT_SIZE = (IntPtr)59;
        public readonly IntPtr XA_RESOLUTION = (IntPtr)60;
        public readonly IntPtr XA_COPYRIGHT = (IntPtr)61;
        public readonly IntPtr XA_NOTICE = (IntPtr)62;
        public readonly IntPtr XA_FONT_NAME = (IntPtr)63;
        public readonly IntPtr XA_FAMILY_NAME = (IntPtr)64;
        public readonly IntPtr XA_FULL_NAME = (IntPtr)65;
        public readonly IntPtr XA_CAP_HEIGHT = (IntPtr)66;
        public readonly IntPtr XA_WM_CLASS = (IntPtr)67;
        public readonly IntPtr XA_WM_TRANSIENT_FOR = (IntPtr)68;
#endif


        public readonly IntPtr EDID = default;

        public readonly IntPtr WM_PROTOCOLS = default;
        public readonly IntPtr WM_DELETE_WINDOW = default;
        public readonly IntPtr WM_TAKE_FOCUS = default;
        public readonly IntPtr _NET_SUPPORTED = default;
        public readonly IntPtr _NET_CLIENT_LIST = default;
        public readonly IntPtr _NET_NUMBER_OF_DESKTOPS = default;
        public readonly IntPtr _NET_DESKTOP_GEOMETRY = default;
        public readonly IntPtr _NET_DESKTOP_VIEWPORT = default;
        public readonly IntPtr _NET_CURRENT_DESKTOP = default;
        public readonly IntPtr _NET_DESKTOP_NAMES = default;
        public readonly IntPtr _NET_ACTIVE_WINDOW = default;
        public readonly IntPtr _NET_WORKAREA = default;
        public readonly IntPtr _NET_SUPPORTING_WM_CHECK = default;
        public readonly IntPtr _NET_VIRTUAL_ROOTS = default;
        public readonly IntPtr _NET_DESKTOP_LAYOUT = default;
        public readonly IntPtr _NET_SHOWING_DESKTOP = default;
        public readonly IntPtr _NET_CLOSE_WINDOW = default;
        public readonly IntPtr _NET_MOVERESIZE_WINDOW = default;
        public readonly IntPtr _NET_WM_MOVERESIZE = default;
        public readonly IntPtr _NET_RESTACK_WINDOW = default;
        public readonly IntPtr _NET_REQUEST_FRAME_EXTENTS = default;
        public readonly IntPtr _NET_WM_NAME = default;
        public readonly IntPtr _NET_WM_VISIBLE_NAME = default;
        public readonly IntPtr _NET_WM_ICON_NAME = default;
        public readonly IntPtr _NET_WM_VISIBLE_ICON_NAME = default;
        public readonly IntPtr _NET_WM_DESKTOP = default;
        public readonly IntPtr _NET_WM_WINDOW_TYPE = default;
        public readonly IntPtr _NET_WM_STATE = default;
        public readonly IntPtr _NET_WM_ALLOWED_ACTIONS = default;
        public readonly IntPtr _NET_WM_STRUT = default;
        public readonly IntPtr _NET_WM_STRUT_PARTIAL = default;
        public readonly IntPtr _NET_WM_ICON_GEOMETRY = default;
        public readonly IntPtr _NET_WM_ICON = default;
        public readonly IntPtr _NET_WM_PID = default;
        public readonly IntPtr _NET_WM_HANDLED_ICONS = default;
        public readonly IntPtr _NET_WM_USER_TIME = default;
        public readonly IntPtr _NET_FRAME_EXTENTS = default;
        public readonly IntPtr _NET_WM_PING = default;
        public readonly IntPtr _NET_WM_SYNC_REQUEST = default;
        public readonly IntPtr _NET_WM_SYNC_REQUEST_COUNTER = default;
        public readonly IntPtr _NET_SYSTEM_TRAY_S = default;
        public readonly IntPtr _NET_SYSTEM_TRAY_ORIENTATION = default;
        public readonly IntPtr _NET_SYSTEM_TRAY_OPCODE = default;
        public readonly IntPtr _NET_WM_STATE_MAXIMIZED_HORZ = default;
        public readonly IntPtr _NET_WM_STATE_MAXIMIZED_VERT = default;
        public readonly IntPtr _NET_WM_STATE_FULLSCREEN = default;
        public readonly IntPtr _XEMBED = default;
        public readonly IntPtr _XEMBED_INFO = default;
        public readonly IntPtr _MOTIF_WM_HINTS = default;
        public readonly IntPtr _NET_WM_STATE_SKIP_TASKBAR = default;
        public readonly IntPtr _NET_WM_STATE_ABOVE = default;
        public readonly IntPtr _NET_WM_STATE_MODAL = default;
        public readonly IntPtr _NET_WM_STATE_HIDDEN = default;
        public readonly IntPtr _NET_WM_CONTEXT_HELP = default;
        public readonly IntPtr _NET_WM_WINDOW_OPACITY = default;
        public readonly IntPtr _NET_WM_WINDOW_TYPE_DESKTOP = default;
        public readonly IntPtr _NET_WM_WINDOW_TYPE_DOCK = default;
        public readonly IntPtr _NET_WM_WINDOW_TYPE_TOOLBAR = default;
        public readonly IntPtr _NET_WM_WINDOW_TYPE_MENU = default;
        public readonly IntPtr _NET_WM_WINDOW_TYPE_UTILITY = default;
        public readonly IntPtr _NET_WM_WINDOW_TYPE_SPLASH = default;
        public readonly IntPtr _NET_WM_WINDOW_TYPE_DIALOG = default;
        public readonly IntPtr _NET_WM_WINDOW_TYPE_NORMAL = default;
        public readonly IntPtr CLIPBOARD = default;
        public readonly IntPtr CLIPBOARD_MANAGER = default;
        public readonly IntPtr SAVE_TARGETS = default;
        public readonly IntPtr MULTIPLE = default;
        public readonly IntPtr PRIMARY = default;
        public readonly IntPtr OEMTEXT = default;
        public readonly IntPtr UNICODETEXT = default;
        public readonly IntPtr TARGETS = default;
        public readonly IntPtr UTF8_STRING = default;
        public readonly IntPtr UTF16_STRING = default;
        public readonly IntPtr ATOM_PAIR = default;
        public readonly IntPtr MANAGER = default;
        public readonly IntPtr _KDE_NET_WM_BLUR_BEHIND_REGION = default;
        public readonly IntPtr INCR = default;
        public readonly IntPtr _NET_WM_STATE_FOCUSED = default;


        // Added
        public readonly IntPtr _NET_WM_STATE_STICKY = default;
        public readonly IntPtr _NET_WM_STATE_SHADED = default;
        public readonly IntPtr _NET_WM_STATE_BELOW = default;
    }
}