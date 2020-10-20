using FryScript.Compilation;
using FryScript.Debugging;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ExpressionNode : DebugNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (CompilerContext.HasDebugHook)
                return WrapDebugExpression(DebugEvent.Expression, scope, s => GetChildExpression(s));

            Expression expr = CompilerContext.DetailedExceptions == true
                ? GetDetailedExceptionExpression(GetChildExpression(scope), this, scope)
                : GetChildExpression(scope);

            return expr;
        }
    }
}
