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

            if (right is TupleNamesNode tuplesNames)
                right = Transform<AssignTupleExpressionNode>(right);

            if (left is IdentifierNode identifier)
            {
                identifier.CreateIdentifier(scope);
                var assignExpr = identifier.SetIdentifier(scope, right.GetExpression(scope));

                return assignExpr;
            }

            var declareTuple = Transform<TupleDeclarationNode>(new[]{
                null,
                left,
                null,
                right
            }) as TupleDeclarationNode;
            declareTuple.AllowOut = true;

            var declareTupleExpr = declareTuple.GetExpression(scope);

            return declareTupleExpr;
        }
    }
}
