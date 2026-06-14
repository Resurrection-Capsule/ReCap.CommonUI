using System;
using System.Collections.Generic;
using System.Linq;
using ReCap.CommonUI.Util.OperatingSystem;
using ReCap.CommonUI.Util.OperatingSystem.Linux;
using OperatingSystemInformationPair = System.Collections.Generic.KeyValuePair<string, object>;

namespace ReCap.CommonUI.Demo.ViewModels.Pages
{
    public sealed class DiagnosticsViewModel
        : ViewModelBase
    {
        public const string SEPARATOR_HEADER = "-";




        IEnumerable<OperatingSystemInformationPair> _operatingSystemGeneralInformation = CleanUp________________(new OperatingSystemInformationPair[]
        {
            new(nameof(OSInfo.Version), FormatVersionInformation()),
            ________________(),
            new(nameof(OSInfo.CurrentOS), OSInfo.CurrentOS),
            ________________(),
            new(nameof(OSInfo.IsWindows), OSInfo.IsWindows),
            new(nameof(OSInfo.IsLinux), OSInfo.IsLinux),
            new(nameof(OSInfo.IsMacOS), OSInfo.IsMacOS),
        });
        public IEnumerable<OperatingSystemInformationPair> OperatingSystemGeneralInformation
        {
            get => _operatingSystemGeneralInformation;
            private set => RASIC(ref _operatingSystemGeneralInformation, value);
        }




        IEnumerable<OperatingSystemInformationPair> _operatingSystemPlatformSpecificInformation = TryGetPlatformSpecificInformation(out IEnumerable<OperatingSystemInformationPair> platformSpecificInformation)
            ? CleanUp________________(platformSpecificInformation)
            : Array.Empty<OperatingSystemInformationPair>()
        ;
        public IEnumerable<OperatingSystemInformationPair> OperatingSystemPlatformSpecificInformation
        {
            get => _operatingSystemPlatformSpecificInformation;
            private set => RASIC(ref _operatingSystemPlatformSpecificInformation, value);
        }




        static string FormatVersionInformation()
        {
            string osVersion = OSInfo.Version.ToString();
#if DEBUG
            if (!OSInfo.IsVersionDefinitelyAccurate)
                return $"{osVersion} (potentially inaccurate)";
#endif
            return osVersion;
        }




        static bool TryGetPlatformSpecificInformation(out IEnumerable<OperatingSystemInformationPair> platformSpecificInformation)
        {
            var platformInfos = OSInfo.CurrentOS switch
            {
                OSType.Windows => GetWindowsInformation(),
                OSType.Linux => GetLinuxInformation(),
                OSType.MacOS => GetMacOSInformation(),
                _ => null,
            };


            if (platformInfos == null)
                goto fail;
            else if (platformInfos.Any())
            {
                platformSpecificInformation = platformInfos;
                return true;
            }

            fail:
            platformSpecificInformation = null;
            return false;
        }




        static IEnumerable<OperatingSystemInformationPair> GetWindowsInformation()
            => null;


        static IEnumerable<OperatingSystemInformationPair> GetLinuxInformation()
        {
            var details = OSInfo.GetOSDetails<LinuxDetails>();


            {
                if (MakePairIf(nameof(details.XDGSessionType), details.XDGSessionType, out OperatingSystemInformationPair xdgSessionType))
                    yield return xdgSessionType;
            }
            yield return ________________();
            {
                if (MakePairIf(nameof(details.XDGCurrentDesktop), details.XDGCurrentDesktop, out OperatingSystemInformationPair xdgCurrentDesktop))
                    yield return xdgCurrentDesktop;

                if (MakePairIf(nameof(details.XDGSessionDesktop), details.XDGSessionDesktop, out OperatingSystemInformationPair xdgSessionDesktop))
                    yield return xdgSessionDesktop;

                if (MakePairIf(nameof(details.DesktopSession), details.DesktopSession, out OperatingSystemInformationPair desktopSession))
                    yield return desktopSession;
            }
            yield return ________________();
            {
                yield return new(nameof(details.CurrentDisplayServer), details.CurrentDisplayServer);
                yield return new(nameof(details.CurrentDesktopEnvironment), details.CurrentDesktopEnvironment);
            }
            yield return ________________();
            {
                yield return new(nameof(details.GTKClientSideDecorations), details.GTKClientSideDecorations);
                yield return new(nameof(details.QtWaylandClientSideDecoration), details.QtWaylandClientSideDecoration);
                yield return new(nameof(details.OpenMAUIClientSideDecorations), details.OpenMAUIClientSideDecorations);
            }
        }


        static IEnumerable<OperatingSystemInformationPair> GetMacOSInformation()
            => null;




        static bool MakePairIf(string key, object value, string str, out OperatingSystemInformationPair pair)
            => MakePairIf(key, value, !string.IsNullOrWhiteSpace(str), out pair);
        static bool MakePairIf(string key, string value, out OperatingSystemInformationPair pair)
            => MakePairIf(key, value, value, out pair);
        static bool MakePairIf(string key, object value, bool condition, out OperatingSystemInformationPair pair)
        {
            if (condition)
            {
                pair = new(key, value);
                return true;
            }
            else
            {
                pair = default;
                return false;
            }
        }




        static OperatingSystemInformationPair ________________()
            => new(SEPARATOR_HEADER, null);


        static bool Is________________(OperatingSystemInformationPair pair)
            => SEPARATOR_HEADER == pair.Key;


        static List<OperatingSystemInformationPair> CleanUp________________(IEnumerable<OperatingSystemInformationPair> platformInfos)
        {
            if (platformInfos == null)
                return null;
            else if (!platformInfos.Any())
                return new List<OperatingSystemInformationPair>();

            List<OperatingSystemInformationPair> result = new(platformInfos);

            while (result.Any())
            {
                var item = result[0];
                if (Is________________(item))
                    result.RemoveAt(0);
                else
                    break;
            }


            while (result.Any())
            {
                var last = result.Last();
                if (Is________________(last))
                    result.RemoveAt(result.Count - 1);
                else
                    break;
            }


            for (int i = 1; i < result.Count; i++)
            {
                if (Is________________(result[i]) && Is________________(result[i - 1]))
                {
                    result.RemoveAt(i);
                    i--;
                }
            }

            return result;
        }
    }
}