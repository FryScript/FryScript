using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class TernaryExpressionNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
                return GetChildExpression(scope);

            scope = scope.New(this);
            
            if (scope.Hoisted)
                return TryMakeAwaitable(scope);

            var tempExpr = scope.AddTempMember(TempPrefix.TernaryFlag, this, typeof(object));

            var testExpr = ChildNodes.First().GetExpression(scope);
            var leftExpr = ChildNodes.Length == 2
                ? tempExpr
                : ChildNodes.Skip(1).First().GetExpression(scope);
            var rightExpr = ChildNodes.Skip(ChildNodes.Length == 2 ? 1 : 2).First().GetExpression(scope);

            var convertTempExpr = ExpressionHelper.DynamicConvert(tempExpr, typeof(bool));
            var assignTempExpr = Expression.Assign(tempExpr, testExpr);

            var ifExpr = Expression.Condition(convertTempExpr, leftExpr, rightExpr, typeof(object));

            var blockExpr = scope.ScopeBlock(assignTempExpr, ifExpr);

            return blockExpr;
        }

        private Expression TryMakeAwaitable(Scope scope)
        {
            Scope testScope = scope.Clone(),
                leftScope = scope.Clone(),
                rightScope = scope.Clone();

            testScope.SetData(ScopeData.AwaitContexts, new List<Expression>());
            leftScope.SetData(ScopeData.AwaitContexts, new List<Expression>());
            rightScope.SetData(ScopeData.AwaitContexts, new List<Expression>());

            var tempExpr = scope.AddTempMember(TempPrefix.TernaryFlag, this, typeof(object));

            var testExpr = ChildNodes.First().GetExpression(testScope);
            var leftExpr = ChildNodes.Length == 2
                ? tempExpr
                : ChildNodes.Skip(1).First().GetExpression(leftScope);
            var rightExpr = ChildNodes.Skip(ChildNodes.Length == 2 ? 1 : 2).First().GetExpression(rightScope);

            var convertTempExpr = ExpressionHelper.DynamicConvert(tempExpr, typeof(bool));
            var assignTempExpr = Expression.Assign(tempExpr, testExpr);


            var testAwaitExpr = ExpressionHelper.AwaitStatement(testScope, assignTempExpr);
            var ifExpr = Expression.Condition(
                convertTempExpr,
                ExpressionHelper.AwaitStatement(leftScope, Expression.Assign(tempExpr, leftExpr)),
                ExpressionHelper.AwaitStatement(rightScope, Expression.Assign(tempExpr, rightExpr))
                );

            scope.TryGetData(ScopeData.AwaitContexts, out List<Expression> awaitContexts);
            awaitContexts.Add(scope.ScopeBlock(testAwaitExpr, ifExpr));

            return tempExpr;
        }
    }
}
