using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ScriptImportNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var name = ChildNodes.Skip(1).Take(1).First();
            var nameStr = name.ValueString;
            var alias = ChildNodes.Skip(3).Take(1).First();

            var importScript = new ScriptObjectReference(() => CompilerContext?.ScriptRuntime?.Get(nameStr, CompilerContext.Uri));

            var importScriptExpr = Expression.Constant(importScript);
            alias.CreateIdentifier(scope);
            var assignAliasExpr = alias.SetIdentifier(scope, importScriptExpr);

            CompilerContext.ImportInfos.Add(new ImportInfo
            {
                Alias = alias.ValueString,
                Object = importScript
            });

            return assignAliasExpr;
        }
    }
}
