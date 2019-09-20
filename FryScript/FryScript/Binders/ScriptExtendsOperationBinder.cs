using FryScript.Helpers;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript.Binders
{
    public class ScriptExtendsOperationBinder : DynamicMetaObjectBinder
    {
        public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
        {
            var value = args[0];

            if (target is MetaScriptObjectBase metaScriptObjectBase)
                return metaScriptObjectBase.BindExtendsOperation(this, value);

            return FallbackExtends(target, value);
        }

        public DynamicMetaObject FallbackExtends(DynamicMetaObject target, DynamicMetaObject value)
        {
            if (TryNullCheck(target, out DynamicMetaObject result) || TryNullCheck(value, out result))
                return result;

            return BindHelper.BindExtendsOperation(this, target, value);
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
