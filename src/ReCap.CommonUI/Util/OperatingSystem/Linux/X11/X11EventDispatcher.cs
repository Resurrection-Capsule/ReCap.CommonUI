#if X11_EVENT_LISTEN
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ReCap.CommonUI.Util.OperatingSystem.Linux
{
    internal class X11EventDispatcher
    {
        public delegate void EventHandler(ref XEvent xEvent);
        /*
        static readonly LinuxDetails _DETAILS = OSInfo.GetOSDetails<LinuxDetails>();
        static readonly X11Details _X11_DETAILS;
        static X11EventDispatcher()
        {
            _X11_DETAILS = _DETAILS.GetDisplayServerDetails<X11Details>();
        }
        */




        public readonly Dictionary<IntPtr, EventHandler> EventHandlers = new();


        //public int Fd { get; }
        //public readonly Dictionary<IntPtr, EventHandler> EventHandlers = new();
        readonly IntPtr _display;
        readonly CancellationToken _token = new();
        readonly Thread _thread;
        SynchronizationContext _syncContext;
        public X11EventDispatcher(IntPtr display)
        {
            _display = display;
            _syncContext = new();
            /*
            _token.Register(DispatchX11Events);
            Dispatcher.UIThread.MainLoop(_token);
            */
            _thread = new(DispatchX11Events);
            _thread.Start();
        }


        void DispatchX11Events()
        {
            Debug.WriteLine($"{nameof(DispatchX11Events)}()");
            bool pending = true;
            while (pending)
            {
                _syncContext.Post(_ => pending = DoDispatch(), null);
            }
            LibX11.XFlush(_display);
        }


        unsafe bool DoDispatch()
        {
            Debug.WriteLine($"{nameof(DoDispatch)}()");
            if (LibX11.XPending(_display) != 0)
                return false;
            else if (_token.IsCancellationRequested)
                return false;
                
            LibX11.XNextEvent(_display, out XEvent xEvent);
            if (LibX11.XFilterEvent(ref xEvent, IntPtr.Zero))
                return true;

            if (xEvent.type == XEventName.GenericEvent)
                X11Helper.GetEventData(_display, ref xEvent.GenericEventCookie);

            try
            {
                /*
                if (xEvent.type == XEventName.GenericEvent)
                {
                    if (_platform.XI2 != null && _platform.Info.XInputOpcode ==
                        xEvent.GenericEventCookie.extension)
                    {
                        _platform.XI2.OnEvent((XIEvent*)xEvent.GenericEventCookie.data);
                    }
                }
                else */if (EventHandlers.TryGetValue(xEvent.AnyEvent.window, out var handler))
                    _syncContext.Post(_ => handler(ref xEvent), null);
            }
            finally
            {
                if (xEvent.type == XEventName.GenericEvent && xEvent.GenericEventCookie.data != null)
                    X11Helper.FreeEventData(_display, ref xEvent.GenericEventCookie);
            }
            return true;
        }
    }
}
#endif