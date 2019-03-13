using FryScript.Parsing;
using System;

namespace FryScript.Compilation
{
    public interface IScriptCompiler
    {
        Func<ScriptObject, object> Compile(string script, string fileName, CompilerContext complilerContext);

        Func<IScriptObject, object> Compile2(string source, string name, CompilerContext compilerContext);

        IScriptParser Parser { get; }

        IScriptParser ExpressionParser { get; }
    }
}
