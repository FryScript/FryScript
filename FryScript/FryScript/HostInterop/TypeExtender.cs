using FryScript.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace FryScript.HostInterop
{
    public class TypeExtender
    {
        private readonly Type _extendedType;
        private readonly ConcurrentDictionary<Tuple<Type, ScriptableBinaryOperater>, MethodInfo> _binaryOperators = new ConcurrentDictionary<Tuple<Type, ScriptableBinaryOperater>, MethodInfo>();
        private readonly ConcurrentDictionary<ScriptableTypeOperator, MethodInfo> _typeOperators = new ConcurrentDictionary<ScriptableTypeOperator, MethodInfo>();
        private readonly ConcurrentDictionary<string, MethodInfo> _extensionMethods = new ConcurrentDictionary<string, MethodInfo>();
        private readonly ConcurrentDictionary<Type, MethodInfo> _convertOperators = new ConcurrentDictionary<Type, MethodInfo>();

        public TypeExtender(Type extendedType)
        {
            _extendedType = extendedType ?? throw new ArgumentNullException(nameof(extendedType));
        }

        public void RegisterBinaryOperator(ScriptableBinaryOperater operation, MethodInfo methodInfo)
        {
            methodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));

            var parameterInfos = methodInfo.GetParameters();

            EnsureExtendedType(methodInfo, parameterInfos);
            EnsureReturnType(methodInfo);
            EnsureParameterCount(methodInfo, parameterInfos, 2);

            var key = GetOperatorKey(parameterInfos[1].ParameterType, operation);

            if (_binaryOperators.TryGetValue(key, out MethodInfo existingMethod))
                throw new InvalidOperationException(
                    string.Format("Type {0} already has an {1} operator defined by type {2}",
                        _extendedType.FullName,
                        Enum.GetName(typeof(ScriptableBinaryOperater), operation),
                        existingMethod.DeclaringType.FullName
                        ));

            _binaryOperators[key] = methodInfo;
        }

        public MethodInfo GetBinaryOperator(ScriptableBinaryOperater operation, Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            var key = GetOperatorKey(type, operation);
            _binaryOperators.TryGetValue(key, out MethodInfo op);
            return op;
        }

        public void RegisterTypeOperator(ScriptableTypeOperator operation, MethodInfo methodInfo)
        {
            methodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));

            var parameterInfos = methodInfo.GetParameters();
            
            EnsureExtendedType(methodInfo, parameterInfos);
            EnsureReturnType(methodInfo);

            if (_typeOperators.TryGetValue(operation, out MethodInfo existingMethod))
                throw new InvalidOperationException(
                    string.Format("Type {0} already has an {1} operator defined by type {2}",
                    _extendedType.FullName,
                    Enum.GetName(typeof(ScriptableTypeOperator), operation),
                    existingMethod.DeclaringType.FullName));

            _typeOperators[operation] = methodInfo;
        }

        public MethodInfo GetTypeOperator(ScriptableTypeOperator operation)
        {
            _typeOperators.TryGetValue(operation, out MethodInfo op);
            return op;
        }

        public void RegisterConvertOperator(MethodInfo methodInfo)
        {
            methodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));

            var parameterInfos = methodInfo.GetParameters();

            EnsureExtendedType(methodInfo, parameterInfos);
            EnsureParameterCount(methodInfo, parameterInfos, 1);

            if (_convertOperators.TryGetValue(methodInfo.ReturnType, out MethodInfo existingMethod))
                throw new InvalidOperationException(
                    string.Format("Type {0} already has a convert operator for type {1} defined by type {2}",
                        _extendedType.FullName,
                        methodInfo.ReturnType.FullName,
                        existingMethod.DeclaringType.FullName
                        ));

            _convertOperators[methodInfo.ReturnType] = methodInfo;
        }

        public MethodInfo GetConvertOperator(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            _convertOperators.TryGetValue(type, out MethodInfo op);

            return op;
        }

        public void RegisterExtensionMethod(string name, MethodInfo methodInfo)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));
            methodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));

            var parameterInfos = methodInfo.GetParameters();

            EnsureExtendedType(methodInfo, parameterInfos);
            EnsureReturnType(methodInfo);

            if (_extensionMethods.TryGetValue(name, out MethodInfo existingMethod))
                throw new InvalidOperationException(
                    string.Format("Type {0} already has an extension method named {1} defined by type {2}",
                        _extendedType.FullName,
                        name,
                        existingMethod.DeclaringType.FullName
                        ));

            _extensionMethods[name] = methodInfo;
        }

        public MethodInfo GetExtensionMethod(string name)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));

            _extensionMethods.TryGetValue(name, out MethodInfo extension);
            return extension;
        }

        public bool HasExtensionMethod(string name)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));

            return _extensionMethods.ContainsKey(name);
        }

        public IEnumerable<string> GetExtensionMethodsNames()
        {
            return _extensionMethods.Keys;
        }

        private void EnsureExtendedType(MethodInfo methodInfo, ParameterInfo[] parameterInfos)
        {
            if (parameterInfos[0].ParameterType != _extendedType)
                throw new InvalidOperationException($"The first parameter of method {methodInfo.Name} of type {parameterInfos[0].ParameterType.FullName} must be of type {_extendedType.FullName}");

            if (!methodInfo.IsExtensionMethod())
                throw new ArgumentException($"Method {methodInfo.Name} declared by {methodInfo.DeclaringType.FullName} must be an extension method to use it as a script extension", nameof(methodInfo));
        }

        private static void EnsureParameterCount(MethodInfo methodInfo, ParameterInfo[] parameterInfos, int count)
        {
            if (parameterInfos.Length != count)
                throw new ArgumentException($"Method {methodInfo.Name} of type {methodInfo.DeclaringType.FullName} must have exactly {count} parameters",nameof(methodInfo));
        }

        private static void EnsureReturnType(MethodInfo methodInfo)
        {
            if (methodInfo.ReturnType != typeof(object))
                throw new ArgumentException($"Method {methodInfo.Name} declared by {methodInfo.DeclaringType.FullName} must have a return type of {typeof(object).FullName}", nameof(methodInfo));
        }

        private static Tuple<Type, T> GetOperatorKey<T>(Type type, T operation)
        {
            return new Tuple<Type, T>(type, operation);
        }
    }
}
