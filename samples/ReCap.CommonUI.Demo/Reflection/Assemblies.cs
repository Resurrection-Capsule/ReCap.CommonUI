using System.Reflection;

namespace ReCap.CommonUI.Demo.Reflection
{
    public static class Assemblies
    {
        public static readonly Assembly AVALONIA_CONTROLS_ASSEMBLY = typeof(Avalonia.Controls.Control).Assembly;
        public static readonly Assembly COMMON_UI_ASSEMBLY = typeof(ReCapTheme).Assembly;
        public static readonly Assembly DEMO_ASSEMBLY = typeof(Assemblies).Assembly;
    }
}