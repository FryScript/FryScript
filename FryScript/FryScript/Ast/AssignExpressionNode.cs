using FryScript.Compilation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class AssignExpressionNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
                return GetChildExpression(scope);

            var left = (IdentifierExpressionNode)ChildNodes.First();
            var right = ChildNodes.Skip(2).First();

            var valueExpr = right.GetExpression(scope);

            return left.SetIdentifier(scope, valueExpr);
        }
    }
}
