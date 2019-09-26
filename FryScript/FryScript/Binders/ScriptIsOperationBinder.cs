using FryScript.Helpers;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript.Binders
{
    public class ScriptIsOperationBinder : DynamicMetaObjectBinder
    {
        public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
        {
            var value = args[0];

            if (target is MetaScriptObjectBase metaScriptObjectBase)
                return metaScriptObjectBase.BindIsOperation(this, args[0]);

            return FallbackIs(target, value);
        }

        public DynamicMetaObject FallbackIs(DynamicMetaObject target, DynamicMetaObject value)
        {
            if (TryNullCheck(target, out DynamicMetaObject result) || TryNullCheck(value, out result))
                return result;

            return BindHelper.BindIsOperation(this, target, value);
        }

        private static bool TryNullCheck(DynamicMetaObject target, out DynamicMetaObject result)
        {
            result = null;

            if (target.Value == null)
            {
                var restrictions = RestrictionsHelper.TypeOrNullRestriction(target);
                result = new DynamicMetaObject(Expression.Constant(false, typeof(object)), restrictions);
            }

            return result != null;
        }
    }
}
