using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class NotExpressionNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
                return GetChildExpression(scope);

            var right = ChildNodes.Skip(1).First();

            var rightExpr = ExpressionHelper.DynamicConvert(right.GetExpression(scope), typeof (bool));

            return Expression.Convert(Expression.Not(rightExpr), typeof(object));
        }
    }
}
