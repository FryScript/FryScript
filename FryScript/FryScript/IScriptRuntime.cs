using System;

namespace FryScript
{
    public interface IScriptRuntime
    {
        IScriptObject Get(string name, Uri relativeTo = null);

        //IScriptObject Import(Type type);

        //IScriptObject Eval(string script);
    }
}
