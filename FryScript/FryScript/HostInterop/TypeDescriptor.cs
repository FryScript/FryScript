using FryScript.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FryScript.HostInterop
{

    public class TypeDescriptor
    {
        private readonly string _name;
        private readonly Type _describedType;
        private readonly ConcurrentDictionary<string, ScriptableMethodInfo> _methods = new ConcurrentDictionary<string, ScriptableMethodInfo>();
        private readonly ConcurrentDictionary<string, PropertyInfo> _properties = new ConcurrentDictionary<string, PropertyInfo>();
        private readonly ConcurrentDictionary<Type, PropertyInfo> _indexes = new ConcurrentDictionary<Type, PropertyInfo>();
        private readonly ConcurrentDictionary<Type, MethodInfo> _converts = new ConcurrentDictionary<Type, MethodInfo>();

        public string Name => _name;

        public TypeDescriptor(Type type)
        {
            _describedType = type ?? throw new ArgumentNullException("type");

            AnalyseType(_describedType, ref _name);
        }

        public ScriptableMethodInfo GetMethod(string name)
        {
            return GetValue(_methods, name ?? throw new ArgumentNullException(nameof(name)));
        }

        public bool HasMethod(string name)
        {
            return _methods.ContainsKey(name ?? throw new ArgumentNullException(nameof(name)));
        }

        public PropertyInfo GetProperty(string name)
        {
            return GetValue(_properties, name ?? throw new ArgumentNullException(nameof(name)));
        }

        public bool HasProperty(string name)
        {
            return _properties.ContainsKey(name ?? throw new ArgumentNullException(nameof(name)));
        }

        public PropertyInfo GetIndex(Type indexType)
        {
            return GetValue(_indexes, indexType ?? throw new ArgumentNullException(nameof(indexType)));
        }

        public bool HasIndex(Type indexType)
        {
            return _indexes.ContainsKey(indexType ?? throw new ArgumentNullException(nameof(indexType)));
        }

        public IEnumerable<string> GetProperties()
        {
            return _properties.Keys;
        }

        public IEnumerable<string> GetMethodNames()
        {
            return _methods.Keys;
        }

        public IEnumerable<ScriptableMethodInfo> GetMethods()
        {
            return _methods.Values;
        }

        public MethodInfo GetConvert(Type toType)
        {
            if(!_converts.TryGetValue(toType, out MethodInfo method))
            {
                _describedType.TryCanConvert(toType, out method);
                _converts[toType] = method;
            }

            return method;
        }

        private void AnalyseType(Type type, ref string name)
        {
            name = type.GetTypeInfo().GetCustomAttribute<ScriptableTypeAttribute>()?.Name;

            var scriptableMembers = from m in type.GetTypeInfo().GetRuntimeMethods().Cast<MemberInfo>()
                                    .Union(type.GetTypeInfo().GetRuntimeProperties().Cast<MemberInfo>())
                let a = m.GetCustomAttributes(typeof (ScriptableBaseAttribute), true).Cast<ScriptableBaseAttribute>().SingleOrDefault()
                where a != null
                select new
                {
                    Member = m,
                    Attribute = a
                };

            foreach (var details in scriptableMembers)
            {
                AddMember(details.Member, details.Attribute);
            }
        }

        private void AddMember(MemberInfo memberInfo, ScriptableBaseAttribute attribute)
        {
            var attributeType = attribute.GetType();
            
            if(attributeType == typeof(ScriptableMethodAttribute))
                AddMethod(memberInfo as MethodInfo, attribute as ScriptableMethodAttribute);

            if(attributeType == typeof(ScriptablePropertyAttribute))
                AddProperty(memberInfo as PropertyInfo, attribute as ScriptablePropertyAttribute);

            if(attributeType == typeof(ScriptableIndexAttribute))
                AddIndex(memberInfo as PropertyInfo, attribute as ScriptableIndexAttribute);

            if(attributeType == typeof(RuntimeOverrideAttribute))
            {
                _methods[attribute.Name].Method = memberInfo as MethodInfo;
            }
        }

        private void AddMethod(MethodInfo methodInfo, ScriptableMethodAttribute attribute)
        {
            if (methodInfo.IsStatic)
                throw new InvalidOperationException($"Type {_describedType.FullName} cannot declare a static method as a scriptable method named {attribute.Name}");

            if (_methods.ContainsKey(attribute.Name))
                throw new InvalidOperationException($"Type {_describedType.FullName} declares multiple scriptable methods named {attribute.Name}");

            _methods[attribute.Name] = new ScriptableMethodInfo
            {
                Name = attribute.Name,
                Method = methodInfo
            };
        }

        private void AddProperty(PropertyInfo propertyInfo, ScriptablePropertyAttribute attribute)
        {
            if (propertyInfo.GetAccessors().Any(m => m.IsStatic))
                throw new InvalidOperationException($"Type {_describedType.FullName} cannot declare a static property as a scriptable property named {attribute.Name}");

            if (_properties.ContainsKey(attribute.Name))
                throw new InvalidOperationException($"Type {_describedType.FullName} declares multiple scriptable properties named {attribute.Name}");

            _properties[attribute.Name] = propertyInfo;
        }

        private void AddIndex(PropertyInfo propertyInfo, ScriptableIndexAttribute attribute)
        {
            var indexInfo = propertyInfo.GetIndexParameters().SingleOrDefault();

            if (indexInfo == null)
                throw new InvalidOperationException($"Type {_describedType.FullName} scriptable index with return type {propertyInfo.PropertyType} can only have 1 parameter");

            if (propertyInfo.GetAccessors().Any(m => m.IsStatic))
                throw new InvalidOperationException($"Type {_describedType.FullName} cannot declare a static property as a scriptable property named {attribute.Name}");

            if (_indexes.ContainsKey(indexInfo.ParameterType))
                throw new InvalidOperationException($"Type {_describedType} declares multiple scriptable indexes with parameter type {indexInfo.ParameterType}");

            _indexes[indexInfo.ParameterType] = propertyInfo;
        }

        private static TValue GetValue<TKey, TValue>(ConcurrentDictionary<TKey, TValue> dictionary, TKey key)
        {
            dictionary.TryGetValue(key, out TValue value);
            return value;
        }
    }
}
