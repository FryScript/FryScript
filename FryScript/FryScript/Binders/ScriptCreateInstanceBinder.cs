using System;
using System.Dynamic;

namespace FryScript.Binders
{

    public class ScriptCreateInstanceBinder : CreateInstanceBinder
    {
        public ScriptCreateInstanceBinder(int argCount)
            : base(new CallInfo(argCount))
        {
        }

        public override DynamicMetaObject FallbackCreateInstance(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }
}
