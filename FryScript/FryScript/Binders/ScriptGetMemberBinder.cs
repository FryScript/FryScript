using FryScript.Helpers;
using FryScript.HostInterop;
using System.Dynamic;

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
            var metaObject = TypeProvider.Current.GetMetaObject(target.Expression, target.Value);

            if (metaObject == null)
                ExceptionHelper.NonGetMember(target.LimitType);

            metaObject = metaObject.BindGetMember(this);

            return new DynamicMetaObject(metaObject.Expression, GetDefaultRestrictions(target));
        }

        private BindingRestrictions GetDefaultRestrictions(DynamicMetaObject target)
        {
            return RestrictionsHelper.TypeOrNullRestriction(target);
        }
    }
}
