using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class InvokeMemberExpressionNode: DebugNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var target = ChildNodes.First();
            var member = ChildNodes.Skip(1).First().ValueString;
            var args = ChildNodes.Skip(2).First();

            var targetExpr = target.GetExpression(scope);
            var argExprs = args.ChildNodes.Select(c => c.GetExpression(scope)).ToArray();
            var invokeExpr = ExpressionHelper.DynamicInvokeMember(targetExpr, member, argExprs);

            if (CompilerContext.HasDebugHook)
            {
                return WrapDebugStack(scope, s => invokeExpr);
            }

            return invokeExpr;
        }
    }
}
