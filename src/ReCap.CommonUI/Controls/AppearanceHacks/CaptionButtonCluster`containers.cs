using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using ReCap.CommonUI.Attached.WindowChrome;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    partial class CaptionButtonCluster
    {
        protected override bool NeedsContainerOverride(object item, int index, out object recycleKey)
        {
            bool baseResult = base.NeedsContainerOverride(item, index, out _);
            recycleKey = null;
            return (item is not CaptionButton) && baseResult;
        }




        protected override Control CreateContainerForItemOverride(object item, int index, object recycleKey)
        {
            base.CreateContainerForItemOverride(item, index, recycleKey);
            return new CaptionButton();
        }




        protected override void PrepareContainerForItemOverride(Control container, object item, int index)
        {
            base.PrepareContainerForItemOverride(container, item, index);

            CaptionButton button = (CaptionButton)container;
            CaptionButtonRole role = (CaptionButtonRole)item;

            button.Role = role;
            button.BindState(_hostWindow);

            IBinding themeBinding;
            IBinding contentTemplateBinding;
            if (role == CaptionButtonRole.WindowMenu)
            {
                themeBinding = this[!WindowMenuItemContainerThemeProperty];
                contentTemplateBinding = this[!WindowMenuItemTemplateProperty];
            }
            else
            {
                themeBinding = this[!ItemContainerThemeProperty];
                contentTemplateBinding = this[!ItemTemplateProperty];
            }

            button.BindingDisposables ??= new();
            button.BindingDisposables.Add(button.Bind(ThemeProperty, themeBinding));
            button.BindingDisposables.Add(button.Bind(ContentControl.ContentTemplateProperty, contentTemplateBinding));
            ManagedWindowChrome.PLATFORM_IMPL.Prepare(button);
        }




        protected override void ClearContainerForItemOverride(Control container)
        {
            if (container is CaptionButton button)
            {
                button.DisposeBindingDisposables();

                button.ClearValue(CaptionButton.IsRoleCheckableProperty);
                button.ClearValue(CaptionButton.IsRoleCheckedProperty);
                button.ClearValue(IsEnabledProperty);
                button.ClearValue(CaptionButton.RoleProperty);
                button.ClearValue(ContentControl.ContentProperty);
                button.ClearValue(ContentControl.ContentTemplateProperty);
                button.ClearValue(ThemeProperty);
                button.ClearValue(CaptionButton.UseManagedToolTipProperty);
            }

            base.ClearContainerForItemOverride(container);
        }
    }
}