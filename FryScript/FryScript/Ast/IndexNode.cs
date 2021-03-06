﻿using FryScript.Compilation;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class IndexNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            return GetChildExpression(scope);
        }
    }
}
