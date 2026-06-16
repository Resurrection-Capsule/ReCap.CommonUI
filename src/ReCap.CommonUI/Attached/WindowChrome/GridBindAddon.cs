using System;
using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI.Attached
{
    internal abstract class GridBindAddon
        : AvaloniaObject
    {
        public static readonly AttachedProperty<ColumnDefinitions> ColumnDefsProperty =
            AvaloniaProperty.RegisterAttached<GridBindAddon, Grid, ColumnDefinitions>("ColumnDefs");
        public static ColumnDefinitions GetColumnDefs(Grid grid)
            => grid.GetValue(ColumnDefsProperty);
        public static void SetColumnDefs(Grid grid, ColumnDefinitions value)
            => grid.SetValue(ColumnDefsProperty, value);


        public static readonly AttachedProperty<RowDefinitions> RowDefsProperty =
            AvaloniaProperty.RegisterAttached<GridBindAddon, Grid, RowDefinitions>("RowDefs");
        public static RowDefinitions GetRowDefs(Grid grid)
            => grid.GetValue(RowDefsProperty);
        public static void SetRowDefs(Grid grid, RowDefinitions value)
            => grid.SetValue(RowDefsProperty, value);




        static GridBindAddon()
        {
            ColumnDefsProperty.Changed.AddClassHandler<Grid>(ColumnDefsProperty_Changed);
            RowDefsProperty.Changed.AddClassHandler<Grid>(RowDefsProperty_Changed);
        }


        static void ColumnDefsProperty_Changed(Grid grid, AvaloniaPropertyChangedEventArgs e)
            => grid.ColumnDefinitions = e.GetNewValue<ColumnDefinitions>();


        static void RowDefsProperty_Changed(Grid grid, AvaloniaPropertyChangedEventArgs e)
            => grid.RowDefinitions = e.GetNewValue<RowDefinitions>();
    }
}