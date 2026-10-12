using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Chrome;
using ReCap.CommonUI.Attached.WindowChrome;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    [ChromeElementPseudoClassesPlaceholder]
    public sealed partial class CaptionButtonsBroker
        : CaptionButtons
        , IPseudoClassesProvider
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
            
            WindowChrome
                .GetChrome(hostWindow)
                .AttachElement(this)
            ;
        }


        public override void Detach()
        {
            base.Detach();
            if (TryGetHostWindow(out Window hostWindow) && WindowChrome.TryGetChrome(hostWindow, out WindowChrome chrome))
                chrome.DetachElement(this);
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




#region IPseudoClassesProvider
        IPseudoClasses IPseudoClassesProvider.GetPseudoClasses()
            => PseudoClasses;
#endregion
    }
}