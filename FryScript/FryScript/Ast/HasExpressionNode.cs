using FryScript.Binders;
using FryScript.Compilation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class HasExpressionNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
                return GetChildExpression(scope);

            var identifier = ChildNodes.First();
            var member = ChildNodes.Skip(2).First();

            var identifierExpr = identifier.GetExpression(scope);

            var binder = BinderCache.Current.HasOperationBinder(member.ValueString);
            var hasExpr = Expression.Dynamic(binder, typeof(object), identifierExpr);

            return hasExpr;
        }
    }
}
