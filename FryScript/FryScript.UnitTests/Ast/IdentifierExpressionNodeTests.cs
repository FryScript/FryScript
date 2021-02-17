using FryScript.Ast;
using FryScript.Binders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class IdentifierExpressionNodeTests : AstNodeTestBase<IdentifierExpressionNode>
    {
        [TestMethod]
        public void GetExpression_Calls_GetIdentifier()
        {
            var expectedIdentifierExpr = Expression.Constant(new object());

            Node
                .Configure()
                .GetIdentifier(Scope)
                .Returns(expectedIdentifierExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedIdentifierExpr, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetIdentifier_Null_Scope()
        {
            Node.GetIdentifier(null);
        }

        [TestMethod]
        public void GetIdentifier_Gets_Identifier()
        {
            var expectedIdentifierExpr = Expression.Constant(new object());
            var identifierNode = Node<IdentifierNode>.Empty;
            identifierNode.GetIdentifier(Scope).Returns(expectedIdentifierExpr);

            Node.SetChildren(identifierNode);

            var result = Node.GetIdentifier(Scope);


            Assert.AreEqual(expectedIdentifierExpr, result);
        }

        [TestMethod]
        public void GetIdentifier_Gets_Member()
        {
            var expectedLeftExpr = Expression.Constant(new object());
            var leftNode = Node<AstNode>.Empty;
            leftNode.GetExpression(expectedLeftExpr, Scope);

            var rightNode = Node<IdentifierNode>.WithValueString("member");

            Node.SetChildren(leftNode, rightNode);

            var result = Node.GetIdentifier(Scope) as DynamicExpression;

            Assert.AreEqual(ExpressionType.Dynamic, result.NodeType);
            Assert.AreEqual(1, result.Arguments.Count);
            Assert.AreEqual(expectedLeftExpr, result.Arguments[0]);

            var binder = result.Binder as ScriptGetMemberBinder;
            Assert.AreEqual("member", binder.Name);
        }

        [TestMethod]
        public void GetIdentifier_Gets_Index()
        {
            var expectedLeftExpr = Expression.Constant(new object());
            var leftNode = Node<AstNode>.Empty;
            leftNode.GetExpression(expectedLeftExpr, Scope);

            var expectedRightExpr = Expression.Constant(new object());
            var rightNode = Node<IndexNode>.Empty;
            rightNode.GetExpression(Scope).Returns(expectedRightExpr);

            Node.SetChildren(leftNode, rightNode);

            var result = Node.GetIdentifier(Scope) as DynamicExpression;

            Assert.AreEqual(ExpressionType.Dynamic, result.NodeType);
            Assert.AreEqual(2, result.Arguments.Count);
            Assert.AreEqual(expectedLeftExpr, result.Arguments[0]);            
            Assert.AreEqual(expectedRightExpr, result.Arguments[1]);

            Assert.IsInstanceOfType(result.Binder, typeof(ScriptGetIndexBinder));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetIdentifier_Unexpected_Right_Hand_Node()
        {
            var expectedLeftExpr = Expression.Constant(new object());
            var leftNode = Node<AstNode>.Empty;
            leftNode.GetExpression(expectedLeftExpr, Scope);

            var rightNode = Node<AstNode>.Empty;

            Node.SetChildren(leftNode, rightNode);

            Node.GetIdentifier(Scope);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetIdentifier_Null_Scope()
        {
            Node.SetIdentifier(null, Expression.Empty());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetIdentifier_Null_Expression()
        {
            Node.SetIdentifier(Scope, null);
        }

        [TestMethod]
        public void SetIdentifier_Sets_Identifier()
        {
            var valueExpr = Expression.Constant(new object());
            var expectedSetIdentifierExpr = Expression.Constant(new object());
            var identifierNode = Node<IdentifierNode>.Empty;
            identifierNode.SetIdentifier(Scope, valueExpr)
                .Returns(expectedSetIdentifierExpr);

            Node.SetChildren(identifierNode);

            var result = Node.SetIdentifier(Scope, valueExpr);

            Assert.AreEqual(expectedSetIdentifierExpr, result);
        }

        [TestMethod]
        public void SetIdentifier_Sets_Member()
        {
            var valueExpr = Expression.Constant(new object());
            var expectedLeftExpr = Expression.Constant(new object());
            var leftNode = Node<AstNode>.Empty;
            leftNode.GetExpression(expectedLeftExpr, Scope);

            var rightNode = Node<IdentifierNode>.WithValueString("member");

            Node.SetChildren(leftNode, rightNode);

            var result = Node.SetIdentifier(Scope, valueExpr) as DynamicExpression;

            Assert.AreEqual(ExpressionType.Dynamic, result.NodeType);
            Assert.AreEqual(2, result.Arguments.Count);
            Assert.AreEqual(expectedLeftExpr, result.Arguments[0]);
            Assert.AreEqual(valueExpr, result.Arguments[1]);

            var binder = result.Binder as ScriptSetMemberBinder;
            Assert.AreEqual("member", binder.Name);
        }

        [TestMethod]
        public void SetIdentifier_Sets_Index()
        {
            var valueExpr = Expression.Constant(new object());
            var expectedLeftExpr = Expression.Constant(new object());
            var leftNode = Node<AstNode>.Empty;
            leftNode.GetExpression(expectedLeftExpr, Scope);

            var expectedRightExpr = Expression.Constant(new object());
            var rightNode = Node<IndexNode>.Empty;
            rightNode.GetExpression(expectedRightExpr, Scope);

            Node.SetChildren(leftNode, rightNode);

            var result = Node.SetIdentifier(Scope, valueExpr) as DynamicExpression;

            Assert.AreEqual(ExpressionType.Dynamic, result.NodeType);
            Assert.AreEqual(3, result.Arguments.Count);
            Assert.AreEqual(expectedLeftExpr, result.Arguments[0]);
            Assert.AreEqual(expectedRightExpr, result.Arguments[1]);
            Assert.AreEqual(valueExpr, result.Arguments[2]);

            Assert.IsInstanceOfType(result.Binder, typeof(ScriptSetIndexBinder));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetIdentifier_Unexpected_Right_Hand_Node()
        {
            var valueExpr = Expression.Constant(new object());
            var leftNode = Node<AstNode>.Empty;
            var rightNode = Node<AstNode>.Empty;

            Node.SetChildren(leftNode, rightNode);

            Node.SetIdentifier(Scope, valueExpr);
        }
    }
}
