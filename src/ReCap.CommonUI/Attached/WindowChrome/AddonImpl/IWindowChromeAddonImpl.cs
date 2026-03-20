using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal interface IWindowChromeAddonImpl
    {
        bool CanUseManagedWindowChrome
        {
            get;
        }

        bool PrefersManagedWindowChrome
        {
            get;
        }

        bool DefaultIconInTitleBar
        {
            get;
        }

        CaptionButtonRolesPair DefaultCaptionButtons
        {
            get;
        }

        IEnumerable<CaptionButtonRole> ValidCaptionButtonRoles
        {
            get;
        }


        void Init();


        bool GetDesiredManagedChrome(Window window, ManagedChromeMode chromeMode);
        void ApplyDesiredManagedChrome(Window window, bool desiredManagedChrome, ref bool useManagedChrome);
        void ExecuteExtendedCaptionButton(Window window, CaptionButtonClickEventArgs e);
    }
}