using FryScript.Helpers;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript
{
    [ScriptableType("fibre")]
    public class ScriptFibre : ScriptFunction
    {
        public ScriptFibre()
            : base(new Action(() => {}))
        {
            ObjectCore.Builder = Builder.ScriptFibreBuilder;
        }

        public ScriptFibre(Delegate target)
            : base(target)
        {
            ObjectCore.Builder = Builder.ScriptFibreBuilder;
        }

        internal static ScriptFibre New(Delegate target)
        {
            return new ScriptFibre(target);
        }

        public override DynamicMetaObject GetMetaObject(Expression parameter)
        {
            return new MetaScriptFibre(parameter, BindingRestrictions.Empty, this);
        }

        public ScriptFibreContext Begin(params object[] args)
        {
            return TargetDelegate.DynamicInvoke(args) as ScriptFibreContext;
        }
    }
}
