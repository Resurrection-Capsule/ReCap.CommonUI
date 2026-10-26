using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace ReCap.CommonUI.Markup.Xaml
{
    public sealed class ImportGeometryExtension
        : MarkupExtension
        , INotifyPropertyChanged
    {
#region Properties
        Geometry _geometry = null;
        public Geometry Geometry
        {
            get => _geometry;
            set => SetAndRaise(ref _geometry, value);
        }


        Transform _transform = null;
        [TypeConverter(typeof(TransformTypeConverter))]
        public Transform Transform
        {
            get => _transform;
            set => SetAndRaise(ref _transform, value);
        }


        void SetAndRaise<T>(ref T backingField, T value, [CallerMemberName] string propertyName = null)
        {
            backingField = value;
            PropertyChanged?.Invoke(this, new(propertyName));
        }
#endregion




        public ImportGeometryExtension()
            : base()
        {}
        public ImportGeometryExtension(Geometry geometry)
            : this()
        {
            Geometry = geometry;
        }
        public ImportGeometryExtension(Geometry geometry, Transform transform)
            : this(geometry)
        {
            Transform = transform;
        }


        public override object ProvideValue(IServiceProvider serviceProvider)
            => Geometry.Combine(Geometry, null, GeometryCombineMode.Union, Transform);




        public event PropertyChangedEventHandler PropertyChanged;
    }
}