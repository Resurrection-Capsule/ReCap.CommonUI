using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;

namespace ReCap.CommonUI.Demo.Views.Parts
{
    public partial class ExtendClientAreaTitleBarHeightHintCustomizationView
        : HeaderedContentControl
    {
        const double _HeightHint_DEFAULT = 0d;
        public static readonly DirectProperty<ExtendClientAreaTitleBarHeightHintCustomizationView, double> HeightHintProperty =
            AvaloniaProperty.RegisterDirect<ExtendClientAreaTitleBarHeightHintCustomizationView, double>(
                nameof(HeightHint)
                , s => s.HeightHint
                , (s, v) => s.HeightHint = v
                , unsetValue: _HeightHint_DEFAULT
            );
        double _heightHint = _HeightHint_DEFAULT;
        public double HeightHint
        {
            get => _heightHint;
            set => SetAndRaise(HeightHintProperty, ref _heightHint, value);
        }





        public ExtendClientAreaTitleBarHeightHintCustomizationView()
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
