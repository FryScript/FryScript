using System;

namespace FryScript
{
    public interface IScriptRuntime
    {
        IScriptObject Get(string name, string relativeTo = null);

        //IScriptObject Import(Type type);

        //IScriptObject Eval(string script);
    }
}
