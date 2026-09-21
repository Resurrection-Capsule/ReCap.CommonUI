using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Templates;
using Avalonia.Styling;
using ReCap.CommonUI.Attached.WindowChrome;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    public enum CaptionButtonClusterPosition
        : sbyte
    {
        Left = -1,
        Unknown = 0,
        Right = 1,
    }




    [PseudoClasses(_PSEUD_BUTTONS, _PSEUD_LEFT, _PSEUD_RIGHT)]
    public sealed partial class CaptionButtonCluster
        : ItemsControl
    {
        const string _PSEUD_BUTTONS = ":buttons";
        const string _PSEUD_LEFT = ":left";
        const string _PSEUD_RIGHT = ":right";




        int MergeTargetIndex
        {
            get => Position switch
            {
                CaptionButtonClusterPosition.Right => 0,
                CaptionButtonClusterPosition.Left => ItemCount - 1,
                _ => -1337,
            };
        }


        const CaptionButtonClusterPosition _DEFAULT_Position = CaptionButtonClusterPosition.Unknown;
        public static readonly DirectProperty<CaptionButtonCluster, CaptionButtonClusterPosition> PositionProperty
            = AvaloniaProperty.RegisterDirect<CaptionButtonCluster, CaptionButtonClusterPosition>(nameof(Position)
            , getter: o => o.Position
            , unsetValue: _DEFAULT_Position
        );
        CaptionButtonClusterPosition _position = _DEFAULT_Position;
        public CaptionButtonClusterPosition Position
        {
            get => _position;
            private set => SetAndRaise(PositionProperty, ref _position, value);
        }


        public static readonly StyledProperty<ControlTheme> WindowMenuItemContainerThemeProperty =
            AvaloniaProperty.Register<CaptionButton, ControlTheme>(nameof(WindowMenuItemContainerTheme), null);
        public ControlTheme WindowMenuItemContainerTheme
        {
            get => GetValue(WindowMenuItemContainerThemeProperty);
            set => SetValue(WindowMenuItemContainerThemeProperty, value);
        }


#nullable enable
        public static readonly StyledProperty<IDataTemplate?> WindowMenuItemTemplateProperty =
            AvaloniaProperty.Register<CaptionButton, IDataTemplate?>(nameof(WindowMenuItemTemplate), null);
        public IDataTemplate? WindowMenuItemTemplate
        {
            get => GetValue(WindowMenuItemTemplateProperty);
            set => SetValue(WindowMenuItemTemplateProperty, value);
        }
#nullable restore


        const bool _DEFAULT_HasWindowMenuAtMergeIndex = false;
        public static readonly DirectProperty<CaptionButtonCluster, bool> HasWindowMenuAtMergeIndexProperty
            = AvaloniaProperty.RegisterDirect<CaptionButtonCluster, bool>(nameof(HasWindowMenuAtMergeIndex)
            , getter: o => o.HasWindowMenuAtMergeIndex
            , unsetValue: _DEFAULT_HasWindowMenuAtMergeIndex
        );
        bool _hasWindowMenuAtMergeIndex = _DEFAULT_HasWindowMenuAtMergeIndex;
        public bool HasWindowMenuAtMergeIndex
        {
            get => _hasWindowMenuAtMergeIndex;
            private set => SetAndRaise(HasWindowMenuAtMergeIndexProperty, ref _hasWindowMenuAtMergeIndex, value);
        }


        const bool _DEFAULT_HasNonWindowMenuButtons = false;
        public static readonly DirectProperty<CaptionButtonCluster, bool> HasNonWindowMenuButtonsProperty
            = AvaloniaProperty.RegisterDirect<CaptionButtonCluster, bool>(nameof(HasNonWindowMenuButtons)
            , getter: o => o.HasNonWindowMenuButtons
            , unsetValue: _DEFAULT_HasNonWindowMenuButtons
        );
        bool _hasNonWindowMenuButtons = _DEFAULT_HasNonWindowMenuButtons;
        public bool HasNonWindowMenuButtons
        {
            get => _hasNonWindowMenuButtons;
            private set => SetAndRaise(HasNonWindowMenuButtonsProperty, ref _hasNonWindowMenuButtons, value);
        }




        static CaptionButtonCluster()
        {
            IsVisibleProperty.Changed.AddClassHandler<CaptionButtonCluster>(IsVisibleProperty_Changed);
            PositionProperty.Changed.AddClassHandler<CaptionButtonCluster>(PositionProperty_Changed);
        }

        static void IsVisibleProperty_Changed(CaptionButtonCluster cluster, AvaloniaPropertyChangedEventArgs args)
            => cluster.OnIsEffectivelyVisibleChanged();


        void OnIsEffectivelyVisibleChanged()
            => OnIsEffectivelyVisibleChanged(IsEffectivelyVisible);
        void OnIsEffectivelyVisibleChanged(bool isEffectivelyVisible)
        {
            double targetWidth;
            if (isEffectivelyVisible)
            {
                MaxWidth = double.PositiveInfinity;
                targetWidth = Bounds.Width;
            }
            else
            {
                MaxWidth = 0d;
                targetWidth = 0d;
            }
            IsEffectivelyVisibleChanged?.Invoke(this, new(isEffectivelyVisible, targetWidth));
        }


        static void PositionProperty_Changed(CaptionButtonCluster cluster, AvaloniaPropertyChangedEventArgs args)
            => cluster.OnPositionChanged(args.GetNewValue<CaptionButtonClusterPosition>());
        void OnPositionChanged(CaptionButtonClusterPosition position)
        {
            PseudoClasses.Set(_PSEUD_LEFT, position == CaptionButtonClusterPosition.Left);
            PseudoClasses.Set(_PSEUD_RIGHT, position == CaptionButtonClusterPosition.Right);
        }




        public CaptionButtonCluster()
            : base()
        {
            ItemsView.CollectionChanged += ItemsView_CollectionChanged;
        }


        void ItemsView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => OnItemsChanged();


        protected override void OnSizeChanged(SizeChangedEventArgs e)
        {
            base.OnSizeChanged(e);
            OnIsEffectivelyVisibleChanged();
        }


        IDisposable _attachmentDisposable = null;
        CaptionButtonsBroker _hostBroker = null;
        Window _hostWindow = null;
        internal void Attach(CaptionButtonsBroker broker, Window hostWindow, CaptionButtonClusterPosition position)
        {
            if (hostWindow == null)
                throw new ArgumentNullException(nameof(hostWindow));
            else if (_hostWindow != null)
                Detach();

            AvaloniaProperty<CaptionButtonRoles> captionButtonsProperty = position switch
            {
                CaptionButtonClusterPosition.Left => CaptionButtonsBroker.LeftCaptionButtonRolesProperty,
                CaptionButtonClusterPosition.Right => CaptionButtonsBroker.RightCaptionButtonRolesProperty,
                _ => throw new InvalidEnumArgumentException(nameof(position), (int)position, typeof(CaptionButtonClusterPosition)),
            };


            Position = position;
            _hostBroker = broker;
            _hostWindow = hostWindow;

            SizeChanged += This_SizeChanged;
            IsEffectivelyVisibleChanged += This_IsEffectivelyVisibleChanged;


            List<IDisposable> disposables = new()
            {
                this
                    .GetObservable(IsVisibleProperty)
                    .Subscribe(isVisible => RefreshSize())
                ,
                broker
                    .GetObservable(captionButtonsProperty)
                    .Subscribe(OnButtonRolesChanged)
                ,
                Bind(ItemsSourceProperty, broker[!captionButtonsProperty]),
            };


            disposables.AddRange(new[]
            {
                hostWindow
                    .GetObservable(Window.CanResizeProperty)
                    .Subscribe(HostWindow_PropertyChanged)
                ,
                hostWindow
                    .GetObservable(WindowBase.IsActiveProperty)
                    .Subscribe(HostWindow_PropertyChanged)
                ,
                hostWindow
                    .GetObservable(WindowBase.TopmostProperty)
                    .Subscribe(HostWindow_PropertyChanged)
                ,
                hostWindow
                    .GetObservable(Window.WindowStateProperty)
                    .Subscribe(HostWindow_WindowStateChanged)
                ,
            });


            _attachmentDisposable = new CompositeDisposable(disposables.ToArray());
            RefreshSize();
        }
        void OnButtonRolesChanged(IEnumerable<CaptionButtonRole> roles)
            => OnItemsChanged();
        void HostWindow_PropertyChanged<D>(D _)
            => OnItemsChanged();
        void HostWindow_WindowStateChanged(WindowState windowState)
            => OnItemsChanged();


        void OnItemsChanged()
        {
            bool hasNonWindowMenuButtons = false;
            bool hasWindowMenuAtMergeIndex = false;
            int mergeTargetIndex = MergeTargetIndex;
            int itemCount = ItemCount;

            for (int i = 0; i < itemCount; i++)
            {
                var role = (CaptionButtonRole)ItemsView[i];

                if (role != CaptionButtonRole.WindowMenu)
                    hasNonWindowMenuButtons = true;
                else if (i == mergeTargetIndex)
                    hasWindowMenuAtMergeIndex = true;
            }

            HasNonWindowMenuButtons = hasNonWindowMenuButtons;
            HasWindowMenuAtMergeIndex = hasWindowMenuAtMergeIndex;
        }




        internal void ExecuteCaptionButton(CaptionButton button, CaptionButtonClickEventArgs e)
            => _hostBroker?.ExecuteCaptionButton(button, e);


        internal void Detach()
        {
            _attachmentDisposable?.Dispose();
            _attachmentDisposable = null;
            if (_hostWindow == null)
                return;

            _hostBroker = null;
            _hostWindow = null;
            Position = CaptionButtonClusterPosition.Unknown;

            SizeChanged -= This_SizeChanged;
            IsEffectivelyVisibleChanged -= This_IsEffectivelyVisibleChanged;
        }


        void This_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ComputeNewCaptionButtonsSize(sender, e, out double newWidth))
                SetWindowCaptionButtonsSize(newWidth);
        }
        void This_IsEffectivelyVisibleChanged(object sender, CaptionButtonClusterUpdateEventArgs e)
        {
            if (ComputeNewCaptionButtonsSize(sender, e.TargetWidth, out double newWidth))
                SetWindowCaptionButtonsSize(newWidth);
        }



        const double _DEFAULT_RefreshSize_fallback = 0d;
        internal void RefreshSize(double fallback = _DEFAULT_RefreshSize_fallback)
            => PropagateNewSize(Bounds.Width, fallback);
        void PropagateNewSize(double newSize, double fallback = _DEFAULT_RefreshSize_fallback)
        {
            if (ComputeNewCaptionButtonsSize(this, newSize, out double newWidth))
            {
                SetWindowCaptionButtonsSize(newWidth);
                return;
            }

            double fallbackSize = fallback;

            if (fallbackSize >= 0d)
                SetWindowCaptionButtonsSize(fallbackSize);
        }


        bool ComputeNewCaptionButtonsSize(object sender, SizeChangedEventArgs e, out double result)
            => ComputeNewCaptionButtonsSize(sender, e.NewSize.Width, out result);
        bool ComputeNewCaptionButtonsSize(object sender, double clusterWidth, out double result)
        {
            if (sender is CaptionButtonCluster cluster)
                return ComputeNewCaptionButtonsSize(cluster, clusterWidth, out result);

            result = default;
            return false;
        }
        bool ComputeNewCaptionButtonsSize(CaptionButtonCluster cluster, double clusterWidth, out double result)
        {
            if (cluster == null)
                goto fail;
            else if (!cluster.IsEffectivelyVisible)
            {
                result = 0d;
                return true;
            }


            result = clusterWidth;
            return true;


            fail:
            result = 0d;
            return false;
        }


        void SetWindowCaptionButtonsSize(double newSize)
        {
            if (_hostWindow == null)
                return;

            switch (Position)
            {
                case CaptionButtonClusterPosition.Left:
                    ManagedWindowChrome.SetLeftCaptionButtonsWidth(_hostWindow, newSize);
                    break;

                case CaptionButtonClusterPosition.Right:
                    ManagedWindowChrome.SetRightCaptionButtonsWidth(_hostWindow, newSize);
                    break;
            }
        }




        internal event EventHandler<CaptionButtonClusterUpdateEventArgs> IsEffectivelyVisibleChanged;
    }




    internal sealed class CaptionButtonClusterUpdateEventArgs
        : EventArgs
    {
        public readonly bool IsEffectivelyVisible;
        public readonly double TargetWidth;
        public CaptionButtonClusterUpdateEventArgs(bool isEffectivelyVisible, double targetWidth)
        {
            IsEffectivelyVisible = isEffectivelyVisible;
            TargetWidth = targetWidth;
        }
    }
}