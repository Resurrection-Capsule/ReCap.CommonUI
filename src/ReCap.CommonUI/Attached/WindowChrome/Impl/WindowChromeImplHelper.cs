using System;
using Avalonia.Controls;
using Avalonia.Threading;
using ReCap.CommonUI.Controls.AppearanceHacks;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal static class WindowChromeImplHelper
    {
        public static void ApplyDesiredManagedChrome(
            IWindowChromeImpl impl, bool desiredManagedChrome
            , bool fallbackToSystemDecorationsProperty
            , Action<bool> applyUseManagedChrome
        )
        {
            Window window = impl.GetHostWindow();
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


                Dispatcher.UIThread.Invoke(() => applyUseManagedChrome(window.IsExtendedIntoWindowDecorations || desiredManagedChrome));
            });
        }



        public delegate bool TrafficLightCondition(CaptionButton button);
        static bool DEFAULT_PrepareTrafficLight_condition(CaptionButton button)
            => button.Role switch
            {
                CaptionButtonRole.Minimize => true,
                CaptionButtonRole.Maximize => true,
                CaptionButtonRole.Close => true,
                _ => false,
            };

        public static void AutoSetTrafficLight(this CaptionButton button)
            => button.SetTrafficLight(DEFAULT_PrepareTrafficLight_condition);
        public static void SetTrafficLight(this CaptionButton button, TrafficLightCondition condition)
            => button.SetTrafficLight(condition(button));
        public static void SetTrafficLight(this CaptionButton button, bool isTrafficLight)
            => ((IPseudoClassesProvider)button)
                .GetPseudoClasses()
                .Set(":traffic-light", isTrafficLight)
            ;
    }
}