using FryScript.Compilation;
using FryScript.Debugging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Helpers
{
    [ExcludeFromCodeCoverage]
    public static class DebugExpressionHelper
    {
        public static Expression GetCallStackExpression(Scope scope, Func<Scope, Expression> func, string name, int line, int column, int length, DebugHook debugHook, DebugEvent pushEvent = DebugEvent.PushStackFrame, DebugEvent popEvent = DebugEvent.PopStackFrame)
        {
            var startFrameExpr = GetDebugEventExpression(pushEvent, name, line, column, length, debugHook);
            var endFrameExpr = GetDebugEventExpression(popEvent, name, line, column, length, debugHook);

            var resultExpr = Expression.Variable(typeof(object), scope.GetTempName(TempPrefix.Debug));
            var assignResultExpr = Expression.Assign(resultExpr, func(scope));
            var blockExpr = Expression.Block(
                new[] { resultExpr },
                startFrameExpr,
                assignResultExpr,
                endFrameExpr,
                resultExpr
                );

            return blockExpr;
        }

        public static Expression GetDebugVariablesExpression(Scope scope)
        {
            return Expression.NewArrayInit(
                typeof(DebugVariable),
                scope.GetExpressions()
                    .Select(e => Expression.Call(
                        typeof(DebugVariable),
                        "Create",
                        null,
                        Expression.Constant(e.Name),
                        e)));
        }

        public static Expression GetDebugEventExpression(DebugEvent debugEvent, Scope scope, Func<Scope, Expression> func, string name, int line, int column, int length, DebugHook debugHook)
        {
            debugHook = debugHook ?? throw new ArgumentNullException(nameof(debugHook));

            var createContextExpr = GetDebugContextExpression(debugEvent, scope, name, line, length, column);

            var debugHookExpr = Expression.Constant(debugHook);
            var invokeExpr = Expression.Invoke(debugHookExpr, createContextExpr);

            var eventExpr = func == null
                ? (Expression)invokeExpr
                : Expression.Block(typeof(object), new[] { invokeExpr, func(scope) });

            return eventExpr;
        }

        public static Expression GetDebugEventExpression(DebugEvent debugEvent, string name, int line, int column, int length, DebugHook debugHook)
        {
            return GetDebugEventExpression(debugEvent, null, null, name, line, column, length, debugHook);
        }

        private static Expression GetDebugContextExpression(DebugEvent debugEvent, Scope scope, string name, int line, int length, int column)
        {
            var debugEventExpr = Expression.Constant(debugEvent);
            var nameExpr = Expression.Constant(name, typeof(string));
            var lineExpr = Expression.Constant(line);
            var columnExpr = Expression.Constant(column);
            var lengthExpr = Expression.Constant(length);
            var debugVarsExprs = scope != null
                ? GetDebugVariablesExpression(scope)
                : Expression.Constant(null, typeof(DebugVariable[]));

            var createContextExpr = Expression.Call(
                typeof(DebugContext),
                "Create",
                null,
                debugEventExpr,
                nameExpr,
                lineExpr,
                columnExpr,
                lengthExpr,
                debugVarsExprs);

            return createContextExpr;
        }
    }
}
