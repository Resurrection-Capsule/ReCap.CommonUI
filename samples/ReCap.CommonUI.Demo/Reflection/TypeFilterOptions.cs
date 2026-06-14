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


        public TypeFilterKindFlags IncludeTypeKinds
        {
            get;
            init;
        } = TypeFilterKindFlags.Class;


        public TypeFilterModifierFlags IncludeModifiers
        {
            get;
            init;
        } = TypeFilterModifierFlags.Public;


        public bool IncludeNested
        {
            get;
            init;
        } = false;


        public bool IncludeAbstract
        {
            get;
            init;
        } = false;


        public bool IncludeGeneric
        {
            get;
            init;
        } = false;


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