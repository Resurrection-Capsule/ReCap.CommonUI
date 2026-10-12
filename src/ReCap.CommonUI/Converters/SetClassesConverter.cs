using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Converters
{
    internal sealed class SetClassesConverter
        : IMultiValueConverter
    {
        public static readonly SetClassesConverter Instance = new();
        private SetClassesConverter()
        {}




        const string _SEPARATOR = ",";
        static readonly char[] _SEPARATORS = _SEPARATOR.ToCharArray();
        public object Convert(IList<object> values, Type targetType, object parameter, CultureInfo culture)
        {
            int valuesCount = values.Count;

            if ((!ConverterHelper.TryGetString(parameter, out string paramStr)) || string.IsNullOrWhiteSpace(paramStr))
                throw new ArgumentNullException(nameof(parameter));

            var names = paramStr.Contains(_SEPARATOR)
                ? paramStr.Split(_SEPARATORS, StringSplitOptions.None)
                : new[]
                {
                    paramStr,
                }
            ;
            int namesCount = names.Length;
            if (namesCount > valuesCount)
                throw new IndexOutOfRangeException($"{nameof(parameter)} has {namesCount} class names, but only {valuesCount} bindings were provided!");
            else if (namesCount < valuesCount)
                throw new IndexOutOfRangeException($"{nameof(parameter)} only has {namesCount} class names, but {valuesCount} bindings were provided!");


            var valuesArray = new bool[valuesCount];
            for (int i = 0; i < valuesCount; i++)
            {
                object raw = values[i];
                if (!ConverterHelper.TryGetBool(raw, out bool value))
                {
                    value = false;
                    Debug.WriteLine($"{nameof(SetClassesConverter)}: {nameof(MultiBinding)}.{nameof(MultiBinding.Bindings)}[{i}] did not produce a boolean!");
                }

                valuesArray[i] = value;
            }


            ClassValues classes = new();
            for (int i = 0; i < valuesCount; i++)
            {
                string name = names[i].Trim();
                bool value = valuesArray[i];

                classes.Add(name, value);
            }
            return classes;
        }
    }
}
