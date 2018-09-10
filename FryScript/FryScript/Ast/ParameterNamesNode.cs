using FryScript.Compilation;
using System;
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

        public void DeclareParameters(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
            {
                var identifier = ChildNodes.First();
                identifier.CreateIdentifier(scope);
            }


            if (ChildNodes.Length == 2)
            {
                var parameterNames = (ParameterNamesNode) ChildNodes.First();
                parameterNames.DeclareParameters(scope);

                var identifier = ChildNodes.Skip(1).First();
                identifier.CreateIdentifier(scope);
            }
        }
    }
}
