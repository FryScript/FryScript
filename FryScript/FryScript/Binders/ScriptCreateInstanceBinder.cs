using System;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FryScript.Helpers;
using FryScript.HostInterop;

namespace FryScript.Binders
{

    public class ScriptCreateInstanceBinder : CreateInstanceBinder
    {
        public ScriptCreateInstanceBinder(int argCount)
            : base(new CallInfo(argCount))
        {
        }

        public override DynamicMetaObject FallbackCreateInstance(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
        {
            if (!TypeProvider.Current.TryGetTypeOperator(target.LimitType, ScriptableTypeOperator.Ctor, out MethodInfo ctorInfo))
                throw new FryScriptException(string.Format("Type {0} does not have a scriptable constructor defined", target.LimitType.FullName));

            var parameterTypes = ctorInfo.GetParameters().Select(p => p.ParameterType).ToArray();
            var argExprs = parameterTypes.Select(
               (t, i) => ExpressionHelper.DynamicConvert(args[i].Expression, t)
               ).ToArray();

            var invokeExpr = Expression.Call(ctorInfo, argExprs);

            return new DynamicMetaObject(
                invokeExpr,
                BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType)
                );
        }
    }
}
