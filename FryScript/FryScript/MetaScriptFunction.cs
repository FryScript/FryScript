using FryScript.Helpers;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript
{
    public class MetaScriptFunction : MetaScriptObject
    {
        public ScriptFunction ScriptFunction => Value as ScriptFunction;

        public Expression ScriptFunctionExpr => Expression.Convert(Expression, typeof(ScriptFunction));

        public Expression ScriptFunctionTargetDelegateExpr => Expression.PropertyOrField(ScriptFunctionExpr, nameof(ScriptFunction.TargetDelegate));

        public MetaScriptFunction(Expression expression, BindingRestrictions restrictions, object value)
            : base(expression, restrictions, value)
        {
        }

        public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
        {
            var delegateTarget = new DynamicMetaObject(ScriptFunctionTargetDelegateExpr, BindingRestrictions.Empty, ScriptFunction.TargetDelegate);

            return BindHelper.BindInvoke(binder, this, delegateTarget, args);
        }
    }
}
