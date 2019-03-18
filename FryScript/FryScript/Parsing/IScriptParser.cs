using FryScript.Ast;
using FryScript.Compilation;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FryScript.Parsing
{
    public interface IScriptParser
    {
        AstNode Parse(string script, string fileName, CompilerContext compilerContext);

        IRootNode Parse2(string source, string name, CompilerContext compilerContext);
    }
}
