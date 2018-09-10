using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ObjectLiteralExpressionNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            scope = scope.New();

            var paramExpr = Expression.Parameter(typeof(ScriptObject), scope.GetTempName(TempPrefix.ObjectLiteral));
            scope.SetData(ScopeData.ObjectLiteralContext, paramExpr);

            if(ChildNodes.Length == 0)
                return Expression.New(typeof(ScriptObject));

            var bodyExpr = GetChildExpression(scope);

            if (bodyExpr == null)
                return Expression.New(typeof(ScriptObject));

            var lambdaExpr = Expression.Lambda<Func<ScriptObject, object>>(bodyExpr, paramExpr);

            var newObjExpr = ExpressionHelper.NewScriptObject(ctor: lambdaExpr);

            return newObjExpr;
        }
    }
}
