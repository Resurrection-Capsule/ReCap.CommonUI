using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using ReCap.CommonUI.Util.OperatingSystem;
using ReCap.CommonUI.Util.OperatingSystem.Linux;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed partial class LinuxWindowChromeImpl
        : WindowChromeImplBase
    {
        static readonly LinuxDetails _DETAILS = OSInfo.GetOSDetails<LinuxDetails>();
        static readonly X11Details _X11_DETAILS;
        static readonly X11Atoms _ATOMS;
        static LinuxWindowChromeImpl()
        {
            _X11_DETAILS = _DETAILS.GetDisplayServerDetails<X11Details>();
            _ATOMS = _X11_DETAILS.Atoms;


#if CAPTIONBUTTONROLES_WIP
            _ShowOnAllDesktopsProperty_ATOMS = new[]
            {
                _ATOMS._NET_WM_STATE_STICKY,
            };
            WindowChrome.ShowOnAllDesktopsProperty.Changed.AddClassHandler<Window>(ShowOnAllDesktopsProperty_Changed);


            _ShadedProperty_ATOMS = new[]
            {
                _ATOMS._NET_WM_STATE_SHADED,
            };
            WindowChrome.ShadedProperty.Changed.AddClassHandler<Window>(ShadedProperty_Changed);
#endif

            _BottommostProperty_ATOMS = new[]
            {
                _ATOMS._NET_WM_STATE_BELOW,
            };
            WindowChrome.BottommostProperty.Changed.AddClassHandler<Window>(BottommostProperty_Changed);
        }


#if CAPTIONBUTTONROLES_WIP
        static readonly IEnumerable<IntPtr> _ShowOnAllDesktopsProperty_ATOMS;
        static void ShowOnAllDesktopsProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
            => window
                .GetChromePlatformImpl<LinuxWindowChromeImpl>()
                .OnAffectsAtomsPropertyChanged(e, _ShowOnAllDesktopsProperty_ATOMS)
            ;


        static readonly IEnumerable<IntPtr> _ShadedProperty_ATOMS;
        static void ShadedProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
        {
            var impl = window.GetChromePlatformImpl<LinuxWindowChromeImpl>();
            if (!window.GetIsChromeManaged())
            {
                impl.OnShadedPropertyChanged(e);
                return;
            }

            if (window.SystemDecorations != SystemDecorations.Full)
                window.SystemDecorations = SystemDecorations.Full;

            Dispatcher.UIThread.Invoke(() => impl.OnShadedPropertyChanged(e), DispatcherPriority.SystemIdle);
        }
#endif


        static readonly IEnumerable<IntPtr> _BottommostProperty_ATOMS;
        static void BottommostProperty_Changed(Window window, AvaloniaPropertyChangedEventArgs e)
            => window
                .GetChromePlatformImpl<LinuxWindowChromeImpl>()
                .OnAffectsAtomsPropertyChanged(e, _BottommostProperty_ATOMS)
            ;




        public LinuxWindowChromeImpl(WindowChrome chrome)
            : base(chrome)
        {
#if X11_EVENT_LISTEN
            _X11_DETAILS.EventDispatcher.EventHandlers[Handle] = OnEvent;
#endif
        }


        public override void ApplyDesiredManagedChrome(bool desiredManagedChrome, Action<bool> applyUseManagedChrome)
            => WindowChromeImplHelper.ApplyDesiredManagedChrome(
                this, desiredManagedChrome
                , fallbackToSystemDecorationsProperty: true
                , applyUseManagedChrome
            );




#if CAPTIONBUTTONROLES_WIP
        bool _allowShowOnAllDesktopsChange = true;
        public override bool CoerceShowOnAllDesktops(bool proposedValue)
            => _allowShowOnAllDesktopsChange
                ? proposedValue
                : GetWindowProperty(_ShowOnAllDesktopsProperty_ATOMS)
            ;


        bool _allowShadedChange = true;
        public override bool CoerceShaded(bool proposedValue)
            => _allowShadedChange
                ? proposedValue
                : GetWindowProperty(_ShadedProperty_ATOMS)
            ;
#endif


        bool _allowBottommostChange = true;
        public override bool CoerceBottommost(bool proposedValue)
            => _allowBottommostChange
                ? proposedValue
                : GetWindowProperty(_BottommostProperty_ATOMS)
            ;




#if X11_EVENT_LISTEN
        void OnEvent(ref XEvent xEvent)
        {
            //[TODO: implement]
            System.Diagnostics.Debug.WriteLine(xEvent);
        }
#endif




#if CAPTIONBUTTONROLES_WIP
        protected override bool ExecuteShowOnAllDesktops(CaptionButtonClickEventArgs e)
        {
            if (!TryEnsureEventAndTryGetHostWindow(e, out Window window))
                return false;

            window.SetShowOnAllDesktops(!window.GetShowOnAllDesktops());
            return true;
        }


        protected override bool ExecuteShade(CaptionButtonClickEventArgs e)
        {
            if (!TryEnsureEventAndTryGetHostWindow(e, out Window window))
                return false;

            window.SetShaded(!window.GetShaded());
            return true;
        }


        void OnShadedPropertyChanged(AvaloniaPropertyChangedEventArgs e)
            => OnAffectsAtomsPropertyChanged(e, _ShadedProperty_ATOMS);
#endif


        protected override bool ExecuteKeepBelow(CaptionButtonClickEventArgs e)
        {
            if (!TryEnsureEventAndTryGetHostWindow(e, out Window window))
                return false;

            window.SetBottommost(!window.GetBottommost());
            return true;
        }




        void OnAffectsAtomsPropertyChanged(AvaloniaPropertyChangedEventArgs e, IEnumerable<IntPtr> atoms)
            => X11Helper.ChangeWMAtoms(Handle, e.GetNewValue<bool>(), atoms);




        bool GetWindowProperty(IEnumerable<IntPtr> atoms)
        {
            IntPtr[] array = X11Helper.GetWindowPropertyArray(Handle);
            int arrayLength = array.Length;
            for (int i = 0; i < arrayLength; i++)
            {
                foreach (IntPtr atom in atoms)
                {
                    if (array[i] == atom)
                        return true;
                }
            }
            return false;
        }


#if X11_EVENT_LISTEN
        public override void Dispose()
        {
            base.Dispose();
            _X11_DETAILS.EventDispatcher.EventHandlers.Remove(Handle);
        }
#endif
    }
}