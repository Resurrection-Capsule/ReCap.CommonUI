#if REFLECTION_HELPERS
using System;

namespace ReCap.CommonUI.Demo.Reflection
{
    public readonly struct TypeFilterOptions
    {
        public bool SearchRecursive
        {
            get;
            init;
        } = false;


        public TypeFilterTypeFlags IncludeTypes
        {
            get;
            init;
        } = TypeFilterTypeFlags.Class;


        public TypeFilterModifierFlags IncludeModifiers
        {
            get;
            init;
        } = TypeFilterModifierFlags.Public;


        public Type BaseType
        {
            get;
            init;
        } = null;


        public TypeFilterOptions()
        {}

        public static readonly TypeFilterOptions DEFAULT = new();
    }
}
#endif