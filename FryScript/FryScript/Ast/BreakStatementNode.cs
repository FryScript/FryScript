using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.Parsing;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class BreakStatementNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (!scope.TryGetData(ScopeData.BreakTarget, out LabelTarget breakTarget))
                ExceptionHelper.InvalidContext(Keywords.Break, this);

            return Expression.Goto(breakTarget, ExpressionHelper.Null(), typeof(object));
        }
    }
}
