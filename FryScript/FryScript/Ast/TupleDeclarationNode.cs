using FryScript.Compilation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class TupleDeclarationNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            return ChildNodes.First().GetExpression(scope);
        }
    }
}
