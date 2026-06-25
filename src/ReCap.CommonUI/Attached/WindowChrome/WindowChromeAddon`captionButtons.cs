using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    partial class WindowChromeAddon
    {
#region Caption buttons ordering
        public static readonly AttachedProperty<CaptionButtonRoles> LeftCaptionButtonsProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, CaptionButtonRoles>("LeftCaptionButtons", PlatformDefaultCaptionButtons.Left);
        public static CaptionButtonRoles GetLeftCaptionButtons(Window control)
            => control.GetValue(LeftCaptionButtonsProperty);
        public static void SetLeftCaptionButtons(Window control, CaptionButtonRoles value)
            => control.SetValue(LeftCaptionButtonsProperty, value);


        public static readonly AttachedProperty<CaptionButtonRoles> RightCaptionButtonsProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, CaptionButtonRoles>("RightCaptionButtons", PlatformDefaultCaptionButtons.Right);
        public static CaptionButtonRoles GetRightCaptionButtons(Window control)
            => control.GetValue(RightCaptionButtonsProperty);
        public static void SetRightCaptionButtons(Window control, CaptionButtonRoles value)
            => control.SetValue(RightCaptionButtonsProperty, value);
#endregion




#region Bindable reference properties
        public static readonly AttachedProperty<double> LeftCaptionButtonsWidthProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, double>("LeftCaptionButtonsWidth", 0.0f);
        public static double GetLeftCaptionButtonsWidth(Window control)
            => control.GetValue(LeftCaptionButtonsWidthProperty);
        internal static void SetLeftCaptionButtonsWidth(Window control, double value)
            => control.SetValue(LeftCaptionButtonsWidthProperty, value);


        public static readonly AttachedProperty<double> RightCaptionButtonsWidthProperty =
            AvaloniaProperty.RegisterAttached<WindowChromeAddon, Window, double>("RightCaptionButtonsWidth", 0.0f);
        public static double GetRightCaptionButtonsWidth(Window control)
            => control.GetValue(RightCaptionButtonsWidthProperty);
        internal static void SetRightCaptionButtonsWidth(Window control, double value)
            => control.SetValue(RightCaptionButtonsWidthProperty, value);
#endregion Properties



        public static CaptionButtonRolesPair PlatformDefaultCaptionButtons
        {
            get => _IMPL.DefaultCaptionButtons;
        }



        static void CaptionButtonsInit()
        {
            Dbg.DoChangedDebugOutput<Window>(LeftCaptionButtonsProperty, RightCaptionButtonsProperty);
        }


        internal static void ExecuteExtendedCaptionButton(Window window, CaptionButtonClickEventArgs e)
            => _IMPL.ExecuteExtendedCaptionButton(window, e);


        public static IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get => _IMPL.ValidCaptionButtonRoles;
        }
    }



    internal sealed class CaptionButtonClickEventArgs
        : EventArgs
    {
        public Visual Visual
        {
            get;
            init;
        }

        public CaptionButtonRole Role
        {
            get;
            init;
        }

        public PointerEventArgs PointerArgs
        {
            get;
            init;
        }

        public MouseButton MouseButton
        {
            get;
            init;
        }

        public bool Pressed
        {
            get;
            init;
        }

        public int ClickCount
        {
            get;
            init;
        }



        public bool IsDoubleClick
        {
            get => ClickCount > 1;
        }


        public CaptionButtonClickEventArgs(Visual visual, CaptionButtonRole role, bool pressed, PointerEventArgs baseArgs)
        {
            Visual = visual;
            Role = role;
            Pressed = pressed;
            MouseButton = GetMouseButton(baseArgs);


            PointerPointProperties basePtProps = baseArgs.Properties;
            KeyModifiers baseKeyModifiers = baseArgs.KeyModifiers;
            RawInputModifiers rawInputModifiers = RawInputModifiers.None;

            {
                if (basePtProps.IsLeftButtonPressed)
                    rawInputModifiers |= RawInputModifiers.LeftMouseButton;

                if (basePtProps.IsRightButtonPressed)
                    rawInputModifiers |= RawInputModifiers.RightMouseButton;

                if (basePtProps.IsMiddleButtonPressed)
                    rawInputModifiers |= RawInputModifiers.MiddleMouseButton;

                if (basePtProps.IsXButton1Pressed)
                    rawInputModifiers |= RawInputModifiers.XButton1MouseButton;

                if (basePtProps.IsXButton2Pressed)
                    rawInputModifiers |= RawInputModifiers.XButton2MouseButton;

                if (basePtProps.IsBarrelButtonPressed)
                    rawInputModifiers |= RawInputModifiers.PenBarrelButton;
            }

            {
                if (baseKeyModifiers.HasFlag(KeyModifiers.Alt))
                    rawInputModifiers |= RawInputModifiers.Alt;

                if (baseKeyModifiers.HasFlag(KeyModifiers.Control))
                    rawInputModifiers |= RawInputModifiers.Control;

                if (baseKeyModifiers.HasFlag(KeyModifiers.Shift))
                    rawInputModifiers |= RawInputModifiers.Shift;

                if (baseKeyModifiers.HasFlag(KeyModifiers.Meta))
                    rawInputModifiers |= RawInputModifiers.Meta;
            }


            PointerPointProperties ptProps = new(
                modifiers: rawInputModifiers,
                basePtProps.PointerUpdateKind,
                twist: basePtProps.Twist,
                pressure: basePtProps.Pressure,
                xTilt: basePtProps.XTilt,
                yTilt: basePtProps.YTilt
            );

#nullable enable
            Visual? root = (Visual?)visual.GetVisualRoot();
#nullable restore
            PointerArgs = new PointerEventArgs(
                routedEvent: baseArgs.RoutedEvent,
                source: baseArgs.Source,
                pointer: baseArgs.Pointer,
                rootVisual: root,
                rootVisualPosition: baseArgs.GetPosition(root),
                timestamp: baseArgs.Timestamp,
                properties: ptProps,
                modifiers: baseKeyModifiers
            );


            if (baseArgs is PointerPressedEventArgs pressedArgs)
                ClickCount = pressedArgs.ClickCount;
            else
                ClickCount = -1;
        }


        static MouseButton GetMouseButton(PointerEventArgs e)
            => (e != null)
                ? GetMouseButton(e.Properties)
                : MouseButton.None
            ;


        static MouseButton GetMouseButton(PointerPointProperties props)
        {
            if (props != null)
            {
                if (props.IsLeftButtonPressed)
                    return MouseButton.Left;
                else if (props.IsRightButtonPressed)
                    return MouseButton.Right;
                else if (props.IsMiddleButtonPressed)
                    return MouseButton.Middle;
                else if (props.IsXButton1Pressed)
                    return MouseButton.XButton1;
                else if (props.IsXButton2Pressed)
                    return MouseButton.XButton2;
                /*
                // MouseButton enum has nothing for...whatever a 'BarrelButton' is
                else if (props.IsBarrelButtonPressed)
                    return MouseButton.Barrel;
                */
            }

            return MouseButton.None;
        }
    }
}
