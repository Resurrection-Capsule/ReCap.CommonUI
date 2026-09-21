using System;
using System.Collections.Generic;
using Avalonia.Controls;
using ReCap.CommonUI.Util.OperatingSystem;
using ReCap.CommonUI.Util.OperatingSystem.Linux;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed partial class LinuxWindowChromeImpl
        : WindowChromeImplBaseBase
    {
        static readonly LinuxDetails _DETAILS = OSInfo.GetOSDetails<LinuxDetails>();




#region Properties
        bool _x11NeedsInit = false;
        X11WindowChromeSubImpl _x11 = null;
        X11WindowChromeSubImpl X11Impl
        {
            get => EnsureSubImpl(ref _x11, ref _x11NeedsInit, () => new(_DETAILS));
        }


#if WAYLAND
        bool _waylandNeedsInit = false;
        WaylandWindowChromeSubImpl _wayland = null;
        WaylandWindowChromeSubImpl WaylandImpl
        {
            get => EnsureSubImpl(ref _wayland, ref _waylandNeedsInit, () => new(_DETAILS));
        }
#endif


        IWindowChromeImpl CurrentSubImpl
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




        public override bool CanUseManagedWindowChrome
            => CurrentSubImpl.CanUseManagedWindowChrome;

        public override bool PrefersManagedWindowChrome
            => CurrentSubImpl.PrefersManagedWindowChrome;

        public override bool DefaultShowCaptionIcon
            => CurrentSubImpl.DefaultShowCaptionIcon;

        public override bool DefaultShowCaptionText
            => CurrentSubImpl.DefaultShowCaptionText;


        readonly CaptionButtonRolesPair _defaultCaptionButtonRoles;
        public override CaptionButtonRolesPair DefaultCaptionButtonRoles
        {
            get => _defaultCaptionButtonRoles;
        }


        public override IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get => CurrentSubImpl.ValidCaptionButtonRoles;
        }
#endregion




        public override void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, Action<bool> applyUseManagedChrome)
            => CurrentSubImpl.ApplyDesiredManagedChrome(window, desiredManagedChrome, applyUseManagedChrome);

        public override bool ExecuteButton(Window window, CaptionButtonClickEventArgs e, Action roleAction)
            => CurrentSubImpl.ExecuteButton(window, e, roleAction);

        public override bool GetDesiredManagedChrome(Window window, ManagedChromeHint chromeMode)
            => CurrentSubImpl.GetDesiredManagedChrome(window, chromeMode);



        public LinuxWindowChromeImpl()
            : base()
        {
            if (TryImportCaptionButtons(out CaptionButtonRolesPair imported))
            {
                _defaultCaptionButtonRoles = imported;
            }
            else
            {
                _defaultCaptionButtonRoles = new()
                {
                    //[TODO: Detect e.g. Unity DE?]
                    //[TODO: platform-level user settings?]
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
            }
        }




        bool _isInitialized = false;
        public override void Init()
        {
            base.Init();


            _x11NeedsInit = true;
#if WAYLAND
            _waylandNeedsInit = true;
#endif

            _isInitialized = true;
            _ = CurrentSubImpl;
        }


        T EnsureSubImpl<T>(ref T impl, ref bool needsInit, Func<T> create)
            where T
                : IWindowChromeImpl
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