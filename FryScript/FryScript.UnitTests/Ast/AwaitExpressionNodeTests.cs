using FryScript.Ast;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FryScript.UnitTests.Ast
{
    [TestClass]
    public class AwaitExpressionNodeTests : AstNodeTestBase<AwaitExpressionNode>
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetExpression_Null_Scope()
        {
            Node.GetExpression(null);
        }
    }
}
