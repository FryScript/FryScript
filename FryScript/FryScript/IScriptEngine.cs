using System;

namespace FryScript
{
    public interface IScriptEngine
    {
        string Extension { get; }

        dynamic Eval(string script);

        ScriptObject Compile(string name, string script);

        ScriptObject GetOrCompile(string name);

        ScriptObject GetScript(string name);

        void Import(string name, Type type);

        void Import<T>(string name) where T : class, IScriptable, new();

        void Import<T>(string name, Func<T> ctor) where T : class, IScriptable;
    }
}
