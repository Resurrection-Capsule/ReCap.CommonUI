using System;
using System.Runtime.InteropServices;

namespace ReCap.CommonUI.Util.OperatingSystem.Linux
{
    internal static partial class LibX11
    {
        const string _LIBX11 = "libX11.so.6";




        [DllImport(_LIBX11)]
        public static extern IntPtr XOpenDisplay(IntPtr display);

        [DllImport(_LIBX11)]
        public static extern IntPtr XRootWindow(IntPtr display, int screen_number);

        [DllImport(_LIBX11)]
        public static extern IntPtr XNextEvent(IntPtr display, out XEvent xevent);

        [DllImport(_LIBX11)]
        public static unsafe extern IntPtr XNextEvent(IntPtr display, XEvent* xevent);

        [DllImport(_LIBX11)]
        public static extern int XPending(IntPtr diplay);

        [DllImport(_LIBX11)]
        public static extern int XFlush(IntPtr display);

        [DllImport(_LIBX11)]
        public static extern LibX11ErrorCode XSendEvent(IntPtr display, IntPtr window, bool propagate, IntPtr eventMask, ref XEvent sendEvent);

        [DllImport(_LIBX11)]
        public static extern int XFree(IntPtr data);

        [DllImport(_LIBX11)]
        public static unsafe extern int XFree(void* data);

        [DllImport(_LIBX11)]
        public static extern IntPtr XInternAtom(IntPtr display, string atom_name, bool only_if_exists);

        [DllImport(_LIBX11)]
        public static extern int XInternAtoms(IntPtr display, string[] atom_names, int atom_count, bool only_if_exists, IntPtr[] atoms);

        [DllImport(_LIBX11)]
        public static extern IntPtr XGetAtomName(IntPtr display, IntPtr atom);

        [DllImport(_LIBX11)]
        public static extern int XDefaultScreen(IntPtr display);

        [DllImport(_LIBX11)]
        public static extern int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type,
            int format, PropertyMode mode, ref MotifWmHints data, int nelements);
        [DllImport(_LIBX11)]
        public static extern int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type,
            int format, PropertyMode mode, ref uint value, int nelements);
        [DllImport(_LIBX11)]
        public static extern int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type,
            int format, PropertyMode mode, ref IntPtr value, int nelements);
        [DllImport(_LIBX11)]
        public static extern int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type,
            int format, PropertyMode mode, byte[] data, int nelements);
        [DllImport(_LIBX11)]
        public static extern int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type,
            int format, PropertyMode mode, uint[] data, int nelements);
        [DllImport(_LIBX11)]
        public static extern int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type,
            int format, PropertyMode mode, int[] data, int nelements);
        [DllImport(_LIBX11)]
        public static extern int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type,
            int format, PropertyMode mode, IntPtr[] data, int nelements);
        [DllImport(_LIBX11)]
        public static extern int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, Atom type,
            int format, PropertyMode mode, IntPtr[] data, int nelements);
        [DllImport(_LIBX11)]
        public static unsafe extern int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type,
            int format, PropertyMode mode, void* data, int nelements);
        [DllImport(_LIBX11)]
        public static extern int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type,
            int format, PropertyMode mode, IntPtr atoms, int nelements);

        [DllImport(_LIBX11, CharSet = CharSet.Ansi)]
        public static extern int XChangeProperty(IntPtr display, IntPtr window, IntPtr property, IntPtr type,
            int format, PropertyMode mode, string text, int text_length);

        [DllImport(_LIBX11)]
        public static extern LibX11ErrorCode XGetWindowProperty(IntPtr display, IntPtr window, IntPtr atom
            , IntPtr long_offset, IntPtr long_length, bool delete, IntPtr req_type
            , out IntPtr actual_type
            , out int actual_format
            , out IntPtr nitems
            , out IntPtr bytes_after
            , out IntPtr prop
        );
        [DllImport(_LIBX11)]
        public static extern LibX11ErrorCode XGetWindowProperty(IntPtr display, IntPtr window, IntPtr atom
            , IntPtr long_offset, IntPtr long_length, bool delete, Atom req_type
            , out Atom actual_type
            , out int actual_format
            , out IntPtr nitems
            , out IntPtr bytes_after
            , out IntPtr prop
        );

        [DllImport(_LIBX11)]
        public static extern bool XFilterEvent(ref XEvent xevent, IntPtr window);

#if X11_EVENT_LISTEN
        [DllImport(_LIBX11)]
        public static unsafe extern bool XGetEventData(IntPtr display, void* cookie);
        [DllImport(_LIBX11)]
        public static extern bool XGetEventData(IntPtr display, IntPtr cookie);

        [DllImport(_LIBX11)]
        public static unsafe extern void XFreeEventData(IntPtr display, void* cookie);
        [DllImport(_LIBX11)]
        public static extern void XFreeEventData(IntPtr display, IntPtr cookie);
#endif 
    }
}