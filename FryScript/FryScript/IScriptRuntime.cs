using System;

namespace FryScript
{
    public interface IScriptRuntime
    {
        IScriptObject Get(string name, Uri relativeTo = null);

        IScriptObject Import(Type type);

        IScriptObject New(string name);

        //IScriptObject Eval(string script);
    }
}
