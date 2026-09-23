using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReCap.CommonUI.Demo.Views.Pages.Styles
{
    public partial class WindowView
        : UserControl
    {
        public WindowView()
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
