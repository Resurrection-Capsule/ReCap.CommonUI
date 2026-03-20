using System;
using System.Collections.Specialized;
using System.Diagnostics;
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
using DynamicData;
using ReCap.CommonUI.Attached.WindowChrome;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    [TemplatePart(_PART_LeftButtons, typeof(ItemsControl))]
    [TemplatePart(_PART_RightButtons, typeof(ItemsControl))]
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
                    //, (s, v) => s.HasMenuAsLastOnLeft = v
                );
        bool _hasMenuAsLastOnLeft = false;
        public bool HasMenuAsLastOnLeft
        {
            get => _hasMenuAsLastOnLeft;
            internal set => SetAndRaise(HasMenuAsLastOnLeftProperty, ref _hasMenuAsLastOnLeft, value);
        }




#if NO
        static CaptionButtons2()
        {
            LeftCaptionButtonsProperty.Changed.AddClassHandler<CaptionButtons2>(LeftCaptionButtonsProperty_Changed);
        }

        private static void LeftCaptionButtonsProperty_Changed(CaptionButtons2 buttons, AvaloniaPropertyChangedEventArgs args)
        {
            Debug.WriteLine($"{nameof(LeftCaptionButtons)} changed!");
            var newRoles = args.GetNewValue<CaptionButtonRoles>();
            if ((newRoles.Count > 0) && (newRoles.Last() == CaptionButtonRole.Menu))
                Debug.WriteLine($"    => ENDS WITH {nameof(CaptionButtonRole.Menu)}!");
            else
                Debug.WriteLine($"    => {newRoles}");
            
        }


#endif
        ItemsControl _leftButtons = null;
        IDisposable _leftButtonsDisposable = null;
        ItemsControl _rightButtons = null;
        IDisposable _rightButtonsDisposable = null;
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            _leftButtonsDisposable?.Dispose();
            _leftButtonsDisposable = null;
            if (_leftButtons != null)
            {
                _leftButtons.SizeChanged -= LeftButtons_SizeChanged;
                _leftButtons = null;
            }

            _rightButtonsDisposable?.Dispose();
            _rightButtonsDisposable = null;
            if (_rightButtons != null)
            {
                _rightButtons.SizeChanged -= RightButtons_SizeChanged;
                _rightButtons = null;
            }


            base.OnApplyTemplate(e);
            if (e.NameScope.TryFind(_PART_LeftButtons, out ItemsControl leftButtons))
            {
                _leftButtons = leftButtons;
                _leftButtons.SizeChanged += LeftButtons_SizeChanged;
                _leftButtonsDisposable = new CompositeDisposable()
                {
                    _leftButtons
                        .GetObservable(IsVisibleProperty)
                        .Subscribe(isVisible => RefreshLeftButtonsSize(0d))
                    ,
                    _leftButtons.Bind(ItemsControl.ItemsSourceProperty, this[!LeftCaptionButtonsProperty])
                    /*
                    this
                        .GetObservable(LeftCaptionButtonsProperty)
                        .Select(r => HasTrailingMenu(r, out CaptionButtonRoles r2) ? r2 : r)
                        .ToBinding()
                    */
                    ,
                };
            }

            if (e.NameScope.TryFind(_PART_RightButtons, out ItemsControl rightButtons))
            {
                _rightButtons = rightButtons;
                _rightButtons.SizeChanged += RightButtons_SizeChanged;
                _rightButtonsDisposable = new CompositeDisposable()
                {
                    _rightButtons
                        .GetObservable(IsVisibleProperty)
                        .Subscribe(isVisible => RefreshRightButtonsSize(0d))
                    ,
                    _rightButtons.Bind(ItemsControl.ItemsSourceProperty, this[!RightCaptionButtonsProperty]),
                };
            }

            RefreshLeftButtonsSize();
            RefreshRightButtonsSize();
            /*
            Debug.WriteLine($"{nameof(LeftCaptionButtons)}: {LeftCaptionButtons}");
            Debug.WriteLine($"{nameof(RightCaptionButtons)}: {RightCaptionButtons}");
            */
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
#nullable enable
#nullable restore


        void LeftButtons_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ComputeNewCaptionButtonsSize(sender, e, out Window window, out double newWidth))
                WindowChromeAddon.SetLeftCaptionButtonsWidth(window, newWidth);
        }


        void RightButtons_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ComputeNewCaptionButtonsSize(sender, e, out Window window, out double newWidth))
                WindowChromeAddon.SetRightCaptionButtonsWidth(window, newWidth);
        }


        const double _DEFAULT_RefreshButtonsSize_fallback = -1d;
        void RefreshLeftButtonsSize(double fallback = _DEFAULT_RefreshButtonsSize_fallback)
            => RefreshOneButtonsSize(_rightButtons, WindowChromeAddon.SetRightCaptionButtonsWidth, fallback);

        void RefreshRightButtonsSize(double fallback = _DEFAULT_RefreshButtonsSize_fallback)
            => RefreshOneButtonsSize(_rightButtons, WindowChromeAddon.SetRightCaptionButtonsWidth, fallback);


        void RefreshOneButtonsSize(ItemsControl buttons, Action<Window, double> setter, double fallback)
            => RefreshOneButtonsSize(buttons, buttons?.Bounds.Width ?? 0d, setter, fallback);
        void RefreshOneButtonsSize(ItemsControl buttons, double newSize, Action<Window, double> setter, double fallback)
        {
            if (ComputeNewCaptionButtonsSize(buttons, newSize, out Window window, out double newWidth))
                setter(window, newWidth);
            else if ((fallback >= 0d) && (window != null))
                setter(window, fallback);
        }




        bool ComputeNewCaptionButtonsSize(object sender, SizeChangedEventArgs e, out Window window, out double result)
        {
            if (sender is ItemsControl itemsCtl)
                return ComputeNewCaptionButtonsSize(itemsCtl, e.NewSize.Width, out window, out result);

            window = default;
            result = default;
            return false;
        }


        bool ComputeNewCaptionButtonsSize(ItemsControl itemsCtl, double itemsCtlWidth, out Window window, out double result)
        {
            window = HostWindow;
            if (window == null)
                goto fail;
            else if (itemsCtl == null)
                goto fail;
            else if (!itemsCtl.IsEffectivelyVisible)
            {
                result = 0d;
                return true;
            }


            result = itemsCtlWidth;
            return true; //result != itemsCtl.Bounds.Width;


            fail:
            result = 0d;
            return false;
        }



        IDisposable _mainDisposable = null;
        public override void Attach(Window hostWindow)
        {
            base.Attach(hostWindow);
            
            _mainDisposable = new CompositeDisposable()
            {
                //Bind(LeftCaptionButtonsProperty, hostWindow[!WindowChromeAddon.LeftCaptionButtonsProperty]),
                /*
                Bind(LeftCaptionButtonsProperty,
                        new Binding()
                        {
                            Source = this,
                            Path = nameof(LeftCaptionButtons),
                            Converter = CaptionButtonsRemoveTrailingMenuConverter.Instance,
                            Mode = BindingMode.OneWay,
                        }
                ),
                */
                Bind(RightCaptionButtonsProperty, hostWindow[!WindowChromeAddon.RightCaptionButtonsProperty]),
                this
                    .GetObservable(IsVisibleProperty)
                    .Subscribe(isVisible =>
                    {
                        RefreshLeftButtonsSize(0d);
                        RefreshRightButtonsSize(0d);
                    })
                ,
                hostWindow.GetObservable(WindowChromeAddon.LeftCaptionButtonsProperty)
                    /*
                    .Select(roles => HasTrailingMenu(roles, out CaptionButtonRoles r2)
                        ? r2
                        : roles
                    )
                    .Subscribe()
                    */
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
            //=> e.Pressed == null; // && (e.PointerArgs == null); //e.PointerArgs.Properties.IsLeftButtonPressed;
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