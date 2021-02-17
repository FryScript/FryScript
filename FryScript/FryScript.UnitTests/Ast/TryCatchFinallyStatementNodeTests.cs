using FryScript.Ast;
using FryScript.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class TryCatchFinallyStatementNodeTests : AstNodeTestBase<TryCatchFinallyStatementNode>
    {
        private TryStatementNode _tryNode;
        private CatchStatementNode _catchNode;
        private FinallyStatementNode _finallyNode;

        public override void OnTestInitialize()
        {
            _tryNode = Node<TryStatementNode>.Empty;
            _catchNode = Node<CatchStatementNode>.Empty;
            _finallyNode = Node<FinallyStatementNode>.Empty;
        }

        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_Two_Part_Try()
        {
            Node.SetChildren(_tryNode, _catchNode);

            var expectedExpr = Expression.Constant(new object());
            Node.Configure()
                .GetTwoPartTry(Scope)
                .Returns(expectedExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);
        }

        [TestMethod]
        public void GetExpression_Three_Part_Try()
        {
            Node.SetChildren(_tryNode, _catchNode, _finallyNode);

            var expectedExpr = Expression.Constant(new object());
            Node.Configure()
                .GetTryCatchFinally(Scope)
                .Returns(expectedExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);
        }

        [TestMethod]
        public void GetTwoPartTry_Catch_Block()
        {
            Node.SetChildren(_tryNode, _catchNode);

            var expectedExpr = Expression.Constant(new object());
            Node.Configure()
                .GetTryCatch(_tryNode, _catchNode, Scope)
                .Returns(expectedExpr);

            var result = Node.GetTwoPartTry(Scope);

            Assert.AreEqual(expectedExpr, result);
        }

        [TestMethod]
        public void GetTwoPartTry_Finally_Block()
        {
            Node.SetChildren(_tryNode, _finallyNode);

            var expectedExpr = Expression.Constant(new object());
            Node.Configure()
                .GetTryFinally(_tryNode, _finallyNode, Scope)
                .Returns(expectedExpr);

            var result = Node.GetTwoPartTry(Scope);

            Assert.AreEqual(expectedExpr, result);
        }

        [TestMethod]
        public void GetTryCatch_Creates_Try_Catch_Block()
        {
            var expectedTryExpr = Expression.Constant(new object());
            _tryNode.GetExpression(expectedTryExpr, Scope);

            var expectedCatchExpr = Expression.Catch(
                Expression.Parameter(typeof(Exception)),
                Expression.Block(typeof(object), ExpressionHelper.Null()));
            _catchNode.GetCatchBlock(Scope).Returns(expectedCatchExpr);

            var result = Node.GetTryCatch(_tryNode, _catchNode, Scope) as TryExpression;

            Assert.AreEqual(expectedTryExpr, result.Body);
            Assert.AreEqual(null, result.Fault);
        }

        [TestMethod]
        public void GetTryFinally_Creates_Try_Finally_Block()
        {
            var expectedTryExpr = Expression.Constant(new object());
            _tryNode.GetExpression(expectedTryExpr, Scope);

            var expectedFinallyExpr = Expression.Constant(new object());
            _finallyNode.GetExpression(Scope).Returns(expectedFinallyExpr);

            var result = Node.GetTryFinally(_tryNode, _finallyNode, Scope) as TryExpression;

            Assert.AreEqual(expectedTryExpr, result.Body);
            Assert.AreEqual(expectedFinallyExpr, result.Finally);
        }

        [TestMethod]
        public void GetTryCatchFinally_Creates_Try_Catch_Finally_Block()
        {
            var expectedTryExpr = Expression.Constant(new object());
            _tryNode.GetExpression(expectedTryExpr, Scope);

            var expectedCatchExpr = Expression.Catch(
                Expression.Parameter(typeof(Exception)),
                Expression.Block(typeof(object), ExpressionHelper.Null()));
            _catchNode.GetCatchBlock(Scope).Returns(expectedCatchExpr);

            var expectedFinallyExpr = Expression.Constant(new object());
            _finallyNode.GetExpression(Scope).Returns(expectedFinallyExpr);

            Node.SetChildren(_tryNode, _catchNode, _finallyNode);

            var result = Node.GetTryCatchFinally(Scope) as TryExpression;

            Assert.AreEqual(expectedTryExpr, result.Body);
            Assert.AreEqual(null, result.Fault);
            Assert.AreEqual(expectedFinallyExpr, result.Finally);
        }
    }
}
