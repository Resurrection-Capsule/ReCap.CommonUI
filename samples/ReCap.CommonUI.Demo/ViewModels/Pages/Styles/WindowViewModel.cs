using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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


        readonly WindowCaptionButtonsSideViewModel _leftCaptionButtonRoles = new(CommonUI.Controls.AppearanceHacks.CaptionButtonClusterPosition.Left);
        public WindowCaptionButtonsSideViewModel LeftCaptionButtonRoles
        {
            get => _leftCaptionButtonRoles;
        }


        readonly WindowCaptionButtonsSideViewModel _rightCaptionButtonRoles = new(CommonUI.Controls.AppearanceHacks.CaptionButtonClusterPosition.Right);
        public WindowCaptionButtonsSideViewModel RightCaptionButtonRoles
        {
            get => _rightCaptionButtonRoles;
        }


        readonly ObservableCollection<CaptionButtonRole> _allCaptionButtonRoles = new();
        public ObservableCollection<CaptionButtonRole> AllCaptionButtonRoles
        {
            get => _allCaptionButtonRoles;
        }


        
        static readonly KeyValuePair<string, CaptionButtonRolesPair?> _PLATFORM_DEFAULTS = new("Platform Defaults", null);
        readonly ObservableCollection<KeyValuePair<string, CaptionButtonRolesPair?>> _captionButtonPresets = new()
        {
            _PLATFORM_DEFAULTS,
            new("Lone WindowMenus", new(
                new[]
                {
                    CaptionButtonRole.WindowMenu,
                },
                new[]
                {
                    CaptionButtonRole.WindowMenu,
                }
            )),
            new("Innermost WindowMenus", new(
                new[]
                {
                    CaptionButtonRole.FullScreen,
                    CaptionButtonRole.WindowMenu,
                },
                new[]
                {
                    CaptionButtonRole.WindowMenu,
                    CaptionButtonRole.Minimize,
                    CaptionButtonRole.Maximize,
                    CaptionButtonRole.Close,
                }
            )),
            new("Sandwiched WindowMenus", new(
                new[]
                {
                    CaptionButtonRole.FullScreen,
                    CaptionButtonRole.WindowMenu,
                    CaptionButtonRole.Minimize,
                },
                new[]
                {
                    CaptionButtonRole.Maximize,
                    CaptionButtonRole.WindowMenu,
                    CaptionButtonRole.Close,
                }
            )),
            new("Empty", new(
                Array.Empty<CaptionButtonRole>(),
                Array.Empty<CaptionButtonRole>()
            )),
        };
        public ObservableCollection<KeyValuePair<string, CaptionButtonRolesPair?>> CaptionButtonPresets
        {
            get => _captionButtonPresets;
        }


        static readonly KeyValuePair<string, CaptionButtonRolesPair?> _NO_PRESET = new(null, null);
        KeyValuePair<string, CaptionButtonRolesPair?> _selectedCaptionButtonPreset = _NO_PRESET;
        public KeyValuePair<string, CaptionButtonRolesPair?> SelectedCaptionButtonPreset
        {
            get => _selectedCaptionButtonPreset;
            set => RASIC(ref _selectedCaptionButtonPreset, value);
        }




        public WindowViewModel()
            : base()
        {
            SelectedCaptionButtonPreset = _PLATFORM_DEFAULTS;
            RefreshAllCaptionButtonRoles();
            PropertyChanged += This_PropertyChanged;
            LeftCaptionButtonRoles.CaptionButtonsChanged += CaptionButtonsSide_CaptionButtonsChanged;
            RightCaptionButtonRoles.CaptionButtonsChanged += CaptionButtonsSide_CaptionButtonsChanged;
            AllCaptionButtonRoles.CollectionChanged += AllCaptionButtonRoles_CollectionChanged;
        }


        int _presetChanging = 0;
        void CaptionButtonsSide_CaptionButtonsChanged(object sender, EventArgs e)
        {
            if (_presetChanging > 0)
                _presetChanging--;
            else
                SelectedCaptionButtonPreset = _NO_PRESET;
        }


        void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SelectedCaptionButtonPreset))
                return;

            var kvp = SelectedCaptionButtonPreset;
            if (_NO_PRESET.Equals(kvp))
                return;

            if (_PLATFORM_DEFAULTS.Equals(kvp))
                ApplyPreset(WindowChrome.PlatformDefaultCaptionButtonRoles);
            else
                ApplyPreset(kvp);
        }
        bool ApplyPreset(KeyValuePair<string, CaptionButtonRolesPair?> preset)
        {
            var pairMaybe = preset.Value;
            if (!pairMaybe.HasValue)
                return false;

            ApplyPreset(pairMaybe.Value);
            return true;
        }
        void ApplyPreset(CaptionButtonRolesPair preset)
        {
            _presetChanging = 2;
            LeftCaptionButtonRoles.CaptionButtons = new(preset.Left);
            RightCaptionButtonRoles.CaptionButtons = new(preset.Right);
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

            Dispatcher.UIThread.Post(() =>
            {
                AllCaptionButtonRoles.Clear();
                foreach (var role in _VALID_CAPTION_BUTTON_ROLES)
                {
                    AllCaptionButtonRoles.Add(role);
                }

                _refreshingAllCaptionButtonRoles = false;
                AllCaptionButtonRoles.CollectionChanged += AllCaptionButtonRoles_CollectionChanged;
            });
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