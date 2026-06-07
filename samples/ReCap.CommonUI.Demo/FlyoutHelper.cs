using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace ReCap.CommonUI.Demo
{
    /*
    public enum TargetFlyout
        : byte
    {
        None        = 0b00000000,
        RightClick  = 0b00000001,
        DoubleClick = 0b00000010,
    }
    */


    [Flags]
    public enum FlyoutShowMode
        : byte
    {
        None        = 0b00000000,
        RightClick  = 0b00000001,
        DoubleClick = 0b00000010,
    }




    public sealed class FlyoutHelper
        : AvaloniaObject
    {
        public static readonly AttachedProperty<FlyoutShowMode> ShowAttachedFlyoutProperty =
            AvaloniaProperty.RegisterAttached<FlyoutHelper, Control, FlyoutShowMode>("ShowAttachedFlyout", defaultValue: FlyoutShowMode.None);
        public static FlyoutShowMode GetShowAttachedFlyout(Control control)
            => control.GetValue(ShowAttachedFlyoutProperty);
        public static void SetShowAttachedFlyout(Control control, FlyoutShowMode value)
            => control.SetValue(ShowAttachedFlyoutProperty, value);


#if SHOW_CONTEXT_FLYOUT
        public static readonly AttachedProperty<FlyoutShowMode> ShowContextFlyoutProperty =
            AvaloniaProperty.RegisterAttached<FlyoutHelper, Control, FlyoutShowMode>("ShowContextFlyout", defaultValue: FlyoutShowMode.None);
        public static FlyoutShowMode GetShowContextFlyout(Control control)
            => control.GetValue(ShowContextFlyoutProperty);
        public static void SetShowContextFlyout(Control control, FlyoutShowMode value)
            => control.SetValue(ShowContextFlyoutProperty, value);
#endif




        static readonly ShowFlyoutPropertyHelper<Control, FlyoutBase> _showAttachedFlyoutHelper = null;
#if SHOW_CONTEXT_FLYOUT
        static readonly ShowFlyoutPropertyHelper<Control, FlyoutBase> _showContextFlyoutHelper = null;
#endif
        static FlyoutHelper()
        {
            _showAttachedFlyoutHelper = new(ShowAttachedFlyoutProperty
                , FlyoutBase.GetAttachedFlyout
                , FlyoutBase.SetAttachedFlyout
                , FlyoutBase.ShowAttachedFlyout
            );
#if SHOW_CONTEXT_FLYOUT
            Button.ContextFlyoutProperty
            _showContextFlyoutHelper = new(ShowContextFlyoutProperty
                , c => c.ContextFlyout
                , (c, f) => c.ContextFlyout = f
                , c => c.context)
#endif
        }


        class ShowFlyoutPropertyHelper<C, F>
            where C
                : Control
            where F
                : FlyoutBase
        {
            readonly Func<C, F> _flyoutGetter;
            readonly Action<C, F> _flyoutSetter;
            readonly Action<C> _flyoutShower;
            public ShowFlyoutPropertyHelper(AttachedProperty<FlyoutShowMode> property, Func<C, F> flyoutGetter, Action<C, F> flyoutSetter, Action<C> flyoutShower)
            {
                _flyoutGetter = flyoutGetter;
                _flyoutSetter = flyoutSetter;
                _flyoutShower = flyoutShower;
                property.Changed.AddClassHandler<C>(Property_Changed);
            }


            public F GetFlyout(C control)
                => _flyoutGetter(control);


            public void SetFlyout(C control, F value)
                => _flyoutSetter(control, value);

            public void ShowFlyout(C control)
                => _flyoutShower(control);


            public bool TryGetSenderControl(object sender, out C control)
            {
                if (sender is C ctrl)
                {
                    control = ctrl;
                    return control != null;
                }
                else
                {
                    control = null;
                    return false;
                }
            }


            public bool TryGetFlyout(C control, out F flyout)
            {
                flyout = GetFlyout(control);
                return flyout != null;
            }




            void Property_Changed(C control, AvaloniaPropertyChangedEventArgs e)
            {
                FlyoutShowMode mode = e.GetNewValue<FlyoutShowMode>();
                if (mode.HasFlag(FlyoutShowMode.RightClick))
                    control.PointerReleased += Control_PointerReleased_RightMouseButton;
                else 
                    control.PointerReleased -= Control_PointerReleased_RightMouseButton;


                if (mode.HasFlag(FlyoutShowMode.DoubleClick))
                    control.DoubleTapped += Control_DoubleTapped;
            }


            void Control_PointerReleased_RightMouseButton(object sender, PointerReleasedEventArgs e)
            {
                if (!e.Properties.IsRightButtonPressed)
                    return;

                if (TryGetSenderControl(sender, out C control))
                    ShowFlyout(control);
            }


            void Control_DoubleTapped(object sender, TappedEventArgs e)
            {
                if (TryGetSenderControl(sender, out C control))
                    ShowFlyout(control);
            }
        }
    }
}