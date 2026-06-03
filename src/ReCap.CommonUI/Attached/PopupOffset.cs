using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace ReCap.CommonUI
{
    public class PopupOffset
        : AvaloniaObject
    {
        public static readonly AttachedProperty<bool> UseInversionProperty =
            AvaloniaProperty.RegisterAttached<ScrollBarInset, Popup, bool>("UseInversion", false);
        public static bool GetUseInversion(Popup control)
            => control.GetValue(UseInversionProperty);
        public static void SetUseInversion(Popup control, bool value)
            => control.SetValue(UseInversionProperty, value);


/*
        public static readonly AttachedProperty<Control> OffsetRelativeToProperty =
            AvaloniaProperty.RegisterAttached<ScrollBarInset, Popup, Control>("OffsetRelativeTo", null);
        public static Control GetOffsetRelativeTo(Popup control)
            => control.GetValue(OffsetRelativeToProperty);
        public static void SetOffsetRelativeTo(Popup control, Control value)
            => control.SetValue(OffsetRelativeToProperty, value);
*/


        public static readonly AttachedProperty<double> VerticalDistanceProperty =
            AvaloniaProperty.RegisterAttached<ScrollBarInset, Popup, double>("VerticalDistance", 0.0);
        public static double GetVerticalDistance(Popup control)
            => control.GetValue(VerticalDistanceProperty);
        public static void SetVerticalDistance(Popup control, double value)
            => control.SetValue(VerticalDistanceProperty, value);


        public static readonly AttachedProperty<double> HorizontalDistanceProperty =
            AvaloniaProperty.RegisterAttached<ScrollBarInset, Popup, double>("HorizontalDistance", 0.0);
        public static double GetHorizontalDistance(Popup control)
            => control.GetValue(HorizontalDistanceProperty);
        public static void SetHorizontalDistance(Popup control, double value)
            => control.SetValue(HorizontalDistanceProperty, value);


        public static readonly AttachedProperty<bool> IsHorizontalInvertedProperty =
            AvaloniaProperty.RegisterAttached<ScrollBarInset, Popup, bool>("IsHorizontalInverted", false);
        public static bool GetIsHorizontalInverted(Popup control)
            => control.GetValue(IsHorizontalInvertedProperty);
        static void SetIsHorizontalInverted(Popup control, bool value)
            => control.SetValue(IsHorizontalInvertedProperty, value);


        public static readonly AttachedProperty<bool> IsVerticalInvertedProperty =
            AvaloniaProperty.RegisterAttached<ScrollBarInset, Popup, bool>("IsVerticalInverted", false);
        public static bool GetIsVerticalInverted(Popup control)
            => control.GetValue(IsVerticalInvertedProperty);
        static void SetIsVerticalInverted(Popup control, bool value)
            => control.SetValue(IsVerticalInvertedProperty, value);




        static PopupOffset()
        {
            UseInversionProperty.Changed.AddClassHandler<Popup>(UseInversionProperty_Changed);
        }

        static void UseInversionProperty_Changed(Popup popup, AvaloniaPropertyChangedEventArgs e)
        {
            bool useInversion = e.GetNewValue<bool>();
            if (useInversion)
            {
                popup.Opened += Popup_Opened;
            }
            else
            {
                popup.Opened -= Popup_Opened;
                SetIsHorizontalInverted(popup, false);
                SetIsVerticalInverted(popup, false);
            }
        }


        static void Popup_Opened(object sender, EventArgs e)
        {
            //Debug.WriteLine($"{nameof(PopupOffset)}.{nameof(Popup_Opened)}({sender}, {e})");
            Popup popup = (Popup)sender;
            IPopupHost host = popup.Host;
            if (host == null)
            {
                //Debug.WriteLine($"{nameof(popup.Holding)} == null");
                return;
            }

            double hOffset = popup.HorizontalOffset;
            double vOffset = popup.VerticalOffset;

            popup.HorizontalOffset = 0d;
            popup.VerticalOffset = 0d;
            popup.UpdateLayout();
            Visual placementTarget = popup.PlacementTarget;


            var popupTreeRoot = host.HostedVisualTreeRoot;


            if ((placementTarget == null) && (popup.Parent is Visual parent))
                placementTarget = parent;


            Visual offsetRelativeTo = /*GetOffsetRelativeTo(popup) ??*/ popup;
            Point popupPoint = offsetRelativeTo.Bounds.TopLeft;
            double hCustomOffset = GetHorizontalDistance(popup);
            double vCustomOffset = GetVerticalDistance(popup);
            popupPoint = new(popupPoint.X + (hCustomOffset - hOffset), popupPoint.Y + (vCustomOffset - vOffset));


            PixelPoint popupRootPxPoint = popupTreeRoot.PointToScreen(popupPoint);
            Point pointInTarget = placementTarget.PointToClient(popupRootPxPoint);
            //Debug.WriteLine($"({hCustomOffset}, {vCustomOffset}), ({popupPoint}), ({popupRootPxPoint}), ({pointInTarget})");


            bool hInvert = pointInTarget.X <= 0;
            double hFinalOffset = hCustomOffset;
            if (!hInvert)
                hFinalOffset = -hFinalOffset;


            bool vInvert = pointInTarget.Y <= 0;
            double vFinalOffset = vCustomOffset;
            if (!vInvert)
                vFinalOffset = -vFinalOffset;


            SetIsHorizontalInverted(popup, hInvert);
            SetIsVerticalInverted(popup, vInvert);


            popup.HorizontalOffset = hFinalOffset;
            popup.VerticalOffset = vFinalOffset;
        }
    }
}