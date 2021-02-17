using FryScript.Parsing;
using System;

namespace FryScript.Compilation
{
    public interface IScriptCompiler
    {
        Func<IScriptObject, object> Compile(string source, string name, CompilerContext compilerContext);

        IScriptParser Parser { get; }

        IScriptParser ExpressionParser { get; }
    }
}
