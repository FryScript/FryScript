using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.Parsing;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{

    public class AddExpressionNode: AstNode
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
                case Operators.Add:
                    operation = ExpressionType.Add;
                    break;
                default:
                    operation = ExpressionType.Subtract;
                    break;
            }

            var leftExpr = left.GetExpression(scope);
            var rightExpr = right.GetExpression(scope);
            var addExpr = ExpressionHelper.DynamicBinaryOperation(leftExpr, operation, rightExpr);

            return addExpr;
        }
    }
}
