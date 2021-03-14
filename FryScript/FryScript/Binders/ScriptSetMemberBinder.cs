using FryScript.Helpers;
using System.Dynamic;

namespace FryScript.Binders
{

    public class ScriptSetMemberBinder : SetMemberBinder
    {
        internal ScriptSetMemberBinder(string name)
            : base(name, false)
        {
        }

        public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
        {
            throw ExceptionHelper.NonSetMember(target.LimitType);
        }

        private BindingRestrictions GetDefaultRestrictions(DynamicMetaObject target)
        {
            return RestrictionsHelper.TypeOrNullRestriction(target);
        }
    }
}
