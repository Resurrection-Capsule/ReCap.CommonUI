#if WAYLAND
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using ReCap.CommonUI.Util.OperatingSystem;
using ReCap.CommonUI.Util.OperatingSystem.Linux;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed partial class WaylandWindowChromeAddonSubImpl
        : WindowChromeAddonImplBase
    {
#region Properties
        public override bool CanUseManagedWindowChrome
        {
            get
            {
                //[TODO: implement]
                throw new NotImplementedException("HELP WANTED");
            }
        }


        public override bool PrefersManagedWindowChrome
        {
            get
            {
                //[TODO: implement]
                throw new NotImplementedException("HELP WANTED");
            }
        }
#endregion




        LinuxDetails _DETAILS;
        public WaylandWindowChromeAddonSubImpl(LinuxDetails details)
            : base()
        {
            _DETAILS = details;
        }


        public override void Init()
        {
            base.Init();
        }




        protected override CaptionButtonRolesPair CreateDefaultCaptionButtons()
            => throw new NotSupportedException($"Use {nameof(LinuxWindowChromeAddonImpl)}.{nameof(LinuxWindowChromeAddonImpl.DefaultCaptionButtons)} instead");



        public override void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, Action<bool> applyUseManagedChrome)
        {
            WriteLine($"{nameof(window)}: {window}, {nameof(desiredManagedChrome)}: {desiredManagedChrome}, {nameof(applyUseManagedChrome)}: {applyUseManagedChrome}");
            //[TODO: implement]
            throw new NotImplementedException("HELP WANTED");
        }


        protected override IEnumerable<CaptionButtonRole> GetValidCaptionButtonRoles()
        {
            //[TODO: implement]
            throw new NotImplementedException("HELP WANTED");
        }




        static void WriteLine(object value, [CallerMemberName] string memberName = null)
        {
            string text;


            if (value == null)
                text = "null";
            else if (value is string valueString)
                text = valueString;
            else
                text = value.ToString();


            if (!string.IsNullOrWhiteSpace(memberName))
                text = $"{memberName}: {text}";


            text = $"[{nameof(WaylandWindowChromeAddonSubImpl)}] {text}";
            //System.Diagnostics.Debug.WriteLine(text);
            Console.WriteLine(text);
        }
    }
}
#endif