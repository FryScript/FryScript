using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.Parsing;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class UnaryPrefixExpressionNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
                return GetChildExpression(scope);

            var op = ChildNodes.First().ValueString;
            var identifierExpression = (IdentifierExpressionNode) ChildNodes.Skip(1).First();

            ExpressionType operation;

            switch (op)
            {
                case Operators.Increment:
                    operation = ExpressionType.Add;
                    break;
                default:
                    operation = ExpressionType.Subtract;
                    break;
            }

            var identifierExpr = identifierExpression.GetIdentifier(scope);
            var unaryExpr = ExpressionHelper.DynamicBinaryOperation(identifierExpr, operation, Expression.Constant(1));

            return identifierExpression.SetIdentifier(scope, unaryExpr);
        }
    }
}
