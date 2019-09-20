using FryScript.Helpers;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript.Binders
{
    public class ScriptExtendsOperationBinder : DynamicMetaObjectBinder
    {
        public override DynamicMetaObject Bind(DynamicMetaObject target, DynamicMetaObject[] args)
        {
            DynamicMetaObject result;
            var value = args[0];

            if (TryNullCheck(target, out result) || TryNullCheck(value, out result))
                return result;

            var bindExtendsOperationProvider = target as IBindExtendsOperationProvider;

            if (bindExtendsOperationProvider == null)
                return FallbackExtends(target, value);
            return bindExtendsOperationProvider.BindExtendsOperation(this, value);
        }

        public DynamicMetaObject FallbackExtends(DynamicMetaObject target, DynamicMetaObject value)
        {
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
