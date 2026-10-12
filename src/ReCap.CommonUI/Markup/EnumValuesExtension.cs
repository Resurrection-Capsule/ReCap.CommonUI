using System;
using Avalonia.Markup.Xaml;

namespace ReCap.CommonUI.Markup.Xaml
{
    public sealed class EnumValuesExtension
        : MarkupExtension
    {
        readonly Type _type;
        public EnumValuesExtension(Type type)
        {
            if (!type.IsEnum)
                throw new ArgumentException($"Bad {nameof(type)}: {type}.{nameof(Type.IsEnum)} must be true!");

            _type = type;
        }


        public override object ProvideValue(IServiceProvider serviceProvider)
            => Enum.GetValues(_type);
    }
}