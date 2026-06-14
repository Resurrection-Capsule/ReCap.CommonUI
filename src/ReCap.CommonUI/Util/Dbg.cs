#if PRINT_PROPERTY_CHANGES && DEBUG
#define PRINT_PROP_CHANGES
#endif

using System;
using System.Diagnostics;
using Avalonia;

namespace ReCap.CommonUI.Util
{
    internal static class Dbg
    {
        public const string ATTRIB_CONDITIONAL_DEBUG = "DEBUG";





        [Conditional(ATTRIB_CONDITIONAL_DEBUG)]
        public static void DoChangedDebugOutput<TTarget>(params AvaloniaProperty[] properties)
            where TTarget
                : AvaloniaObject
        {
#if PRINT_PROP_CHANGES
            foreach (var property in properties)
            {
                property.Changed.AddClassHandler<TTarget>(ChangedDebugPropertyChangedHandler);
            }
#endif
        }


#if PRINT_PROP_CHANGES
        static void ChangedDebugPropertyChangedHandler<TTarget>(TTarget sender, AvaloniaPropertyChangedEventArgs e)
        {
            Console.WriteLine($"{typeof(TTarget).Name} '{sender}' PROPERTY '{e.Property.Name}' CHANGED:");
            Console.WriteLine($"    '{e.OldValue}' ==> '{e.NewValue}'");
        }
#endif
    }
}