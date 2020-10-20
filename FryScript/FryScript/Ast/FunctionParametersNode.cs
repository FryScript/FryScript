using FryScript.Compilation;
using FryScript.Parsing;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class FunctionParametersNode : AstNode
    {
        public int Count => ChildNodes.Count();
        
        public override Expression GetExpression(Scope scope)
        {
            throw new NotImplementedException();
        }

        public virtual void DeclareParameters(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 0)
                return;

            var parameters = ChildNodes.First();

            if (parameters is ParameterNamesNode)
            {
                (parameters as ParameterNamesNode).DeclareParameters(scope);
                return;
            }

            if (parameters is ParamsNode)
            {
                scope.AddKeywordMember<ScriptParams>(Keywords.Params, parameters);
                return;
            }

            if (parameters is IdentifierNode)
            {
                parameters.CreateIdentifier(scope);
                return;
            }
        }
    }
}
