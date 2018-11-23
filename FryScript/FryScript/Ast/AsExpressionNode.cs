using FryScript.Compilation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class AsExpressionNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
                return GetChildExpression(scope);

            var left = ChildNodes.Skip(2).First();

            var right = ChildNodes.First();

            if (left is IdentifierNode identifier)
            {
                var paramExpr = identifier.CreateIdentifier(scope);
                var assignExpr = Expression.Assign(paramExpr, right.GetExpression(scope));

                return assignExpr;
            }

            var tupleNames = left as TupleNamesNode;

            var assignTuple = Transform<AssignTupleExpressionNode>(new []{
                left,
                null,
                right
            });

            var assignTupleExpr = assignTuple.GetExpression(scope);

            return assignTupleExpr;
        }
    }
}
