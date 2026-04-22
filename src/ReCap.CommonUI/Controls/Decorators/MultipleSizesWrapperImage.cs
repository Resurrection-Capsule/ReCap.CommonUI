using Avalonia;
using Avalonia.Media;
using System;
using ReCap.CommonUI.Util;

//using Extensions = ReCap.CommonUI.Util.Extensions;
using AvBitmap = Avalonia.Media.Imaging.Bitmap;
using IAvBitmapsEnumerable = System.Collections.Generic.IEnumerable<Avalonia.Media.Imaging.Bitmap>;

namespace ReCap.CommonUI.Controls.Decorators
{
    public sealed class MultipleSizesWrapperImage
        : AvaloniaObject
        , IPresentableImage
    {
        static readonly Point _POINT_EMPTY = new(0d, 0d);
        static readonly Size _SIZE_EMPTY = new(0d, 0d);
        const StretchDirection _DEFAULT_StretchDirection = StretchDirection.Both;
        public static readonly DirectProperty<MultipleSizesWrapperImage, StretchDirection> StretchDirectionProperty
            = AvaloniaProperty.RegisterDirect<MultipleSizesWrapperImage, StretchDirection>(nameof(StretchDirection)
                , getter: x => x.StretchDirection
                , setter: (x, v) => x.StretchDirection = v
            );
        StretchDirection _isHostWindowActive = _DEFAULT_StretchDirection;
        public StretchDirection StretchDirection
        {
            get => _isHostWindowActive;
            set => SetAndRaise(StretchDirectionProperty, ref _isHostWindowActive, value);
        }




        static MultipleSizesWrapperImage()
        {
            StretchDirectionProperty.Changed.AddClassHandler<MultipleSizesWrapperImage>((s, e) => s.OnAnyPropertyChanged(e));
        }




        readonly IAvBitmapsEnumerable _variants;
        AvBitmap _currentVariant = null;
        //Rect _renderRect = default;
        public MultipleSizesWrapperImage(IAvBitmapsEnumerable variants)
            : base()
        {
            _variants = variants;
        }
        public MultipleSizesWrapperImage(params AvBitmap[] variants)
            : this((IAvBitmapsEnumerable)variants)
        {}
        public MultipleSizesWrapperImage(IAvBitmapsEnumerable sizes, StretchDirection stretchDirection)
            : this(sizes)
        {
            StretchDirection = stretchDirection;
        }




        public Size MeasureOverride(ImagePresenter presenter, Size availableSize, Size baseSize)
        {
            if (_variants == null)
                return _SIZE_EMPTY;

            baseSize.Deconstruct(out double width, out double height);
            PixelSize desiredSize = new(Helpers.RoundToInt(width), Helpers.RoundToInt(height));

            if (PlatformIconHelper.TryGetAtSize(_variants, desiredSize, out _currentVariant))
            {
                var size = _currentVariant.Size;
                //_renderRect = new Rect(_POINT_EMPTY, baseSize).CenterRect(new(_POINT_EMPTY, size));

                return size;
            }

            //_renderRect = default;
            _currentVariant = null;
            return _SIZE_EMPTY;
        }


        public void Render(ImagePresenter presenter, DrawingContext context)
        {
            if (_currentVariant == null)
                return;

            context.DrawImage(_currentVariant, new(_POINT_EMPTY, presenter.Bounds.Size));
        }


        void OnAnyPropertyChanged(AvaloniaPropertyChangedEventArgs args)
        {
            AffectRender?.Invoke(this, args);
            AffectMeasure?.Invoke(this, args);
        }


        public event EventHandler<AvaloniaPropertyChangedEventArgs> AffectMeasure;
        public event EventHandler<AvaloniaPropertyChangedEventArgs> AffectRender;
    }
}