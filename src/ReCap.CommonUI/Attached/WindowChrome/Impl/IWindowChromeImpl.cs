using System;
using ReCap.CommonUI.Controls.AppearanceHacks;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal interface IWindowChromeImpl
        : IDisposable
    {
        WindowChrome Chrome
        {
            get;
        }


        bool GetDesiredManagedChrome(ManagedChromeHint chromeMode);
        void ApplyDesiredManagedChrome(bool desiredManagedChrome, Action<bool> applyUseManagedChrome);
        bool ExecuteButton(CaptionButtonClickEventArgs e, Action roleAction);
        void Prepare(CaptionButton button);
    }
}