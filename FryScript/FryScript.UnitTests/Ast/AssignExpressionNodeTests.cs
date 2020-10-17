using FryScript.Ast;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class AssignExpressionNodeTests : AstNodeTestBase<AssignExpressionNode>
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
            TestSingleChildNode();
        }

        [TestMethod]
        public void GetExpression_Assigns_Right_To_Left()
        {
            var rightExpr = Expression.Constant(true);
            var right = Node<AstNode>.Empty.GetExpression(rightExpr);

            var leftExpr = Expression.Constant(true);
            var left = Node<IdentifierExpressionNode>.Empty;

            left.SetIdentifier(Scope, rightExpr).Returns(leftExpr);
            
            Node.SetChildren(left, null, right);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(leftExpr, result);
        }
    }
}
