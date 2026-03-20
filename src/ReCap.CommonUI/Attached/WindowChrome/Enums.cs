using System;
using Avalonia.Controls;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    public enum CaptionButtonClickState
    {
        Pressed,
        Released,
    }


    public enum CaptionButtonsOrder
    {
        MinMaxClose,
        MaxMinClose,
    }


    public enum CaptionButtonRole
    {
        Minimize = WindowState.Minimized,
        Maximize = WindowState.Maximized,
        FullScreen = WindowState.FullScreen,
        Close = 10,
        Menu = 11,
        /*
        ApplicationMenu = 12,
        ShowOnAllDesktops = 13,
        ContextHelp = 14,
        Shade = 15,
        KeepBelow = 16,
        */
        KeepAbove = 17,
    }


    public enum ManagedChromeMode
    {
        Never = 0,
        Auto,
        WheneverPossible,
    }
}