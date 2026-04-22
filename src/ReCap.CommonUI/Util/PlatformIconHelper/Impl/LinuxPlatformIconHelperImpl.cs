using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Microsoft.Extensions.Configuration.Ini;

using AvBitmap = Avalonia.Media.Imaging.Bitmap;
using IAvBitmapsEnumerable = System.Collections.Generic.IEnumerable<Avalonia.Media.Imaging.Bitmap>;

namespace ReCap.CommonUI.Util
{
    internal sealed class LinuxPlatformIconHelperImpl
        : IPlatformIconHelperImpl
    {
        const char _CHAR_NULL = '\0';


        public IAvBitmapsEnumerable GetIconVariantsForWindow(Window window, bool fallbackToAppIcon, out bool handledFallbackToAppIcon)
            => throw new NotImplementedException();


        public IAvBitmapsEnumerable GetIconVariantsForWindowIcon(WindowIcon windowIcon)
            => throw new NotImplementedException();


        const string _BAMF_DESKTOP_FILE_HINT_PREFIX = "BAMF_DESKTOP_FILE_HINT=";
        static readonly int _BAMF_DESKTOP_FILE_HINT_PREFIX_LENGTH = _BAMF_DESKTOP_FILE_HINT_PREFIX.Length;
        string _bamfDesktopFileHint = null;
        public IAvBitmapsEnumerable GetIconVariantsForApp()
        {
            string bamfDesktopFileHint = _bamfDesktopFileHint;
            if ((bamfDesktopFileHint == null) && !TryGetBamfDesktopFileHint(out bamfDesktopFileHint))
                return Array.Empty<AvBitmap>();

            if (!File.Exists(bamfDesktopFileHint))
                return Array.Empty<AvBitmap>();
            
            IDictionary<string, string> desktopFile;
            using (var desktopFileStream = File.OpenRead(bamfDesktopFileHint))
            {
                desktopFile = IniStreamConfigurationProvider.Read(desktopFileStream);
            }

            if (!desktopFile.TryGetValue("Desktop Entry:Icon", out string iconPath))
                return Array.Empty<AvBitmap>();

            if (!File.Exists(iconPath))
                iconPath = Path.GetFullPath(iconPath);

            if (!File.Exists(iconPath))
                return Array.Empty<AvBitmap>();

            using MemoryStream iconStream = new();
            using (var iconFileStream = File.OpenRead(iconPath))
            {
                iconFileStream.CopyTo(iconStream);
                iconStream.Position = 0;
            }

            AvBitmap icon = new(iconStream);
            return Helpers.LonerArray(icon);
        }


        static bool TryGetBamfDesktopFileHint(out string bamfDesktopFileHint)
        {
            var processID = Process.GetCurrentProcess().Id;
            var lines1 = File.ReadAllLines($"/proc/{processID}/environ");
            foreach (string line1 in lines1)
            {
                IEnumerable<string> lines2 = line1.Contains(_CHAR_NULL)
                    ? line1.Split(_CHAR_NULL)
                    : Helpers.LonerArray(line1)
                ;
                foreach (string line2 in lines2)
                {
                    if (line2.StartsWith(_BAMF_DESKTOP_FILE_HINT_PREFIX))
                    {
                        bamfDesktopFileHint = line2.Substring(_BAMF_DESKTOP_FILE_HINT_PREFIX_LENGTH);
                        return true;
                    }
                }
            }

            bamfDesktopFileHint = null;
            return false;
        }
    }
}