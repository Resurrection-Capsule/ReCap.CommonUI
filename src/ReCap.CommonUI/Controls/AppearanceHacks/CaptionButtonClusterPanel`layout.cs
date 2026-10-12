using System;
using System.Collections.Generic;
using Avalonia;
using ReCap.CommonUI.Attached.WindowChrome;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    partial class CaptionButtonClusterPanel
    {
        /// <summary>
        /// General StackPanel layout behavior is to grow unbounded in the "stacking" direction (Size To Content).
        /// Children in this dimension are encouraged to be as large as they like.  In the other dimension,
        /// StackPanel will assume the maximum size of its children.
        /// </summary>
        /// <param name="availableSize">Constraint</param>
        /// <returns>Desired size</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            double stackDesiredWidth = 0d;
            double stackDesiredHeight = 0d;

            if (!TryGetCaptionButtons(Children, out List<CaptionButton> buttons, out int childrenCount))
                goto end;

            if (!Position.Evaluate(out bool left, out bool right))
                goto end;

            // Allow children as much size as they want along the stack.
            Size layoutSlotSize = availableSize.WithWidth(double.PositiveInfinity);

            // Iterate through children.
            int lastIdx = childrenCount - 1;
            for (int i = 0; i < childrenCount; i++)
            {
                CaptionButton button = buttons[i];


                // Measure the button.
                button.Measure(layoutSlotSize);
                Size buttonDesiredSize = button.DesiredSize;

                // Accumulate button size.
                stackDesiredWidth += buttonDesiredSize.Width;

                if (right || (i > 0))
                    stackDesiredWidth += button.LeftGap;

                if (left || (i < lastIdx))
                    stackDesiredWidth += button.RightGap;

                stackDesiredHeight = Math.Max(stackDesiredHeight, buttonDesiredSize.Height);
            }

            end:
            return new(stackDesiredWidth, stackDesiredHeight);
        }


        /// <summary>
        /// Content arrangement.
        /// </summary>
        /// <param name="finalSize">Arrange size</param>
        protected override Size ArrangeOverride(Size finalSize)
        {
            finalSize.Deconstruct(out double finalWidth, out double finalHeight);
            if (!TryGetCaptionButtons(Children, out List<CaptionButton> buttons, out int childrenCount))
                goto end;

            double buttonX = 0d;
            double buttonWidth = 0d;

            // Arrange and Position Children.
            for (int i = 0; i < childrenCount; i++)
            {
                CaptionButton button = buttons[i];

                /*
                if (right || (i > 0))
                    buttonX += button.LeftGap;

                buttonX += buttonWidth;
                */
                buttonX += button.LeftGap + buttonWidth;
                buttonWidth = button.DesiredSize.Width;
                finalHeight = Math.Max(finalHeight, button.DesiredSize.Height);

                button.Arrange(new(buttonX, 0d, buttonWidth, finalHeight));

                /*
                if (left || (i < lastIdx))
                    buttonWidth += button.RightGap;
                */
                buttonWidth += button.RightGap;
            }
            finalWidth = Math.Max(finalWidth, buttonWidth);

            end:
            return new(finalWidth, finalHeight);
        }
    }
}