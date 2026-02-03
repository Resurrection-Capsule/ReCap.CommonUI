using Avalonia;
using Avalonia.ReactiveUI;
using System;

namespace ReCap.CommonUI.Demo
{
    static class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine("START");
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Console.WriteLine("Handler attached");

            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            Console.WriteLine("END");
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();


        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("========    AppDomain.CurrentDomain.UnhandledException    ========");

            /*
            Console.WriteLine($"\n\n{nameof(sender)} ({sender != null}):");
            Console.WriteLine(sender);

            Console.WriteLine($"\n\n{nameof(e)} ({e != null}):");
            Console.WriteLine(e);

            Console.WriteLine($"\n\n{nameof(e.ExceptionObject)} ({e.ExceptionObject != null}):");
            Console.WriteLine(e.ExceptionObject);
            */

            WriteLinesExceptionField(sender, nameof(sender));

            if (WriteLinesExceptionField(e, nameof(e)))
                WriteLinesExceptionField(e.ExceptionObject, nameof(e.ExceptionObject));
        }


        static bool WriteLinesExceptionField(object obj, string name)
        {
            if (obj == null)
            {
                Console.WriteLine($"{name}: null");
                return false;
            }

            Console.WriteLine($"{name}:");
            Console.WriteLine(obj);
            Console.WriteLine(string.Empty);
            return true;
        }
    }
}
