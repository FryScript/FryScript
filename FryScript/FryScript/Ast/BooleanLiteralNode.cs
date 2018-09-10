using FryScript.Compilation;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class BooleanLiteralNode : TokenNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var boolExpr = Expression.Constant(bool.Parse(ValueString));

            return Expression.Convert(boolExpr, typeof(object));
        }
    }
}
