using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class YieldStatementNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var expression = ChildNodes.Skip(1).FirstOrDefault()
                ?? ChildNodes.Skip(2).FirstOrDefault();

            var isYieldReturn = IsYieldReturn();

            if (!scope.TryGetData(ScopeData.YieldTarget, out LabelTarget yieldTarget))
                throw ExceptionHelper.InvalidContext(Keywords.Yield, this);

            scope.TryGetData(ScopeData.YieldLabels, out List<LabelTarget> yieldLabels);

            scope.TryGetData(ScopeData.FibreContext, out ParameterExpression contextFrame);

            var yieldStateExpr = isYieldReturn
                ? Expression.Constant(ScriptFibreContext.CompletedState)
                : Expression.Constant(yieldLabels.Count);
            var yieldLabel = Expression.Label(typeof(void), scope.GetTempName(TempPrefix.Yield));

            if(!isYieldReturn)
                yieldLabels.Add(yieldLabel);

            var assignYieldStateExpr = Expression.Assign(
                Expression.Property(contextFrame, nameof(ScriptFibreContext.YieldState)),
                yieldStateExpr
                );

            var returnValueExpr = expression?.GetExpression(scope) ?? Expression.Constant(ScriptFibreContext.NoResult);

            var returnExpr = Expression.Return(yieldTarget, returnValueExpr, typeof(object));

            if (isYieldReturn)
                return Expression.Block(typeof(object), assignYieldStateExpr, returnExpr);

            var yieldBlock = Expression.Block(typeof(object),
                assignYieldStateExpr,
                returnExpr,
                Expression.Label(yieldLabel),
                ExpressionHelper.Null());

            return yieldBlock;
        }

        internal bool IsYieldReturn()
        {
            return (ChildNodes.Length == 2 && ChildNodes[1] == null)
                || ChildNodes.Length == 3;
        }
    }
}
