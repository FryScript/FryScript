using System;

namespace FryScript
{
    public interface IScriptRuntime
    {
        bool DetailedExceptions { get; set; }

        IScriptObject Get(string name, Uri relativeTo = null);

        IScriptObject Import(Type type);

        IScriptObject New(string name, params object[] args);

        object Eval(string script);
    }
}
