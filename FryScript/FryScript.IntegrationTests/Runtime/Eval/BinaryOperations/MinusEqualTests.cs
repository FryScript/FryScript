using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class MinusEqualTests : IntegrationTestBase
    {
        [TestMethod]
        public void Minus_Equal_Variable()
        {
            Eval("var x = 8;");
            Eval("x -= 4;");
            Assert.AreEqual(4, Eval("x;"));
        }

        [TestMethod]
        public void Minus_Equal_Member()
        {
            Eval("this.x = 88;");
            Eval("this.x -= 80;");
            Assert.AreEqual(8, Eval("this.x;"));
        }

            [TestMethod]
            public void Minus_Equal_Index()
            {
                Eval("this[\"x\"] = 7;");
                Eval("this[\"x\"] -= 7;");
                Assert.AreEqual(0, Eval("this[\"x\"];"));
            }

    }
}