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

            var values = ChildNodes.First();
            
            var identifiers = ChildNodes.Skip(2).First();

            if (values is TupleNamesNode tuplesNames)
                values = Transform<AssignTupleExpressionNode>(values);

            if (identifiers is IdentifierNode identifier)
            {
                identifier.CreateIdentifier(scope);
                var assignExpr = identifier.SetIdentifier(scope, values.GetExpression(scope));

                return assignExpr;
            }

            var declareTuple = Transform<TupleDeclarationNode>(new[]{
                null,
                identifiers,
                null,
                values
            }) as TupleDeclarationNode;
            declareTuple.AllowOut = true;

            var declareTupleExpr = declareTuple.GetExpression(scope);

            return declareTupleExpr;
        }
    }
}
