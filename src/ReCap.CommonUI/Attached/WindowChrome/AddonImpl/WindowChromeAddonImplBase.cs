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
            get => false;
        }


        protected virtual bool ShouldSetSystemDecorationsAsFallback
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
            => chromeMode switch
            {
                ManagedChromeMode.WheneverPossible => CanUseManagedWindowChrome,
                ManagedChromeMode.Auto => PrefersManagedWindowChrome,
                _ => false,
            };


        public virtual void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, ref bool useManagedChrome)
        {
            bool oldIsExtendedIntoWindowDecorations = window.IsExtendedIntoWindowDecorations;
            bool isUsingManagedChrome = default;


            Dispatcher.UIThread.Invoke(() =>
            {
                if (ShouldSetSystemDecorationsAsFallback)
                {
                    if (desiredManagedChrome && !window.IsExtendedIntoWindowDecorations)
                        window.SystemDecorations = SystemDecorations.None;
                    else if ((!desiredManagedChrome) && !oldIsExtendedIntoWindowDecorations)
                        window.SystemDecorations = SystemDecorations.Full;
                }


                Dispatcher.UIThread.Invoke(() =>
                {
                    isUsingManagedChrome = window.IsExtendedIntoWindowDecorations || desiredManagedChrome;
                });
            });


            useManagedChrome = isUsingManagedChrome;
        }





        readonly IEnumerable<CaptionButtonRole> _validCaptionButtonRoles;
        public IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get => _validCaptionButtonRoles;
        }
        /*
        protected virtual IEnumerable<CaptionButtonRole> GetValidCaptionButtonRoles()
            => new List<CaptionButtonRole>()
            {
                CaptionButtonRole.Minimize,
                CaptionButtonRole.Maximize,
                CaptionButtonRole.Close,
            };
        */
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