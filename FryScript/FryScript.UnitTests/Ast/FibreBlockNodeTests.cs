using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class FibreBlockNodeTests : AstNodeTestBase<FibreBlockNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        public void GetExpression_No_Child_Nodes_Injects_Yield_Statement()
        {
            var expectedStatementNode = Node<StatementNode>.Empty;
            var expectedYieldStatementNode = Node<YieldStatementNode>.Empty;

            Node.Configure()
                .Transform<YieldStatementNode>(null, null)
                .Returns(expectedYieldStatementNode);

            Node.Configure()
                .Transform<StatementNode>(expectedYieldStatementNode)
                .Returns(expectedStatementNode);

            var expectedExpr = Expression.Constant(new object());

            Node.Configure()
                .TransformBlockStatement(expectedStatementNode);

            Node.Configure()
                .GetChildExpression(Scope)
                .Returns(expectedExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);

            Node.Received().TransformBlockStatement(expectedStatementNode);
        }

        [TestMethod]
        public void GetExpression_Last_Child_Is_Expression_Transforms_To_Yield_Statement()
        {
            var childNode = Node<ExpressionNode>.Empty;

            Node.SetChildren(childNode);

            var expectedStatementNode = Node<StatementNode>.Empty;
            var expectedYieldStatementNode = Node<YieldStatementNode>.Empty;

            Node.Configure()
                .Transform<YieldStatementNode>(null, null, childNode)
                .Returns(expectedYieldStatementNode);

            Node.Configure()
                .Transform<StatementNode>(expectedYieldStatementNode)
                .Returns(expectedStatementNode);

            var expectedExpr = Expression.Constant(new object());
            Node.Configure()
                .GetChildExpression(Scope)
                .Returns(expectedExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);
            Assert.AreEqual(expectedStatementNode, Node.ChildNodes.Last());
        }

        [TestMethod]
        public void GetExpression_Last_Child_Is_Not_Expression_Gets_Child_Expression()
        {
            var childNode = Node<AstNode>.Empty;

            Node.SetChildren(childNode);

            var expectedExpr = Expression.Constant(new object());

            Node.Configure()
                .TransformBlockStatement(childNode);
            
            Node.Configure()
                .GetChildExpression(Scope)
                .Returns(expectedExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);

            Node.Received().TransformBlockStatement(childNode);
        }

        [TestMethod]
        public void TransformBlockStatement_No_Children_Creates_Yield_Return()
        {
            var blockStatement = Node<AstNode>.Empty;

            var expectedStatementNode = Node<StatementNode>.Empty;
            var expectedYieldStatementNode = Node<YieldStatementNode>.Empty;

            Node.Configure()
                .Transform<YieldStatementNode>(null, null)
                .Returns(expectedYieldStatementNode);

            Node.Configure()
                .Transform<StatementNode>(expectedYieldStatementNode)
                .Returns(expectedStatementNode);

            Node.TransformBlockStatement(blockStatement);

            Assert.AreEqual(1, blockStatement.ChildNodes.Length);
            Assert.AreEqual(expectedStatementNode, blockStatement.ChildNodes.Single());
        }

        [TestMethod]
        public void TransformBlockStatement_Transforms_Last_Expression_To_Yield_Return()
        {
            var blockStatement = Node<AstNode>.Empty;

            var blockChild1 = Node<AstNode>.Empty;
            var lastStatement = Node<AstNode>.Empty;
            var expression = Node<ExpressionNode>.Empty;

            blockStatement.SetChildren(blockChild1);
            blockChild1.SetChildren(lastStatement);
            lastStatement.SetChildren(expression);

            var transformedExpression = Node<YieldStatementNode>.Empty;
            Node.Configure()
                .Transform<YieldStatementNode>(null, null, expression)
                .Returns(transformedExpression);

            Node.TransformBlockStatement(blockStatement);

            Assert.AreEqual(transformedExpression, lastStatement.ChildNodes.Last());
        }

        [TestMethod]
        public void TransformBlockStatement_Transforms_Last_Yield_To_Yield_Return()
        {
            var blockStatement = Node<AstNode>.Empty;

            var blockChild1 = Node<AstNode>.Empty;
            var lastStatement = Node<AstNode>.Empty;
            var yieldStatement = Node<YieldStatementNode>.Empty;

            blockStatement.SetChildren(blockChild1);
            blockChild1.SetChildren(lastStatement);
            lastStatement.SetChildren(yieldStatement);

            Node.TransformBlockStatement(blockStatement);

            Assert.AreEqual(1, yieldStatement.ChildNodes.Length);
        }

        [TestMethod]
        public void TransformBlockStatement_Does_Not_Transform_Last_Yield_If_Already_Yield_Return()
        {
            var blockStatement = Node<AstNode>.Empty;

            var blockChild1 = Node<AstNode>.Empty;
            var lastStatement = Node<AstNode>.Empty;
            var yieldStatement = Node<YieldStatementNode>.Empty;
            yieldStatement.SetChildren(null, null);

            blockStatement.SetChildren(blockChild1);
            blockChild1.SetChildren(lastStatement);
            lastStatement.SetChildren(yieldStatement);

            Node.TransformBlockStatement(blockStatement);

            Assert.AreEqual(2, yieldStatement.ChildNodes.Length);
        }

        [TestMethod]
        public void TransformBlockStatement_Adds_Implicit_Yield_Return()
        {
            var blockStatement = Node<AstNode>.Empty;

            var blockChild1 = Node<AstNode>.Empty;
            var lastStatement = Node<AstNode>.Empty;
            var other = Node<AstNode>.Empty;

            blockStatement.SetChildren(blockChild1);
            blockChild1.SetChildren(lastStatement);
            lastStatement.SetChildren(other);

            var expectedYield = Node<YieldStatementNode>.Empty;
            Node.Configure()
                .Transform<YieldStatementNode>(null, null)
                .Returns(expectedYield);

            Node.TransformBlockStatement(blockStatement);

            Assert.AreEqual(2, lastStatement.ChildNodes.Length);
            Assert.AreEqual(expectedYield, lastStatement.ChildNodes.Last());
        }
    }
}
