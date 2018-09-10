using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class InvokeExpressionNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var target = ChildNodes.First();
            var args = ChildNodes.Skip(1).First();

            var targetExpr = target.GetExpression(scope);
            var argExprs = args.ChildNodes.Select(c => c.GetExpression(scope)).ToArray();
            var invokeExpr = ExpressionHelper.DynamicInvoke(targetExpr, argExprs);
   
            return invokeExpr;
        }
    }
}
