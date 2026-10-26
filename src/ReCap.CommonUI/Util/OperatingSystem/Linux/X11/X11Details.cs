using System;

namespace ReCap.CommonUI.Util.OperatingSystem.Linux
{
    internal sealed class X11Details
        : LinuxSubPlatformDetailsBase
    {
        static IntPtr OpenDisplay(IntPtr parameter) //, ref int count)
        {
            IntPtr result = LibX11.XOpenDisplay(parameter);
            //count++;

            if (result == IntPtr.Zero)
                throw new($"{nameof(LibX11.XOpenDisplay)} failed"); // ({count})");

            return result;
        }


        //static int _failCount = 0;
        public readonly IntPtr Display; //, ref _failCount);
        public readonly IntPtr DeferredDisplay; //, ref _failCount);
        public readonly IntPtr RootWindow;
        public readonly X11Atoms Atoms;
#if X11_EVENT_LISTEN
        public readonly X11EventDispatcher EventDispatcher;
#endif
        public X11Details()
        {
            Display = OpenDisplay(IntPtr.Zero); //, ref _failCount);
            DeferredDisplay = OpenDisplay(IntPtr.Zero); //, ref _failCount);
            RootWindow = LibX11.XRootWindow(Display, LibX11.XDefaultScreen(Display));
            Atoms = new(Display);
#if X11_EVENT_LISTEN
            EventDispatcher = new(Display);
#endif
        }
    }
}