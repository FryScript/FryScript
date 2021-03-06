﻿using FryScript.Ast;
using FryScript.Binders;
using FryScript.Compilation;
using FryScript.HostInterop;
using FryScript.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FryScript.Helpers
{
    public static class ExpressionHelper
    {
        public static Expression Null(Type type)
        {
            type = type ?? throw new ArgumentNullException(nameof(type));

            if (type.IsValueType)
                return Expression.Default(type);

            return Expression.Constant(null, type);
        }

        public static Expression Null()
        {
            return Null(typeof(object));
        }

        public static Expression DynamicConvert(Expression expression, Type type)
        {
            expression = expression ?? throw new ArgumentNullException(nameof(expression));
            type = type ?? throw new ArgumentNullException(nameof(expression));

            var binder = BinderCache.Current.ConvertBinder(type);
            return Expression.Dynamic(binder, type, expression);
        }

        public static Expression DynamicSetMember(string name, Expression instance, Expression value)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));
            instance = instance ?? throw new ArgumentNullException(nameof(instance));
            value = value ?? throw new ArgumentNullException(nameof(value));

            var binder = BinderCache.Current.SetMemberBinder(name);
            return Expression.Dynamic(binder, typeof(object), instance, value);
        }

        public static Expression DynamicGetMember(string name, Expression instance)
        {
            name = name ?? throw new ArgumentNullException(nameof(name));
            instance = instance ?? throw new ArgumentNullException(nameof(instance));

            var binder = BinderCache.Current.GetMemberBinder(name);
            return Expression.Dynamic(binder, typeof(object), instance);
        }

        public static Expression DynamicSetIndex(Expression instance, Expression value, params Expression[] indexes)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance));
            value = value ?? throw new ArgumentNullException(nameof(value));
#if NET452
            indexes ??= new Expression[0];
#else
            indexes ??= Array.Empty<Expression>();
#endif

            var binder = BinderCache.Current.SetIndexBinder(indexes.Length);
            var args = new List<Expression> { instance };
            args.AddRange(indexes);
            args.Add(value);
            return Expression.Dynamic(binder, typeof(object), args);
        }

        public static Expression DynamicGetIndex(Expression instance, params Expression[] indexes)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance));
#if NET452
            indexes ??= new Expression[0];
#else
            indexes ??= Array.Empty<Expression>();
#endif

            var binder = BinderCache.Current.GetIndexBinder(indexes.Length);
            var args = new List<Expression> { instance };
            args.AddRange(indexes);
            return Expression.Dynamic(binder, typeof(object), args);
        }

        public static Expression DynamicInvoke(Expression instance, params Expression[] args)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance));
#if NET452
            args ??= new Expression[0];
#else
            args ??= Array.Empty<Expression>();
#endif

            var count = args == null
                ? 0
                : args.Length;
            var binder = BinderCache.Current.InvokeBinder(count);
            var args2 = new List<Expression> { instance };
            args2.AddRange(args);

            return Expression.Dynamic(binder, typeof(object), args2);
        }

        public static Expression DynamicBinaryOperation(Expression left, ExpressionType operation, Expression right)
        {
            left = left ?? throw new ArgumentNullException(nameof(left));
            right = right ?? throw new ArgumentNullException(nameof(right));

            var binder = BinderCache.Current.BinaryOperationBinder(operation);
            return Expression.Dynamic(binder, typeof(object), left, right);
        }

        public static Expression DynamicInvokeMember(Expression instance, string name, params Expression[] args)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance));
            name = name ?? throw new ArgumentNullException(nameof(name));
#if NET452
            args ??= new Expression[0];
#else
            args ??= Array.Empty<Expression>();
#endif

            var binder = BinderCache.Current.InvokeMemberBinder(name, args.Length);
            var args2 = new List<Expression> { instance };
            args2.AddRange(args);
            return Expression.Dynamic(binder, typeof(object), args2);
        }

        public static Expression DynamicCreateInstance(Expression instance, params Expression[] args)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance));
#if NET452
            args ??= new Expression[0];
#else
            args ??= Array.Empty<Expression>();
#endif

            var binder = BinderCache.Current.CreateInstanceBinder(args.Length);
            var args2 = new List<Expression> { instance };
            args2.AddRange(args);
            return Expression.Dynamic(binder, typeof(object), args2);
        }

        public static Expression DynamicIsOperation(Expression instance, Expression value)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance));
            value = value ?? throw new ArgumentNullException(nameof(value));

            var binder = BinderCache.Current.IsOperationBinder();
            return Expression.Dynamic(binder, typeof(object), instance, value);
        }

        public static bool TryGetMethodExpression(Expression instance, string name, out Expression expression)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance));
            name = name ?? throw new ArgumentNullException(nameof(name));

            expression = null;
            if (TypeProvider.Current.TryGetMethod(instance.Type, name, out ScriptableMethodInfo scriptMethod))
            {
                expression = ScriptableMethodHelper.CreateMethod(instance, scriptMethod.Method);
            }
            else if (TypeProvider.Current.TryGetExtensionMethod(instance.Type, name, out MethodInfo methodInfo))
            {
                expression = ScriptableMethodHelper.CreateExtensionMethod(instance, methodInfo);
            }

            return expression != null;
        }

        public static Expression AwaitExpression(AstNode node, Scope scope)
        {
            node = node ?? throw new ArgumentNullException(nameof(node));
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (!scope.TryGetData(ScopeData.AwaitContexts, out List<Expression> awaitContexts) ||
                !scope.TryGetData(ScopeData.FibreContext, out ParameterExpression fibreContextExpr) ||
                !scope.TryGetData(ScopeData.YieldLabels, out List<LabelTarget> yieldLabels) ||
                !scope.TryGetData(ScopeData.YieldTarget, out LabelTarget yieldTarget))
                throw ExceptionHelper.InvalidContext(Keywords.Await, node);

            var targetNode = node.ChildNodes.Skip(1).First();

            var targetExpr = targetNode.GetExpression(scope);
            var awaitExpr = Expression.Call(typeof(AwaitHelper), nameof(AwaitHelper.EnsureAwait), null, targetExpr);

            var awaitContextExpr = scope.AddTempMember(TempPrefix.Await, node, typeof(ScriptFibreContext));
            var assignAwaitContextExpr = Expression.Assign(awaitContextExpr, awaitExpr);
            var callContextAwaitExpr = Expression.Call(fibreContextExpr, nameof(ScriptFibreContext.Await), null, assignAwaitContextExpr);
            var awaitContextResultExpr = Expression.Property(awaitContextExpr, nameof(ScriptFibreContext.Result));

            BlockExpression yieldBlock = GetYieldExpression(fibreContextExpr,
                yieldLabels,
                yieldTarget,
                callContextAwaitExpr,
                scope.GetTempName(TempPrefix.Yield));

            awaitContexts.Add(yieldBlock);

            return awaitContextResultExpr;
        }

        public static Expression AwaitStatement(Scope scope, Expression statementExpr = null)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            scope.TryGetData(ScopeData.AwaitContexts, out List<Expression> awaitContexts);

            if (statementExpr != null)
                awaitContexts.Add(statementExpr);

            var awaitingExprs = awaitContexts.ToArray();

            awaitContexts.Clear();

            return Expression.Block(typeof(object), awaitingExprs);
        }

        private static BlockExpression GetYieldExpression(ParameterExpression fibreContextExpr, List<LabelTarget> yieldLabels, LabelTarget yieldTarget, MethodCallExpression callContextAwaitExpr, string labelPrefix)
        {
            var yieldStateExpr = Expression.Constant(yieldLabels.Count);

            var yieldLabel = Expression.Label(typeof(void), labelPrefix);

            var assignYieldStateExpr = Expression.Assign(
            Expression.Property(fibreContextExpr, nameof(ScriptFibreContext.YieldState)),
            yieldStateExpr);

            var returnExpr = Expression.Return(yieldTarget, Expression.Constant(ScriptFibreContext.NoResult), typeof(object));
            var yieldBlock = Expression.Block(typeof(object),
                assignYieldStateExpr,
                callContextAwaitExpr,
                returnExpr,
                Expression.Label(yieldLabel),
                ExpressionHelper.Null());

            yieldLabels.Add(yieldLabel);
            return yieldBlock;
        }
    }
}
