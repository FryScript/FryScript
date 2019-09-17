using FryScript.Extensions;
using FryScript.HostInterop.Extensions;
using FryScript.HostInterop.Operators;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FryScript.HostInterop
{
    public class TypeProvider : ITypeProvider
    {
        public static TypeProvider Current = new TypeProvider();

        private readonly ConcurrentBag<Type> _primitives = new ConcurrentBag<Type>();
        private readonly ConcurrentDictionary<Type, TypeDescriptor> _typeDescriptors = new ConcurrentDictionary<Type, TypeDescriptor>();
        private readonly ConcurrentDictionary<Type, TypeExtender> _typeExtenders = new ConcurrentDictionary<Type, TypeExtender>();
        private readonly ConcurrentDictionary<Type, Func<ScriptObject, object>> _ctors = new ConcurrentDictionary<Type, Func<ScriptObject, object>>();
        private readonly ConcurrentDictionary<Type, Type> _proxyTypes = new ConcurrentDictionary<Type, Type>();

        private readonly Type[] _numericOrder = new[]
        {
            typeof(float),
            typeof(int)
        };

        public TypeProvider()
        {
            RegisterPrimitive(typeof(string));
            RegisterPrimitive(typeof(float));
            RegisterPrimitive(typeof(int));
            RegisterPrimitive(typeof(bool));

            RegisterExtensions(typeof(StringOperators));
            RegisterExtensions(typeof(FloatOperators));
            RegisterExtensions(typeof(Int32Operators));
            RegisterExtensions(typeof(BooleanOperators));
            RegisterExtensions(typeof(FallbackOperators));

            RegisterExtensions(typeof(StringExtensions));
            RegisterExtensions(typeof(FloatExtensions));
            RegisterExtensions(typeof(Int32Extensions));
            RegisterExtensions(typeof(BooleanExtensions));
            RegisterExtensions(typeof(FallbackExtensions));
        }

        public bool TryGetMethod(Type type, string name, out ScriptableMethodInfo methodInfo)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            methodInfo = GetDescriptor(type, d => d.GetMethod(name));

            return methodInfo != null;
        }

        public IEnumerable<ScriptableMethodInfo> GetMethods(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            return FindTypeDescriptor(type).GetMethods();
        }

        public bool HasMethod(Type type, string name)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            return GetDescriptor(type, d => d.HasMethod(name));
        }

        public bool TryGetProperty(Type type, string name, out PropertyInfo propertyInfo)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            propertyInfo = GetDescriptor(type, d => d.GetProperty(name));

            return propertyInfo != null;
        }

        public bool HasProperty(Type type, string name)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            return GetDescriptor(type, d => d.HasProperty(name));
        }

        public bool TryGetExtensionMethod(Type type, string name, out MethodInfo methodInfo)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            methodInfo = GetExtender(type, e => e.GetExtensionMethod(name))
                ?? GetExtender(typeof(object), e => e.GetExtensionMethod(name));

            return methodInfo != null;
        }

        public bool HasMember(Type type, string name)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            return GetDescriptor(type, d => d.HasMethod(name) || d.HasProperty(name))
                   || GetExtender(type, e => e.HasExtensionMethod(name))
                   || GetExtender(typeof(object), e => e.HasExtensionMethod(name));
        }

        public bool TryGetIndex(Type type, Type indexType, out PropertyInfo propertyInfo)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            propertyInfo = GetDescriptor(type, d => d.GetIndex(indexType));

            return propertyInfo != null;
        }

        public bool HasIndex(Type type, Type indexType)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            return GetDescriptor(type, d => d.HasIndex(indexType));
        }

        public bool TryGetBinaryOperator(Type left, ScriptableBinaryOperater operation, Type right, out MethodInfo methodInfo)
        {
            methodInfo = GetExtender(left ?? throw new ArgumentNullException(nameof(left)),
                e => e.GetBinaryOperator(operation,
                right ?? throw new ArgumentNullException(nameof(right))));

            return methodInfo != null;
        }

        public bool TryGetTypeOperator(Type type, ScriptableTypeOperator operation, out MethodInfo methodInfo)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            methodInfo = GetExtender(type, e => e.GetTypeOperator(operation));

            return methodInfo != null;
        }

        public Func<ScriptObject, object> GetCtor(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            if (!_ctors.TryGetValue(type, out Func<ScriptObject, object> ctor))
            {
                _ctors[type] = ctor = ScriptableCtorHelper.GetCtor(type);
            }

            return ctor;
        }

        public string GetTypeName(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            return FindTypeDescriptor(type).Name;
        }

        public void RegisterPrimitive(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            if (_primitives.Contains(type))
                return;

            _primitives.Add(type);
        }

        public void RegisterPrimitive<T>(ScriptPrimitive<T> primitive)
        {
            primitive = primitive ?? throw new ArgumentNullException(nameof(primitive));

            var primitiveType = typeof(T);

            if (_primitives.Contains(primitiveType))
                return;

            _primitives.Add(primitiveType);
        }

        public void
        RegisterExtensions(Type extensionType)
        {
            extensionType = extensionType ?? throw new ArgumentNullException(nameof(extensionType));


            if (!extensionType.IsExtensionType())
                throw new ArgumentException("Type must be an extension type", "extensionType");

            var scriptableExtensions = from m in extensionType.GetTypeInfo().DeclaredMethods
                                       let a = m.GetCustomAttribute<ScriptableBaseAttribute>()
                                       where a != null
                                       select new
                                       {
                                           Method = m,
                                           Attribute = a
                                       };

            foreach (var details in scriptableExtensions)
            {
                RegisterExtension(details.Method, details.Attribute);
            }
        }

        public IEnumerable<Type> GetPrimitives()
        {
            return _primitives;
        }

        public bool IsPrimitive(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            return _primitives.Contains(type);
        }

        public IEnumerable<string> GetMemberNames(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            return GetDescriptor(type, d => d.GetProperties().Union(d.GetMethodNames()))
                .Union(GetExtender(type, e => e.GetExtensionMethodsNames()));
        }

        public bool TryGetConvertOperator(Type fromType, Type toType, out MethodInfo methodInfo)
        {
            fromType = fromType ?? throw new ArgumentNullException(nameof(fromType));
            toType = toType ?? throw new ArgumentNullException(nameof(toType));

            methodInfo = GetExtender(fromType, e => e.GetConvertOperator(toType))
                ?? GetExtender(typeof(object), e => e.GetConvertOperator(toType))
                ?? GetDescriptor(fromType, d => d.GetConvert(toType));

            return methodInfo != null;
        }

        public DynamicMetaObject GetMetaObject(Expression expression, object primitive)
        {
            expression = expression ?? throw new ArgumentNullException(nameof(expression));

            if (primitive == null)
                return new MetaPrimitive(expression, primitive);

            if (expression.Type.IsGenericType && expression.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var wrappedType = expression.Type.GetGenericArguments()[0];
                expression = Expression.Convert(expression, wrappedType);
            }

            return primitive != null && _primitives.Contains(primitive.GetType())
                ? new MetaPrimitive(expression, primitive)
                : null;
        }

        public Type GetHighestNumericType(params Type[] types)
        {
            types = types ?? throw new ArgumentNullException(nameof(types));

            return _numericOrder
                .Where(t => types.Any(c => c == t))
                .Select((t, i) => new { t, i })
                .OrderBy(q => q.i)
                .Select(q => q.t)
                .FirstOrDefault();
        }

        public bool IsNumericType(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            return _numericOrder.Contains(type);
        }

        public Type GetProxy(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            if (!_proxyTypes.TryGetValue(type, out Type proxyType))
            {
                _proxyTypes[type] = proxyType = TypeFactory.Current.CreateScriptableType(type);
            }

            return proxyType;
        }

        private TypeDescriptor FindTypeDescriptor(Type type)
        {
            if (!_typeDescriptors.TryGetValue(type, out TypeDescriptor typeDescriptor))
            {
                _typeDescriptors[type] = typeDescriptor = new TypeDescriptor(type);
            }

            return typeDescriptor;
        }

        private TypeExtender FindTypeExtender(Type type)
        {
            if (!_typeExtenders.TryGetValue(type, out TypeExtender typeExtender))
            {
                _typeExtenders[type] = typeExtender = new TypeExtender(type);
            }

            return typeExtender;
        }

        private T GetDescriptor<T>(Type type, Func<TypeDescriptor, T> func)
        {
            var typeDescriptor = FindTypeDescriptor(type);
            return func(typeDescriptor);
        }

        private T GetExtender<T>(Type type, Func<TypeExtender, T> func)
        {
            var typeExtender = FindTypeExtender(type);
            return func(typeExtender);
        }

        private void RegisterExtension(MethodInfo methodInfo, ScriptableBaseAttribute attribute)
        {
            var parameterInfos = methodInfo.GetParameters();

            if (parameterInfos.Length == 0)
                throw new InvalidOperationException(string.Format("Method {0} must have 1 or more parameters to be used as type extension", methodInfo.Name));

            var typeExtender = FindTypeExtender(parameterInfos[0].ParameterType);

            if (attribute is ScriptableBinaryOperationAttribute binaryOp)
                typeExtender.RegisterBinaryOperator(binaryOp.Operation, methodInfo);

            if (attribute is ScriptableTypeOperationAttribute typeOp)
                typeExtender.RegisterTypeOperator(typeOp.Operation, methodInfo);

            if (attribute is ScriptableConvertOperationAttribute convertOp)
                typeExtender.RegisterConvertOperator(methodInfo);

            if (attribute is ScriptableMethodAttribute method)
                typeExtender.RegisterExtensionMethod(method.Name, methodInfo);
        }
    }
}
