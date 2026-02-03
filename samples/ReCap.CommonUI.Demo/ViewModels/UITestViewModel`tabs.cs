using System;
using ReCap.CommonUI.Demo.ViewModels.Pages;

#if AUTO_PAGE_TABS
using System.Collections.Generic;

#if REFLECTION_HELPERS
using ReCap.CommonUI.Demo.Reflection;
#endif

#else
using ReCap.CommonUI.Demo.ViewModels.Pages.Controls;
using ReCap.CommonUI.Demo.ViewModels.Pages.Styles;
#endif

namespace ReCap.CommonUI.Demo.ViewModels
{
    public partial class UITestViewModel
        : TabsViewModelBase
    {
        public UITestViewModel()
            : base()
        {
            AddTabs(new PageTabViewModel[]
            {
                new("Standard control styles", new UITestSubViewModel(
#if AUTO_PAGE_TABS
                    GetFromNamespace($"{_NS_PREFIX}.Styles")
#else
                    new PageTabViewModel[]
                    {
                        new("Button", new ButtonViewModel()),
                        new("ComboBox", new ComboBoxViewModel()),
                        new("ListBox", new ListBoxViewModel()),
                        new("ScrollViewer", new ScrollViewerViewModel()),
                        new("TabControl", new TabControlViewModel()),
                        new("TextBox", new TextBoxViewModel()),
                        new("WindowChrome", new WindowChromeViewModel()),
                    }
#endif
                )),
                new("New controls", new UITestSubViewModel(
#if AUTO_PAGE_TABS
                    GetFromNamespace($"{_NS_PREFIX}.Controls")
#else
                    new PageTabViewModel[]
                    {
                        new("AlignableWrapPanel", new AlignableWrapPanelViewModel()),
                        new("AngledBorders", new AngledBordersViewModel()),
                        new("Closeable", new CloseableViewModel()),
                    }
#endif
                )),
            });
        }




#if AUTO_PAGE_TABS
        const string _NS_PREFIX = nameof(ReCap) + "." + nameof(CommonUI) + "." + nameof(Demo) + "." + nameof(ViewModels) + "." + nameof(Pages);


#if REFLECTION_HELPERS
        static readonly TypeFilterOptions _VIEWMODEL_FILTER = new()
        {
            BaseType = typeof(ViewModelBase)
        };
#endif


        static IEnumerable<PageTabViewModel> GetFromNamespace(string ns)
        {
            List<PageTabViewModel> tabVMs = new();
#if REFLECTION_HELPERS
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
#else
            // [TODO: ]
            tabVMs.Add(new PageTabViewModel($"TEST ({ns})", null));
#endif
            return tabVMs;
        }
#endif
    }
}