using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class EmptyStatement: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            _ = scope ?? throw new ArgumentNullException(nameof(scope));

            return ExpressionHelper.Null();
        }
    }
}
