using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace ReCap.CommonUI.Converters
{
    public sealed class ItemsControlToSelectedItemHeightConverter
        : IValueConverter
    {
        public static readonly ItemsControlToSelectedItemHeightConverter Instance = new();
        private ItemsControlToSelectedItemHeightConverter()
        {}




        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ItemsControl itemsCtl = (ItemsControl)value;
            int index = NumberConvUtils.ObjectToInt(parameter);
            Control container = itemsCtl.ContainerFromIndex(index);
            if (container != null)
                return container.Bounds.Height;
            else
                return BindingOperations.DoNothing;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}