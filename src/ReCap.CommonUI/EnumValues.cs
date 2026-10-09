using System;
using Avalonia.Markup.Xaml;

namespace ReCap.CommonUI
{
    public sealed class EnumValues
        : MarkupExtension
    {
        readonly Type _type;
        public EnumValues(Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException($"Bad {nameof(type)}: {type}.{nameof(Type.IsEnum)} must be true!");

            _type = type;
        }


        public override object ProvideValue(IServiceProvider serviceProvider)
            => Enum.GetValues(_type);
    }
}