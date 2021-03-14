using FryScript.Ast;
using FryScript.Compilation;

namespace FryScript.Parsing
{
    public interface IScriptParser
    {
        IRootNode Parse(string source, string name, CompilerContext compilerContext);

        AstNode ParseExpression(string source, string name, CompilerContext compilerContext);
    }
}
