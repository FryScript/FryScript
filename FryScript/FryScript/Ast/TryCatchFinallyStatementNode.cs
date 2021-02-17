using FryScript.Compilation;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.Ast
{
    public class TryCatchFinallyStatementNode: AstNode
    {
        public override Expression GetExpression(Scope scope)
        {
            scope = scope ?? throw new ArgumentNullException(nameof(scope));

            if (ChildNodes.Length == 2)
                return GetTwoPartTry(scope);

            return GetTryCatchFinally(scope);
        }

        protected internal virtual Expression GetTwoPartTry(Scope scope)
        {
            var tryBlock = ChildNodes.First() as TryStatementNode;
            var secondBlock = ChildNodes.Skip(1).First();

            if (secondBlock is CatchStatementNode catchBlock)
                return GetTryCatch(tryBlock, catchBlock, scope);

            var finallyblock = secondBlock as FinallyStatementNode;

            return GetTryFinally(tryBlock, finallyblock, scope);
        }

        protected internal virtual Expression GetTryCatch(TryStatementNode tryBlock, CatchStatementNode catchBlock, Scope scope)
        {
            var tryExpr = tryBlock.GetExpression(scope);
            var catchBlockExpr = catchBlock.GetCatchBlock(scope);

            var tryCatchExpr = Expression.TryCatch(
                tryExpr,
                catchBlockExpr);

            return tryCatchExpr;
        }

        protected internal virtual Expression GetTryFinally(TryStatementNode tryBlock, FinallyStatementNode finallyBlock, Scope scope)
        {
            var tryExpr = tryBlock.GetExpression(scope);
            var finallyExpr = finallyBlock.GetExpression(scope);

            var tryFinallyExpr = Expression.TryFinally(tryExpr, finallyExpr);

            return tryFinallyExpr;
        }

        protected internal virtual Expression GetTryCatchFinally(Scope scope)
        {
            var tryBlock = ChildNodes.First();
            var catchBlock = ChildNodes.Skip(1).First() as CatchStatementNode;
            var finallyBlock = ChildNodes.Skip(2).First();

            var tryExpr = tryBlock.GetExpression(scope);
            var catchBlockExpr = catchBlock.GetCatchBlock(scope);
            var finallyExpr = finallyBlock.GetExpression(scope);

            var tryCatchFinallyExpr = Expression.TryCatchFinally(tryExpr, finallyExpr, catchBlockExpr);

            return tryCatchFinallyExpr;
        }
    }
}
