using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using ReCap.CommonUI.Controls.AppearanceHacks;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    partial class WindowChrome
    {
        internal const string PSEUD_MINIMIZED = ":minimized";
        internal const string PSEUD_NORMAL = ":normal";
        internal const string PSEUD_MAXIMIZED = ":maximized";
        internal const string PSEUD_FULLSCREEN = ":fullscreen";




        /// <summary>
        /// Whether a <see cref="Window"/> is currently using managed chrome.
        /// </summary>
        const bool _DEFAULT_IsChromeManaged = false;
        public static readonly DirectProperty<WindowChrome, bool> IsChromeManagedProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, bool>(nameof(IsChromeManaged)
                , getter: o => o.IsChromeManaged
                , unsetValue: _DEFAULT_IsChromeManaged
            );
        bool _isChromeManaged = _DEFAULT_IsChromeManaged;
        /// <summary>
        /// Whether a <see cref="Window"/> is currently using managed chrome.
        /// </summary>
        public bool IsChromeManaged
        {
            get => _isChromeManaged;
            internal set => SetAndRaise(IsChromeManagedProperty, ref _isChromeManaged, value);
        }


        /// <summary>
        /// Whether a <see cref="Window"/> is currently using managed chrome.
        /// </summary>
        const bool _DEFAULT_IsTitleVisible = true;
        public static readonly DirectProperty<WindowChrome, bool> IsTitleVisibleProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, bool>(nameof(IsTitleVisible)
                , getter: o => o.IsTitleVisible
                , unsetValue: _DEFAULT_IsTitleVisible
            );
        bool _isTitleVisible = _DEFAULT_IsTitleVisible;
        /// <summary>
        /// Whether a <see cref="Window"/> using managed chrome is showing its <see cref="Window.Title"/>.
        /// </summary>
        public bool IsTitleVisible
        {
            get => _isTitleVisible;
            internal set => SetAndRaise(IsTitleVisibleProperty, ref _isTitleVisible, value);
        }


        /// <summary>
        /// Whether a <see cref="Window"/> is currently using managed chrome.
        /// </summary>
        const bool _DEFAULT_IsIconVisible = true;
        public static readonly DirectProperty<WindowChrome, bool> IsIconVisibleProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, bool>(nameof(IsIconVisible)
                , getter: o => o.IsIconVisible
                , unsetValue: _DEFAULT_IsIconVisible
            );
        bool _isIconVisible = _DEFAULT_IsIconVisible;
        /// <summary>
        /// Whether a <see cref="Window"/> using managed chrome is showing its <see cref="Window.Icon"/>.
        /// </summary>
        public bool IsIconVisible
        {
            get => _isIconVisible;
            internal set => SetAndRaise(IsIconVisibleProperty, ref _isIconVisible, value);
        }




        /// <summary>
        /// Bridged width of a <see cref="Window"/>'s managed chrome's left caption buttons.
        /// </summary>
        const double _DEFAULT_LeftCaptionButtonsWidth = 0d;
        public static readonly DirectProperty<WindowChrome, double> LeftCaptionButtonsWidthProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, double>(nameof(LeftCaptionButtonsWidth)
                , getter: o => o.LeftCaptionButtonsWidth
                , unsetValue: _DEFAULT_LeftCaptionButtonsWidth
            );
        double _leftCaptionButtonsWidth = _DEFAULT_LeftCaptionButtonsWidth;
        /// <summary>
        /// Whether a <see cref="Window"/> using managed chrome is showing its <see cref="Window.Icon"/>.
        /// </summary>
        public double LeftCaptionButtonsWidth
        {
            get => _leftCaptionButtonsWidth;
            internal set => SetAndRaise(LeftCaptionButtonsWidthProperty, ref _leftCaptionButtonsWidth, value);
        }


        /// <summary>
        /// Bridged width of a <see cref="Window"/>'s managed chrome's right caption buttons.
        /// </summary>
        const double _DEFAULT_RightCaptionButtonsWidth = 0d;
        public static readonly DirectProperty<WindowChrome, double> RightCaptionButtonsWidthProperty
            = AvaloniaProperty.RegisterDirect<WindowChrome, double>(nameof(RightCaptionButtonsWidth)
                , getter: o => o.RightCaptionButtonsWidth
                , unsetValue: _DEFAULT_RightCaptionButtonsWidth
            );
        double _rightCaptionButtonsWidth = _DEFAULT_RightCaptionButtonsWidth;
        /// <summary>
        /// Whether a <see cref="Window"/> using managed chrome is showing its <see cref="Window.Icon"/>.
        /// </summary>
        public double RightCaptionButtonsWidth
        {
            get => _rightCaptionButtonsWidth;
            internal set => SetAndRaise(RightCaptionButtonsWidthProperty, ref _rightCaptionButtonsWidth, value);
        }




        static void StateInit()
        {
            IsChromeManagedProperty.Changed.AddClassHandler<WindowChrome>((s, e) => s.OnPropertyWithClassChanged(e, "managedchrome"));
            IsTitleVisibleProperty.Changed.AddClassHandler<WindowChrome>((s, e) => s.OnPropertyWithClassChanged(e, "titlevisible"));
            IsIconVisibleProperty.Changed.AddClassHandler<WindowChrome>((s, e) => s.OnPropertyWithClassChanged(e, "iconvisible"));
        }




        void OnPropertyWithClassChanged(AvaloniaPropertyChangedEventArgs args, string className)
        {
            RefreshElements();
            PropertySetClass(HostWindow, className, args.GetNewValue<bool>());
        }
        static void PropertySetClass(Window window, string className, bool value)
        {
#if PSEUDOCLASS_HACK
            if (window.TryGetPseudoClasses(out IPseudoClasses pseudoClasses))
                pseudoClasses.Set($":{className}", value);
#else
            window.Classes.Set(className, value);
#endif
        }




        internal void RefreshShowIcon()
        {
            RefreshShowIcon(GetShowIconHint(HostWindow));
        }
        internal void RefreshShowIcon(ManagedChromeElementHint hint)
            => IsIconVisible = hint.ResolveVisibility(PLATFORM_IMPL.DefaultShowCaptionIcon);


        internal void RefreshShowTitle()
        {
            RefreshShowTitle(GetShowTitleHint(HostWindow));
        }
        internal void RefreshShowTitle(ManagedChromeElementHint hint)
            => IsTitleVisible = hint.ResolveVisibility(PLATFORM_IMPL.DefaultShowCaptionText);

        internal void RefreshShowIconAndTitle()
        {
            RefreshShowIcon();
            RefreshShowTitle();
        }


        public void Dispose()
        {
            if (_disposables != null)
            {
                _disposables.Dispose();
                _disposables = null;
            }

            if (_elements != null)
            {
                int elementCount = _elements.Count;
                for (int i = 0; i < elementCount; i++)
                {
                    DetachElement(_elements[0]);
                }
                _elements = null;
            }

            HostWindow = null;
        }



        List<IChromeElement> _elements = new();
        internal void AttachElement(IChromeElement element)
        {
            _elements.Add(element);
            RefreshElements();
        }
        internal void DetachElement(IChromeElement element)
        {
            _elements.Remove(element);
            element.IsHostWindowActive = true;
            element.HostWindowState = WindowState.Normal;
        }


        void RefreshElements()
        {
            EvaluateSideProperties(CaptionButtonClusterPosition.Left
                , out bool leftHasNonWindowMenuButtons
                , out bool leftHasWindowMenuAtMergeIndex
                , out int leftButtonCount
                , out WindowMenuPresence leftWindowMenuPresence
            );
            EvaluateSideProperties(CaptionButtonClusterPosition.Right
                , out bool rightHasNonWindowMenuButtons
                , out bool rightHasWindowMenuAtMergeIndex
                , out int rightButtonCount
                , out WindowMenuPresence rightWindowMenuPresence
            );


            Window window = HostWindow;

            bool isHostWindowActive = window.IsActive;
            WindowState hostWindowState = window.WindowState;

            bool isHostWindowMinimized = hostWindowState == WindowState.Minimized;
            bool isHostWindowNormal = hostWindowState == WindowState.Normal;
            bool isHostWindowMaximized = hostWindowState == WindowState.Maximized;
            bool isHostWindowFullScreen = hostWindowState == WindowState.FullScreen;

            foreach (var element in _elements)
            {
                /*
                element.Update(isHostWindowActive, hostWindowState
                    , leftHasNonWindowMenuButtons, leftHasWindowMenuAtMergeIndex, leftButtonCount, leftWindowMenuPresence
                    , rightHasNonWindowMenuButtons, rightHasWindowMenuAtMergeIndex, rightButtonCount, rightWindowMenuPresence
                );
                */
                element.IsHostWindowActive = isHostWindowActive;
                element.HostWindowState = hostWindowState;

                var pseudoClasses = element.PseudoClasses;
                pseudoClasses.Set(PSEUD_MINIMIZED, isHostWindowMinimized);
                pseudoClasses.Set(PSEUD_NORMAL, isHostWindowNormal);
                pseudoClasses.Set(PSEUD_MAXIMIZED, isHostWindowMaximized);
                pseudoClasses.Set(PSEUD_FULLSCREEN, isHostWindowFullScreen);

                element.LeftHasNonWindowMenuButtons = leftHasNonWindowMenuButtons;
                element.LeftHasWindowMenuAtMergeIndex = leftHasWindowMenuAtMergeIndex;
                element.LeftButtonCount = leftButtonCount;
                element.LeftWindowMenuPresence = leftWindowMenuPresence;

                element.RightHasNonWindowMenuButtons = rightHasNonWindowMenuButtons;
                element.RightHasWindowMenuAtMergeIndex = rightHasWindowMenuAtMergeIndex;
                element.RightButtonCount = rightButtonCount;
                element.RightWindowMenuPresence = rightWindowMenuPresence;
            }
        }


        void EvaluateRoles(bool left
            , out List<CaptionButtonRole> roles
            , out int count
            , out int mergeTargetIndex
        )
        {
            roles = new(left
                ? LeftCaptionButtonRoles
                : RightCaptionButtonRoles
            );
            count = roles.Count();
            mergeTargetIndex = left
                ? count - 1
                : 0
            ;
        }
        void EvaluateSideProperties(CaptionButtonClusterPosition position
            , out bool hasNonWindowMenuButtons
            , out bool hasWindowMenuAtMergeIndex
            , out int buttonCount
            , out WindowMenuPresence windowMenuPresence
        )
        {
            Window window = HostWindow;
            EvaluateRoles(position.IsLeft()
                , out List<CaptionButtonRole> roles
                , out buttonCount
                , out int mergeTargetIndex
            );

            hasNonWindowMenuButtons = false;
            hasWindowMenuAtMergeIndex = false;
            //CaptionButtonRoles roles;

            for (int i = 0; i < buttonCount; i++)
            {
                var role = roles[i];

                if (role != CaptionButtonRole.WindowMenu)
                    hasNonWindowMenuButtons = true;
                else if (i == mergeTargetIndex)
                    hasWindowMenuAtMergeIndex = true;
            }


            windowMenuPresence = WindowMenuPresence.Irrelevant;

            if (buttonCount <= 0)
                return;
            else if (!IsIconVisible)
                return;
            else if (!hasNonWindowMenuButtons)
                windowMenuPresence = WindowMenuPresence.Lone;
            else if (hasWindowMenuAtMergeIndex && hasNonWindowMenuButtons)
                windowMenuPresence =  WindowMenuPresence.Innermost;
        }
    }
}