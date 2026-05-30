using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    [PseudoClasses(_PSEUD_NONE, _PSEUD_TRANSPARENT, _PSEUD_BLUR, _PSEUD_ACRYLIC_BLUR, _PSEUD_MICA, _PSEUD_ANY_BLUR)]
    public sealed class DetectRootTransparencyControl
        : Decorator
    {
        /// <summary>
        /// Defines the <see cref="TransparencyLevelHint"/> property.
        /// </summary>
        public static readonly DirectProperty<DetectRootTransparencyControl, IReadOnlyList<WindowTransparencyLevel>> TransparencyLevelHintProperty
            = AvaloniaProperty.RegisterDirect<DetectRootTransparencyControl, IReadOnlyList<WindowTransparencyLevel>>(nameof(TransparencyLevelHint)
                , o => o.TransparencyLevelHint
                , (o, v) => o.TransparencyLevelHint = v
                , unsetValue: Array.Empty<WindowTransparencyLevel>()
            );
        IReadOnlyList<WindowTransparencyLevel> _transparencyLevelHint = Array.Empty<WindowTransparencyLevel>();
        /// <summary>
        /// Gets the <see cref="WindowTransparencyLevel"/> that the containing <see cref="TopLevel"/> should use when possible.
        /// </summary>
        /// <remarks>
        /// <seealso cref="TopLevel.TransparencyLevelHint"/>
        /// </remarks>
        public IReadOnlyList<WindowTransparencyLevel> TransparencyLevelHint
        {
            get => _transparencyLevelHint;
            private set => SetAndRaise(TransparencyLevelHintProperty, ref _transparencyLevelHint, value);
        }


        /// <summary>
        /// Defines the <see cref="ActualTransparencyLevel"/> property.
        /// </summary>
        public static readonly DirectProperty<DetectRootTransparencyControl, WindowTransparencyLevel> ActualTransparencyLevelProperty
            = TopLevel.ActualTransparencyLevelProperty.AddOwner<DetectRootTransparencyControl>(
                o => o.ActualTransparencyLevel
                , (o, v) => o.ActualTransparencyLevel = v
                , unsetValue: WindowTransparencyLevel.None
            );
        WindowTransparencyLevel _actualTransparencyLevel = WindowTransparencyLevel.None;
        /// <summary>
        /// Gets the achieved <see cref="WindowTransparencyLevel"/> that the platform was able to provide to the containing <see cref="TopLevel"/>.
        /// </summary>
        /// <remarks>
        /// <seealso cref="TopLevel.ActualTransparencyLevel"/>
        /// </remarks>
        public WindowTransparencyLevel ActualTransparencyLevel
        {
            get => _actualTransparencyLevel;
            private set => SetAndRaise(ActualTransparencyLevelProperty, ref _actualTransparencyLevel, value);
        }




        IDisposable _transparencyLevelDisposable = null;
        static DetectRootTransparencyControl()
        {
            ActualTransparencyLevelProperty.Changed.AddClassHandler<DetectRootTransparencyControl>(ActualTransparencyLevelProperty_Changed);
        }


        static void ActualTransparencyLevelProperty_Changed(DetectRootTransparencyControl sender, AvaloniaPropertyChangedEventArgs e)
            => sender.UpdatePseudoClasses(e.GetNewValue<WindowTransparencyLevel>());




        const string _PSEUD_NONE = ":opaque";
        const string _PSEUD_TRANSPARENT = ":transparent";
        const string _PSEUD_BLUR = ":blur";
        const string _PSEUD_ACRYLIC_BLUR = ":acrylic-blur";
        const string _PSEUD_MICA = ":mica";
        const string _PSEUD_ANY_BLUR = ":any-blur";
        static readonly ReadOnlyDictionary<WindowTransparencyLevel, string> _TRANSPARENCY_LEVEL_PSEUDS = new(new Dictionary<WindowTransparencyLevel, string>()
        {
            [WindowTransparencyLevel.None] = _PSEUD_NONE,
            [WindowTransparencyLevel.Transparent] = _PSEUD_TRANSPARENT,
            [WindowTransparencyLevel.Blur] = _PSEUD_BLUR,
            [WindowTransparencyLevel.AcrylicBlur] = _PSEUD_ACRYLIC_BLUR,
            [WindowTransparencyLevel.Mica] = _PSEUD_MICA,
        });
        static readonly IEnumerable<WindowTransparencyLevel> _BLUR_TRANSPARENCY_LEVELS = new[]
        {
            WindowTransparencyLevel.Blur,
            WindowTransparencyLevel.AcrylicBlur,
            WindowTransparencyLevel.Mica,
        };
        void UpdatePseudoClasses(WindowTransparencyLevel newTransparencyLevel)
        {
            bool anyBlur = false;
            foreach (var pair in _TRANSPARENCY_LEVEL_PSEUDS)
            {
                WindowTransparencyLevel transparencyLevel = pair.Key;
                PseudoClasses.Set(pair.Value, newTransparencyLevel == transparencyLevel);
                if (!anyBlur)
                    anyBlur = _BLUR_TRANSPARENCY_LEVELS.Contains(transparencyLevel);
            }


            PseudoClasses.Set(_PSEUD_ANY_BLUR, anyBlur);
        }




        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            PopupRoot popupRoot = this.FindAncestorOfType<PopupRoot>(false);
            if (popupRoot == null)
                return;

            Detach();
            Attach(popupRoot);
        }


        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnDetachedFromVisualTree(e);
            Detach();
        }




        void Attach(PopupRoot popupRoot)
        {
            _transparencyLevelDisposable = new CompositeDisposable()
            {
                Bind(TransparencyLevelHintProperty, popupRoot[!TopLevel.TransparencyLevelHintProperty]),
                Bind(ActualTransparencyLevelProperty, popupRoot[!TopLevel.ActualTransparencyLevelProperty]),
            };
            Dispatcher.UIThread.Post(() => UpdatePseudoClasses(ActualTransparencyLevel));
        }


        void Detach()
        {
            _transparencyLevelDisposable?.Dispose();
            _transparencyLevelDisposable = null;
        }
    }
}