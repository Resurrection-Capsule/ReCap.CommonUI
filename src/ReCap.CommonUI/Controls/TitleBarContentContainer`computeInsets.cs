using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using ReCap.CommonUI.Attached.WindowChrome;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Controls
{
    partial class TitleBarContentContainer
    {
        Thickness ComputeInsetsThickness()
            => new(ComputedLeftInset, ComputedTopInset, ComputedRightInset, 0d);


        void InvalidateInsets(bool deferUpdate)
        {
            InvalidateMeasure();
            InvalidateArrange();
            InvalidateVisual();
            if (deferUpdate)
                UpdateInsetsDeferred();
            else
                UpdateInsets();
        }




        bool _isUpdating = false;
        void UpdateInsetsDeferred()
        {
            _isUpdating = true;
            Avalonia.Threading.Dispatcher.UIThread.Post(UpdateInsets);
        }
        void UpdateInsets()
        {
            _isUpdating = true;
            //Debug.WriteLine($"{nameof(UpdateInsets)}();");
            if (TryGetWindow(out Window window))
            {
                if (TryComputeInsets(window, out double left, out double top, out double right, out bool isContentInsideReservedArea, out double titleBarHeightHint))
                {
                    ComputedLeftInset = left;
                    ComputedTopInset = top;
                    ComputedRightInset = right;

                    IsContentInsideReservedArea = isContentInsideReservedArea;
                    window.ExtendClientAreaTitleBarHeightHint = titleBarHeightHint;
                    return;
                }
                else
                {
                    window.ExtendClientAreaTitleBarHeightHint = WindowChromeOptions.GetReservedCaptionHeight(window);
                }
            }

            ComputedLeftInset = 0d;
            ComputedTopInset = 0d;
            ComputedRightInset = 0d;
            IsContentInsideReservedArea = false;
            _isUpdating = false;
        }


        bool TryComputeInsets(Window window
            , out double left, out double top, out double right
            , out bool isContentInsideReservedArea
            , out double titleBarHeightHint
        )
        {
            var padding = Padding;
            var bounds = Bounds;
            var topLevelRootMargin = TopLevelRootMargin;


            if (!this.TryTranslatePoint(_POINT_ZERO, window, out Point tl))
            {
                Console.WriteLine($"{nameof(TitleBarContentContainer)}.{nameof(TryComputeInsets)}: FAIL 1");
                goto fail;
            }
            tl += new Point(topLevelRootMargin.Left, topLevelRootMargin.Top);

            if (!this.TryTranslatePoint(bounds.Size.ToPoint(), window, out Point br))
            {
                Console.WriteLine($"{nameof(TitleBarContentContainer)}.{nameof(TryComputeInsets)}: FAIL 2");
                goto fail;
            }
            br -= new Point(topLevelRootMargin.Right, topLevelRootMargin.Bottom);

            if (!_insetReference.TryTranslatePoint(_POINT_ZERO, window, out Point tlContent))
            {
                Console.WriteLine($"{nameof(TitleBarContentContainer)}.{nameof(TryComputeInsets)}: FAIL 3");
                goto fail;
            }


            bool isUsingManagedChrome = WindowChromeAddon.GetIsUsingManagedChrome(window);
            bool reserveCaptionArea = WindowChromeOptions.GetReserveCaptionArea(window);
            double reservedHeight = GetReservedHeight(window, isUsingManagedChrome);
            double upper = tl.Y;
            titleBarHeightHint = br.Y;

            if (isUsingManagedChrome)
            {
                if (reserveCaptionArea)
                {
                    isContentInsideReservedArea = tlContent.Y < reservedHeight;

                    top = reservedHeight - upper;
                    left = Math.Max(0d, WindowChromeAddon.GetLeftCaptionButtonsWidth(window) - tl.X);
                    right = Math.Max(0d, WindowChromeAddon.GetRightCaptionButtonsWidth(window) - (window.Bounds.Width - br.X));
                }
                else
                {
                    isContentInsideReservedArea = false;

                    top = 0d;
                    left = 0d;
                    right = 0d;
                }
            }
            else
            {
                isContentInsideReservedArea = false;
                //titleBarHeightHint += reservedHeight;
                top = reservedHeight;
                left = 0d;
                right = 0d;
            }


            //titleBarHeightHint += padding.Top + padding.Bottom;
            return true;


            fail:
            left = 0d;
            top = 0d;
            right = 0d;
            isContentInsideReservedArea = false;
            titleBarHeightHint = 0d;
            //Debug.WriteLine($"FAILED: {nameof(TryComputeInsets)}({nameof(window)}, out {nameof(left)} {left}, out {nameof(top)} {top}, out {nameof(right)} {right}, out {nameof(isContentInsideReservedArea)} {isContentInsideReservedArea}, out {nameof(titleBarHeightHint)} {titleBarHeightHint})");
            return false;
        }


        double GetReservedHeight(Window window, bool isUsingManagedChrome)
        {
            if (!isUsingManagedChrome)
                return 0d;
            else if (WindowChromeOptions.GetReserveCaptionArea(window))
                return WindowChromeOptions.GetReservedCaptionHeight(window);
            else
                return 0d;
        }
    }
}