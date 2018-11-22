using FryScript.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class TupleNamesNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ParameterExpression> GetIdentifiers(Scope scope, List<ParameterExpression> exprs = null)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            exprs = exprs ?? new List<ParameterExpression>();

            var firstNode = ChildNodes.First();
            if (firstNode is IdentifierNode identifier)
                exprs.Add(identifier.GetExpression(scope) as ParameterExpression);

            if (firstNode is TupleNamesNode tupleNames)
                tupleNames.GetIdentifiers(scope, exprs);

            var secondNode = ChildNodes.Skip(1).First() as IdentifierNode;
            exprs.Add(secondNode.GetExpression(scope) as ParameterExpression);

            return exprs;
        }

        public IEnumerable<ParameterExpression> DeclareVariables(Scope scope, List<ParameterExpression> exprs  = null)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            exprs = exprs ?? new List<ParameterExpression>();

            var firstNode = ChildNodes.First();
            if (firstNode is IdentifierNode identifier)
                exprs.Add(identifier.CreateIdentifier(scope));

            var secondNode = ChildNodes.Skip(1).First() as IdentifierNode;
            exprs.Add(secondNode.CreateIdentifier(scope));

            return exprs;
        }
    }
}
