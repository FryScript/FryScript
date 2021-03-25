using FryScript.Compilation;
using FryScript.Helpers;
using FryScript.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class BooleanExpressionNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 1)
                return GetChildExpression(scope);

            var left = ChildNodes.First();
            var op = ChildNodes.Skip(1).First();
            var right = ChildNodes.Skip(2).First();

            if (scope.Hoisted)
                return TryMakeAwaitable(left, op, right, scope);

            return GetBooleanExpression(left.GetExpression(scope), op.ValueString, right.GetExpression(scope));
        }

        private Expression GetBooleanExpression(Expression leftExpr, string op, Expression rightExpr)
        {
            leftExpr = ExpressionHelper.DynamicConvert(leftExpr, typeof(bool));
            rightExpr = ExpressionHelper.DynamicConvert(rightExpr, typeof(bool));

            switch (op)
            {
                case Operators.And:
                    return Expression.Convert(Expression.MakeBinary(ExpressionType.AndAlso, leftExpr, rightExpr), typeof(object));
                default:
                    return Expression.Convert(Expression.MakeBinary(ExpressionType.OrElse, leftExpr, rightExpr), typeof(object));
            }
        }

        private Expression TryMakeAwaitable(AstNode left, AstNode op, AstNode right, Scope scope)
        {
            Scope leftScope = scope.Clone(),
                rightScope = scope.Clone();

            var leftAwaitContexts = leftScope.SetData(ScopeData.AwaitContexts, new List<Expression>());
            var leftExpr = left.GetExpression(leftScope);

            var rightAwaitContexts = rightScope.SetData(ScopeData.AwaitContexts, new List<Expression>());
            var rightExpr = right.GetExpression(rightScope);

            if (leftAwaitContexts.Count != 0 || rightAwaitContexts.Count != 0)
            {
                Expression flagExpr = scope.AddTempMember(TempPrefix.BooleanFlag, this, typeof(bool));

                var convertLeftExpr = ExpressionHelper.DynamicConvert(leftExpr, typeof(bool));
                var assignFlagExpr = Expression.Assign(flagExpr, convertLeftExpr);
                var convertAssignFlagExpr = ExpressionHelper.DynamicConvert(assignFlagExpr, typeof(object));

                var leftAwaitExpr = ExpressionHelper.AwaitStatement(leftScope, convertAssignFlagExpr);

                var testExpr = op.ValueString == Operators.And
                    ? flagExpr
                    : Expression.Not(flagExpr);

                var convertRightExpr = ExpressionHelper.DynamicConvert(rightExpr, typeof(bool));
                assignFlagExpr = Expression.Assign(flagExpr, convertRightExpr);
                convertAssignFlagExpr = ExpressionHelper.DynamicConvert(assignFlagExpr, typeof(object));

                var rightAwaitExpr = ExpressionHelper.AwaitStatement(rightScope, convertAssignFlagExpr);

                var conditionExpr = Expression.IfThen(testExpr, rightAwaitExpr);

                scope.TryGetData(ScopeData.AwaitContexts, out List<Expression> awaitContexts);
                awaitContexts.Add(leftAwaitExpr);
                awaitContexts.Add(conditionExpr);

                var convertFlagExpr = ExpressionHelper.DynamicConvert(flagExpr, typeof(object));

                return convertFlagExpr;
            }

            var booleanExpr = GetBooleanExpression(leftExpr, op.ValueString, rightExpr);
            return booleanExpr;
        }
    }
}
