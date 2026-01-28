using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReCap.CommonUI.Demo.Views.Pages.Styles
{
    public partial class WindowChromeView
        : UserControl
    {
        public WindowChromeView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
