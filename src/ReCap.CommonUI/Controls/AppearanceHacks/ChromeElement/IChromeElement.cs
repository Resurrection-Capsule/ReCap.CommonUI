using Avalonia.Controls;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    internal interface IChromeElement
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


        IPseudoClasses PseudoClasses
        {
            get;
        }
        /*
        void Update(bool isHostWindowActive, WindowState hostWindowState
            , bool leftHasNonWindowMenuButtons, bool leftHasWindowMenuAtMergeIndex, int leftButtonCount, WindowMenuPresence leftPresence
            , bool rightHasNonWindowMenuButtons, bool rightHasWindowMenuAtMergeIndex, int rightButtonCount, WindowMenuPresence rightPresence
        );
    }




    internal static class ChromeElementExtensions
    {
        public static void ApplyWindowState(this IPseudoClasses pseudoClasses, WindowState hostWindowState)
        {
            ExpandWindowState(hostWindowState
                , out bool isHostWindowMinimized
                , out bool isHostWindowNormal
                , out bool isHostWindowMaximized
                , out bool isHostWindowFullScreen
            );
            pseudoClasses.Set(WindowChrome.PSEUD_MINIMIZED, isHostWindowMinimized);
            pseudoClasses.Set(WindowChrome.PSEUD_NORMAL, isHostWindowNormal);
            pseudoClasses.Set(WindowChrome.PSEUD_MAXIMIZED, isHostWindowMaximized);
            pseudoClasses.Set(WindowChrome.PSEUD_FULLSCREEN, isHostWindowFullScreen);
        }


        static void ExpandWindowState(
            WindowState hostWindowState
            , out bool isHostWindowMinimized
            , out bool isHostWindowNormal
            , out bool isHostWindowMaximized
            , out bool isHostWindowFullScreen
        )
        {
            isHostWindowMinimized = hostWindowState == WindowState.Minimized;
            isHostWindowNormal = hostWindowState == WindowState.Normal;
            isHostWindowMaximized = hostWindowState == WindowState.Maximized;
            isHostWindowFullScreen = hostWindowState == WindowState.FullScreen;
        }
        */
    }
}