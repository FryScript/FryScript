using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class MultiplyEqualTests : IntegrationTestBase
    {
        [TestMethod]
        public void Multiply_Equal_Variable()
        {
            Eval("var x = 2;");
            Eval("x *= 2;");
            Assert.AreEqual(4, Eval("x;"));
        }

        [TestMethod]
        public void Multiply_Equal_Member()
        {
            Eval("this.x = 10;");
            Eval("this.x *= 10;");
            Assert.AreEqual(100, Eval("this.x;"));
        }

        [TestMethod]
        public void Multiply_Equal_Index()
        {
            Eval("this[\"x\"] = 8;");
            Eval("this[\"x\"] *= 3;");
            Assert.AreEqual(24, Eval("this[\"x\"];"));
        }
    }
}