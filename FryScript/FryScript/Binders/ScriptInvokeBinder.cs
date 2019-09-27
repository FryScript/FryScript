﻿using FryScript.Helpers;
using FryScript.HostInterop;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FryScript.Binders
{

    public class ScriptInvokeBinder : InvokeBinder
    {
        public ScriptInvokeBinder(int argCount)
            : base(new CallInfo(argCount))
        {
        }

        public override DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
        {
            if (!TypeProvider.Current.IsPrimitive(target.LimitType) ||
                !TypeProvider.Current.TryGetTypeOperator(target.LimitType, ScriptableTypeOperator.Invoke,
                out MethodInfo invokeMethod)
                )
                throw ExceptionHelper.NonInvokable(target.LimitType);

            var invokeArgs = new[]{
                target.Expression
            }.Concat(invokeMethod.GetParameters()
            .Select(
                (p, i) => ExpressionHelper.DynamicConvert(args[i].Expression, p.ParameterType))
            );
            
            var invokeExpr = Expression.Call(invokeMethod, invokeArgs);
            var convertExpr = ExpressionHelper.DynamicConvert(invokeExpr, typeof(object));

            return new DynamicMetaObject(
                convertExpr,
                BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType)
            );
        }
    }
}
