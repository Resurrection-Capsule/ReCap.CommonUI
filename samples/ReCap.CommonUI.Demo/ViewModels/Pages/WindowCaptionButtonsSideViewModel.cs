using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using Avalonia.Threading;
using ReCap.CommonUI.Attached.WindowChrome;
using ReCap.CommonUI.Controls.AppearanceHacks;
using ReCap.CommonUI.Util;
using Sortable.Avalonia;

namespace ReCap.CommonUI.Demo.ViewModels.Pages
{
    public sealed class WindowCaptionButtonsSideViewModel
        : RxObjectBase
    {
        string _inputText;
        public string InputText
        {
            get => _inputText;
            set => RASIC(ref _inputText, value);
        }



        static readonly IEnumerable<object> _ERRORS_NONE = Array.Empty<object>();
        static readonly IEnumerable<object> _ERRORS_FAIL = new[]
        {
            new Exception("Invalid input"),
        };
        IEnumerable<object> _errors = Array.Empty<object>();
        public IEnumerable<object> Errors
        {
            get => _errors;
            set => RASIC(ref _errors, value);
        }


        ObservableCollection<CaptionButtonRole> _roles = null;
        public ObservableCollection<CaptionButtonRole> CaptionButtons
        {
            get => _roles;
            set
            {
                if (_roles != null)
                    _roles.CollectionChanged -= CaptionButtons_CollectionChanged;

                RASIC(ref _roles, value ?? new());

                _roles.CollectionChanged += CaptionButtons_CollectionChanged;
            }
        }


        ObservableCollection<CaptionButtonRole> GetUpdatedCaptionButtons(IEnumerable<CaptionButtonRole> roles)
        {
            ObservableCollection<CaptionButtonRole> captionButtons = CaptionButtons;

            if (roles != null)
                captionButtons = new(roles);
            else if (captionButtons != null)
                captionButtons.Clear();
            else
                captionButtons = new();

            return captionButtons;
        }
        void SetCaptionButtons(IEnumerable<CaptionButtonRole> roles)
            => CaptionButtons = roles != null
                ? new(roles)
                : new()
            ;


        readonly Func<IEnumerable<CaptionButtonRole>> _getter;
        IEnumerable<CaptionButtonRole> GetRoles()
            => _getter();




        static CaptionButtonRolesPair GetAppOrPlatformDefaultCaptionButtonRoles()
            => TryGetAppCaptionButtonRoles(out CaptionButtonRolesPair rolesPair)
                ? rolesPair
                : WindowChrome.PlatformDefaultCaptionButtonRoles
            ;
        static bool TryGetAppCaptionButtonRoles(out CaptionButtonRolesPair rolesPair)
        {
            var app = App.Current;
            if (app == null)
                goto fail;

            CaptionButtonRolesPair? rolesMaybe = app.CaptionButtonRoles;
            if (!rolesMaybe.HasValue)
                goto fail;

            rolesPair = rolesMaybe.Value;
            return true;

            fail:
            rolesPair = default;
            return false;
        }


        static IEnumerable<CaptionButtonRole> LeftGet()
            => GetAppOrPlatformDefaultCaptionButtonRoles().Left;
        static IEnumerable<CaptionButtonRole> RightGet()
            => GetAppOrPlatformDefaultCaptionButtonRoles().Right;




        readonly CaptionButtonClusterPosition _position;
        public WindowCaptionButtonsSideViewModel(CaptionButtonClusterPosition position)
            : this()
        {
            _position = position;
            _getter = position switch
            {
                CaptionButtonClusterPosition.Left => LeftGet,
                CaptionButtonClusterPosition.Right => RightGet,
                _ => throw new InvalidEnumArgumentException(nameof(position), (int)position, typeof(CaptionButtonClusterPosition)),
            };

            var roles = GetRoles();
            SetCaptionButtons(roles);
            InputText = roles.ToString();

            PropertyChanged += This_PropertyChanged;
        }
        private WindowCaptionButtonsSideViewModel()
            : base()
        {}

        void CaptionButtons_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            => OnCaptionButtonsChanged();

        void This_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string propName = e.PropertyName;
            switch (propName)
            {
                case nameof(InputText):
                    OnInputTextChanged();
                    break;

                case nameof(CaptionButtons):
                    OnCaptionButtonsChanged();
                    break;

                default:
                    break;
            }
        }


        void OnInputTextChanged()
        {
            if (WindowChromeEnumsHelper.TryParseCaptionButtonRoles(InputText, out IEnumerable<CaptionButtonRole> roles))
            {
                SetCaptionButtons(roles);
                Errors = _ERRORS_NONE;
            }
            else
            {
                Errors = _ERRORS_FAIL;
            }
        }


        void OnCaptionButtonsChanged()
            => Dispatcher.UIThread.Post(OnCaptionButtonsChangedInternal);
        void OnCaptionButtonsChangedInternal()
        {
            App.Current?.UpdateCaptionButtonRoles(CaptionButtons, _position);
            CaptionButtonsChanged?.Invoke(this, new());
        }




        public override Type GetViewType()
            => null;


#region Sortable.Avalonia
        public void CBSideUpdateCommand(object parameter)
            => CBSideUpdate((SortableUpdateEventArgs)parameter);
        public void CBSideUpdate(SortableUpdateEventArgs e)
        {
            var result = e.ApplyUpdateMutation();
            Debug.WriteLine($"{nameof(CBSideUpdate)}({e}) => {result};");
        }


        public void CBSideDropCommand(object parameter)
            => CBSideDrop((SortableDropEventArgs)parameter);
        public void CBSideDrop(SortableDropEventArgs e)
        {
            var result = e.ApplyDropMutation();
            Debug.WriteLine($"{nameof(CBSideDrop)}({e}) => {result};");
        }


        public void CBSideReleaseCommand(object parameter)
            => CBSideRelease((SortableReleaseEventArgs)parameter);
        public void CBSideRelease(SortableReleaseEventArgs e)
        {
            int oldIdx = e.OldIndex;
            CaptionButtons.RemoveAt(oldIdx);
        }
#endregion




        public event EventHandler CaptionButtonsChanged;
    }
}