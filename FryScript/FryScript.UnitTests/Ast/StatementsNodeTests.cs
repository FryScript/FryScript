using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class StatementsNodeTests : AstNodeTestBase<StatementsNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_No_Child_Nodes_Returns_Null()
        {
            Assert.IsNull(Node.GetExpression(Scope));
        }

        [TestMethod]
        public void GetExpression_Has_Child_Nodes_Gets_Child_Expression()
        {
            Node.SetChildren(Node<AstNode>.Empty);

            var expectedExpr = Expression.Constant(new object());
            Node.Configure()
                .GetChildExpression(Scope)
                .Returns(expectedExpr);

            var result = Node.GetExpression(Scope);

            Assert.AreEqual(expectedExpr, result);
        }
    }
}
