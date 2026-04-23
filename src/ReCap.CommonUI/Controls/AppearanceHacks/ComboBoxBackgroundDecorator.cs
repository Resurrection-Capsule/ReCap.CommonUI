using System;
using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using ReCap.CommonUI.Controls.Decorators;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    public sealed partial class ComboBoxBackgroundDecorator
        : AngledBorderBase
    {
        /// <summary>
        /// Defines the <see cref="RadiusX"/> property.
        /// </summary>
        public static readonly StyledProperty<double> RadiusXProperty =
            Rectangle.RadiusXProperty.AddOwner<Rectangle>();
        /// <summary>
        /// Gets or sets the radius on the X-axis used to round the non-cut corners of the <see cref="ComboBoxBackgroundDecorator"/>.
        /// Corner radii are represented by an ellipse so this is the X-axis width of the ellipse.
        /// </summary>
        public double RadiusX
        {
            get => GetValue(RadiusXProperty);
            set => SetValue(RadiusXProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="RadiusY"/> property.
        /// </summary>
        public static readonly StyledProperty<double> RadiusYProperty =
            Rectangle.RadiusYProperty.AddOwner<Rectangle>();
        /// <summary>
        /// Gets or sets the radius on the Y-axis used to round the non-cut corners of the <see cref="ComboBoxBackgroundDecorator"/>.
        /// Corner radii are represented by an ellipse so this is the Y-axis height of the ellipse.
        /// </summary>
        public double RadiusY
        {
            get => GetValue(RadiusYProperty);
            set => SetValue(RadiusYProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="CutSize"/> property.
        /// </summary>
        public static readonly StyledProperty<double> CutSizeProperty =
            AvaloniaProperty.Register<Rectangle, double>(nameof(CutSize), 0d);
        public double CutSize
        {
            get => GetValue(CutSizeProperty);
            set => SetValue(CutSizeProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="TrimColor"/> property.
        /// </summary>
        public static readonly StyledProperty<Color> TrimColorProperty =
            AvaloniaProperty.Register<Rectangle, Color>(nameof(TrimColor), Colors.Transparent);
        public Color TrimColor
        {
            get => GetValue(TrimColorProperty);
            set => SetValue(TrimColorProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="TrimSize"/> property.
        /// </summary>
        public static readonly StyledProperty<double> TrimSizeProperty =
            AvaloniaProperty.Register<Rectangle, double>(nameof(TrimSize), 0d);
        public double TrimSize
        {
            get => GetValue(TrimSizeProperty);
            set => SetValue(TrimSizeProperty, value);
        }




        static ComboBoxBackgroundDecorator()
        {
            var properties = new AvaloniaProperty[]
            {
                RadiusXProperty,
                RadiusYProperty,
                CutSizeProperty,
                TrimColorProperty,
                TrimSizeProperty,
            };
            TrimColorProperty.Changed.AddClassHandler<ComboBoxBackgroundDecorator>((s, e) => s?.OnTrimColorChanged(e));
            AffectsGeometry<ComboBoxBackgroundDecorator>(properties);
        }


        public ComboBoxBackgroundDecorator()
            : base()
        {
            RenderOptions.SetRequiresFullOpacityHandling(this, true);
        }


        double _trimOpacity = 1d;
        void OnTrimColorChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (!e.TryGetNewValue(out Color color))
                return;

            if (e.TryGetOldValue(out Color oldColor) && (Math.Max(color.A, oldColor.A) <= 0d))
            {
                _trimBottomGeometry = null;
                return;
            }

            _trimOpacity = color.A / 255d;
            _trimBrushStops[0].Color = new(0x00, color.R, color.G, color.B);
            _trimBrushStops[1].Color = new(0xFF, color.R, color.G, color.B);
            UpdateBrush(ref _trimLeftBrush);
            UpdateBrush(ref _trimBottomBrush);
            UpdateBrush(ref _trimCutBrush);

            /*
            if (IsMeasureValid && IsArrangeValid)
                InvalidateVisual();
            */
        }


        public override void Render(DrawingContext context)
        {
            if (_hasTrim)
            {
                using (context.PushOpacity(_trimOpacity))
                {
                    context.DrawGeometry(_trimLeftBrush, null, _trimLeftGeometry);
                    context.DrawGeometry(_trimBottomBrush, null, _trimBottomGeometry);
                    context.DrawGeometry(_trimCutBrush, null, _trimCutGeometry);
                    context.DrawGeometry(_trimRightBrush, null, _trimRightGeometry);
                }
            }

            base.Render(context);
        }



        bool _hasTrim = false;
        Geometry _trimLeftGeometry = null;
        Geometry _trimBottomGeometry = null;
        StreamGeometry _trimCutGeometry = null;
        Geometry _trimRightGeometry = null;

        LinearGradientBrush _trimLeftBrush = new();
        LinearGradientBrush _trimBottomBrush = new();
        LinearGradientBrush _trimCutBrush = new();
        LinearGradientBrush _trimRightBrush = new();


        GradientStops _trimBrushStops = new()
        {
            new(Colors.Transparent, 0d),
            new(Colors.Transparent, 1d),
        };
        void UpdateBrush(ref LinearGradientBrush brush, RelativePoint startPoint, RelativePoint endPoint)
        {
            brush.StartPoint = startPoint;
            brush.EndPoint = endPoint;
            UpdateBrush(ref brush);
        }
        void UpdateBrush(ref LinearGradientBrush brush)
            => brush.GradientStops = _trimBrushStops;


        protected override void RefreshGeometry(out Geometry fillGeometry, out Geometry strokeGeometry, out RoundedRect glowRect)
        {
            double strokeThickness = StrokeThickness;
            double strokeThicknessTwice = strokeThickness * 2d;

            var size = Bounds.Size;
            double width = size.Width;
            double height = size.Height;
            double w = width - strokeThicknessTwice;
            double h = height - strokeThicknessTwice;

            double l = strokeThickness;
            double t = strokeThickness;
            double r = l + w;
            double b = t + h;

            double radiusX = RadiusX;
            double radiusY = RadiusY;


            double rdX = radiusX - strokeThickness;
            double rdY = radiusY - strokeThickness;

            double cut = CutSize;

            Rect rect = new(strokeThickness, strokeThickness, w, h);

            Vector rdVector = new(rdX, rdY);
            glowRect = new(rect, rdVector, rdVector, new(cut, cut), rdVector);

            StreamGeometry fillGeom = new();
            CreateGeometry(ref fillGeom
                , l, t, r, b
                , rdX, rdY
                , cut
            );
            fillGeometry = fillGeom;


            strokeGeometry = null;
            if (strokeThickness > 0d)
            {
                StreamGeometry strokeGeom = new();
                CreateGeometry(ref strokeGeom
                    , width, height
                    , radiusX, radiusY
                    , cut
                );
                strokeGeometry = strokeGeom;
            }


            double trimSize = TrimSize;
            if (trimSize > 0d)
            {
                Color trimColor = TrimColor;
                if (trimColor.A > 0d)
                {
                    double trimAngleDiff = trimSize;
                    double trimAngleDiff_07_ = trimAngleDiff * 0.70711d;


                    RectangleGeometry leftRectG = new(new(l, t, trimSize, h));
                    _trimLeftGeometry = new CombinedGeometry(GeometryCombineMode.Intersect, fillGeometry, leftRectG);

                    double bottomRectWidth = w - cut;
                    double bottomRectY = b - trimSize;
                    RectangleGeometry bottomRectG = new(new(l, bottomRectY, w, trimSize));
                    _trimBottomGeometry = new CombinedGeometry(GeometryCombineMode.Intersect, fillGeometry, bottomRectG);

                    double cutGeomL = l + bottomRectWidth;
                    double cutGeomB = b - cut;
                    _trimCutGeometry = new();
                    double trimSize_AngleDiff_07_ = trimSize + trimAngleDiff_07_;
                    using (var ctx = _trimCutGeometry.Open())
                    {
                        ctx.BeginFigure(new(cutGeomL - trimSize_AngleDiff_07_, b), true);
                        ctx.LineTo(new(cutGeomL, b));
                        ctx.LineTo(new(r, cutGeomB));
                        ctx.LineTo(new(r, cutGeomB - trimSize_AngleDiff_07_));
                        ctx.EndFigure(true);
                    }

                    RectangleGeometry rightRectG = new(new(r - trimSize, t, trimSize, h));
                    _trimRightGeometry = new CombinedGeometry(GeometryCombineMode.Intersect, fillGeometry, rightRectG);

                    UpdateBrush(ref _trimLeftBrush
                        , new(1d, 0d, RelativeUnit.Relative)
                        , RelativePoint.TopLeft
                    );

                    UpdateBrush(ref _trimBottomBrush
                        , RelativePoint.TopLeft
                        , new(0d, 1d, RelativeUnit.Relative)
                    );

                    var cutGeomBounds = _trimBottomGeometry.Bounds;
                    double cutBrushBaseX = cutGeomBounds.Right - trimAngleDiff_07_;
                    double cutBrushBaseY = cutGeomBounds.Y;
                    UpdateBrush(ref _trimCutBrush
                        , new(cutBrushBaseX - trimAngleDiff, cutBrushBaseY, RelativeUnit.Absolute)
                        , new(cutBrushBaseX, cutBrushBaseY + trimAngleDiff, RelativeUnit.Absolute)
                    );

                    UpdateBrush(ref _trimRightBrush
                        , RelativePoint.TopLeft
                        , new(1d, 0d, RelativeUnit.Relative)
                    );


                    _hasTrim = true;
                    return;
                }
            }
                
            _hasTrim = false;
        }


        const double _ARC_TO_CORNER_ANGLE = 0d;
        static void CreateGeometry(ref StreamGeometry geom
            , double width, double height
            , double rdX, double rdY
            , double cut
        )
            => CreateGeometry(ref geom
                , 0d, 0d, width, height
                , rdX, rdY
                , cut
            );
        static void CreateGeometry(ref StreamGeometry geom
            , double left, double top, double right, double bottom
            , double radiusX, double radiusY
            , double cut
        )
        {
            double width = right - left;
            double height = bottom - top;

            double rdX = Math.Min(radiusX, width * 0.5d);
            double rdY = Math.Min(radiusY, height * 0.5d);
            Size rd = new(rdX, rdY);
            bool hasRd = Math.Max(rdX, rdY) > 0d;

            double top_rdY = top + rdY;
            double bottom_rdY = bottom - rdY;
            double left_rdX = left + rdX;
            Point trArcStart = new(right - rdX, top);
            Point trArcEnd = new(right, top_rdY);

            double cutLineStartX = right;
            double cutLineStartY = bottom - cut;

            double cutLineEndX = right - cut;
            double cutLineEndY = bottom;
            if (cutLineEndX < rdX)
            {
                cutLineEndY += cutLineEndX - rdX;
                cutLineEndX = rdX;
            }

            using var ctx = geom.Open();
            ctx.BeginFigure(trArcStart, true);


            if (height > (cut + rdY))
            {
                if (hasRd)
                    ctx.ArcTo(trArcEnd, rd, _ARC_TO_CORNER_ANGLE, false, SweepDirection.Clockwise);

                ctx.LineTo(new(cutLineStartX, cutLineStartY));
            }
            else if (height > cut)
            {
                double trArcAngle = 90d;

                double cutLineSlope = (cutLineEndY - cutLineStartY) / (cutLineEndX - cutLineStartX);
                double cutLineYIntercept = cutLineStartY - (cutLineStartX * cutLineSlope);
                double cutLine = (cutLineSlope * cutLineStartX) + cutLineYIntercept;

                
                double h = trArcStart.X;
                double aP2 = Math.Pow(rdX, 2);
                double bP2 = Math.Pow(rdY, 2);

                double insX = Math.Sqrt(
                    (1 - (Math.Pow(cutLine - top_rdY, 2) / bP2))
                    * aP2
                ) + h;

                double insY = top_rdY - Math.Sqrt(
                    (1 - (Math.Pow(insX - h, 2) / aP2))
                    * bP2
                );


                trArcAngle = Math.Atan2(insY, insX) * (180 / Math.PI);

                double xDiff = trArcEnd.X - insX;
                trArcEnd = new(insX + xDiff, insY);
                trArcStart = new(trArcStart.X + xDiff, trArcStart.Y);
                ctx.LineTo(trArcStart);
                ctx.ArcTo(trArcEnd, rd, trArcAngle, false, SweepDirection.Clockwise);
            }
            else
            {
                ctx.LineTo(new(cutLineStartX + cutLineStartY, 0d));
            }

            ctx.LineTo(new(cutLineEndX, cutLineEndY));
            ctx.LineTo(new(left_rdX, bottom));

            if (hasRd)
                ctx.ArcTo(new(left, bottom_rdY), rd, _ARC_TO_CORNER_ANGLE, false, SweepDirection.Clockwise);

            ctx.LineTo(new(left, top_rdY));

            if (hasRd)
                ctx.ArcTo(new(left_rdX, top), rd, _ARC_TO_CORNER_ANGLE, false, SweepDirection.Clockwise);


            ctx.EndFigure(true);
        }
    }
}