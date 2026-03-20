using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration.Ini;
using ReCap.CommonUI.Util;

namespace ReCap.CommonUI.Attached.WindowChrome
{
    internal sealed class LinuxWindowChromeAddonImpl
        : WindowChromeAddonImplBase
    {
        public override bool CanUseManagedWindowChrome
        {
            get
            {
                if (OSInfo.LinuxIsUsingX11)
                    return true;
                else // [TODO: Wayland?????]
                    return false;
            }
        }


        public override bool PrefersManagedWindowChrome
        {
            get
            {
                if (int.TryParse(Environment.GetEnvironmentVariable("GTK_CSD"), out int gtkCSD))
                    return gtkCSD == 1;
                else
                    return OSInfo.LinuxIsUsingGnome;
            }
        }



        protected override IEnumerable<CaptionButtonRole> GetValidCaptionButtonRoles()
            => Enum.GetValues(typeof(CaptionButtonRole))
                .Cast<CaptionButtonRole>()
                .ToArray()
            ;


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
                : new[]
                {
                    gtkCaptionButtonsStr
                }
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
                /*
                if (!kwinrc.TryGetValue("Settings:gtk-decoration-layout", out string gtkCaptionButtons))
                    goto fail;
                */
                /*
                foreach (var pair in kwinrc)
                {
                    Debug.WriteLine($"[{pair.Key}]='{pair.Value}'");
                }
                */
                if (!kwinrc.TryGetValue("org.kde.kdecoration2:ButtonsOnLeft", out string kwinButtonsOnLeft))
                    goto fail;
                if (!kwinrc.TryGetValue("org.kde.kdecoration2:ButtonsOnRight", out string kwinButtonsOnRight))
                    goto fail;

                CaptionButtonRoles leftButtons = ParseCaptionButtonsKWin(kwinButtonsOnLeft);
                CaptionButtonRoles rightButtons = ParseCaptionButtonsKWin(kwinButtonsOnRight);
                rolesPair = new(leftButtons, rightButtons);
                return true;
            }

            fail:
            rolesPair = default;
            return false;
        }
        protected override CaptionButtonRolesPair CreateDefaultCaptionButtons()
        {
            if (TryGetCaptionButtonsKWin(out CaptionButtonRolesPair kwinRolesPair))
                return kwinRolesPair;
            /*
            else if (TryGetCaptionButtonsGTK3(out CaptionButtonRolesPair gtk3RolesPair))
                return gtk3RolesPair;
            */
            //KWin();
            return new()
            {
                //[TODO: Detect e.g. Unity DE?]
                //[TODO: platform-level user settings?]
                Left = new()
                {
                    /*
                    CaptionButtonRole.Close,
                    CaptionButtonRole.Minimize,
                    CaptionButtonRole.Maximize,
                    */
                    CaptionButtonRole.FullScreen,
                },
                Right = new()
                {
                    CaptionButtonRole.Minimize,
                    CaptionButtonRole.Maximize,
                    CaptionButtonRole.Close,
                },
            };
        }
    }
}