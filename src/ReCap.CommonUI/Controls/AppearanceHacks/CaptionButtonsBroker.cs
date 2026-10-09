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
        , IChromeElement
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
            
            WindowChrome chrome = WindowChrome.GetChrome(hostWindow);
            chrome.AttachElement(this);

            _mainDisposable = new()
            {
                this
                    .GetObservable(IsVisibleProperty)
                    .Subscribe(isVisible =>
                    {
                        LeftCluster?.RefreshSize();
                        RightCluster?.RefreshSize();
                    })
                ,
            };
        }


        public override void Detach()
        {
            base.Detach();
            bool hasHostWindow = TryGetHostWindow(out Window oldHost);
            _mainDisposable?.Dispose();

            if (!hasHostWindow)
                return;
            else if (WindowChrome.TryGetChrome(oldHost, out WindowChrome chrome))
            {
                chrome.LeftCaptionButtonsWidth = 0d;
                chrome.RightCaptionButtonsWidth = 0d;

                chrome.DetachElement(this);
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
                WindowChrome.PLATFORM_IMPL.ExecuteButton(hostWindow, e, roleAction);
        }




#region IChromeElement
        IPseudoClasses IChromeElement.PseudoClasses
        {
            get => PseudoClasses;
        }


        public static readonly DirectProperty<CaptionButtonsBroker, bool> IsHostWindowActiveProperty
            = TitleBar2.IsHostWindowActiveProperty.AddOwner<CaptionButtonsBroker>(s => s.IsHostWindowActive);
        bool _isHostWindowActive = false;
        public bool IsHostWindowActive
        {
            get => _isHostWindowActive;
            private set => SetAndRaise(IsHostWindowActiveProperty, ref _isHostWindowActive, value);
        }
        bool IChromeElement.IsHostWindowActive
        {
            get => IsHostWindowActive;
            set => IsHostWindowActive = value;
        }


        public static readonly DirectProperty<CaptionButtonsBroker, WindowState> HostWindowStateProperty
            = TitleBar2.HostWindowStateProperty.AddOwner<CaptionButtonsBroker>(s => s.HostWindowState);
        WindowState _hostWindowState = WindowState.Normal;
        public WindowState HostWindowState
        {
            get => _hostWindowState;
            private set => SetAndRaise(HostWindowStateProperty, ref _hostWindowState, value);
        }
        WindowState IChromeElement.HostWindowState
        {
            get => HostWindowState;
            set => HostWindowState = value;
        }


        public static readonly DirectProperty<CaptionButtonsBroker, bool> LeftHasWindowMenuAtMergeIndexProperty
            = TitleBar2.LeftHasWindowMenuAtMergeIndexProperty.AddOwner<CaptionButtonsBroker>(o => o.LeftHasWindowMenuAtMergeIndex);
        bool _leftHasWindowMenuAtMergeIndex = TitleBar2.DEFAULT_LeftHasWindowMenuAtMergeIndex;
        public bool LeftHasWindowMenuAtMergeIndex
        {
            get => _leftHasWindowMenuAtMergeIndex;
            private set => SetAndRaise(LeftHasWindowMenuAtMergeIndexProperty, ref _leftHasWindowMenuAtMergeIndex, value);
        }
        bool IChromeElement.LeftHasWindowMenuAtMergeIndex
        {
            get => LeftHasWindowMenuAtMergeIndex;
            set => LeftHasWindowMenuAtMergeIndex = value;
        }


        public static readonly DirectProperty<CaptionButtonsBroker, bool> LeftHasNonWindowMenuButtonsProperty
            = TitleBar2.LeftHasNonWindowMenuButtonsProperty.AddOwner<CaptionButtonsBroker>(o => o.LeftHasNonWindowMenuButtons);
        bool _leftHasNonWindowMenuButtons = TitleBar2.DEFAULT_LeftHasNonWindowMenuButtons;
        public bool LeftHasNonWindowMenuButtons
        {
            get => _leftHasNonWindowMenuButtons;
            private set => SetAndRaise(LeftHasNonWindowMenuButtonsProperty, ref _leftHasNonWindowMenuButtons, value);
        }
        bool IChromeElement.LeftHasNonWindowMenuButtons
        {
            get => LeftHasNonWindowMenuButtons;
            set => LeftHasNonWindowMenuButtons = value;
        }


        public static readonly DirectProperty<CaptionButtonsBroker, int> LeftButtonCountProperty
            = TitleBar2.LeftButtonCountProperty.AddOwner<CaptionButtonsBroker>(o => o.LeftButtonCount);
        int _leftButtonCount = TitleBar2.DEFAULT_LeftButtonCount;
        public int LeftButtonCount
        {
            get => _leftButtonCount;
            private set => SetAndRaise(LeftButtonCountProperty, ref _leftButtonCount, value);
        }
        int IChromeElement.LeftButtonCount
        {
            get => LeftButtonCount;
            set => LeftButtonCount = value;
        }


        public static readonly DirectProperty<CaptionButtonsBroker, WindowMenuPresence> LeftWindowMenuPresenceProperty
            = TitleBar2.LeftWindowMenuPresenceProperty.AddOwner<CaptionButtonsBroker>(
                getter: o => o.LeftWindowMenuPresence
                , unsetValue: TitleBar2.DEFAULT_LeftWindowMenuPresence
            );
        WindowMenuPresence _leftWindowMenuPresence = TitleBar2.DEFAULT_LeftWindowMenuPresence;
        public WindowMenuPresence LeftWindowMenuPresence
        {
            get => _leftWindowMenuPresence;
            private set => SetAndRaise(LeftWindowMenuPresenceProperty, ref _leftWindowMenuPresence, value);
        }
        WindowMenuPresence IChromeElement.LeftWindowMenuPresence
        {
            get => LeftWindowMenuPresence;
            set => LeftWindowMenuPresence = value;
        }




        public static readonly DirectProperty<CaptionButtonsBroker, bool> RightHasWindowMenuAtMergeIndexProperty
            = TitleBar2.RightHasWindowMenuAtMergeIndexProperty.AddOwner<CaptionButtonsBroker>(o => o.RightHasWindowMenuAtMergeIndex);
        bool _rightHasWindowMenuAtMergeIndex = TitleBar2.DEFAULT_RightHasWindowMenuAtMergeIndex;
        public bool RightHasWindowMenuAtMergeIndex
        {
            get => _rightHasWindowMenuAtMergeIndex;
            private set => SetAndRaise(RightHasWindowMenuAtMergeIndexProperty, ref _rightHasWindowMenuAtMergeIndex, value);
        }
        bool IChromeElement.RightHasWindowMenuAtMergeIndex
        {
            get => RightHasWindowMenuAtMergeIndex;
            set => RightHasWindowMenuAtMergeIndex = value;
        }


        public static readonly DirectProperty<CaptionButtonsBroker, bool> RightHasNonWindowMenuButtonsProperty
            = TitleBar2.RightHasNonWindowMenuButtonsProperty.AddOwner<CaptionButtonsBroker>(o => o.RightHasNonWindowMenuButtons);
        bool _rightHasNonWindowMenuButtons = TitleBar2.DEFAULT_RightHasNonWindowMenuButtons;
        public bool RightHasNonWindowMenuButtons
        {
            get => _rightHasNonWindowMenuButtons;
            private set => SetAndRaise(RightHasNonWindowMenuButtonsProperty, ref _rightHasNonWindowMenuButtons, value);
        }
        bool IChromeElement.RightHasNonWindowMenuButtons
        {
            get => RightHasNonWindowMenuButtons;
            set => RightHasNonWindowMenuButtons = value;
        }


        public static readonly DirectProperty<CaptionButtonsBroker, int> RightButtonCountProperty
            = TitleBar2.RightButtonCountProperty.AddOwner<CaptionButtonsBroker>(o => o.RightButtonCount);
        int _rightButtonCount = TitleBar2.DEFAULT_RightButtonCount;
        public int RightButtonCount
        {
            get => _rightButtonCount;
            private set => SetAndRaise(RightButtonCountProperty, ref _rightButtonCount, value);
        }
        int IChromeElement.RightButtonCount
        {
            get => RightButtonCount;
            set => RightButtonCount = value;
        }


        public static readonly DirectProperty<CaptionButtonsBroker, WindowMenuPresence> RightWindowMenuPresenceProperty
            = TitleBar2.RightWindowMenuPresenceProperty.AddOwner<CaptionButtonsBroker>(
                getter: o => o.RightWindowMenuPresence
                , unsetValue: TitleBar2.DEFAULT_RightWindowMenuPresence
            );
        WindowMenuPresence _rightWindowMenuPresence = TitleBar2.DEFAULT_RightWindowMenuPresence;
        public WindowMenuPresence RightWindowMenuPresence
        {
            get => _rightWindowMenuPresence;
            private set => SetAndRaise(RightWindowMenuPresenceProperty, ref _rightWindowMenuPresence, value);
        }
        WindowMenuPresence IChromeElement.RightWindowMenuPresence
        {
            get => RightWindowMenuPresence;
            set => RightWindowMenuPresence = value;
        }
#endregion
    }
}