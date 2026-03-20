using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using ReCap.CommonUI.Attached.WindowChrome;
using Sortable.Avalonia;

namespace ReCap.CommonUI.Demo.ViewModels.Pages.Styles
{
    partial class WindowViewModel
    {
        public partial class CaptionButtonsSide
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




            public CaptionButtonsSide(Func<CaptionButtonRoles> getter, Action<CaptionButtonRoles> setter)
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
            public void StUpdateCommand(object parameter)
                => StUpdate((SortableUpdateEventArgs)parameter);
            public void StDropCommand(object parameter)
                => StDrop((SortableDropEventArgs)parameter);
            public void StReleaseCommand(object parameter)
                => StRelease((SortableReleaseEventArgs)parameter);
#if DROP_OUTSIDE_HACK
            sealed class StActionInfo
            {
                public int OldIndex
                {
                    get;
                    init;
                }


                public int NewIndex
                {
                    get;
                    init;
                }


                public Action StAction
                {
                    get;
                    init;
                }




                public StActionInfo(int oldIndex, int newIndex, Action stAction)
                {
                    OldIndex = oldIndex;
                    NewIndex = newIndex;
                    StAction = stAction;
                }


                public StActionInfo(SortableUpdateEventArgs e, Action stAction)
                    : this(e.OldIndex, e.NewIndex, stAction)
                {}


                public StActionInfo(SortableDropEventArgs e, Action stAction)
                    : this(e.OldIndex, e.NewIndex, stAction)
                {}
            }


            bool _executeNext = true;
            StActionInfo _nextStAction = null;
            public void StUpdate(SortableUpdateEventArgs e)
                => _nextStAction = new(e, () =>
                    {
                        bool mutationResult = e.ApplyUpdateMutation();
                        Debug.WriteLine($"{nameof(StUpdate)}({nameof(e)})\n    => {mutationResult};");
                    }
                );

            public void StDrop(SortableDropEventArgs e)
            {
                if (e.SourceCollection.IsReadOnly)
                {
                    _executeNext = false;
                    e.TransferMode = SortableTransferMode.Copy;

                    bool mutationResult = e.ApplyDropMutation();
                    Debug.WriteLine($"{nameof(StDrop)}({nameof(e)})\n    => {mutationResult};");
                }
                else
                {
                    _nextStAction = new(e, () =>
                        {
                            bool mutationResult = e.ApplyDropMutation();
                            Debug.WriteLine($"{nameof(StDrop)}({nameof(e)})\n    => {mutationResult};");
                        }
                    );
                }
            }
#endif
            public void StUpdate(SortableUpdateEventArgs e)
#if DROP_OUTSIDE_HACK
                => _nextStAction = new(e, () =>
#endif
                    {
                        bool mutationResult = e.ApplyUpdateMutation();
                        Debug.WriteLine($"{nameof(StUpdate)}({nameof(e)})\n    => {mutationResult};");
                    }
#if DROP_OUTSIDE_HACK
                );
#endif

            public void StDrop(SortableDropEventArgs e)
            {
#if DROP_OUTSIDE_HACK
                if (e.SourceCollection.IsReadOnly)
                {
                    _executeNext = false;
                    e.TransferMode = SortableTransferMode.Copy;

                    bool mutationResult = e.ApplyDropMutation();
                    Debug.WriteLine($"{nameof(StDrop)}({nameof(e)})\n    => {mutationResult};");
                }
                else
                {
                    _nextStAction = new(e, () =>
                        {
                            bool mutationResult = e.ApplyDropMutation();
                            Debug.WriteLine($"{nameof(StDrop)}({nameof(e)})\n    => {mutationResult};");
                        }
                    );
                }
#else
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

                Debug.WriteLine($"{nameof(StDrop)}({nameof(e)})\n    => {mutationResult};");
#endif
            }


            public void StRelease(SortableReleaseEventArgs e)
            {
                int oldIdx = e.OldIndex;
                if (oldIdx >= 0)
                    CaptionButtons.RemoveAt(oldIdx);
            }




            void StUpdate_Manual(SortableUpdateEventArgs e)
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
            void StDrop_Manual(SortableDropEventArgs e)
            {
                var captionButtons = CaptionButtons;
                int oldIdx = e.OldIndex;
                int newIdx = e.OldIndex;
                //Debug.WriteLine($"{nameof(StUpdate)}({e}):");
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
}