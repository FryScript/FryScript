using System;
using FryScript.Debugging;

namespace FryScript
{
    public interface IScriptRuntime
    {
        bool DetailedExceptions { get; set; }

        DebugHook DebugHook { get; set; }

        IScriptObject Get(string name, Uri relativeTo = null);

        IScriptObject Import(Type type);

        IScriptObject Import(string name, IScriptObject instance);

        IScriptObject New(string name, params object[] args);

        object Eval(string script);
    }
}
