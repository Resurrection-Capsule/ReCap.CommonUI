using System;

namespace ReCap.CommonUI.Demo.Reflection
{
    [Flags]
    public enum TypeFilterKindFlags
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
        Public      = 0b00000001,
        Internal    = 0b00000010,
        Protected   = 0b00000100,
        Private     = 0b10000000,
    }
}