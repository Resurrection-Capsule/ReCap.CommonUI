using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.VisualTree;
using ReCap.CommonUI.Controls.AppearanceHacks;
using ReCap.CommonUI.Util;
using ReCap.CommonUI.Util.OperatingSystem.Win32;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed partial class Win32WindowChromeImpl
        : WindowChromeImplBase
    {
        public override bool CanUseManagedWindowChrome
        {
            get => true;
        }


        public override bool PrefersManagedWindowChrome
        {
            get => true;
        }


        public override bool DefaultShowCaptionIcon
        {
            get => true;
        }


        public override bool DefaultShowCaptionText
        {
            get => true;
        }

        
        protected override IEnumerable<CaptionButtonRole> InitValidButtonRoles()
            => new[]
            {
                CaptionButtonRole.WindowMenu,
                CaptionButtonRole.Minimize,
                CaptionButtonRole.Maximize,
                CaptionButtonRole.FullScreen,
                CaptionButtonRole.Close,
                CaptionButtonRole.KeepAbove,
            };


        public override void Init()
        {
            Win32Properties.NonClientHitTestResultProperty.Changed.AddClassHandler<Visual>(NonClientHitTestVisual_ResultChanged);
        }


        void NonClientHitTestVisual_ResultChanged(Visual visual, AvaloniaPropertyChangedEventArgs args)
        {
            if (visual.IsAttachedToVisualTree() && (visual.GetVisualRoot() is Window window))
            {
                WindowData data = GetDataOrRegisterWindow(window);
                data.AddNonClientHitTestVisual(visual);
            }

            visual.DetachedFromVisualTree += NonClientHitTestVisual_DetachedFromVisualTree;
            visual.AttachedToVisualTree += NonClientHitTestVisual_AttachedToVisualTree;
        }



        public override bool GetDesiredManagedChrome(Window window, ManagedChromeHint chromeMode)
            => DEFAULT_IWindowChromeImpl.GetDesiredManagedChrome_IMPL(this, window, chromeMode);



        protected override bool ExecuteWindowMenu(Window window, CaptionButtonClickEventArgs e)
        {
            goto end;
            /*
            https://stackoverflow.com/questions/73927623/how-to-show-the-windows-system-menu-programmatically-when-formborderstyle-is-no
            https://stackoverflow.com/a/73928532
            https://stackoverflow.com/questions/73927623/how-to-show-the-windows-system-menu-programmatically-when-formborderstyle-is-no/73928532#73928532
            */
            if (!window.TryGetHWnd(out IntPtr hWnd))
                goto end;
            if (e.MouseButton != MouseButton.Left)
                goto end;


            if (e.ClickCount > 1)
            {
                if (e.Pressed)
                    goto end;

                ExecuteWindowMenuDefaultItem(hWnd);
            }
            else
            {
                if (!e.Pressed)
                    goto end;

                var visual = e.Visual;
                /*
                var pt = e.Visual.Bounds.BottomLeft;
                var pxPoint = new(Helpers.RoundToInt(pt.X), Helpers.RoundToInt(pt.Y))
                */
                PixelPoint bottomLeft = visual.PointToScreen(new(0d, visual.Bounds.Height));
                ShowWindowMenu(hWnd, bottomLeft);
            }
            end:
            return true;
        }




        static void ExecuteWindowMenuDefaultItem(IntPtr hWnd)
        {
            //GetWindowMenu(hWnd);
            User32.PostMessage(hWnd, WindowMessage.SYSCOMMAND, (IntPtr)SystemCommand.DEFAULT, IntPtr.Zero);
        }

        static IntPtr GetWindowMenu(IntPtr hWnd)
        {
            IntPtr hMenu = User32.GetSystemMenu(hWnd, false);
            /*
            User32.EnableMenuItem(hMenu, 5, MenuItemEnablementFlags.BYPOSITION | MenuItemEnablementFlags.ENABLED);
            */
            return hMenu;
        }


        const TrackPopupMenuFlags _MENU_FLAGS = 
            //(TrackPopupMenuFlags)0x102
            // TrackPopupMenuFlags.RETURNCMD | TrackPopupMenuFlags.RIGHTBUTTON
            TrackPopupMenuFlags.LEFTALIGN | TrackPopupMenuFlags.TOPALIGN | TrackPopupMenuFlags.RETURNCMD | TrackPopupMenuFlags.RIGHTBUTTON
        ;


        static void ShowWindowMenu(IntPtr hWnd, PixelPoint pxPoint)
        {
            //User32.SendMessage(hWnd, WindowMessage.SYSCOMMAND, new(0xF090), MakeParam(pxPoint));

            IntPtr hMenu = GetWindowMenu(hWnd);
            int menuIdentifier = User32.TrackPopupMenu(hMenu, _MENU_FLAGS, pxPoint.X, pxPoint.Y, 0, hWnd, IntPtr.Zero);

            if (menuIdentifier != 0)
                User32.PostMessage(hWnd, WindowMessage.SYSCOMMAND, (IntPtr)menuIdentifier, IntPtr.Zero);
        }


        /*
        public override void ExecuteExtendedCaptionButton(Window window, CaptionButtonClickEventArgs e)
        {
            var visBounds = e.Visual.Bounds;
            var role = e.Role;
            switch (role)
            {
                case CaptionButtonRole.WindowMenu:
                {
                    / *
                    https://stackoverflow.com/questions/73927623/how-to-show-the-windows-system-menu-programmatically-when-formborderstyle-is-no
                    https://stackoverflow.com/a/73928532
                    https://stackoverflow.com/questions/73927623/how-to-show-the-windows-system-menu-programmatically-when-formborderstyle-is-no/73928532#73928532
                    * /
                    if (!window.TryGetHWnd(out IntPtr hWnd))
                        break;

                    if (input == CaptionButtonInputAction.DoubleClick)
                    {
                        //[TODO: not working?]
                        Win32Methods.PostMessage(hWnd, WindowMessage.SYSCOMMAND, (IntPtr)SystemCommand.DEFAULT, IntPtr.Zero);
                    }
                    else if (input == CaptionButtonInputAction.LeftClick)
                    {
                        PixelPoint bottomLeft = visual.PointToScreen(new(0d, visBounds.Height));
                        IntPtr hMenu = Win32Methods.GetSystemMenu(hWnd, false);
                        int menuIdentifier = Win32Methods.TrackPopupMenu(hMenu, _MENU_FLAGS, bottomLeft.X, bottomLeft.Y, 0, hWnd, IntPtr.Zero);

                        if (menuIdentifier != 0)
                            Win32Methods.PostMessage(hWnd, WindowMessage.SYSCOMMAND, (IntPtr)menuIdentifier, IntPtr.Zero);
                    }
                    break;
                }
                default:
                {
                    base.ExecuteExtendedCaptionButton(window, visual, role, input);
                    break;
                }
            }
        }
        */


        void NonClientHitTestVisual_AttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
        {
            Visual visual = (Visual)sender;
            if (e.Root is Window window)
                GetDataOrRegisterWindow(window).AddNonClientHitTestVisual(visual);

        }

        void NonClientHitTestVisual_DetachedFromVisualTree(object sender, VisualTreeAttachmentEventArgs e)
        {
            Visual visual = (Visual)sender;
            if (e.Root is not Window window)
                return;
            else if (TryGetDataForWindow(window, out WindowData data))
                data.RemoveNonClientHitTestVisual(visual);
        }

        /*
        void AddNonClientHitTestVisual(Window window, Visual visual)
        {
            if (Win32Properties.GetNonClientHitTestResult(visual) == Win32Properties.Win32HitTestValue.Client)
            {
                RemoveNonClientHitTestVisual(window, visual);
                return;
            }

            if (!_ncHitTestVisuals.TryGetValue(window, out List<Visual> ncHitTestVisuals))
                ncHitTestVisuals = new();

            if (!ncHitTestVisuals.Contains(visual))
                ncHitTestVisuals.Add(visual);

            ncHitTestVisuals = ncHitTestVisuals.OrderByDescending(c => c.CalculateDistanceFromAncestor(window)).ToList();
            _ncHitTestVisuals[window] = ncHitTestVisuals;
        }

        void RemoveNonClientHitTestVisual(Window window, Visual visual)
        {
            if (!_ncHitTestVisuals.TryGetValue(window, out List<Visual> ncHitTestVisuals))
                return;

            ncHitTestVisuals.Remove(visual);
            _ncHitTestVisuals[window] = ncHitTestVisuals;
        }
        */


        public override void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, Action<bool> applyUseManagedChrome)
        {
            window.ExtendClientAreaToDecorationsHint = desiredManagedChrome;
            /*
            DefaultWindowChromeImpl.ApplyDesiredManagedChrome_Default(
                this, window, desiredManagedChrome, ref useManagedChrome
                , fallbackToSystemDecorationsProperty: false
            );
            */
            bool useManagedChrome = default;
            DEFAULT_IWindowChromeImpl.ApplyDesiredManagedChrome_IMPL(
                this, window, desiredManagedChrome
                , fallbackToSystemDecorationsProperty: false
                , value =>
                {
                    useManagedChrome = value;
                    applyUseManagedChrome(value);
                }
            );

            if (useManagedChrome)
            {
                WindowData data = GetDataOrRegisterWindow(window);
                data.AddWndProc();
            }
            else
            {
                if (TryGetDataForWindow(window, out WindowData data))
                    data.RemoveWndProc();
            }

            Dispatcher.UIThread.Post(() => Dispatcher.UIThread.Post(() => {
                VisualPositionHack(window);
            }));
        }


        /// <summary>
        /// Workaround for improper positioning of window visual after change (possible Avalonia bug?)
        /// </remarks>
        void VisualPositionHack(Window window)
        {
#if WINDOWCHROME_WINDOWS_LESS_HACKY
            window.InvalidateMeasure();
            window.InvalidateArrange();
            window.InvalidateVisual();
#else
            var winState = window.WindowState;
            Dispatcher.UIThread.Post(() => 
            {
                Action after;
                if ((winState == WindowState.Maximized) || (winState == WindowState.FullScreen))
                    after = Maximized(window, winState);
                else
                    after = Restored(window, winState);

                Dispatcher.UIThread.Post(after);
            });
#endif
        }


        static Action Restored(Window window, WindowState winState)
        {
            Action after;
            double width = window.Width;
            double height = window.Height;

            if (width > window.MinWidth)
            {
                window.Width--;
                after = () => window.Width++;
            }
            else if (width < window.MaxWidth)
            {
                window.Width++;
                after = () => window.Width--;
            }
            else if (height > window.MinHeight)
            {
                window.Height--;
                after = () => window.Height++;
            }
            else if (height < window.MaxHeight)
            {
                window.Height++;
                after = () => window.Height--;
            }
            else
            {
                window.WindowState = WindowState.Maximized;
                after = () => window.WindowState = winState;
            }

            return after;
        }


        static Action Maximized(Window window, WindowState winState)
        {
            window.WindowState = WindowState.Normal;
            return () => window.WindowState = winState;
        }




        public override void Prepare(CaptionButton button)
        {
            DEFAULT_IWindowChromeImpl.Prepare_IMPL(this, button);
            Win32Properties.Win32HitTestValue hitTestValue;
            switch (button.Role)
            {
                case CaptionButtonRole.Minimize:
                    button.UseManagedToolTip = false;
                    hitTestValue = NCHitTestResult.MINBUTTON;
                    break;

                case CaptionButtonRole.Maximize:
                    button.UseManagedToolTip = false;
                    hitTestValue = NCHitTestResult.MAXBUTTON;
                    break;

                case CaptionButtonRole.Close:
                    button.UseManagedToolTip = false;
                    hitTestValue = NCHitTestResult.CLOSE;
                    break;

                case CaptionButtonRole.WindowMenu:
                    button.UseManagedToolTip = false;
                    hitTestValue = NCHitTestResult.SYSMENU;
                    break;

                default:
                    return;
            }
            Win32Properties.SetNonClientHitTestResult(button, hitTestValue);
        }
    }
}