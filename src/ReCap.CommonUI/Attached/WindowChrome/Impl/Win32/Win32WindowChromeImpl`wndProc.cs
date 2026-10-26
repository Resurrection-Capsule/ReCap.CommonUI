using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using ReCap.CommonUI.Util.OperatingSystem.Win32;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    partial class Win32WindowChromeImpl
    {
            public Window Window
            {
                get;
                init;
            }


            bool _isWndProcAttached = false;
            public bool IsWndProcAttached
            {
                get => _isWndProcAttached;
                private set
                {
                    if (_isWndProcAttached == value)
                        return;
                    else if (this.TryGetHostWindow(out Window hostWindow))
                    {
                        if (value)
                            Win32Properties.AddWndProcHookCallback(hostWindow, NonClientWndProc);
                        else
                            Win32Properties.RemoveWndProcHookCallback(hostWindow, NonClientWndProc);
                    }

                    _isWndProcAttached = value;
                }
            }


            List<Visual> _ncHitTestVisuals = new();
            void ReOrderNonClientHitTestVisuals()
            {
                _ncHitTestVisuals = _ncHitTestVisuals
                    //.OrderByDescending(c => c.CalculateDistanceFromAncestor(window))
                    .OrderBy(c =>
                    {
                        var size = c.Bounds.Size;
                        return size.Width * size.Height;
                    }
                ).ToList();
            }


            void AddNonClientHitTestVisual(Visual visual)
            {
                if (!_ncHitTestVisuals.Contains(visual))
                    _ncHitTestVisuals.Add(visual);

                ReOrderNonClientHitTestVisuals();
            }
            void RemoveNonClientHitTestVisual(Visual visual)
            {
                _ncHitTestVisuals.Remove(visual);
                ReOrderNonClientHitTestVisuals();
            }


            bool TryGetVisualFor(Win32Properties.Win32HitTestValue hitResult, out Visual visual)
            {
                foreach (var ncHitTestVisual in _ncHitTestVisuals)
                {
                    if (hitResult != Win32Properties.GetNonClientHitTestResult(ncHitTestVisual))
                        continue;

                    visual = ncHitTestVisual;
                    return true;
                }

                visual = null;
                return false;
            }


            bool NonClientHitTest(PixelPoint pxPoint, out Win32Properties.Win32HitTestValue hitResult)
            {
                foreach (var ncHitTestVisual in _ncHitTestVisuals)
                {
                    PixelPoint tl = ncHitTestVisual.PointToScreen(_POINT_ZERO);
                    var size = ncHitTestVisual.Bounds.Size;
                    PixelPoint br = ncHitTestVisual.PointToScreen(new(size.Width, size.Height));
                    PixelRect ncHitTestRect = new(tl, br);
                    if (!ncHitTestRect.Contains(pxPoint))
                        continue;

                    Win32Properties.Win32HitTestValue result = Win32Properties.GetNonClientHitTestResult(ncHitTestVisual);
                    if (result == NCHitTestResult.CLIENT)
                        continue;

                    hitResult = result;
                    return true;
                }
                hitResult = NCHitTestResult.CLIENT;
                return false;
            }


        internal IntPtr NonClientWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            WindowMessage message = (WindowMessage)msg;
            switch (message)
            {
                case WindowMessage.SYSCOMMAND:
                    return SYSCOMMAND(wParam, lParam, ref handled);
                case WindowMessage.INITMENU:
                    return INITMENU(wParam, lParam, ref handled);
                case WindowMessage.NCCALCSIZE:
                    return NCCALCSIZE(wParam, lParam, ref handled);
                /*
                case WindowMessage.NCHITTEST:
                    return NCHITTEST(wParam, lParam, ref handled);
                */
                case WindowMessage.NCLBUTTONDOWN:
                    return NCLBUTTONDOWN(wParam, lParam, ref handled);
                case WindowMessage.NCLBUTTONDBLCLK:
                    return NCLBUTTONDBLCLK(wParam, lParam, ref handled);

                default:
                    goto unhandled;
            }

            unhandled:
            handled = false;
            return IntPtr.Zero;
        }


        IntPtr INITMENU(IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            /*
            For some reason, Avalonia disables the close button/menu item. Re-enable it here because we don't want that.
            (Why was that even a thing in the first place?)
            */
            handled = MenuEnableClose(wParam);
            return IntPtr.Zero;
        }


        IntPtr SYSCOMMAND(IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            SystemCommand command = (SystemCommand)wParam;
            if (command.HasFlag(SystemCommand.CLOSE))
            {
                this.GetHostWindow()?.Close();
                handled = true;
            }
            return IntPtr.Zero;
        }



        const int _BORDER_WIDTH = 4;
        IntPtr NCCALCSIZE(IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //Console.WriteLine($"{nameof(NCCALCSIZE)}({wParam}, {lParam})");
            handled = true;
            RECT clientRect;
            void ModifyRect()
                => clientRect.Deflate(_BORDER_WIDTH);


            // https://www.cyotek.com/blog/painting-the-borders-of-a-custom-control-using-wm-ncpaint
            if (wParam == IntPtr.Zero)
            {
                clientRect = Marshal.PtrToStructure<RECT>(lParam);
                ModifyRect();

                Marshal.StructureToPtr(clientRect, lParam, false);
                return IntPtr.Zero;
            }
            else
            {
                NCCALCSIZE_PARAMS parameters = Marshal.PtrToStructure<NCCALCSIZE_PARAMS>(lParam);

                clientRect = parameters.rgrc[0];
                ModifyRect();

                parameters.rgrc[0] = clientRect;
                Marshal.StructureToPtr(parameters, lParam, false);
                return new(0x0300);
            }
        }


        IntPtr NCHITTEST(IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //Console.WriteLine($"{nameof(NCHITTEST)}({wParam}, {lParam})");
            PixelPoint pxPoint = MakePoint(lParam);
            if (!NonClientHitTest(pxPoint, out Win32Properties.Win32HitTestValue hitResult))
                return IntPtr.Zero;

            handled = true;
            return new IntPtr((int)hitResult);
        }


        IntPtr NCLBUTTONDOWN(IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            Win32Properties.Win32HitTestValue hitResult = (Win32Properties.Win32HitTestValue)wParam.ToInt32();
            Console.WriteLine($"{nameof(NCLBUTTONDOWN)}({hitResult}, {lParam})");
            switch (hitResult)
            {
                case NCHitTestResult.SYSMENU:
                {
                    PixelPoint pxPoint;
                    if (TryGetVisualFor(NCHitTestResult.SYSMENU, out Visual visual))
                        pxPoint = visual.PointToScreen(new(0, visual.Bounds.Height));
                    else
                        pxPoint = MakePoint(lParam);

                    ShowWindowMenu(Handle, pxPoint);
                    handled = false;
                    break;
                }
            }
            return IntPtr.Zero;
        }


        IntPtr NCLBUTTONDBLCLK(IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            Win32Properties.Win32HitTestValue hitResult = (Win32Properties.Win32HitTestValue)wParam.ToInt32();
            Console.WriteLine($"{nameof(NCLBUTTONDBLCLK)}({hitResult}, {lParam})");
            switch (hitResult)
            {
                case NCHitTestResult.SYSMENU:
                {
                    User32.PostMessage(Handle, WindowMessage.SYSCOMMAND, SystemCommand.DEFAULT, IntPtr.Zero);
                    handled = true;
                    break;
                }
                /*
                case NCHitTestResult.CAPTION:
                {
                    var window = impl.Window;
                    var state = window.WindowState;
                    if ((state == WindowState.Maximized) || (state == WindowState.FullScreen))
                        state = WindowState.Normal;
                    else
                        state = WindowState.Maximized;

                    window.WindowState = state;
                    handled = true;
                    return IntPtr.Zero;
                }
                */
            }
            return IntPtr.Zero;
        }




        static PixelPoint MakePoint(IntPtr value)
            => MakePoint(unchecked(value.ToInt32()));
        static PixelPoint MakePoint(int value)
        {
            int x = (short)(value & 0xffff);
            int y = (short)((value >> 16) & 0xffff);
            return new(x, y);
        }


        static IntPtr MakeParam(PixelPoint point)
            => MakeParam(point.X, point.Y);
        static IntPtr MakeParam(int x, int y)
            => new((y << 16) | (x & 0xFFFF));
    }
}