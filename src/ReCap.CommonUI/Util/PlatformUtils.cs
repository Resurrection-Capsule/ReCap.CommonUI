using System;
using ReCap.CommonUI.Util.Reflection;

namespace ReCap.CommonUI.Util
{
    internal static class PlatformUtils
    {
        internal static T GetForPlatform<T>()
        {
            Type iType = typeof(T);
            string implTypeName = InterfaceTypeNameToImplTypeName(iType.FullName);
            Type implType = iType.Assembly.GetType(implTypeName);
            return ReflectionHelper.CreateInstance<T>(implType);
        }


        internal static T GetForPlatform<T, TWindows, TLinux, TMacOS>()
            where TWindows : T, new()
            where TLinux : T, new()
            where TMacOS : T, new()
            => GetForPlatformInternal<T>(typeof(TWindows), typeof(TLinux), typeof(TMacOS));


        static T GetForPlatformInternal<T>(Type windowsImpl, Type linuxImpl, Type macOSImpl)
        {
            Type implType;
            if (OSInfo.IsWindows)
                implType = windowsImpl;
            else if (OSInfo.IsLinux)
                implType = linuxImpl;
            else if (OSInfo.IsMacOS)
                implType = macOSImpl;
            else
                throw new PlatformNotSupportedException();

            return ReflectionHelper.CreateInstance<T>(implType);
        }


        static string InterfaceTypeNameToImplTypeName(string interfaceTypeFullName)
        {
            int namespaceEnd = interfaceTypeFullName.LastIndexOf('.') + 1;
            string typeNamespace = interfaceTypeFullName.Substring(0, namespaceEnd);

            string implTypeName;
            if (OSInfo.IsWindows)
                implTypeName = "Windows";
            else if (OSInfo.IsLinux)
                implTypeName = "Linux";
            else if (OSInfo.IsMacOS)
                implTypeName = "MacOS";
            else
                throw new PlatformNotSupportedException();

            return typeNamespace + implTypeName + interfaceTypeFullName.Substring(namespaceEnd + 1);
        }
    }
}