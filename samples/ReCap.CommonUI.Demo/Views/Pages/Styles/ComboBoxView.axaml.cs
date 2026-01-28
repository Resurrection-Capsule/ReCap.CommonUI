using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReCap.CommonUI.Demo.Views.Pages.Styles
{
    public partial class ComboBoxView
        : UserControl
    {
        public ComboBoxView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
