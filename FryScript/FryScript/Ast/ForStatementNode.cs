using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ForStatementNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var init = ChildNodes.Skip(1).First();
            var condition = ChildNodes.Skip(2).First();
            var action = ChildNodes.Skip(3).First();
            var statement = ChildNodes.Skip(4).First();

            var loopScope = scope.New(this);

            var breakTarget = loopScope.SetData(ScopeData.BreakTarget, Expression.Label(typeof(object), scope.GetTempName(TempPrefix.BreakTarget)));
            var continueTarget = loopScope.SetData(ScopeData.ContinueTarget, Expression.Label(typeof(object), scope.GetTempName(TempPrefix.ContinueTarget)));

            var initExpr = init.GetExpression(loopScope);
            var conditionExpr = ExpressionHelper.DynamicConvert(condition.GetExpression(loopScope), typeof(bool));
            var actionExpr = action.GetExpression(loopScope);

            var statementScope = loopScope.New(this);
            var statementExpr = statement.GetExpression(statementScope);

            var ifBreakExpr = Expression.IfThenElse(
                conditionExpr,
                statementExpr,
                Expression.Break(breakTarget, ExpressionHelper.Null(), typeof(object))
                );

            var continueLabelExpr = Expression.Label(continueTarget, ExpressionHelper.Null());

            var loopBodyExpr = statementScope.ScopeBlock(ifBreakExpr, continueLabelExpr, actionExpr);

            var loopExpr = Expression.Loop(loopBodyExpr, breakTarget);

            var loopInitExpr = loopScope.ScopeBlock(initExpr, loopExpr);

            return loopInitExpr;
        }
    }
}
