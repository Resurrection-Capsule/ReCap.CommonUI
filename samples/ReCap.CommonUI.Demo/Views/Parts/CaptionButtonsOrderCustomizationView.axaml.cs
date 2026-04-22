using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace ReCap.CommonUI.Demo.Views.Parts
{
    public partial class CaptionButtonsOrderCustomizationView
        : HeaderedContentControl
    {
        public CaptionButtonsOrderCustomizationView()
        {
            InitializeComponent();
        }


        void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


        protected override Type StyleKeyOverride
            => typeof(HeaderedContentControl);
    }
}
