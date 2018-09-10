using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class WhileStatementNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var condition = ChildNodes.Skip(1).First();
            var statement = ChildNodes.Skip(2).First();

            scope = scope.New();

            var breakTarget = scope.SetData(ScopeData.BreakTarget, Expression.Label(typeof(object), scope.GetTempName(TempPrefix.BreakTarget)));
            var continueTarget = scope.SetData(ScopeData.ContinueTarget, Expression.Label(typeof(object), scope.GetTempName(TempPrefix.ContinueTarget)));

            var conditionExpr = ExpressionHelper.DynamicConvert(condition.GetExpression(scope), typeof(bool));
            var statementExpr = statement.GetExpression(scope);

            var ifBreakExpr = Expression.IfThenElse(
                conditionExpr,
                statementExpr,
                Expression.Break(breakTarget, ExpressionHelper.Null(), typeof(object))
                );

            var continueLabelExpr = Expression.Label(continueTarget, ExpressionHelper.Null());

            var loopBodyExpr = scope.ScopeBlock(ifBreakExpr, continueLabelExpr);

            var loopExpr = Expression.Loop(loopBodyExpr, breakTarget);

            return loopExpr;
        }
    }
}
