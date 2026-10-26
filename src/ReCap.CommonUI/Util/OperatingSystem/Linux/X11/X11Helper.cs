using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace ReCap.CommonUI.Util.OperatingSystem.Linux
{
    internal static class X11Helper
    {
        static readonly LinuxDetails _DETAILS = OSInfo.GetOSDetails<LinuxDetails>();
        static readonly X11Details _X11_DETAILS;
        static readonly X11Atoms _ATOMS;
        static X11Helper()
        {
            _X11_DETAILS = _DETAILS.GetDisplayServerDetails<X11Details>();
            _ATOMS = _X11_DETAILS.Atoms;
        }




        public static bool TryXSendEvent(IntPtr window, bool propagate, IntPtr eventMask, ref XEvent sendEvent)
            => TryXSendEvent(_X11_DETAILS.Display, window, propagate, eventMask, ref sendEvent);
        public static bool TryXSendEvent(IntPtr display, IntPtr window, bool propagate, IntPtr eventMask, ref XEvent sendEvent)
            => LibX11.XSendEvent(display, window, propagate, eventMask, ref sendEvent) == LibX11ErrorCode.Success;


        /// <summary>
        /// remove/unset property
        /// </summary>
        static readonly IntPtr _NET_WM_STATE_REMOVE = IntPtr.Zero;
        /// <summary>
        /// add/set property
        /// </summary>
        static readonly IntPtr _NET_WM_STATE_ADD    = new(1);




        static readonly IntPtr _EVENT_MASK = new((int)(EventMask.SubstructureRedirectMask | EventMask.SubstructureNotifyMask));
        public static void SendNetWMMessage(IntPtr window, IntPtr messageType, params IntPtr[] l)
        {
            int length = l.Length;
            if ((length < 1) || (length > 5))
                throw new ArgumentException();

            SendNetWMMessageInternal(window, messageType
                , l[0]
                , length > 1
                    ? l[1]
                    : IntPtr.Zero
                , length > 2
                    ? l[2]
                    : IntPtr.Zero
                , length > 3
                    ? l[3]
                    : IntPtr.Zero
                , length > 4
                    ? l[4]
                    : IntPtr.Zero
            );
        }
        public static bool SendNetWMMessage(IntPtr window, IntPtr messageType
            , IntPtr l0
            , IntPtr? l1 = null
            , IntPtr? l2 = null
            , IntPtr? l3 = null
            , IntPtr? l4 = null
        )
            => SendNetWMMessageInternal(window, messageType, l0, l1 ?? IntPtr.Zero, l2 ?? IntPtr.Zero, l3 ?? IntPtr.Zero, l4 ?? IntPtr.Zero);
        static bool SendNetWMMessageInternal(IntPtr window, IntPtr messageType
            , IntPtr l0
            , IntPtr l1
            , IntPtr l2
            , IntPtr l3
            , IntPtr l4
        )
        {
            XEvent e = new()
            {
                ClientMessageEvent = new()
                {
                    type = XEventName.ClientMessage,
                    send_event = 1,
                    window = window,
                    message_type = messageType,
                    format = 32,
                    ptr1 = l0,
                    ptr2 = l1,
                    ptr3 = l2,
                    ptr4 = l3,
                    ptr5 = l4,
                },
            };
            return TryXSendEvent(_X11_DETAILS.RootWindow, false, _EVENT_MASK, ref e);
        }




        public static string GetAtomName(IntPtr display, IntPtr atom)
        {
            var ptr = LibX11.XGetAtomName(display, atom);
            if (ptr == IntPtr.Zero)
                return null;
            var s = Marshal.PtrToStringAnsi(ptr);
            LibX11.XFree(ptr);
            return s;
        }




        /*
        public static bool TryXGetWindowProperty(IntPtr display, IntPtr window, IntPtr atom, Atom reqType
            , out Atom actualType
            , out int actualFormat
            , out IntPtr nitems
            , out IntPtr bytesAfter
            , out IntPtr prop
        )
        */
        static readonly IntPtr _INTPTR_0x7FFFFFFF = new(0x7FFFFFFF);
        public static bool TryXGetWindowProperty(IntPtr display, IntPtr window, IntPtr atom, Atom reqType
            , out Atom actualType
            , out int actualFormat
            , out IntPtr nitems
            , out IntPtr bytesAfter
            , out IntPtr prop
        )
            => TryXGetWindowProperty(display, window, atom
                , IntPtr.Zero, _INTPTR_0x7FFFFFFF, false, reqType
                , out actualType
                , out actualFormat
                , out nitems
                , out bytesAfter
                , out prop
            );
        public static bool TryXGetWindowProperty(IntPtr display, IntPtr window, IntPtr atom
            , IntPtr longOffset, IntPtr longLength, bool delete, Atom reqType
            , out Atom actualType
            , out int actualFormat
            , out IntPtr nitems
            , out IntPtr bytesAfter
            , out IntPtr prop
        )
            => LibX11.XGetWindowProperty(display, window, atom
                , longOffset, longLength, delete, reqType
                , out actualType
                , out actualFormat
                , out nitems
                , out bytesAfter
                , out prop
            ) == LibX11ErrorCode.Success;
        public static IntPtr[] GetWindowPropertyArray(IntPtr window)
            => GetWindowPropertyArray(_X11_DETAILS.Display, window, _ATOMS._NET_WM_STATE, Atom.XA_ATOM);
        public static IntPtr[] GetWindowPropertyArray(IntPtr display, IntPtr window, IntPtr atom, Atom reqType)
        {
            if (!TryXGetWindowProperty(display, window, atom, reqType
                , out var actualType
                , out var actualFormat
                , out var nitems
                , out _
                , out var prop
            ))
            {
                return null;
            }

            try
            {
                if (actualType != reqType || actualFormat != 32 || nitems == IntPtr.Zero)
                    return null;

                var buffer = new IntPtr[nitems.ToInt32()];
                Marshal.Copy(prop, buffer, 0, buffer.Length);
                return buffer;
            }
            finally
            {
                LibX11.XFree(prop);
            }
        }




        public static void SetWMAtoms(IntPtr handle, IReadOnlyDictionary<IntPtr, bool> readOnlyDictionary)
            => SetWMAtoms(handle, atomValues: readOnlyDictionary);
        public static void SetWMAtoms(IntPtr handle, IDictionary<IntPtr, bool> dictionary)
            => SetWMAtoms(handle, atomValues: dictionary);
        public static void SetWMAtoms(IntPtr handle, params KeyValuePair<IntPtr, bool>[] array)
            => SetWMAtoms(handle, atomValues: array);
        public static void SetWMAtoms(IntPtr handle, IEnumerable<KeyValuePair<IntPtr, bool>> atomValues)
        {
            List<IntPtr> enableAtoms = new();
            List<IntPtr> disableAtoms = new();

            foreach (var pair in atomValues)
            {
                if (pair.Value)
                    enableAtoms.Add(pair.Key);
                else
                    disableAtoms.Add(pair.Key);
            }

            if (enableAtoms.Count > 0)
                ChangeWMAtoms(handle, true, enableAtoms);

            if (disableAtoms.Count > 0)
                ChangeWMAtoms(handle, false, disableAtoms);
        }
        public static void ChangeWMAtoms(IntPtr handle, bool enable, IEnumerable<IntPtr> atoms)
        {
            IntPtr[] array;
            if (atoms is IntPtr[] arr)
                array = arr;
            else if (atoms != null)
                array = atoms.ToArray();
            else
                array = Array.Empty<IntPtr>();

            ChangeWMAtoms(handle, enable, array);
        }
        public static void ChangeWMAtoms(IntPtr handle, bool enable, params IntPtr[] atoms)
        {
            int length = atoms.Length;
            if ((length != 1) && (length != 2))
                throw new ArgumentException();


            //[TODO: why is this backwards???]
            bool shouldEnable = !enable;


            /*
            if (!_mapped)
            {
            */
            HashSet<IntPtr> newAtoms = new(GetWindowPropertyArray(handle));
            
            foreach (var atom in atoms)
            {
                newAtoms.Add(atom);
            }

            LibX11.XChangeProperty(_X11_DETAILS.Display, handle
                , _ATOMS._NET_WM_STATE
                , Atom.XA_ATOM
                , 32
                , PropertyMode.Replace
                , newAtoms.ToArray()
                , newAtoms.Count
            );
            /*
            }
            */


            SendNetWMMessage(handle
                , _ATOMS._NET_WM_STATE
                , l: atoms
                    .Prepend(shouldEnable
                        ? _NET_WM_STATE_ADD
                        : _NET_WM_STATE_REMOVE
                    )
                    .ToArray()
            );
        }


#if X11_EVENT_LISTEN
        public static unsafe bool GetEventData<T>(IntPtr display, ref T cookie)
        {
            GCHandle handle = GCHandle.Alloc(cookie);
            bool result = LibX11.XGetEventData(display, GCHandle.ToIntPtr(handle));
            handle.Free();
            return result;
        }


        public static unsafe void FreeEventData<T>(IntPtr display, ref T cookie)
        {
            GCHandle handle = GCHandle.Alloc(cookie);
            LibX11.XFreeEventData(display, GCHandle.ToIntPtr(handle));
            handle.Free();
        }
#endif
    }
}