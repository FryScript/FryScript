using FryScript.Binders;
using FryScript.HostInterop;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FryScript.Helpers
{

    public static class BindHelper
    {
        private static readonly ConstructorInfo ScriptParamsObjArrayCtor =
            typeof(ScriptParams).GetConstructor(new[] { typeof(object[]) });

        private static readonly PropertyInfo ScriptParamsObjectIntIndex =
           (from p in typeof(ScriptParams).GetProperties()
            from i in p.GetIndexParameters()
            where i.ParameterType == typeof(int)
                  && p.PropertyType == typeof(object)
            select p).Single();

        private static DynamicMetaObject GetBindBinaryOperation(MethodInfo opInfo, DynamicMetaObject left, ScriptableBinaryOperater op, DynamicMetaObject right)
        {
            var parameters = opInfo.GetParameters();
            var leftExpr = Expression.Convert(left.Expression, parameters[0].ParameterType);
            var rightExpr = Expression.Convert(right.Expression, parameters[1].ParameterType);

            var expr = Expression.Call(opInfo, leftExpr, rightExpr);

            var restrictions = RestrictionsHelper.TypeOrNullRestriction(left)
                .Merge(RestrictionsHelper.TypeOrNullRestriction(right));

            return new DynamicMetaObject(expr, restrictions);
        }

        public static DynamicMetaObject BindBinaryOperation(BinaryOperationBinder binder, DynamicMetaObject target, DynamicMetaObject arg)
        {
            binder = binder ?? throw new ArgumentNullException(nameof(binder));
            target = target ?? throw new ArgumentNullException(nameof(target));
            arg = arg ?? throw new ArgumentNullException(nameof(arg));

            MethodInfo opInfo;
            var leftType = target.LimitType;
            var rightType = arg.LimitType;
            var scriptableOp = (ScriptableBinaryOperater)binder.Operation;

            if (leftType == rightType)
            {
                if (TypeProvider.Current.TryGetBinaryOperator(target.LimitType, scriptableOp, arg.LimitType, out opInfo))
                    return GetBindBinaryOperation(opInfo, target, scriptableOp, arg);

                var convertType = leftType;

                var leftExpr = Expression.Convert(target.Expression, target.LimitType);
                var rightExpr = Expression.Convert(arg.Expression, arg.LimitType);

                var binaryExpr = Expression.MakeBinary(binder.Operation, leftExpr, rightExpr);
                var convertExpr = Expression.Convert(binaryExpr, typeof(object));

                var restrictions = RestrictionsHelper.TypeOrNullRestriction(target)
                    .Merge(RestrictionsHelper.TypeOrNullRestriction(arg));

                return new DynamicMetaObject(convertExpr, restrictions);
            }

            if (leftType != rightType && TypeProvider.Current.IsNumericType(leftType) && TypeProvider.Current.IsNumericType(rightType))
            {
                if (TypeProvider.Current.TryGetBinaryOperator(target.LimitType, scriptableOp, arg.LimitType, out opInfo))
                    return GetBindBinaryOperation(opInfo, target, scriptableOp, arg);

                var convertType = TypeProvider.Current.GetHighestNumericType(leftType, rightType);

                var leftExpr = Expression.Convert(target.Expression, target.LimitType);
                var rightExpr = Expression.Convert(arg.Expression, arg.LimitType);

                if (leftType != convertType)
                    leftExpr = Expression.Convert(leftExpr, convertType);

                if (rightType != convertType)
                    rightExpr = Expression.Convert(rightExpr, convertType);

                var binaryExpr = Expression.MakeBinary(binder.Operation, leftExpr, rightExpr);
                var convertExpr = Expression.Convert(binaryExpr, typeof(object));

                var restrictions = RestrictionsHelper.TypeOrNullRestriction(target)
                    .Merge(RestrictionsHelper.TypeOrNullRestriction(arg));

                return new DynamicMetaObject(convertExpr, restrictions);
            }

            if (TypeProvider.Current.TryGetBinaryOperator(typeof(object), (ScriptableBinaryOperater)binder.Operation, typeof(object), out opInfo))
                return GetBindBinaryOperation(opInfo, target, scriptableOp, arg);

            throw ExceptionHelper.InvalidBinaryOperation(binder.Operation, target.LimitType, arg.LimitType);
        }

        public static DynamicMetaObject BindIsOperation(ScriptIsOperationBinder binder, DynamicMetaObject target, DynamicMetaObject arg)
        {
            binder = binder ?? throw new ArgumentNullException(nameof(binder));
            target = target ?? throw new ArgumentNullException(nameof(target));
            arg = arg ?? throw new ArgumentNullException(nameof(arg));

            var targetBuilder = (target?.Value as IScriptObject)?.ObjectCore?.Builder;
            var argBuilder = (arg?.Value as IScriptObject)?.ObjectCore?.Builder;

            if (targetBuilder != null && argBuilder != null)
            {
                var result = targetBuilder.Is(argBuilder);

                var convertTargetExpr = Expression.Convert(target.Expression, typeof(IScriptObject));
                var targetObjectCoreExpr = Expression.Property(convertTargetExpr, nameof(IScriptObject.ObjectCore));
                var targetObjectCoreNotNullExpr = Expression.NotEqual(targetObjectCoreExpr, ExpressionHelper.Null());

                var targetObjectBuilderExpr = Expression.Field(targetObjectCoreExpr, nameof(ObjectCore.Builder));
                var targetBuilderNotNullExpr = Expression.NotEqual(targetObjectBuilderExpr, ExpressionHelper.Null());

                var convertArgExpr = Expression.Convert(arg.Expression, typeof(IScriptObject));
                var argObjectCoreExpr = Expression.Property(convertArgExpr, nameof(IScriptObject.ObjectCore));
                var argObjectCoreNotNullExpr = Expression.NotEqual(argObjectCoreExpr, ExpressionHelper.Null());

                var argObjectBuilderExpr = Expression.Field(argObjectCoreExpr, nameof(ObjectCore.Builder));
                var argBuilderNotNullExpr = Expression.NotEqual(argObjectBuilderExpr, ExpressionHelper.Null());

                var targetUriExpr = Expression.Property(targetObjectBuilderExpr, nameof(ScriptObjectBuilder<ScriptObject>.Uri));
                var targetUriEqualExpr = Expression.Equal(targetUriExpr, Expression.Constant(targetBuilder.Uri));

                var argUriExpr = Expression.Property(argObjectBuilderExpr, nameof(ScriptObjectBuilder<ScriptObject>.Uri));
                var argUriEqualExpr = Expression.Equal(argUriExpr, Expression.Constant(argBuilder.Uri));

                var restrictions = BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType)
                    .Merge(BindingRestrictions.GetTypeRestriction(arg.Expression, arg.LimitType))
                    .Merge(BindingRestrictions.GetExpressionRestriction(targetObjectCoreNotNullExpr))
                    .Merge(BindingRestrictions.GetExpressionRestriction(argObjectCoreNotNullExpr))
                    .Merge(BindingRestrictions.GetExpressionRestriction(targetBuilderNotNullExpr))
                    .Merge(BindingRestrictions.GetExpressionRestriction(argBuilderNotNullExpr))
                    .Merge(BindingRestrictions.GetExpressionRestriction(targetUriEqualExpr))
                    .Merge(BindingRestrictions.GetExpressionRestriction(argUriEqualExpr));

                var resultExpr = Expression.Constant(result, typeof(object));

                return new DynamicMetaObject(resultExpr, restrictions);
            }
            else
            {
                var isType = target.LimitType == arg.LimitType;
                var isTypeExpr = Expression.Constant(isType, typeof(object));

                var restrictions = BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType)
                    .Merge(BindingRestrictions.GetTypeRestriction(arg.Expression, arg.LimitType));

                return new DynamicMetaObject(isTypeExpr, restrictions);
            }
        }

        public static DynamicMetaObject BindExtendsOperation(ScriptExtendsOperationBinder binder, DynamicMetaObject target, DynamicMetaObject arg)
        {
            binder = binder ?? throw new ArgumentNullException(nameof(binder));
            target = target ?? throw new ArgumentNullException(nameof(target));
            arg = arg ?? throw new ArgumentNullException(nameof(arg));

            var targetBuilder = (target?.Value as IScriptObject)?.ObjectCore?.Builder;
            var argBuilder = (arg.Value as IScriptObject)?.ObjectCore?.Builder;

            if (targetBuilder != null && argBuilder != null)
            {
                var result = targetBuilder.Extends(argBuilder);

                var convertTargetExpr = Expression.Convert(target.Expression, typeof(IScriptObject));
                var targetObjectCoreExpr = Expression.Property(convertTargetExpr, nameof(IScriptObject.ObjectCore));
                var targetObjectCoreNotNullExpr = Expression.NotEqual(targetObjectCoreExpr, ExpressionHelper.Null());

                var targetObjectBuilderExpr = Expression.Field(targetObjectCoreExpr, nameof(ObjectCore.Builder));
                var targetBuilderNotNullExpr = Expression.NotEqual(targetObjectBuilderExpr, ExpressionHelper.Null());

                var convertArgExpr = Expression.Convert(arg.Expression, typeof(IScriptObject));
                var argObjectCoreExpr = Expression.Property(convertArgExpr, nameof(IScriptObject.ObjectCore));
                var argObjectCoreNotNullExpr = Expression.NotEqual(argObjectCoreExpr, ExpressionHelper.Null());

                var argObjectBuilderExpr = Expression.Field(argObjectCoreExpr, nameof(ObjectCore.Builder));
                var argBuilderNotNullExpr = Expression.NotEqual(argObjectBuilderExpr, ExpressionHelper.Null());

                var targetUriExpr = Expression.Property(targetObjectBuilderExpr, nameof(ScriptObjectBuilder<ScriptObject>.Uri));
                var targetUriEqualExpr = Expression.Equal(targetUriExpr, Expression.Constant(targetBuilder.Uri));

                var argUriExpr = Expression.Property(argObjectBuilderExpr, nameof(ScriptObjectBuilder<ScriptObject>.Uri));
                var argUriEqualExpr = Expression.Equal(argUriExpr, Expression.Constant(argBuilder.Uri));

                var restrictions = BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType)
                    .Merge(BindingRestrictions.GetTypeRestriction(arg.Expression, arg.LimitType))
                    .Merge(BindingRestrictions.GetExpressionRestriction(targetObjectCoreNotNullExpr))
                    .Merge(BindingRestrictions.GetExpressionRestriction(argObjectCoreNotNullExpr))
                    .Merge(BindingRestrictions.GetExpressionRestriction(targetBuilderNotNullExpr))
                    .Merge(BindingRestrictions.GetExpressionRestriction(argBuilderNotNullExpr))
                    .Merge(BindingRestrictions.GetExpressionRestriction(targetUriEqualExpr))
                    .Merge(BindingRestrictions.GetExpressionRestriction(argUriEqualExpr));

                var resultExpr = Expression.Constant(result, typeof(object));

                return new DynamicMetaObject(resultExpr, restrictions);
            }
            else
            {
                var result = arg.LimitType != target.LimitType && arg.LimitType.IsAssignableFrom(target.LimitType);
                var resultExpr = Expression.Constant(result, typeof(object));

                var restrictions = BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType)
                    .Merge(BindingRestrictions.GetTypeRestriction(arg.Expression, arg.LimitType));

                return new DynamicMetaObject(resultExpr, restrictions);
            }

            throw new NotImplementedException();
        }

        public static DynamicMetaObject BindHasOperation(ScriptHasOperationBinder binder, DynamicMetaObject target)
        {
            binder = binder ?? throw new ArgumentNullException(nameof(binder));
            target = target ?? throw new ArgumentNullException(nameof(target));

            var hasMember = target.Value != null
                ? TypeProvider.Current.HasMember(target.LimitType, binder.Name) :
                false;

            return new DynamicMetaObject(
                    Expression.Constant(hasMember, typeof(object)),
                    RestrictionsHelper.TypeOrNullRestriction(target)
                );
        }

        public static DynamicMetaObject BindInvoke(DynamicMetaObjectBinder binder, DynamicMetaObject target, DynamicMetaObject delegateTarget, DynamicMetaObject[] args)
        {
            binder = binder ?? throw new ArgumentNullException(nameof(binder));
            target = target ?? throw new ArgumentNullException(nameof(target));
            delegateTarget = delegateTarget ?? throw new ArgumentNullException(nameof(delegateTarget));
            args = args ?? throw new ArgumentNullException(nameof(args));

            var del = delegateTarget.Value as Delegate;

            var paramTypes = del.GetType().GetMethod("Invoke").GetParameters().Select(p => p.ParameterType).ToArray();

            return TryWrapParams(binder, target, delegateTarget, args, paramTypes)
                ?? TryUnwrapParams(binder, target, delegateTarget, args, paramTypes)
                ?? GetInvokeExpression(target, delegateTarget, args, paramTypes);
        }

        private static DynamicMetaObject GetInvokeExpression(DynamicMetaObject target, DynamicMetaObject delegateTarget, DynamicMetaObject[] args, Type[] paramTypes)
        {
            var argExprs = GetInvokeArgs(paramTypes, args);
            var convertDelExpr = Expression.Convert(delegateTarget.Expression, delegateTarget.LimitType);
            var invokeExpr = Expression.Invoke(convertDelExpr, argExprs);

            var restrictions = RestrictionsHelper.TypeOrNullRestriction(target)
                .Merge(RestrictionsHelper.TypeOrNullRestriction(delegateTarget));

            return new DynamicMetaObject(ConvertReturnType(invokeExpr, delegateTarget.Value as Delegate), restrictions);
        }

        private static IEnumerable<Expression> GetInvokeArgs(Type[] paramTypes, DynamicMetaObject[] args)
        {
            var argExprs =
                paramTypes.Select(
                    (t, i) => i < args.Length ? ExpressionHelper.DynamicConvert(args[i].Expression, t) : Expression.Default(t));

            return argExprs;
        }

        private static DynamicMetaObject TryWrapParams(DynamicMetaObjectBinder binder, DynamicMetaObject target, DynamicMetaObject delegateTarget, DynamicMetaObject[] args, Type[] paramTypes)
        {
            if (IsParamsDelegate(paramTypes) && !IsParamsArg(args))
            {
                var param = new ScriptParams(args.Select(a => a.Value).ToArray());

                var paramExpr = Expression.Variable(typeof(ScriptParams), "params");
                var newObjArrayExpr = Expression.NewArrayInit(typeof(object), args.Select(a => ExpressionHelper.DynamicConvert(a.Expression, typeof(object))));
                var newScriptParamsExpr = Expression.New(ScriptParamsObjArrayCtor, newObjArrayExpr);
                var assignParamExpr = Expression.Assign(paramExpr, newScriptParamsExpr);

                var newArgs = new[]
                {
                    param.GetMetaObject(paramExpr)
                };

                var metaObject = BindInvoke(binder, target, delegateTarget, newArgs);
                var blockExpr = Expression.Block(typeof(object), new[] { paramExpr }, assignParamExpr, metaObject.Expression);

                var restrictions = RestrictionsHelper.TypeOrNullRestriction(target)
                .Merge(RestrictionsHelper.TypeOrNullRestriction(delegateTarget));

                return new DynamicMetaObject(blockExpr, restrictions);
            }

            return null;
        }

        private static DynamicMetaObject TryUnwrapParams(DynamicMetaObjectBinder binder, DynamicMetaObject target, DynamicMetaObject delegateTarget, DynamicMetaObject[] args, Type[] paramTypes)
        {
            if (!IsParamsDelegate(paramTypes) && IsParamsArg(args))
            {
                var arg = args[0];

                var param = (ScriptParams)arg.Value;

                var newArgs =
                    param.Select(
                        (p, i) =>
                            new DynamicMetaObject(
                                Expression.MakeIndex(arg.Expression, ScriptParamsObjectIntIndex, new[] { Expression.Constant(i) }),
                                BindingRestrictions.Empty,
                                p
                                )).ToArray();

                var result = BindInvoke(binder, target, delegateTarget, newArgs);

                var scriptParamsExpr = Expression.Convert(args[0].Expression, typeof(ScriptParams));
                var countExpr = Expression.Property(scriptParamsExpr, "Count");
                var valueExpr = Expression.Constant(param.Count, typeof(int));
                var equalExpr = Expression.Equal(countExpr, valueExpr);

                var restrictions = BindingRestrictions.GetTypeRestriction(arg.Expression, arg.LimitType)
                    .Merge(BindingRestrictions.GetExpressionRestriction(equalExpr));

                return new DynamicMetaObject(result.Expression, result.Restrictions.Merge(restrictions));
            }

            return null;
        }

        private static bool IsParamsArg(DynamicMetaObject[] args)
        {
            if (args.Length == 1 && args[0].Value is ScriptParams)
                return true;

            return false;
        }

        private static bool IsParamsDelegate(Type[] paramTypes)
        {
            if (paramTypes.Length == 1 && paramTypes[0] == typeof(ScriptParams))
                return true;

            return false;
        }

        private static Expression ConvertReturnType(Expression expression, Delegate del)
        {
            if (del.Method.ReturnType == typeof(void))
            {
                return Expression.Block(typeof(object),
                    expression,
                    Expression.Constant(null, typeof(object)));
            }

            if (del.Method.ReturnType.IsValueType)
            {
                return ExpressionHelper.DynamicConvert(expression, typeof(object));
            }

            return expression;
        }
    }
}
