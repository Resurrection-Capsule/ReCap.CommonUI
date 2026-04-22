using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.MarkupExtensions;

using DemoWindowViewModel = ReCap.CommonUI.Demo.ViewModels.Pages.Styles.WindowViewModel;
using AvControls = Avalonia.Controls.Controls;

namespace ReCap.CommonUI.Demo.Views.Pages.Styles
{
    public partial class WindowView
        : UserControl
    {
        //public const string DEMO_CONTENT_THEME_KEY = "DemoWindowContent";


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
