using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using ReCap.CommonUI.Controls.AppearanceHacks;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal abstract class WindowChromeImplBase
        : IWindowChromeImpl
    {
        IntPtr _handle = IntPtr.Zero;
        public IntPtr Handle
        {
            get => _handle;
            private set => _handle = value;
        }


        WeakReference<WindowChrome> _chromeRef = null;
        public WindowChrome Chrome
        {
            get => WeakReferenceHelper.PropertyGet(ref _chromeRef);
            private set => WeakReferenceHelper.PropertySet(ref _chromeRef, value);
        }




        public WindowChromeImplBase(WindowChrome chrome)
            : base()
        {
            Chrome = chrome;
            Handle = InitHandle(chrome);
        }


        protected virtual IntPtr InitHandle(WindowChrome chrome)
            => chrome.HostWindow.GetHWnd();




        public virtual bool GetDesiredManagedChrome(ManagedChromeHint chromeMode)
            => chromeMode switch
            {
                ManagedChromeHint.WheneverPossible => WindowChrome.FACTORY.CanUseManagedWindowChrome,
                ManagedChromeHint.Auto => WindowChrome.FACTORY.PrefersManagedWindowChrome,
                _ => false,
            };


        public abstract void ApplyDesiredManagedChrome(bool desiredManagedChrome, Action<bool> applyUseManagedChrome);




        public virtual bool ExecuteButton(CaptionButtonClickEventArgs e, Action roleAction)
        {
            CaptionButtonRole role = e.Role;
            bool handled = role switch
            {
                CaptionButtonRole.Minimize => ExecuteMinimize(e),
                CaptionButtonRole.Maximize => ExecuteMaximize(e),
                CaptionButtonRole.FullScreen => ExecuteFullScreen(e),
                CaptionButtonRole.Close => ExecuteClose(e),
                CaptionButtonRole.WindowMenu => ExecuteWindowMenu(e),
#if CAPTIONBUTTONROLES_NYI
                CaptionButtonRole.ApplicationMenu => ExecuteApplicationMenu(e),
#endif
#if CAPTIONBUTTONROLES_WIP
                CaptionButtonRole.ShowOnAllDesktops => ExecuteShowOnAllDesktops(e),
#endif
#if CAPTIONBUTTONROLES_NYI
                CaptionButtonRole.ContextHelp => ExecuteContextHelp(e),
#endif
#if CAPTIONBUTTONROLES_WIP
                CaptionButtonRole.Shade => ExecuteShade(e),
#endif
                CaptionButtonRole.KeepBelow => ExecuteKeepBelow(e),
                CaptionButtonRole.KeepAbove => ExecuteKeepAbove(e),
                _ => false,
            };


            return handled;
        }


        protected virtual bool ExecuteMinimize(CaptionButtonClickEventArgs e)
            => false;
        protected virtual bool ExecuteMaximize(CaptionButtonClickEventArgs e)
            => false;
        protected virtual bool ExecuteFullScreen(CaptionButtonClickEventArgs e)
            => false;
        protected virtual bool ExecuteClose(CaptionButtonClickEventArgs e)
            => false;
        protected virtual bool ExecuteWindowMenu(CaptionButtonClickEventArgs e)
            => false;
#if CAPTIONBUTTONROLES_NYI
        protected virtual bool ExecuteApplicationMenu(CaptionButtonClickEventArgs e)
            => false;
#endif
#if CAPTIONBUTTONROLES_WIP
        protected virtual bool ExecuteShowOnAllDesktops(CaptionButtonClickEventArgs e)
            => false;
#endif
#if CAPTIONBUTTONROLES_NYI
        protected virtual bool ExecuteContextHelp(CaptionButtonClickEventArgs e)
            => false;
#endif
#if CAPTIONBUTTONROLES_WIP
        protected virtual bool ExecuteShade(CaptionButtonClickEventArgs e)
            => false;
#endif
        protected virtual bool ExecuteKeepBelow(CaptionButtonClickEventArgs e)
            => false;
        protected virtual bool ExecuteKeepAbove(CaptionButtonClickEventArgs e)
        {
            if (!TryEnsureEventAndTryGetHostWindow(e, out Window window))
                return false;

            window.Topmost = !window.Topmost;
            return true;
        }


        public virtual void Prepare(CaptionButton button)
            => button.UseManagedToolTip = button.Role != CaptionButtonRole.WindowMenu;




        protected bool TryEnsureEventAndTryGetHostWindow(CaptionButtonClickEventArgs e, out Window window, [CallerMemberName] string methodName = null)
        {
            if (!this.TryGetHostWindow(out window))
                return false;

            bool isClick = e.PointerArgs.RoutedEvent == Button.ClickEvent;
            Debug.WriteLine($"{methodName}({e}) => {isClick}");
            return isClick;
        }


#if CAPTIONBUTTONROLES_WIP
        public virtual bool CoerceShowOnAllDesktops(bool proposedValue)
            => false;

        public virtual bool CoerceShaded(bool proposedValue)
            => false;
#endif

        public virtual bool CoerceBottommost(bool proposedValue)
            => false;




        public virtual void Dispose()
        {
            Chrome = null;
            Handle = IntPtr.Zero;
        }
    }
}