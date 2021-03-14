using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace FryScript.Extensions
{
    public static class ReflectionExtensions
    {
        private const string Implicit = "op_Implicit";
        private const string Explicit = "op_Explicit";

        public static bool TryCanConvert(this Type fromType, Type toType, out MethodInfo method)
        {
            fromType = fromType ?? throw new ArgumentNullException(nameof(fromType));
            toType = toType ?? throw new ArgumentNullException(nameof(toType));

            method = (from m in fromType.GetTypeInfo().DeclaredMethods
                let ps = m.GetParameters()
                where ps.Length == 1
                      && ps[0].ParameterType == fromType
                      && m.ReturnType == toType
                      && (m.Name == Explicit || m.Name == Implicit)
                select m
                ).FirstOrDefault();

            return method != null;
        }

        public static bool IsExtensionType(this Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            return type.GetTypeInfo().GetCustomAttribute<ExtensionAttribute>() != null;
        }

        public static bool IsExtensionMethod(this MethodInfo methodInfo)
        {
            methodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));

            return methodInfo.GetCustomAttribute<ExtensionAttribute>() != null;
        }

        public static bool IsNullable(this Type type, Type ofType = null)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && ofType == null)
                return true;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>) && type.GetGenericArguments()[0] == ofType)
                return true;

            return false;
        }

        public static Type UnwrapNullable(this Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            return type.GetGenericArguments()[0];
        }

        public static object GetDefaultValue(this Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            if (type == typeof(string))
                return string.Empty;

            var getDefaultValueMethod = typeof(ReflectionExtensions).GetTypeInfo().GetMethod(nameof(ReflectionExtensions.GetDefaultValue), BindingFlags.Static | BindingFlags.NonPublic);
            getDefaultValueMethod = getDefaultValueMethod.MakeGenericMethod(type);

            return getDefaultValueMethod.Invoke(null, null);
        }

        private static T GetDefaultValue<T>()
        {
            return default(T);
        }
    }
}
