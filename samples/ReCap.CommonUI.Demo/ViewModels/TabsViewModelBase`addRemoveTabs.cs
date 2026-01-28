using System;
using System.Collections.Generic;
using System.Linq;

namespace ReCap.CommonUI.Demo.ViewModels
{
    public abstract partial class TabsViewModelBase
    {
        protected bool AddTab(PageTabViewModel tabVM)
        {
            /*
            if (tabVM == null)
                return false;
            */
            if (_tabs.Contains(tabVM))
                return false;

            _tabs.Add(tabVM);
            return true;
        }


        protected bool[] AddTabs(IEnumerable<PageTabViewModel> tabVMs)
            => AddTabs(tabVMs.ToArray());
        protected bool[] AddTabs(params PageTabViewModel[] tabVMs)
        {
            /*
            if (tabVMs?.Any() ?? false)
                return Array.Empty<bool>();
            */

            int tabVMsCount = tabVMs.Length;
            bool[] ret = new bool[tabVMsCount];

            for (int i = 0; i < tabVMsCount; i++)
            {
                ret[i] = AddTab(tabVMs[i]);
            }

            return ret;
        }


        protected bool RemoveTab(PageTabViewModel tabVM)
            => /*_tabs.Contains(tabVM)
            && */_tabs.Remove(tabVM)
        ;


        protected bool[] RemoveTabs(IEnumerable<PageTabViewModel> tabVMs)
            => RemoveTabs(tabVMs.ToArray());
        protected bool[] RemoveTabs(params PageTabViewModel[] tabVMs)
        {
            /*
            if (tabVMs?.Any() ?? false)
                return Array.Empty<bool>();
            */

            int tabVMsCount = tabVMs.Length;
            bool[] ret = new bool[tabVMsCount];

            for (int i = 0; i < tabVMsCount; i++)
            {
                ret[i] = RemoveTab(tabVMs[i]);
            }

            return ret;
        }
    }
}