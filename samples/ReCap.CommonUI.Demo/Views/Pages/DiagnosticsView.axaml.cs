using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ReCap.CommonUI.Demo.Views.Pages
{
    public partial class DiagnosticsView
        : UserControl
    {
        public DiagnosticsView()
        {
            InitializeComponent();
        }


        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
