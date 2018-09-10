using FryScript.Helpers;
using System.Dynamic;

namespace FryScript.Binders
{

    public class ScriptInvokeBinder : InvokeBinder
    {
        public ScriptInvokeBinder(int argCount)
            : base(new CallInfo(argCount))
        {
        }

        public override DynamicMetaObject FallbackInvoke(DynamicMetaObject target, DynamicMetaObject[] args, DynamicMetaObject errorSuggestion)
        {
            throw ExceptionHelper.NonInvokable(target.LimitType);
        }
    }
}
