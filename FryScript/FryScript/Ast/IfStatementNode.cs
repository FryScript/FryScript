using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class IfStatementNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (scope.Hoisted)
                return TryMakeAwaitable(scope);

            var conditionExpr = ExpressionHelper.DynamicConvert(ChildNodes.Skip(1).First().GetExpression(scope), typeof(bool));

            var thenScope = scope.New(this);
            var thenExpr = ChildNodes.Skip(2).First().GetExpression(thenScope);
            var thenBlockExpr = thenScope.ScopeBlock(thenExpr);

            var elseScope = scope.New(this);
            var elseExpr = ChildNodes.Length == 3
                ? ExpressionHelper.Null()
                : ChildNodes.Skip(4).First().GetExpression(elseScope);
            var elseBlockExpr = elseScope.ScopeBlock(elseExpr);

            var ifThenElseExpr = Expression.Condition(conditionExpr, thenBlockExpr, elseBlockExpr, typeof(object));
            return ifThenElseExpr;
        }

        protected internal virtual Expression TryMakeAwaitable(Scope scope)
        {
            var conditionScope = scope.Clone();
            conditionScope.SetData(ScopeData.AwaitContexts, new List<Expression>());

            var conditionExpr = ExpressionHelper.DynamicConvert(ChildNodes.Skip(1).First().GetExpression(conditionScope), typeof(bool));

            var thenScope = scope.New(this);
            var thenExpr = ChildNodes.Skip(2).First().GetExpression(thenScope);
            var thenBlockExpr = thenScope.ScopeBlock(thenExpr);

            var elseScope = scope.New(this);
            var elseExpr = ChildNodes.Length == 3
                ? ExpressionHelper.Null()
                : ChildNodes.Skip(4).First().GetExpression(elseScope);
            var elseBlockExpr = elseScope.ScopeBlock(elseExpr);

            var ifThenElseExpr = Expression.Condition(conditionExpr, thenBlockExpr, elseBlockExpr, typeof(object));
            var awaitIfThenElseExpr = ExpressionHelper.AwaitStatement(conditionScope, ifThenElseExpr);

            return awaitIfThenElseExpr;
        }
    }
}
