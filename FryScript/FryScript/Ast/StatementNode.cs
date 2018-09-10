using FryScript.Compilation;
using FryScript.Debugging;
using FryScript.Helpers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class StatementNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var expr = CompilerContext.HasDebugHook
                ? WrapDebugExpression(DebugEvent.Statement, scope, s => GetChildExpression(s))
                : GetChildExpression(scope);

            if (scope.Hoisted && scope.TryGetData(ScopeData.AwaitContexts, out List<Expression> awaitContexts) && awaitContexts.Count != 0)
                return ExpressionHelper.AwaitStatement(scope, expr);

            return expr;
        }
    }
}
