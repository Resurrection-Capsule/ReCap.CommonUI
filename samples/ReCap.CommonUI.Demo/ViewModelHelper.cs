using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReCap.CommonUI.Demo.ViewModels;
using System;

namespace ReCap.CommonUI.Demo
{
    public static class ViewModelHelper
    {
        public static bool TryResolveObjectAsViewModel(object viewModelMaybe, out ViewModelBase resolved)
        {
            if (viewModelMaybe == null)
                goto fail;
            else if (viewModelMaybe is not ViewModelBase vm)
                goto fail;
            else
                return TryResolveViewModel(vm, out resolved);

            fail:
            resolved = null;
            return false;
        }


        public static bool TryResolveViewModel(ViewModelBase viewModel, out ViewModelBase resolved)
        {
            if (viewModel == null)
                goto fail;


            resolved = null;
            while (viewModel != resolved)
            {
                if (!TryResolveViewModelInternal(viewModel, out ViewModelBase resolvedVM))
                    goto fail;
            }

            return resolved != null;


            fail:
            resolved = null;
            return false;
        }


        static bool TryResolveViewModelInternal(ViewModelBase viewModel, out ViewModelBase resolved)
        {
            if (viewModel == null)
                goto fail;


            resolved = viewModel;

            if (resolved is TabsViewModelBase tabsVM)
                resolved = tabsVM.Tabs[tabsVM.SelectedIndex];

            if (resolved is PageTabViewModel pageTabVM)
                resolved = pageTabVM.ContentVM;


            return resolved != null;

            fail:
            resolved = null;
            return false;
        }




        public static bool TryBuild(this IDataTemplate template, object data, out Control view)
        {
            if (template == null)
                goto fail;
            else if (!template.Match(data))
                goto fail;


            view = template.Build(data);
            if (view != null)
                return true;


            fail:
            view = null;
            return false;
        }
    }
}