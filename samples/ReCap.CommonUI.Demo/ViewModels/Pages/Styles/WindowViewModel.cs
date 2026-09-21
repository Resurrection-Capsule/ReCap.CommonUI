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
        ManagedChromeHint _managedChromeHint = App.Current.ManagedChromeHint;
        public ManagedChromeHint ManagedChromeHint
        {
            get => _managedChromeHint;
            set
            {
                RASIC(ref _managedChromeHint, value);
                App.Current.ManagedChromeHint = _managedChromeHint;
            }
        }


        bool _useReserveCaptionArea = true;
        public bool UseReservedCaptionArea
        {
            get => _useReserveCaptionArea;
            set
            {
                RASIC(ref _useReserveCaptionArea, value);
                App.Current.MainVM.UseReservedCaptionArea = _useReserveCaptionArea;
            }
        }


        readonly WindowCaptionButtonsSideViewModel _leftCaptionButtonRoles = new(
            () => App.Current.LeftCaptionButtonRoles,
            v => App.Current.LeftCaptionButtonRoles = v
        );
        public WindowCaptionButtonsSideViewModel LeftCaptionButtonRoles
        {
            get => _leftCaptionButtonRoles;
        }


        readonly WindowCaptionButtonsSideViewModel _rightCaptionButtonRoles = new(
            () => App.Current.RightCaptionButtonRoles,
            v => App.Current.RightCaptionButtonRoles = v
        );
        public WindowCaptionButtonsSideViewModel RightCaptionButtonRoles
        {
            get => _rightCaptionButtonRoles;
        }


        readonly IReadOnlyCollection<CaptionButtonRole> _allCaptionButtonRoles;
        public IReadOnlyCollection<CaptionButtonRole> AllCaptionButtonRoles
        {
            get => _allCaptionButtonRoles;
            private init => RASIC(ref _allCaptionButtonRoles, value);
        }




        public WindowViewModel()
            : base()
        {
            AllCaptionButtonRoles = ManagedWindowChrome.ValidCaptionButtonRoles
                .ToList()
                .AsReadOnly()
            ;
        }




#region Sortable.Avalonia
            public void CBPoolUpdateCommand(object parameter)
                => CBPoolUpdate((SortableUpdateEventArgs)parameter);
            public void CBPoolUpdate(SortableUpdateEventArgs e)
            {
                bool mutationResult = e.ApplyUpdateMutation();
                Debug.WriteLine($"{nameof(CBPoolUpdate)}({nameof(e)})\n    => {mutationResult};");
                if (mutationResult)
                    return;
            }


            public void CBPoolDropCommand(object parameter)
                => CBPoolDrop((SortableDropEventArgs)parameter);
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

                Debug.WriteLine($"{nameof(CBPoolDrop)}({nameof(e)})\n    => {mutationResult};");
            }
#endregion
    }
}