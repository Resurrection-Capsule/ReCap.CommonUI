using Avalonia.Media;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Markup.Xaml
{
    internal sealed class TransformTypeConverter
        : TransformConverter
    {
        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            => ConverterHelper.ObjectToTransform(base.ConvertFrom(context, culture, value));
    }
}