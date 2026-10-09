using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    partial class WindowChrome
    {
        public static readonly AttachedProperty<CaptionButtonRolesPair?> OverrideCaptionButtonRolesProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, CaptionButtonRolesPair?>("OverrideCaptionButtonRoles", null);
        public static CaptionButtonRolesPair? GetOverrideCaptionButtonRoles(Window window)
            => window.GetValue(OverrideCaptionButtonRolesProperty);
        public static void SetOverrideCaptionButtonRoles(Window window, CaptionButtonRolesPair? value)
            => window.SetValue(OverrideCaptionButtonRolesProperty, value);


        public static readonly DirectProperty<WindowChrome, IEnumerable<CaptionButtonRole>> LeftCaptionButtonRolesProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, IEnumerable<CaptionButtonRole>>(nameof(LeftCaptionButtonRoles)
                , getter: o => o.LeftCaptionButtonRoles
            );
        IEnumerable<CaptionButtonRole> _leftCaptionButtonRoles = Array.Empty<CaptionButtonRole>();
        public IEnumerable<CaptionButtonRole> LeftCaptionButtonRoles
        {
            get => _leftCaptionButtonRoles;
            private set => SetAndRaise(LeftCaptionButtonRolesProperty, ref _leftCaptionButtonRoles, value);
        }


        public static readonly DirectProperty<WindowChrome, IEnumerable<CaptionButtonRole>> RightCaptionButtonRolesProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, IEnumerable<CaptionButtonRole>>(nameof(RightCaptionButtonRoles)
                , getter: o => o.RightCaptionButtonRoles
            );
        IEnumerable<CaptionButtonRole> _rightCaptionButtonRoles = Array.Empty<CaptionButtonRole>();
        public IEnumerable<CaptionButtonRole> RightCaptionButtonRoles
        {
            get => _rightCaptionButtonRoles;
            private set => SetAndRaise(RightCaptionButtonRolesProperty, ref _rightCaptionButtonRoles, value);
        }




        const bool _DEFAULT_ReserveCaptionArea = true;
        public static readonly StyledProperty<bool> ReserveCaptionAreaProperty
            = AvaloniaProperty.Register<WindowChrome, bool>(nameof(ReserveCaptionArea), _DEFAULT_ReserveCaptionArea);
        public bool ReserveCaptionArea
        {
            get => GetValue(ReserveCaptionAreaProperty);
            set => SetValue(ReserveCaptionAreaProperty, value);
        }


        public static readonly AttachedProperty<double> DefaultCaptionAreaHeightProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, double>("DefaultCaptionAreaHeight", 1d);
        public static double GetDefaultCaptionAreaHeight(Window window)
            => window.GetValue(DefaultCaptionAreaHeightProperty);
        public static void SetDefaultCaptionAreaHeight(Window window, double value)
            => window.SetValue(DefaultCaptionAreaHeightProperty, value);


        public static readonly StyledProperty<double?> CaptionAreaHeightHintProperty
            = AvaloniaProperty.Register<WindowChrome, double?>(nameof(CaptionAreaHeightHint), null);
        public double? CaptionAreaHeightHint
        {
            get => GetValue(CaptionAreaHeightHintProperty);
            set => SetValue(CaptionAreaHeightHintProperty, value);
        }


        const double _DEFAULT_CaptionAreaHeight = 0d;
        public static readonly DirectProperty<WindowChrome, double> CaptionAreaHeightProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, double>(nameof(CaptionAreaHeight)
                , getter: o => o.CaptionAreaHeight
                , unsetValue: _DEFAULT_CaptionAreaHeight
            );
        double _captionAreaHeight = _DEFAULT_CaptionAreaHeight;
        public double CaptionAreaHeight
        {
            get => _captionAreaHeight;
            private set => SetAndRaise(CaptionAreaHeightProperty, ref _captionAreaHeight, value);
        }




        static void AttachedInit()
        {
            AffectsAll(new AvaloniaProperty[]
            {
                LeftCaptionButtonRolesProperty,
                RightCaptionButtonRolesProperty,
                ReserveCaptionAreaProperty,
                DefaultCaptionAreaHeightProperty,
                CaptionAreaHeightHintProperty,
                OverrideCaptionButtonRolesProperty,
            });

            ReserveCaptionAreaProperty.Changed.AddClassHandler<WindowChrome>(ReserveCaptionAreaProperty_Changed);
            DefaultCaptionAreaHeightProperty.Changed.AddClassHandler<Window>(DefaultCaptionAreaHeightProperty_Changed);
            CaptionAreaHeightHintProperty.Changed.AddClassHandler<WindowChrome>(CaptionAreaHeightHintProperty_Changed);

            OverrideCaptionButtonRolesProperty.Changed.AddClassHandler<Window>(OverrideCaptionButtonRolesProperty_Changed);
        }


        static void ReserveCaptionAreaProperty_Changed(WindowChrome chrome, AvaloniaPropertyChangedEventArgs e)
            => chrome.OnCaptionAreaPropertiesChanged(e.GetNewValue<bool>(), chrome.CaptionAreaHeightHint, GetDefaultCaptionAreaHeight(chrome.HostWindow));
        static void DefaultCaptionAreaHeightProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
        {
            WindowChrome chrome = GetChrome(window);
            chrome?.OnCaptionAreaPropertiesChanged(chrome.ReserveCaptionArea, chrome.CaptionAreaHeightHint, e.GetNewValue<double>());
        }

        static void CaptionAreaHeightHintProperty_Changed(WindowChrome chrome, AvaloniaPropertyChangedEventArgs e)
            => chrome.OnCaptionAreaPropertiesChanged(chrome.ReserveCaptionArea, e.GetNewValue<double?>(), GetDefaultCaptionAreaHeight(chrome.HostWindow));


        static void OverrideCaptionButtonRolesProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
        {
            WindowChrome chrome = GetChrome(window);

            if (e.TryGetNewValue(out CaptionButtonRolesPair? newRoles) && newRoles.HasValue)
                chrome.RefreshCaptionButtonRoles(newRoles.Value);
            else
                chrome.RefreshOverrideCaptionButtonRoles();
        }


        void RefreshCaptionAreaProperties()
            => OnCaptionAreaPropertiesChanged(ReserveCaptionArea, CaptionAreaHeight, GetDefaultCaptionAreaHeight(HostWindow));


        void OnCaptionAreaPropertiesChanged(bool reserve, double? heightHint, double defaultHeight)
            => CaptionAreaHeight = reserve
                ? heightHint ?? defaultHeight
                : 0d
            ;


        void RefreshOverrideCaptionButtonRoles()
            => RefreshCaptionButtonRoles(GetOverrideCaptionButtonRoles(HostWindow) ?? PlatformDefaultCaptionButtonRoles);
        void RefreshCaptionButtonRoles(CaptionButtonRolesPair roles)
        {
            LeftCaptionButtonRoles = roles.Left;
            RightCaptionButtonRoles = roles.Right;
        }
    }
}
