using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.Parsing;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class UnarySuffixExpressionNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            scope = scope.New(this);

            var identifierExpression = (IdentifierExpressionNode)ChildNodes.First();
            var op = ChildNodes.Skip(1).First().ValueString;

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
            var tempExpr = Expression.Variable(typeof (object), scope.GetTempName(TempPrefix.Unary));
            var assignTempExpr = Expression.Assign(tempExpr, identifierExpr);
            var unaryExpr = ExpressionHelper.DynamicBinaryOperation(identifierExpr, operation, Expression.Constant(1));
            var setExpr = identifierExpression.SetIdentifier(scope, unaryExpr);

            var blockExpr = Expression.Block(typeof (object), new[] {tempExpr}, assignTempExpr, setExpr, tempExpr);

            return blockExpr;
        }
    }
}
