using FryScript.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ParameterNamesNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ParameterExpression> DeclareParameters(Scope scope, List<ParameterExpression> exprs = null)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            exprs = exprs ?? new List<ParameterExpression>();

            if (ChildNodes.Length == 1)
            {
                var identifier = ChildNodes.First();
                exprs.Add(identifier.CreateIdentifier(scope));
            }


            if (ChildNodes.Length == 2)
            {
                var parameterNames = (ParameterNamesNode) ChildNodes.First();
                parameterNames.DeclareParameters(scope, exprs);

                var identifier = ChildNodes.Skip(1).First();
                exprs.Add(identifier.CreateIdentifier(scope));
            }

            return exprs;
        }
    }
}
