using System;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Media;
using Avalonia.VisualTree;
using ReCap.CommonUI.Attached.WindowChrome;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    [PseudoClasses(_PSEUD_ROLE_ACTIVE)]
    public sealed partial class CaptionButton
        : Button
    {
        const string _PSEUD_ROLE_ACTIVE = ":roleactive";


        const bool _DEFAULT_IsHostWindowActive = true;
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


        const WindowState _DEFAULT_HostWindowState = WindowState.Normal;
        public static readonly DirectProperty<CaptionButton, WindowState> HostWindowStateProperty
            = AvaloniaProperty.RegisterDirect<CaptionButton, WindowState>(nameof(HostWindowState)
                , getter: o => o.HostWindowState
                , unsetValue: _DEFAULT_HostWindowState
            );
        WindowState _hostWindowState = _DEFAULT_HostWindowState;
        public WindowState HostWindowState
        {
            get => _hostWindowState;
            private set => SetAndRaise(HostWindowStateProperty, ref _hostWindowState, value);
        }


        internal const CaptionButtonRole DEFAULT_Role = (CaptionButtonRole)(-1);
        public static readonly DirectProperty<CaptionButton, CaptionButtonRole> RoleProperty
            = AvaloniaProperty.RegisterDirect<CaptionButton, CaptionButtonRole>(nameof(Role)
                , getter: o => o.Role
                , setter: (o, v) => o.Role = v
                , unsetValue: DEFAULT_Role
            );
        CaptionButtonRole _role = DEFAULT_Role;
        public CaptionButtonRole Role
        {
            get => _role;
            set => SetAndRaise(RoleProperty, ref _role, value);
        }


        public static readonly StyledProperty<string> CurrentRoleTitleProperty
            = AvaloniaProperty.Register<CaptionButton, string>(nameof(CurrentRoleTitle), null);
        public string CurrentRoleTitle
        {
            get => GetValue(CurrentRoleTitleProperty);
            set => SetValue(CurrentRoleTitleProperty, value);
        }


        public static readonly StyledProperty<CaptionButtonGlyphs> GlyphsProperty
            = AvaloniaProperty.Register<CaptionButton, CaptionButtonGlyphs>(nameof(Glyphs), null);
        public CaptionButtonGlyphs Glyphs
        {
            get => GetValue(GlyphsProperty);
            set => SetValue(GlyphsProperty, value);
        }


        const Geometry _DEFAULT_CurrentGlyph = null;
        public static readonly DirectProperty<CaptionButton, Geometry> CurrentGlyphProperty
            = AvaloniaProperty.RegisterDirect<CaptionButton, Geometry>(nameof(CurrentGlyph)
                , getter: o => o.CurrentGlyph
                , unsetValue: _DEFAULT_CurrentGlyph
            );
        Geometry _currentGlyph = _DEFAULT_CurrentGlyph;
        public Geometry CurrentGlyph
        {
            get => _currentGlyph;
            private set => SetAndRaise(CurrentGlyphProperty, ref _currentGlyph, value);
        }


        const bool _DEFAULT_UseManagedToolTip = false;
        public static readonly DirectProperty<CaptionButton, bool> UseManagedToolTipProperty
            = AvaloniaProperty.RegisterDirect<CaptionButton, bool>(nameof(UseManagedToolTip)
                , getter: o => o.UseManagedToolTip
                , setter: (o, v) => o.UseManagedToolTip = v
                , unsetValue: _DEFAULT_UseManagedToolTip
            );
        bool _useManagedToolTip = _DEFAULT_UseManagedToolTip;
        public bool UseManagedToolTip
        {
            get => _useManagedToolTip;
            set => SetAndRaise(UseManagedToolTipProperty, ref _useManagedToolTip, value);
        }


        const bool _DEFAULT_IsRoleCheckable = false;
        public static readonly DirectProperty<CaptionButton, bool> IsRoleCheckableProperty
            = AvaloniaProperty.RegisterDirect<CaptionButton, bool>(nameof(IsRoleCheckable)
                , getter: o => o.IsRoleCheckable
                , unsetValue: _DEFAULT_IsRoleCheckable
            );
        bool _isRoleCheckable = _DEFAULT_IsRoleCheckable;
        public bool IsRoleCheckable
        {
            get => _isRoleCheckable;
            internal set => SetAndRaise(IsRoleCheckableProperty, ref _isRoleCheckable, value);
        }


        internal static readonly StyledProperty<bool> IsRoleCheckedProperty
            = AvaloniaProperty.Register<CaptionButton, bool>(nameof(IsRoleChecked), false, coerce: IsRoleCheckedProperty_Coerce);
        static bool IsRoleCheckedProperty_Coerce(AvaloniaObject o, bool value)
            => value && ((CaptionButton)o).IsRoleCheckable;
        public bool IsRoleChecked
        {
            get => GetValue(IsRoleCheckedProperty);
            internal set => SetValue(IsRoleCheckedProperty, value);
        }


        Window _hostWindow = null;
        internal Window HostWindow
        {
            get => _hostWindow;
            private set => _hostWindow = value;
        }


        internal CompositeDisposable BindingDisposables = null;
        internal void DisposeBindingDisposables()
        {
            if (BindingDisposables == null)
                return;
            BindingDisposables.Dispose();
            BindingDisposables = null;
        }




        static CaptionButton()
        {
            FocusableProperty.OverrideDefaultValue<CaptionButton>(false);
            IsTabStopProperty.OverrideDefaultValue<CaptionButton>(false);

            AffectsRender<CaptionButton>(new AvaloniaProperty[]
            {
                IsHostWindowActiveProperty,
                HostWindowStateProperty,
            });

            IsRoleCheckableProperty.Changed.AddClassHandler<CaptionButton>(IsRoleCheckableProperty_Changed);
            IsRoleCheckedProperty.Changed.AddClassHandler<CaptionButton>(IsRoleCheckedProperty_Changed);
            RoleProperty.Changed.AddClassHandler<CaptionButton>(RoleProperty_Changed);
            GlyphsProperty.Changed.AddClassHandler<CaptionButton>(GlyphsProperty_Changed);
            UseManagedToolTipProperty.Changed.AddClassHandler<CaptionButton>(UseManagedToolTipProperty_Changed);

            var props = new AvaloniaProperty[]
            {
                RoleProperty,
                CurrentGlyphProperty,
            };

            AffectsArrange<CaptionButton>(props);
            AffectsMeasure<CaptionButton>(props);
            AffectsRender<CaptionButton>(props);
        }


        static void IsRoleCheckableProperty_Changed(CaptionButton button, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.GetNewValue<bool>())
                return;
            else
                button.IsRoleChecked = false;
        }


        static void IsRoleCheckedProperty_Changed(CaptionButton button, AvaloniaPropertyChangedEventArgs e)
        {
            bool value = e.GetNewValue<bool>();

            if (button.IsRoleCheckable)
                button.RefreshGlyph(button.Role, value);
            else if (value)
                button.IsRoleChecked = false;

            button.PseudoClasses.Set(_PSEUD_ROLE_ACTIVE, button.IsRoleChecked);
        }


        static void GlyphsProperty_Changed(CaptionButton button, AvaloniaPropertyChangedEventArgs args)
        {
            (CaptionButtonGlyphs oldGlyphs, CaptionButtonGlyphs newGlyphs) = args.GetOldAndNewValue<CaptionButtonGlyphs>();
            button.OnGlyphsChanged(oldGlyphs, newGlyphs);
        }
        void OnGlyphsChanged(CaptionButtonGlyphs oldGlyphs, CaptionButtonGlyphs newGlyphs)
        {
            if (oldGlyphs != null)
                oldGlyphs.AnyGlyphGeometryPropertyChanged -= Glyphs_AnyGlyphGeometryPropertyChanged;

            if (newGlyphs != null)
                newGlyphs.AnyGlyphGeometryPropertyChanged += Glyphs_AnyGlyphGeometryPropertyChanged;

            Refresh();
        }
        void Glyphs_AnyGlyphGeometryPropertyChanged(object sender, GlyphGeometryPropertyChangeEventArgs e)
        {
            var role = Role;
            if (role == e.Role)
                Refresh(role);
        }




        static void UseManagedToolTipProperty_Changed(CaptionButton button, AvaloniaPropertyChangedEventArgs args)
            => button.Refresh();




        static void RoleProperty_Changed(CaptionButton button, AvaloniaPropertyChangedEventArgs e)
            => button.Refresh(e.GetNewValue<CaptionButtonRole>());


        CompositeDisposable _contentBindingDisposable = null;
        void Refresh()
            => Refresh(Role, IsRoleChecked);
        void Refresh(CaptionButtonRole role)
            => Refresh(role, IsRoleChecked);
        void Refresh(CaptionButtonRole role, bool roleChecked)
        {
            _contentBindingDisposable?.Dispose();
            _contentBindingDisposable = null;

            RefreshGlyph(role, roleChecked);
            IsRoleCheckable = ManagedWindowChromeEnumsHelper.ACTIVATABLE_ROLES.Contains(role);

            if (!role.TryGetCaptionButtonRoleTitleDynamicResource(roleChecked, out DynamicResourceExtension roleTitleDynamicResource))
                return;

            _contentBindingDisposable = new()
            {
                Bind(CurrentRoleTitleProperty, roleTitleDynamicResource),
                Bind(ContentProperty, _currentRoleTitleBinding),
            };

            if (UseManagedToolTip)
                _contentBindingDisposable.Add(Bind(ToolTip.TipProperty, _currentRoleTitleBinding));
        }


        void RefreshGlyph(CaptionButtonRole role, bool roleChecked)
            => CurrentGlyph = Glyphs?.GetGlyph(role, roleChecked);



        readonly IBinding _currentRoleTitleBinding;
        public CaptionButton()
            : base()
        {
            _currentRoleTitleBinding = this[!CurrentRoleTitleProperty];
            Refresh();
        }


        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnAttachedToLogicalTree(e);
            Refresh();
        }


        CaptionButtonCluster _hostCluster = null;
        IDisposable _visualTreeDisposable = null;
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            Refresh();

            var hostCluster = this.FindAncestorOfType<CaptionButtonCluster>();
            if (hostCluster == null)
                goto fail;

            if (e.Root is not Window hostWindow)
                goto fail;

            HostWindow = hostWindow;
            _hostCluster = hostCluster;
            _visualTreeDisposable = new CompositeDisposable()
            {
                hostWindow.GetObservable(WindowBase.IsActiveProperty)
                    .Subscribe(HostWindow_IsActiveChanged)
                ,
                hostWindow.GetObservable(Window.WindowStateProperty)
                    .Subscribe(HostWindow_WindowStateChanged)
                ,
                Disposable.Create(() =>
                {
                    hostWindow = null;
                    _hostCluster = null;
                    ResetHostWindowProperties();
                }),
            };
            IsHostWindowActive = hostWindow.IsActive;
            HostWindowState = hostWindow.WindowState;
            return;

            fail:
            ResetHostWindowProperties();
        }


        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            _visualTreeDisposable?.Dispose();
            _visualTreeDisposable = null;
        }


        void ResetHostWindowProperties()
        {
            IsHostWindowActive = _DEFAULT_IsHostWindowActive;
            HostWindowState = _DEFAULT_HostWindowState;
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
            if (_hostCluster != null)
                _hostCluster.ExecuteCaptionButton(this, e);
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
            Refresh();
        }


        void HostWindow_WindowStateChanged(WindowState windowState)
        {
            HostWindowState = windowState;
            Refresh();
        }


        internal event EventHandler<CaptionButtonClickEventArgs> Executed;
    }
}
