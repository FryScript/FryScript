using FryScript.Compilation;
using FryScript.Parsing;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class FunctionExtendStatementNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var identifier = ChildNodes.First();
            var function = ChildNodes.Skip(2).First();

            scope = scope.New(this, hoisted: false);

            var baseExpr = scope.AddKeywordMember<ScriptFunction>(Keywords.Base, this);
            var assignBaseExpr = Expression.Assign(
                baseExpr,
                Expression.Call(
                    typeof (ScriptFunction),
                    nameof(ScriptFunction.Extend),
                    null,
                    identifier.GetIdentifier(scope)
                    )
                );

            var functionExpr = function.GetExpression(scope);
            var assignIdentifierExpr = identifier.SetIdentifier(scope, functionExpr);

            var extendExpr = scope.ScopeBlock(assignBaseExpr, assignIdentifierExpr);

            return extendExpr;
        }
    }
}
