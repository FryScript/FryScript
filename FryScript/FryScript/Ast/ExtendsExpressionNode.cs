using FryScript.Binders;
using FryScript.Compilation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ExtendsExpressionNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
                return GetChildExpression(scope);

            var identifier = ChildNodes.First();
            var type = ChildNodes.Skip(2).First();

            var instanceExpr = identifier.GetExpression(scope);
            var valueExpr = type.GetExpression(scope);

            var binder = BinderCache.Current.ExtendsOperationBinder();
            return Expression.Dynamic(binder, typeof(object), instanceExpr, valueExpr);
        }
    }
}
