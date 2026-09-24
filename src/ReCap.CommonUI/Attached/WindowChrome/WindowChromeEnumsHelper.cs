using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Styling;
using ReCap.CommonUI.Controls.AppearanceHacks;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal static partial class WindowChromeEnumsHelper
    {
        const string _CAPTIONBUTTONROLE_TITLE_KEY_FORMAT = "CaptionButton.RoleTitle.{0}";
        public static readonly IEnumerable<CaptionButtonRole> ALL_ROLES;
        public static readonly IEnumerable<CaptionButtonRole> ACTIVATABLE_ROLES = new[]
        {
            CaptionButtonRole.Minimize,
            CaptionButtonRole.Maximize,
            CaptionButtonRole.FullScreen,
            CaptionButtonRole.WindowMenu, //HACK: Technically not activatable, but needed for ShowIconHint/IsIconVisible
#if CAPTIONBUTTONROLES_NYI
            CaptionButtonRole.ShowOnAllDesktops,
            CaptionButtonRole.Shade,
            CaptionButtonRole.KeepBelow,
#endif
            CaptionButtonRole.KeepAbove,
        };
        public static readonly IEnumerable<CaptionButtonRole> NON_ACTIVATABLE_ROLES;
        static readonly IReadOnlyDictionary<(CaptionButtonRole Role, bool RoleActive), DynamicResourceExtension> _ROLES_DYNAMIC_RESOURCES;
        delegate bool TryGetCaptionButtonRoleTitleFunc<T>(T resourceSource, ThemeVariant theme, string key, out object o);
        static WindowChromeEnumsHelper()
        {
            ALL_ROLES = Enum.GetValues(typeof(CaptionButtonRole)).Cast<CaptionButtonRole>();
            NON_ACTIVATABLE_ROLES = ALL_ROLES.Where(role => !ACTIVATABLE_ROLES.Contains(role));

            static void MakePair(CaptionButtonRole role, bool roleActive, out (CaptionButtonRole Role, bool RoleActive) keyPair, out DynamicResourceExtension dynamicResource)
            {
                keyPair = (role, roleActive);
                var key = role.GetCaptionButtonRoleTitleResourceKey(roleActive);
                dynamicResource = new(key);
            }



            Dictionary<(CaptionButtonRole Role, bool RoleActive), DynamicResourceExtension> rolesDynamicResources = new();

            foreach (CaptionButtonRole role in ACTIVATABLE_ROLES)
            {
                MakePair(role, false, out var fKeyPair, out DynamicResourceExtension fDynamicResource);
                rolesDynamicResources[fKeyPair] = fDynamicResource;

                MakePair(role, true, out var tKeyPair, out DynamicResourceExtension tDynamicResource);
                rolesDynamicResources[tKeyPair] = tDynamicResource;
            }

            foreach (CaptionButtonRole role in NON_ACTIVATABLE_ROLES)
            {
                MakePair(role, false, out var keyPair, out DynamicResourceExtension dynamicResource);
                rolesDynamicResources[keyPair] = dynamicResource;
                rolesDynamicResources[(role, true)] = dynamicResource;
            }

            _ROLES_DYNAMIC_RESOURCES = rolesDynamicResources;
        }




#region CaptionButton state binding
        public static bool BindState(this CaptionButton button, Window hostWindow)
        {
            if (!TryBindStateInternal(button, hostWindow, out CompositeDisposable bindingDisposables))
                return false;

            button.BindingDisposables = bindingDisposables;
            return true;
        }
        static bool TryBindStateInternal(CaptionButton button, Window hostWindow, out CompositeDisposable bindingDisposables)
        {
            if (button == null)
                goto fail;

            CaptionButtonRole role = button.Role;
            bool roleActive = button.IsRoleActive;

            bindingDisposables = button.BindingDisposables ?? new();
            bindingDisposables.Add(button.Bind(ContentControl.ContentProperty, button[!CaptionButton.RoleProperty]));

            IBinding isActiveBinding;
            IBinding isEnabledBinding;
            switch (role)
            {
                case CaptionButtonRole.Minimize:
                    role.GetStateBindingsForWindowState(hostWindow
                        , Window.CanMinimizeProperty
                        , out isActiveBinding
                        , out isEnabledBinding
                    );
                    break;

                case CaptionButtonRole.Maximize:
                case CaptionButtonRole.FullScreen:
                    role.GetStateBindingsForWindowState(hostWindow
                        , Window.CanMaximizeProperty
                        , out isActiveBinding
                        , out isEnabledBinding
                    );
                    break;

                case CaptionButtonRole.WindowMenu:
                    isActiveBinding = WindowChrome.GetStateInfo(hostWindow)
                        .GetObservable(WindowChrome.IsIconVisibleProperty)
                        .ToBinding()
                    ;
                    isEnabledBinding = null;
                    break;

#if CAPTIONBUTTONROLES_NYI
                case CaptionButtonRole.ShowOnAllDesktops:
                    isActiveBinding = //[TODO: ]
                    isEnabledBinding = //[TODO: ]
                    break;

                case CaptionButtonRole.Shade:
                    isActiveBinding = //[TODO: ]
                    isEnabledBinding = //[TODO: ]
                    break;

                case CaptionButtonRole.KeepBelow:
                    isActiveBinding = //[TODO: ]
                    isEnabledBinding = //[TODO: ]
                    break;

#endif
                case CaptionButtonRole.KeepAbove:
                    isActiveBinding = hostWindow
                        .GetObservable(WindowBase.TopmostProperty)
                        .ToBinding()
                    ;

                    isEnabledBinding = null;
                    break;

                default:
                    goto fail;
            }

            if (isActiveBinding != null)
            {
                button.IsRoleActivatable = true;
                bindingDisposables.Add(button.Bind(CaptionButton.IsRoleActiveProperty, isActiveBinding));
            }
            else
            {
                button.IsRoleActivatable = false;
            }


            if (isEnabledBinding != null)
                bindingDisposables.Add(button.Bind(InputElement.IsEnabledProperty, isEnabledBinding));

            return true;

            fail:
            button.IsRoleActivatable = false;
            bindingDisposables = null;
            return false;
        }


        static void GetStateBindingsForWindowState(this CaptionButtonRole role
            , Window hostWindow
            , AvaloniaProperty<bool> isEnabledTargetProperty
            , out IBinding isRoleActiveBinding
            , out IBinding isEnabledBinding
        )
        {
            WindowState windowState = (WindowState)role;
            isRoleActiveBinding = hostWindow
                .GetObservable(Window.WindowStateProperty)
                .Select(ws => ws == windowState)
                .ToBinding()
            ;

            isEnabledBinding = hostWindow
                .GetObservable(isEnabledTargetProperty)
                .ToBinding()
            ;
        }
#endregion




        public static string GetCaptionButtonRoleTitleResourceKey(this CaptionButtonRole role, bool roleActive)
            => TryGetCaptionButtonRoleTitleResourceKeyInternal(role, roleActive, out string key, out Exception exception)
                ? key
                : throw exception
            ;


        public static bool TryGetCaptionButtonRoleTitleResourceKey(this CaptionButtonRole role, bool roleActive, out string key)
            => TryGetCaptionButtonRoleTitleResourceKeyInternal(role, roleActive, out key, out _);
        static bool TryGetCaptionButtonRoleTitleResourceKeyInternal(CaptionButtonRole role, bool roleActive, out string key, out Exception exception)
        {
            if (!ALL_ROLES.Contains(role))
            {
                exception = new InvalidEnumArgumentException(nameof(role), (int)role, typeof(CaptionButtonRole));
                goto fail;
            }

            key = role.ToString();
            if (string.IsNullOrWhiteSpace(key))
            {
                exception = new NullReferenceException($"{nameof(key)} 1");
                goto fail;
            }
            else if (roleActive && ACTIVATABLE_ROLES.Contains(role))
            {
                key = $"Un{key}";
            }

            key = string.Format(_CAPTIONBUTTONROLE_TITLE_KEY_FORMAT, key);
            if (string.IsNullOrWhiteSpace(key))
            {
                exception = new NullReferenceException($"{nameof(key)} 2");
                goto fail;
            }
            exception = null;
            return true;

            fail:
            key = default;
            return false;
        }




        public static bool TryGetCaptionButtonRoleTitle(this CaptionButtonRole role, bool roleActive
            , IThemeVariantHost resourceSource
            , out string title
        )
            => role.TryGetCaptionButtonRoleTitleInternal(roleActive
                , TryGetCaptionButtonRoleTitleFunc_IResourceHost
                , resourceSource
                , resourceSource.ActualThemeVariant
                , out title
            );


        static bool TryGetCaptionButtonRoleTitleFunc_IResourceHost(IResourceHost resourceSource, ThemeVariant theme, string key, out object o)
        {
            if (resourceSource.TryGetResource(key, theme, out o))
                return true;
            else if (resourceSource.TryGetResource(key, out o))
                return true;

            o = null;
            return false;
        }
        public static bool TryGetCaptionButtonRoleTitle(this CaptionButtonRole role, bool roleActive
            , IResourceHost resourceSource
            , ThemeVariant theme
            , out string title
        )
            => role.TryGetCaptionButtonRoleTitleInternal(roleActive
                , TryGetCaptionButtonRoleTitleFunc_IResourceHost
                , resourceSource
                , theme
                , out title
            );


        static bool TryGetCaptionButtonRoleTitleFunc_IResourceNode(IResourceNode resourceSource, ThemeVariant theme, string key, out object o)
            => resourceSource.TryGetResource(key, theme, out o);
        public static bool TryGetCaptionButtonRoleTitle(this CaptionButtonRole role, bool roleActive
            , IResourceNode resourceSource
            , ThemeVariant theme
            , out string title
        )
            => role.TryGetCaptionButtonRoleTitleInternal(roleActive
                , TryGetCaptionButtonRoleTitleFunc_IResourceNode
                , resourceSource
                , theme
                , out title
            );


        static bool TryGetCaptionButtonRoleTitleInternal<T>(this CaptionButtonRole role, bool roleActive
            , TryGetCaptionButtonRoleTitleFunc<T> func
            , T resourceSource
            , ThemeVariant theme
            , out string title
        )
        {
            if (!role.TryGetCaptionButtonRoleTitleResourceKey(roleActive, out string key))
                goto fail;
            else if (!func(resourceSource, theme, key, out object o))
                goto fail;
            else if (o == null)
                goto fail;
            else if (o is not string s)
                goto fail;
            else if (string.IsNullOrWhiteSpace(s))
                goto fail;
            else
            {
                title = s;
                return true;
            }


            fail:
            title = null;
            return false;
        }




        public static bool TryGetCaptionButtonRoleTitleDynamicResource(this CaptionButtonRole role, bool roleActive, out DynamicResourceExtension dynamicResource)
            => (role, roleActive).TryGetCaptionButtonRoleTitleDynamicResource(out dynamicResource);
        public static bool TryGetCaptionButtonRoleTitleDynamicResource(this (CaptionButtonRole Role, bool RoleActive) keyPair, out DynamicResourceExtension dynamicResource)
            => _ROLES_DYNAMIC_RESOURCES.TryGetValue(keyPair, out dynamicResource);




        internal static bool ResolveVisibility(this ManagedChromeElementHint hint, bool auto)
            => hint switch
            {
                ManagedChromeElementHint.Hide => false,
                ManagedChromeElementHint.Show => true,
                _ => auto,
            };
    }
}