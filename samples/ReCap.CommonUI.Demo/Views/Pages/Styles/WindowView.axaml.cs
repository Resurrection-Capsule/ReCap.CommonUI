using System;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ReCap.CommonUI.Demo.ViewModels.Pages.Styles;

namespace ReCap.CommonUI.Demo.Views.Pages.Styles
{
    public partial class WindowView
        : UserControl
    {
        public WindowView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
#if DROP_OUTSIDE_HACK
            AttachItemsControl(this.FindControl<ItemsControl>("LeftCaptionButtons"));
            AttachItemsControl(this.FindControl<ItemsControl>("RightCaptionButtons"));
#endif
        }


#if DROP_OUTSIDE_HACK
        static void AttachItemsControl(ItemsControl itemsCtl)
        {
            /*
            if (itemsCtl.DataContext is not WindowViewModel.CaptionButtonsSide captionButtonsSide)
                return;
            */
            var captionButtonsSide = itemsCtl.DataContext as WindowViewModel.CaptionButtonsSide;
            captionButtonsSide.Attach(itemsCtl);
        }
#endif
    }
}
