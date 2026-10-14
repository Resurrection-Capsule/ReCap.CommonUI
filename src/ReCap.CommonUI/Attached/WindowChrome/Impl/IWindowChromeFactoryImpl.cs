using System.Collections.Generic;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal interface IWindowChromeFactoryImpl
    {
        bool CanUseManagedWindowChrome
        {
            get;
        }

        bool PrefersManagedWindowChrome
        {
            get;
        }

        bool DefaultShowCaptionIcon
        {
            get;
        }

        bool DefaultShowCaptionText
        {
            get;
        }

        CaptionButtonRolesPair DefaultCaptionButtonRoles
        {
            get;
        }

        IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get;
        }


        IWindowChromeImpl CreateImpl(WindowChrome chrome);
    }
}