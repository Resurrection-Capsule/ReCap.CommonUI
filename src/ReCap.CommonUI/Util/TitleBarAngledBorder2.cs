using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using ReCap.CommonUI.Controls.Decorators;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    public partial class TitleBarAngledBorder2
        : AngledBorderBase
    {
        public static readonly StyledProperty<double> TopLeftMajorCutProperty =
            AvaloniaProperty.Register<TitleBarAngledBorder2, double>(nameof(TopLeftMajorCut), 0);
        public double TopLeftMajorCut
        {
            get => GetValue(TopLeftMajorCutProperty);
            set => SetValue(TopLeftMajorCutProperty, value);
        }


        public static readonly StyledProperty<double> TopLeftMinorCutProperty =
            AvaloniaProperty.Register<TitleBarAngledBorder2, double>(nameof(TopLeftMinorCut), 0);
        public double TopLeftMinorCut
        {
            get => GetValue(TopLeftMinorCutProperty);
            set => SetValue(TopLeftMinorCutProperty, value);
        }


        public static readonly StyledProperty<double> TopLeftInsetProperty =
            AngledBorderEx.TopLeftInsetProperty.AddOwner<TitleBarAngledBorder2>();
        public double TopLeftInset
        {
            get => GetValue(TopLeftInsetProperty);
            set => SetValue(TopLeftInsetProperty, value);
        }
        public static readonly StyledProperty<double> TopRightMajorCutProperty =
            AvaloniaProperty.Register<TitleBarAngledBorder2, double>(nameof(TopRightMajorCut), 0);
        public double TopRightMajorCut
        {
            get => GetValue(TopRightMajorCutProperty);
            set => SetValue(TopRightMajorCutProperty, value);
        }


        public static readonly StyledProperty<double> TopRightMinorCutProperty =
            AvaloniaProperty.Register<TitleBarAngledBorder2, double>(nameof(TopRightMinorCut), 0);
        public double TopRightMinorCut
        {
            get => GetValue(TopRightMinorCutProperty);
            set => SetValue(TopRightMinorCutProperty, value);
        }


        public static readonly StyledProperty<double> TopRightInsetProperty =
            AngledBorderEx.TopRightInsetProperty.AddOwner<TitleBarAngledBorder2>();
        public double TopRightInset
        {
            get => GetValue(TopRightInsetProperty);
            set => SetValue(TopRightInsetProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="BottomRightCut"/> property.
        /// </summary>
        public static readonly StyledProperty<double> BottomRightCutProperty =
            AngledBorderEx.BottomRightCutProperty.AddOwner<TitleBarAngledBorder>();
        /// <summary>
        /// Gets or sets the size of the cut corner
        /// </summary>
        public double BottomRightCut
        {
            get => GetValue(BottomRightCutProperty);
            set => SetValue(BottomRightCutProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="BottomLeftCut"/> property.
        /// </summary>
        public static readonly StyledProperty<double> BottomLeftCutProperty =
            AngledBorderEx.BottomLeftCutProperty.AddOwner<TitleBarAngledBorder>();
        /// <summary>
        /// Gets or sets the size of the cut corner
        /// </summary>
        public double BottomLeftCut
        {
            get => GetValue(BottomLeftCutProperty);
            set => SetValue(BottomLeftCutProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="BottomRightInset"/> property.
        /// </summary>
        public static readonly StyledProperty<double> BottomRightInsetProperty =
            AngledBorderEx.BottomRightInsetProperty.AddOwner<TitleBarAngledBorder>();
        /// <summary>
        /// Gets or sets the inset distance of the cut corner
        /// </summary>
        public double BottomRightInset
        {
            get => GetValue(BottomRightInsetProperty);
            set => SetValue(BottomRightInsetProperty, value);
        }


        /// <summary>
        /// Defines the <see cref="BottomLeftInset"/> property.
        /// </summary>
        public static readonly StyledProperty<double> BottomLeftInsetProperty =
            AngledBorderEx.BottomLeftInsetProperty.AddOwner<TitleBarAngledBorder>();
        /// <summary>
        /// Gets or sets the inset distance of the cut corner
        /// </summary>
        public double BottomLeftInset
        {
            get => GetValue(BottomLeftInsetProperty);
            set => SetValue(BottomLeftInsetProperty, value);
        }




        static TitleBarAngledBorder2()
        {
            AvaloniaProperty[] props =
            {
                TopLeftMajorCutProperty,
                TopLeftMinorCutProperty,
                TopLeftInsetProperty,
                TopRightMajorCutProperty,
                TopRightMinorCutProperty,
                TopRightInsetProperty,
                BottomRightCutProperty,
                BottomLeftCutProperty,
                BottomRightInsetProperty,
                BottomLeftInsetProperty,
            };


            AffectsGeometry<TitleBarAngledBorder2>(props);
            AffectsRender<TitleBarAngledBorder2>(props);
        }


        double _tlMa = 0d;
        double _tlMi = 0d;
        double _tlInset = 0d;

        double _trMa = 0d;
        double _trMi = 0d;
        double _trInset = 0d;

        double _br = 0d;
        double _brInset = 0d;

        double _bl = 0d;
        double _blInset = 0d;


        double _tMaMin = 0d;
        double _complexLowerPortionThreshold = 0d;
        double _bMax = 0d;
        double _minDimen = 0d;
        protected override void RefreshGeometry(out Geometry fillGeometry, out Geometry strokeGeometry, out RoundedRect glowRect)
        {
            //Console.WriteLine($"Updating geometries...");
            var bounds = Bounds;
            double width = Math.Round(bounds.Width);
            double height = Math.Round(bounds.Height);

            _minDimen = Math.Min(width, height);

            _tlMa = TopLeftMajorCut; //Math.Min(TopLeftMajorCut, minDimen);
            _tlMi = Math.Min(TopLeftMinorCut, _minDimen);
            _tlInset = Math.Min(TopLeftInset, width);

            _trMa = TopRightMajorCut; //Math.Min(TopRightMajorCut, minDimen);
            _trMi = Math.Min(TopRightMinorCut, _minDimen);
            _trInset = Math.Min(TopRightInset, width);

            _br = Math.Min(BottomRightCut, _minDimen);
            _brInset = Math.Min(BottomRightInset, width);

            _bl = Math.Min(BottomLeftCut, _minDimen);
            _blInset = Math.Min(BottomLeftInset, width);



            double strokeThickness = StrokeThickness;
            bool hasStroke = strokeThickness > 0;
            double borderBothSides = strokeThickness * 2;

            double fillWidth = width - borderBothSides;
            double fillHeight = height - borderBothSides;


            _bMax = Math.Max(_bl, _br);
            _tMaMin = Math.Min(_tlMa, _trMa);
            _complexLowerPortionThreshold = _tMaMin + _bMax;

            Rect fillBounds = new(strokeThickness, strokeThickness, fillWidth, fillHeight);
            StreamGeometry fillGeom = new();
            DefineGeometry(ref fillGeom, fillBounds);
            
            fillGeometry = fillGeom;
            if (hasStroke)
            {
                Rect strokeBounds = new(0, 0, width, height);
                StreamGeometry strokeGeom = new();
                DefineGeometry(ref strokeGeom, strokeBounds);
                //CreateGeometry(ref strokeGeom, fillBounds);

                strokeGeometry = strokeGeom;
            }
            else
            {
                strokeGeometry = new StreamGeometry();
            }


            glowRect = new(fillBounds, Math.Max(_bl, _br), Math.Max(_blInset, _brInset));
        }



        StreamGeometry CreateGeometry(Rect rect)
        {
            StreamGeometry geom = new();
            DefineGeometry(ref geom, rect);
            return geom;
        }
        StreamGeometry CreateGeometry(double rectLeft, double rectTop, double rectRight, double rectBottom)
        {
            StreamGeometry geom = new();
            DefineGeometry(ref geom, rectLeft, rectTop, rectRight, rectBottom);
            return geom;
        }


#if DEBUG
        double _rectBottomMinusTop = 0d;
        bool _useComplexLowerPortion = false;
#endif
        void DefineGeometry(ref StreamGeometry geom, Rect rect)
            => DefineGeometry(ref geom
                , rect.Left, rect.Top, rect.Right, rect.Bottom
            );
        void DefineGeometry(ref StreamGeometry geom
            , double rectLeft, double rectTop, double rectRight, double rectBottom
        )
        {
            using StreamGeometryContext ctx = geom.Open();
            double insetBottom = rectBottom - _bMax;

            double rectBottomMinusTop = rectBottom - rectTop;
            bool useComplexLowerPortion = rectBottomMinusTop >= _complexLowerPortionThreshold;
#if DEBUG
            _rectBottomMinusTop = rectBottomMinusTop;
            _useComplexLowerPortion = useComplexLowerPortion;
            //Debug.WriteLine($"{nameof(useComplexLowerPortion)}: {useComplexLowerPortion}");
#endif

            Point leftPt = new(rectLeft + _tlMi + _tlInset, rectTop + _tlMa);
            if (useComplexLowerPortion)
            {
                ctx.BeginFigure(new(rectLeft + _bl + _blInset, insetBottom), true);
                ctx.LineTo(new(rectLeft + _blInset, insetBottom + _bl));
                ctx.LineTo(new(rectLeft, insetBottom + _bl));

                ctx.LineTo(new(rectLeft, rectTop + _tlMi + _tlMa));
                ctx.LineTo(new(rectLeft + _tlMi, rectTop + _tlMa));
                ctx.LineTo(leftPt);
            }
            else
            {
                ctx.BeginFigure(leftPt, true);
            }

            ctx.LineTo(new(rectLeft + _tlMi + _tlInset + _tlMa, rectTop));
            ctx.LineTo(new(rectRight - _trMi - _trInset - _trMa, rectTop));


            ctx.LineTo(new(rectRight - _trMi - _trInset, rectTop + _trMa));
            if (useComplexLowerPortion)
            {
                ctx.LineTo(new(rectRight - _trMi, rectTop + _trMa));
                ctx.LineTo(new(rectRight, rectTop + _trMi + _trMa));
                /*

                */
                ctx.LineTo(new(rectRight, insetBottom + _br));
                ctx.LineTo(new(rectRight - _brInset, insetBottom + _br));
                ctx.LineTo(new(rectRight - _br - _brInset, insetBottom));
            }
            /*
            else
            {
                double l = rectLeft + _tlInset;
                double r = rectRight + _trInset;
                ctx.BeginFigure(new(l, insetBottom), true);
                ctx.LineTo(new(l + _tlMa, rectTop));
                ctx.LineTo(new(r - _trMa, rectTop));
                ctx.LineTo(new(r, insetBottom));
            }
            */

            ctx.EndFigure(true);
        }


#if NO //DEBUG
        public override void Render(DrawingContext context)
        {
#if TBAB2_DBG_OVERCOMPLICATED
            DbgRender(context);
#else
            var size = Bounds.Size;
            double width = size.Width;
            double height = size.Height;


            Rect bMaxRect = new(0d, 0d, _DBG_VIS_WIDTH, height - _bMax);
            context.FillRectangle(_DBG_PINK, bMaxRect);

            Rect tMaMinRect = new(width - _DBG_VIS_WIDTH, 0d, _DBG_VIS_WIDTH, height - _bMax);
            context.FillRectangle(_DBG_GREEN, bMaxRect);
#endif
            base.Render(context);


            /*
            string textToFormat
            CultureInfo culture
            FlowDirection flowDirection
            Typeface typeface
            double emSize
            IBrush? foreground)
            */
            double textTop = 10d;
            double textLeft = _DBG_VIS_WIDTH + textTop;

            var dbgLines = new[]
            {
                $"{nameof(height)}:                        {height}",
                $"{nameof(_bMax)}:                         {_bMax}",
                $"{nameof(_tMaMin)}:                       {_tMaMin}",
                $"{nameof(_rectBottomMinusTop)}:           {_rectBottomMinusTop}",
                $"{nameof(_useComplexLowerPortion)}:       {_useComplexLowerPortion}",
                $"{nameof(_complexLowerPortionThreshold)}: {_complexLowerPortionThreshold}",
            };

#if NO
            string text = string.Join("\n", dbgLines);
            Rect textBgRect = new(textLeft, textTop, width - (textLeft * 2d), height * 0.5d);
            context.FillRectangle(Brushes.DarkBlue, textBgRect);

            FormattedText fmtText = new(text
                , culture: CultureInfo.InvariantCulture
                , flowDirection: FlowDirection.LeftToRight
                , typeface: Typeface.Default
                , emSize: 1d
                , foreground: Brushes.White
            );
            context.DrawText(fmtText, new(textLeft, textTop));
#else
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), $"{nameof(TitleBarAngledBorder2)}.txt");
            File.WriteAllLines(path, dbgLines);
#endif
        }


        const double _DBG_VIS_WIDTH = 10d;
        static readonly SolidColorBrush _DBG_RED    = new(Color.FromArgb(0x80, 0xFF, 0x00, 0x00));
        static readonly SolidColorBrush _DBG_BLUE   = new(Color.FromArgb(0x80, 0x00, 0xFF, 0x00));
        static readonly SolidColorBrush _DBG_GREEN  = new(Color.FromArgb(0x80, 0x00, 0x00, 0xFF));

        static readonly SolidColorBrush _DBG_PINK   = new(Color.FromArgb(0x80, 0xFF, 0x00, 0xFF));
        static readonly SolidColorBrush _DBG_YELLOW = new(Color.FromArgb(0x80, 0xFF, 0xFF, 0x00));
#endif
    }
}