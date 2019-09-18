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
            var arg = args[0];

            if (TryNullCheck(target, out result) || TryNullCheck(arg, out result))
                return result;

            var metaObject = DynamicMetaObjectHelper.GetDynamicMetaObject(target);

            var bindIsOperationProvider = metaObject as IBindIsOperationProvider;

            if (bindIsOperationProvider == null)
                return new DynamicMetaObject(
                    Expression.Constant(false, typeof(object)),
                    BindingRestrictions.GetTypeRestriction(target.Expression, target.LimitType)
                    );

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
