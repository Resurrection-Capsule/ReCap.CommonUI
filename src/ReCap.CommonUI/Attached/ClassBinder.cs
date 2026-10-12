using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI
{
    internal class ClassBinder
        : Layoutable
    {
        internal static Type CLASS_VALUES_TYPE = typeof(ClassValues);




        public static readonly AttachedProperty<ClassValues> ClassesProperty =
            AvaloniaProperty.RegisterAttached<ClassBinder, Control, ClassValues>("Classes", null);
        public static ClassValues GetClasses(Control control)
            => control.GetValue(ClassesProperty);
        public static void SetClasses(Control control, ClassValues value)
            => control.SetValue(ClassesProperty, value);




        static ClassBinder()
        {
            AffectsRender<Control>(ClassesProperty);

            ClassesProperty.Changed.AddClassHandler<Control>((s, e) =>
            {
                if (e.TryGetNewValue(out ClassValues classes))
                {
                    s.Classes.SetAll(classes);
                    Debug.WriteLine($"{ClassesProperty.Name}: {classes}");
                }
                else
                {
                    Debug.WriteLine($"{ClassesProperty.Name}: nope");
                }
            });
        }
    }
}