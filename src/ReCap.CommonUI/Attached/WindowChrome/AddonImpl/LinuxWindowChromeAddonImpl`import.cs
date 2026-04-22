using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Microsoft.Extensions.Configuration.Ini;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    partial class LinuxWindowChromeAddonImpl
    {
        static bool TryParseRoleGTK3(string gtkCaptionButton, out CaptionButtonRole role)
        {
            switch (gtkCaptionButton)
            {
                case "minimize":
                {
                    role = CaptionButtonRole.Minimize;
                    break;
                }
                case "maximize":
                {
                    role = CaptionButtonRole.Maximize;
                    break;
                }
                case "close":
                {
                    role = CaptionButtonRole.Close;
                    break;
                }
                default:
                {
                    role = default;
                    return false;
                }
            }
            return true;
        }
        /*
        const char _GTK3_SPLIT_1 = ':';
        static readonly string _GTK3_SPLIT_1_STR = $"{_GTK3_SPLIT_1}";
        */
        const string _GTK3_SPLIT_1 = ":";
        static readonly char[] _GTK3_SPLIT_1_ARR = _GTK3_SPLIT_1.ToCharArray();
        /*
        const char _GTK3_SPLIT_2 = ',';
        static readonly string _GTK3_SPLIT_2_STR = $"{_GTK3_SPLIT_2}";
        */
        const string _GTK3_SPLIT_2 = ",";
        static readonly char[] _GTK3_SPLIT_2_ARR = _GTK3_SPLIT_2.ToCharArray();
        static CaptionButtonRoles ParseRolesGTK3(string gtkCaptionButtonsStr)
        {
            CaptionButtonRoles roles = new();
            if (string.IsNullOrWhiteSpace(gtkCaptionButtonsStr))
                return roles;

            string[] gtkCaptionButtons = gtkCaptionButtonsStr.Contains(_GTK3_SPLIT_2)
                ? gtkCaptionButtonsStr.Split(_GTK3_SPLIT_2_ARR, StringSplitOptions.None)
                : Helpers.LonerArray(gtkCaptionButtonsStr)
            ;
            foreach (string gtkCaptionButton in gtkCaptionButtons)
            {
                if (TryParseRoleGTK3(gtkCaptionButton, out CaptionButtonRole role))
                    roles.Add(role);
            }
            return roles;
        }

        // https://developers.redhat.com/blog/2018/11/07/dotnet-special-folder-api-linux
        static bool TryGetCaptionButtonsGTK3(out CaptionButtonRolesPair rolesPair)
        {
            string dotConfig = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string gtk3SettingsPath = Path.Combine(dotConfig, "gtk-3.0", "settings.ini");
            if (!File.Exists(gtk3SettingsPath))
                goto fail;

            using (var gtk3SettingsStream = File.OpenRead(gtk3SettingsPath))
            {
                var gtk3Settings = IniStreamConfigurationProvider.Read(gtk3SettingsStream);
                if (!gtk3Settings.TryGetValue("Settings:gtk-decoration-layout", out string gtkCaptionButtons))
                    goto fail;

                if (!gtkCaptionButtons.Contains(_GTK3_SPLIT_1))
                    goto fail;

                string[] gtkCaptionButtonsLR = gtkCaptionButtons.Split(_GTK3_SPLIT_1_ARR, StringSplitOptions.None);
                string gtkCaptionButtonsStrL = gtkCaptionButtonsLR[0].Trim();
                string gtkCaptionButtonsStrR = gtkCaptionButtonsLR[gtkCaptionButtonsLR.Length - 1].Trim();

                CaptionButtonRoles captionButtonsRolesL = ParseRolesGTK3(gtkCaptionButtonsStrL);
                CaptionButtonRoles captionButtonsRolesR = ParseRolesGTK3(gtkCaptionButtonsStrR);
                rolesPair = new(captionButtonsRolesL, captionButtonsRolesR);
                Debug.WriteLine("CaptionButtons imported from GTK3!");
                return true;
            }

            fail:
            rolesPair = default;
            return false;
        }


        // https://reddit.com/r/kde/comments/n160vc/hidden_titlebarbuttons_in/gwbyyj4/
        static readonly IReadOnlyDictionary<char, CaptionButtonRole> _KWIN_CAPTION_BUTTONS = new Dictionary<char, CaptionButtonRole>()
        {
            ['I'] = CaptionButtonRole.Minimize,
            ['A'] = CaptionButtonRole.Maximize,
            ['X'] = CaptionButtonRole.Close,
            ['F'] = CaptionButtonRole.KeepAbove,
            ['M'] = CaptionButtonRole.Menu,
        };
        static CaptionButtonRoles ParseCaptionButtonsKWin(string kwinButtons)
        {
            CaptionButtonRoles roles = new();
            if (string.IsNullOrWhiteSpace(kwinButtons))
                return roles;

            char[] kwinCaptionButtons = kwinButtons.ToCharArray();
            foreach (char kwinCaptionButton in kwinCaptionButtons)
            {
                if (_KWIN_CAPTION_BUTTONS.TryGetValue(kwinCaptionButton, out CaptionButtonRole role))
                    roles.Add(role);
            }
            return roles;
        }
        static bool TryGetCaptionButtonsKWin(out CaptionButtonRolesPair rolesPair)
        {
            string dotConfig = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string kwinrcPath = Path.Combine(dotConfig, "kwinrc");
            if (!File.Exists(kwinrcPath))
                goto fail;

            using (var kwinrcStream = File.OpenRead(kwinrcPath))
            {
                var kwinrc = IniStreamConfigurationProvider.Read(kwinrcStream);

                bool hasLeftButtons = kwinrc.TryGetValue("org.kde.kdecoration2:ButtonsOnLeft", out string kwinButtonsOnLeft);
                bool hasRightButtons = kwinrc.TryGetValue("org.kde.kdecoration2:ButtonsOnRight", out string kwinButtonsOnRight);
                if (hasLeftButtons || hasRightButtons)
                {
                    CaptionButtonRoles leftButtons = hasLeftButtons
                        ? ParseCaptionButtonsKWin(kwinButtonsOnLeft)
                        : new() // [TODO: is this correct?]
                    ;
                    CaptionButtonRoles rightButtons = hasRightButtons
                        ? ParseCaptionButtonsKWin(kwinButtonsOnRight)
                        : new()
                        {
                            // [TODO: is this correct?]
                            CaptionButtonRole.Minimize,
                            CaptionButtonRole.Maximize,
                            CaptionButtonRole.Close,
                        }
                    ;
                    rolesPair = new(leftButtons, rightButtons);
                    Debug.WriteLine("CaptionButtons imported from kwin!");
                    return true;
                }
            }

            fail:
            rolesPair = default;
            return false;
        }


        bool TryImportCaptionButtons(out CaptionButtonRolesPair imported)
        {
#if NO //DEBUG
            imported = DefaultWindowChromeAddonImpl.DefaultCaptionButtons_Default(this);
            return true;
#else
            if (TryGetCaptionButtonsKWin(out imported))
                return true;
            else if (TryGetCaptionButtonsGTK3(out imported))
                return true;
            //[TODO: what others?]

            imported = default;
            return false;
#endif
        }
    }
}