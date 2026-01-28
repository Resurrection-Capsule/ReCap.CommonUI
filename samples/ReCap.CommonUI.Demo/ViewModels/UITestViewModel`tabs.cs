using System;
using System.Collections.Generic;
using ReCap.CommonUI.Demo.Reflection;
using ReCap.CommonUI.Demo.ViewModels.Pages;

namespace ReCap.CommonUI.Demo.ViewModels
{
    public partial class UITestViewModel
        : TabsViewModelBase
    {
        static readonly TypeFilterOptions _VIEWMODEL_FILTER = new()
        {
            BaseType = typeof(ViewModelBase)
        };


        const string _NS_PREFIX = nameof(ReCap) + "." + nameof(CommonUI) + "." + nameof(Demo) + "." + nameof(ViewModels) + "." + nameof(Pages);
        static IEnumerable<PageTabViewModel> GetFromNamespace(string ns)
        {
            List<PageTabViewModel> tabVMs = new();
            IEnumerable<Type> types = TypeHelper.GetTypesInNamespace(Assemblies.DEMO_ASSEMBLY, ns, _VIEWMODEL_FILTER);

            foreach (var type in types)
            {
                if (!TypeHelper.TryCreateInstanceAs(type, out ViewModelBase vm))
                {
                    Console.WriteLine($"!{nameof(TypeHelper)}.{nameof(TypeHelper.TryCreateInstanceAs)}<{type.Name}>({nameof(type)}, out {nameof(vm)})");
                    continue;
                }

                string title = type.Name.Replace("ViewModel", string.Empty);
                PageTabViewModel tabVM = new(title, vm);

                tabVMs.Add(tabVM);
            }

            return tabVMs;
        }




        public UITestViewModel()
            : base()
        {
            AddTabs(new PageTabViewModel[]
            {
                new("Standard control styles", new UITestSubViewModel(
                    GetFromNamespace($"{_NS_PREFIX}.Styles")
                )),
                new("New controls", new UITestSubViewModel(
                    GetFromNamespace($"{_NS_PREFIX}.Controls")
                )),
            });
        }
    }
}