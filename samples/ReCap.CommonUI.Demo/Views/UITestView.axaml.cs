using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ReCap.CommonUI.Demo.ViewModels;

namespace ReCap.CommonUI.Demo.Views
{
    public partial class UITestView
        : UserControl
    {
        public UITestViewModel VM
        {
            get => DataContext as UITestViewModel;
        }




        public UITestView()
        {
            InitializeComponent();
            Dispatcher.UIThread.Post(() =>
            {
                var topLevel = TopLevel.GetTopLevel(this);
                
                topLevel.AddHandler(
                    KeyDownEvent
                    , This_KeyDown
                    , handledEventsToo: true
                );
                topLevel.PointerWheelChanged += This_PointerWheelChanged;
            }, DispatcherPriority.ApplicationIdle);
        }


        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }




        TabsViewModelBase GetVM(bool subTab)
        {
            var mainVM = VM;

            if (subTab)
            {
                int tabIdx = mainVM.SelectedIndex;
                var tab = mainVM.Tabs[tabIdx];

                if (tab.ContentVM is TabsViewModelBase tabVM)
                {
                    return tabVM;
                }
            }

            return mainVM;
        }
        TabsViewModelBase GetVM(KeyModifiers modifiers)
            => GetVM(!modifiers.HasFlag(KeyModifiers.Alt));




        void This_PointerWheelChanged(object sender, PointerWheelEventArgs e)
        {
            if (!e.KeyModifiers.HasFlag(KeyModifiers.Control))
                return;
            
            var deltaY = e.Delta.Y;
            
            if (deltaY > 0)
                VM.AdjustScaleFactor(true);
            else if (deltaY < 0)
                VM.AdjustScaleFactor(false);
            else
                return;
            
            e.Handled = true;
        }




        static readonly IReadOnlyList<Key> _ZOOM_IN_KEYS = new List<Key>()
        {
            Key.Add,
            Key.OemPlus,
        }.AsReadOnly();
        static readonly IReadOnlyList<Key> _ZOOM_OUT_KEYS = new List<Key>()
        {
            Key.Subtract,
            Key.OemMinus,
        }.AsReadOnly();
        static readonly IReadOnlyList<Key> _ZOOM_RESET_KEYS = new List<Key>()
        {
            Key.D0,
            Key.NumPad0,
        }.AsReadOnly();


        static readonly char _TAB_INDEX_KEY_PREFIX_TRIM_END_CHAR = '1';
        static readonly IReadOnlyList<string> _TAB_INDEX_KEY_PREFIXES = new List<string>()
        {
            nameof(Key.D1).TrimEnd(_TAB_INDEX_KEY_PREFIX_TRIM_END_CHAR),
            nameof(Key.NumPad1).TrimEnd(_TAB_INDEX_KEY_PREFIX_TRIM_END_CHAR)
        }.AsReadOnly();

        static bool IsNumericalKey(Key key, out int keyNumber)
        {
            string keyName = Enum.GetName(key);
            if (string.IsNullOrEmpty(keyName) || string.IsNullOrWhiteSpace(keyName))
                goto none;
            
            foreach (string prefix in _TAB_INDEX_KEY_PREFIXES)
            {
                if (!keyName.StartsWith(prefix))
                    continue;
                
                if (int.TryParse(keyName.Substring(prefix.Length), out keyNumber))
                    return true;
            }

            none:
            keyNumber = -1;
            return false;
        }


        void This_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.KeyModifiers.HasFlag(KeyModifiers.Control))
                return;

            var tabsVM = GetVM(e.KeyModifiers);
            var key = e.Key;
            if (key == Key.Tab)
            {
                if (e.KeyModifiers.HasFlag(KeyModifiers.Shift))
                    tabsVM.PreviousTabCommand();
                else
                    tabsVM.NextTabCommand();
            }
            else if (e.KeyModifiers.HasFlag(KeyModifiers.Shift))
            {
                return;
            }
            else if (key == Key.PageUp)
            {
                tabsVM.PreviousTabCommand();
            }
            else if (key == Key.PageDown)
            {
                tabsVM.NextTabCommand();
            }
            else if (IsNumericalKey(key, out int keyNumber) && tabsVM.JumpToTab(keyNumber - 1))
            {
                //Do nothing - tabsVM.JumpToTab call is in condition instead of body to ensure CTRL+0 is still handled elsewhere
            }
            else if (_ZOOM_IN_KEYS.Contains(key))
            {
                VM.AdjustScaleFactor(true);
            }
            else if (_ZOOM_OUT_KEYS.Contains(key))
            {
                VM.AdjustScaleFactor(false);
            }
            else if (_ZOOM_RESET_KEYS.Contains(key))
            {
                VM.ResetScaleFactor();
            }
            else
            {
                return;
            }
            
            e.Handled = true;
        }
    }
}
