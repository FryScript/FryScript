using FryScript.Binders;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace FryScript.HostInterop
{
    public class TypeFactory : ITypeFactory
    {
        private class TypeContext
        {
            public string ScriptName;
            public Type BaseType;
            public TypeBuilder NewType;
            public TypeInfo TypeInfo;
            public readonly Dictionary<string, FieldBuilder> NewFields = new Dictionary<string, FieldBuilder>();
            public readonly Dictionary<string, MethodBuilder> NewMethods = new Dictionary<string, MethodBuilder>();
            public readonly Dictionary<string, PropertyBuilder> NewProperties = new Dictionary<string, PropertyBuilder>();
            public readonly Dictionary<Type, string> ConvertCallSites = new Dictionary<Type, string>();
            public readonly Dictionary<string, Tuple<string, string, int>> InvokeCallSites = new Dictionary<string, Tuple<string, string, int>>();
        }

        public readonly AssemblyBuilder _assemblyBuilder;
        public readonly ModuleBuilder _moduleBuilder;

        public static TypeFactory Current { get; set; }

        static TypeFactory()
        {
            Current = new TypeFactory();
        }

        public TypeFactory()
        {
            _assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("FryScript.Runtime"), AssemblyBuilderAccess.RunAndCollect);
            _moduleBuilder = _assemblyBuilder.DefineDynamicModule("FryScript.Runtime.dll");
        }

        public Type CreateScriptableType(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            if (!type.IsPublic && !type.IsNestedPublic)
                throw new ArgumentException($"Type {type.FullName} must be a public type", nameof(type));

            if (type.IsInterface)
                throw new ArgumentException($"Cannot create a scriptable type from an interface type", nameof(type));

            var typeInfo = type.GetTypeInfo();

            var scriptableTypeAttribute = typeInfo.GetCustomAttributes<ScriptableTypeAttribute>().SingleOrDefault();

            if (scriptableTypeAttribute == null)
                throw new ArgumentException($"Type {type.FullName} must be decorated with a {typeof(ScriptableTypeAttribute).FullName}", nameof(type));

            var context = new TypeContext
            {
                ScriptName = scriptableTypeAttribute.Name,
                BaseType = type,
                NewType = _moduleBuilder.DefineType($"Runtime.{type.FullName}.{Guid.NewGuid()}", TypeAttributes.Public, type),
                TypeInfo = typeInfo
            };

            ImplementInterface<IScriptable>(context);
            ImplementInterface<IScriptObject>(context);
            OverrideVirtualMethods(context);
            DefineStaticCtor(context);
            DefineParameterlessConstructor(context);

            var newType = context.NewType.CreateTypeInfo().AsType();

            //_assemblyBuilder.Save("FryScript.Runtime.dll");

            return newType;
        }

        private static void ImplementInterface<T>(TypeContext context)
        {
            var typeInfo = typeof(T).GetTypeInfo();

            if (context.TypeInfo.ImplementedInterfaces.Any(t => t == typeof(T)))
                return;

            context.NewType.AddInterfaceImplementation(typeof(T));

            var methodAttribs = MethodAttributes.Public
            | MethodAttributes.Virtual
            | MethodAttributes.Final
            | MethodAttributes.SpecialName
            | MethodAttributes.HideBySig
            | MethodAttributes.NewSlot;

            typeInfo
                .DeclaredProperties
                .ToList()
                .ForEach(p =>
                {
                    var property = context.NewProperties[p.Name] = context.NewType.DefineProperty(p.Name, PropertyAttributes.None, p.PropertyType, Type.EmptyTypes);

                    var fieldName = $"<FryScript>{p.Name}";
                    var field = context.NewFields[fieldName] = context.NewType.DefineField(fieldName, p.PropertyType, FieldAttributes.Private);

                    if (p.CanRead)
                    {
                        var methodName = $"get_{p.Name}";
                        var getMethod = context.NewMethods[methodName] = context.NewType.DefineMethod(methodName, methodAttribs, p.PropertyType, Type.EmptyTypes);
                        var il = getMethod.GetILGenerator();
                        il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Ldfld, field);
                        il.Emit(OpCodes.Ret);

                        property.SetGetMethod(getMethod);
                    }

                    if (p.CanWrite)
                    {
                        var methodName = $"set_{p.Name}";
                        var setMethod = context.NewMethods[methodName] = context.NewType.DefineMethod(methodName, methodAttribs, typeof(void), new[] { p.PropertyType });
                        var il = setMethod.GetILGenerator();
                        il.Emit(OpCodes.Ldarg_0);
                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Stfld, field);
                        il.Emit(OpCodes.Ret);

                        property.SetSetMethod(setMethod);
                    }
                });

            if (typeof(IDynamicMetaObjectProvider).IsAssignableFrom(typeof(T)))
            {
                var method = context.NewType.DefineMethod(
                    nameof(IDynamicMetaObjectProvider.GetMetaObject),
                    methodAttribs,
                    typeof(DynamicMetaObject),
                    new[]{
                        typeof(Expression)
                    });
                var il = method.GetILGenerator();
                il.Emit(OpCodes.Nop);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ldsfld, typeof(BindingRestrictions).GetTypeInfo().DeclaredFields.First(f => f.Name == nameof(BindingRestrictions.Empty)));
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Newobj, typeof(MetaScriptObject).GetTypeInfo().GetConstructor(new[] { typeof(Expression), typeof(BindingRestrictions), typeof(object) }));
                il.Emit(OpCodes.Ret);
            }
        }

        private static void OverrideVirtualMethods(TypeContext context)
        {
            context
                .TypeInfo
                .DeclaredMethods
                .Select(m => new
                {
                    N = m.GetCustomAttribute<ScriptableMethodAttribute>()?.Name,
                    M = m
                })
                .Where(q => q.M.IsVirtual && q.M.IsPublic && q.N != null)
                .ToList()
                .ForEach(q =>
                {
                    OverrideVirtualMethod(q.N, q.M, context);
                    ProxyBaseMethod(q.N, q.M, context);
                });
        }

        private static void OverrideVirtualMethod(string scriptName, MethodInfo baseMethodInfo, TypeContext context)
        {
            GetConvertCallSite(baseMethodInfo, context);
            GetInvokeCallSite(scriptName, baseMethodInfo, context);

            var methodOverride = context.NewType.DefineMethod(
                baseMethodInfo.Name,
                MethodAttributes.Public
                | MethodAttributes.HideBySig
                | MethodAttributes.Virtual,
                baseMethodInfo.ReturnType,
                baseMethodInfo.GetParameters().Select(p => p.ParameterType).ToArray());

            var baseParameters = baseMethodInfo.GetParameters();

            baseParameters.Select((p, i) => new
            {
                P = p,
                I = i
            })
            .ToList()
            .ForEach(q =>
            {
                methodOverride.DefineParameter(q.I + 1, ParameterAttributes.None, q.P.Name);
            });

            var il = methodOverride.GetILGenerator();

            FieldBuilder convertField = null;
            if (baseMethodInfo.ReturnType != typeof(void))
            {
                var convertFieldName = context.ConvertCallSites[baseMethodInfo.ReturnType];
                convertField = context.NewFields[convertFieldName];


                il.Emit(OpCodes.Ldsfld, convertField);

                var convertTargetField = convertField.FieldType.GetField("Target");
                il.Emit(OpCodes.Ldfld, convertTargetField);
                il.Emit(OpCodes.Ldsfld, convertField);
            }

            var invokeFieldName = context.InvokeCallSites[baseMethodInfo.Name].Item2;
            var invokeField = context.NewFields[invokeFieldName];

            il.Emit(OpCodes.Ldsfld, invokeField);

            var invokeTargetField = invokeField.FieldType.GetField("Target");
            il.Emit(OpCodes.Ldfld, invokeTargetField);
            il.Emit(OpCodes.Ldsfld, invokeField);

            //il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_0);
            //il.Emit(OpCodes.Call, context.NewProperties["Script"].GetGetMethod());

            baseParameters
                .Select((p, i) => new
                {
                    I = i + 1,
                    T = p.ParameterType
                })
                .ToList()
                .ForEach(q =>
                {
                    il.Emit(OpCodes.Ldarg, q.I);

                    if (q.T.IsValueType)
                        il.Emit(OpCodes.Box, q.T);
                });

            var invokeDelegateType = invokeField.FieldType.GetGenericArguments()[0];
            var invokeInvokeMethod = invokeDelegateType.GetMethod("Invoke");
            il.Emit(OpCodes.Callvirt, invokeInvokeMethod);

            if (baseMethodInfo.ReturnType != typeof(void))
            {
                var convertDelegateType = convertField.FieldType.GetGenericArguments()[0];
                var convertInvokeMethod = convertDelegateType.GetMethod("Invoke");
                il.Emit(OpCodes.Callvirt, convertInvokeMethod);
            }

            if (baseMethodInfo.ReturnType == typeof(void))
            {
                il.Emit(OpCodes.Pop);
            }

            il.Emit(OpCodes.Ret);
        }

        private static void ProxyBaseMethod(string scriptName, MethodInfo baseMethodInfo, TypeContext context)
        {
            var parameters = baseMethodInfo.GetParameters().Select((p, i) => new
            {
                P = p,
                I = i + 1
            })
                .ToList();

            var proxyMethodName = $"<FryScript><{baseMethodInfo.Name}>_proxy";
            var proxyMethod = context.NewType.DefineMethod(
                proxyMethodName,
                MethodAttributes.Public
                | MethodAttributes.Final,
                baseMethodInfo.ReturnType,
                parameters.Select(p => p.P.ParameterType).ToArray());

            parameters
                .ForEach(q =>
                {
                    proxyMethod.DefineParameter(q.I, ParameterAttributes.None, q.P.Name);
                });

            var runtimeOverrideAttr = new CustomAttributeBuilder(
                typeof(RuntimeOverrideAttribute).GetTypeInfo().GetConstructor(new[] { typeof(string) }),
                new object[] { scriptName }
                );

            proxyMethod.SetCustomAttribute(runtimeOverrideAttr);

            var il = proxyMethod.GetILGenerator();

            if (baseMethodInfo.IsAbstract)
            {
                var exCtor = typeof(NotImplementedException).GetTypeInfo().GetConstructor(Type.EmptyTypes);
                il.Emit(OpCodes.Newobj, exCtor);
                il.Emit(OpCodes.Throw);
                return;
            }

            il.Emit(OpCodes.Ldarg_0);

            parameters
                .ForEach(q =>
                {
                    il.Emit(OpCodes.Ldarg, q.I);
                });

            il.Emit(OpCodes.Call, baseMethodInfo);
            il.Emit(OpCodes.Ret);


        }

        private static FieldInfo GetConvertCallSite(MethodInfo methodInfo, TypeContext context)
        {
            if (methodInfo.ReturnType == typeof(void) || context.ConvertCallSites.ContainsKey(methodInfo.ReturnType))
                return null;

            var callSiteName = $"<FryScript><{methodInfo.ReturnType.FullName}>_convert";

            var callSiteDelType = Expression.GetFuncType(new[] { typeof(CallSite), typeof(object), methodInfo.ReturnType });
            var callSiteType = typeof(CallSite<>).MakeGenericType(callSiteDelType);

            var field = context.NewFields[callSiteName] = context.NewType.DefineField(callSiteName, callSiteType, FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly);

            context.ConvertCallSites[methodInfo.ReturnType] = callSiteName;

            return field;
        }

        private static FieldInfo GetInvokeCallSite(string scriptName, MethodInfo baseMethodInfo, TypeContext context)
        {
            var callSiteName = $"<FryScript><{baseMethodInfo.Name}>_invoke";

            var types = new[] { typeof(CallSite), typeof(object) }
            .Concat(baseMethodInfo
                .GetParameters()
                .Select(p => typeof(object)))
            .Concat(new[]{
                typeof(object)
            })
            .ToArray();

            var callSiteDelType = Expression.GetFuncType(types);
            var callSiteType = typeof(CallSite<>).MakeGenericType(callSiteDelType);

            var field = context.NewFields[callSiteName] = context.NewType.DefineField(callSiteName, callSiteType, FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.InitOnly);

            context.InvokeCallSites[baseMethodInfo.Name] = new Tuple<string, string, int>(scriptName, callSiteName, types.Length - 1);

            return field;
        }

        private static void DefineParameterlessConstructor(TypeContext context)
        {
            var ctor = context.NewType.DefineConstructor(
                MethodAttributes.Public
                | MethodAttributes.HideBySig
                | MethodAttributes.SpecialName
                | MethodAttributes.RTSpecialName,
                CallingConventions.Standard,
                Type.EmptyTypes);

            var paramterlessCtor = context.TypeInfo.GetConstructor(Type.EmptyTypes);

            var il = ctor.GetILGenerator();
            if (context.NewType.ImplementedInterfaces.Any(i => i == typeof(IScriptObject)))
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Newobj, typeof(ObjectCore).GetTypeInfo().GetConstructor(new Type[0]));
                il.Emit(OpCodes.Stfld, context.NewFields.First(f => f.Value.FieldType == typeof(ObjectCore)).Value);
            }
            if (paramterlessCtor != null)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, paramterlessCtor);
            }
            il.Emit(OpCodes.Ret);
        }

        private static void DefineStaticCtor(TypeContext context)
        {


            var staticCtor = context.NewType.DefineConstructor(
                MethodAttributes.HideBySig
                | MethodAttributes.SpecialName
                | MethodAttributes.RTSpecialName
                | MethodAttributes.Static,
                CallingConventions.Standard,
                Type.EmptyTypes);

            var il = staticCtor.GetILGenerator();

            var binderCacheCurrentField = typeof(BinderCache).GetTypeInfo().GetField("Current");

            context
                .ConvertCallSites
                .ToList()
                .ForEach(kvp =>
                {
                    il.Emit(OpCodes.Ldsfld, binderCacheCurrentField);

                    var callSiteName = kvp.Value;
                    var callSiteField = context.NewFields[callSiteName];

                    var convertType = kvp.Key;
                    var getTypeFromHandleMethod = typeof(Type).GetTypeInfo().GetMethod("GetTypeFromHandle");
                    il.Emit(OpCodes.Ldtoken, convertType);
                    il.Emit(OpCodes.Call, getTypeFromHandleMethod);

                    var binderCacheConvertMethod = typeof(BinderCache).GetTypeInfo().GetMethod("ConvertBinder");
                    il.Emit(OpCodes.Callvirt, binderCacheConvertMethod);

                    var callSiteCreateMethod = callSiteField.FieldType.GetTypeInfo().GetMethod("Create");
                    il.Emit(OpCodes.Call, callSiteCreateMethod);
                    il.Emit(OpCodes.Stsfld, callSiteField);
                });

            context
                .InvokeCallSites
                .ToList()
                .ForEach(kvp =>
                {
                    il.Emit(OpCodes.Ldsfld, binderCacheCurrentField);

                    var callSiteName = kvp.Value.Item2;
                    var callSiteField = context.NewFields[callSiteName];

                    il.Emit(OpCodes.Ldstr, kvp.Value.Item1);
                    il.Emit(OpCodes.Ldc_I4, kvp.Value.Item3);

                    var binderCacheInvokeMemberMethod = typeof(BinderCache).GetTypeInfo().GetMethod("InvokeMemberBinder");
                    il.Emit(OpCodes.Callvirt, binderCacheInvokeMemberMethod);

                    var callSiteCreateMethod = callSiteField.FieldType.GetTypeInfo().GetMethod("Create");
                    il.Emit(OpCodes.Call, callSiteCreateMethod);
                    il.Emit(OpCodes.Stsfld, callSiteField);
                });

            il.Emit(OpCodes.Ret);
        }
    }
}
