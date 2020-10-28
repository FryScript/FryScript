using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class NullNodeTests : AstNodeTestBase<NullNode>
    {
        [TestMethod]
        public void GetExpression_Generates_Null_Constant()
        {
            var result = Node.GetExpression(Scope) as ConstantExpression;

            Assert.IsNull(result.Value);
            Assert.AreEqual(typeof(object), result.Type);
        }
    }
}
