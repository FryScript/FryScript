using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.Parsing;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class RelationalExpressionNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
                return GetChildExpression(scope);

            var left = ChildNodes.First();
            var op = ChildNodes.Skip(1).First().ValueString;
            var right = ChildNodes.Skip(2).First();

            ExpressionType operation;

            switch (op)
            {
                case Operators.Equal:
                    operation = ExpressionType.Equal;
                    break;
                case Operators.NotEqual:
                    operation = ExpressionType.NotEqual;
                    break;
                case Operators.GreaterThan:
                    operation = ExpressionType.GreaterThan;
                    break;
                case Operators.LessThan:
                    operation = ExpressionType.LessThan;
                    break;
                case Operators.GreaterThanOrEqual:
                    operation = ExpressionType.GreaterThanOrEqual;
                    break;
                default:
                    operation = ExpressionType.LessThanOrEqual;
                    break;
            }

            var leftExpr = left.GetExpression(scope);
            var rightExpr = right.GetExpression(scope);
            var relationalExpr = ExpressionHelper.DynamicBinaryOperation(leftExpr, operation, rightExpr);

            return relationalExpr;
        }
    }
}
