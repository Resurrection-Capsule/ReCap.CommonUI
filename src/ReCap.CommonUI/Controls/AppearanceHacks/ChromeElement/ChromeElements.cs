#if NO
using System;
using System.Collections.Generic;
using Avalonia.Controls;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    internal sealed class ChromeElements
    {
        TitleBar2 _titleBar = null;
        public TitleBar2 TitleBar
        {
            get => _titleBar;
            private set => _titleBar = value;
        }


        /*
        TitleBar2 _titleBar = null;
        public TitleBar2 TitleBar
        {
            get => _titleBar;
            private set => _titleBar = value;
        }
        */


        //Window _hostWindow = null;
        public Window HostWindow
        {
            get => ((IChromeRootElement)TitleBar).HostWindow;
            /*
            get => _hostWindow;
            private set => _hostWindow = value;
            */
        }




        List<IChromeElement> _elements = new();
        public IEnumerable<IChromeElement> Elements
        {
            get => _elements;
        }


        public void AttachTitleBar(TitleBar2 titleBar)
        {
            if (titleBar == null)
                throw new ArgumentNullException(nameof(titleBar));
            else if (TitleBar != null)
                DetachTitleBar();

            TitleBar = titleBar;
        }


        public void DetachTitleBar()
        {
            TitleBar = null;
        }
    }
}
#endif