using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Converters;
using ReCap.CommonUI.Attached.WindowChrome;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    [TemplatePart(_PART_LeftButtons, typeof(CaptionButtonCluster))]
    [TemplatePart(_PART_RightButtons, typeof(CaptionButtonCluster))]
    public sealed partial class CaptionButtons2
        : CaptionButtons
    {
        const string _PART_LeftButtons = "PART_LeftButtons";
        const string _PART_RightButtons = "PART_RightButtons";

#nullable enable
        internal static readonly DirectProperty<CaptionButtons2, CaptionButtonRoles> LeftCaptionButtonsProperty =
                AvaloniaProperty.RegisterDirect<CaptionButtons2, CaptionButtonRoles>(nameof(LeftCaptionButtons)
                    , s => s.LeftCaptionButtons
                    , (s, v) => s.LeftCaptionButtons = v
                );
        CaptionButtonRoles _leftCaptionButtons = new();
        internal CaptionButtonRoles LeftCaptionButtons
        {
            get => _leftCaptionButtons;
            set => SetAndRaise(LeftCaptionButtonsProperty, ref _leftCaptionButtons, value);
        }


        internal static readonly DirectProperty<CaptionButtons2, CaptionButtonRoles> RightCaptionButtonsProperty =
                AvaloniaProperty.RegisterDirect<CaptionButtons2, CaptionButtonRoles>(nameof(RightCaptionButtons)
                    , s => s.RightCaptionButtons
                    , (s, v) => s.RightCaptionButtons = v
                );
        CaptionButtonRoles _rightCaptionButtons = new();
        internal CaptionButtonRoles RightCaptionButtons
        {
            get => _rightCaptionButtons;
            set => SetAndRaise(RightCaptionButtonsProperty, ref _rightCaptionButtons, value);
        }
#nullable restore


        public static readonly DirectProperty<CaptionButtons2, bool> HasMenuAsLastOnLeftProperty =
                AvaloniaProperty.RegisterDirect<CaptionButtons2, bool>(nameof(HasMenuAsLastOnLeft)
                    , s => s.HasMenuAsLastOnLeft
                );
        bool _hasMenuAsLastOnLeft = false;
        public bool HasMenuAsLastOnLeft
        {
            get => _hasMenuAsLastOnLeft;
            internal set => SetAndRaise(HasMenuAsLastOnLeftProperty, ref _hasMenuAsLastOnLeft, value);
        }




        CaptionButtonCluster _leftButtons = null;
        CaptionButtonCluster _rightButtons = null;
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            _leftButtons?.Detach();
            _leftButtons = null;

            _rightButtons?.Detach();
            _rightButtons = null;


            base.OnApplyTemplate(e);
            var hostWindow = HostWindow;


            _leftButtons = e.NameScope.Find<CaptionButtonCluster>(_PART_LeftButtons);
            _leftButtons?.AttachToLeft(this, hostWindow);

            _rightButtons = e.NameScope.Find<CaptionButtonCluster>(_PART_RightButtons);
            _rightButtons?.AttachToRight(this, hostWindow);
        }


        static bool HasTrailingMenu(CaptionButtonRoles captionButtons, out CaptionButtonRoles modified)
        {
            int count = captionButtons.Count;
            if (captionButtons.Count <= 0)
                goto preserve;

            if (captionButtons.Last() != CaptionButtonRole.Menu)
                goto preserve;

            var stripped = captionButtons.ToList();
            stripped.RemoveAt(count - 1);
            modified = new(stripped);
            return true;

            preserve:
            modified = default;
            return false;
        }







        IDisposable _mainDisposable = null;
        public override void Attach(Window hostWindow)
        {
            base.Attach(hostWindow);
            
            _mainDisposable = new CompositeDisposable()
            {
                Bind(RightCaptionButtonsProperty, hostWindow[!WindowChromeAddon.RightCaptionButtonsProperty]),
                this
                    .GetObservable(IsVisibleProperty)
                    .Subscribe(isVisible =>
                    {
                        _leftButtons?.RefreshSize();
                        _rightButtons?.RefreshSize();
                    })
                ,
                hostWindow.GetObservable(WindowChromeAddon.LeftCaptionButtonsProperty)
                    .Subscribe(roles =>
                    {
                        if (_prevRoles != null)
                            _prevRoles.CollectionChanged -= LeftCaptionButtonRoles_CollectionChanged;

                        roles.CollectionChanged += LeftCaptionButtonRoles_CollectionChanged;
                        _prevRoles = roles;
                        OnLeftCaptionButtonRolesUpdated(roles);
                    })
                ,
            };
        }


        CaptionButtonRoles _prevRoles = null;
        void LeftCaptionButtonRoles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => OnLeftCaptionButtonRolesUpdated((CaptionButtonRoles)sender);
        void OnLeftCaptionButtonRolesUpdated(CaptionButtonRoles roles)
        {
            if (HasTrailingMenu(roles, out CaptionButtonRoles modified))
            {
                HasMenuAsLastOnLeft = true;
                LeftCaptionButtons = modified;
            }
            else
            {
                HasMenuAsLastOnLeft = false;
                LeftCaptionButtons = roles;
            }
        }


        public override void Detach()
        {
            Window oldHost = HostWindow;
            base.Detach();

            _mainDisposable?.Dispose();

            if (oldHost == null)
                return;

            WindowChromeAddon.SetLeftCaptionButtonsWidth(oldHost, 0d);
            WindowChromeAddon.SetRightCaptionButtonsWidth(oldHost, 0d);
        }




        internal void ExecuteCaptionButton(CaptionButton button, CaptionButtonClickEventArgs e)
        {
            Window window = HostWindow;
            if (window == null)
                return;

            CaptionButtonRole role = button.Role;
            if (!IsClicked(e))
            {
                WindowChromeAddon.ExecuteExtendedCaptionButton(window, e);
                return;
            }


            switch (role)
            {
                case CaptionButtonRole.Close:
                {
                    window.Close();
                    break;
                }
                case CaptionButtonRole.Maximize:
                {
                    OnRestore();
                    break;
                }
                case CaptionButtonRole.Minimize:
                {
                    OnMinimize();
                    break;
                }
                case CaptionButtonRole.FullScreen:
                {
                    OnToggleFullScreen();
                    break;
                }
            }
        }
        static bool IsClicked(CaptionButtonClickEventArgs e)
            => e.PointerArgs.RoutedEvent == Button.ClickEvent;


        sealed class CaptionButtonsRemoveTrailingMenuConverter
            : IValueConverter
        {
            public static readonly CaptionButtonsRemoveTrailingMenuConverter Instance = new();
            private CaptionButtonsRemoveTrailingMenuConverter()
            {}
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is not CaptionButtonRoles roles)
                    return BindingOperations.DoNothing;

                if (HasTrailingMenu(roles, out CaptionButtonRoles modified))
                    return modified;

                return roles;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }
    }
}