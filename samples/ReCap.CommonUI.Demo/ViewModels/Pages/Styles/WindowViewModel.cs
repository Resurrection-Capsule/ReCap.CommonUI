using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ReCap.CommonUI.Attached.WindowChrome;
#if SORTABLE_AVALONIA
using Sortable.Avalonia;
#endif

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    public partial class WindowViewModel
        : ViewModelBase
    {
        ManagedChromeMode _currentChromeMode = App.Current.ManagedChromeHint;
        public ManagedChromeMode CurrentChromeMode
        {
            get => _currentChromeMode;
            set
            {
                RASIC(ref _currentChromeMode, value);
                UpdateChromeMode(value);
            }
        }


        readonly CaptionButtonsSide _leftCaptionButtons = new(
            () => App.Current.LeftCaptionButtons,
            v => App.Current.LeftCaptionButtons = v
        );
        public CaptionButtonsSide LeftCaptionButtons
        {
            get => _leftCaptionButtons;
        }


        readonly CaptionButtonsSide _rightCaptionButtons = new(
            () => App.Current.RightCaptionButtons,
            v => App.Current.RightCaptionButtons = v
        );
        public CaptionButtonsSide RightCaptionButtons
        {
            get => _rightCaptionButtons;
        }


        readonly IReadOnlyCollection<CaptionButtonRole> _allCaptionButtons;
        public IReadOnlyCollection<CaptionButtonRole> AllCaptionButtons
        {
            get => _allCaptionButtons;
            //set
            private init
                => RASIC(ref _allCaptionButtons, value);
        }




        static WindowViewModel()
        {
        }
        public WindowViewModel()
            : base()
        {
            AllCaptionButtons = 
                Enum.GetValues<CaptionButtonRole>()
                .ToList()
                .AsReadOnly()
            ;
        }




        static void UpdateChromeMode(ManagedChromeMode mode)
            => App.Current.ManagedChromeHint = mode;

#if SORTABLE_AVALONIA
#region Sortable.Avalonia
            public void StUpdateCommand(object parameter)
                => StUpdate((SortableUpdateEventArgs)parameter);
            public void StDropCommand(object parameter)
                => StDrop((SortableDropEventArgs)parameter);


            public void StUpdate(SortableUpdateEventArgs e)
            {
                bool mutationResult = false; //e.ApplyUpdateMutation();
                Debug.WriteLine($"{nameof(StUpdate)}({Fmt(e)})\n    => {mutationResult};");
                if (mutationResult)
                    return;
            }
            public void StDrop(SortableDropEventArgs e)
            {
                bool mutationResult = false; //e.ApplyDropMutation();
                Debug.WriteLine($"{nameof(StDrop)}({Fmt(e)})\n    => {mutationResult};");
                if (mutationResult)
                    return;
            }
            static string Fmt(SortableUpdateEventArgs e)
                => nameof(e);
            static string Fmt(SortableDropEventArgs e)
                => nameof(e);
#endregion
#endif
    }
}