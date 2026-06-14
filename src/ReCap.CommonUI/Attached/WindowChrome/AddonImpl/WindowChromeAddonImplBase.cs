using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal abstract class WindowChromeAddonImplBase
        : IWindowChromeAddonImpl
    {
        public abstract bool CanUseManagedWindowChrome
        {
            get;
        }


        public abstract bool PrefersManagedWindowChrome
        {
            get;
        }


        public virtual bool DefaultIconInTitleBar
        {
            get => true;
        }


        protected abstract CaptionButtonRolesPair CreateDefaultCaptionButtons();
        public CaptionButtonRolesPair DefaultCaptionButtons
        {
            get => CreateDefaultCaptionButtons();
        }


        public WindowChromeAddonImplBase()
        {
            _validCaptionButtonRoles = GetValidCaptionButtonRoles();
            if (_validCaptionButtonRoles.Any())
                _validCaptionButtonRoles = _validCaptionButtonRoles.OrderBy(x => x);
        }


        public virtual void Init()
        {
        }




        public virtual bool GetDesiredManagedChrome(Window window, ManagedChromeMode chromeMode)
            => DefaultWindowChromeAddonImpl.GetDesiredManagedChrome_Default(this, window, chromeMode);


        public abstract void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, Action<bool> applyUseManagedChrome);




        readonly IEnumerable<CaptionButtonRole> _validCaptionButtonRoles;
        public IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get => _validCaptionButtonRoles;
        }
        protected abstract IEnumerable<CaptionButtonRole> GetValidCaptionButtonRoles();
        public virtual void ExecuteExtendedCaptionButton(Window window, CaptionButtonClickEventArgs e)
        {
            var role = e.Role;
            switch (role)
            {
                case CaptionButtonRole.Menu:
                {
                    ExecuteMenu(window, e);
                    break;
                }
                /*

                case CaptionButtonRole.ApplicationMenu:
                {
                    break;
                }

                case CaptionButtonRole.ShowOnAllDesktops:
                {
                    break;
                }

                case CaptionButtonRole.ContextHelp:
                {
                    break;
                }

                case CaptionButtonRole.Shade:
                {
                    break;
                }

                case CaptionButtonRole.KeepBelow:
                {
                    break;
                }

                */
                case CaptionButtonRole.KeepAbove:
                {
                    ExecuteKeepAbove(window, e);
                    break;
                }
            }
        }

        protected virtual void ExecuteKeepAbove(Window window, CaptionButtonClickEventArgs e)
            => window.Topmost = !window.Topmost;
        protected virtual void ExecuteMenu(Window window, CaptionButtonClickEventArgs e)
        {}
    }
}