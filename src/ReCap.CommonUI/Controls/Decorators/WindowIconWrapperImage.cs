#if NO
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;
using ReCap.CommonUI.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReCap.CommonUI.Controls.Decorators
{
    public sealed class PlatformIconWrapperImage
        : AvaloniaObject
        , IPresentableImage
    {
        /// <summary>
        /// Defines the <see cref="Icon"/> property.
        /// </summary>
        public static readonly StyledProperty<IPlatformIcon> IconProperty =
            Window.IconProperty.AddOwner<PlatformIconWrapperImage>();
        /// <summary>
        /// Gets or sets the icon that will be displayed.
        /// </summary>
        [Content]
        public IPlatformIcon Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }




        static PlatformIconWrapperImage()
        {
            IconProperty.Changed.AddClassHandler<PlatformIconWrapperImage>((s, e) => s.OnIconPropertyChanged(e));
        }


        public PlatformIconWrapperImage()
            : base()
        {}




        static readonly Size _SIZE_EMPTY = new(0d, 0d);
        public Size MeasureOverride(ImagePresenter presenter, Size availableSize, Size baseSize)
        {
            var icon = Icon;
            if (icon == null)
                return _SIZE_EMPTY;

            var pxSizes = icon.PixelSizes;
            foreach (var pxSize in pxSizes)
            {

            }
        }

        public void Render(ImagePresenter presenter, DrawingContext context)
        {
            throw new NotImplementedException();
        }

        void OnIconPropertyChanged(AvaloniaPropertyChangedEventArgs args)
        {
            AffectRender?.Invoke(this, args);
            AffectMeasure?.Invoke(this, args);
        }


        public event EventHandler<AvaloniaPropertyChangedEventArgs> AffectMeasure;
        public event EventHandler<AvaloniaPropertyChangedEventArgs> AffectRender;
    }
}
#endif