using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace ReCap.CommonUI.Converters
{
    public static class ThicknessToGridLengthsConverters
    {
        abstract class ThicknessToAxisDefinitionsConverterBase<TDef, TDefsList>
            : IValueConverter
            where TDef
                : DefinitionBase
            where TDefsList
                : DefinitionList<TDef>
        {
            public abstract TDefsList GetDefinitions(double near, double far);
            public abstract (double near, double far) GetRelevantSides(Thickness value);
            

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is not Thickness val)
                    return null;
                
                var (near, far) = GetRelevantSides(val);
                return GetDefinitions(near, far);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }


        public static readonly IValueConverter ToColumnDefinitions = new ThicknessToColumnDefinitionsConverter();
        sealed class ThicknessToColumnDefinitionsConverter
            : ThicknessToAxisDefinitionsConverterBase<ColumnDefinition, ColumnDefinitions>
        {
            public override ColumnDefinitions GetDefinitions(double near, double far)
                => ColumnDefinitions.Parse($"{near},*,{far}");
            public override (double near, double far) GetRelevantSides(Thickness value)
                => (value.Left, value.Right);
        }




        public static readonly IValueConverter ToRowDefinitions = new ThicknessToRowDefinitionsConverter();
        sealed class ThicknessToRowDefinitionsConverter
            : ThicknessToAxisDefinitionsConverterBase<RowDefinition, RowDefinitions>
        {
            public override RowDefinitions GetDefinitions(double near, double far)
                => RowDefinitions.Parse($"{near},*,{far}");
            public override (double near, double far) GetRelevantSides(Thickness value)
                => (value.Left, value.Right);
        }
    }
}
