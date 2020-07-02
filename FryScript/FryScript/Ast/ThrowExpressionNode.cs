using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.Parsing;
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

            var isRethrow = ChildNodes.Length == 1;

            var toThrowExpr = isRethrow
                ? HandleRethrow(scope)
                : HandleThrow(scope);

            var callThrowExpr = WrapException(scope, toThrowExpr, isRethrow);

            var throwExpr = Expression.Throw(callThrowExpr, typeof(object));

            return throwExpr;
        }

        private MethodCallExpression WrapException(Scope scope, Expression toThrowExpr, bool rethrow)
        {
            var location = ParseNode.Span.Location;

            return Expression.Call(typeof(FryScriptException),
                            nameof(FryScriptException.Throw),
                            null,
                            toThrowExpr,
                            scope.TryGetData(ScopeData.CurrentException, out ParameterExpression currentException)
                                ? currentException
                                : ExpressionHelper.Null(typeof(Exception)),
                            Expression.Constant(CompilerContext.Name ?? CompilerContext?.Uri?.AbsoluteUri ?? string.Empty),
                            Expression.Constant(location.Line),
                            Expression.Constant(location.Column),
                            Expression.Constant(rethrow)
                            );
        }

        private Expression HandleThrow(Scope scope)
        {
            var throwTarget = ChildNodes.Skip(1).First();
            var nullNode = throwTarget.FindChild<NullNode>();

            if (nullNode != null)
                throw ExceptionHelper.InvalidContext(Keywords.Null, nullNode);

            return throwTarget.GetExpression(scope);
        }

        private Expression HandleRethrow(Scope scope)
        {
            if (!scope.TryGetData(ScopeData.CurrentException, out ParameterExpression exceptionExpr))
                throw ExceptionHelper.InvalidContext(Keywords.Throw, this);

            return exceptionExpr;
        }
    }
}
