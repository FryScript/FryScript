using FryScript.Helpers;
using FryScript.HostInterop;
using System;
using System.Dynamic;

namespace FryScript.Binders
{

    public class ScriptInvokeMemberBinder : InvokeMemberBinder
    {
        public ScriptInvokeMemberBinder(string name, int argCount)
            : base(name, false, new CallInfo(argCount))
        {
        }

        public override DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
        {
            throw new NotImplementedException();
        }

        public override DynamicMetaObject FallbackInvokeMember(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
        {
            var metaObject = TypeProvider.Current.GetMetaObject(target.Expression, target.Value);
            metaObject = metaObject.BindInvokeMember(this, args);

            return new DynamicMetaObject(metaObject.Expression, GetDefaultRestrictions(target));
        }

        private BindingRestrictions GetDefaultRestrictions(DynamicMetaObject target)
        {
            return RestrictionsHelper.TypeOrNullRestriction(target);
        }
    }
}
