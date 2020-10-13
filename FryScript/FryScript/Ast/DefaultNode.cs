using FryScript.Compilation;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class DefaultNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            throw new NotImplementedException("No ast defined");
        }
    }
}
