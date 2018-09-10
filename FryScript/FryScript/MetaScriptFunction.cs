using FryScript.Helpers;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript
{
    public class MetaScriptFunction : MetaScriptObject
    {
        public MetaScriptFunction(Expression expression, BindingRestrictions restrictions, object value)
            : base(expression, restrictions, value)
        {
        }

        public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
        {
            var delegateTarget = new DynamicMetaObject(ScriptObjectTargetExpr, BindingRestrictions.Empty, ScriptObject.Target);

            return BindHelper.BindInvoke(binder, this, delegateTarget, args);
        }
    }
}
