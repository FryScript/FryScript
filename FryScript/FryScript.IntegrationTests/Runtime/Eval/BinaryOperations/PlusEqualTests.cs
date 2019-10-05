using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class PlusEqualTests : IntegrationTestBase
    {
        [TestMethod]
        public void Plus_Equal_Variable()
        {
            Eval("var x = 8;");
            Eval("x += 2;");
            Assert.AreEqual(10, Eval("x;"));
        }

        [TestMethod]
        public void Plus_Equal_Member()
        {
            Eval("this.x = 1001;");
            Eval("this.x += 3;");
            Assert.AreEqual(1004, Eval("this.x;"));
        }

        [TestMethod]
        public void Plus_Equal_Index()
        {
            Eval("this[\"x\"] = 1;");
            Eval("this[\"x\"] += 1;");
            Assert.AreEqual(2, Eval("this[\"x\"];"));
        }
    }
}