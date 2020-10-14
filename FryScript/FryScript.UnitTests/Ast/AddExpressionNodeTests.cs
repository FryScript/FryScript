using System;
using System.Linq.Expressions;
using FryScript.Ast;
using FryScript.Binders;
using FryScript.Compilation;
using FryScript.Parsing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class AddExpressionNodeTests : AstNodeTestBase<AddExpressionNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        public void GetExpression_Single_Child_Gets_Child_Expression()
        {
            Node.SetChildren(Node<AstNode>.Empty);

            var expr = Expression.Empty();

            ChildExpressionVisitor.GetExpression(Node, Scope).Returns(expr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expr, result);
        }

        [TestMethod]
        public void GetExpression_Op_Add()
        {
            var rightNode = Node<AstNode>.WithValue(1);
            var opNode = Node<AstNode>.WithValueString(Operators.Add);
            var leftNode = Node<AstNode>.WithValue(1);

            Node.SetChildren(rightNode, opNode, leftNode);

            var result = Node.GetExpression(Scope) as DynamicExpression;

            rightNode.Received().GetExpression(Scope);
            leftNode.Received().GetExpression(Scope);

            Assert.IsInstanceOfType(result.Binder, typeof(ScriptBinaryOperationBinder));
            Assert.AreEqual(2, result.Arguments.Count);
            Assert.AreEqual(ExpressionType.Add, (result.Binder as ScriptBinaryOperationBinder).Operation);
        }

        [TestMethod]
        public void GetExpression_Op_Subtract()
        {
            var rightNode = Node<AstNode>.WithValue(1);
            var opNode = Node<AstNode>.WithValueString(Operators.Subtract);
            var leftNode = Node<AstNode>.WithValue(1);

            Node.SetChildren(rightNode, opNode, leftNode);

            var result = Node.GetExpression(Scope) as DynamicExpression;

            rightNode.Received().GetExpression(Scope);
            leftNode.Received().GetExpression(Scope);

            Assert.IsInstanceOfType(result.Binder, typeof(ScriptBinaryOperationBinder));
            Assert.AreEqual(2, result.Arguments.Count);
            Assert.AreEqual(ExpressionType.Subtract, (result.Binder as ScriptBinaryOperationBinder).Operation);
        }
    }
}