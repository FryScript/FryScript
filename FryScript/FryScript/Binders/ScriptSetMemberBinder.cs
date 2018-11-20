using FryScript.Helpers;
using FryScript.HostInterop;
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
            var metaObject = TypeProvider.Current.GetMetaObject(target.Expression, target.Value);

            if (metaObject == null)
                ExceptionHelper.NonSetMember(target.LimitType);

            metaObject = metaObject.BindSetMember(this, value);

            return new DynamicMetaObject(metaObject.Expression, GetDefaultRestrictions(target));
        }

        private BindingRestrictions GetDefaultRestrictions(DynamicMetaObject target)
        {
            return RestrictionsHelper.TypeOrNullRestriction(target);
        }
    }
}
