using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ReCap.CommonUI.Attached.WindowChrome;
using Sortable.Avalonia;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    public partial class WindowViewModel
        : PageViewModelBase
    {
        ManagedChromeMode _managedChromeHint = App.Current.ManagedChromeHint;
        public ManagedChromeMode ManagedChromeHint
        {
            get => _managedChromeHint;
            set
            {
                RASIC(ref _managedChromeHint, value);
                App.Current.ManagedChromeHint = value;
            }
        }


        bool _useReserveCaptionArea = true;
        public bool UseReservedCaptionArea
        {
            get => _useReserveCaptionArea;
            set
            {
                RASIC(ref _useReserveCaptionArea, value);
                App.Current.MainVM.UseReservedCaptionArea = value;
            }
        }


        readonly WindowCaptionButtonsSideViewModel _leftCaptionButtons = new(
            () => App.Current.LeftCaptionButtons,
            v => App.Current.LeftCaptionButtons = v
        );
        public WindowCaptionButtonsSideViewModel LeftCaptionButtons
        {
            get => _leftCaptionButtons;
        }


        readonly WindowCaptionButtonsSideViewModel _rightCaptionButtons = new(
            () => App.Current.RightCaptionButtons,
            v => App.Current.RightCaptionButtons = v
        );
        public WindowCaptionButtonsSideViewModel RightCaptionButtons
        {
            get => _rightCaptionButtons;
        }


        readonly IReadOnlyCollection<CaptionButtonRole> _allCaptionButtons;
        public IReadOnlyCollection<CaptionButtonRole> AllCaptionButtons
        {
            get => _allCaptionButtons;
            private init => RASIC(ref _allCaptionButtons, value);
        }




        public WindowViewModel()
            : base()
        {
            //AllCaptionButtons = Enum.GetValues<CaptionButtonRole>()
            AllCaptionButtons = WindowChromeAddon.ValidCaptionButtonRoles
                .ToList()
                .AsReadOnly()
            ;
        }




            ///*
#region Sortable.Avalonia
            public void CBPoolUpdateCommand(object parameter)
                => CBPoolUpdate((SortableUpdateEventArgs)parameter);
            public void CBPoolDropCommand(object parameter)
                => CBPoolDrop((SortableDropEventArgs)parameter);


            public void CBPoolUpdate(SortableUpdateEventArgs e)
            {
                bool mutationResult = e.ApplyUpdateMutation();
                Debug.WriteLine($"{nameof(CBPoolUpdate)}({Fmt(e)})\n    => {mutationResult};");
                if (mutationResult)
                    return;
            }
            public void CBPoolDrop(SortableDropEventArgs e)
            {
                if (e.SourceCollection.IsReadOnly)
                    return;

                bool mutationResult;
                if (e.TargetCollection.IsReadOnly)
                {
                    e.SourceCollection.RemoveAt(e.OldIndex);
                    mutationResult = true;
                }
                else
                {
                    mutationResult = e.ApplyDropMutation();
                }

                Debug.WriteLine($"{nameof(CBPoolDrop)}({Fmt(e)})\n    => {mutationResult};");
            }
            static string Fmt(SortableUpdateEventArgs e)
                => nameof(e);
            static string Fmt(SortableDropEventArgs e)
                => nameof(e);
#endregion
            //*/
    }
}