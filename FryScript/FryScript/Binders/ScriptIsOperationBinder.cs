using FryScript.Helpers;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript.Binders
{
    public class ScriptIsOperationBinder : DynamicMetaObjectBinder
    {
        public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
        {
            DynamicMetaObject result;
            var value = args[0];

            if (TryNullCheck(target, out result) || TryNullCheck(value, out result))
                return result;

            var bindIsOperationProvider = target as MetaScriptObjectBase;

            if (bindIsOperationProvider == null)
                return FallbackIs(target, value);

            return bindIsOperationProvider.BindIsOperation(this, args[0]);
        }

        public DynamicMetaObject FallbackIs(DynamicMetaObject target, DynamicMetaObject value)
        {
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
