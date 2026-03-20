using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Input;
using Avalonia.VisualTree;
using ReCap.CommonUI.Attached.WindowChrome;
using ReCap.CommonUI.Util;
using ReCap.CommonUI.Util.Win32;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    [PseudoClasses(_PSEUD_WINDOW_ACTIVE)]
    public sealed partial class CaptionButton
        : Button
    {
        const bool _DEFAULT_IsHostWindowActive = true;
        const string _PSEUD_WINDOW_ACTIVE = ":windowactive";
        public static readonly DirectProperty<CaptionButton, bool> IsHostWindowActiveProperty
            = AvaloniaProperty.RegisterDirect<CaptionButton, bool>(nameof(IsHostWindowActive)
            , getter: o => o.IsHostWindowActive
            , unsetValue: _DEFAULT_IsHostWindowActive
        );
        bool _isHostWindowActive = _DEFAULT_IsHostWindowActive;
        public bool IsHostWindowActive
        {
            get => _isHostWindowActive;
            private set => SetAndRaise(IsHostWindowActiveProperty, ref _isHostWindowActive, value);
        }


        public static readonly DirectProperty<CaptionButton, WindowState> HostWindowStateProperty
            = AvaloniaProperty.RegisterDirect<CaptionButton, WindowState>(nameof(HostWindowState)
            , getter: o => o.HostWindowState
        );
        WindowState _hostWindowState = WindowState.Normal;
        public WindowState HostWindowState
        {
            get => _hostWindowState;
            private set => SetAndRaise(HostWindowStateProperty, ref _hostWindowState, value);
        }


        const CaptionButtonRole _NO_ROLE = (CaptionButtonRole)(-1);
        public static readonly DirectProperty<CaptionButton, CaptionButtonRole> RoleProperty
            = AvaloniaProperty.RegisterDirect<CaptionButton, CaptionButtonRole>(nameof(Role)
            , getter: o => o.Role
            , setter: (o, v) => o.Role = v
            , unsetValue: _NO_ROLE
        );
        CaptionButtonRole _role = _NO_ROLE;
        public CaptionButtonRole Role
        {
            get => _role;
            set => SetAndRaise(RoleProperty, ref _role, value);
        }


        static CaptionButton()
        {
            FocusableProperty.OverrideDefaultValue<CaptionButton>(false);
            IsTabStopProperty.OverrideDefaultValue<CaptionButton>(false);

            AvaloniaProperty[] props =
            {
                IsHostWindowActiveProperty,
                HostWindowStateProperty,
                RoleProperty,
            };
            AffectsRender<CaptionButton>(props);

            IsHostWindowActiveProperty.Changed.AddClassHandler<CaptionButton>(IsHostWindowActiveProperty_Changed);
            RoleProperty.Changed.AddClassHandler<CaptionButton>(RoleProperty_Changed);
        }

        static void IsHostWindowActiveProperty_Changed(CaptionButton button, AvaloniaPropertyChangedEventArgs e)
        {
            bool isHostWindowActive = e.GetNewValue<bool>();
            button.PseudoClasses.Set(_PSEUD_WINDOW_ACTIVE, isHostWindowActive);
        }



        internal static readonly IReadOnlyDictionary<CaptionButtonRole, NCHitTestResult> ROLE_TO_NCHITTEST = new Dictionary<CaptionButtonRole, NCHitTestResult>()
        {
            [CaptionButtonRole.Minimize] = NCHitTestResult.MINBUTTON,
            [CaptionButtonRole.Maximize] = NCHitTestResult.MAXBUTTON,
            [CaptionButtonRole.Close] = NCHitTestResult.CLOSE,
            [CaptionButtonRole.Menu] = NCHitTestResult.SYSMENU,
        };
        static void RoleProperty_Changed(CaptionButton button, AvaloniaPropertyChangedEventArgs e)
        {
            var role = e.GetNewValue<CaptionButtonRole>();
            button.OnRoleChanged(role);
        }
        void OnRoleChanged(CaptionButtonRole role)
        {
            if (!ROLE_TO_NCHITTEST.TryGetValue(role, out NCHitTestResult hitTestResult))
                hitTestResult = NCHitTestResult.CLIENT;

            WindowChromeAddon.SetNonClienHitTestResult(this, hitTestResult);
        }


        public CaptionButton()
            : base()
        {
            PseudoClasses.Set(_PSEUD_WINDOW_ACTIVE, IsHostWindowActive);
            OnRoleChanged(Role);
        }


        IDisposable _hostWindowDisposable = null;
        CaptionButtons2 _captionButtons = null;
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            if (e.Root is not Window window)
            {
                IsHostWindowActive = _DEFAULT_IsHostWindowActive;
                return;
            }

            _hostWindowDisposable = new CompositeDisposable()
            {
                window.GetObservable(WindowBase.IsActiveProperty)
                    .Subscribe(HostWindow_IsActiveChanged)
                ,
                window.GetObservable(Window.WindowStateProperty)
                    .Subscribe(UpdateHostWindowState)
                ,
            };
            IsHostWindowActive = window.IsActive;

            CaptionButtons2 captionButtons = this.FindAncestorOfType<CaptionButtons2>();
            if (captionButtons != null)
                _captionButtons = captionButtons;
        }


        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            _hostWindowDisposable?.Dispose();
            _hostWindowDisposable = null;

            _captionButtons = null;
            IsHostWindowActive = _DEFAULT_IsHostWindowActive;
        }


        protected override void OnClick()
        {
            if (IsExtended)
            {
                base.OnClick();
                return;
            }

            PointerPointProperties ptProps = new(RawInputModifiers.LeftMouseButton, PointerUpdateKind.LeftButtonPressed);
            PointerEventArgs e = new(ClickEvent, this, null, VisualRoot as Visual, Bounds.Center, default, ptProps, KeyModifiers.None);
            Execute(new(this, Role, true, e), fallback: null);
        }


        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            Execute(true, e);
        }


        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            Execute(false, e);
        }


        void Execute(bool pressed, PointerEventArgs baseArgs)
            => Execute(new(this, Role, pressed, baseArgs));
        void Execute(CaptionButtonClickEventArgs e)
            => Execute(e, () => Executed?.Invoke(this, e));
        void Execute(CaptionButtonClickEventArgs e, Action fallback)
        {
            if (_captionButtons != null)
                _captionButtons.ExecuteCaptionButton(this, e);
            else
                fallback?.Invoke();
        }


        bool IsExtended
        {
            get
            {
                var role = Role;
                if (role == CaptionButtonRole.Minimize)
                    return false;
                else if (role == CaptionButtonRole.Maximize)
                    return false;
                else if (role == CaptionButtonRole.FullScreen)
                    return false;
                else if (role == CaptionButtonRole.Close)
                    return false;
                else
                    return true;
            }
        }


        void HostWindow_IsActiveChanged(bool isActive)
        {
            IsHostWindowActive = isActive;
        }


        void UpdateHostWindowState(WindowState windowState)
        {
            HostWindowState = windowState;
        }


        internal event EventHandler<CaptionButtonClickEventArgs> Executed;
    }
}
