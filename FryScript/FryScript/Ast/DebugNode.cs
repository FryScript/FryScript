using FryScript.Compilation;
using FryScript.Debugging;
using FryScript.Helpers;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public abstract class DebugNode : AstNode
    {
        protected internal virtual Expression WrapDebugExpression(DebugEvent debugEvent, Scope scope, Func<Scope, Expression> func)
        {
            var span = ParseNode.Span;
            var location = span.Location;

            // Request a break point
            var bp = CompilerContext.DebugHook.NewBreakpoint(CompilerContext.Uri, location.Line);

            // Get the debug vars
            var variablesExpr = DebugExpressionHelper.GetDebugVariablesExpression(scope);

            var bpExpr = Expression.Constant(bp);
            var bpExecuteExpr = Expression.Call(bpExpr, nameof(Breakpoint.Execute), null, variablesExpr);

            var expr = func(scope);

            return Expression.Block(typeof(object), new[] { bpExecuteExpr, expr });

            //return DebugExpressionHelper.GetDebugEventExpression(
            //    debugEvent,
            //    scope,
            //    func,
            //    CompilerContext.Name,
            //    location.Line,
            //    location.Position,
            //    span.Length,
            //    CompilerContext.DebugHook);
        }

        protected internal virtual Expression WrapDebugStack(Scope scope, Func<Scope, Expression> func, DebugEvent pushEvent = DebugEvent.PushStackFrame, DebugEvent popEvent = DebugEvent.PopStackFrame)
        {
            var span = ParseNode.Span;
            var location = span.Location;

            // Push the current stack
            var debugHookExpr = Expression.Constant(CompilerContext.DebugHook);
            var debugHookPushExpr = Expression.Call(debugHookExpr, nameof(DebugHook2.Push), null);
            var debugHookPopExpr = Expression.Call(debugHookExpr, nameof(DebugHook2.Pop), null);

            var resultExpr = Expression.Variable(typeof(object), scope.GetTempName(TempPrefix.Debug));
            var assignResultExpr = Expression.Assign(resultExpr, func(scope));

            return Expression.Block(new[] { resultExpr },
                debugHookPushExpr,
                assignResultExpr,
                debugHookPopExpr,
                resultExpr);

            //return DebugExpressionHelper.GetCallStackExpression(
            //    scope,
            //    func,
            //    CompilerContext.Name,
            //    location.Line,
            //    location.Column,
            //    span.Length,
            //    CompilerContext.DebugHook,
            //    pushEvent,
            //    popEvent);
        }

        protected internal virtual Expression GetDetailedExceptionExpression(Expression expression, AstNode astNode, Scope scope)
        {
            expression = expression ?? throw new ArgumentNullException(nameof(expression));
            astNode = astNode ?? throw new ArgumentNullException(nameof(astNode));
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var location = astNode.ParseNode.Span.Location;

            var exceptionExpr = Expression.Parameter(typeof(Exception), scope.GetTempName(TempPrefix.Exception));
            var wrapExpr = Expression.Call(typeof(FryScriptException),
                nameof(FryScriptException.FormatException),
                null,
                exceptionExpr,
                Expression.Constant(astNode.CompilerContext.Name),
                Expression.Constant(location.Line),
                Expression.Constant(location.Column));

            var scriptExExpr = Expression.Parameter(typeof(FryScriptException), scope.GetTempName(TempPrefix.Exception));
            var scriptExCatchExpr = Expression.Catch(scriptExExpr, Expression.Rethrow(expression.Type));

            var throwExpr = Expression.Throw(wrapExpr, expression.Type);
            var catchAllBlock = Expression.Catch(exceptionExpr, throwExpr);

            var tryCatchExpr = Expression.TryCatch(expression, scriptExCatchExpr, catchAllBlock);

            return tryCatchExpr;
        }
    }
}
