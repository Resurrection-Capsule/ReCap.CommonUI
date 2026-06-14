using System.Linq;
using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI.Demo.Views.Pages
{
    public partial class PopOutWindow
        : Window
    {
        public PopOutWindow(object dataContext)
            : this(dataContext, dataContext)
        {}
        public PopOutWindow(object dataContext, object content)
        {
            DataContext = dataContext;

            if (content != null)
                Content = content;
            else if (Application.Current.DataTemplates.OfType<ViewLocator>().FirstOrDefault()?.TryBuild(dataContext, out Control view) ?? false)
                Content = view;
            else
                Content = dataContext;

            InitializeComponent();
        }
    }
}