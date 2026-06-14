using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Markup.Xaml;
using ReCap.CommonUI.Attached.WindowChrome;

namespace ReCap.CommonUI.Demo.Views.Parts
{
    public partial class ManagedChromeHintCustomizationView
        : HeaderedContentControl
    {
        const ManagedChromeMode _Mode_DEFAULT = ManagedChromeMode.Auto;
        public static readonly DirectProperty<ManagedChromeHintCustomizationView, ManagedChromeMode> ModeProperty =
            AvaloniaProperty.RegisterDirect<ManagedChromeHintCustomizationView, ManagedChromeMode>(
                nameof(Mode)
                , s => s.Mode
                , (s, v) => s.Mode = v
                , unsetValue: _Mode_DEFAULT
            );
        ManagedChromeMode _mode = _Mode_DEFAULT;
        public ManagedChromeMode Mode
        {
            get => _mode;
            set => SetAndRaise(ModeProperty, ref _mode, value);
        }





        public ManagedChromeHintCustomizationView()
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
