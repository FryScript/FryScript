using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ThrowExpressionNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));
            var toThrow = ChildNodes.Skip(1).First();

            var location = ParseNode.Span.Location;

            var toThrowExpr = toThrow.GetExpression(scope);

            var callThrowExpr = Expression.Call(typeof(FryScriptException),
                nameof(FryScriptException.Throw),
                null,
                toThrowExpr,
                scope.TryGetData(ScopeData.CurrentException, out ParameterExpression currentException)
                    ? currentException
                    : ExpressionHelper.Null(typeof(Exception)),
                Expression.Constant(CompilerContext.Name ?? CompilerContext?.Uri?.AbsoluteUri ?? string.Empty),
                Expression.Constant(location.Line),
                Expression.Constant(location.Column)
                );

            var throwExpr = Expression.Throw(callThrowExpr, typeof(object));

            return throwExpr;
        }
    }
}
