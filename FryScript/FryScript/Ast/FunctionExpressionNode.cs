using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FryScript.Ast
{
    public class FunctionExpressionNode: AstNode
    {
        private static readonly ConstructorInfo ScriptFunction_DelegateCtor =
            typeof (ScriptFunction).GetConstructor(new[] {typeof (Delegate)});

        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var parameters = ChildNodes.First() as FunctionParametersNode;
            var block = ChildNodes.Skip(1).First();

            scope = scope.New(resetDataBag: true);

            parameters.DeclareParameters(scope);

            var parameterExprs = scope.GetLocalExpressions().ToArray();

            scope = scope.New();
            var returnTarget = scope.SetData(ScopeData.ReturnTarget, Expression.Label(typeof (object), scope.GetTempName(TempPrefix.ReturnTarget)));

            var blockExpr = block.ChildNodes.Length == 0
                ? ExpressionHelper.Null()
                : block.GetExpression(scope);

            Expression returnExpr = Expression.Label(returnTarget, blockExpr);

            if (CompilerContext.HasDebugHook)
            {
                var span = ParseNode.Span;
                var location = span.Location;

                returnExpr = DebugExpressionHelper.GetCallStackExpression(scope, s => returnExpr, CompilerContext.Name, location.Line, location.Column, span.Length, CompilerContext.DebugHook);
            }

            var lambdaType =
                Expression.GetFuncType(parameterExprs.Select(p => p.Type).Concat(new[] {typeof (object)}).ToArray());

            var lambdaExpr = Expression.Lambda(lambdaType, returnExpr, parameterExprs);

            var newFuncExpr = Expression.New(ScriptFunction_DelegateCtor, lambdaExpr);

            return newFuncExpr;
        }
    }
}
