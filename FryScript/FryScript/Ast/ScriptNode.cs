﻿using FryScript.Compilation;
using FryScript.Debugging;
using FryScript.Parsing;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ScriptNode : AstNode, IRootNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var paramExpr = scope.AddKeywordMember<ScriptObject>(Keywords.This, this);

            scope = scope.New();

            var bodyExpr = scope.ScopeBlock(GetChildExpression(scope));

            if (CompilerContext.HasDebugHook)
                bodyExpr = WrapDebugStack(scope, s => bodyExpr, DebugEvent.ScriptInitializing, DebugEvent.ScriptInitialized);

            var lambda = Expression.Lambda<Func<ScriptObject, object>>(bodyExpr, paramExpr);

            return lambda;
        }

        public Func<ScriptObject, object> Compile(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var expression = (Expression<Func<ScriptObject, object>>)GetExpression(scope);
            var func = expression.Compile();

            return func;
        }

        public Func<IScriptObject, object> Compile2(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var expression = (Expression<Func<IScriptObject, object>>)GetExpression2(scope);
            var func = expression.Compile();

            return func;
        }

        public Expression GetExpression2(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var paramExpr = scope.AddKeywordMember<IScriptObject>(Keywords.This, this);

            var bodyExpr = GetChildExpression(scope);

            if (CompilerContext.HasDebugHook)
                bodyExpr = WrapDebugStack(scope, s => bodyExpr, DebugEvent.ScriptInitializing, DebugEvent.ScriptInitialized);

            var lambda = Expression.Lambda<Func<IScriptObject, object>>(bodyExpr, paramExpr);

            return lambda;
        }
    }
}
