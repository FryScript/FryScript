using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class RelationalTests : IntegrationTestBase
    {
        [TestMethod]
        public void And()
        {
            Assert.AreEqual(true && true, Eval("true && true;"));
        }

        [TestMethod]
        public void Or()
        {
            Assert.AreEqual(false || true, Eval("false || true;"));
        }

        [TestMethod]
        public void Operator_Precedence()
        {
            Assert.AreEqual(true && false || false && true || true, Eval("true && false || false && true || true;"));
        }
    }
}