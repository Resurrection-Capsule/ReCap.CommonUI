using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using ReCap.CommonUI.Attached.WindowChrome;

namespace ReCap.CommonUI.Controls.AppearanceHacks
{
    public sealed class CaptionButtonGlyphs
        : AvaloniaObject
    {
        public static readonly StyledProperty<Geometry> MinimizeGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(MinimizeGlyph), null);
        public Geometry MinimizeGlyph
        {
            get => GetValue(MinimizeGlyphProperty);
            set => SetValue(MinimizeGlyphProperty, value);
        }


        public static readonly StyledProperty<Geometry> UnMinimizeGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(UnMinimizeGlyph), null);
        public Geometry UnMinimizeGlyph
        {
            get => GetValue(UnMinimizeGlyphProperty);
            set => SetValue(UnMinimizeGlyphProperty, value);
        }


        public static readonly StyledProperty<Geometry> MaximizeGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(MaximizeGlyph), null);
        public Geometry MaximizeGlyph
        {
            get => GetValue(MaximizeGlyphProperty);
            set => SetValue(MaximizeGlyphProperty, value);
        }


        public static readonly StyledProperty<Geometry> UnMaximizeGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(UnMaximizeGlyph), null);
        public Geometry UnMaximizeGlyph
        {
            get => GetValue(UnMaximizeGlyphProperty);
            set => SetValue(UnMaximizeGlyphProperty, value);
        }


        public static readonly StyledProperty<Geometry> FullScreenGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(FullScreenGlyph), null);
        public Geometry FullScreenGlyph
        {
            get => GetValue(FullScreenGlyphProperty);
            set => SetValue(FullScreenGlyphProperty, value);
        }


        public static readonly StyledProperty<Geometry> UnFullScreenGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(UnFullScreenGlyph), null);
        public Geometry UnFullScreenGlyph
        {
            get => GetValue(UnFullScreenGlyphProperty);
            set => SetValue(UnFullScreenGlyphProperty, value);
        }


        public static readonly StyledProperty<Geometry> CloseGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(CloseGlyph), null);
        public Geometry CloseGlyph
        {
            get => GetValue(CloseGlyphProperty);
            set => SetValue(CloseGlyphProperty, value);
        }


        public static readonly StyledProperty<Geometry> WindowMenuGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(WindowMenuGlyph), null);
        public Geometry WindowMenuGlyph
        {
            get => GetValue(WindowMenuGlyphProperty);
            set => SetValue(WindowMenuGlyphProperty, value);
        }


#if CAPTIONBUTTONROLES_NYI
        public static readonly StyledProperty<Geometry> ApplicationMenuGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(ApplicationMenuGlyph), null);
        public Geometry ApplicationMenuGlyph
        {
            get => GetValue(ApplicationMenuGlyphProperty);
            set => SetValue(ApplicationMenuGlyphProperty, value);
        }


        public static readonly StyledProperty<Geometry> ShowOnAllDesktopsGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(ShowOnAllDesktopsGlyph), null);
        public Geometry ShowOnAllDesktopsGlyph
        {
            get => GetValue(ShowOnAllDesktopsGlyphProperty);
            set => SetValue(ShowOnAllDesktopsGlyphProperty, value);
        }


        public static readonly StyledProperty<Geometry> UnShowOnAllDesktopsGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(UnShowOnAllDesktopsGlyph), null);
        public Geometry UnShowOnAllDesktopsGlyph
        {
            get => GetValue(UnShowOnAllDesktopsGlyphProperty);
            set => SetValue(UnShowOnAllDesktopsGlyphProperty, value);
        }


        public static readonly StyledProperty<Geometry> ContextHelpGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(ContextHelpGlyph), null);
        public Geometry ContextHelpGlyph
        {
            get => GetValue(ContextHelpGlyphProperty);
            set => SetValue(ContextHelpGlyphProperty, value);
        }


        public static readonly StyledProperty<Geometry> ShadeGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(ShadeGlyph), null);
        public Geometry ShadeGlyph
        {
            get => GetValue(ShadeGlyphProperty);
            set => SetValue(ShadeGlyphProperty, value);
        }


        public static readonly StyledProperty<Geometry> UnShadeGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(UnShadeGlyph), null);
        public Geometry UnShadeGlyph
        {
            get => GetValue(UnShadeGlyphProperty);
            set => SetValue(UnShadeGlyphProperty, value);
        }


        public static readonly StyledProperty<Geometry> KeepBelowGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(KeepBelowGlyph), null);
        public Geometry KeepBelowGlyph
        {
            get => GetValue(KeepBelowGlyphProperty);
            set => SetValue(KeepBelowGlyphProperty, value);
        }


        public static readonly StyledProperty<Geometry> UnKeepBelowGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(UnKeepBelowGlyph), null);
        public Geometry UnKeepBelowGlyph
        {
            get => GetValue(UnKeepBelowGlyphProperty);
            set => SetValue(UnKeepBelowGlyphProperty, value);
        }
#endif


        public static readonly StyledProperty<Geometry> KeepAboveGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(KeepAboveGlyph), null);
        public Geometry KeepAboveGlyph
        {
            get => GetValue(KeepAboveGlyphProperty);
            set => SetValue(KeepAboveGlyphProperty, value);
        }


        public static readonly StyledProperty<Geometry> UnKeepAboveGlyphProperty =
            AvaloniaProperty.Register<CaptionButton, Geometry>(nameof(UnKeepAboveGlyph), null);
        public Geometry UnKeepAboveGlyph
        {
            get => GetValue(UnKeepAboveGlyphProperty);
            set => SetValue(UnKeepAboveGlyphProperty, value);
        }




        static readonly IEnumerable<AvaloniaProperty<Geometry>> _GLYPH_PROPERTIES = new[]
        {
            MinimizeGlyphProperty,
            MaximizeGlyphProperty,
            FullScreenGlyphProperty,
            CloseGlyphProperty,
            WindowMenuGlyphProperty,
#if CAPTIONBUTTONROLES_NYI
            ApplicationMenuGlyphProperty,
            ShowOnAllDesktopsGlyphProperty,
            ContextHelpGlyphProperty,
            ShadeGlyphProperty,
            KeepBelowGlyphProperty,
#endif
            KeepAboveGlyphProperty,
        };
        static CaptionButtonGlyphs()
        {
            foreach (var property in _GLYPH_PROPERTIES)
            {
                property.Changed.AddClassHandler<CaptionButtonGlyphs>(GlyphProperty_Changed);
            }
        }


        static void GlyphProperty_Changed(CaptionButtonGlyphs glyphs, AvaloniaPropertyChangedEventArgs args)
        {
            var property = (AvaloniaProperty<Geometry>)args.Property;
            GlyphGeometryPropertyChangeEventArgs e = new(property, GetRole(property));
            glyphs.AnyGlyphGeometryPropertyChanged?.Invoke(glyphs, e);
        }




        static CaptionButtonRole GetRole(AvaloniaProperty<Geometry> property)
        {
            //Can't use switch since AvaloniaProperty declarations aren't const. Still gross though...

            if (property == MinimizeGlyphProperty)
                return CaptionButtonRole.Minimize;
            else if (property == UnMinimizeGlyphProperty)
                return CaptionButtonRole.Minimize;
            else if (property == MaximizeGlyphProperty)
                return CaptionButtonRole.Maximize;
            else if (property == UnMaximizeGlyphProperty)
                return CaptionButtonRole.Maximize;
            else if (property == UnFullScreenGlyphProperty)
                return CaptionButtonRole.FullScreen;
            else if (property == FullScreenGlyphProperty)
                return CaptionButtonRole.FullScreen;
            else if (property == CloseGlyphProperty)
                return CaptionButtonRole.Close;
            else if (property == WindowMenuGlyphProperty)
                return CaptionButtonRole.WindowMenu;
#if CAPTIONBUTTONROLES_NYI
            else if (property == ApplicationMenuGlyphProperty)
                return CaptionButtonRole.ApplicationMenu;
            else if (property == ShowOnAllDesktopsGlyphProperty)
                return CaptionButtonRole.ShowOnAllDesktops;
            else if (property == UnShowOnAllDesktopsGlyphProperty)
                return CaptionButtonRole.ShowOnAllDesktops;
            else if (property == ContextHelpGlyphProperty)
                return CaptionButtonRole.ContextHelp;
            else if (property == ShadeGlyphProperty)
                return CaptionButtonRole.Shade;
            else if (property == UnShadeGlyphProperty)
                return CaptionButtonRole.Shade;
            else if (property == KeepBelowGlyphProperty)
                return CaptionButtonRole.KeepBelow;
            else if (property == UnKeepBelowGlyphProperty)
                return CaptionButtonRole.KeepBelow;
#endif
            else if (property == KeepAboveGlyphProperty)
                return CaptionButtonRole.KeepAbove;
            else if (property == UnKeepAboveGlyphProperty)
                return CaptionButtonRole.KeepAbove;
            else
                return CaptionButton.DEFAULT_Role;
        }


        internal Geometry GetGlyph(CaptionButtonRole role, bool roleChecked)
            => role switch
            {
                CaptionButtonRole.Minimize => roleChecked
                    ? UnMinimizeGlyph
                    : MinimizeGlyph
                ,
                CaptionButtonRole.Maximize => roleChecked
                    ? UnMaximizeGlyph
                    : MaximizeGlyph
                ,
                CaptionButtonRole.FullScreen => roleChecked
                    ? UnFullScreenGlyph
                    : FullScreenGlyph
                ,
                CaptionButtonRole.Close => CloseGlyph,
                CaptionButtonRole.WindowMenu => WindowMenuGlyph,
#if CAPTIONBUTTONROLES_NYI
                CaptionButtonRole.ApplicationMenu => ApplicationMenuGlyph,
                CaptionButtonRole.ShowOnAllDesktops => roleChecked
                    ? UnShowOnAllDesktopsGlyph
                    : ShowOnAllDesktopsGlyph
                ,
                CaptionButtonRole.ContextHelp => ContextHelpGlyph,
                CaptionButtonRole.Shade => roleChecked
                    ? UnShadeGlyph
                    : ShadeGlyph
                ,
                CaptionButtonRole.KeepBelow => roleChecked
                    ? UnKeepBelowGlyph
                    : KeepBelowGlyph
                ,
#endif
                CaptionButtonRole.KeepAbove => roleChecked
                    ? UnKeepAboveGlyph
                    : KeepAboveGlyph
                ,
                _ => null,
            };




        internal event EventHandler<GlyphGeometryPropertyChangeEventArgs> AnyGlyphGeometryPropertyChanged;
    }




    internal sealed class GlyphGeometryPropertyChangeEventArgs
    {
        public readonly AvaloniaProperty<Geometry> Property;
        public readonly CaptionButtonRole Role;
        public GlyphGeometryPropertyChangeEventArgs(AvaloniaProperty<Geometry> property, CaptionButtonRole role)
        {
            Property = property;
            Role = role;
        }
    }
}