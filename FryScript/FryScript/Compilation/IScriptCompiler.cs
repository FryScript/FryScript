using FryScript.Parsing;
using System;

namespace FryScript.Compilation
{
    public interface IScriptCompiler
    {
        Func<ScriptObject, object> Compile(string script, string fileName, CompilerContext complilerContext);

        IScriptObjectBuilder Compile(string name, CompilerContext context);

        IScriptParser Parser { get; }

        IScriptParser ExpressionParser { get; }
    }
}
