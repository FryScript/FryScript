using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.Parsing;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class MultiplyExpressionNode: AstNode
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
                case Operators.Multiply:
                    operation = ExpressionType.Multiply;
                    break;
                case Operators.Divide:
                    operation = ExpressionType.Divide;
                    break;
                default:
                    operation = ExpressionType.Modulo;
                    break;
            }

            var leftExpr = left.GetExpression(scope);
            var rightExpr = right.GetExpression(scope);
            var opExpr = ExpressionHelper.DynamicBinaryOperation(leftExpr, operation, rightExpr);

            return opExpr;
        }
    }
}
