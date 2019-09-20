using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Reflection;

namespace FryScript.HostInterop
{
    public interface ITypeProvider
    {
        Func<ScriptObject, object> GetCtor(Type type);
        Type GetHighestNumericType(params Type[] types);
        IEnumerable<string> GetMemberNames(Type type);

        IEnumerable<ScriptableMethodInfo> GetMethods(Type type);
        IEnumerable<Type> GetPrimitives();
        Type GetProxy(Type type);
        string GetTypeName(Type type);
        bool HasIndex(Type type, Type indexType);
        bool HasMember(Type type, string name);
        bool HasMethod(Type type, string name);
        bool HasProperty(Type type, string name);
        bool IsNumericType(Type type);
        bool IsPrimitive(Type type);
        void RegisterExtensions(Type extensionType);
        void RegisterPrimitive(Type type);
        void RegisterPrimitive<T>(ScriptPrimitive<T> primitive);
        bool TryGetBinaryOperator(Type left, ScriptableBinaryOperater operation, Type right, out MethodInfo methodInfo);
        bool TryGetConvertOperator(Type fromType, Type toType, out MethodInfo methodInfo);
        bool TryGetExtensionMethod(Type type, string name, out MethodInfo methodInfo);
        bool TryGetIndex(Type type, Type indexType, out PropertyInfo propertyInfo);
        bool TryGetMethod(Type type, string name, out ScriptableMethodInfo methodInfo);
        bool TryGetProperty(Type type, string name, out PropertyInfo propertyInfo);
        bool TryGetTypeOperator(Type type, ScriptableTypeOperator operation, out MethodInfo methodInfo);
    }
}
