using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ReCap.CommonUI.Util.OperatingSystem.Linux
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct XAnyEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XKeyEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
        public IntPtr root;
        public IntPtr subwindow;
        public IntPtr time;
        public int x;
        public int y;
        public int x_root;
        public int y_root;
        public XModifierMask state;
        public int keycode;
        public int same_screen;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XButtonEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
        public IntPtr root;
        public IntPtr subwindow;
        public IntPtr time;
        public int x;
        public int y;
        public int x_root;
        public int y_root;
        public XModifierMask state;
        public int button;
        public int same_screen;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XMotionEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
        public IntPtr root;
        public IntPtr subwindow;
        public IntPtr time;
        public int x;
        public int y;
        public int x_root;
        public int y_root;
        public XModifierMask state;
        public byte is_hint;
        public int same_screen;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XCrossingEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
        public IntPtr root;
        public IntPtr subwindow;
        public IntPtr time;
        public int x;
        public int y;
        public int x_root;
        public int y_root;
        public NotifyMode mode;
        public NotifyDetail detail;
        public int same_screen;
        public int focus;
        public XModifierMask state;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XFocusChangeEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
        public int mode;
        public NotifyDetail detail;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XKeymapEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
        public byte key_vector0;
        public byte key_vector1;
        public byte key_vector2;
        public byte key_vector3;
        public byte key_vector4;
        public byte key_vector5;
        public byte key_vector6;
        public byte key_vector7;
        public byte key_vector8;
        public byte key_vector9;
        public byte key_vector10;
        public byte key_vector11;
        public byte key_vector12;
        public byte key_vector13;
        public byte key_vector14;
        public byte key_vector15;
        public byte key_vector16;
        public byte key_vector17;
        public byte key_vector18;
        public byte key_vector19;
        public byte key_vector20;
        public byte key_vector21;
        public byte key_vector22;
        public byte key_vector23;
        public byte key_vector24;
        public byte key_vector25;
        public byte key_vector26;
        public byte key_vector27;
        public byte key_vector28;
        public byte key_vector29;
        public byte key_vector30;
        public byte key_vector31;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XExposeEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
        public int x;
        public int y;
        public int width;
        public int height;
        public int count;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XGraphicsExposeEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr drawable;
        public int x;
        public int y;
        public int width;
        public int height;
        public int count;
        public int major_code;
        public int minor_code;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XNoExposeEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr drawable;
        public int major_code;
        public int minor_code;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XVisibilityEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
        public int state;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XCreateWindowEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr parent;
        public IntPtr window;
        public int x;
        public int y;
        public int width;
        public int height;
        public int border_width;
        public int override_redirect;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XDestroyWindowEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr xevent;
        public IntPtr window;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XUnmapEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr xevent;
        public IntPtr window;
        public int from_configure;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XMapEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr xevent;
        public IntPtr window;
        public int override_redirect;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XMapRequestEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr parent;
        public IntPtr window;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XReparentEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr xevent;
        public IntPtr window;
        public IntPtr parent;
        public int x;
        public int y;
        public int override_redirect;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XConfigureEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr xevent;
        public IntPtr window;
        public int x;
        public int y;
        public int width;
        public int height;
        public int border_width;
        public IntPtr above;
        public int override_redirect;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XGravityEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr xevent;
        public IntPtr window;
        public int x;
        public int y;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XResizeRequestEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
        public int width;
        public int height;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XConfigureRequestEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr parent;
        public IntPtr window;
        public int x;
        public int y;
        public int width;
        public int height;
        public int border_width;
        public IntPtr above;
        public int detail;
        public IntPtr value_mask;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XCirculateEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr xevent;
        public IntPtr window;
        public int place;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XCirculateRequestEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr parent;
        public IntPtr window;
        public int place;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XPropertyEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
        public IntPtr atom;
        public IntPtr time;
        public int state;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XSelectionClearEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
        public IntPtr selection;
        public IntPtr time;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XSelectionRequestEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr owner;
        public IntPtr requestor;
        public IntPtr selection;
        public IntPtr target;
        public IntPtr property;
        public IntPtr time;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XSelectionEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr requestor;
        public IntPtr selection;
        public IntPtr target;
        public IntPtr property;
        public IntPtr time;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XColormapEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
        public IntPtr colormap;
        public int c_new;
        public int state;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XClientMessageEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
        public IntPtr message_type;
        public int format;
        public IntPtr ptr1;
        public IntPtr ptr2;
        public IntPtr ptr3;
        public IntPtr ptr4;
        public IntPtr ptr5;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XMappingEvent
    {
        public XEventName type;
        public IntPtr serial;
        public int send_event;
        public IntPtr display;
        public IntPtr window;
        public int request;
        public int first_keycode;
        public int count;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XErrorEvent
    {
        public XEventName type;
        public IntPtr display;
        public IntPtr resourceid;
        public IntPtr serial;
        public byte error_code;
        public XRequest request_code;
        public byte minor_code;
    }


    [StructLayout(LayoutKind.Sequential)]
    internal struct XEventPad
    {
        public IntPtr pad0;
        public IntPtr pad1;
        public IntPtr pad2;
        public IntPtr pad3;
        public IntPtr pad4;
        public IntPtr pad5;
        public IntPtr pad6;
        public IntPtr pad7;
        public IntPtr pad8;
        public IntPtr pad9;
        public IntPtr pad10;
        public IntPtr pad11;
        public IntPtr pad12;
        public IntPtr pad13;
        public IntPtr pad14;
        public IntPtr pad15;
        public IntPtr pad16;
        public IntPtr pad17;
        public IntPtr pad18;
        public IntPtr pad19;
        public IntPtr pad20;
        public IntPtr pad21;
        public IntPtr pad22;
        public IntPtr pad23;
        public IntPtr pad24;
        public IntPtr pad25;
        public IntPtr pad26;
        public IntPtr pad27;
        public IntPtr pad28;
        public IntPtr pad29;
        public IntPtr pad30;
        public IntPtr pad31;
        public IntPtr pad32;
    }




    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct XGenericEventCookie
    {
        /// <summary>
        /// of event. Always GenericEvent
        /// </summary>
        internal int type;
        /// <summary>
        /// # of last request processed
        /// </summary>
        internal IntPtr serial;
        /// <summary>
        /// <see langword="true"/> if from SendEvent request
        /// </summary>
        internal int send_event;
        /// <summary>
        /// Display the event was read from
        /// </summary>
        internal IntPtr display;
        /// <summary>
        /// major opcode of extension that caused the event
        /// </summary>
        internal int extension;
        /// <summary>
        /// actual event type.
        /// </summary>
        internal int evtype;
        internal uint cookie;
        internal void* data;


        public T GetEvent<T>()
            where T
                : unmanaged
        {
            if (data == null)
                throw new InvalidOperationException();
            return Unsafe.ReadUnaligned<T>(data);
        }
    }




    [StructLayout(LayoutKind.Explicit)]
    internal struct XEvent
    {
        [FieldOffset(0)]
        public XEventName type;


        [FieldOffset(0)]
        public XAnyEvent AnyEvent;


        [FieldOffset(0)]
        public XKeyEvent KeyEvent;


        [FieldOffset(0)]
        public XButtonEvent ButtonEvent;


        [FieldOffset(0)]
        public XMotionEvent MotionEvent;


        [FieldOffset(0)]
        public XCrossingEvent CrossingEvent;


        [FieldOffset(0)]
        public XFocusChangeEvent FocusChangeEvent;


        [FieldOffset(0)]
        public XExposeEvent ExposeEvent;


        [FieldOffset(0)]
        public XGraphicsExposeEvent GraphicsExposeEvent;


        [FieldOffset(0)]
        public XNoExposeEvent NoExposeEvent;


        [FieldOffset(0)]
        public XVisibilityEvent VisibilityEvent;


        [FieldOffset(0)]
        public XCreateWindowEvent CreateWindowEvent;


        [FieldOffset(0)]
        public XDestroyWindowEvent DestroyWindowEvent;


        [FieldOffset(0)]
        public XUnmapEvent UnmapEvent;


        [FieldOffset(0)]
        public XMapEvent MapEvent;


        [FieldOffset(0)]
        public XMapRequestEvent MapRequestEvent;


        [FieldOffset(0)]
        public XReparentEvent ReparentEvent;


        [FieldOffset(0)]
        public XConfigureEvent ConfigureEvent;


        [FieldOffset(0)]
        public XGravityEvent GravityEvent;


        [FieldOffset(0)]
        public XResizeRequestEvent ResizeRequestEvent;


        [FieldOffset(0)]
        public XConfigureRequestEvent ConfigureRequestEvent;


        [FieldOffset(0)]
        public XCirculateEvent CirculateEvent;


        [FieldOffset(0)]
        public XCirculateRequestEvent CirculateRequestEvent;


        [FieldOffset(0)]
        public XPropertyEvent PropertyEvent;


        [FieldOffset(0)]
        public XSelectionClearEvent SelectionClearEvent;


        [FieldOffset(0)]
        public XSelectionRequestEvent SelectionRequestEvent;


        [FieldOffset(0)]
        public XSelectionEvent SelectionEvent;


        [FieldOffset(0)]
        public XColormapEvent ColormapEvent;


        [FieldOffset(0)]
        public XClientMessageEvent ClientMessageEvent;


        [FieldOffset(0)]
        public XMappingEvent MappingEvent;


        [FieldOffset(0)]
        public XErrorEvent ErrorEvent;


        [FieldOffset(0)]
        public XKeymapEvent KeymapEvent;


        [FieldOffset(0)]
        public XGenericEventCookie GenericEventCookie;


        [FieldOffset(0)]
        public XEventPad Pad;




        public override string ToString()
            => type switch
            {
                XEventName.ButtonPress or XEventName.ButtonRelease => ToString(ButtonEvent),
                XEventName.CirculateNotify or XEventName.CirculateRequest => ToString(CirculateEvent),
                XEventName.ClientMessage => ToString(ClientMessageEvent),
                XEventName.ColormapNotify => ToString(ColormapEvent),
                XEventName.ConfigureNotify => ToString(ConfigureEvent),
                XEventName.ConfigureRequest => ToString(ConfigureRequestEvent),
                XEventName.CreateNotify => ToString(CreateWindowEvent),
                XEventName.DestroyNotify => ToString(DestroyWindowEvent),
                XEventName.Expose => ToString(ExposeEvent),
                XEventName.FocusIn or XEventName.FocusOut => ToString(FocusChangeEvent),
                XEventName.GraphicsExpose => ToString(GraphicsExposeEvent),
                XEventName.GravityNotify => ToString(GravityEvent),
                XEventName.KeymapNotify => ToString(KeymapEvent),
                XEventName.MapNotify => ToString(MapEvent),
                XEventName.MappingNotify => ToString(MappingEvent),
                XEventName.MapRequest => ToString(MapRequestEvent),
                XEventName.MotionNotify => ToString(MotionEvent),
                XEventName.NoExpose => ToString(NoExposeEvent),
                XEventName.PropertyNotify => ToString(PropertyEvent),
                XEventName.ReparentNotify => ToString(ReparentEvent),
                XEventName.ResizeRequest => ToString(ResizeRequestEvent),
                XEventName.SelectionClear => ToString(SelectionClearEvent),
                XEventName.SelectionNotify => ToString(SelectionEvent),
                XEventName.SelectionRequest => ToString(SelectionRequestEvent),
                XEventName.UnmapNotify => ToString(UnmapEvent),
                XEventName.VisibilityNotify => ToString(VisibilityEvent),
                XEventName.EnterNotify or XEventName.LeaveNotify => ToString(CrossingEvent),
                _ => type.ToString(),
            };


        const BindingFlags _TOSTRING_FIELDS_FLAGS = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
        static string ToString(object ev)
		{
			string result = string.Empty;
			Type type = ev.GetType();
			var fields = type.GetFields(_TOSTRING_FIELDS_FLAGS);
			for (int i = 0; i < fields.Length; i++)
            {
				if (!string.IsNullOrEmpty(result))
					result += ", ";

				var field = fields[i];
                var value = field.GetValue(ev) ?? "<null>";
				result += $"{field.Name}={value}";
			}
			return $"{type.Name} ({result})";
		}
    }




    
    [StructLayout(LayoutKind.Sequential)]
    internal struct MotifWmHints
    {
        public IntPtr flags;
        public IntPtr functions;
        public IntPtr decorations;
        public IntPtr input_mode;
        public IntPtr status;

        /*
        public override string ToString()
            => $"MotifWmHints <flags={(MotifFlags)flags.ToInt32()}, functions={(MotifFunctions)functions.ToInt32()}, decorations={(MotifDecorations)decorations.ToInt32()}, input_mode={(MotifInputMode)input_mode.ToInt32()}, status={status.ToInt32()}";
        */
    }
}