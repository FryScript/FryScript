﻿using FryScript.Compilation;
using FryScript.Debugging;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ExpressionNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (CompilerContext.HasDebugHook)
                return WrapDebugExpression(DebugEvent.Expression, scope, s => GetChildExpression(s));

            Expression expr = CompilerContext.DetailedExceptions == true
                ? ExpressionHelper.WrapNativeCall(GetChildExpression(scope), this, scope)
                : GetChildExpression(scope);

            return expr;
        }
    }
}
