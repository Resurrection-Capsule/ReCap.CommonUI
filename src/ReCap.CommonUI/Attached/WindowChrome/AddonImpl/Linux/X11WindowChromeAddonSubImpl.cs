using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Threading;
using ReCap.CommonUI.Util.OperatingSystem;
using ReCap.CommonUI.Util.OperatingSystem.Linux;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed class X11WindowChromeAddonSubImpl
        : WindowChromeAddonImplBase
    {
        readonly LinuxDetails _DETAILS;
        public X11WindowChromeAddonSubImpl(LinuxDetails details)
            : base()
        {
            _DETAILS = details;
        }




        public override bool CanUseManagedWindowChrome
        {
            get => true;
        }


        public override bool PrefersManagedWindowChrome
        {
            get => _DETAILS.GTKClientSideDecorations;
        }



        protected override IEnumerable<CaptionButtonRole> GetValidCaptionButtonRoles()
            => Enum.GetValues(typeof(CaptionButtonRole))
                .Cast<CaptionButtonRole>()
                .ToArray()
            ;


        public override void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, Action<bool> applyUseManagedChrome)
        {
            if (_DETAILS.CurrentDisplayServer == KnownDisplayServer.Wayland)
            {
                ApplyDesiredManagedChrome_Wayland(
                    window, desiredManagedChrome
                    , fallbackToSystemDecorationsProperty: true
                    , applyUseManagedChrome
                );
            }
            else
            {
                DefaultWindowChromeAddonImpl.ApplyDesiredManagedChrome_Default(
                    this, window, desiredManagedChrome
                    , fallbackToSystemDecorationsProperty: true
                    , applyUseManagedChrome
                );
            }
        }


        void ApplyDesiredManagedChrome_Wayland(
            Window window, bool desiredManagedChrome
            , bool fallbackToSystemDecorationsProperty
            , Action<bool> applyUseManagedChrome
        )
        {
            bool oldIsExtendedIntoWindowDecorations = window.IsExtendedIntoWindowDecorations;


            Dispatcher.UIThread.Invoke(() =>
            {
                if (fallbackToSystemDecorationsProperty)
                {
                    if (desiredManagedChrome && !window.IsExtendedIntoWindowDecorations)
                        window.SystemDecorations = SystemDecorations.None;
                    else if ((!desiredManagedChrome) && !oldIsExtendedIntoWindowDecorations)
                        window.SystemDecorations = SystemDecorations.Full;
                }


                Dispatcher.UIThread.Invoke(() =>
                {
                    applyUseManagedChrome(window.IsExtendedIntoWindowDecorations || desiredManagedChrome);
                });
            });
        }


        protected override CaptionButtonRolesPair CreateDefaultCaptionButtons()
            => throw new NotSupportedException($"Use {nameof(LinuxWindowChromeAddonImpl)}.{nameof(LinuxWindowChromeAddonImpl.DefaultCaptionButtons)} instead");
    }
}