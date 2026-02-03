#if REFLECTION_HELPERS
using System;

namespace ReCap.CommonUI.Demo.Reflection
{
    [Flags]
    public enum TypeFilterTypeFlags
        : byte
    {
        Class       = 0b00000001,
        Struct      = 0b00000010,
        Enum        = 0b00000100,
        Interface   = 0b00001000,
    }


    [Flags]
    public enum TypeFilterModifierFlags
        : byte
    {
        Public      = 0b00000000,
        NonPublic   = 0b00000001,
        Generic     = 0b00000010,
        Abstract    = 0b00000100,
    }
}
#endif