using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Metadata;
using ReCap.CommonUI.Controls.Decorators;
using AvBitmap = Avalonia.Media.Imaging.Bitmap;

namespace ReCap.CommonUI.Util
{
    [NotClientImplementable]
    public interface IPlatformIcon
        : IPresentableImage
    {
        IEnumerable<PixelSize> PixelSizes
        {
            get;
        }


        //AvBitmap GetBitmapAtSize(PixelSize pxSize);
        AvBitmap this[PixelSize pxSize]
        {
            get;
        }
    }


    internal abstract class PlatformIconBase
        : IPlatformIcon
    {
#region IPlatformIcon
        protected Dictionary<PixelSize, AvBitmap> _bitmaps = new();
        public Dictionary<PixelSize, AvBitmap> Bitmaps
        {
            get => _bitmaps;
            internal set => _bitmaps = value;
        }


        static readonly IEnumerable<PixelSize> _PIXELSIZES_EMPTY = Array.Empty<PixelSize>();
        public IEnumerable<PixelSize> PixelSizes
        {
            get
            {
                var bitmaps = Bitmaps;
                if (bitmaps == null)
                    return _PIXELSIZES_EMPTY;

                return bitmaps.Keys;
            }
        }
        public AvBitmap this[PixelSize pxSize]
            => Bitmaps[pxSize];
#endregion


#region IPresentableImage
        static readonly Size _SIZE_EMPTY = new(0d, 0d);
        public Size MeasureOverride(ImagePresenter presenter, Size availableSize, Size baseSize)
            => TryMeasurePx(baseSize, out PixelSize pxSize)
                ? new(pxSize.Width, pxSize.Height)
                : _SIZE_EMPTY
            ;

        bool TryMeasurePx(Size baseSize, out PixelSize pxSize)
        {
            if (this.TryFindNearestSize(baseSize, out PixelSize optimalSize))
            {
                pxSize = optimalSize;
                return true;
            }

            var sizes = PixelSizes;
            if (sizes?.Any() == true)
            {
                pxSize = sizes.First();
                return true;
            }

            pxSize = default;
            return false;
        }

        public void Render(ImagePresenter presenter, DrawingContext context)
        {
            if (!TryMeasurePx(presenter.Bounds.Size, out PixelSize pxSize))
                return;
            if (!Bitmaps.TryGetValue(pxSize, out AvBitmap bitmap))
                return;

            Rect renderBounds = new(0d, 0d, pxSize.Width, pxSize.Height);
            context.DrawImage(bitmap, renderBounds);
        }

        void OnIconPropertyChanged(AvaloniaPropertyChangedEventArgs args)
        {
            AffectRender?.Invoke(this, args);
            AffectMeasure?.Invoke(this, args);
        }


        public event EventHandler<AvaloniaPropertyChangedEventArgs> AffectMeasure;
        public event EventHandler<AvaloniaPropertyChangedEventArgs> AffectRender;
#endregion


        /*
        public PlatformIconBase(AvBitmap bitmap)
            : this(new[] { bitmap })
        {}
        */
        public PlatformIconBase(params AvBitmap[] bitmaps)
            : this((IEnumerable<AvBitmap>)bitmaps)
        {}
        public PlatformIconBase(IEnumerable<AvBitmap> bitmaps)
        {
            var sortedBitmaps = bitmaps.OrderBy(x => x.PixelSize.ToInts().Min());
            foreach (AvBitmap bitmap in sortedBitmaps)
            {
                Bitmaps.Add(bitmap.PixelSize, bitmap);
            }
        }
    }
}