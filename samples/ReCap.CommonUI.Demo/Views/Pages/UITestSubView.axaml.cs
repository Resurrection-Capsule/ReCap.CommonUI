using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReCap.CommonUI.Demo.ViewModels;

namespace ReCap.CommonUI.Demo.Views.Pages
{
    public partial class UITestSubView
        : UserControl
    {
        public TabsViewModelBase VM
        {
            get => DataContext as TabsViewModelBase;
        }
        
        public UITestSubView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
