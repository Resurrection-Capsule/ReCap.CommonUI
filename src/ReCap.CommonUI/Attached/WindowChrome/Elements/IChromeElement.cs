using Avalonia.Controls;
using ReCap.CommonUI.Attached.WindowChrome;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    internal interface IChromeElement
        : IPseudoClassesProvider
    {
        bool IsHostWindowActive
        {
            get;
            set;
        }
        WindowState HostWindowState
        {
            get;
            set;
        }


        bool LeftHasWindowMenuAtMergeIndex
        {
            get;
            set;
        }
        bool LeftHasNonWindowMenuButtons
        {
            get;
            set;
        }
        int LeftButtonCount
        {
            get;
            set;
        }
        WindowMenuPresence LeftWindowMenuPresence
        {
            get;
            set;
        }


        bool RightHasWindowMenuAtMergeIndex
        {
            get;
            set;
        }
        bool RightHasNonWindowMenuButtons
        {
            get;
            set;
        }
        int RightButtonCount
        {
            get;
            set;
        }
        WindowMenuPresence RightWindowMenuPresence
        {
            get;
            set;
        }
    }
}