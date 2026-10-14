using System;
using System.Collections.Generic;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    partial class WindowChrome
    {
        public static bool PlatformCanUseManagedWindowChrome
        {
            get => FACTORY.CanUseManagedWindowChrome;
        }


        public static bool PlatformPrefersManagedWindowChrome
        {
            get => PlatformCanUseManagedWindowChrome && FACTORY.PrefersManagedWindowChrome;
        }


        public static CaptionButtonRolesPair PlatformDefaultCaptionButtonRoles
        {
            get => FACTORY.DefaultCaptionButtonRoles;
        }


        public static IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get => FACTORY.ValidCaptionButtonRoles;
        }
    }
}