using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ConditionalAssignExpressionNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
                return GetChildExpression(scope);

            if (scope.Hoisted)
                return TryMakeAwaitable(scope);

            scope = scope.New(this);

            var left = (IdentifierExpressionNode)ChildNodes.First();
            var right = ChildNodes.Skip(2).First();

            var tempExpr = scope.AddTempMember(TempPrefix.ConditionalAssign, this);

            var leftExpr = left.GetIdentifier(scope);
            var rightExpr = right.GetExpression(scope);
            var assignTempExpr = Expression.Assign(tempExpr, leftExpr);
            var testExpr = Expression.NotEqual(tempExpr, ExpressionHelper.Null());
            var conditionExpr = Expression.Condition(testExpr, tempExpr, left.SetIdentifier(scope, rightExpr));

            var blockExpr = scope.ScopeBlock(assignTempExpr, conditionExpr);

            return blockExpr;
        }

        private Expression TryMakeAwaitable(Scope scope)
        {
            scope = scope.New(this);

            var rightScope = scope.Clone();
            rightScope.SetData(ScopeData.AwaitContexts, new List<Expression>());

            var left = (IdentifierExpressionNode)ChildNodes.First();
            var right = ChildNodes.Skip(2).First();

            var tempExpr = scope.AddTempMember(TempPrefix.ConditionalAssign, this);

            var leftExpr = left.GetIdentifier(scope);
            var rightExpr = right.GetExpression(rightScope);
            var assignTempExpr = Expression.Assign(tempExpr, leftExpr);
            var testExpr = Expression.NotEqual(tempExpr, ExpressionHelper.Null());
            var conditionExpr = Expression.Condition(testExpr, tempExpr, left.SetIdentifier(scope, rightExpr));

            var awaitStatement = ExpressionHelper.AwaitStatement(rightScope);
            var awaitConditionExpr = Expression.Condition(
                Expression.Equal(tempExpr, ExpressionHelper.Null()), 
                awaitStatement, 
                ExpressionHelper.Null());
            var awaitBlockExpr = scope.ScopeBlock(assignTempExpr, awaitConditionExpr);
            scope.TryGetData(ScopeData.AwaitContexts, out List<Expression> awaitContexts);
            awaitContexts.Add(awaitBlockExpr);

            return conditionExpr;
        }
    }
}