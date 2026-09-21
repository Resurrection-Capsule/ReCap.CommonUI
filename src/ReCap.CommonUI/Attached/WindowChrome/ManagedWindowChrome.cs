using System.Collections.Generic;
using Avalonia.Layout;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public partial class ManagedWindowChrome
        : Layoutable
    {
#if TEST_CAPTIONBUTTONROLES
        static readonly CaptionButtonRolesPair _TEST_ROLES = new()
        {
            Left = new()
            {
                CaptionButtonRole.FullScreen,
                CaptionButtonRole.WindowMenu,
            },
            Right = new()
            {
                CaptionButtonRole.WindowMenu,
                CaptionButtonRole.Minimize,
                CaptionButtonRole.Maximize,
                CaptionButtonRole.Close,
            },
        };
#endif


        
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
#if TEST_CAPTIONBUTTONROLES
            get => _TEST_ROLES;
#else
            get => PLATFORM_IMPL.DefaultCaptionButtonRoles;
#endif
        }


        public static IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get => PLATFORM_IMPL.ValidCaptionButtonRoles;
        }
    }
}