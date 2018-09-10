using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.Parsing;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ReturnStatementNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (!scope.TryGetData(ScopeData.ReturnTarget, out LabelTarget returnTarget))
                throw ExceptionHelper.InvalidContext(Keywords.Return, this);

            if (ChildNodes.Length == 1)
                return Expression.Return(returnTarget, Expression.Constant(null), typeof(object));

            var expression = ChildNodes.Skip(1).First();
            return Expression.Return(returnTarget, expression.GetExpression(scope), typeof(object));
        }
    }
}
