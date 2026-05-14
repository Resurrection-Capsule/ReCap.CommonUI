using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReCap.CommonUI.Demo.Views.Pages.Styles
{
    public partial class TabStripView
        : UserControl
    {
        public TabStripView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
