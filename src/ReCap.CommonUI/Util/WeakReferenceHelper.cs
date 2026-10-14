using System;

namespace ReCap.CommonUI.Util
{
    internal static class WeakReferenceHelper
    {
        public static T PropertyGet<T>(ref WeakReference<T> reference, T fallback = null)
            where T
                : class
        {
            if (reference == null)
                return fallback;
            else if (!reference.TryGetTarget(out T result))
                return fallback;
            else
                return result;
        }


        public static void PropertySet<T>(ref WeakReference<T> reference, T value)
            where T
                : class
        {
            reference?.SetTarget(null);
            
            reference = value != null
                ? new(value)
                : null
            ;
        }
    }
}