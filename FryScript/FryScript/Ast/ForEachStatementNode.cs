using FryScript.Compilation;
using FryScript.Helpers;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class ForEachStatementNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            var identifier = ChildNodes.Skip(2).First();
            var obj = ChildNodes.Skip(4).First();
            var statement = ChildNodes.Skip(5).First();

            var enumeratorScope = scope.New(this);
            var loopScope = enumeratorScope.New(this);
            var statementScope = loopScope.New(this);

            var breakTarget = statementScope.SetData(ScopeData.BreakTarget,
                Expression.Label(typeof (object), TempPrefix.BreakTarget));
            var continueTarget = statementScope.SetData(ScopeData.ContinueTarget, Expression.Label(typeof(object), scope.GetTempName(TempPrefix.ContinueTarget)));

            var objExpr = obj.GetExpression(enumeratorScope);
            var enumerableObjExpr = Expression.Call(
                typeof(EnumerableHelper),
                "GetEnumerator",
                null,
                objExpr
                );
            var enumeratorExpr = enumeratorScope.AddTempMember(TempPrefix.Enumerator, this, typeof(IEnumerator));
            var assignEnumeratorExpr = Expression.Assign(enumeratorExpr, enumerableObjExpr);
            
            var enumeratorCurrentExpr = Expression.Property(enumeratorExpr, "Current");

            identifier.CreateIdentifier(statementScope);

            var assignIdentifierExpr = identifier.SetIdentifier(statementScope, enumeratorCurrentExpr);
            var statementExpr = statement.GetExpression(statementScope);
            var statementBlockExpr = statementScope.ScopeBlock(
                assignIdentifierExpr,
                statementExpr
                );

            var ifBreakExpr = Expression.IfThenElse(
                Expression.Call(
                    enumeratorExpr,
                    "MoveNext",
                    null
                    ),
                statementBlockExpr,
                Expression.Break(breakTarget, ExpressionHelper.Null(), typeof(object))
                );
            var continueLabelExpr = Expression.Label(continueTarget, ExpressionHelper.Null());
   
            var loopBodyExpr = loopScope.ScopeBlock(ifBreakExpr, continueLabelExpr);

            var loopExpr = Expression.Loop(loopBodyExpr, breakTarget);

            var initEnumeratorExpr = enumeratorScope.ScopeBlock(assignEnumeratorExpr, loopExpr);
            
            return initEnumeratorExpr;
        }
    }
}
