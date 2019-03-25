using FryScript.Compilation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class CatchStatementNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            throw new NotImplementedException();
        }

        public CatchBlock GetCatchBlock(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            scope = scope.New();

            var identifier = ChildNodes.Skip(1).First() as IdentifierNode;
            var block = ChildNodes.Skip(2).First();

            identifier.CreateIdentifier(scope);
            var exceptionExpr = Expression.Parameter(typeof(Exception), scope.GetTempName(TempPrefix.Exception));
            scope.SetData(ScopeData.CurrentException, exceptionExpr);

            var exMessageExpr = Expression.Call(typeof(FryScriptException),
                "GetCatchObject",
                null,
                exceptionExpr);

            var assignIdentifierExpr = identifier.SetIdentifier(scope, exMessageExpr);

            var newScope = scope.New();
            var blockExpr = block.GetExpression(newScope);

            var catchBlockExpr = scope.ScopeBlock(
                assignIdentifierExpr,
                blockExpr);

            var catchBlock = Expression.Catch(exceptionExpr, catchBlockExpr);

            return catchBlock;
        }
    }
}
