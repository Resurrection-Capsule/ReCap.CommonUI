using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using Avalonia.Data;
using Avalonia.Data.Converters;
using ReCap.CommonUI.Attached.WindowChrome;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    public sealed partial class CaptionButtonsBroker
        : CaptionButtons
    {
        new internal Window HostWindow
        {
            get => base.HostWindow;
        }
        internal bool TryGetHostWindow(out Window hostWindow)
        {
            hostWindow = HostWindow;
            return hostWindow != null;
        }


        TitleBar2 _hostTitleBar = null;
        internal bool TryGetHostTitleBar(out TitleBar2 hostTitleBar)
        {
            hostTitleBar = _hostTitleBar;
            return hostTitleBar != null;
        }
        


#nullable enable
        internal static readonly DirectProperty<CaptionButtonsBroker, CaptionButtonRoles> LeftCaptionButtonRolesProperty =
                AvaloniaProperty.RegisterDirect<CaptionButtonsBroker, CaptionButtonRoles>(nameof(LeftCaptionButtonRoles)
                    , s => s.LeftCaptionButtonRoles
                    , (s, v) => s.LeftCaptionButtonRoles = v
                );
        CaptionButtonRoles _leftCaptionButtonRoles = new();
        internal CaptionButtonRoles LeftCaptionButtonRoles
        {
            get => _leftCaptionButtonRoles;
            set => SetAndRaise(LeftCaptionButtonRolesProperty, ref _leftCaptionButtonRoles, value);
        }


        internal static readonly DirectProperty<CaptionButtonsBroker, CaptionButtonRoles> RightCaptionButtonRolesProperty =
                AvaloniaProperty.RegisterDirect<CaptionButtonsBroker, CaptionButtonRoles>(nameof(RightCaptionButtonRoles)
                    , s => s.RightCaptionButtonRoles
                    , (s, v) => s.RightCaptionButtonRoles = v
                );
        CaptionButtonRoles _rightCaptionButtonRoles = new();
        internal CaptionButtonRoles RightCaptionButtonRoles
        {
            get => _rightCaptionButtonRoles;
            set => SetAndRaise(RightCaptionButtonRolesProperty, ref _rightCaptionButtonRoles, value);
        }
#nullable restore


        public static readonly DirectProperty<CaptionButtonsBroker, CaptionButtonCluster> LeftClusterProperty =
                AvaloniaProperty.RegisterDirect<CaptionButtonsBroker, CaptionButtonCluster>(nameof(LeftCluster)
                    , s => s.LeftCluster
                    , (s, v) => s.LeftCluster = v
                );
        CaptionButtonCluster _leftCluster = null;
        public CaptionButtonCluster LeftCluster
        {
            get => _leftCluster;
            set => SetAndRaise(LeftClusterProperty, ref _leftCluster, value);
        }


        public static readonly DirectProperty<CaptionButtonsBroker, CaptionButtonCluster> RightClusterProperty =
                AvaloniaProperty.RegisterDirect<CaptionButtonsBroker, CaptionButtonCluster>(nameof(RightCluster)
                    , s => s.RightCluster
                    , (s, v) => s.RightCluster = v
                );
        CaptionButtonCluster _rightCluster = null;
        public CaptionButtonCluster RightCluster
        {
            get => _rightCluster;
            set => SetAndRaise(RightClusterProperty, ref _rightCluster, value);
        }



        static CaptionButtonsBroker()
        {
            static void ClusterProperty_Changed(CaptionButtonsBroker broker, AvaloniaPropertyChangedEventArgs args, CaptionButtonClusterPosition position)
            {
                if (args.TryGetOldAndNewValue(out CaptionButtonCluster oldCluster, out CaptionButtonCluster newCluster))
                    broker.OnCaptionButtonClusterChanged(oldCluster, newCluster, position);
            }

            LeftClusterProperty.Changed.AddClassHandler<CaptionButtonsBroker>((b, s) => ClusterProperty_Changed(b, s, CaptionButtonClusterPosition.Left));
            RightClusterProperty.Changed.AddClassHandler<CaptionButtonsBroker>((b, s) => ClusterProperty_Changed(b, s, CaptionButtonClusterPosition.Right));
        }




        readonly IReadOnlyDictionary<CaptionButtonRole, Action> _roleActions;
        public CaptionButtonsBroker()
        {
            _roleActions = new Dictionary<CaptionButtonRole, Action>()
            {
                [CaptionButtonRole.Close] = OnClose,
                [CaptionButtonRole.Maximize] = OnRestore,
                [CaptionButtonRole.Minimize] = OnMinimize,
                [CaptionButtonRole.FullScreen] = OnToggleFullScreen,
            };
        }


        void OnCaptionButtonClusterChanged(CaptionButtonCluster oldCluster, CaptionButtonCluster newCluster, CaptionButtonClusterPosition position)
        {
            if (!TryGetHostWindow(out Window hostWindow))
                return;

            oldCluster?.Detach();
            newCluster?.Attach(this, hostWindow, position);
        }


        static bool HasTrailingMenu(CaptionButtonRoles roles, out CaptionButtonRoles modified)
        {
            int count = roles.Count;
            if (roles.Count <= 0)
                goto preserve;

            if (roles.Last() != CaptionButtonRole.WindowMenu)
                goto preserve;

            var stripped = roles.ToList();
            stripped.RemoveAt(count - 1);
            modified = new(stripped);
            return true;

            preserve:
            modified = default;
            return false;
        }







        CompositeDisposable _mainDisposable = null;
        public override void Attach(Window hostWindow)
        {
            base.Attach(hostWindow);
            _hostTitleBar = TemplatedParent is TitleBar2 hostTitleBar
                ? hostTitleBar
                : null
            ;
            var leftCluster = LeftCluster;
            bool hasLeftCluster = leftCluster != null;

            var rightCluster = RightCluster;
            bool hasRightCluster = rightCluster != null;

            if (hasLeftCluster)
                leftCluster.Attach(this, hostWindow, CaptionButtonClusterPosition.Left);

            if (hasRightCluster)
                rightCluster.Attach(this, hostWindow, CaptionButtonClusterPosition.Right);
            
            _mainDisposable = new()
            {
                Bind(LeftCaptionButtonRolesProperty, hostWindow[!ManagedWindowChrome.LeftCaptionButtonRolesProperty]),
                Bind(RightCaptionButtonRolesProperty, hostWindow[!ManagedWindowChrome.RightCaptionButtonRolesProperty]),
                this
                    .GetObservable(IsVisibleProperty)
                    .Subscribe(isVisible =>
                    {
                        LeftCluster?.RefreshSize();
                        RightCluster?.RefreshSize();
                    })
                ,
            };

            if (hasLeftCluster)
            {
                _mainDisposable.Add(leftCluster
                    .GetObservable(CaptionButtonCluster.HasWindowMenuAtMergeIndexProperty)
                    .Subscribe(hasWindowMenuAtMergeIndex => _hostTitleBar?.UpdateLeftClusterProperties(HostWindow, hasWindowMenuAtMergeIndex, leftCluster.HasNonWindowMenuButtons, leftCluster.ItemCount))
                );
                _mainDisposable.Add(leftCluster
                    .GetObservable(CaptionButtonCluster.HasNonWindowMenuButtonsProperty)
                    .Subscribe(hasNonWindowMenuButtonsProperty => _hostTitleBar?.UpdateLeftClusterProperties(HostWindow, leftCluster.HasWindowMenuAtMergeIndex, hasNonWindowMenuButtonsProperty, leftCluster.ItemCount))
                );
                _mainDisposable.Add(leftCluster
                    .GetObservable(ItemsControl.ItemCountProperty)
                    .Subscribe(count => _hostTitleBar?.UpdateLeftClusterProperties(HostWindow, leftCluster.HasWindowMenuAtMergeIndex, leftCluster.HasNonWindowMenuButtons, count))
                );
            }

            if (hasRightCluster)
            {
                _mainDisposable.Add(rightCluster
                    .GetObservable(CaptionButtonCluster.HasWindowMenuAtMergeIndexProperty)
                    .Subscribe(hasWindowMenuAtMergeIndex => _hostTitleBar?.UpdateRightClusterProperties(HostWindow, hasWindowMenuAtMergeIndex, rightCluster.HasNonWindowMenuButtons, rightCluster.ItemCount))
                );
                _mainDisposable.Add(rightCluster
                    .GetObservable(CaptionButtonCluster.HasNonWindowMenuButtonsProperty)
                    .Subscribe(hasNonWindowMenuButtonsProperty => _hostTitleBar?.UpdateRightClusterProperties(HostWindow, rightCluster.HasWindowMenuAtMergeIndex, hasNonWindowMenuButtonsProperty, rightCluster.ItemCount))
                );
                _mainDisposable.Add(rightCluster
                    .GetObservable(ItemsControl.ItemCountProperty)
                    .Subscribe(count => _hostTitleBar?.UpdateRightClusterProperties(HostWindow, rightCluster.HasWindowMenuAtMergeIndex, rightCluster.HasNonWindowMenuButtons, count))
                );
            }
        }


        public override void Detach()
        {
            base.Detach();
            bool hasHostWindow = TryGetHostWindow(out Window oldHost);
            _mainDisposable?.Dispose();

            if (hasHostWindow)
            {
                ManagedWindowChrome.SetLeftCaptionButtonsWidth(oldHost, 0d);
                ManagedWindowChrome.SetRightCaptionButtonsWidth(oldHost, 0d);
            }
        }




        internal void ExecuteCaptionButton(CaptionButton button, CaptionButtonClickEventArgs e)
        {
            if (!TryGetHostWindow(out Window hostWindow))
                return;

            if (!_roleActions.TryGetValue(button.Role, out Action roleAction))
                roleAction = null;

            if (e.PointerArgs.RoutedEvent == Button.ClickEvent)
                roleAction?.Invoke();
            else
                ManagedWindowChrome.PLATFORM_IMPL.ExecuteButton(hostWindow, e, roleAction);
        }


        sealed class CaptionButtonsRemoveTrailingMenuConverter
            : IValueConverter
        {
            public static readonly CaptionButtonsRemoveTrailingMenuConverter Instance = new();
            private CaptionButtonsRemoveTrailingMenuConverter()
            {}
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                if (value is not CaptionButtonRoles roles)
                    return BindingOperations.DoNothing;

                if (HasTrailingMenu(roles, out CaptionButtonRoles modified))
                    return modified;

                return roles;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                => throw new NotSupportedException();
        }
    }
}