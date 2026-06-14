using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ReCap.CommonUI.Util.Reflection
{
    internal static class ReflectionHelper
    {
        const BindingFlags _FLAGS_ALL = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        
        static bool TryGetMethodInternal(Type ownerType, string name, BindingFlags flags, out MethodInfo methodInfo)
        {
            methodInfo = null;
            
            var methods = ownerType.GetMethods(flags);
            foreach (MethodInfo mInfo in methods)
            {
                if (methodInfo.Name.Contains(name))
                {
                    methodInfo = mInfo;
                    break;
                }
            }
            
            return methodInfo != null;
        }
        static MethodInfo GetMethodInternal(Type ownerType, string name, BindingFlags flags)
        {
            if (!TryGetMethodInternal(ownerType, name, flags, out MethodInfo info))
                throw new NullReferenceException($"Type '{ownerType.Name}' has no '{name}' method!");
            
            return info;
        }

        public static MethodWrapper GetUntypedMethod(Type ownerType, string name, BindingFlags flags = _FLAGS_ALL)
        {
            var methodInfo = GetMethodInternal(ownerType, name, flags);
            return new MethodWrapper(methodInfo);
        }
        
        public static OMethodWrapper<TOwner> GetOwnerTypedMethod<TOwner>(string name, BindingFlags flags = _FLAGS_ALL)
        {
            var methodInfo = GetMethodInternal(typeof(TOwner), name, flags);
            //if (methodInfo.ReturnType == typeof(void))
            
            return new OMethodWrapper<TOwner>(methodInfo);
        }

        public static OPMethodWrapper<TOwner, TParam1> GetOwnerTypedMethodWithTypedParams<TOwner, TParam1>(string name, BindingFlags flags = _FLAGS_ALL)
        {
            var methodInfo = GetMethodInternal(typeof(TOwner), name, flags);
            return new OPMethodWrapper<TOwner, TParam1>(methodInfo);
        }

        public static OPMethodWrapper<TOwner, TParam1, TParam2> GetOwnerTypedMethodWithTypedParams<TOwner, TParam1, TParam2>(string name, BindingFlags flags = _FLAGS_ALL)
        {
            var methodInfo = GetMethodInternal(typeof(TOwner), name, flags);
            return new OPMethodWrapper<TOwner, TParam1, TParam2>(methodInfo);
        }

        public static OPMethodWrapper<TOwner, TParam1, TParam2, TParam3> GetOwnerTypedMethodWithTypedParams<TOwner, TParam1, TParam2, TParam3>(string name, BindingFlags flags = _FLAGS_ALL)
        {
            var methodInfo = GetMethodInternal(typeof(TOwner), name, flags);
            return new OPMethodWrapper<TOwner, TParam1, TParam2, TParam3>(methodInfo);
        }

        public static OPMethodWrapper<TOwner, TParam1, TParam2, TParam3, TParam4> GetOwnerTypedMethodWithTypedParams<TOwner, TParam1, TParam2, TParam3, TParam4>(string name, BindingFlags flags = _FLAGS_ALL)
        {
            var methodInfo = GetMethodInternal(typeof(TOwner), name, flags);
            return new OPMethodWrapper<TOwner, TParam1, TParam2, TParam3, TParam4>(methodInfo);
        }


        public static bool IsAssignableTo(this Type type, Type other)
            => other.IsAssignableFrom(type);

        public static bool IsAssignableTo<T>(this Type type)
            => type.IsAssignableTo(typeof(T));

        public static bool IsAssignableFrom<T>(this Type type)
            => type.IsAssignableFrom(typeof(T));


        public static bool IsParameterValid<TParam>(this ParameterInfo info)
            => IsParameterValid(info, typeof(TParam));
        public static bool IsParameterValid(this ParameterInfo info, Type expectedType)
            => info.ParameterType.IsAssignableTo(expectedType);
        
        public static bool AreParametersValid(this MethodInfo methodInfo, params Type[] expectedTypes)
            => AreParametersValid(methodInfo.GetParameters(), expectedTypes);
        public static bool AreParametersValid(IEnumerable<ParameterInfo> methodParams, params Type[] expectedTypes)
            => AreParametersValid(methodParams.ToArray(), expectedTypes);
        public static bool AreParametersValid(ParameterInfo[] methodParams, params Type[] expectedTypes)
        {
            int actualParamCount = methodParams.Length;
            int expectedParamCount = expectedTypes.Length;
            if (expectedParamCount > actualParamCount)
            {
                throw new IndexOutOfRangeException($"Expected {expectedParamCount} parameters, but matched method has only {actualParamCount} parameters!");
            }
            else if (expectedParamCount < actualParamCount)
            {
                throw new IndexOutOfRangeException($"Matched method has {actualParamCount} parameters, but only {expectedParamCount} parameters were expected!");
            }
                
            
            for (int i = 0; i < actualParamCount; i++)
            {
                var paramInfo = methodParams[i];
                var expectedType = expectedTypes[i];
                if (!paramInfo.IsParameterValid(expectedType))
                    throw new InvalidCastException($"Parameter at [{i}] is of type '{paramInfo.ParameterType}', which is incompatible with expected type '{expectedType}'!");
            }
            
            return true;
        }
        public static bool AreParametersValid(this MethodInfo methodInfo, out Exception exception, params Type[] expectedTypes)
            => AreParametersValid(methodInfo.GetParameters(), out exception, expectedTypes);
        public static bool AreParametersValid(IEnumerable<ParameterInfo> methodParams, out Exception exception, params Type[] expectedTypes)
            => AreParametersValid(methodParams.ToArray(), out exception, expectedTypes);
        public static bool AreParametersValid(ParameterInfo[] methodParams, out Exception exception, params Type[] expectedTypes)
        {
            exception = null;
            try
            {
                return AreParametersValid(methodParams, expectedTypes);
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }
        }

        /*
        public static ORMethodWrapper<TOwner, TRet> GetOwnerAndReturnTypedMethod<TOwner, TRet>(string name, BindingFlags flags = _FLAGS_ALL)
        {
            var methodInfo = GetMethodInternal(typeof(TOwner), name, flags);
            var returnType = methodInfo.ReturnType;
            if (!returnType.IsAssignableTo(typeof(TRet))) //typeof(void)
                throw new InvalidCastException($"Method found has return type '{returnType}', which is incompatible with requested return type '{typeof(TRet)}'");
            
            return new ORMethodWrapper<TOwner, TRet>(methodInfo);
        }
        */


        internal const bool DEFAULT_CreateInstance_nonPublic = false;
        internal static T CreateInstance<T>(Type type, bool nonPublic = DEFAULT_CreateInstance_nonPublic)
        {
            object obj;
            Exception exception;
            try
            {
                obj = Activator.CreateInstance(type, nonPublic);
                if (obj is T typedObj)
                    return typedObj;

                exception = null;
            }
            catch (Exception ex)
            {
                exception = ex;
                obj = null;
            }


            string paramTypeName = typeof(T).FullName;
            string msg;
            if (obj == null)
            {
                msg = $"Could not create instance of type {paramTypeName}!";
                if (exception != null)
                    throw new NullReferenceException(msg, exception);
                else
                    throw new NullReferenceException(msg);
            }
            else
            {
                string objTypeName = obj.GetType().FullName;
                msg = $"'{objTypeName}' is not assignable to '{paramTypeName}'!";
                if (exception != null)
                    throw new InvalidCastException(msg, exception);
                else
                    throw new InvalidCastException(msg);
            }
        }
    }
}