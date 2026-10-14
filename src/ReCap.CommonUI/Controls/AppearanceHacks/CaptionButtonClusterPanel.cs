using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReCap.CommonUI.Attached.WindowChrome;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    internal sealed partial class CaptionButtonClusterPanel
        : Panel
    {
#region Properties
        public static readonly DirectProperty<CaptionButtonClusterPanel, CaptionButtonClusterPosition> PositionProperty
            = AvaloniaProperty.RegisterDirect<CaptionButtonClusterPanel, CaptionButtonClusterPosition>(nameof(Position)
                , getter: o => o.Position
                , setter: (o, v) => o.Position = v
                , unsetValue: CaptionButtonCluster.DEFAULT_Position
            );
        CaptionButtonClusterPosition _position = CaptionButtonCluster.DEFAULT_Position;
        CaptionButtonClusterPosition Position
        {
            get => _position;
            set => SetAndRaise(PositionProperty, ref _position, value);
        }


        const double _DEFAULT_BridgedWidth = 0d;
        static readonly DirectProperty<CaptionButtonClusterPanel, double> BridgedWidthProperty
            = AvaloniaProperty.RegisterDirect<CaptionButtonClusterPanel, double>(nameof(BridgedWidth)
                , getter: o => o.BridgedWidth
                , unsetValue: _DEFAULT_BridgedWidth
            );
        double _bridgedWidth = _DEFAULT_BridgedWidth;
        public double BridgedWidth
        {
            get => _bridgedWidth;
            private set => SetAndRaise(BridgedWidthProperty, ref _bridgedWidth, value);
        }


        static readonly DirectProperty<CaptionButtonClusterPanel, double> CosmeticBridgedWidthProperty
            = AvaloniaProperty.RegisterDirect<CaptionButtonClusterPanel, double>(nameof(CosmeticBridgedWidth)
                , getter: o => o.CosmeticBridgedWidth
                , unsetValue: _DEFAULT_BridgedWidth
            );
        double _cosmeticBridgedWidth = _DEFAULT_BridgedWidth;
        public double CosmeticBridgedWidth
        {
            get => _cosmeticBridgedWidth;
            private set => SetAndRaise(CosmeticBridgedWidthProperty, ref _cosmeticBridgedWidth, value);
        }
#endregion




        static CaptionButtonClusterPanel()
        {
            BridgedWidthProperty.Changed.AddClassHandler<CaptionButtonClusterPanel>(BridgedWidthProperties_Changed);
            CosmeticBridgedWidthProperty.Changed.AddClassHandler<CaptionButtonClusterPanel>(BridgedWidthProperties_Changed);

            var properties = new AvaloniaProperty[]
            {
                PositionProperty,
                BridgedWidthProperty,
                CosmeticBridgedWidthProperty,
            };
            AffectsArrange<CaptionButtonClusterPanel>(properties);
            AffectsMeasure<CaptionButtonClusterPanel>(properties);
            AffectsRender<CaptionButtonClusterPanel>(properties);

            var childProperties = new AvaloniaProperty[]
            {
                CaptionButton.InsetProperty,
                CaptionButton.RoleProperty,
                CaptionButton.CurrentGlyphProperty,
                CaptionButton.IsRoleActivatableProperty,
                CaptionButton.IsRoleActiveProperty,
                CaptionButton.LeftGapProperty,
                CaptionButton.RightGapProperty,
            };
            AffectsParentArrange<CaptionButtonClusterPanel>(childProperties);
            AffectsParentMeasure<CaptionButtonClusterPanel>(childProperties);
        }

        static void BridgedWidthProperties_Changed(CaptionButtonClusterPanel panel, AvaloniaPropertyChangedEventArgs e)
            => panel.PropagateBridgedWidth();




        Window _hostWindow = null;
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            if (e.Root is not Window hostWindow)
                return;

            _hostWindow = hostWindow;
            PropagateBridgedWidth();
        }


        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            _hostWindow = null;
        }


        protected override void ArrangeCore(Rect finalRect)
        {
            base.ArrangeCore(finalRect);
            UpdateBridgedWidth();
        }




        void PropagateBridgedWidth()
        {
            if (!_hostWindow.TryGetChrome(out WindowChrome chrome))
                return;

            switch (Position)
            {
                case CaptionButtonClusterPosition.Left:
                    chrome.LeftCaptionButtonsWidth = BridgedWidth;
                    chrome.CosmeticLeftCaptionButtonsWidth = CosmeticBridgedWidth;
                    break;

                case CaptionButtonClusterPosition.Right:
                    chrome.RightCaptionButtonsWidth = BridgedWidth;
                    chrome.CosmeticRightCaptionButtonsWidth = CosmeticBridgedWidth;
                    break;
            }
        }


        void UpdateBridgedWidth()
        {
            ComputeBridgedWidths(out double content, out double cosmetic);
            BridgedWidth = content;
            CosmeticBridgedWidth = cosmetic;
        }


        void ComputeBridgedWidths(out double content, out double cosmetic)
        {
            if (_hostWindow == null)
            {
                content = -1d;
                cosmetic = -1d;
                return;
            }

            double nearest = double.MaxValue;
            double furthest = double.MinValue;

            bool hasComputed = false;
            var children = Children;
            foreach (var child in children)
            {
                if (!child.IsEffectivelyVisible)
                    continue;
                else if (child.Opacity <= 0d)
                    continue;

                var bounds = child.Bounds;

                nearest = Math.Min(nearest, bounds.Left);
                furthest = Math.Max(furthest, bounds.Right);
                hasComputed = true;
            }


            double computed = hasComputed
                ? furthest - nearest
                : 0d
            ;
            System.Diagnostics.Debug.Assert(computed >= 0d);

            content = computed;
            cosmetic = Math.Max(Bounds.Width, DesiredSize.Width);
        }




        static bool TryGetCaptionButtons(IEnumerable<Control> controls, out List<CaptionButton> buttons, out int buttonCount)
        {
            buttons = new();
            if (controls == null)
                goto fail;

            foreach (var child in controls)
            {
                if (child == null)
                    continue;
                if (!child.IsEffectivelyVisible)
                    continue;
                if (child is CaptionButton button)
                    buttons.Add(button);
            }

            buttonCount = buttons.Count;
            return buttonCount > 0;

            fail:
            buttons = null;
            buttonCount = -1;
            return false;
        }
    }
}