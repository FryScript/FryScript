using FryScript.Compilation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ScriptImportFromNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var aliases = ChildNodes.Skip(1).Cast<ImportAliasListNode>().First();
            var scriptName = ChildNodes.Skip(3).First().ValueString;

            var scriptObject = CompilerContext.ScriptRuntime.Get(scriptName, CompilerContext.Uri);

            var importExpr = aliases.GetExpression(scope, scriptObject);

            return importExpr;
        }
    }
}
