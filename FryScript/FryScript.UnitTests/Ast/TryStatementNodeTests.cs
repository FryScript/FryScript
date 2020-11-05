using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class TryStatementNodeTests : AstNodeTestBase<TryStatementNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_Gets_Block_Expression()
        {
            var blockNode = Node<AstNode>.Empty;

            var expectedExpr = Expression.Constant(new object());
            blockNode.GetExpression(expectedExpr, Scope);

            Node.SetChildren(null, blockNode);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);
        }
    }
}
