using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.Extensions;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class LiteralNodeTests : AstNodeTestBase<LiteralNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }
        [TestMethod]
        public void GetExpression_Converts_Double_To_Single()
        {
            Node.Configure().Value.Returns(10.0d);

            var result = Node.GetExpression(Scope) as ConstantExpression;

            Assert.IsInstanceOfType(result.Value, typeof(float));
            Assert.AreEqual(10.0f, result.Value);
        }

        [TestMethod]
        public void GetExpression_Generates_Value_Expression()
        {
            Node.Configure().Value.Returns(100);

            var result = Node.GetExpression(Scope) as ConstantExpression;

            Assert.IsInstanceOfType(result.Value, typeof(int));
            Assert.AreEqual(100, result.Value);
        }
    }
}
