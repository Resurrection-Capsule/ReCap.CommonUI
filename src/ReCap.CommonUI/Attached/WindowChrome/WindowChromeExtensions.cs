using System;
using Avalonia.Controls;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public static class WindowChromeExtensions
    {
        internal static Window GetHostWindow(this IWindowChromeImpl impl, bool throwIfFailed = false)
        {
            if (impl.Chrome.TryGetHostWindow(out Window hostWindow))
                return hostWindow;
            else if (throwIfFailed)
                throw new Exception();
            else
                return null;
        }
        internal static bool TryGetHostWindow(this IWindowChromeImpl impl, out Window hostWindow)
        {
            hostWindow = impl.GetHostWindow(throwIfFailed: false);
            return hostWindow != null;
        }


        internal static bool TryGetHostWindow(this WindowChrome chrome, out Window hostWindow)
        {
            if (chrome != null)
            {
                hostWindow = chrome.HostWindow;
                return hostWindow != null;
            }
            else
            {
                hostWindow = null;
                return false;
            }
        }




        /// <inheritdoc cref="WindowChrome.GetChrome" />
        public static WindowChrome GetChrome(this Window window)
            => WindowChrome.GetChrome(window);

        internal static bool TryGetChrome(this TopLevel topLevel, out WindowChrome chrome)
        {
            if (topLevel == null)
                goto fail;
            if (topLevel is not Window window)
                goto fail;

            chrome = GetChrome(window);
            return chrome != null;

            fail:
            chrome = null;
            return false;
        }
        internal static T GetChromePlatformImpl<T>(this Window window)
            where T
                : IWindowChromeImpl
            => window
                .GetChrome()
                .GetPlatformImpl<T>()
            ;
        internal static T GetPlatformImpl<T>(this WindowChrome chrome)
            where T
                : IWindowChromeImpl
            => (T)chrome.PlatformImpl;


        /// <inheritdoc cref="WindowChrome.GetHint" />
        public static ManagedChromeHint GetHint(this Window window)
            => WindowChrome.GetHint(window);
        /// <inheritdoc cref="WindowChrome.SetHint" />
        public static void SetHint(this Window window, ManagedChromeHint value)
            => WindowChrome.SetHint(window, value);


        /// <inheritdoc cref="WindowChrome.GetShowTitleHint" />
        public static ManagedChromeElementHint GetShowTitleHint(this Window window)
            => WindowChrome.GetShowTitleHint(window);
        /// <inheritdoc cref="WindowChrome.SetShowTitleHint" />
        public static void SetShowTitleHint(this Window window, ManagedChromeElementHint value)
            => WindowChrome.SetShowTitleHint(window, value);


        /// <inheritdoc cref="WindowChrome.GetShowIconHint" />
        public static ManagedChromeElementHint GetShowIconHint(this Window window)
            => WindowChrome.GetShowIconHint(window);
        /// <inheritdoc cref="WindowChrome.SetShowIconHint" />
        public static void SetShowIconHint(this Window window, ManagedChromeElementHint value)
            => WindowChrome.SetShowIconHint(window, value);


        /// <inheritdoc cref="WindowChrome.GetOverrideCaptionButtonRoles" />
        public static CaptionButtonRolesPair? GetOverrideCaptionButtonRoles(this Window window)
            => WindowChrome.GetOverrideCaptionButtonRoles(window);
        /// <inheritdoc cref="WindowChrome.SetOverrideCaptionButtonRoles" />
        public static void SetOverrideCaptionButtonRoles(this Window window, CaptionButtonRolesPair? value)
            => WindowChrome.SetOverrideCaptionButtonRoles(window, value);


        public static bool GetIsChromeManaged(this Window window)
            => window.GetChrome().IsChromeManaged;
        public static bool GetIsTitleVisible(this Window window)
            => window.GetChrome().IsTitleVisible;
        public static bool GetIsIconVisible(this Window window)
            => window.GetChrome().IsIconVisible;
    }
}