using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.Parsing;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ContinueStatementNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (!scope.TryGetData(ScopeData.ContinueTarget, out LabelTarget continueTarget))
                throw ExceptionHelper.InvalidContext(Keywords.Continue, this);

            return Expression.Goto(continueTarget, ExpressionHelper.Null(), typeof(object));
        }
    }
}
