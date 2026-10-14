using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using ReCap.CommonUI.Controls.AppearanceHacks;
using ReCap.CommonUI.Util;
using ReCap.CommonUI.Util.OperatingSystem.Win32;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed partial class Win32WindowChromeImpl
        : WindowChromeImplBase
    {
        static readonly Point _POINT_ZERO = new(0d, 0d);




        CompositeDisposable _disposable = null;
        public Win32WindowChromeImpl(WindowChrome chrome)
            : base(chrome)
        {
            Window hostWindow = chrome.HostWindow;
            HWnd = hostWindow.GetHWnd();
            hostWindow.Opened += HostWindow_PropertyThatCallsAvalonia_Win32_WindowImpl_ExtendClientArea_Changed;

            _disposable = new()
            {
                hostWindow
                    .GetObservable(Window.WindowStateProperty)
                    .Subscribe(HostWindow_PropertyThatCallsAvalonia_Win32_WindowImpl_ExtendClientArea_Changed)
                ,
                hostWindow
                    .GetObservable(Window.ExtendClientAreaToDecorationsHintProperty)
                    .Subscribe(HostWindow_PropertyThatCallsAvalonia_Win32_WindowImpl_ExtendClientArea_Changed)
                ,
                hostWindow
                    .GetObservable(Window.ExtendClientAreaChromeHintsProperty)
                    .Subscribe(HostWindow_PropertyThatCallsAvalonia_Win32_WindowImpl_ExtendClientArea_Changed)
                ,
                hostWindow
                    .GetObservable(Window.ExtendClientAreaTitleBarHeightHintProperty)
                    .Subscribe(HostWindow_PropertyThatCallsAvalonia_Win32_WindowImpl_ExtendClientArea_Changed)
                ,
                Disposable.Create(() => hostWindow.Opened -= HostWindow_PropertyThatCallsAvalonia_Win32_WindowImpl_ExtendClientArea_Changed),
            };
        }
        void HostWindow_PropertyThatCallsAvalonia_Win32_WindowImpl_ExtendClientArea_Changed<T>(object sender, T _)
            where T
                : EventArgs
            => HandlePropertyThatCallsAvalonia_Win32_WindowImpl_ExtendClientArea();
        void HostWindow_PropertyThatCallsAvalonia_Win32_WindowImpl_ExtendClientArea_Changed<T>(T _)
            => HandlePropertyThatCallsAvalonia_Win32_WindowImpl_ExtendClientArea();
        void HandlePropertyThatCallsAvalonia_Win32_WindowImpl_ExtendClientArea()
        {
            /*
            For some reason, Avalonia disables the close button/menu item. Re-enable it here because we don't want that.
            (Why was that even a thing in the first place?)
            */
            IntPtr hMenu = User32.GetSystemMenu(HWnd, false);
            MenuEnableClose(hMenu);
        }




        static IntPtr GetWindowMenu(IntPtr hWnd)
        {
            IntPtr hMenu = User32.GetSystemMenu(hWnd, false);
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




        internal void OnNonClientHitTestVisualResultChanged(Visual visual, AvaloniaPropertyChangedEventArgs e)
        {
            var impl = this.GetHostWindow().GetChromePlatformImpl<Win32WindowChromeImpl>();
            impl.AddNonClientHitTestVisual(visual);

            visual.DetachedFromVisualTree += impl.NonClientHitTestVisual_DetachedFromVisualTree;
            visual.AttachedToVisualTree += impl.NonClientHitTestVisual_AttachedToVisualTree;
        }



        protected override bool ExecuteWindowMenu(CaptionButtonClickEventArgs e)
            => true; //Handled via WndProc


        void NonClientHitTestVisual_AttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
        {
            Visual visual = (Visual)sender;
            AddNonClientHitTestVisual(visual);
        }

        void NonClientHitTestVisual_DetachedFromVisualTree(object sender, VisualTreeAttachmentEventArgs e)
        {
            Visual visual = (Visual)sender;
            RemoveNonClientHitTestVisual(visual);
        }


        public override void ApplyDesiredManagedChrome(bool desiredManagedChrome, Action<bool> applyUseManagedChrome)
        {
            Window hostWindow = this.GetHostWindow();
            hostWindow.ExtendClientAreaToDecorationsHint = desiredManagedChrome;
            bool useManagedChrome = default;
            WindowChromeImplHelper.ApplyDesiredManagedChrome(
                this, desiredManagedChrome
                , fallbackToSystemDecorationsProperty: false
                , value =>
                {
                    useManagedChrome = value;
                    applyUseManagedChrome(value);
                }
            );

            IsWndProcAttached = useManagedChrome;
            Dispatcher.UIThread.Post(() => Dispatcher.UIThread.Post(() => VisualPositionHack()));
        }
        static bool MenuEnableClose(IntPtr hMenu)
            => User32.EnableMenuItem(hMenu, SystemCommand.CLOSE, MenuItemEnablementFlags.BYCOMMAND | MenuItemEnablementFlags.ENABLED);


        /// <summary>
        /// Workaround for improper positioning of window visual after change (possible Avalonia bug?)
        /// </remarks>
        void VisualPositionHack()
        {
            Window hostWindow = this.GetHostWindow();
#if WINDOWCHROME_WINDOWS_LESS_HACKY
            hostWindow.InvalidateMeasure();
            hostWindow.InvalidateArrange();
            hostWindow.InvalidateVisual();
#else
            var winState = hostWindow.WindowState;
            Dispatcher.UIThread.Post(() => 
            {
                Action after;
                if ((winState == WindowState.Maximized) || (winState == WindowState.FullScreen))
                    after = Maximized(hostWindow, winState);
                else
                    after = Restored(hostWindow, winState);

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
            base.Prepare(button);
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


        public override void Dispose()
        {
            base.Dispose();
            if (_disposable == null)
                return;

            _disposable.Dispose();
            _disposable = null;
        }
    }
}