using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class HasExpressionNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
                return GetChildExpression(scope);

            var identifier = ChildNodes.First();
            var member = ChildNodes.Skip(2).First();

            var identifierExpr = identifier.GetExpression(scope);
            var hasExpr = ExpressionHelper.DynamicHasOperation(member.ValueString, identifierExpr);

            return hasExpr;
        }
    }
}
