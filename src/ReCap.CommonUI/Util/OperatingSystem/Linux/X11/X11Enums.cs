using System;

namespace ReCap.CommonUI.Util.OperatingSystem.Linux
{
    internal enum LibX11ErrorCode
    {
        /// <summary>
        /// everything's okay
        /// </summary>
        Success             = 0,
        /// <summary>
        /// bad request code
        /// </summary>
        BadRequest          = 1,
        /// <summary>
        /// int parameter out of range
        /// </summary>
        BadValue            = 2,
        /// <summary>
        /// parameter not a Window
        /// </summary>
        BadWindow           = 3,
        /// <summary>
        /// parameter not a Pixmap
        /// </summary>
        BadPixmap           = 4,
        /// <summary>
        /// parameter not an Atom
        /// </summary>
        BadAtom             = 5,
        /// <summary>
        /// parameter not a Cursor
        /// </summary>
        BadCursor           = 6,
        /// <summary>
        /// parameter not a Font
        /// </summary>
        BadFont             = 7,
        /// <summary>
        /// parameter mismatch
        /// </summary>
        BadMatch            = 8,
        /// <summary>
        /// parameter not a Pixmap or Window
        /// </summary>
        BadDrawable         = 9,
        /// <summary>
        /// depending on context:
        /// <list>
        ///     <item>
        ///         <description>key/button already grabbed</description>
        ///     </item>
        ///     <item>
        ///         <description>attempt to free an illegal cmap entry</description>
        ///     </item>
        ///     <item>
        ///         <description>attempt to store into a read-only color map entry.</description>
        ///     </item>
        ///     <item>
        ///         <description>attempt to modify the access control list from other than the local host.</description>
        ///     </item>
        /// </list>
        /// </summary>
        BadAccess           = 10,
       
        /// <summary>
        /// insufficient resources
        /// </summary>
        BadAlloc            = 11,
        /// <summary>
        /// no such colormap
        /// </summary>
        BadColor            = 12,
        /// <summary>
        /// parameter not a GC
        /// </summary>
        BadGC               = 13,
        /// <summary>
        /// choice not in range or already used
        /// </summary>
        BadIDChoice         = 14,
        /// <summary>
        /// font or color name doesn't exist
        /// </summary>
        BadName             = 15,
        /// <summary>
        /// Request length incorrect
        /// </summary>
        BadLength           = 16,
        /// <summary>
        /// server is defective
        /// </summary>
        BadImplementation   = 17,

        FirstExtensionError = 128,
        LastExtensionError  = 255,
    }




    internal enum XEventName
    {
		KeyPress            = 2,
		KeyRelease          = 3,
		ButtonPress         = 4,
		ButtonRelease       = 5,
		MotionNotify        = 6,
		EnterNotify         = 7,
		LeaveNotify         = 8,
		FocusIn             = 9,
		FocusOut            = 10,
		KeymapNotify        = 11,
		Expose              = 12,
		GraphicsExpose      = 13,
		NoExpose            = 14,
		VisibilityNotify    = 15,
		CreateNotify        = 16,
		DestroyNotify       = 17,
		UnmapNotify         = 18,
		MapNotify           = 19,
		MapRequest          = 20,
		ReparentNotify      = 21,
		ConfigureNotify     = 22,
		ConfigureRequest    = 23,
		GravityNotify       = 24,
		ResizeRequest       = 25,
		CirculateNotify     = 26,
		CirculateRequest    = 27,
		PropertyNotify      = 28,
		SelectionClear      = 29,
		SelectionRequest    = 30,
		SelectionNotify     = 31,
		ColormapNotify      = 32,
		ClientMessage       = 33,
		MappingNotify       = 34,
        GenericEvent        = 35,
		LASTEvent,
	}




    [Flags]
    internal enum XModifierMask
    {
        ShiftMask   = 1 << 0,
        LockMask    = 1 << 1,
        ControlMask = 1 << 2,
        Mod1Mask    = 1 << 3,
        Mod2Mask    = 1 << 4,
        Mod3Mask    = 1 << 5,
        Mod4Mask    = 1 << 6,
        Mod5Mask    = 1 << 7,
        Button1Mask = 1 << 8,
        Button2Mask = 1 << 9,
        Button3Mask = 1 << 10,
        Button4Mask = 1 << 11,
        Button5Mask = 1 << 12,
        AnyModifier = 1 << 15,
    }




    internal enum NotifyMode
    {
        NotifyNormal    = 0,
        NotifyGrab      = 1,
        NotifyUngrab    = 2,
    }




    internal enum NotifyDetail
    {
        NotifyAncestor          = 0,
        NotifyVirtual           = 1,
        NotifyInferior          = 2,
        NotifyNonlinear         = 3,
        NotifyNonlinearVirtual  = 4,
        NotifyPointer           = 5,
        NotifyPointerRoot       = 6,
        NotifyDetailNone        = 7,
    }




    internal enum XRequest
        : byte
    {
        X_CreateWindow              = 1,
        X_ChangeWindowAttributes    = 2,
        X_GetWindowAttributes       = 3,
        X_DestroyWindow             = 4,
        X_DestroySubwindows         = 5,
        X_ChangeSaveSet             = 6,
        X_ReparentWindow            = 7,
        X_MapWindow                 = 8,
        X_MapSubwindows             = 9,
        X_UnmapWindow               = 10,
        X_UnmapSubwindows           = 11,
        X_ConfigureWindow           = 12,
        X_CirculateWindow           = 13,
        X_GetGeometry               = 14,
        X_QueryTree                 = 15,
        X_InternAtom                = 16,
        X_GetAtomName               = 17,
        X_ChangeProperty            = 18,
        X_DeleteProperty            = 19,
        X_GetProperty               = 20,
        X_ListProperties            = 21,
        X_SetSelectionOwner         = 22,
        X_GetSelectionOwner         = 23,
        X_ConvertSelection          = 24,
        X_SendEvent                 = 25,
        X_GrabPointer               = 26,
        X_UngrabPointer             = 27,
        X_GrabButton                = 28,
        X_UngrabButton              = 29,
        X_ChangeActivePointerGrab   = 30,
        X_GrabKeyboard              = 31,
        X_UngrabKeyboard            = 32,
        X_GrabKey                   = 33,
        X_UngrabKey                 = 34,
        X_AllowEvents               = 35,
        X_GrabServer                = 36,
        X_UngrabServer              = 37,
        X_QueryPointer              = 38,
        X_GetMotionEvents           = 39,
        X_TranslateCoords           = 40,
        X_WarpPointer               = 41,
        X_SetInputFocus             = 42,
        X_GetInputFocus             = 43,
        X_QueryKeymap               = 44,
        X_OpenFont                  = 45,
        X_CloseFont                 = 46,
        X_QueryFont                 = 47,
        X_QueryTextExtents          = 48,
        X_ListFonts                 = 49,
        X_ListFontsWithInfo         = 50,
        X_SetFontPath               = 51,
        X_GetFontPath               = 52,
        X_CreatePixmap              = 53,
        X_FreePixmap                = 54,
        X_CreateGC                  = 55,
        X_ChangeGC                  = 56,
        X_CopyGC                    = 57,
        X_SetDashes                 = 58,
        X_SetClipRectangles         = 59,
        X_FreeGC                    = 60,
        X_ClearArea                 = 61,
        X_CopyArea                  = 62,
        X_CopyPlane                 = 63,
        X_PolyPoint                 = 64,
        X_PolyLine                  = 65,
        X_PolySegment               = 66,
        X_PolyRectangle             = 67,
        X_PolyArc                   = 68,
        X_FillPoly                  = 69,
        X_PolyFillRectangle         = 70,
        X_PolyFillArc               = 71,
        X_PutImage                  = 72,
        X_GetImage                  = 73,
        X_PolyText8                 = 74,
        X_PolyText16                = 75,
        X_ImageText8                = 76,
        X_ImageText16               = 77,
        X_CreateColormap            = 78,
        X_FreeColormap              = 79,
        X_CopyColormapAndFree       = 80,
        X_InstallColormap           = 81,
        X_UninstallColormap         = 82,
        X_ListInstalledColormaps    = 83,
        X_AllocColor                = 84,
        X_AllocNamedColor           = 85,
        X_AllocColorCells           = 86,
        X_AllocColorPlanes          = 87,
        X_FreeColors                = 88,
        X_StoreColors               = 89,
        X_StoreNamedColor           = 90,
        X_QueryColors               = 91,
        X_LookupColor               = 92,
        X_CreateCursor              = 93,
        X_CreateGlyphCursor         = 94,
        X_FreeCursor                = 95,
        X_RecolorCursor             = 96,
        X_QueryBestSize             = 97,
        X_QueryExtension            = 98,
        X_ListExtensions            = 99,
        X_ChangeKeyboardMapping     = 100,
        X_GetKeyboardMapping        = 101,
        X_ChangeKeyboardControl     = 102,
        X_GetKeyboardControl        = 103,
        X_Bell                      = 104,
        X_ChangePointerControl      = 105,
        X_GetPointerControl         = 106,
        X_SetScreenSaver            = 107,
        X_GetScreenSaver            = 108,
        X_ChangeHosts               = 109,
        X_ListHosts                 = 110,
        X_SetAccessControl          = 111,
        X_SetCloseDownMode          = 112,
        X_KillClient                = 113,
        X_RotateProperties          = 114,
        X_ForceScreenSaver          = 115,
        X_SetPointerMapping         = 116,
        X_GetPointerMapping         = 117,
        X_SetModifierMapping        = 118,
        X_GetModifierMapping        = 119,
        X_NoOperation               = 127,
    }




    [Flags]
    internal enum EventMask
    {
        NoEventMask                 = 0,
        KeyPressMask                = 1 << 0,
        KeyReleaseMask              = 1 << 1,
        ButtonPressMask             = 1 << 2,
        ButtonReleaseMask           = 1 << 3,
        EnterWindowMask             = 1 << 4,
        LeaveWindowMask             = 1 << 5,
        PointerMotionMask           = 1 << 6,
        PointerMotionHintMask       = 1 << 7,
        Button1MotionMask           = 1 << 8,
        Button2MotionMask           = 1 << 9,
        Button3MotionMask           = 1 << 10,
        Button4MotionMask           = 1 << 11,
        Button5MotionMask           = 1 << 12,
        ButtonMotionMask            = 1 << 13,
        KeymapStateMask             = 1 << 14,
        ExposureMask                = 1 << 15,
        VisibilityChangeMask        = 1 << 16,
        StructureNotifyMask         = 1 << 17,
        ResizeRedirectMask          = 1 << 18,
        SubstructureNotifyMask      = 1 << 19,
        SubstructureRedirectMask    = 1 << 20,
        FocusChangeMask             = 1 << 21,
        PropertyChangeMask          = 1 << 22,
        ColormapChangeMask          = 1 << 23,
        OwnerGrabButtonMask         = 1 << 24,
    }



    internal enum Atom
    {
        AnyPropertyType         = 0,
        XA_PRIMARY              = 1,
        XA_SECONDARY            = 2,
        XA_ARC                  = 3,
        XA_ATOM                 = 4,
        XA_BITMAP               = 5,
        XA_CARDINAL             = 6,
        XA_COLORMAP             = 7,
        XA_CURSOR               = 8,
        XA_CUT_BUFFER0          = 9,
        XA_CUT_BUFFER1          = 10,
        XA_CUT_BUFFER2          = 11,
        XA_CUT_BUFFER3          = 12,
        XA_CUT_BUFFER4          = 13,
        XA_CUT_BUFFER5          = 14,
        XA_CUT_BUFFER6          = 15,
        XA_CUT_BUFFER7          = 16,
        XA_DRAWABLE             = 17,
        XA_FONT                 = 18,
        XA_INTEGER              = 19,
        XA_PIXMAP               = 20,
        XA_POINT                = 21,
        XA_RECTANGLE            = 22,
        XA_RESOURCE_MANAGER     = 23,
        XA_RGB_COLOR_MAP        = 24,
        XA_RGB_BEST_MAP         = 25,
        XA_RGB_BLUE_MAP         = 26,
        XA_RGB_DEFAULT_MAP      = 27,
        XA_RGB_GRAY_MAP         = 28,
        XA_RGB_GREEN_MAP        = 29,
        XA_RGB_RED_MAP          = 30,
        XA_STRING               = 31,
        XA_VISUALID             = 32,
        XA_WINDOW               = 33,
        XA_WM_COMMAND           = 34,
        XA_WM_HINTS             = 35,
        XA_WM_CLIENT_MACHINE    = 36,
        XA_WM_ICON_NAME         = 37,
        XA_WM_ICON_SIZE         = 38,
        XA_WM_NAME              = 39,
        XA_WM_NORMAL_HINTS      = 40,
        XA_WM_SIZE_HINTS        = 41,
        XA_WM_ZOOM_HINTS        = 42,
        XA_MIN_SPACE            = 43,
        XA_NORM_SPACE           = 44,
        XA_MAX_SPACE            = 45,
        XA_END_SPACE            = 46,
        XA_SUPERSCRIPT_X        = 47,
        XA_SUPERSCRIPT_Y        = 48,
        XA_SUBSCRIPT_X          = 49,
        XA_SUBSCRIPT_Y          = 50,
        XA_UNDERLINE_POSITION   = 51,
        XA_UNDERLINE_THICKNESS  = 52,
        XA_STRIKEOUT_ASCENT     = 53,
        XA_STRIKEOUT_DESCENT    = 54,
        XA_ITALIC_ANGLE         = 55,
        XA_X_HEIGHT             = 56,
        XA_QUAD_WIDTH           = 57,
        XA_WEIGHT               = 58,
        XA_POINT_SIZE           = 59,
        XA_RESOLUTION           = 60,
        XA_COPYRIGHT            = 61,
        XA_NOTICE               = 62,
        XA_FONT_NAME            = 63,
        XA_FAMILY_NAME          = 64,
        XA_FULL_NAME            = 65,
        XA_CAP_HEIGHT           = 66,
        XA_WM_CLASS             = 67,
        XA_WM_TRANSIENT_FOR     = 68,
        XA_LAST_PREDEFINED      = 68,
    }


    internal enum PropertyMode
    {
		Replace = 0,
		Prepend = 1,
		Append  = 2,
	}
}