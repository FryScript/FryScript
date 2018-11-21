using FryScript.Compilation;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class DefaultNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            if (ChildNodes.Length == 1)
                return GetChildExpression(scope);

            throw new NotImplementedException(ParseNode.Term.Name + " no ast defined");
        }
    }
}
