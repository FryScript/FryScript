using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class IdentifierExpressionNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            return GetIdentifier(scope);
        }

        public override Expression GetIdentifier(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
            {
                var identifierNode = ChildNodes.First();
                return identifierNode.GetIdentifier(scope);
            }

            var left = ChildNodes.First();
            var right = ChildNodes.Skip(1).First();

            var leftExpr = left.GetExpression(scope);
            if (right is IdentifierNode)
            {
                var getExpr = ExpressionHelper.DynamicGetMember(right.ValueString, leftExpr);
                
                return getExpr;
            }

            if (right is IndexNode)
            {
                var getIndexExpr = ExpressionHelper.DynamicGetIndex(leftExpr, right.GetExpression(scope));

                return getIndexExpr;
            }

            throw new InvalidOperationException("Unexpected right hand node");
        }

        public override Expression SetIdentifier(Scope scope, Expression value)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));
            value = value ?? throw new ArgumentNullException(nameof(value));

            if (ChildNodes.Length == 1)
            {
                var identifierNode = (IdentifierNode) ChildNodes.First();
                return identifierNode.SetIdentifier(scope, value);
            }

            var left = ChildNodes.First();
            var right = ChildNodes.Skip(1).First();

            var leftExpr = left.GetExpression(scope);
            if (right is IdentifierNode)
            {
                var setExpr = ExpressionHelper.DynamicSetMember(right.ValueString, leftExpr, value);
    
                return setExpr;
            }

            if (right is IndexNode)
            {
                var setIndexExpr = ExpressionHelper.DynamicSetIndex(leftExpr, value, right.GetExpression(scope));

                return setIndexExpr;
            }

            throw new NotImplementedException();
        }
    }
}
