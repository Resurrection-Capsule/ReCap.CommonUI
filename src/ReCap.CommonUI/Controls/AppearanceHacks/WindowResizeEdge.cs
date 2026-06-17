using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Rendering;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    public sealed class WindowResizeEdge
        : Control
        , ICustomHitTest
    {
        static readonly Cursor _FALLBACK = new(StandardCursorType.DragMove);
        static readonly IReadOnlyDictionary<WindowEdge, Cursor> _EDGE_TO_CURSOR = new Dictionary<WindowEdge, Cursor>()
        {
            [WindowEdge.NorthWest] = new(StandardCursorType.TopLeftCorner),
            [WindowEdge.North] = new(StandardCursorType.TopSide),
            [WindowEdge.NorthWest] = new(StandardCursorType.TopRightCorner),

            [WindowEdge.West] = new(StandardCursorType.LeftSide),
            [WindowEdge.East] = new(StandardCursorType.RightSide),

            [WindowEdge.SouthWest] = new(StandardCursorType.BottomLeftCorner),
            [WindowEdge.South] = new(StandardCursorType.BottomSide),
            [WindowEdge.SouthWest] = new(StandardCursorType.BottomRightCorner),
        };




        public static readonly StyledProperty<Thickness> BorderThicknessProperty =
            Border.BorderThicknessProperty.AddOwner<WindowResizeEdge>();
        public Thickness BorderThickness
        {
            get => GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }




        static WindowResizeEdge()
        {
            IsEnabledProperty.Changed.AddClassHandler<WindowResizeEdge>(IsEnabledProperty_Changed);
        }


        static void IsEnabledProperty_Changed(WindowResizeEdge edge, AvaloniaPropertyChangedEventArgs args)
            => edge.OnIsEnabledChanged(args.GetNewValue<bool>());




        Window _window = null;
        bool _hasCustomCursor = false;
        bool _canResize = true;
        public WindowResizeEdge()
            : base()
        {
            _canResize = IsEnabled;
            Cursor = null;
            _hasCustomCursor = false;
        }




        void OnIsEnabledChanged(bool canResize)
        {
            if (_canResize && (!canResize))
            {
                Cursor = null;
                _hasCustomCursor = false;
            }

            _canResize = canResize;
        }




        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            if (e.Root is Window window)
                _window = window;
        }


        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            _window = null;
        }


        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            if (!_canResize)
                return;

            if (TryGetWindowEdgeUnderCursor(e, out WindowEdge edge))
                _window?.BeginResizeDrag(edge, e);
        }


        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            if (!_canResize)
                return;

            if (TryGetWindowEdgeUnderCursor(e, out WindowEdge edge))
            {
                Cursor = _EDGE_TO_CURSOR.TryGetValue(edge, out Cursor cursor)
                    ? cursor
                    : _FALLBACK
                ;
                _hasCustomCursor = true;
            }
            else if (_hasCustomCursor)
            {
                Cursor = null;
                _hasCustomCursor = false;
            }
        }




        bool TryGetWindowEdgeUnderCursor(PointerEventArgs e, out WindowEdge edge)
            => TryGetWindowEdgeAtPoint(e.GetPosition(this), out edge);

        bool TryGetWindowEdgeAtPoint(Point point, out WindowEdge edge)
            => TryGetWindowEdge(Bounds.Size, BorderThickness, point, out edge);

        static bool TryGetWindowEdge(Size size, Thickness thickness, Point point, out WindowEdge edge)
        {
            size.Deconstruct(out double width, out double height);
            point.Deconstruct(out double pointerX, out double pointerY);

            bool left = pointerX <= thickness.Left;
            bool right = pointerX > (width - thickness.Right);

            bool top = pointerY <= thickness.Top;
            bool bottom = pointerY > (height - thickness.Bottom);


            if (left)
            {
                if (top)
                    edge = WindowEdge.NorthWest;
                else if (bottom)
                    edge = WindowEdge.SouthWest;
                else
                    edge = WindowEdge.West;
            }
            else if (right)
            {
                if (top)
                    edge = WindowEdge.NorthEast;
                else if (bottom)
                    edge = WindowEdge.SouthEast;
                else
                    edge = WindowEdge.East;
            }
            else if (top)
            {
                edge = WindowEdge.North;
            }
            else if (bottom)
            {
                edge = WindowEdge.South;
            }
            else
            {
                edge = default;
                return false;
            }


            return true;
        }




#region ICustomHitTest
        public bool HitTest(Point point)
        {
            Rect bounds = Bounds;
            if (!bounds.Contains(point))
                return false;

            return !bounds
                .Deflate(BorderThickness)
                .Contains(point)
            ;
        }
#endregion
    }
}