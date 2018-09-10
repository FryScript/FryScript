using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ObjectLiteralMemberNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var nameNode = ChildNodes.First();
            var valueNode = ChildNodes.Skip(1).First();

            var valueExpr = valueNode.GetExpression(scope);

            scope.TryGetData(ScopeData.ObjectLiteralContext, out ParameterExpression thisExpr);
            var memberExpr = ExpressionHelper.DynamicSetMember(nameNode.ValueString, thisExpr, valueExpr);

            return memberExpr;
        }
    }
}
