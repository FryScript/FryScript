using FryScript.Ast;
using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class BlockStatementNodeTests : AstNodeTestBase<BlockStatementNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }

        [TestMethod]
        public void GetExpression_No_Children()
        {
            var result = Node.GetExpression(Scope) as ConstantExpression;

            Assert.AreEqual(null, result.Value);
        }

        [TestMethod]
        public void Get_Expression_Creates_New_Scope_Block()
        {
            var expectedExpr = Expression.Constant(new object());

            Node.Configure()
                .GetChildExpression(Arg.Is<Scope>(s => s.Parent == Scope))
                .Returns(expectedExpr);

            Node.SetChildren(Node<AstNode>.Empty);

            var result = Node.GetExpression(Scope) as BlockExpression;

            Assert.AreEqual(1, result.Expressions.Count);
            Assert.AreEqual(expectedExpr, result.Expressions[0]);
        }
    }
}
