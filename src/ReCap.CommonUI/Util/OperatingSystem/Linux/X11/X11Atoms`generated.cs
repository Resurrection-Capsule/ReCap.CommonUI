using System;

namespace ReCap.CommonUI.Util.OperatingSystem.Linux
{
    partial class X11Atoms
    {
        public X11Atoms(IntPtr display)
        {
            _display = display;
            var atoms = new IntPtr[82];
            var atomNames = new string[]
            {
                nameof(EDID),
                nameof(WM_PROTOCOLS),
                nameof(WM_DELETE_WINDOW),
                nameof(WM_TAKE_FOCUS),
                nameof(_NET_SUPPORTED),
                nameof(_NET_CLIENT_LIST),
                nameof(_NET_NUMBER_OF_DESKTOPS),
                nameof(_NET_DESKTOP_GEOMETRY),
                nameof(_NET_DESKTOP_VIEWPORT),
                nameof(_NET_CURRENT_DESKTOP),
                nameof(_NET_DESKTOP_NAMES),
                nameof(_NET_ACTIVE_WINDOW),
                nameof(_NET_WORKAREA),
                nameof(_NET_SUPPORTING_WM_CHECK),
                nameof(_NET_VIRTUAL_ROOTS),
                nameof(_NET_DESKTOP_LAYOUT),
                nameof(_NET_SHOWING_DESKTOP),
                nameof(_NET_CLOSE_WINDOW),
                nameof(_NET_MOVERESIZE_WINDOW),
                nameof(_NET_WM_MOVERESIZE),
                nameof(_NET_RESTACK_WINDOW),
                nameof(_NET_REQUEST_FRAME_EXTENTS),
                nameof(_NET_WM_NAME),
                nameof(_NET_WM_VISIBLE_NAME),
                nameof(_NET_WM_ICON_NAME),
                nameof(_NET_WM_VISIBLE_ICON_NAME),
                nameof(_NET_WM_DESKTOP),
                nameof(_NET_WM_WINDOW_TYPE),
                nameof(_NET_WM_STATE),
                nameof(_NET_WM_ALLOWED_ACTIONS),
                nameof(_NET_WM_STRUT),
                nameof(_NET_WM_STRUT_PARTIAL),
                nameof(_NET_WM_ICON_GEOMETRY),
                nameof(_NET_WM_ICON),
                nameof(_NET_WM_PID),
                nameof(_NET_WM_HANDLED_ICONS),
                nameof(_NET_WM_USER_TIME),
                nameof(_NET_FRAME_EXTENTS),
                nameof(_NET_WM_PING),
                nameof(_NET_WM_SYNC_REQUEST),
                nameof(_NET_WM_SYNC_REQUEST_COUNTER),
                nameof(_NET_SYSTEM_TRAY_S),
                nameof(_NET_SYSTEM_TRAY_ORIENTATION),
                nameof(_NET_SYSTEM_TRAY_OPCODE),
                nameof(_NET_WM_STATE_MAXIMIZED_HORZ),
                nameof(_NET_WM_STATE_MAXIMIZED_VERT),
                nameof(_NET_WM_STATE_FULLSCREEN),
                nameof(_XEMBED),
                nameof(_XEMBED_INFO),
                nameof(_MOTIF_WM_HINTS),
                nameof(_NET_WM_STATE_SKIP_TASKBAR),
                nameof(_NET_WM_STATE_ABOVE),
                nameof(_NET_WM_STATE_MODAL),
                nameof(_NET_WM_STATE_HIDDEN),
                nameof(_NET_WM_CONTEXT_HELP),
                nameof(_NET_WM_WINDOW_OPACITY),
                nameof(_NET_WM_WINDOW_TYPE_DESKTOP),
                nameof(_NET_WM_WINDOW_TYPE_DOCK),
                nameof(_NET_WM_WINDOW_TYPE_TOOLBAR),
                nameof(_NET_WM_WINDOW_TYPE_MENU),
                nameof(_NET_WM_WINDOW_TYPE_UTILITY),
                nameof(_NET_WM_WINDOW_TYPE_SPLASH),
                nameof(_NET_WM_WINDOW_TYPE_DIALOG),
                nameof(_NET_WM_WINDOW_TYPE_NORMAL),
                nameof(CLIPBOARD),
                nameof(CLIPBOARD_MANAGER),
                nameof(SAVE_TARGETS),
                nameof(MULTIPLE),
                nameof(PRIMARY),
                nameof(OEMTEXT),
                nameof(UNICODETEXT),
                nameof(TARGETS),
                nameof(UTF8_STRING),
                nameof(UTF16_STRING),
                nameof(ATOM_PAIR),
                nameof(MANAGER),
                nameof(_KDE_NET_WM_BLUR_BEHIND_REGION),
                nameof(INCR),
                nameof(_NET_WM_STATE_FOCUSED),
                nameof(_NET_WM_STATE_STICKY),
                nameof(_NET_WM_STATE_SHADED),
                nameof(_NET_WM_STATE_BELOW),
            };
            LibX11.XInternAtoms(display, atomNames, atomNames.Length, true, atoms);
            InitAtom(ref EDID, nameof(EDID), atoms[0]);
            InitAtom(ref WM_PROTOCOLS, nameof(WM_PROTOCOLS), atoms[1]);
            InitAtom(ref WM_DELETE_WINDOW, nameof(WM_DELETE_WINDOW), atoms[2]);
            InitAtom(ref WM_TAKE_FOCUS, nameof(WM_TAKE_FOCUS), atoms[3]);
            InitAtom(ref _NET_SUPPORTED, nameof(_NET_SUPPORTED), atoms[4]);
            InitAtom(ref _NET_CLIENT_LIST, nameof(_NET_CLIENT_LIST), atoms[5]);
            InitAtom(ref _NET_NUMBER_OF_DESKTOPS, nameof(_NET_NUMBER_OF_DESKTOPS), atoms[6]);
            InitAtom(ref _NET_DESKTOP_GEOMETRY, nameof(_NET_DESKTOP_GEOMETRY), atoms[7]);
            InitAtom(ref _NET_DESKTOP_VIEWPORT, nameof(_NET_DESKTOP_VIEWPORT), atoms[8]);
            InitAtom(ref _NET_CURRENT_DESKTOP, nameof(_NET_CURRENT_DESKTOP), atoms[9]);
            InitAtom(ref _NET_DESKTOP_NAMES, nameof(_NET_DESKTOP_NAMES), atoms[10]);
            InitAtom(ref _NET_ACTIVE_WINDOW, nameof(_NET_ACTIVE_WINDOW), atoms[11]);
            InitAtom(ref _NET_WORKAREA, nameof(_NET_WORKAREA), atoms[12]);
            InitAtom(ref _NET_SUPPORTING_WM_CHECK, nameof(_NET_SUPPORTING_WM_CHECK), atoms[13]);
            InitAtom(ref _NET_VIRTUAL_ROOTS, nameof(_NET_VIRTUAL_ROOTS), atoms[14]);
            InitAtom(ref _NET_DESKTOP_LAYOUT, nameof(_NET_DESKTOP_LAYOUT), atoms[15]);
            InitAtom(ref _NET_SHOWING_DESKTOP, nameof(_NET_SHOWING_DESKTOP), atoms[16]);
            InitAtom(ref _NET_CLOSE_WINDOW, nameof(_NET_CLOSE_WINDOW), atoms[17]);
            InitAtom(ref _NET_MOVERESIZE_WINDOW, nameof(_NET_MOVERESIZE_WINDOW), atoms[18]);
            InitAtom(ref _NET_WM_MOVERESIZE, nameof(_NET_WM_MOVERESIZE), atoms[19]);
            InitAtom(ref _NET_RESTACK_WINDOW, nameof(_NET_RESTACK_WINDOW), atoms[20]);
            InitAtom(ref _NET_REQUEST_FRAME_EXTENTS, nameof(_NET_REQUEST_FRAME_EXTENTS), atoms[21]);
            InitAtom(ref _NET_WM_NAME, nameof(_NET_WM_NAME), atoms[22]);
            InitAtom(ref _NET_WM_VISIBLE_NAME, nameof(_NET_WM_VISIBLE_NAME), atoms[23]);
            InitAtom(ref _NET_WM_ICON_NAME, nameof(_NET_WM_ICON_NAME), atoms[24]);
            InitAtom(ref _NET_WM_VISIBLE_ICON_NAME, nameof(_NET_WM_VISIBLE_ICON_NAME), atoms[25]);
            InitAtom(ref _NET_WM_DESKTOP, nameof(_NET_WM_DESKTOP), atoms[26]);
            InitAtom(ref _NET_WM_WINDOW_TYPE, nameof(_NET_WM_WINDOW_TYPE), atoms[27]);
            InitAtom(ref _NET_WM_STATE, nameof(_NET_WM_STATE), atoms[28]);
            InitAtom(ref _NET_WM_ALLOWED_ACTIONS, nameof(_NET_WM_ALLOWED_ACTIONS), atoms[29]);
            InitAtom(ref _NET_WM_STRUT, nameof(_NET_WM_STRUT), atoms[30]);
            InitAtom(ref _NET_WM_STRUT_PARTIAL, nameof(_NET_WM_STRUT_PARTIAL), atoms[31]);
            InitAtom(ref _NET_WM_ICON_GEOMETRY, nameof(_NET_WM_ICON_GEOMETRY), atoms[32]);
            InitAtom(ref _NET_WM_ICON, nameof(_NET_WM_ICON), atoms[33]);
            InitAtom(ref _NET_WM_PID, nameof(_NET_WM_PID), atoms[34]);
            InitAtom(ref _NET_WM_HANDLED_ICONS, nameof(_NET_WM_HANDLED_ICONS), atoms[35]);
            InitAtom(ref _NET_WM_USER_TIME, nameof(_NET_WM_USER_TIME), atoms[36]);
            InitAtom(ref _NET_FRAME_EXTENTS, nameof(_NET_FRAME_EXTENTS), atoms[37]);
            InitAtom(ref _NET_WM_PING, nameof(_NET_WM_PING), atoms[38]);
            InitAtom(ref _NET_WM_SYNC_REQUEST, nameof(_NET_WM_SYNC_REQUEST), atoms[39]);
            InitAtom(ref _NET_WM_SYNC_REQUEST_COUNTER, nameof(_NET_WM_SYNC_REQUEST_COUNTER), atoms[40]);
            InitAtom(ref _NET_SYSTEM_TRAY_S, nameof(_NET_SYSTEM_TRAY_S), atoms[41]);
            InitAtom(ref _NET_SYSTEM_TRAY_ORIENTATION, nameof(_NET_SYSTEM_TRAY_ORIENTATION), atoms[42]);
            InitAtom(ref _NET_SYSTEM_TRAY_OPCODE, nameof(_NET_SYSTEM_TRAY_OPCODE), atoms[43]);
            InitAtom(ref _NET_WM_STATE_MAXIMIZED_HORZ, nameof(_NET_WM_STATE_MAXIMIZED_HORZ), atoms[44]);
            InitAtom(ref _NET_WM_STATE_MAXIMIZED_VERT, nameof(_NET_WM_STATE_MAXIMIZED_VERT), atoms[45]);
            InitAtom(ref _NET_WM_STATE_FULLSCREEN, nameof(_NET_WM_STATE_FULLSCREEN), atoms[46]);
            InitAtom(ref _XEMBED, nameof(_XEMBED), atoms[47]);
            InitAtom(ref _XEMBED_INFO, nameof(_XEMBED_INFO), atoms[48]);
            InitAtom(ref _MOTIF_WM_HINTS, nameof(_MOTIF_WM_HINTS), atoms[49]);
            InitAtom(ref _NET_WM_STATE_SKIP_TASKBAR, nameof(_NET_WM_STATE_SKIP_TASKBAR), atoms[50]);
            InitAtom(ref _NET_WM_STATE_ABOVE, nameof(_NET_WM_STATE_ABOVE), atoms[51]);
            InitAtom(ref _NET_WM_STATE_MODAL, nameof(_NET_WM_STATE_MODAL), atoms[52]);
            InitAtom(ref _NET_WM_STATE_HIDDEN, nameof(_NET_WM_STATE_HIDDEN), atoms[53]);
            InitAtom(ref _NET_WM_CONTEXT_HELP, nameof(_NET_WM_CONTEXT_HELP), atoms[54]);
            InitAtom(ref _NET_WM_WINDOW_OPACITY, nameof(_NET_WM_WINDOW_OPACITY), atoms[55]);
            InitAtom(ref _NET_WM_WINDOW_TYPE_DESKTOP, nameof(_NET_WM_WINDOW_TYPE_DESKTOP), atoms[56]);
            InitAtom(ref _NET_WM_WINDOW_TYPE_DOCK, nameof(_NET_WM_WINDOW_TYPE_DOCK), atoms[57]);
            InitAtom(ref _NET_WM_WINDOW_TYPE_TOOLBAR, nameof(_NET_WM_WINDOW_TYPE_TOOLBAR), atoms[58]);
            InitAtom(ref _NET_WM_WINDOW_TYPE_MENU, nameof(_NET_WM_WINDOW_TYPE_MENU), atoms[59]);
            InitAtom(ref _NET_WM_WINDOW_TYPE_UTILITY, nameof(_NET_WM_WINDOW_TYPE_UTILITY), atoms[60]);
            InitAtom(ref _NET_WM_WINDOW_TYPE_SPLASH, nameof(_NET_WM_WINDOW_TYPE_SPLASH), atoms[61]);
            InitAtom(ref _NET_WM_WINDOW_TYPE_DIALOG, nameof(_NET_WM_WINDOW_TYPE_DIALOG), atoms[62]);
            InitAtom(ref _NET_WM_WINDOW_TYPE_NORMAL, nameof(_NET_WM_WINDOW_TYPE_NORMAL), atoms[63]);
            InitAtom(ref CLIPBOARD, nameof(CLIPBOARD), atoms[64]);
            InitAtom(ref CLIPBOARD_MANAGER, nameof(CLIPBOARD_MANAGER), atoms[65]);
            InitAtom(ref SAVE_TARGETS, nameof(SAVE_TARGETS), atoms[66]);
            InitAtom(ref MULTIPLE, nameof(MULTIPLE), atoms[67]);
            InitAtom(ref PRIMARY, nameof(PRIMARY), atoms[68]);
            InitAtom(ref OEMTEXT, nameof(OEMTEXT), atoms[69]);
            InitAtom(ref UNICODETEXT, nameof(UNICODETEXT), atoms[70]);
            InitAtom(ref TARGETS, nameof(TARGETS), atoms[71]);
            InitAtom(ref UTF8_STRING, nameof(UTF8_STRING), atoms[72]);
            InitAtom(ref UTF16_STRING, nameof(UTF16_STRING), atoms[73]);
            InitAtom(ref ATOM_PAIR, nameof(ATOM_PAIR), atoms[74]);
            InitAtom(ref MANAGER, nameof(MANAGER), atoms[75]);
            InitAtom(ref _KDE_NET_WM_BLUR_BEHIND_REGION, nameof(_KDE_NET_WM_BLUR_BEHIND_REGION), atoms[76]);
            InitAtom(ref INCR, nameof(INCR), atoms[77]);
            InitAtom(ref _NET_WM_STATE_FOCUSED, nameof(_NET_WM_STATE_FOCUSED), atoms[78]);
            InitAtom(ref _NET_WM_STATE_STICKY, nameof(_NET_WM_STATE_STICKY), atoms[79]);
            InitAtom(ref _NET_WM_STATE_SHADED, nameof(_NET_WM_STATE_SHADED), atoms[80]);
            InitAtom(ref _NET_WM_STATE_BELOW, nameof(_NET_WM_STATE_BELOW), atoms[81]);
        }
    }
}
