using System;
using System.Dynamic;

namespace FryScript.Binders
{
    public class ScriptSetIndexBinder : SetIndexBinder
    {
        public ScriptSetIndexBinder(int argCount)
            : base(new CallInfo(argCount))
        {

        }

        public override DynamicMetaObject FallbackSetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }
}
