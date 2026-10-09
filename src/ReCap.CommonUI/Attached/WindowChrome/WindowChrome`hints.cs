using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    partial class WindowChrome
    {
#region Properties
        /// <summary>
        /// Whether a <see cref="Window"/> would prefer to use managed or system chrome.
        /// </summary>
        public static readonly AttachedProperty<ManagedChromeHint> HintProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, ManagedChromeHint>("Hint", ManagedChromeHint.Auto);


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
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, ManagedChromeElementHint>("ShowTitleHint", ManagedChromeElementHint.Auto);


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
        /// Only applicable if the <see cref="CaptionButtonRole.WindowMenu"/> is the last element of <see cref="LeftCaptionButtonRoles"/> and/or the first element of <see cref="RightCaptionButtonRoles"/>, since <see cref="Window.Icon"/> is displayed on a <see cref="CaptionButtonRole.WindowMenu"/> button, if at all.
        /// </remarks>
        public static readonly AttachedProperty<ManagedChromeElementHint> ShowIconHintProperty =
            AvaloniaProperty.RegisterAttached<WindowChrome, Window, ManagedChromeElementHint>("ShowIconHint", ManagedChromeElementHint.Auto);


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
#endregion




        static void HintsInit()
        {
            ShowIconHintProperty.Changed.AddClassHandler<Window>(ShowIconHintProperty_Changed);
            ShowTitleHintProperty.Changed.AddClassHandler<Window>(ShowTitleHintProperty_Changed);
            HintProperty.Changed.AddClassHandler<Window>(HintProperty_Changed);

            AffectsAll(new[]
            {
                ShowIconHintProperty,
                ShowTitleHintProperty,
            });
        }


        static void HintProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
            => RefreshHint(window, e.GetNewValue<ManagedChromeHint>());


        static void RefreshShowIconAndTitle(Window window)
        {
            if (!TryGetChrome(window, out WindowChrome chrome))
                return;

            chrome.RefreshShowIcon();
            chrome.RefreshShowTitle();
        }


        static void ShowIconHintProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs args)
        {
            if (TryGetChrome(window, out WindowChrome chrome))
                chrome.RefreshShowIcon();
        }
        static void ShowTitleHintProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs args)
        {
            
            if (TryGetChrome(window, out WindowChrome chrome))
                chrome.RefreshShowTitle();
        }
    }
}
