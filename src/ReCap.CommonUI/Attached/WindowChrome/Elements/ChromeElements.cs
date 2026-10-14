using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls;
using ReCap.CommonUI.Controls.AppearanceHacks;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed class ChromeElements
    {
        public const string PSEUD_WINDOW_ACTIVE = ":window-active";
        public const string PSEUD_WINDOW_MANAGED_CHROME = ":window-managed-chrome";
        public const string PSEUD_WINDOW_RESERVE_CAPTION_AREA = ":window-reserve-caption-area";
        public const string PSEUD_WINDOW_ICON_VISIBLE = ":window-icon-visible";
        public const string PSEUD_WINDOW_TITLE_VISIBLE = ":window-title-visible";

        public const string PSEUD_WINDOW_MINIMIZED = ":window-minimized";
        public const string PSEUD_WINDOW_NORMAL = ":window-normal";
        public const string PSEUD_WINDOW_MAXIMIZED = ":window-maximized";
        public const string PSEUD_WINDOW_FULLSCREEN = ":window-fullscreen";

        public const string PSEUD_WINDOW_LEFT_ANY = ":window-leftbuttons-any";
        public const string PSEUD_WINDOW_LEFT_MULTIPLE_BUTTONS = ":window-leftbuttons-multiple";

        public const string PSEUD_WINDOW_LEFT_HAS_WINDOWMENU_BUTTON = ":window-leftbuttons-has-windowmenu";
        public const string PSEUD_WINDOW_LEFT_HAS_NON_WINDOWMENU_BUTTONS = ":window-leftbuttons-has-non-windowmenu";
        public const string PSEUD_WINDOW_LEFT_HAS_WINDOWMENU_AT_MERGEINDEX = ":window-leftbuttons-has-windowmenu-at-mergeindex";

        public const string PSEUD_WINDOW_LEFT_WINDOWMENUPRESENCE_IRRELEVANT = ":window-leftbuttons-windowmenu-irrelevant";
        public const string PSEUD_WINDOW_LEFT_WINDOWMENUPRESENCE_LONE = ":window-leftbuttons-windowmenu-lone";
        public const string PSEUD_WINDOW_LEFT_WINDOWMENUPRESENCE_INNERMOST = ":window-leftbuttons-windowmenu-innermost";


        public const string PSEUD_WINDOW_RIGHT_HAS_BUTTONS = ":window-rightbuttons-any";
        public const string PSEUD_WINDOW_RIGHT_MULTIPLE_BUTTONS = ":window-rightbuttons-multiple";

        public const string PSEUD_WINDOW_RIGHT_HAS_WINDOWMENU_BUTTON = ":window-rightbuttons-has-windowmenu";
        public const string PSEUD_WINDOW_RIGHT_HAS_NON_WINDOWMENU_BUTTONS = ":window-rightbuttons-has-non-windowmenu-buttons";
        public const string PSEUD_WINDOW_RIGHT_HAS_WINDOWMENU_AT_MERGEINDEX = ":window-rightbuttons-has-windowmenu-at-mergeindex";

        public const string PSEUD_WINDOW_RIGHT_WINDOWMENUPRESENCE_IRRELEVANT = ":window-rightbuttons-windowmenu-irrelevant";
        public const string PSEUD_WINDOW_RIGHT_WINDOWMENUPRESENCE_LONE = ":window-rightbuttons-windowmenu-lone";
        public const string PSEUD_WINDOW_RIGHT_WINDOWMENUPRESENCE_INNERMOST = ":window-rightbuttons-windowmenu-innermost";





        List<IPseudoClassesProvider> _elements = new();
        public IEnumerable<IPseudoClassesProvider> Elements
        {
            get => _elements;
        }


        WeakReference<WindowChrome> _chromeRef;
        public WindowChrome Chrome
        {
            get => WeakReferenceHelper.PropertyGet(ref _chromeRef);
            private set => WeakReferenceHelper.PropertySet(ref _chromeRef, value);
        }




        public void Refresh()
        {
            var pseudoClassValues = EvaluatePseudoClasses();
            _elements.AllSetAll(pseudoClassValues);

            if (Chrome.HostWindow.TryGetPseudoClasses(out IPseudoClasses pseudoClasses))
                pseudoClasses.SetAll(pseudoClassValues);

            Chrome.UpdateHack();
        }




        IEnumerable<KeyValuePair<string, bool>> EvaluatePseudoClasses()
        {
            var chrome = Chrome;
            if (chrome == null)
                return null;

            if (!EvaluateSideProperties(CaptionButtonClusterPosition.Left
                , out bool leftHasindowMenuButton
                , out bool leftHasNonWindowMenuButtons
                , out bool leftHasWindowMenuAtMergeIndex
                , out _
                , out bool leftHasButtons
                , out bool leftHasMultipleButtons
                , out WindowMenuPresence leftWindowMenuPresence
            ))
            {
                return null;
            }
            if (!EvaluateSideProperties(CaptionButtonClusterPosition.Right
                , out bool rightHasindowMenuButton
                , out bool rightHasNonWindowMenuButtons
                , out bool rightHasWindowMenuAtMergeIndex
                , out _
                , out bool rightHasButtons
                , out bool rightHasMultipleButtons
                , out WindowMenuPresence rightWindowMenuPresence
            ))
            {
                return null;
            }



            leftWindowMenuPresence.Expand(
                out bool leftWindowMenuIrrelevant
                , out bool leftWindowMenuLone
                , out bool leftWindowMenuInnermost
            );
            rightWindowMenuPresence.Expand(
                out bool rightWindowMenuIrrelevant
                , out bool rightWindowMenuLone
                , out bool rightWindowMenuInnermost
            );


            Window window = chrome.HostWindow;

            bool isHostWindowActive = window.IsActive;
            bool isHostWindowUsingManagedChrome = chrome.IsChromeManaged;
            bool isHostReserveCaptionArea = chrome.ReserveCaptionArea;
            bool isHostWindowIconVisible = chrome.IsIconVisible;
            bool isHostWindowTitleVisible = chrome.IsTitleVisible;
            WindowState hostWindowState = window.WindowState;

            bool isHostWindowMinimized = hostWindowState == WindowState.Minimized;
            bool isHostWindowNormal = hostWindowState == WindowState.Normal;
            bool isHostWindowMaximized = hostWindowState == WindowState.Maximized;
            bool isHostWindowFullScreen = hostWindowState == WindowState.FullScreen;


            return new Dictionary<string, bool>()
            {
                [PSEUD_WINDOW_ACTIVE] = isHostWindowActive,
                [PSEUD_WINDOW_MANAGED_CHROME] = isHostWindowUsingManagedChrome,
                [PSEUD_WINDOW_RESERVE_CAPTION_AREA] = isHostReserveCaptionArea,
                [PSEUD_WINDOW_ICON_VISIBLE] = isHostWindowIconVisible,
                [PSEUD_WINDOW_TITLE_VISIBLE] = isHostWindowTitleVisible,

                [PSEUD_WINDOW_MINIMIZED] = isHostWindowMinimized,
                [PSEUD_WINDOW_NORMAL] = isHostWindowNormal,
                [PSEUD_WINDOW_MAXIMIZED] = isHostWindowMaximized,
                [PSEUD_WINDOW_FULLSCREEN] = isHostWindowFullScreen,


                [PSEUD_WINDOW_LEFT_ANY] = leftHasButtons,
                [PSEUD_WINDOW_LEFT_MULTIPLE_BUTTONS] = leftHasMultipleButtons,

                [PSEUD_WINDOW_LEFT_HAS_WINDOWMENU_BUTTON] = leftHasindowMenuButton,
                [PSEUD_WINDOW_LEFT_HAS_NON_WINDOWMENU_BUTTONS] = leftHasNonWindowMenuButtons,
                [PSEUD_WINDOW_LEFT_HAS_WINDOWMENU_AT_MERGEINDEX] = leftHasWindowMenuAtMergeIndex,

                [PSEUD_WINDOW_LEFT_WINDOWMENUPRESENCE_IRRELEVANT] = leftWindowMenuIrrelevant,
                [PSEUD_WINDOW_LEFT_WINDOWMENUPRESENCE_LONE] = leftWindowMenuLone,
                [PSEUD_WINDOW_LEFT_WINDOWMENUPRESENCE_INNERMOST] = leftWindowMenuInnermost,


                [PSEUD_WINDOW_RIGHT_HAS_BUTTONS] = rightHasButtons,
                [PSEUD_WINDOW_RIGHT_MULTIPLE_BUTTONS] = rightHasMultipleButtons,

                [PSEUD_WINDOW_RIGHT_HAS_WINDOWMENU_BUTTON] = rightHasindowMenuButton,
                [PSEUD_WINDOW_RIGHT_HAS_NON_WINDOWMENU_BUTTONS] = rightHasNonWindowMenuButtons,
                [PSEUD_WINDOW_RIGHT_HAS_WINDOWMENU_AT_MERGEINDEX] = rightHasWindowMenuAtMergeIndex,

                [PSEUD_WINDOW_RIGHT_WINDOWMENUPRESENCE_IRRELEVANT] = rightWindowMenuIrrelevant,
                [PSEUD_WINDOW_RIGHT_WINDOWMENUPRESENCE_LONE] = rightWindowMenuLone,
                [PSEUD_WINDOW_RIGHT_WINDOWMENUPRESENCE_INNERMOST] = rightWindowMenuInnermost,
            };
        }




        public ChromeElements(WindowChrome chrome)
        {
            Chrome = chrome;
        }




        public void Attach(IPseudoClassesProvider element)
        {
            _elements.Add(element);
            element.SetAll(EvaluatePseudoClasses());
        }
        public void Detach(IPseudoClassesProvider element)
        {
            _elements.Remove(element);
        }




        bool EvaluateRoles(CaptionButtonClusterPosition position
            , out List<CaptionButtonRole> roles
            , out int count
            , out int mergeTargetIndex
        )
        {
            var chrome = Chrome;
            if (chrome == null)
                goto fail;
            else if (!position.Evaluate(out bool left, out bool right, throwIfInvalid: true))
                goto fail;
            else if (left && TryGetListAndCount(chrome.LeftCaptionButtonRoles, out roles, out count))
            {
                mergeTargetIndex = count - 1;
                return true;
            }
            else if (right && TryGetListAndCount(chrome.RightCaptionButtonRoles, out roles, out count))
            {
                mergeTargetIndex = 0;
                return true;
            }
            else
            {
                Debug.Assert(false, "This should never happen.");
            }


            fail:
            roles = null;
            count = 0;
            mergeTargetIndex = -1337;
            return false;
        }
        static bool TryGetListAndCount(IEnumerable<CaptionButtonRole> enumerable, out List<CaptionButtonRole> list, out int count)
        {
            if (enumerable != null)
            {
                list = new(enumerable);
                count = list.Count;
                return true;
            }
            else
            {
                list = null;
                count = default;
                return false;
            }
        }
        bool EvaluateSideProperties(CaptionButtonClusterPosition position
            , out bool hasWindowMenuButton
            , out bool hasNonWindowMenuButtons
            , out bool hasWindowMenuAtMergeIndex
            , out int buttonCount
            , out bool hasButtons
            , out bool hasMultipleButtons
            , out WindowMenuPresence windowMenuPresence
        )
        {
            if (!EvaluateRoles(position
                , out List<CaptionButtonRole> roles
                , out buttonCount
                , out int mergeTargetIndex
            ))
            {
                hasWindowMenuButton = default;
                hasNonWindowMenuButtons = default;
                hasWindowMenuAtMergeIndex = default;
                buttonCount = default;
                hasButtons = default;
                hasMultipleButtons = default;
                windowMenuPresence = default;
                return false;
            }

            hasButtons = buttonCount > 0;
            hasMultipleButtons = buttonCount > 1;

            hasWindowMenuButton = false;
            hasNonWindowMenuButtons = false;
            hasWindowMenuAtMergeIndex = false;
            //CaptionButtonRoles roles;

            for (int i = 0; i < buttonCount; i++)
            {
                var role = roles[i];

                if (role != CaptionButtonRole.WindowMenu)
                {
                    hasNonWindowMenuButtons = true;
                    continue;
                }

                hasWindowMenuButton = true;

                if (i == mergeTargetIndex)
                    hasWindowMenuAtMergeIndex = true;
            }


            windowMenuPresence = WindowMenuPresence.Irrelevant;

            if (hasWindowMenuAtMergeIndex && hasNonWindowMenuButtons)
                windowMenuPresence =  WindowMenuPresence.Innermost;
            else if ((buttonCount == 1) && !hasNonWindowMenuButtons)
                windowMenuPresence = WindowMenuPresence.Lone;
            /*
            else if (buttonCount <= 0)
                return;
            else if (!Chrome.IsIconVisible)
                return;
            */
            return true;
        }




        public void Dispose()
        {
            Chrome = null;

            if (_elements == null)
                return;

            int elementCount = _elements.Count;
            for (int i = 0; i < elementCount; i++)
            {
                Detach(_elements[0]);
            }
            _elements = null;
        }
    }
}