using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using ReCap.CommonUI.Attached.WindowChrome;
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
        static readonly IEnumerable<object> _ERRORS_FAIL = Helpers.LonerArray<Exception>(new("Invalid input"));
        IEnumerable<object> _errors = Array.Empty<object>();
        public IEnumerable<object> Errors
        {
            get => _errors;
            set =>  RASIC(ref _errors, value);
        }


        CaptionButtonRoles _captionButtons = null;
        public CaptionButtonRoles CaptionButtons
        {
            get => _captionButtons;
            set => RASIC(ref _captionButtons, value);
        }


        readonly Func<CaptionButtonRoles> _getter;
        readonly Action<CaptionButtonRoles> _setter;
        CaptionButtonRoles ExternalCaptionButtons
        {
            get => _getter();
            set => _setter(value);
        }




        public WindowCaptionButtonsSideViewModel(Func<CaptionButtonRoles> getter, Action<CaptionButtonRoles> setter)
        {
            _getter = getter;
            _setter = setter;

            var extCaptionButtons = ExternalCaptionButtons;
            CaptionButtons = extCaptionButtons;
            InputText = extCaptionButtons.ToString();

            PropertyChanged += This_PropertyChanged;
        }


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
            if (CaptionButtonRoles.TryParse(InputText, out CaptionButtonRoles captionButtons))
            {
                CaptionButtons = captionButtons;
                Errors = _ERRORS_NONE;
            }
            else
            {
                Errors = _ERRORS_FAIL;
            }
        }


        void OnCaptionButtonsChanged()
        {
            ExternalCaptionButtons = CaptionButtons;
        }




        public override Type GetViewType()
            => null;


        public bool MoveCommand(object parameter)
        {
            if (parameter is (int sourceIndex, int targetIndex))
            {
                var captionButtons = CaptionButtons;
                captionButtons.Move(sourceIndex, targetIndex);
                return true;
            }
            return false;
        }


#region Sortable.Avalonia
        public void CBSideUpdateCommand(object parameter)
            => CBSideUpdate((SortableUpdateEventArgs)parameter);
        public void CBSideDropCommand(object parameter)
            => CBSideDrop((SortableDropEventArgs)parameter);
        public void CBSideReleaseCommand(object parameter)
            => CBSideRelease((SortableReleaseEventArgs)parameter);


        public void CBSideUpdate(SortableUpdateEventArgs e)
            {
                bool mutationResult = e.ApplyUpdateMutation();
                Debug.WriteLine($"{nameof(CBSideUpdate)}({nameof(e)})\n    => {mutationResult};");
            }


        public void CBSideDrop(SortableDropEventArgs e)
        {
            object mutationResult;

            if (e.SourceCollection.IsReadOnly)
            {
                CaptionButtons.Insert(e.NewIndex, (CaptionButtonRole)e.Item);
                mutationResult = 2;
            }
            else
            {
                mutationResult = e.ApplyDropMutation();
            }

            Debug.WriteLine($"{nameof(CBSideDrop)}({nameof(e)})\n    => {mutationResult};");
        }


        public void CBSideRelease(SortableReleaseEventArgs e)
        {
            int oldIdx = e.OldIndex;
            if (oldIdx >= 0)
                CaptionButtons.RemoveAt(oldIdx);
        }




        void CBSideUpdate_Manual(SortableUpdateEventArgs e)
        {
            var captionButtons = CaptionButtons;
            int oldIdx = e.OldIndex;
            int newIdx = e.OldIndex;
            //Debug.WriteLine($"{nameof(StUpdate)}({e}):");
            Debug.WriteLine($"[{oldIdx}] => [{newIdx}]");

            switch (e.Mode)
            {
                case SortableMode.Sort:
                {
                    captionButtons.Move(oldIdx, newIdx);
                    break;
                }

                case SortableMode.Swap:
                {
                    var oldItem = captionButtons[oldIdx];
                    var newItem = captionButtons[newIdx];

                    if (newIdx > oldIdx)
                    {
                        captionButtons.RemoveAt(newIdx);
                        captionButtons.RemoveAt(oldIdx);
                        captionButtons.Insert(oldIdx, newItem);
                        captionButtons.Insert(newIdx, oldItem);
                    }
                    else if (oldIdx > newIdx)
                    {
                        captionButtons.RemoveAt(oldIdx);
                        captionButtons.RemoveAt(newIdx);
                        captionButtons.Insert(newIdx, oldItem);
                        captionButtons.Insert(oldIdx, newItem);
                    }
                    break;
                }

                default:
                    break;
            }
        }
        void CBSideDrop_Manual(SortableDropEventArgs e)
        {
            var captionButtons = CaptionButtons;
            int oldIdx = e.OldIndex;
            int newIdx = e.OldIndex;
            Debug.WriteLine($"[{oldIdx}] => [{newIdx}]");

            switch (e.TransferMode)
            {
                case SortableTransferMode.Move:
                {
                    captionButtons.Move(oldIdx, newIdx);
                    break;
                }

                case SortableTransferMode.Swap:
                {
                    var oldItem = captionButtons[oldIdx];
                    var newItem = captionButtons[newIdx];

                    if (newIdx > oldIdx)
                    {
                        captionButtons.RemoveAt(newIdx);
                        captionButtons.RemoveAt(oldIdx);
                        captionButtons.Insert(oldIdx, newItem);
                        captionButtons.Insert(newIdx, oldItem);
                    }
                    else if (oldIdx > newIdx)
                    {
                        captionButtons.RemoveAt(oldIdx);
                        captionButtons.RemoveAt(newIdx);
                        captionButtons.Insert(newIdx, oldItem);
                        captionButtons.Insert(oldIdx, newItem);
                    }
                    break;
                }

                default:
                    break;
            }
        }
#endregion
    }
}