using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class VariableDeclarationNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var identifierNode = ChildNodes.Skip(1).First();

            identifierNode.CreateIdentifier(scope);
            var valueExpr = ChildNodes.Length == 2
                ? ExpressionHelper.Null()
                : ChildNodes.Skip(3).First().GetExpression(scope);
            var assignExpr = identifierNode.SetIdentifier(scope, valueExpr);

            return assignExpr;
        }
    }
}
