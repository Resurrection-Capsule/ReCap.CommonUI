using System;
using System.Collections.Generic;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    partial class WindowChrome
    {
        public static bool PlatformCanUseManagedWindowChrome
        {
            get => PLATFORM_IMPL.CanUseManagedWindowChrome;
        }


        public static bool PlatformPrefersManagedWindowChrome
        {
            get => PlatformCanUseManagedWindowChrome && PLATFORM_IMPL.PrefersManagedWindowChrome;
        }


        public static CaptionButtonRolesPair PlatformDefaultCaptionButtonRoles
        {
            get => PLATFORM_IMPL.DefaultCaptionButtonRoles;
        }


        public static IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get => PLATFORM_IMPL.ValidCaptionButtonRoles;
        }
    }
}