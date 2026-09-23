using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    partial class ManagedWindowChrome
    {
        public static readonly AttachedProperty<bool> MakeAvailableProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, bool>("MakeAvailable", false);
        internal static bool GetMakeAvailable(Window window)
            => window.GetValue(MakeAvailableProperty);
        internal static void SetMakeAvailable(Window window, bool value)
            => window.SetValue(MakeAvailableProperty, value);




#region Properties
        /// <summary>
        /// Whether a <see cref="Window"/> would prefer to use managed or system chrome.
        /// </summary>
        public static readonly AttachedProperty<ManagedChromeHint> HintProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, ManagedChromeHint>("Hint", ManagedChromeHint.Auto);
        /// <summary>
        /// Gets the value of the Hint attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <returns>The value of the Hint attached property.</returns>
        public static ManagedChromeHint GetHint(Window window)
            => window.GetValue(HintProperty);
        /// <summary>
        /// Sets the value of the Hint attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="control">The <see cref="Window"/>.</param>
        /// <param name="value">The value of the Hint attached property.</param>
        public static void SetHint(Window window, ManagedChromeHint value)
            => window.SetValue(HintProperty, value);


        /// <summary>
        /// Whether a <see cref="Window"/>'s managed chrome should show its <see cref="Window.Title"/>.
        /// </summary>
        public static readonly AttachedProperty<ManagedChromeElementHint> ShowTitleHintProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, ManagedChromeElementHint>("ShowTitleHint", ManagedChromeElementHint.Auto);
        /// <summary>
        /// Gets the value of the ShowTitleHint attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <returns>The value of the ShowTitleHint attached property.</returns>
        public static ManagedChromeElementHint GetShowTitleHint(Window window)
            => window.GetValue(ShowTitleHintProperty);
        /// <summary>
        /// Sets the value of the ShowTitleHint attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <param name="value">The value of the ShowTitleHint attached property.</param>
        public static void SetShowTitleHint(Window window, ManagedChromeElementHint value)
            => window.SetValue(ShowTitleHintProperty, value);


        /// <summary>
        /// Whether a <see cref="Window"/>'s managed chrome should show its <see cref="Window.Icon"/>.
        /// </summary>
        /// <remarks>
        /// Only applicable if the <see cref="CaptionButtonRoles.WindowMenu"/> is the last element of LeftCaptionButtonRoles and/or the first element of RightCaptionButtonRoles, since <see cref="Window.Icon"/> is displayed on a <see cref="CaptionButtonRoles.WindowMenu"/> button, if at all.
        /// </remarks>
        public static readonly AttachedProperty<ManagedChromeElementHint> ShowIconHintProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, ManagedChromeElementHint>("ShowIconHint", ManagedChromeElementHint.Auto);
        /// <summary>
        /// Gets the value of the ShowIconHint attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <returns>The value of the ShowIconHint attached property.</returns>
        public static ManagedChromeElementHint GetShowIconHint(Window window)
            => window.GetValue(ShowIconHintProperty);
        /// <summary>
        /// Sets the value of the ShowIconHint attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <param name="value">The value the the ShowIconHint attached property.</param>
        public static void SetShowIconHint(Window window, ManagedChromeElementHint value)
            => window.SetValue(ShowIconHintProperty, value);


        /// <summary>
        /// The <see cref="CaptionButtonRole"/>s on the left end of a <see cref="Window"/>'s managed chrome's title bar.
        /// </summary>
        public static readonly AttachedProperty<CaptionButtonRoles> LeftCaptionButtonRolesProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, CaptionButtonRoles>("LeftCaptionButtonRoles", PlatformDefaultCaptionButtonRoles.Left);
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
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, CaptionButtonRoles>("RightCaptionButtonRoles", PlatformDefaultCaptionButtonRoles.Right);
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
#endregion




#region Read-only properties
        /// <summary>
        /// Whether a <see cref="Window"/> is currently using managed chrome.
        /// </summary>
        public static readonly AttachedProperty<bool> IsChromeManagedProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, bool>("IsChromeManaged", false);
        /// <summary>
        /// Gets the value of the IsChromeManaged attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <returns>The value of the IsChromeManaged attached property.</returns>
        public static bool GetIsChromeManaged(Window window)
            => window.GetValue(IsChromeManagedProperty);
        internal static void SetIsChromeManaged(Window window, bool value)
            => window.SetValue(IsChromeManagedProperty, value);


        /// <summary>
        /// Whether a <see cref="Window"/> using managed chrome is showing its <see cref="Window.Title"/>.
        /// </summary>
        public static readonly AttachedProperty<bool> IsTitleVisibleProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, bool>("IsTitleVisible", true);
        /// <summary>
        /// Gets the value of the IsChromeManaged attached property on the specified <see cref="Window"/>.
        /// </summary>
        /// <param name="window">The <see cref="Window"/>.</param>
        /// <returns>The value of the IsChromeManaged attached property.</returns>
        public static bool GetIsTitleVisible(Window window)
            => window.GetValue(IsTitleVisibleProperty);
        internal static void SetIsTitleVisible(Window window, bool value)
            => window.SetValue(IsTitleVisibleProperty, value);


        /// <summary>
        /// Whether a <see cref="Window"/> using managed chrome is showing its <see cref="Window.Icon"/>.
        /// </summary>
        public static readonly AttachedProperty<bool> IsIconVisibleProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, bool>("IsIconVisible", true);
        public static bool GetIsIconVisible(Window window)
            => window.GetValue(IsIconVisibleProperty);
        internal static void SetIsIconVisible(Window window, bool value)
            => window.SetValue(IsIconVisibleProperty, value);
#endregion


        public static readonly AttachedProperty<bool> ReserveCaptionAreaProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, bool>("ReserveCaptionArea", true);
        public static bool GetReserveCaptionArea(Window window)
            => window.GetValue(ReserveCaptionAreaProperty);
        public static void SetReserveCaptionArea(Window window, bool value)
            => window.SetValue(ReserveCaptionAreaProperty, value);


        public static readonly AttachedProperty<double> ReservedCaptionHeightProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, double>("ReservedCaptionHeight", 1d);
        public static double GetReservedCaptionHeight(Window window)
            => window.GetValue(ReservedCaptionHeightProperty);
        public static void SetReservedCaptionHeight(Window window, double value)
            => window.SetValue(ReservedCaptionHeightProperty, value);




        public static readonly AttachedProperty<double> LeftCaptionButtonsWidthProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, double>("LeftCaptionButtonsWidth", 0d);
        public static double GetLeftCaptionButtonsWidth(Window window)
            => window.GetValue(LeftCaptionButtonsWidthProperty);
        internal static void SetLeftCaptionButtonsWidth(Window window, double value)
            => window.SetValue(LeftCaptionButtonsWidthProperty, value);


        public static readonly AttachedProperty<double> RightCaptionButtonsWidthProperty =
            AvaloniaProperty.RegisterAttached<ManagedWindowChrome, Window, double>("RightCaptionButtonsWidth", 0d);
        public static double GetRightCaptionButtonsWidth(Window window)
            => window.GetValue(RightCaptionButtonsWidthProperty);
        internal static void SetRightCaptionButtonsWidth(Window window, double value)
            => window.SetValue(RightCaptionButtonsWidthProperty, value);
    }
}
