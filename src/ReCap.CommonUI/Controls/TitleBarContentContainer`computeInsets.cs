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
                    var padding = Padding;
                    window.ExtendClientAreaTitleBarHeightHint = titleBarHeightHint + padding.Top + padding.Bottom;
                    return;
                }
                else
                {
                    window.ExtendClientAreaTitleBarHeightHint = WindowChromeAddon.GetDefaultTitleBarHeight(window);
                }
            }

            ComputedLeftInset = 0d;
            ComputedTopInset = 0d;
            ComputedRightInset = 0d;
            IsContentInsideReservedArea = false;
            _isUpdating = false;
        }


        bool TryComputeInsets(Window window, out double left, out double top, out double right, out bool isContentInsideReservedArea, out double titleBarHeightHint)
        {
            var bounds = Bounds;


            if (!this.TryTranslatePoint(_POINT_ZERO, window, out Point tl))
                goto fail;
            if (!this.TryTranslatePoint(bounds.Size.ToPoint(), window, out Point br))
                goto fail;
            if (!_insetReference.TryTranslatePoint(_POINT_ZERO, window, out Point tlContent))
                goto fail;


            bool isUsingManagedChrome = WindowChromeAddon.GetIsUsingManagedChrome(window);
            double reservedHeight = GetReservedHeight(window, isUsingManagedChrome);
            double upper = tl.Y;
            top = reservedHeight - upper;

            if (isUsingManagedChrome)
            {
                isContentInsideReservedArea = tlContent.Y < reservedHeight;
                if (isContentInsideReservedArea)
                {
                    left = WindowChromeAddon.GetLeftCaptionButtonsWidth(window) - tl.X;
                    right = WindowChromeAddon.GetRightCaptionButtonsWidth(window) - (window.Bounds.Width - br.X);

                    left = Math.Max(0d, left);
                    right = Math.Max(0d, right);
                }
                else
                {
                    left = 0d;
                    right = 0d;
                }
            }
            else
            {
                isContentInsideReservedArea = false;
                left = 0d;
                right = 0d;
            }


            titleBarHeightHint = br.Y;
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
            else if (WindowChromeAddon.GetReserveCaptionArea(window))
                return WindowChromeAddon.GetDefaultTitleBarHeight(window);
            else
                return 0d;
        }
    }
}