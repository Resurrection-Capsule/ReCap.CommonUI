using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using ReCap.CommonUI.Attached.WindowChrome;

namespace ReCap.CommonUI.Controls
{
    [TemplatePart(_PART_ContentPresenter, typeof(ContentPresenter), IsRequired = true)]
    [TemplatePart(_PART_InsetReference, typeof(Control), IsRequired = true)]
    [PseudoClasses(_PSEUD_MANAGED_DECO)]
    public sealed partial class TitleBarContentContainer
        : ContentControl
    {
        const string _PART_ContentPresenter = "PART_ContentPresenter";
        const string _PART_InsetReference = "PART_InsetReference";
        const string _PSEUD_MANAGED_DECO = ":managed_decorations";
        static readonly Thickness _THICKNESS_ZERO = new(0d);
        static readonly Point _POINT_ZERO = new(0d, 0d);




#region Properties
        public static readonly StyledProperty<Thickness> TopLevelRootMarginProperty =
            AvaloniaProperty.Register<TitleBarContentContainer, Thickness>(nameof(TopLevelRootMargin), new(0d));
        public Thickness TopLevelRootMargin
        {
            get => GetValue(TopLevelRootMarginProperty);
            set => SetValue(TopLevelRootMarginProperty, value);
        }


        public static readonly DirectProperty<TitleBarContentContainer, double> ComputedLeftInsetProperty
            = AvaloniaProperty.RegisterDirect<TitleBarContentContainer, double>(nameof(ComputedLeftInset)
                , getter: x => x.ComputedLeftInset
            );
        double _computedLeftInset = 0d;
        public double ComputedLeftInset
        {
            get => _computedLeftInset;
            private set => SetAndRaise(ComputedLeftInsetProperty, ref _computedLeftInset, value);
        }


        public static readonly DirectProperty<TitleBarContentContainer, double> ComputedTopInsetProperty
            = AvaloniaProperty.RegisterDirect<TitleBarContentContainer, double>(nameof(ComputedTopInset)
                , getter: x => x.ComputedTopInset
            );
        double _computedTopInset = 0d;
        public double ComputedTopInset
        {
            get => _computedTopInset;
            private set => SetAndRaise(ComputedTopInsetProperty, ref _computedTopInset, value);
        }


        public static readonly DirectProperty<TitleBarContentContainer, double> ComputedRightInsetProperty
            = AvaloniaProperty.RegisterDirect<TitleBarContentContainer, double>(nameof(ComputedRightInset)
                , getter: x => x.ComputedRightInset
            );
        double _computedRightInset = 0d;
        public double ComputedRightInset
        {
            get => _computedRightInset;
            private set => SetAndRaise(ComputedRightInsetProperty, ref _computedRightInset, value);
        }


        public static readonly DirectProperty<TitleBarContentContainer, Thickness> ComputedInsetsProperty
            = AvaloniaProperty.RegisterDirect<TitleBarContentContainer, Thickness>(nameof(ComputedInsets)
                , getter: x => x.ComputedInsets
            );
        Thickness _computedInsets = new(0d);
        public Thickness ComputedInsets
        {
            get => _computedInsets;
            private set => SetAndRaise(ComputedInsetsProperty, ref _computedInsets, value);
        }


        public static readonly DirectProperty<TitleBarContentContainer, bool> IsContentInsideReservedAreaProperty
            = AvaloniaProperty.RegisterDirect<TitleBarContentContainer, bool>(nameof(IsContentInsideReservedArea)
                , getter: x => x.IsContentInsideReservedArea
            );
        bool _isContentInsideReservedArea = false;
        public bool IsContentInsideReservedArea
        {
            get => _isContentInsideReservedArea;
            private set => SetAndRaise(IsContentInsideReservedAreaProperty, ref _isContentInsideReservedArea, value);
        }
#endregion




        static TitleBarContentContainer()
        {
            DockPanel.DockProperty.OverrideDefaultValue<TitleBarContentContainer>(Dock.Top);
            VerticalAlignmentProperty.OverrideDefaultValue<TitleBarContentContainer>(Avalonia.Layout.VerticalAlignment.Top);


            InsetAffectsStuff(new[]
            {
                ComputedLeftInsetProperty,
                ComputedTopInsetProperty,
                ComputedRightInsetProperty,
            });
            InsetAffectsStuff(IsContentInsideReservedAreaProperty);


            var props = new AvaloniaProperty[]
            {
                IsContentInsideReservedAreaProperty,
                ComputedInsetsProperty,
                TopLevelRootMarginProperty,
            };
            AffectsStuff(props);
            TopLevelRootMarginProperty.Changed.AddClassHandler<TitleBarContentContainer>(TopLevelRootMarginProperty_Changed);
        }


        static void TopLevelRootMarginProperty_Changed(TitleBarContentContainer container, AvaloniaPropertyChangedEventArgs args)
        {
            (Thickness oldValue, Thickness newValue) = args.GetOldAndNewValue<Thickness>();
            bool[] changes =
            {
                oldValue.Left != newValue.Left,
                oldValue.Top != newValue.Top,
                oldValue.Right != newValue.Right,
                oldValue.Bottom != newValue.Bottom,
            };
            Console.WriteLine($"{nameof(TopLevelRootMargin)}: '{oldValue}' => '{newValue}' ({string.Join(",", changes)})");
            container.UpdateLayout();
        }


        static void AffectsStuff(params AvaloniaProperty[] properties)
        {
            AffectsMeasure<TitleBarContentContainer>(properties);
            AffectsArrange<TitleBarContentContainer>(properties);
        }


        static void InsetAffectsStuff<T>(params DirectProperty<TitleBarContentContainer, T>[] properties)
        {
            AffectsStuff(properties);
            foreach (var prop in properties)
            {
                prop.Changed.AddClassHandler<TitleBarContentContainer>(InsetProperties_Changed);
            }
        }
        static void InsetProperties_Changed(TitleBarContentContainer tbrs, AvaloniaPropertyChangedEventArgs args)
            => tbrs.ComputedInsets = tbrs.ComputeInsetsThickness();


        static void AnyTitleBarProperty_Changed(TitleBarContentContainer tbrs, AvaloniaPropertyChangedEventArgs args)
            => tbrs.UpdateInsets();


        public TitleBarContentContainer()
            : base()
        {
            LayoutUpdated += This_LayoutUpdated;
        }

        void This_LayoutUpdated(object sender, EventArgs e)
        {
            if (!_isUpdating)
                UpdateInsetsDeferred();
        }

        Window _window = null;
        IDisposable _windowDisposable = null;
        bool TryGetWindow(out Window window)
        {
            if (_window != null)
            {
                window = _window;
                return true;
            }
            else
            {
                window = null;
                return false;
            }
        }




        bool _needsUpdateOnceTemplateApplied = false;
        ContentPresenter _contentPresenter = null;
        Control _insetReference = null;
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            var ns = e.NameScope;

            _contentPresenter = ns.Find<ContentPresenter>(_PART_ContentPresenter);

            _insetReference = ns.Find<Control>(_PART_InsetReference);


            if (_needsUpdateOnceTemplateApplied)
            {
                _needsUpdateOnceTemplateApplied = false;
                UpdateInsetsDeferred();
            }
        }


        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            if (e.Root is Window window)
            {
                Attach(window);
                UpdateInsetsDeferred();
            }
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            Detach();
            UpdateInsetsDeferred();
        }

        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateInsets();
        }


        void Attach(Window window)
        {
            Detach();

            _window = window;
            _windowDisposable = new CompositeDisposable()
            {
                _window
                    .GetObservable(WindowChromeAddon.IsUsingManagedChromeProperty)
                    .Subscribe(t =>
                    {
                        PseudoClasses.Set(_PSEUD_MANAGED_DECO, (_window != null) && WindowChromeAddon.GetIsUsingManagedChrome(_window));
                        PropertyObservablesHandler(t);
                    })
                ,
                _window
                    .GetObservable(WindowChromeAddon.LeftCaptionButtonsWidthProperty)
                    .Subscribe(PropertyObservablesHandler)
                ,
                _window
                    .GetObservable(WindowChromeAddon.RightCaptionButtonsWidthProperty)
                    .Subscribe(PropertyObservablesHandler)
                ,
                _window
                    .GetObservable(WindowChromeOptions.ReserveCaptionAreaProperty)
                    .Subscribe(PropertyObservablesHandler)
                ,
                _window
                    .GetObservable(WindowChromeOptions.ReservedCaptionHeightProperty)
                    .Subscribe(PropertyObservablesHandler)
                ,
            };
        }
        void PropertyObservablesHandler<T>(T _)
            => InvalidateInsets(true);


        void Detach()
        {
            _windowDisposable?.Dispose();
            _windowDisposable = null;
            _window = null;
            PseudoClasses.Set(_PSEUD_MANAGED_DECO, false);
        }
    }
}