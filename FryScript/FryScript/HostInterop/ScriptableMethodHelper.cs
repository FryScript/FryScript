using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using FryScript.Helpers;

namespace FryScript.HostInterop
{

    public static class ScriptableMethodHelper
    {
        private static readonly ConstructorInfo ScriptFunction_DelegateCtor =
            typeof(ScriptFunction).GetConstructor(new[] { typeof(Delegate) });

        public static Expression CreateMethod(Expression instance, MethodInfo methodInfo)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance));
            methodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));

            var argExprs = (from parameter in methodInfo.GetParameters()
                            select Expression.Parameter(parameter.ParameterType, parameter.Name)).ToArray();

            var callExpr = Expression.Call(instance, methodInfo, argExprs);
            var lambdaExpr = Expression.Lambda(callExpr, argExprs);

            var newFuncExpr = Expression.New(ScriptFunction_DelegateCtor, lambdaExpr);

            return newFuncExpr;
        }

        public static Expression CreateExtensionMethod(Expression instance, MethodInfo methodInfo)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance));
            methodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));

            var argExprs = (from parameter in methodInfo.GetParameters()
                            select Expression.Parameter(parameter.ParameterType, parameter.Name)).ToArray();

            var callExpr = Expression.Call(methodInfo, argExprs);
            var lambdaArgExprs = argExprs.Skip(1);
            var lambdaExpr = Expression.Lambda(callExpr, lambdaArgExprs);

            var factoryArgExpr = argExprs.First();
            var factoryLambdaExpr = Expression.Lambda(lambdaExpr, factoryArgExpr);

            var newFuncExpr = Expression.New(
                ScriptFunction_DelegateCtor,
                Expression.Invoke(factoryLambdaExpr, ExpressionHelper.DynamicConvert(instance, factoryArgExpr.Type)));

            return newFuncExpr;
        }
    }
}
