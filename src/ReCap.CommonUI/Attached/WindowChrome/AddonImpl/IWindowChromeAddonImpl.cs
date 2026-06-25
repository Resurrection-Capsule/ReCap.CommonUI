using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal interface IWindowChromeAddonImpl
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

        CaptionButtonRolesPair DefaultCaptionButtons
        {
            get;
        }

        IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get;
        }


        void Init();


        bool GetDesiredManagedChrome(Window window, ManagedChromeMode chromeMode);
        void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, Action<bool> applyUseManagedChrome);
        void ExecuteExtendedCaptionButton(Window window, CaptionButtonClickEventArgs e);
    }


    internal static class DefaultWindowChromeAddonImpl
    {
        public static CaptionButtonRolesPair DefaultCaptionButtons_Default(IWindowChromeAddonImpl impl)
            => new()
            {
                Left = new()
                {
                    CaptionButtonRole.Menu,
                },
                Right = new()
                {
                    CaptionButtonRole.Minimize,
                    CaptionButtonRole.Maximize,
                    CaptionButtonRole.Close,
                },
            };


        public static void ApplyDesiredManagedChrome_Default(
            IWindowChromeAddonImpl impl, Window window, bool desiredManagedChrome
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


        public static void ApplyDesiredManagedChrome_Default_Old(
            IWindowChromeAddonImpl impl, Window window, bool desiredManagedChrome, ref bool useManagedChrome
            , bool fallbackToSystemDecorationsProperty
        )
        {
            bool oldIsExtendedIntoWindowDecorations = window.IsExtendedIntoWindowDecorations;
            bool isUsingManagedChrome = default;


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
                    isUsingManagedChrome = window.IsExtendedIntoWindowDecorations || desiredManagedChrome;
                });
            });


            useManagedChrome = isUsingManagedChrome;
        }


        public static bool GetDesiredManagedChrome_Default(IWindowChromeAddonImpl impl, Window window, ManagedChromeMode chromeMode)
            => chromeMode switch
            {
                ManagedChromeMode.WheneverPossible => impl.CanUseManagedWindowChrome,
                ManagedChromeMode.Auto => impl.PrefersManagedWindowChrome,
                _ => false,
            };
    }
}