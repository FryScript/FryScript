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
            var nameStr = ScriptTypeHelper.NormalizeTypeName(name.ValueString);
            var alias = ChildNodes.Skip(3).Take(1).First();

            var importScript = new ScriptObjectReference();
            var scriptEngine = CompilerContext.ScriptEngine;
            importScript.SetResolver(() => scriptEngine.Get(nameStr, CompilerContext.Name));

            var importScriptExpr = Expression.Constant(importScript);
            var aliasExpr = alias.CreateIdentifier(scope);
            var assignAliasExpr = Expression.Assign(aliasExpr, importScriptExpr);

            CompilerContext.ImportInfos.Add(new ImportInfo
            {
                Alias = alias.ValueString,
                Object = importScript
            });

            return assignAliasExpr;
        }
    }
}
