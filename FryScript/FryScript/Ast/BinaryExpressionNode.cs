using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.Parsing;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class BinaryExpressionNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
                return GetChildExpression(scope);

            var left = (IdentifierExpressionNode) ChildNodes.First();
            var op = ChildNodes.Skip(1).First().ValueString;
            var right = ChildNodes.Skip(2).First();

            ExpressionType operation;

            switch (op)
            {
                case Operators.IncrementAssign:
                    operation = ExpressionType.Add;
                    break;
                case Operators.DecrementAssign:
                    operation = ExpressionType.Subtract;
                    break;
                case Operators.MultiplyAssign:
                    operation = ExpressionType.Multiply;
                    break;
                case Operators.DivideAssign:
                    operation = ExpressionType.Divide;
                    break;
                case Operators.Modulo:
                    operation = ExpressionType.Modulo;
                    break;
                default:
                    throw new InvalidOperationException($"Unknown operator {op}");
            }
            
            var leftExpr = left.GetIdentifier(scope);
            var rightExpr = right.GetExpression(scope);
            var binaryExpr = ExpressionHelper.DynamicBinaryOperation(leftExpr, operation, rightExpr);
            
            return left.SetIdentifier(scope, binaryExpr);
        }
    }
}
