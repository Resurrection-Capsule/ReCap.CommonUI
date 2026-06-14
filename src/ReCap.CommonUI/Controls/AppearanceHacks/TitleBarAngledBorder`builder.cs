using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using ReCap.CommonUI.Controls.Decorators;

using IPointsEnumerable = System.Collections.Generic.IEnumerable<Avalonia.Point>;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    public partial class TitleBarAngledBorder
        : AngledBorderBase
    {
        sealed class PointsBuilder
            : IDisposable
        {
#region Corners
            public double TopLeftMajorCut
            {
                get;
                init;
            }


            public double TopLeftMinorCut
            {
                get;
                init;
            }


            public double TopLeftInset
            {
                get;
                init;
            }


            public double TopRightMajorCut
            {
                get;
                init;
            }


            public double TopRightMinorCut
            {
                get;
                init;
            }


            public double TopRightInset
            {
                get;
                init;
            }


            public double BottomLeftCut
            {
                get;
                init;
            }


            public double BottomLeftInset
            {
                get;
                init;
            }


            public double BottomRightCut
            {
                get;
                init;
            }


            public double BottomRightInset
            {
                get;
                init;
            }
#endregion




#region Bounds
            readonly double RectLeft;
            readonly double RectTop;
            readonly double RectRight;
            readonly double RectBottom;

            readonly double RectWidth;
            readonly double RectHeight;
#endregion


            public double StrokeThickness
            {
                get;
                init;
            }




            List<Point> _points = new();
            StreamGeometry _geometry;


            const bool _DEFAULT__isFilled = true;
            readonly bool _isFilled;

            const bool _DEFAULT__isStroked = true;
            readonly bool _isStroked;

            const bool _DEFAULT__isClosed = true;
            readonly bool _isClosed;
            private PointsBuilder(StreamGeometry geometry, bool isFilled, bool isStroked, bool isClosed
                , double rectLeft, double rectTop, double rectRight, double rectBottom
            )
            {
                _geometry = geometry;
                _isFilled = isFilled;
                _isStroked = isStroked;
                _isClosed = isClosed;

                RectLeft = rectLeft;
                RectTop = rectTop;
                RectRight = rectRight;
                RectBottom = rectBottom;

                RectWidth = RectRight - RectLeft;
                RectHeight = RectBottom - RectTop;
            }


            public PointsBuilder(StreamGeometry geometry
                , double rectLeft, double rectTop, double rectRight, double rectBottom
                , bool isFilled = _DEFAULT__isFilled
                , bool isStroked = _DEFAULT__isStroked
                , bool isClosed = _DEFAULT__isClosed
            )
                : this(geometry, isFilled, isStroked, isClosed
                    , rectLeft, rectTop, rectRight, rectBottom
                )
            {}


            public PointsBuilder(StreamGeometry geometry
                , Rect rect
                , bool isFilled = _DEFAULT__isFilled
                , bool isStroked = _DEFAULT__isStroked
                , bool isClosed = _DEFAULT__isClosed
            )
                : this(geometry, isFilled, isStroked, isClosed
                    , rect.Left, rect.Top, rect.Right, rect.Bottom
                )
            {}




            public void AddPoint(Point point)
                => _points.Add(point);


            public void AddPoints(IPointsEnumerable points)
                => _points.AddRange(points);
            public void AddPoints(params Point[] points)
                => AddPoints((IPointsEnumerable)points);



            public IPointsEnumerable CreateAllPoints()
            {
                double tlMajorCut = TopLeftMajorCut;
                double tlMinorCut = TopLeftMinorCut;
                double tlInset = TopLeftInset;

                double trMajorCut = TopRightMajorCut;
                double trMinorCut = TopRightMinorCut;
                double trInset = TopRightInset;

                double blCut = BottomLeftCut;
                double blInset = BottomLeftInset;

                double brCut = BottomRightCut;
                double brInset = BottomRightInset;

                double tlLimit = tlMajorCut + tlMinorCut;
                double trLimit = trMajorCut + trMinorCut;

                _bottomAdjustAmount = Math.Max(blCut, brCut);
                if (_isStroked)
                {
                    _strokeAdjust = StrokeThickness * 2d;
                    _bottomAdjustAmount += _strokeAdjust;
                }
                else
                {
                    _strokeAdjust = 0d;
                }
                _bottomAdjusted = RectBottom - _bottomAdjustAmount;


                List<Point> all = new();
                all.AddRange(CreatePointsForSide(false
                    , tlMajorCut, tlMinorCut, tlInset
                    , blCut, blInset
                    , tlLimit
                ));
                all.AddRange(CreatePointsForSide(true
                    , trMajorCut, trMinorCut, trInset
                    , brCut, brInset
                    , trLimit
                ));
                return all;
            }




            IPointsEnumerable CreatePointsForSide(bool rightSide
                , double topMajorCut, double topMinorCut, double topInset
                , double bottomCut, double bottomInset
                , double topLimit
            )
            {
                List<Point> points = new();

                var bottomPoints = CreateBottomPoints(rightSide
                    , bottomCut, bottomInset
                    , topLimit
                );

                bool hasBottomPoints = bottomPoints.Any();
                if (hasBottomPoints)
                {
                    points.AddRange(bottomPoints);
                }

                points.AddRange(CreateTopPoints(rightSide
                    , topMajorCut, topMinorCut, topInset
                    , topLimit, hasBottomPoints
                ));


                if (rightSide)
                    points.Reverse();

                return points;
            }




            IPointsEnumerable CreateTopPoints(bool r
                , double topMajorCut, double topMinorCut, double topInset
                , double topLimit, bool hasBottomPoints
            )
            {
                if (hasBottomPoints)
                {
                    yield return PointFromTop(r, 0d, topLimit);

                    yield return PointFromTop(r, topMinorCut, topMajorCut);
                    yield return PointFromTop(r, topInset, topMajorCut);
                    yield return PointFromTop(r, topInset + topMajorCut, 0d);
                }
                else
                {
                    double diff = topMajorCut - (RectHeight - _bottomAdjustAmount);
                    yield return PointFromTop(r, topInset + diff, topMajorCut - diff);
                    yield return PointFromTop(r, topInset + topMajorCut, 0d);
                }
            }


            IPointsEnumerable CreateBottomPoints(bool r
                , double bottomCut, double bottomInset
                , double topLimit
            )
            {
                Point first = PointFromBottom(r, bottomCut + bottomInset, 0d);
                Point last = PointFromBottom(r, 0d, bottomCut);
                if (first.Y > topLimit)
                {
                    yield return first;
                    if ((bottomCut > 0d) && (bottomInset > 0d))
                    {
                        yield return PointFromBottom(r, bottomInset, bottomCut);
                    }

                    yield return last;
                }
            }


            double FromSide(bool fromRight, double x)
                => fromRight
                    ? RectRight - x
                    : RectLeft + x
                ;


            double _strokeAdjust = 0d;
            double _bottomAdjustAmount = 0d;
            double _bottomAdjusted = 0d;
            double FromTop(double y)
                => RectTop + y;
            double FromBottom(double y)
                => _bottomAdjusted + y;


            Point PointFromTop(bool fromRight, double x, double y)
                => new(FromSide(fromRight, x), FromTop(y));
            Point PointFromBottom(bool fromRight, double x, double y)
                => new(FromSide(fromRight, x), FromBottom(y));


            public void Dispose()
            {
                if (_points.Count < 1)
                    goto end;
                else using (StreamGeometryContext ctx = _geometry.Open())
                {
                    ctx.BeginFigure(_points[0], _isFilled);

                    _points.RemoveAt(0);
                    foreach (var point in _points)
                    {
                        ctx.LineTo(point);
                    }

                    ctx.EndFigure(_isClosed);
                }

                end:
                _points = null;
                _geometry = null;
            }
        }
    }
}