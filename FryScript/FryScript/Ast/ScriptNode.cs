using FryScript.Compilation;
using FryScript.Debugging;
using FryScript.Parsing;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ScriptNode : DebugNode, IRootNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var paramExpr = scope.AddKeywordMember<IScriptObject>(Keywords.This, this);

            scope = scope.New(this);

            var bodyExpr = scope.ScopeBlock(GetChildExpression(scope));

            if (CompilerContext.HasDebugHook)
            {
                var wrappedExpr = bodyExpr;
                bodyExpr = WrapDebugStack(scope, s => wrappedExpr, DebugEvent.ScriptInitializing, DebugEvent.ScriptInitialized);
            }

            var lambda = Expression.Lambda<Func<IScriptObject, object>>(bodyExpr, paramExpr);

            return lambda;
        }

        public Func<IScriptObject, object> Compile(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var expression = (Expression<Func<IScriptObject, object>>)GetExpression(scope);
            var func = expression.Compile();

            return func;
        }
    }
}
