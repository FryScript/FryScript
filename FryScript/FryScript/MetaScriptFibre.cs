using FryScript.Binders;
using FryScript.Helpers;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript
{
    public class MetaScriptFibre : MetaScriptObject
    {
        public Expression ScriptFibreExpr => Expression.Convert(Expression, typeof(ScriptFibre));

        public MetaScriptFibre(Expression expression, BindingRestrictions restrictions, object value)
            : base(expression, restrictions, value)
        {
        }

        public override DynamicMetaObject BindInvoke(InvokeBinder binder, DynamicMetaObject[] args)
        {
            var delegateTarget = new DynamicMetaObject(ScriptObjectTargetExpr, BindingRestrictions.Empty, ScriptObject.Target);

            return BindHelper.BindInvoke(binder, this, delegateTarget, args);
        }

        //public override DynamicMetaObject BindAwaitOperation(ScriptAwaitOperationBinder binder, DynamicMetaObject target, DynamicMetaObject[] args)
        //{
        //    var delegateTarget = new DynamicMetaObject(ScriptObjectTargetExpr, BindingRestrictions.Empty, ScriptObject.Target);

        //    return BindHelper.BindInvoke(binder, this, delegateTarget, args);
        //}

        //public override DynamicMetaObject BindBeginOperation(ScriptBeginOperationBinder binder, DynamicMetaObject target, DynamicMetaObject[] args)
        //{
        //    var delegateTarget = new DynamicMetaObject(ScriptObjectTargetExpr, BindingRestrictions.Empty, ScriptObject.Target);

        //    return BindHelper.BindInvoke(binder, this, delegateTarget, args);
        //}
    }
}
