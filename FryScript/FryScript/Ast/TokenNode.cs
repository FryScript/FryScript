using FryScript.Compilation;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    [ExcludeFromCodeCoverage]
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
