using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class NullNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            return ExpressionHelper.Null();
        }
    }
}
