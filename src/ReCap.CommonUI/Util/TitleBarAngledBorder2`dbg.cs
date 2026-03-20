#if TBAB2_DBG_OVERCOMPLICATED
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using ReCap.CommonUI.Controls.Decorators;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    partial class TitleBarAngledBorder2
    {
        void DbgRender(DrawingContext context)
        {
            var size = Bounds.Size;
            double width = size.Width;
            double height = size.Height;

            Rect bMaxRect = new(0d, 0d, width, height - _bMax);
            context.RenderRect(bMaxRect);

            Rect tMaMinRect = new(0d, 0d, width, height - _bMax);
            context.RenderRect(tMaMinRect);
        }
    }


    internal static class DebugTBAB2
    {
        internal readonly struct DebugTBAB2RenderInfo
        {
            public readonly IBrush Fill;
            public readonly IBrush Stroke;
            public readonly Thickness StrokeThickness;


            public DebugTBAB2RenderInfo(IBrush fill = null, IBrush stroke = null, double strokeWidth = MIN_STROKE_WIDTH)
            {
                Fill = fill;
                Stroke = stroke;
                StrokeThickness = GetStrokeThicknessFromStrokeWidth(strokeWidth);
            }
            public DebugTBAB2RenderInfo(IBrush fill = null, IBrush stroke = null, Thickness? strokeThickness = null)
            {
                Fill = fill;
                Stroke = stroke;
                StrokeThickness = TryGetStrokeThickness(strokeThickness, out Thickness realStrokeThickness)
                    ? realStrokeThickness
                    : _MIN_STROKE_THICKNESS
                ;
            }
        }


        public const double MIN_STROKE_WIDTH = 2d;
        public static readonly IBrush DEFAULT_FILL_BRUSH = new SolidColorBrush(Color.FromArgb(0x80, 0x00, 0xFF, 0x00));
        public static readonly IBrush DEFAULT_STROKE_BRUSH = new SolidColorBrush(Color.FromArgb(0x80, 0xFF, 0x00, 0xFF));


        public static void RenderRect(this DrawingContext context, Rect rect
            , IBrush stroke = null
            , double strokeWidth = MIN_STROKE_WIDTH
        )
            => context.RenderRect(rect, DEFAULT_FILL_BRUSH, stroke, strokeWidth);
        public static void RenderRect(this DrawingContext context, Rect rect
            , IBrush fill
            , IBrush stroke
            , double strokeWidth = MIN_STROKE_WIDTH
        )
            => context.RenderRectEx(rect
                , fill
                , stroke
                , GetStrokeThicknessFromStrokeWidth(strokeWidth)
            );


        public static void RenderRectEx(this DrawingContext context, Rect rect
            , IBrush stroke = null
            , Thickness? strokeThickness = null
        )
            => context.RenderRectEx(rect, DEFAULT_FILL_BRUSH, stroke, strokeThickness);

        public static void RenderRectEx(this DrawingContext context, Rect rect
            , IBrush fill = null
            , IBrush stroke = null
            , Thickness? strokeThickness = null
        )
        {
            bool hasFill = fill != null;
            bool hasStroke = stroke != null;

            IBrush fillBrush = fill ?? DEFAULT_FILL_BRUSH;
            IBrush strokeBrush = stroke ?? DEFAULT_STROKE_BRUSH;
            if (!(hasFill && hasStroke))
                return;


            Rect outerRect = new(rect.Position, rect.Size);
            //if (!TryGetStrokeThickness(stroke, strokeThickness, out Thickness realStrokeThickness))
            //if (!TryGetStrokeThickness(hasStroke, strokeThickness, out Thickness realStrokeThickness))
            if (!(hasStroke && TryGetStrokeThickness(strokeThickness, out Thickness realStrokeThickness)))
            {
                context.FillRectangle(fill, outerRect);
                return;
            }


            Geometry outerGeom = new RectangleGeometry(outerRect);
            Rect innerRect = outerRect.Deflate(realStrokeThickness);
            RectangleGeometry innerGeom = new(innerRect);

            outerGeom = Geometry.Combine(outerGeom, innerGeom, GeometryCombineMode.Exclude);


            context.DrawGeometry(fillBrush, null, innerGeom);
            context.DrawGeometry(strokeBrush, null, outerGeom);
        }


        //static readonly Thickness _ZERO_THICKNESS = new(0d);
        static readonly Thickness _MIN_STROKE_THICKNESS = new(MIN_STROKE_WIDTH);
        /*
        static bool TryGetStrokeThickness(IBrush brush, Thickness? baseThickness, out Thickness result)
            => TryGetStrokeThickness(brush != null, baseThickness, out result);
        static bool TryGetStrokeThickness(bool hasStroke, Thickness? baseThickness, out Thickness result)
        {
            if (hasStroke)
                return TryGetStrokeThickness(baseThickness, out result);

            result = default;
            return false;
        }
        */


        static Thickness GetStrokeThicknessFromStrokeWidth(double strokeWidth)
            => GetStrokeThicknessFromStrokeWidth(strokeWidth, _MIN_STROKE_THICKNESS);
        static Thickness GetStrokeThicknessFromStrokeWidth(double strokeWidth, Thickness fallback)
            => GetStrokeThicknessNFromStrokeWidth(strokeWidth) ?? fallback;

        static Thickness? GetStrokeThicknessNFromStrokeWidth(double strokeWidth)
        {
            if (strokeWidth == MIN_STROKE_WIDTH)
                return _MIN_STROKE_THICKNESS;
            else if (strokeWidth >= 0d)
                return new(strokeWidth);
            else
                return null;
        }


        static bool TryGetStrokeThicknessFromStrokeWidth(double strokeWidth, out Thickness thickness)
            => TryGetStrokeThicknessFromStrokeWidth(strokeWidth, out thickness, _MIN_STROKE_THICKNESS);
        static bool TryGetStrokeThicknessFromStrokeWidth(double strokeWidth, out Thickness thickness, Thickness fallback)
        {
            if (TryGetStrokeThicknessNFromStrokeWidth(strokeWidth, out Thickness? result))
            {
                thickness = result.Value;
                return true;
            }
            else
            {
                thickness = fallback;
                return false;
            }
        }
        static bool TryGetStrokeThicknessNFromStrokeWidth(double strokeWidth, out Thickness? thickness)
        {
            /*
            if (strokeWidth == MIN_STROKE_WIDTH)
            {
                thickness = _MIN_STROKE_THICKNESS;
                return true;
            }
            else if (strokeWidth >= 0d)
            {
                thickness = new(strokeWidth);
                return true;
            }
            else
            {
                thickness = null;
                return false;
            }
            */
            thickness = GetStrokeThicknessNFromStrokeWidth(strokeWidth);
            return thickness != null;
        }


        static bool TryGetStrokeThickness(Thickness? baseThickness, out Thickness result)
        {
            if ((baseThickness == null) || !baseThickness.HasValue)
            {
                result = _MIN_STROKE_THICKNESS;
                return true;
            }


            Thickness thickness = baseThickness.Value;
            var sides = thickness.ToDoubles();
            if (sides.Any(x => x < MIN_STROKE_WIDTH))
                goto fail;

            double strokeThicknessLeft = Math.Max(MIN_STROKE_WIDTH, thickness.Left);
            if (thickness.IsUniform)
            {
                result = new(strokeThicknessLeft);
                return true;
            }

            result = new(strokeThicknessLeft
                , Math.Max(MIN_STROKE_WIDTH, thickness.Top)
                , Math.Max(MIN_STROKE_WIDTH, thickness.Right)
                , Math.Max(MIN_STROKE_WIDTH, thickness.Bottom)
            );
            return true;


            fail:
            result = default;
            return false;
        }
    }
}
#endif