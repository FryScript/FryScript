using System;
using System.Dynamic;

namespace FryScript.Binders
{
    public class ScriptGetIndexBinder : GetIndexBinder
    {
        public ScriptGetIndexBinder(int argCount)
            : base(new CallInfo(argCount))
        {

        }

        public override DynamicMetaObject FallbackGetIndex(DynamicMetaObject target, DynamicMetaObject[] indexes, DynamicMetaObject errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }
}
