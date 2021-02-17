using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class EmptyStatementTests : AstNodeTestBase<EmptyStatement>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }

        [TestMethod]
        public void GetExpression_Empty_Statement()
        {
            var result = Node.GetExpression(Scope) as ConstantExpression;

            Assert.IsNull(result.Value);
        }
    }
}
