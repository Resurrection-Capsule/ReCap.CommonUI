using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Avalonia.Threading;
using ReCap.CommonUI.Attached.WindowChrome;
using Sortable.Avalonia;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    public partial class WindowViewModel
        : PageViewModelBase
    {
        static readonly IEnumerable<CaptionButtonRole> _VALID_CAPTION_BUTTON_ROLES
            = WindowChrome.ValidCaptionButtonRoles
                .ToList()
                .AsReadOnly()
            ;
        /*
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


        ManagedChromeElementHint _showTitleHint = App.Current.ShowTitleHint;
        public ManagedChromeElementHint ShowTitleHint
        {
            get => _showTitleHint;
            set
            {
                RASIC(ref _showTitleHint, value);
                App.Current.ShowTitleHint = _showTitleHint;
            }
        }


        ManagedChromeElementHint _showIconHint = App.Current.ShowIconHint;
        public ManagedChromeElementHint ShowIconHint
        {
            get => _showIconHint;
            set
            {
                RASIC(ref _showIconHint, value);
                App.Current.ShowIconHint = _showIconHint;
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
        */


        readonly WindowCaptionButtonsSideViewModel _leftCaptionButtonRoles = new(CommonUI.Controls.AppearanceHacks.CaptionButtonClusterPosition.Left);
        /*
            () => App.Current.LeftCaptionButtonRoles,
            v => App.Current.LeftCaptionButtonRoles = v
        );
        */
        public WindowCaptionButtonsSideViewModel LeftCaptionButtonRoles
        {
            get => _leftCaptionButtonRoles;
        }


        readonly WindowCaptionButtonsSideViewModel _rightCaptionButtonRoles = new(CommonUI.Controls.AppearanceHacks.CaptionButtonClusterPosition.Right);
        /*
            () => App.Current.RightCaptionButtonRoles,
            v => App.Current.RightCaptionButtonRoles = v
        );
        */
        public WindowCaptionButtonsSideViewModel RightCaptionButtonRoles
        {
            get => _rightCaptionButtonRoles;
        }


        /*
        readonly IReadOnlyCollection<CaptionButtonRole> _allCaptionButtonRoles;
        public IReadOnlyCollection<CaptionButtonRole> AllCaptionButtonRoles
        {
            get => _allCaptionButtonRoles;
            private init => RASIC(ref _allCaptionButtonRoles, value);
        }
        */
        /*
        ObservableCollection<CaptionButtonRole> _allCaptionButtonRoles = new();
        public ObservableCollection<CaptionButtonRole> AllCaptionButtonRoles
        {
            get => _allCaptionButtonRoles;
            private set
            {
                if (_allCaptionButtonRoles != null)
                    _allCaptionButtonRoles.CollectionChanged -= AllCaptionButtonRoles_CollectionChanged;

                RASIC(ref _allCaptionButtonRoles, value ?? new());

                _allCaptionButtonRoles.CollectionChanged += AllCaptionButtonRoles_CollectionChanged;
            }
        }
        */
        readonly ObservableCollection<CaptionButtonRole> _allCaptionButtonRoles = new();
        public ObservableCollection<CaptionButtonRole> AllCaptionButtonRoles
        {
            get => _allCaptionButtonRoles;
        }




        public WindowViewModel()
            : base()
        {
            RefreshAllCaptionButtonRoles();
            AllCaptionButtonRoles.CollectionChanged += AllCaptionButtonRoles_CollectionChanged;
        }
        void AllCaptionButtonRoles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => RefreshAllCaptionButtonRoles();


        bool _refreshingAllCaptionButtonRoles = false;
        void RefreshAllCaptionButtonRoles()
        {
            if (_refreshingAllCaptionButtonRoles)
                return;

            _refreshingAllCaptionButtonRoles = true;
            AllCaptionButtonRoles.CollectionChanged -= AllCaptionButtonRoles_CollectionChanged;
            /*
            */
            Dispatcher.UIThread.Post(() =>
            {
                AllCaptionButtonRoles.Clear();
                foreach (var role in _VALID_CAPTION_BUTTON_ROLES)
                {
                    AllCaptionButtonRoles.Add(role);
                }

                _refreshingAllCaptionButtonRoles = false;
                AllCaptionButtonRoles.CollectionChanged += AllCaptionButtonRoles_CollectionChanged;
            }
            //, DispatcherPriority.ApplicationIdle
            );
            /*
            */
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