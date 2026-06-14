using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using ReCap.CommonUI.Demo.ViewModels.Pages.Controls;

namespace ReCap.CommonUI.Demo.Views.Pages.Controls
{
    public partial class TitleBarContentContainerView
        : UserControl
    {
        static void UIThreadRun(Action action)
            => Avalonia.Threading.Dispatcher.UIThread.Post(action, Avalonia.Threading.DispatcherPriority.Render);


        readonly Control _tbContent;
        public TitleBarContentContainerView()
        {
            InitializeComponent();
            _tbContent = this.FindControl<Control>("TitleBarContent");
            UIThreadRun(() =>
            {
                _tbContent.InvalidateMeasure();
                UIThreadRun(() =>
                {
                    TitleBarContentContainerViewModel vm = (TitleBarContentContainerViewModel)DataContext;
                    vm.ContentHeight = _tbContent.DesiredSize.Height;
                });
            });
        }

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }




        public static readonly IValueConverter ContentHeightConverter = new TitleBarContentHeightConverter();
        sealed class TitleBarContentHeightConverter
            : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                double height = (double)value;
                if (height < 0d)
                    return BindingOperations.DoNothing;
                else
                    return height;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }
    }
}
