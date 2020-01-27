using FryScript.Compilation;
using System;

namespace FryScript.Ast
{
    public interface IRootNode
    {
        Func<IScriptObject, object> Compile(Scope scope);
    }
}
