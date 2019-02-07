using FryScript.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ScriptImportFromNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var path = ChildNodes.Length == 3
                ? ChildNodes.Skip(2).First().ValueString
                : ChildNodes.Skip(3).First().ValueString;

            var importObj = CompilerContext.ScriptEngine.Get(path, CompilerContext.Name);

            var importParamsExpr = (ChildNodes.Length == 3
                ? importObj.GetMembers().Select(m => scope.AddMember(m, this))
                : ChildNodes.Skip(1).Cast<ParameterNamesNode>().First().DeclareParameters(scope)).ToList();

            return GetImportExpression(scope, importObj, importParamsExpr);
        }

        private Expression GetImportExpression(Scope scope, IScriptObject importObj, List<ParameterExpression> importParamExprs)
        {
            var importedMemberExprs = new List<Expression>();

            importParamExprs.ForEach(i =>
            {
                var importedObject = ScriptObjectExtensions.GetIndex(importObj, i.Name);
                var importedObjectExpr = Expression.Constant(importedObject, typeof(object));
                var assignParamExpr = Expression.Assign(i, importedObjectExpr);

                importedMemberExprs.Add(assignParamExpr);
            });

            var blockExpr = Expression.Block(importedMemberExprs);

            return blockExpr;
        }
    }
}
