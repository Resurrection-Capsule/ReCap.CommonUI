using System;
using Avalonia;
using Avalonia.Media;
using ReCap.CommonUI.Controls.Decorators;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    public partial class TitleBarAngledBorder
        : AngledBorderBase
    {
        public static readonly StyledProperty<double> TopLeftMajorCutProperty =
            AvaloniaProperty.Register<TitleBarAngledBorder, double>(nameof(TopLeftMajorCut), 0d);
        public double TopLeftMajorCut
        {
            get => GetValue(TopLeftMajorCutProperty);
            set => SetValue(TopLeftMajorCutProperty, value);
        }


        public static readonly StyledProperty<double> TopLeftMinorCutProperty =
            AvaloniaProperty.Register<TitleBarAngledBorder, double>(nameof(TopLeftMinorCut), 0);
        public double TopLeftMinorCut
        {
            get => GetValue(TopLeftMinorCutProperty);
            set => SetValue(TopLeftMinorCutProperty, value);
        }


        public static readonly StyledProperty<double> TopLeftInsetProperty =
            AngledBorderEx.TopLeftInsetProperty.AddOwner<TitleBarAngledBorder>();
        public double TopLeftInset
        {
            get => GetValue(TopLeftInsetProperty);
            set => SetValue(TopLeftInsetProperty, value);
        }
        public static readonly StyledProperty<double> TopRightMajorCutProperty =
            AvaloniaProperty.Register<TitleBarAngledBorder, double>(nameof(TopRightMajorCut), 0d);
        public double TopRightMajorCut
        {
            get => GetValue(TopRightMajorCutProperty);
            set => SetValue(TopRightMajorCutProperty, value);
        }


        public static readonly StyledProperty<double> TopRightMinorCutProperty =
            AvaloniaProperty.Register<TitleBarAngledBorder, double>(nameof(TopRightMinorCut), 0d);
        public double TopRightMinorCut
        {
            get => GetValue(TopRightMinorCutProperty);
            set => SetValue(TopRightMinorCutProperty, value);
        }


        public static readonly StyledProperty<double> TopRightInsetProperty =
            AngledBorderEx.TopRightInsetProperty.AddOwner<TitleBarAngledBorder>();
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




        static TitleBarAngledBorder()
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


            AffectsGeometry<TitleBarAngledBorder>(props);
            AffectsRender<TitleBarAngledBorder>(props);
        }




        protected override void RefreshGeometry(out Geometry fillGeometry, out Geometry strokeGeometry, out bool strokeUseAutoXor, out RoundedRect glowRect)
        {
            strokeUseAutoXor = false;
            var bounds = Bounds;
            double width = Math.Round(bounds.Width);
            double height = Math.Round(bounds.Height);

            double minDimen = Math.Min(width, height);

            double br = Math.Min(BottomRightCut, minDimen);
            double brInset = Math.Min(BottomRightInset, width);

            double bl = Math.Min(BottomLeftCut, minDimen);
            double blInset = Math.Min(BottomLeftInset, width);



            double strokeThickness = StrokeThickness;
            bool hasStroke = strokeThickness > 0;
            double borderBothSides = strokeThickness * 2;

            double fillWidth = width - borderBothSides;
            double fillHeight = height - borderBothSides;

            Rect fillBounds = new(strokeThickness, strokeThickness, fillWidth, fillHeight);
            StreamGeometry fillGeom = new();
            BuildGeometry(ref fillGeom, fillBounds, false);
            
            fillGeometry = fillGeom;
            if (hasStroke)
            {
                Pen strokePen = new(Brushes.Red, borderBothSides);
                Geometry strokeOuterGeometry = fillGeom.GetWidenedGeometry(strokePen);
                strokeGeometry = new CombinedGeometry(GeometryCombineMode.Exclude, strokeOuterGeometry, fillGeom);
            }
            else
            {
                strokeGeometry = new StreamGeometry();
            }


            glowRect = new(fillBounds, Math.Max(bl, br), Math.Max(blInset, brInset));
        }




        void BuildGeometry(ref StreamGeometry geom, Rect rect, bool isStroked)
            => BuildGeometry(ref geom
                , rect.Left, rect.Top, rect.Right, rect.Bottom
                , isStroked
            );
        void BuildGeometry(ref StreamGeometry geom
            , double rectLeft, double rectTop, double rectRight, double rectBottom
            , bool isStroked
        )
        {
            using PointsBuilder builder = new(geom
                , rectLeft, rectTop, rectRight, rectBottom
                , isStroked: isStroked
            )
            {
                TopLeftMajorCut = TopLeftMajorCut,
                TopLeftMinorCut = TopLeftMinorCut,
                TopLeftInset = TopLeftInset,

                TopRightMajorCut = TopRightMajorCut,
                TopRightMinorCut = TopRightMinorCut,
                TopRightInset = TopRightInset,

                BottomLeftCut = BottomLeftCut,
                BottomLeftInset = BottomLeftInset,

                BottomRightCut = BottomRightCut,
                BottomRightInset = BottomRightInset,

                StrokeThickness = StrokeThickness,
            };
            builder.AddPoints(builder.CreateAllPoints());
        }
    }
}