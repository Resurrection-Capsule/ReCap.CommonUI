using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.MarkupExtensions;

namespace ReCap.CommonUI.Demo
{
    public static class Extensions
    {
        internal static T WithDock<T>(this T control, Dock dock)
            where T
                : Control
        {
            DockPanel.SetDock(control, dock);
            return control;
        }


        public static T WithTheme<T>(this T control, string themeKey)
            where T
                : Control
            => control.WithTheme(dynamicResource: new(themeKey));
        public static T WithTheme<T>(this T control, DynamicResourceExtension dynamicResource)
            where T
                : Control
        {
            control[!StyledElement.ThemeProperty] = dynamicResource;
            return control;
        }
    }
}