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




        bool TryGetMainVM(out UITestViewModel mainVM)
        {
            var dataCtx = DataContext;
            if (dataCtx == null)
                goto fail;
            else if (dataCtx is not UITestViewModel uiTestVM)
                goto fail;
            else
            {
                mainVM = uiTestVM;
                return true;
            }

            fail:
            mainVM = default;
            return false;
        }


        bool TryGetAutoVM(KeyModifiers modifiers, out TabsViewModelBase tabsVM)
            => TryGetVMs(modifiers, out _, out tabsVM);
        bool TryGetAutoVM(bool subTab, out TabsViewModelBase tabsVM)
            => TryGetVMs(subTab, out _, out tabsVM);


        bool TryGetVMs(bool subTab, out UITestViewModel mainVM, out TabsViewModelBase tabsVM)
        {
            if (!TryGetMainVM(out mainVM))
            {
                tabsVM = default;
                return false;
            }


            if (!subTab)
                goto noTab;

            int tabIdx = mainVM.SelectedIndex;
            if (tabIdx < 0)
                goto noTab;

            var tabs = mainVM.Tabs;
            if (tabs == null)
                goto noTab;

            if (tabs.Count <= tabIdx)
                goto noTab;

            var tab = tabs[tabIdx];
            /*
            var tab = mainVM.Tabs[mainVM.SelectedIndex];
            */

            if (tab == null)
                goto noTab;

            var tabContent = tab.ContentVM;
            /*
            if (tabContent == null)
                goto noTab;
            else if (tabContent is not TabsViewModelBase tabVM)
                goto noTab;
            else
            */
            if ((tabContent != null) && (tabContent is TabsViewModelBase tabVM))
            {
                tabsVM = tabVM;
                return true;
            }

            noTab:
            tabsVM = mainVM;
            return true;
        }
        bool TryGetVMs(KeyModifiers modifiers, out UITestViewModel mainVM, out TabsViewModelBase tabsVM)
            => TryGetVMs(!modifiers.HasFlag(KeyModifiers.Alt), out mainVM, out tabsVM);




        void This_PointerWheelChanged(object sender, PointerWheelEventArgs e)
        {
            if (!e.KeyModifiers.HasFlag(KeyModifiers.Control))
                return;
            if (!TryGetMainVM(out UITestViewModel mainVM))
                return;

            var deltaY = e.Delta.Y;
            
            if (deltaY > 0)
                mainVM.AdjustScaleFactor(true);
            else if (deltaY < 0)
                mainVM.AdjustScaleFactor(false);
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
            if (!TryGetVMs(e.KeyModifiers, out UITestViewModel mainVM, out TabsViewModelBase tabsVM))
                return;

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
                mainVM.AdjustScaleFactor(true);
            }
            else if (_ZOOM_OUT_KEYS.Contains(key))
            {
                mainVM.AdjustScaleFactor(false);
            }
            else if (_ZOOM_RESET_KEYS.Contains(key))
            {
                mainVM.ResetScaleFactor();
            }
            else
            {
                return;
            }
            
            e.Handled = true;
        }
    }
}
