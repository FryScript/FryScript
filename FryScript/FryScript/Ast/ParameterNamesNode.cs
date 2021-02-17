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

        public virtual void DeclareParameters(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            DeclareParameters(scope, new List<IdentifierNode>())
                .ForEach(i => i.CreateIdentifier(scope));
        }

        protected internal virtual List<IdentifierNode> DeclareParameters(Scope scope, List<IdentifierNode> identifiers = null)
        {
            if (ChildNodes.Length == 1 && ChildNodes[0] is IdentifierNode firstIdentifier)
            {
                identifiers.Add(firstIdentifier);
            }


            if (ChildNodes.Length == 2 && ChildNodes[1] is IdentifierNode secondIdentifier)
            {
                var parameterNames = (ParameterNamesNode) ChildNodes.First();
                parameterNames.DeclareParameters(scope, identifiers);

                identifiers.Add(secondIdentifier);
            }

            return identifiers;
        }
    }
}
