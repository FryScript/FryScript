using FryScript.Ast;
using FryScript.Binders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class NewExpressionNodeTests : AstNodeTestBase<NewExpressionNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Getexpression_Null_Scope()
        {
            Node.GetExpression(Scope);
        }

        [TestMethod]
        public override void GetExpression_Single_Child_Gets_Child_Expression()
        {
            base.GetExpression_Single_Child_Gets_Child_Expression();
        }

        [TestMethod]
        public void GetExpression_Generates_New_Expression()
        {
            var expectedIdentifierExpr = Expression.Constant(new object());
            var targetNode = Node<AstNode>.Empty;
            targetNode.GetIdentifier(Scope).Returns(expectedIdentifierExpr);

            var expectedArgExpr = Expression.Constant(new object());
            var argNode = Node<AstNode>.Empty;
            argNode.GetExpression(expectedArgExpr, Scope);

            var argsNode = Node<AstNode>.Empty;
            argsNode.SetChildren(argNode);

            var invokeNode = Node<AstNode>.Empty;
            invokeNode.SetChildren(targetNode, argsNode);

            Node.SetChildren(null, invokeNode);

            var result = Node.GetExpression(Scope) as DynamicExpression;

            Assert.AreEqual(2, result.Arguments.Count);
            Assert.AreEqual(expectedIdentifierExpr, result.Arguments[0]);
            Assert.AreEqual(expectedArgExpr, result.Arguments[1]);
            Assert.IsInstanceOfType(result.Binder, typeof(ScriptCreateInstanceBinder));
        }
    }
}
