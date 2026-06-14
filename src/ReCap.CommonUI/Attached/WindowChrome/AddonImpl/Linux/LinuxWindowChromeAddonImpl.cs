using System;
using System.Collections.Generic;
using Avalonia.Controls;
using ReCap.CommonUI.Util.OperatingSystem;
using ReCap.CommonUI.Util.OperatingSystem.Linux;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed partial class LinuxWindowChromeAddonImpl
        : IWindowChromeAddonImpl
    {
        static readonly LinuxDetails _DETAILS = OSInfo.GetOSDetails<LinuxDetails>();




#region Properties
        bool _x11NeedsInit = false;
        X11WindowChromeAddonSubImpl _x11 = null;
        X11WindowChromeAddonSubImpl X11Impl
        {
            get => EnsureSubImpl(ref _x11, ref _x11NeedsInit, () => new(_DETAILS));
        }


#if WAYLAND
        bool _waylandNeedsInit = false;
        WaylandWindowChromeAddonSubImpl _wayland = null;
        WaylandWindowChromeAddonSubImpl WaylandImpl
        {
            get => EnsureSubImpl(ref _wayland, ref _waylandNeedsInit, () => new(_DETAILS));
        }
#endif


        IWindowChromeAddonImpl CurrentSubImpl
        {
            get
            {
#if WAYLAND
                if (_DETAILS.CurrentDisplayServer == KnownDisplayServer.Wayland)
                    return WaylandImpl;
                else
#endif
                    return X11Impl;
            }
        }




        public bool CanUseManagedWindowChrome
            => CurrentSubImpl.CanUseManagedWindowChrome;

        public bool PrefersManagedWindowChrome
            => CurrentSubImpl.PrefersManagedWindowChrome;

        public bool DefaultIconInTitleBar
            => CurrentSubImpl.DefaultIconInTitleBar;


        readonly CaptionButtonRolesPair _defaultCaptionButtons;
        public CaptionButtonRolesPair DefaultCaptionButtons
        {
            get => _defaultCaptionButtons;
            private init => _defaultCaptionButtons = value;
        }


        public IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
            => CurrentSubImpl.ValidCaptionButtonRoles;
#endregion




        public void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, Action<bool> applyUseManagedChrome)
            => CurrentSubImpl.ApplyDesiredManagedChrome(window, desiredManagedChrome, applyUseManagedChrome);

        public void ExecuteExtendedCaptionButton(Window window, CaptionButtonClickEventArgs e)
            => CurrentSubImpl.ExecuteExtendedCaptionButton(window, e);

        public bool GetDesiredManagedChrome(Window window, ManagedChromeMode chromeMode)
            => CurrentSubImpl.GetDesiredManagedChrome(window, chromeMode);




        public LinuxWindowChromeAddonImpl()
            : base()
        {
            if (TryImportCaptionButtons(out CaptionButtonRolesPair imported))
            {
                DefaultCaptionButtons = imported;
            }
            else
            {
                DefaultCaptionButtons = new()
                {
                    //[TODO: Detect e.g. Unity DE?]
                    //[TODO: platform-level user settings?]
                    Left = new()
                    {
                        CaptionButtonRole.Menu,
                    },
                    Right = new()
                    {
                        CaptionButtonRole.Minimize,
                        CaptionButtonRole.Maximize,
                        CaptionButtonRole.Close,
                    },
                };
            }
        }




        bool _isInitialized = false;
        public void Init()
        {
            _x11NeedsInit = true;
#if WAYLAND
            _waylandNeedsInit = true;
#endif

            _isInitialized = true;
            _ = CurrentSubImpl;
        }


        T EnsureSubImpl<T>(ref T impl, ref bool needsInit, Func<T> create)
            where T
                : IWindowChromeAddonImpl
        {
            impl ??= create();

            if (needsInit && _isInitialized)
            {
                impl.Init();
                needsInit = false;
            }
            return impl;
        }
    }
}