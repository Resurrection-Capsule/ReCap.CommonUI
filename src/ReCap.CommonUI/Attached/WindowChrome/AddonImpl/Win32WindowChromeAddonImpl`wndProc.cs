using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;
using ReCap.CommonUI.Util;
using ReCap.CommonUI.Util.Win32;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    partial class Win32WindowChromeAddonImpl
    {
        sealed class WindowData
        {
            public readonly IntPtr HWnd;
            public Window Window
            {
                get;
                init;
            }


            bool _hasWndProc = false;
            public bool HasWndProc
            {
                get => _hasWndProc;
                private set => _hasWndProc = value;
            }


            public WindowData(Window window)
            {
                Window = window;
                //Window.Closed += Window_Closed;
                HWnd = window.GetHWnd();
            }


            public void AddWndProc()
            {
                if (HasWndProc)
                    return;

                Win32Properties.AddWndProcHookCallback(Window, NonClientWndProc);
                HasWndProc = true;
            }
            public void RemoveWndProc()
            {
                if (!HasWndProc)
                    return;

                Win32Properties.RemoveWndProcHookCallback(Window, NonClientWndProc);
                HasWndProc = false;
            }


            List<Visual> _ncHitTestVisuals = new();
            void ReOrderNonClientHitTestVisuals()
            {
                var window = Window;
                _ncHitTestVisuals = _ncHitTestVisuals
                    //.OrderByDescending(c => c.CalculateDistanceFromAncestor(window))
                    .OrderBy(c =>
                    {
                        var size = c.Bounds.Size;
                        return size.Width * size.Height;
                    }
                ).ToList();
            }


            public void AddNonClientHitTestVisual(Visual visual)
            {
                if (!_ncHitTestVisuals.Contains(visual))
                    _ncHitTestVisuals.Add(visual);

                ReOrderNonClientHitTestVisuals();
            }
            public void RemoveNonClientHitTestVisual(Visual visual)
            {
                _ncHitTestVisuals.Remove(visual);
                ReOrderNonClientHitTestVisuals();
            }


            public bool TryGetVisualFor(NCHitTestResult hitResult, out Visual visual)
            {
                foreach (var ncHitTestVisual in _ncHitTestVisuals)
                {
                    if (hitResult != WindowChromeAddon.GetNonClienHitTestResult(ncHitTestVisual))
                        continue;

                    visual = ncHitTestVisual;
                    return true;
                }

                visual = null;
                return false;
            }


            public bool NonClientHitTest(PixelPoint pxPoint, out NCHitTestResult hitResult)
            {
                foreach (var ncHitTestVisual in _ncHitTestVisuals)
                {
                    PixelPoint tl = ncHitTestVisual.PointToScreen(_POINT_ZERO);
                    var size = ncHitTestVisual.Bounds.Size;
                    PixelPoint br = ncHitTestVisual.PointToScreen(new(size.Width, size.Height));
                    PixelRect ncHitTestRect = new(tl, br);
                    if (!ncHitTestRect.Contains(pxPoint))
                        continue;

                    NCHitTestResult result = WindowChromeAddon.GetNonClienHitTestResult(ncHitTestVisual); //(NCHitTestResult)Win32Properties.GetNonClientHitTestResult(ncHitTestVisual);
                    if (result == NCHitTestResult.CLIENT)
                        continue;

                    hitResult = result;
                    return true;
                }
                hitResult = NCHitTestResult.CLIENT;
                return false;
            }
        }


        static void Window_Closed(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            if (!TryGetDataForWindow(window, out WindowData data))
                return;

            UnregisterWindow(window);
        }




        static readonly List<WindowData> _windows = new(); //ew
        //static readonly Dictionary<Window, List<Visual>> _ncHitTestVisuals = new(); //ew
        static void RegisterWindow(Window window)
        {
            IntPtr hWnd = window.GetHWnd();
            if (_windows.Any(x => x.Window == window))
                return;

            WindowData data = new(window);
            _windows.Add(data);
        }


        static void UnregisterWindow(Window window)
        {
            if (!TryGetDataForWindow(window, out WindowData data))
                return;

            data.RemoveWndProc();
            _windows.Remove(data);
        }


        static bool TryGetDataForWindow(Window window, out WindowData data)
        {
            data = _windows.FirstOrDefault(x => x.Window == window);
            return data != null;
        }


        static bool TryGetDataForHWnd(IntPtr hWnd, out WindowData data)
        {
            data = _windows.FirstOrDefault(x => x.HWnd == hWnd);
            return data != null;
        }


        static WindowData GetDataOrRegisterWindow(Window window)
        {
            WindowData data = _windows.FirstOrDefault(x => x.Window == window);
            if (data == null)
            {
                RegisterWindow(window);
                data = _windows.First(x => x.Window == window);
            }


            return data;
        }








        const NCHitTestResult _INVALID_HITTEST_VALUE = (NCHitTestResult)(-1337);
        static IntPtr NonClientWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //Console.WriteLine($"{nameof(NonClientWndProc)}({hWnd}, {msg}, {wParam}, {lParam}, {handled})");
            if (TryGetDataForHWnd(hWnd, out WindowData data))
            {
                WindowMessage message = (WindowMessage)msg;
                bool messageMandled = handled;
                IntPtr result = NonClientWndProcHandler(hWnd, data, message, wParam, lParam, ref messageMandled);
                if (messageMandled)
                {
                    //Console.WriteLine($"{message} handled!");
                    return result;
                }
            }

            handled = false;
            return IntPtr.Zero;
        }


        static IntPtr NonClientWndProcHandler(IntPtr hWnd, WindowData data, WindowMessage message, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (message)
            {
                /*
                case WindowMessage.INITMENU:
                    return INITMENU(hWnd, data, wParam, lParam, ref handled);
                */
                case WindowMessage.NCCALCSIZE:
                    return NCCALCSIZE(hWnd, data, wParam, lParam, ref handled);
                case WindowMessage.NCHITTEST:
                    return NCHITTEST(hWnd, data, wParam, lParam, ref handled);
                case WindowMessage.NCLBUTTONDOWN:
                    return NCLBUTTONDOWN(hWnd, data, wParam, lParam, ref handled);
                case WindowMessage.NCLBUTTONDBLCLK:
                    return NCLBUTTONDBLCLK(hWnd, data, wParam, lParam, ref handled);

                default:
                    break;
            }

            handled = false;
            return IntPtr.Zero;
        }



        const int _BORDER_WIDTH = 4;
        static IntPtr NCCALCSIZE(IntPtr hWnd, WindowData data, IntPtr wParam, IntPtr lParam, ref bool handled)
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


        static readonly Point _POINT_ZERO = new(0d, 0d);
        static IntPtr NCHITTEST(IntPtr hWnd, WindowData data, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            //Console.WriteLine($"{nameof(NCHITTEST)}({wParam}, {lParam})");
            PixelPoint pxPoint = MakePoint(lParam);
#if NO
            Point pt = window.PointToClient(pxPoint); //.ToPointWithDpi(window.RenderScaling);
            //Console.WriteLine($"    {pt}");
            if (window.InputHitTest(pt) is not Visual visual)
                return IntPtr.Zero;

            NCHitTestResult hitResult = _INVALID_HITTEST_VALUE;
            while ((visual != null) && (hitResult != NCHitTestResult.Client))
            {
                hitResult = Win32Properties.GetNonClientHitTestResult(visual);
                if (visual.Parent is Visual parent)
                    visual = parent;
                else
                    break;
            }

            //Console.WriteLine($"    => {hitResult};");
            if (hitResult == _INVALID_HITTEST_VALUE)
                hitResult = NCHitTestResult.Client;
#elif NO
            NCHitTestResult hitResult = NCHitTestResult.Client;

            var ncHitTestVisuals = data.NCHitTestVisuals;
            foreach (var ncHitTestVisual in ncHitTestVisuals)
            {
                PixelPoint tl = ncHitTestVisual.PointToScreen(_POINT_ZERO);
                var size = ncHitTestVisual.Bounds.Size;
                PixelPoint br = ncHitTestVisual.PointToScreen(new(size.Width, size.Height));
                PixelRect ncHitTestRect = new(tl, br);
                if (ncHitTestRect.Contains(pxPoint))
                {
                    hitResult = Win32Properties.GetNonClientHitTestResult(ncHitTestVisual);
                    break;
                }
                //hitResult
                //Win32Properties.GetNonClientHitTestResult(ncHitTestVisual) == NCHitTestResult.Client
            }

            if (hitResult != NCHitTestResult.Client)
            {
                //Console.WriteLine($"    => {hitResult};");
            }
#else
            if (!data.NonClientHitTest(pxPoint, out NCHitTestResult hitResult))
                return IntPtr.Zero;

            //Console.WriteLine($"    => {hitResult};");
#endif
            handled = true;
            return new IntPtr((int)hitResult);
        }


        static IntPtr NCLBUTTONDOWN(IntPtr hWnd, WindowData data, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            NCHitTestResult hitResult = (NCHitTestResult)wParam.ToInt32();
            Console.WriteLine($"{nameof(NCLBUTTONDOWN)}({hitResult}, {lParam})");
            switch (hitResult)
            {
                case NCHitTestResult.SYSMENU:
                {
                    PixelPoint pxPoint;
                    if (data.TryGetVisualFor(NCHitTestResult.SYSMENU, out Visual visual))
                        pxPoint = visual.PointToScreen(new(0, visual.Bounds.Height));
                    else
                        pxPoint = MakePoint(lParam);

                    ShowWindowMenu(hWnd, pxPoint);
                    handled = false;
                    break;
                }
            }
            return IntPtr.Zero;
        }


        static IntPtr NCLBUTTONDBLCLK(IntPtr hWnd, WindowData data, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            NCHitTestResult hitResult = (NCHitTestResult)wParam.ToInt32();
            Console.WriteLine($"{nameof(NCLBUTTONDBLCLK)}({hitResult}, {lParam})");
            switch (hitResult)
            {
                case NCHitTestResult.SYSMENU:
                {
                    ExecuteWindowMenuDefaultItem(hWnd);
                    handled = true;
                    break;
                }
                /*
                case NCHitTestResult.CAPTION:
                {
                    var window = data.Window;
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
            return IntPtr.Zero; //new(1);
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