using FryScript.Helpers;
using FryScript.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FryScript.HostInterop
{
    public static class ScriptableCtorHelper
    {
        private static readonly ConstructorInfo ScriptFunction_DelegateCtor =
            typeof(ScriptFunction).GetConstructor(new[] { typeof(Delegate) });

        public static Func<ScriptObject, object> GetCtor(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            //return CreateNativeCtor(type);

            return new Func<ScriptObject, object>(s => s);
        }

        public static Func<ScriptObject, object> GetCtor<T>()
            where T : class, IScriptable, new()
        {
            return GetCtor(typeof (T));
        }

        public static Func<ScriptObject, object> GetCtor<T>(Func<T> ctor)
            where T : class, IScriptable
        {
            ctor = ctor ?? throw new ArgumentNullException(nameof(ctor));

            var type = typeof (T);

            var thisExpr = Expression.Parameter(typeof(ScriptObject), Keywords.This);

            var ctorExpr = Expression.Constant(ctor);
            var invokeCtorExpr = Expression.Invoke(ctorExpr);

            var exprs = new List<Expression> { WrapNativeType(type, thisExpr, invokeCtorExpr) };
            exprs.AddRange(WrapMethods(type, thisExpr));
            exprs.Add(thisExpr);

            var bodyExpr = Expression.Block(exprs);

            var lambdaExpr = Expression.Lambda<Func<ScriptObject, object>>(bodyExpr, thisExpr);

            return lambdaExpr.Compile();
        }

        private static Func<ScriptObject, object> CreateNativeCtor(Type type)
        {
            if (type.GetTypeInfo().ImplementedInterfaces.All(i => i != typeof(IScriptable)))
                throw new ArgumentException(string.Format("Type {0} must implement interface {1}", type.FullName, typeof(IScriptable).FullName), "type");

            var typeCtor =
                type.GetTypeInfo()
                    .DeclaredConstructors.SingleOrDefault(c => c.GetParameters().Length == 0 && c.IsPublic);

            if (typeCtor == null)
                throw new ArgumentException(string.Format("Type {0} must have a public parameterless constructor", type.FullName), "type");

            var thisExpr = Expression.Parameter(typeof(ScriptObject), Keywords.This);

            var newTypeExpr = Expression.New(typeCtor);

            var exprs = new List<Expression> { WrapNativeType(type, thisExpr, newTypeExpr) };
            exprs.AddRange(WrapMethods(type, thisExpr));
            exprs.Add(thisExpr);

            var bodyExpr = Expression.Block(exprs);

            var lambdaExpr = Expression.Lambda<Func<ScriptObject, object>>(bodyExpr, thisExpr);

            return lambdaExpr.Compile();
        }

        private static Expression WrapNativeType(Type type, Expression thisExpr, Expression newTypeExpr)
        {
            var targetExpr = Expression.Convert(
                Expression.PropertyOrField(thisExpr, "Target"),
                type
                );

            var conditionExpr = Expression.Equal(targetExpr, ExpressionHelper.Null());
            var callSetObjectExpr = Expression.Call(thisExpr, "SetTarget", null, newTypeExpr);
            var ifExpr = Expression.IfThenElse(conditionExpr, callSetObjectExpr, thisExpr);

            return ifExpr;
        }

        private static IEnumerable<Expression> WrapMethods(Type type, Expression thisExpr)
        {
            var targetExpr = Expression.Convert(
                Expression.PropertyOrField(thisExpr, "Target"),
                type
                );

            foreach (var info in TypeProvider.Current.GetMethods(type))
            {
                var argExprs = (from parameter in info.Method.GetParameters()
                    select Expression.Parameter(parameter.ParameterType, parameter.Name)).ToArray();

                var callExpr = Expression.Call(targetExpr, info.Method, argExprs);
                var lambdaExpr = Expression.Lambda(callExpr, argExprs);

                var newFuncExpr = Expression.New(ScriptFunction_DelegateCtor, lambdaExpr);

                yield return ExpressionHelper.DynamicSetMember(info.Name, thisExpr, newFuncExpr);
            }
        }
    }
}
