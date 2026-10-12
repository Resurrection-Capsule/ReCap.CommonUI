using System;
using System.Collections.Generic;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using ReCap.CommonUI.Controls.AppearanceHacks;

namespace ReCap.CommonUI.Util
{
    internal static class PseudoClassesHelper
    {
        static readonly PropertyInfo _PSEUDOCLASSES_PROPERTY = typeof(StyledElement)
            .GetProperty("PseudoClasses", BindingFlags.NonPublic | BindingFlags.Instance)
        ;




        static IPseudoClasses GetPseudoClassesInternal(StyledElement element)
            => _PSEUDOCLASSES_PROPERTY.GetValue(element) is IPseudoClasses pseudoClasses
                ? pseudoClasses
                : null
            ;

        public static IPseudoClasses GetPseudoClasses(this StyledElement element, bool throwIfNotFound)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            var pseudoClasses = GetPseudoClassesInternal(element);
            if ((pseudoClasses == null) && throwIfNotFound)
                throw new NullReferenceException($"Couldn't get PseudoClasses for '{element}'!");

            return pseudoClasses;
        }


        public static bool TryGetPseudoClasses(this StyledElement element, out IPseudoClasses pseudoClasses)
        {
            pseudoClasses = null;
            if (element == null)
                return false;

            try
            {
                pseudoClasses = GetPseudoClassesInternal(element);
                return pseudoClasses != null;
            }
            catch (Exception exception)
            {
                // Fail """""gracefully""""" - cursed Reflection can explode without warning
#if DEBUG
                Console.WriteLine(exception);
#endif
            }

            return false;
        }




        public static void AllSetAll(this IEnumerable<IPseudoClassesProvider> providers, IEnumerable<KeyValuePair<string, bool>> pseudoClassValues)
        {
            foreach (var element in providers)
            {
                element.SetAll(pseudoClassValues);
            }
        }
        public static void SetAll(this IPseudoClassesProvider provider, IEnumerable<KeyValuePair<string, bool>> pseudoClassValues)
        {
            if (provider == null)
                throw new ArgumentNullException(nameof(provider));

            provider
                .GetPseudoClasses()
                .SetAll(pseudoClassValues)
            ;
        }
        public static void SetAll(this IPseudoClasses pseudoClasses, IEnumerable<KeyValuePair<string, bool>> pseudoClassValues)
        {
            foreach (var pair in pseudoClassValues)
            {
                pseudoClasses.Set(pair.Key, pair.Value);
            }
        }
    }
}