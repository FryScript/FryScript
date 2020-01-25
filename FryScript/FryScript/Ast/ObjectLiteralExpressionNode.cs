using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ObjectLiteralExpressionNode : AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var newObjExpr = Expression.New(typeof(ScriptObject));

            if (ChildNodes.Length == 0)
                return newObjExpr;

            scope = scope.New();

            //var paramExpr = Expression.Parameter(typeof(ScriptObject), scope.GetTempName(TempPrefix.ObjectLiteral));
            var paramExpr = scope.AddMember(scope.GetTempName(TempPrefix.ObjectLiteral), this, typeof(ScriptObject));
            scope.SetData(ScopeData.ObjectLiteralContext, paramExpr);

            var assignParamExpr = Expression.Assign(paramExpr, newObjExpr);

            var bodyExpr = GetChildExpression(scope);

            if (bodyExpr == null)
                return newObjExpr;

            var initObjExpr = scope.ScopeBlock(assignParamExpr, bodyExpr, paramExpr);

            return initObjExpr;

            // var lambdaExpr = Expression.Lambda<Func<ScriptObject, object>>(bodyExpr, paramExpr);

            // var newObjExpr = ExpressionHelper.NewScriptObject(ctor: lambdaExpr);

            // return newObjExpr;
        }
    }
}
