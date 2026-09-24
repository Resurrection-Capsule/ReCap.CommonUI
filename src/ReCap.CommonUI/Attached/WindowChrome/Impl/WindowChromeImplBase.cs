using System;
using System.Collections.Generic;
using Avalonia.Controls;
using ReCap.CommonUI.Controls.AppearanceHacks;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal abstract class WindowChromeImplBaseBase
        : IWindowChromeImpl
    {
        public abstract bool CanUseManagedWindowChrome
        {
            get;
        }


        public abstract bool PrefersManagedWindowChrome
        {
            get;
        }


        public virtual bool DefaultShowCaptionIcon
        {
            get => true;
        }


        public virtual bool DefaultShowCaptionText
        {
            get => true;
        }


        public abstract CaptionButtonRolesPair DefaultCaptionButtonRoles
        {
            get;
        }


        public abstract IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get;
        }




        public virtual void Init()
        {
        }


        public virtual void OnWindowAttached(Window window)
        {}


        public virtual void OnWindowDetached(Window window)
        {}




        public virtual bool GetDesiredManagedChrome(Window window, ManagedChromeHint chromeMode)
            => DEFAULT_IWindowChromeImpl.GetDesiredManagedChrome_IMPL(this, window, chromeMode);


        public abstract void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, Action<bool> applyUseManagedChrome);




        public virtual bool ExecuteButton(Window window, CaptionButtonClickEventArgs e, Action roleAction)
        {
            bool handled = e.Role switch
            {
                CaptionButtonRole.WindowMenu => ExecuteWindowMenu(window, e),
#if CAPTIONBUTTONROLES_NYI
                CaptionButtonRole.ApplicationMenu => ExecuteApplicationMenu(window, e),
                CaptionButtonRole.ShowOnAllDesktops => ExecuteShowOnAllDesktops(window, e),
                CaptionButtonRole.ContextHelp => ExecuteContextHelp(window, e),
                CaptionButtonRole.Shade => ExecuteShade(window, e),
                CaptionButtonRole.KeepBelow => ExecuteKeepBelow(window, e),
#endif
                CaptionButtonRole.KeepAbove => ExecuteKeepAbove(window, e),
                _ => false,
            };


            if (handled)
                return true;
            else
                return DEFAULT_IWindowChromeImpl.ExecuteButton_IMPL(this, window, e, roleAction);
        }

        protected virtual bool ExecuteWindowMenu(Window window, CaptionButtonClickEventArgs e)
            => false;
#if CAPTIONBUTTONROLES_NYI
        protected virtual bool ExecuteApplicationMenu(Window window, CaptionButtonClickEventArgs e)
            => false;
        protected virtual bool ExecuteShowOnAllDesktops(Window window, CaptionButtonClickEventArgs e)
            => false;
        protected virtual bool ExecuteContextHelp(Window window, CaptionButtonClickEventArgs e)
            => false;
        protected virtual bool ExecuteShade(Window window, CaptionButtonClickEventArgs e)
            => false;
        protected virtual bool ExecuteKeepBelow(Window window, CaptionButtonClickEventArgs e)
            => false;
#endif
        protected virtual bool ExecuteKeepAbove(Window window, CaptionButtonClickEventArgs e)
        {
            window.Topmost = !window.Topmost;
            return true;
        }


        public virtual void Prepare(CaptionButton button)
            => DEFAULT_IWindowChromeImpl.Prepare_IMPL(this, button);
    }




    internal abstract class WindowChromeImplBase
        : WindowChromeImplBaseBase
    {
#region Properties
        readonly CaptionButtonRolesPair _defaultButtonRoles;
        public sealed override CaptionButtonRolesPair DefaultCaptionButtonRoles
        {
            get => _defaultButtonRoles;
        }


        readonly IEnumerable<CaptionButtonRole> _validButtonRoles;
        public sealed override IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get => _validButtonRoles;
        }
#endregion




        public WindowChromeImplBase()
            : base()
        {
            _defaultButtonRoles = InitDefaultButtonRoles();
            _validButtonRoles = InitValidButtonRoles();
        }




        protected virtual CaptionButtonRolesPair InitDefaultButtonRoles()
            => new()
        {
            Left = new()
            {
                CaptionButtonRole.WindowMenu,
            },
            Right = new()
            {
                CaptionButtonRole.Minimize,
                CaptionButtonRole.Maximize,
                CaptionButtonRole.Close,
            },
        };
        protected abstract IEnumerable<CaptionButtonRole> InitValidButtonRoles();
    }
}