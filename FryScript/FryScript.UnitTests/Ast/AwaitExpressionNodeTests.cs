using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class AwaitExpressionNodeTests : AstNodeTestBase<AwaitExpressionNode>
    {
        [TestMethod]
        public override void GetExpression_Null_Scope()
        {
            base.GetExpression_Null_Scope();
        }
    }
}
