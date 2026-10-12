using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Templates;
using Avalonia.Styling;
using ReCap.CommonUI.Attached.WindowChrome;
using ReCap.CommonUI.Util;

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
    [ChromeElementPseudoClassesPlaceholder]
    public sealed partial class CaptionButtonCluster
        : ItemsControl
        , IPseudoClassesProvider
    {
        const string _PSEUD_BUTTONS = ":buttons";
        const string _PSEUD_LEFT = ":left";
        const string _PSEUD_RIGHT = ":right";




        internal const CaptionButtonClusterPosition DEFAULT_Position = CaptionButtonClusterPosition.Unknown;
        public static readonly DirectProperty<CaptionButtonCluster, CaptionButtonClusterPosition> PositionProperty
            = AvaloniaProperty.RegisterDirect<CaptionButtonCluster, CaptionButtonClusterPosition>(nameof(Position)
                , getter: o => o.Position
                , unsetValue: DEFAULT_Position
            );
        CaptionButtonClusterPosition _position = DEFAULT_Position;
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
            var itemsPanelBaseMD = ItemsPanelProperty.GetMetadata<ItemsControl>();
            var itemsPanelNewMD = new StyledPropertyMetadata<ITemplate<Panel>>(null
                , itemsPanelBaseMD.DefaultBindingMode
                , (o, t) => t is CaptionButtonClusterPanelTemplate
                    ? t
                    : null
                , itemsPanelBaseMD.EnableDataValidation.Value
            );
            ItemsPanelProperty.OverrideMetadata<CaptionButtonCluster>(itemsPanelNewMD);
            ItemsPanelProperty.Changed.AddClassHandler<CaptionButtonCluster>((s, e) =>
            {
                if (!e.TryGetOldAndNewValue(out ITemplate<Panel> oldTemplate, out ITemplate<Panel> newTemplate))
                    return;
                else if (oldTemplate == newTemplate)
                    return;
                else if (newTemplate is CaptionButtonClusterPanelTemplate)
                    return;
                else if (oldTemplate is CaptionButtonClusterPanelTemplate oldCaptionButtonClusterPanelTemplate)
                    s.ItemsPanel = oldCaptionButtonClusterPanelTemplate;
            });
        }




        public CaptionButtonCluster()
            : base()
        {
            ItemsPanel = new CaptionButtonClusterPanelTemplate(this);
        }


        CaptionButtonsBroker _hostBroker = null;
        Window _hostWindow = null;
        internal void Attach(CaptionButtonsBroker broker, Window hostWindow, CaptionButtonClusterPosition position)
        {
            if (hostWindow == null)
                throw new ArgumentNullException(nameof(hostWindow));
            else if (_hostWindow != null)
                Detach();

            WindowChrome
                .GetChrome(hostWindow)
                .AttachElement(this)
            ;


            Position = position;
            _hostBroker = broker;
            _hostWindow = hostWindow;
        }




        internal void ExecuteCaptionButton(CaptionButton button, CaptionButtonClickEventArgs e)
            => _hostBroker?.ExecuteCaptionButton(button, e);


        internal void Detach()
        {
            WindowChrome
                .GetChrome(_hostWindow)
                .DetachElement(this)
            ;

            _hostBroker = null;

            if (_hostWindow == null)
                return;
            _hostWindow = null;
            Position = CaptionButtonClusterPosition.Unknown;
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




#region IPseudoClassesProvider
        IPseudoClasses IPseudoClassesProvider.GetPseudoClasses()
            => PseudoClasses;
#endregion
    }
}