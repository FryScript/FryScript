using FryScript.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class FibreExpressionNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var functionNode = ChildNodes.Skip(1).First();

            var parameters = functionNode.ChildNodes.First() as FunctionParametersNode;
            var body = functionNode.ChildNodes.Skip(1).First();
            body = Transform<FibreBlockNode>(body.ChildNodes);

            var paramametersScope = scope.New(this, true, false);

            parameters.DeclareParameters(paramametersScope);

            var fibreScope = paramametersScope.New(this, true, true);

            var parameterExprs = paramametersScope.GetLocalExpressions().ToArray();

            if(parameterExprs.Length > 16)
                throw CompilerException.FromAst("A fibre cannot declare more than 16 parameters", parameters);

            var newFibreContextExpr = GetFibreContextExpression(fibreScope, body);
            newFibreContextExpr = fibreScope.ScopeBlock(typeof(ScriptFibreContext), newFibreContextExpr);

            var lambdaType = Expression.GetFuncType(parameterExprs.Select(p => p.Type).Concat(new[] { typeof(ScriptFibreContext) }).ToArray());
            
            var lambdaExpr = Expression.Lambda(lambdaType, newFibreContextExpr, parameterExprs);

            Expression newFibreExpr = Expression.Call(typeof(ScriptFibre), nameof(ScriptFibre.New), null, lambdaExpr);

            return newFibreExpr;
        }

        public virtual Expression GetFibreContextExpression(Scope scope, AstNode body)
        {
            var contextScope = scope.New(this);

            var contextParam = contextScope.SetData(ScopeData.FibreContext, Expression.Parameter(typeof(ScriptFibreContext), scope.GetTempName(TempPrefix.FibreContext)));
            var yieldLabels = contextScope.SetData(ScopeData.YieldLabels, new List<LabelTarget>());
            var yieldTarget = contextScope.SetData(ScopeData.YieldTarget, Expression.Label(typeof(object), scope.GetTempName(TempPrefix.YieldTarget)));
            contextScope.SetData(ScopeData.AwaitContexts, new List<Expression>());

            var bodyExpr = body.GetExpression(contextScope);

            bodyExpr = AddYieldSwitchExpression(contextScope, contextParam, bodyExpr, yieldLabels);
            bodyExpr = AddReturnExpression(yieldTarget, bodyExpr);

            var lambdaExpr = Expression.Lambda<Func<ScriptFibreContext, object>>(bodyExpr, contextParam);

            var initContextExpr = Expression.Call(typeof(ScriptFibreContext), nameof(ScriptFibreContext.New), null, lambdaExpr);

            return initContextExpr;
        }

        private Expression AddYieldSwitchExpression(Scope scope, ParameterExpression contextParam, Expression bodyExpr, List<LabelTarget> yieldLabels)
        {
            if (yieldLabels.Count == 0)
                return bodyExpr;

            var switchExpr = Expression.Switch(
                Expression.PropertyOrField(contextParam, nameof(ScriptFibreContext.YieldState)),
                yieldLabels.Select((l, i) =>
                    Expression.SwitchCase(Expression.Goto(l), Expression.Constant(i))).ToArray());

            return scope.ScopeBlock(switchExpr, bodyExpr);
        }

        private Expression AddReturnExpression(LabelTarget yieldTarget, Expression bodyExpr)
        {
            var returnExpr = Expression.Label(yieldTarget, bodyExpr);

            return returnExpr;
        }
    }
}
