using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.UnaryOperations
{
    [TestClass]
    public class NotOperatorTests : IntegrationTestBase
    {
        [TestMethod]
        public void Not()
        {
            Assert.AreEqual(!true, Eval("!true;"));
        }

        [TestMethod]
        public void Operator_Precedence()
        {
            Assert.AreEqual(!(!true && true || !false && false), Eval("!(!true && true || !false && false);"));
        }
    }
}