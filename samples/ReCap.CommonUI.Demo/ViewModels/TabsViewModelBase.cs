using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ReCap.CommonUI.Demo.ViewModels
{
    public abstract partial class TabsViewModelBase
        : ViewModelBase
    {
        readonly ObservableCollection<PageTabViewModel> _tabs = new();
        public IReadOnlyList<PageTabViewModel> Tabs
        {
            get => _tabs;
        }


        int _selectedIndex = 0;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => RASIC(ref _selectedIndex, value);
        }

        public bool IsTabIndexValid(int index)
        {
            int tabCount = Tabs.Count;
            if (tabCount <= 0)
                return false;
            
            if (index >= tabCount)
                return false;
            else if (index < 0)
                return false;
            
            return true;
        }

        public int WrapTabIndex(int rawIndex)
        {
            int newIndex = rawIndex;
            
            int tabCount = Tabs.Count;
            while (newIndex >= tabCount)
            {
                newIndex -= tabCount;
            }
            
            while (newIndex < 0)
            {
                newIndex += tabCount;
            }
            
            return newIndex;
        }


        public bool TryGetTabAt(int idx, out PageTabViewModel tab)
        {
            if (IsTabIndexValid(idx))
            {
                tab = Tabs[idx];
                return tab != null;
            }
            else
            {
                tab = null;
                return false;
            }
        }


        public bool TryGetTabContentAt(int idx, out ViewModelBase content)
        {
            if (TryGetTabAt(idx, out PageTabViewModel tab))
            {
                content = tab.ContentVM;
                return content != null;
            }
            else
            {
                content = null;
                return false;
            }
        }


        public TabsViewModelBase()
            : base()
        {}




        public void NextTab()
            => SelectedIndex = WrapTabIndex(SelectedIndex + 1);
        public void PreviousTab()
            => SelectedIndex = WrapTabIndex(SelectedIndex - 1);
        public bool JumpToTab(int index)
        {
            if (!IsTabIndexValid(index))
                return false;
            
            SelectedIndex = index;
            return true;
        }

        
        public void JumpToTabCommand(object parameter)
        {
            if (parameter == null)
                return;
            int index;
            
            if (parameter is int pBool)
            {
                index = pBool;
            }
            else
            {
                string prmStr = (parameter is string sPrm)
                        ? sPrm
                        : parameter.ToString()
                ;
                
                if (!int.TryParse(prmStr, out index))
                    return;
            }

            JumpToTab(index);
        }
        public void NextTabCommand(object _ = null)
            => NextTab();
        public void PreviousTabCommand(object _ = null)
            => PreviousTab();
    }
}