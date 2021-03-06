﻿using FryScript.Helpers;
using System;
using System.Dynamic;
using System.Linq;

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
            target = target ?? throw new ArgumentNullException(nameof(target));
            args = args ?? throw new ArgumentNullException(nameof(args));

            var getBinder = BinderCache.Current.GetMemberBinder(Name);
            var metaobject = getBinder.Bind(target, null);
            var invokeExpr = ExpressionHelper.DynamicInvoke(metaobject.Expression, args.Select(a => a.Expression).ToArray());

            return new DynamicMetaObject(invokeExpr, metaobject.Restrictions);
        }

        private BindingRestrictions GetDefaultRestrictions(DynamicMetaObject target)
        {
            return RestrictionsHelper.TypeOrNullRestriction(target);
        }
    }
}
