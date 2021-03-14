using FryScript.Helpers;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript.Binders
{
    public class ScriptGetMemberBinder : GetMemberBinder
    {
        public ScriptGetMemberBinder(string name)
            : base(name, false)
        {
        }

        public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
        {
            if(target.Value == null)
                ExceptionHelper.MemberUndefined(Name);

            var restrictions = RestrictionsHelper.TypeOrNullRestriction(target);
            if (ExpressionHelper.TryGetMethodExpression(Expression.Convert(target.Expression, target.LimitType), Name, out Expression expression))
                return new DynamicMetaObject(expression, restrictions);

            return new DynamicMetaObject(ExpressionHelper.Null(), restrictions);
        }

        private BindingRestrictions GetDefaultRestrictions(DynamicMetaObject target)
        {
            return RestrictionsHelper.TypeOrNullRestriction(target);
        }
    }
}
