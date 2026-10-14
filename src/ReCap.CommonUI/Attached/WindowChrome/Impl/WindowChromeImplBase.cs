using System;
using Avalonia.Controls;
using ReCap.CommonUI.Controls.AppearanceHacks;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal abstract class WindowChromeImplBase
        : IWindowChromeImpl
    {
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
        }




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
                CaptionButtonRole.ShowOnAllDesktops => ExecuteShowOnAllDesktops(e),
                CaptionButtonRole.ContextHelp => ExecuteContextHelp(e),
                CaptionButtonRole.Shade => ExecuteShade(e),
                CaptionButtonRole.KeepBelow => ExecuteKeepBelow(e),
#endif
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
        protected virtual bool ExecuteShowOnAllDesktops(CaptionButtonClickEventArgs e)
            => false;
        protected virtual bool ExecuteContextHelp(CaptionButtonClickEventArgs e)
            => false;
        protected virtual bool ExecuteShade(CaptionButtonClickEventArgs e)
            => false;
        protected virtual bool ExecuteKeepBelow(CaptionButtonClickEventArgs e)
            => false;
#endif
        protected virtual bool ExecuteKeepAbove(CaptionButtonClickEventArgs e)
        {
            if (this.TryGetHostWindow(out Window window))
                window.Topmost = !window.Topmost;

            return true;
        }


        public virtual void Prepare(CaptionButton button)
            => button.UseManagedToolTip = button.Role != CaptionButtonRole.WindowMenu;




        public virtual void Dispose()
            => Chrome = null;
    }
}