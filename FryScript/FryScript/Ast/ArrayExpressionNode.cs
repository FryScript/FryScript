using FryScript.Compilation;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FryScript.Ast
{

    public class ArrayExpressionNode : AstNode
    {
        private static ConstructorInfo ScriptArray_ObjectArrayCtor =
            typeof (ScriptArray).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public,
                null,
                CallingConventions.Any,
                new[] {typeof (object[])},
                null);

        private static ConstructorInfo ScriptArray_Ctor =
            typeof(ScriptArray).GetConstructor(
                BindingFlags.Instance | BindingFlags.Public,
                null,
                CallingConventions.Any,
                new Type[]{},
                null);

        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var arrayItems = ChildNodes.First();

            Expression newScriptArrayExpr;

            if (arrayItems.ChildNodes.Length == 0)
            {
                newScriptArrayExpr = Expression.New(ScriptArray_Ctor);
            }
            else
            {
                var initExprs = arrayItems.ChildNodes.Select(c => c.GetExpression(scope));
                var objectArrayExpr = Expression.NewArrayInit(typeof (object), initExprs);
                newScriptArrayExpr = Expression.New(ScriptArray_ObjectArrayCtor, objectArrayExpr);
            }

            return newScriptArrayExpr;
        }
    }
}
