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


        public static readonly StyledProperty<bool> CanResizeProperty =
            AvaloniaProperty.Register<WindowResizeEdge, bool>(nameof(CanResize), true);
        public bool CanResize
        {
            get => GetValue(CanResizeProperty);
            set => SetValue(CanResizeProperty, value);
        }


        public static readonly DirectProperty<WindowResizeEdge, bool> IsPointerOverEdgeProperty =
            AvaloniaProperty.RegisterDirect<WindowResizeEdge, bool>(nameof(IsPointerOverEdge)
                , e => e.IsPointerOverEdge
            );
        bool _IsPointerOverEdge = false;
        public bool IsPointerOverEdge
        {
            get => _IsPointerOverEdge;
            private set => SetAndRaise(IsPointerOverEdgeProperty, ref _IsPointerOverEdge, value);
        }




        static WindowResizeEdge()
        {
            CanResizeProperty.Changed.AddClassHandler<WindowResizeEdge>(CanResizeProperty_Changed);
        }


        static void CanResizeProperty_Changed(WindowResizeEdge edge, AvaloniaPropertyChangedEventArgs args)
        {
            if (!args.GetNewValue<bool>())
                edge.Cursor = null;
        }




        Window _window = null;
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
            if (CanResize && TryGetWindowEdgeUnderCursor(e, out WindowEdge edge))
                _window?.BeginResizeDrag(edge, e);
        }


        bool _hasCustomCursor = false;
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            if (CanResize)
            {
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