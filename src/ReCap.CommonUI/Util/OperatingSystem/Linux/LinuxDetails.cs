using System;

namespace ReCap.CommonUI.Util.OperatingSystem.Linux
{
    public enum KnownDisplayServer
    {
        X11,
        Wayland,
        Unknown,
    }


    public enum KnownDesktopEnvironment
    {
        Plasma,
        GNOME,
        Unknown,
    }




    public sealed class LinuxDetails
        : PlatformDetailsBase
    {
        internal LinuxDetails()
            : base()
        {
            CurrentDisplayServer = InitDisplayServerInfo(out XDGSessionType);
            CurrentDesktopEnvironment = InitDesktopEnvironmentInfo(out XDGCurrentDesktop, out XDGSessionDesktop, out DesktopSession);


            string gtkCsd = Environment.GetEnvironmentVariable("GTK_CSD");
            if (string.IsNullOrWhiteSpace(gtkCsd))
                GTKClientSideDecorations = false;
            else if (int.TryParse(gtkCsd, out int gtkCSD))
                GTKClientSideDecorations = gtkCSD == 1;
            else
                GTKClientSideDecorations = false;


            string mauiCsd = Environment.GetEnvironmentVariable("MAUI_PREFER_CSD");
            OpenMAUIClientSideDecorations = (!string.IsNullOrEmpty(mauiCsd)) && (mauiCsd != "0");


            QtWaylandClientSideDecoration = Environment.GetEnvironmentVariable("QT_WAYLAND_DISABLE_WINDOWDECORATION") != "1";
        }




        public readonly string XDGSessionType = null;
        public readonly KnownDisplayServer CurrentDisplayServer = KnownDisplayServer.Unknown;


        public readonly string XDGCurrentDesktop = null;
        public readonly string XDGSessionDesktop = null;
        public readonly string DesktopSession = null;
        public readonly KnownDesktopEnvironment CurrentDesktopEnvironment = KnownDesktopEnvironment.Unknown;


        public readonly bool QtWaylandClientSideDecoration;
        public readonly bool GTKClientSideDecorations;
        public readonly bool OpenMAUIClientSideDecorations;




        KnownDisplayServer InitDisplayServerInfo(out string sessionType)
        {
            sessionType = Environment.GetEnvironmentVariable("XDG_SESSION_TYPE");
            return sessionType switch
            {
                "x11" => KnownDisplayServer.X11,
                "wayland" => KnownDisplayServer.Wayland,
                _ => KnownDisplayServer.Unknown,
            };
        }




        const char _ENV_VAR_SPLIT_CHAR = ':';
        static readonly string _ENV_VAR_SPLIT_STR = $"{_ENV_VAR_SPLIT_CHAR}";
        static readonly char[] _ENV_VAR_SPLIT_CHARS = new[]
        {
            _ENV_VAR_SPLIT_CHAR,
        };
        KnownDesktopEnvironment InitDesktopEnvironmentInfo(
            out string xdgSessionDesktop,
            out string xdgCurrentDesktop,
            out string desktopSession)
        {
            xdgSessionDesktop = Environment.GetEnvironmentVariable("XDG_SESSION_DESKTOP");
            xdgCurrentDesktop = Environment.GetEnvironmentVariable("XDG_CURRENT_DESKTOP");
            desktopSession = Environment.GetEnvironmentVariable("DESKTOP_SESSION");

            if (TryGetFromDesktopIdentifier(xdgSessionDesktop, out KnownDesktopEnvironment desktopEnvironment))
                return desktopEnvironment;


            if (!string.IsNullOrWhiteSpace(xdgCurrentDesktop))
            {
                if (xdgCurrentDesktop.Contains(_ENV_VAR_SPLIT_STR))
                {
                    var xdgCurrentDesktopSegments = xdgCurrentDesktop.Split(_ENV_VAR_SPLIT_CHARS, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string xdgCurrentDesktopSegment in xdgCurrentDesktopSegments)
                    {
                        if (TryGetFromDesktopIdentifier(xdgCurrentDesktopSegment, out desktopEnvironment))
                            return desktopEnvironment;
                    }
                }
                else if (TryGetFromDesktopIdentifier(xdgSessionDesktop, out desktopEnvironment))
                {
                    return desktopEnvironment;
                }
            }


            if (!string.IsNullOrWhiteSpace(desktopSession))
            {
                switch (desktopSession)
                {
                    case "plasma":
                        return KnownDesktopEnvironment.Plasma;

                    case "gnome":
                        return KnownDesktopEnvironment.GNOME;
                }
            }


            return KnownDesktopEnvironment.Unknown;
        }




        bool TryGetFromDesktopIdentifier(string identifier, out KnownDesktopEnvironment result)
        {
            if (string.IsNullOrWhiteSpace(identifier))
                result = KnownDesktopEnvironment.Unknown;
            else if (identifier.Equals("KDE", StringComparison.InvariantCultureIgnoreCase))
                result = KnownDesktopEnvironment.Plasma;
            else if (identifier.Equals("GNOME", StringComparison.InvariantCultureIgnoreCase))
                result = KnownDesktopEnvironment.GNOME;
            else
            {
                result = KnownDesktopEnvironment.Unknown;
                return false;
            }

            return result != KnownDesktopEnvironment.Unknown;
        }
    }
}