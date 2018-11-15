using FryScript.Compilation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class AssignTupleExpressionNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            return ChildNodes.First().GetExpression(scope);
        }
    }
}
