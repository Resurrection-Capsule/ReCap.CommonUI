using System;
using System.Runtime.InteropServices;
using ReCap.CommonUI.Util.OperatingSystem;
using ReCap.CommonUI.Util.Reflection;

namespace ReCap.CommonUI.Util
{
    internal static class PlatformUtils
    {
        public static OSType GetCurrentOSType(out bool windows, out bool linux, out bool macOS)
        {
            windows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            linux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            macOS = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

            if (windows)
                return OSType.Windows;
            else if (linux)
                return OSType.Linux;
            else if (macOS)
                return OSType.MacOS;
            else
                throw new PlatformNotSupportedException($"{nameof(PlatformUtils)}.{nameof(GetCurrentOSType)}");
        }




        internal static T GetForPlatformByNameMatch<T>(bool nonPublic = ReflectionHelper.DEFAULT_CreateInstance_nonPublic)
        {
            Type iType = typeof(T);
            string implTypeName = InterfaceTypeNameToImplTypeName(iType.FullName);
            Type implType = iType.Assembly.GetType(implTypeName);
            return ReflectionHelper.CreateInstance<T>(implType, nonPublic);
        }


        internal static T GetForPlatformWithConstructors<T, TWindows, TLinux, TMacOS>()
            where TWindows
                : T
                , new()
            where TLinux
                : T
                , new()
            where TMacOS
                : T
                , new()
            => OSInfo.CurrentOS switch
            {
                OSType.Windows => new TWindows(),
                OSType.Linux => new TLinux(),
                OSType.MacOS => new TMacOS(),
                _ => throw new PlatformNotSupportedException($"{nameof(PlatformUtils)}.{nameof(GetForPlatformWithConstructors)}"),
            };


        internal static T GetForPlatform<T>(Type windowsImpl, Type linuxImpl, Type macOSImpl, bool nonPublic = ReflectionHelper.DEFAULT_CreateInstance_nonPublic)
        {
            Type implType = OSInfo.CurrentOS switch
            {
                OSType.Windows => windowsImpl,
                OSType.Linux => linuxImpl,
                OSType.MacOS => macOSImpl,
                _ => throw new PlatformNotSupportedException($"{nameof(PlatformUtils)}.{nameof(GetForPlatform)}"),
            };

            return ReflectionHelper.CreateInstance<T>(implType, nonPublic);
        }


        internal static T GetForPlatform<T, TWindows, TLinux, TMacOS>(bool nonPublic = ReflectionHelper.DEFAULT_CreateInstance_nonPublic)
            where TWindows
                : T
            where TLinux
                : T
            where TMacOS
                : T
                => GetForPlatformByCreateFunc(
                    () => ReflectionHelper.CreateInstance<T>(typeof(TWindows), nonPublic),
                    () => ReflectionHelper.CreateInstance<T>(typeof(TLinux), nonPublic),
                    () => ReflectionHelper.CreateInstance<T>(typeof(TMacOS), nonPublic)
            );




        internal static T GetForPlatformByCreateFunc<T>(Func<T> createWindowsImpl, Func<T> createLinuxImpl, Func<T> createMacOSImpl)
        {
            Func<T> create = OSInfo.CurrentOS switch
            {
                OSType.Windows => createWindowsImpl,
                OSType.Linux => createLinuxImpl,
                OSType.MacOS => createMacOSImpl,
                _ => throw new PlatformNotSupportedException($"{nameof(PlatformUtils)}.{nameof(GetForPlatformByCreateFunc)}"),
            };
            return create();
        }
        internal static Func<T> MakeCreateFunc<T>()
            where T
                : new()
            => () => new();
        internal static Func<TBase> MakeCreateFunc<TBase, T>()
            where T
                : TBase
                , new()
            => () => new T();


        static string InterfaceTypeNameToImplTypeName(string interfaceTypeFullName)
        {
            int namespaceEnd = interfaceTypeFullName.LastIndexOf('.') + 1;
            string typeNamespace = interfaceTypeFullName.Substring(0, namespaceEnd);

            string implTypeName = OSInfo.CurrentOS switch
            {
                OSType.Windows => "Win32",
                OSType.Linux => "Linux",
                OSType.MacOS => "MacOS",
                _ => throw new PlatformNotSupportedException($"{nameof(PlatformUtils)}.{nameof(interfaceTypeFullName)}"),
            };

            return typeNamespace + implTypeName + interfaceTypeFullName.Substring(namespaceEnd + 1);
        }
    }
}