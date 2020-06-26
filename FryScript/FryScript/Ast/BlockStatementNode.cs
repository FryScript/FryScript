using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{


    public class BlockStatementNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 0)
                return ExpressionHelper.Null();

            scope = scope.New(this);

            return scope.ScopeBlock(GetChildExpression(scope));
        }
    }
}
