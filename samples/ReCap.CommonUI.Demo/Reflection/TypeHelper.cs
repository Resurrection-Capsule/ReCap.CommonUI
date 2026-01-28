using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using ReactiveUI;

namespace ReCap.CommonUI.Demo.Reflection
{
    public static class TypeHelper
    {
        public static IEnumerable<Type> GetTypesInNamespace(Assembly assembly, string ns)
            => GetTypesInNamespace(assembly, ns, TypeFilterOptions.DEFAULT);
        public static IEnumerable<Type> GetTypesInNamespace(Assembly assembly, string ns, TypeFilterOptions opts)
        {
            TypeFilterTypeFlags includeTypes = opts.IncludeTypes;
            TypeFilterModifierFlags includeModifiers = opts.IncludeModifiers;


            IEnumerable<Type> types = includeModifiers.HasFlag(TypeFilterModifierFlags.NonPublic)
                ? assembly.GetTypes()
                : assembly.GetExportedTypes()
            ;


            Func<Type, bool> match = opts.SearchRecursive
                ? (t => MatchNamespaceRecursive(ns, t.Namespace))
                : (t => ns == t.Namespace)
            ;
#if NO
            if (!includeTypes.HasFlag(TypeFilterTypeFlags.Interface))
                match = t => match(t) && !t.IsInterface;


            if (!includeTypes.HasFlag(TypeFilterTypeFlags.Struct))
            {
                if (!includeTypes.HasFlag(TypeFilterTypeFlags.Enum))
                {
                    match = t => match(t) && (t.IsEnum || !t.IsValueType);
                }
                else
                {
                    match = t => match(t) && !t.IsValueType;
                }
            }
            else if (!includeTypes.HasFlag(TypeFilterTypeFlags.Enum))
            {
                match = t => match(t) && !t.IsEnum;
            }


            if (!includeModifiers.HasFlag(TypeFilterModifierFlags.Abstract))
                match = t => match(t) && !t.IsAbstract;


            if (!includeModifiers.HasFlag(TypeFilterModifierFlags.Generic))
                match = t => match(t) && !(t.IsGenericTypeDefinition || t.IsGenericType);

            Type baseType = opts.BaseType;
#pragma warning disable CS0642
            if (baseType == null);
            else if (baseType == typeof(object));
#pragma warning restore CS0642
            else
                match = t => match(t) && t.IsAssignableTo(baseType);
#endif


            return types
                .Where(match)
            ;
        }


        static bool MatchNamespaceRecursive(string ns, string typeNamespace)
        {
            if (typeNamespace == ns)
                return true;
            else if (!typeNamespace.StartsWith(ns))
                return false;

            return typeNamespace[ns.Length + 1] == '.';
        }


        static bool TryCreateInstance(Func<object> createFunc, out object result)
        {
            try
            {
                result = createFunc();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"{nameof(TryCreateInstance)}({createFunc}) threw:\n{ex}");
            }

            result = default;
            return false;
        }


        static bool TryCreateInstanceInternal(Type type, object[] args, out object result)
        {
            if (TryCreateInstance(() => Activator.CreateInstance(type), out result))
                return true;
            else if (TryCreateInstance(() => Activator.CreateInstance(type, nonPublic: true), out result))
                return true;
            else if ((args != null) && TryCreateInstance(() => Activator.CreateInstance(type, args: args), out result))
                return true;


            result = default;
            return false;
        }


        public static bool TryCreateInstanceAs<TResult>(Type type, out TResult result)
            => TryCreateInstanceAs(type, null, out result);
        public static bool TryCreateInstanceAs<TResult>(Type type, object[] args, out TResult result)
        {
            if (TryCreateInstanceInternal(type, args, out object created) && (created is TResult ret))
            {
                result = ret;
                return true;
            }
            else
            {
                result = default;
                return false;
            }
        }
    }
}