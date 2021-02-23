using FryScript.HostInterop;
using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript.Helpers
{

    public static class RestrictionsHelper
    {
        public static BindingRestrictions TypeOrNullRestriction(DynamicMetaObject target)
        {
            target = target ?? throw new ArgumentNullException(nameof(target));

            if (target.Value == null)
                return
                    BindingRestrictions.GetExpressionRestriction(Expression.Equal(target.Expression,
                        ExpressionHelper.Null()));

            return BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType);
        }

        public static BindingRestrictions ValueRestriction(DynamicMetaObject target)
        {
            target = target ?? throw new ArgumentNullException(nameof(target));

            return
                BindingRestrictions.GetExpressionRestriction(Expression.Equal(Expression.Convert(target.Expression, target.LimitType),
                    Expression.Convert(Expression.Constant(target.Value), target.LimitType)));
        }
    }
}
