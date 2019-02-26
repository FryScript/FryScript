using System;

namespace FryScript
{
    public interface IScriptRuntime
    {
        IScriptObject Get(string name);

        //IScriptObject Import(Type type);

        //IScriptObject Eval(string script);
    }
}
