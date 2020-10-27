using FryScript.Compilation;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class LiteralNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            _ = scope ?? throw new ArgumentNullException(nameof(scope));

            var value = Value;

            if (value is double)
                value = Convert.ToSingle(value);

            return Expression.Constant(value, typeof(object));
        }
    }
}
