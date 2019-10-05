using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class DivideEqualTests : IntegrationTestBase
    {
        [TestMethod]
        public void Divide_Equal_Variable()
        {
            Eval("var x = 6;");
            Eval("x /= 3;");
            Assert.AreEqual(2, Eval("x;"));
        }

        [TestMethod]
        public void Divide_Equal_Member()
        {
            Eval("this.x = 16;");
            Eval("this.x /= 4;");
            Assert.AreEqual(4, Eval("this.x;"));
        }

        [TestMethod]
        public void Divide_Equal_Index()
        {
            Eval("this[\"x\"] = 50;");
            Eval("this[\"x\"] /= 10;");
            Assert.AreEqual(5, Eval("this[\"x\"];"));
        }
    }
}