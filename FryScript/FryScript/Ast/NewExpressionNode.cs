using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class NewExpressionNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
                return GetChildExpression(scope);

            var invoke = ChildNodes.Skip(1).First();
            var identifier = invoke.ChildNodes.First();
            var invokeArgs = invoke.ChildNodes.Skip(1).First();

            var instanceExpr = identifier.GetIdentifier(scope);
            var argExprs = invokeArgs.ChildNodes.Select(c => c.GetExpression(scope)).ToArray();
            var newExpr = ExpressionHelper.DynamicCreateInstance(instanceExpr, argExprs);

            return newExpr;
        }
    }
}
