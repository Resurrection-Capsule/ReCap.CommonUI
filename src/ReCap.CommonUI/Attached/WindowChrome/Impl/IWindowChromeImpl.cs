using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Threading;
using ReCap.CommonUI.Controls.AppearanceHacks;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal interface IWindowChromeImpl
    {
        bool CanUseManagedWindowChrome
        {
            get;
        }

        bool PrefersManagedWindowChrome
        {
            get;
        }

        bool DefaultShowCaptionIcon
        {
            get;
        }

        bool DefaultShowCaptionText
        {
            get;
        }

        CaptionButtonRolesPair DefaultCaptionButtonRoles
        {
            get;
        }

        IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get;
        }


        void Init();


        void OnWindowAttached(Window window);
        void OnWindowDetached(Window window);


        bool GetDesiredManagedChrome(Window window, ManagedChromeHint chromeMode);
        void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, Action<bool> applyUseManagedChrome);
        bool ExecuteButton(Window window, CaptionButtonClickEventArgs e, Action roleAction);
        void Prepare(CaptionButton button);
    }


    internal static class DEFAULT_IWindowChromeImpl
    {
        public static void ApplyDesiredManagedChrome_IMPL(
            IWindowChromeImpl impl, Window window, bool desiredManagedChrome
            , bool fallbackToSystemDecorationsProperty
            , Action<bool> applyUseManagedChrome
        )
        {
            bool oldIsExtendedIntoWindowDecorations = window.IsExtendedIntoWindowDecorations;


            Dispatcher.UIThread.Invoke(() =>
            {
                if (fallbackToSystemDecorationsProperty)
                {
                    if (desiredManagedChrome && !window.IsExtendedIntoWindowDecorations)
                        window.SystemDecorations = SystemDecorations.None;
                    else if ((!desiredManagedChrome) && !oldIsExtendedIntoWindowDecorations)
                        window.SystemDecorations = SystemDecorations.Full;
                }


                Dispatcher.UIThread.Invoke(() =>
                {
                    applyUseManagedChrome(window.IsExtendedIntoWindowDecorations || desiredManagedChrome);
                });
            });
        }


        public static bool ExecuteButton_IMPL(IWindowChromeImpl impl, Window window, CaptionButtonClickEventArgs e, Action roleAction)
        {
            if (roleAction != null)
                return false;

            switch (e.Role)
            {
                case CaptionButtonRole.Minimize:
                case CaptionButtonRole.Maximize:
                case CaptionButtonRole.FullScreen:
                case CaptionButtonRole.Close:
                    roleAction();
                    return true;
            }

            return false;
        }


        public static bool GetDesiredManagedChrome_IMPL(IWindowChromeImpl impl, Window window, ManagedChromeHint chromeMode)
            => chromeMode switch
            {
                ManagedChromeHint.WheneverPossible => impl.CanUseManagedWindowChrome,
                ManagedChromeHint.Auto => impl.PrefersManagedWindowChrome,
                _ => false,
            };


        public static void Prepare_IMPL(IWindowChromeImpl impl, CaptionButton button)
            => button.UseManagedToolTip = button.Role != CaptionButtonRole.WindowMenu;
    }
}