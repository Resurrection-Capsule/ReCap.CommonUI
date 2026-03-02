using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;


namespace ReCap.CommonUI
{
    public partial class ReCapTheme
        : Styles
        , IResourceNode
    {
#nullable enable
        public ReCapTheme(IServiceProvider? sp = null)
#nullable restore
        {
            AvaloniaXamlLoader.Load(sp, this);
        }
    }
}