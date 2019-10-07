using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class ModuloEqualTests : IntegrationTestBase
    {
        [TestMethod]
        public void Modulo_Equal_Variable()
        {
            Eval("var x = 65;");
            Eval("x %= 5;");
            Assert.AreEqual(0, Eval("x;"));
        }

        [TestMethod]
        public void Modulo_Equal_Member()
        {
            Eval("this.x = 3;");
            Eval("this.x %= 2;");
            Assert.AreEqual(1, Eval("this.x;"));
        }

        [TestMethod]
        public void Modulo_Equal_Index()
        {
            Eval("this[\"x\"] = 30;");
            Eval("this[\"x\"] %= 9;");
            Assert.AreEqual(3, Eval("this[\"x\"];"));
        }
    }
}