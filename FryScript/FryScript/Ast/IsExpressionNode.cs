using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class IsExpressionNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
                return GetChildExpression(scope);

            var identifier = ChildNodes.First();
            var type = ChildNodes.Skip(2).First();

            return ExpressionHelper.DynamicIsOperation(identifier.GetExpression(scope), type.GetExpression(scope));
        }
    }
}
