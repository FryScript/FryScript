using System;

namespace FryScript
{
    public interface IScriptRuntime
    {
        IScriptObject Get(string name, Uri relativeTo = null);

        IScriptObject Import(Type type);

        IScriptObject New(string name, params object[] args);

        //IScriptObject Eval(string script);
    }
}
