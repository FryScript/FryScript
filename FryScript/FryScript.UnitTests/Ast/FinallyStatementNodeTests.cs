using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class FinallyStatementNodeTests : AstNodeTestBase<FinallyStatementNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        public void GetExpression_Creates_Finally_Expression()
        {
            var node = Node<AstNode>.Empty;

            var expectedExpr = Expression.Constant(new object());
            node.GetExpression(expectedExpr, Scope);

            Node.SetChildren(null, node);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);
        }
    }
}
