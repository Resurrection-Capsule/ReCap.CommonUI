using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using ReCap.CommonUI.Util;
using ReCap.CommonUI.Util.OperatingSystem;
using ReCap.CommonUI.Util.OperatingSystem.Win32;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public class WindowTransparencyHack
        : AvaloniaObject
    {
        public static readonly AttachedProperty<bool> EnableHackHintProperty =
            AvaloniaProperty.RegisterAttached<WindowTransparencyHack, Window, bool>("EnableHackHint", false);
        internal static bool GetEnableHackHint(Window control)
            => control.GetValue(EnableHackHintProperty);
        internal static void SetEnableHackHint(Window control, bool value)
            => control.SetValue(EnableHackHintProperty, value);




        public static readonly AttachedProperty<bool> ActualIsTransparentProperty =
            AvaloniaProperty.RegisterAttached<WindowTransparencyHack, Window, bool>("ActualIsTransparent", false);
        public static bool GetActualIsTransparent(Window control)
            => control.GetValue(ActualIsTransparentProperty);
        internal static void SetActualIsTransparent(Window control, bool value)
            => control.SetValue(ActualIsTransparentProperty, value);




        static WindowTransparencyHack()
        {
#if WINDOWS_USE_SETWINDOWCOMPOSITIONATTRIBUTE
            EnableHackHintProperty.Changed.AddClassHandler<Window>(EnableHackHintProperty_Changed);
            ActualIsTransparentProperty.Changed.AddClassHandler<Window>(ActualIsTransparentProperty_Changed);
#endif
        }


#if WINDOWS_USE_SETWINDOWCOMPOSITIONATTRIBUTE
        static readonly Version _WIN8_0 = new(6, 2, 9200);
        static readonly Version _WIN8_1 = new(6, 3, 9600);
        static readonly bool _ACTUALLY_USE_WIN8_TRANSPARENCY_HACK = Win8_ActuallyUseTransparencyHack();
        static bool Win8_ActuallyUseTransparencyHack()
        {
            if (!OSInfo.IsWindows)
            {
                // Not even running on Windows
                return false;
            }

            Version osVersion = OSInfo.Version;
            if (osVersion.Major != 6)
            {
                // Windows 10 and 11 are both NT 10.0
                // Windows Server 2003 and older are NT 5.x or lower
                return false;
            }

            Version win8xCompare = OSInfo.IsVersionDefinitelyAccurate
                ? _WIN8_0
                : _WIN8_1
            ;
            return osVersion >= win8xCompare;
        }
        static void EnableHackHintProperty_Changed(Window sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (!_ACTUALLY_USE_WIN8_TRANSPARENCY_HACK)
                return;

            bool newValue = e.GetNewValue<bool>();
            if (GetActualIsTransparent(sender) != newValue)
                SetActualIsTransparent(sender, newValue);
        }


        static void ActualIsTransparentProperty_Changed(Window sender, AvaloniaPropertyChangedEventArgs e)
        {
            var newValue = e.GetNewValue<bool>();
            OnActualIsTransparentPropertyChanged(sender, newValue);
        }


        static void DwmDontExtendFrameIntoClientArea(IntPtr hWnd)
        {

            var margins = new MARGINS()
            {
                cxLeftWidth = 0,
                cyTopHeight = 0,
                cxRightWidth = 0,
                cyBottomHeight = 0,
            };
            var ret = DwmApi.DwmExtendFrameIntoClientArea(hWnd, ref margins);
            Debug.WriteLine($"{nameof(DwmApi.DwmExtendFrameIntoClientArea)}: {ret}");
        }
        static void OnActualIsTransparentPropertyChanged(Window sender, bool newValue)
        {
            if (!sender.TryGetHWnd(out IntPtr hWnd))
                return;

            if (SetWindowCompositionAttributeForTransparency(hWnd, newValue))
            {
                Debug.WriteLine("SafeSetWindowCompositionAttribute");
                Dispatcher.UIThread.Post(() => DwmDontExtendFrameIntoClientArea(hWnd));
            }
        }

        static bool SetWindowCompositionAttributeForTransparency(IntPtr hWnd, bool transparent)
        {
            var accent = new AccentPolicy();
            var accentStructSize = Marshal.SizeOf(accent);

            accent.AccentState = transparent
                ? AccentState.ACCENT_ENABLE_BLURBEHIND_BUT_ITS_PER_PIXEL_ALPHA_ON_WINDOWS_8
                : AccentState.ACCENT_DISABLED
            ;

            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);

            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };
            return SafeSetWindowCompositionAttribute(hWnd, ref data);
        }
        static bool SafeSetWindowCompositionAttribute(IntPtr hWnd, ref WindowCompositionAttributeData data)
            => User32.SetWindowCompositionAttribute(hWnd, ref data) > 0;
#endif
    }
}