using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed partial class Win32WindowChromeFactoryImpl
        : WindowChromeFactoryImplBase
    {
        public override bool CanUseManagedWindowChrome
        {
            get => true;
        }


        public override bool PrefersManagedWindowChrome
        {
            get => true;
        }


        public override bool DefaultShowCaptionIcon
        {
            get => true;
        }


        public override bool DefaultShowCaptionText
        {
            get => true;
        }


        public Win32WindowChromeFactoryImpl()
            : base()
        {
            Win32Properties.NonClientHitTestResultProperty.Changed.AddClassHandler<Visual>(NonClientHitTestVisual_ResultChanged);
        }
        static void NonClientHitTestVisual_ResultChanged(Visual visual, AvaloniaPropertyChangedEventArgs e)
        {
            if (!visual.IsAttachedToVisualTree())
                return;
            /*
            else if (visual.GetVisualRoot() is not Window window)
                return;
            else
                window
            */
            else if (TopLevel.GetTopLevel(visual).TryGetChrome(out WindowChrome chrome))
                chrome
                    .GetPlatformImpl<Win32WindowChromeImpl>()
                    .OnNonClientHitTestVisualResultChanged(visual, e)
                ;
        }




        protected override IEnumerable<CaptionButtonRole> InitValidButtonRoles()
            => new[]
            {
                CaptionButtonRole.WindowMenu,
                CaptionButtonRole.Minimize,
                CaptionButtonRole.Maximize,
                CaptionButtonRole.FullScreen,
                CaptionButtonRole.Close,
                CaptionButtonRole.KeepAbove,
            };


        public override IWindowChromeImpl CreateImpl(WindowChrome chrome)
            => new Win32WindowChromeImpl(chrome);
    }
}