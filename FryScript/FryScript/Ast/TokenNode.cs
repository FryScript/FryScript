﻿using FryScript.Compilation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class TokenNode : AstNode
    {
        public override object Value
        {
            get { return ParseNode.ChildNodes.First().Token.Value; }
        }

        public override string ValueString
        {
            get { return (string) Value; }
        }

        public override Expression GetExpression(Scope scope)
        {
            throw new NotImplementedException();
        }
    }
}
