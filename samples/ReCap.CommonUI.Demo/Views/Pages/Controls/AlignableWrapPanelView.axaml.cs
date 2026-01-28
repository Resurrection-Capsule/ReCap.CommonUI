using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ReCap.CommonUI.Controls;
using ReCap.CommonUI.Demo.ViewModels.Pages.Controls;

namespace ReCap.CommonUI.Demo.Views.Pages.Controls
{
    public partial class AlignableWrapPanelView
        : UserControl
    {

        public AlignableWrapPanelView()
        {
            InitializeComponent();
            AlignableWrapPanel panel = this.FindControl<AlignableWrapPanel>("Panel");
            //panel.Children.Clear();

            Dispatcher.UIThread.Post(() =>
            {
                var rectInfos = (DataContext as AlignableWrapPanelViewModel).Rectangles;
                foreach (var rectInfo in rectInfos)
                {
                    panel.Children.Add(rectInfo.ToRectangle());
                }
            }, DispatcherPriority.ApplicationIdle);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
