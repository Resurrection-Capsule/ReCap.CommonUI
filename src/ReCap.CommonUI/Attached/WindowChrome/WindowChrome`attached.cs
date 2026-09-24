using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    partial class WindowChrome
    {
        /// <summary>
        /// The <see cref="CaptionButtonRole"/>s on the left end of a <see cref="Window"/>'s managed chrome's title bar.
        /// </summary>
        public static readonly AttachedProperty<CaptionButtonRoles> LeftCaptionButtonRolesProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, CaptionButtonRoles>("LeftCaptionButtonRoles", PlatformDefaultCaptionButtonRoles.Left);


        /// <summary>
        /// Gets the value of the LeftCaptionButtonRoles attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <returns>The value of the LeftCaptionButtonRoles attached property.</returns>
        public static CaptionButtonRoles GetLeftCaptionButtonRoles(Window window)
            => window.GetValue(LeftCaptionButtonRolesProperty);


        /// <summary>
        /// Sets the value of the LeftCaptionButtonRoles attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <param name="value">The value the the LeftCaptionButtonRoles attached property.</param>
        public static void SetLeftCaptionButtonRoles(Window window, CaptionButtonRoles value)
            => window.SetValue(LeftCaptionButtonRolesProperty, value);




        /// <summary>
        /// The <see cref="CaptionButtonRole"/>s on the right end of a <see cref="Window"/>'s managed chrome's title bar.
        /// </summary>
        public static readonly AttachedProperty<CaptionButtonRoles> RightCaptionButtonRolesProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, CaptionButtonRoles>("RightCaptionButtonRoles", PlatformDefaultCaptionButtonRoles.Right);


        /// <summary>
        /// Gets the value of the RightCaptionButtonRoles attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <returns>The value of the RightCaptionButtonRoles attached property.</returns>
        public static CaptionButtonRoles GetRightCaptionButtonRoles(Window window)
            => window.GetValue(RightCaptionButtonRolesProperty);


        /// <summary>
        /// Sets the value of the RightCaptionButtonRoles attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <param name="value">The value the the RightCaptionButtonRoles attached property.</param>
        public static void SetRightCaptionButtonRoles(Window window, CaptionButtonRoles value)
            => window.SetValue(RightCaptionButtonRolesProperty, value);




        public static readonly AttachedProperty<bool> ReserveCaptionAreaProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, bool>("ReserveCaptionArea", true);
        public static bool GetReserveCaptionArea(Window window)
            => window.GetValue(ReserveCaptionAreaProperty);
        public static void SetReserveCaptionArea(Window window, bool value)
            => window.SetValue(ReserveCaptionAreaProperty, value);


        public static readonly AttachedProperty<double> ReservedCaptionHeightProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, double>("ReservedCaptionHeight", 1d);
        public static double GetReservedCaptionHeight(Window window)
            => window.GetValue(ReservedCaptionHeightProperty);
        public static void SetReservedCaptionHeight(Window window, double value)
            => window.SetValue(ReservedCaptionHeightProperty, value);




        static void AttachedInit()
        {
            AffectsAll(new AvaloniaProperty[]
            {
                ReserveCaptionAreaProperty,
                ReservedCaptionHeightProperty,
            });
        }
    }
}
