using FryScript.Compilation;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FryScript.Ast
{
    public class TupleExpressionNode : AstNode
    {
        private static ConstructorInfo ScriptTuple_ObjectArrayCtor =
            typeof(ScriptTuple).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public,
                null,
                CallingConventions.Any,
                new[] { typeof(object[]) },
                null);

        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var tupleItemExprs = ChildNodes.First().ChildNodes.Select(e => e.GetExpression(scope));

            var tupleItemsArrayExpr = Expression.NewArrayInit(typeof(object), tupleItemExprs);
            var newTupleExpr = Expression.New(ScriptTuple_ObjectArrayCtor, tupleItemsArrayExpr);
            var convertTupleExpr = Expression.Convert(newTupleExpr, typeof(object));

            return convertTupleExpr;
        }
    }
}
