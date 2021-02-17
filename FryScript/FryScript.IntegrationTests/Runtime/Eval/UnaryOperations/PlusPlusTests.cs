using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.UnaryOperations
{
    [TestClass]
    public class PlusPlusTests : IntegrationTestBase
    {
        [TestMethod]
        public void Prefix_Variable_Plus_Plus()
        {
            Eval("var x = 0;");
            Assert.AreEqual(1, Eval("++x;"));
            Assert.AreEqual(1, Eval("x;"));
        }

        [TestMethod]
        public void Prefix_Member_Plus_Plus()
        {
            Eval("this.x = 10;");
            Assert.AreEqual(11, Eval("++this.x;"));
            Assert.AreEqual(11, Eval("this.x;"));

        }

        [TestMethod]
        public void Prefix_Index_Plus_Plus()
        {
            Eval("this[\"x\"] = 100;");
            Assert.AreEqual(101, Eval("++this[\"x\"];"));
            Assert.AreEqual(101, Eval("this[\"x\"];"));
        }

        [TestMethod]
        public void Suffix_Variable_Plus_Plus()
        {
            Eval("var x = 5;");
            Assert.AreEqual(5, Eval("x++;"));
            Assert.AreEqual(6, Eval("x;"));
        }

        [TestMethod]
        public void Suffix_Member_Plus_Plus()
        {
            Eval("this.x = 99;");
            Assert.AreEqual(99, Eval("this.x++;"));
            Assert.AreEqual(100, Eval("this.x;"));
        }

        [TestMethod]
        public void Suffix_Index_Plus_Plus()
        {
            Eval("this[\"x\"] = 99;");
            Assert.AreEqual(99, Eval("this[\"x\"]++;"));
            Assert.AreEqual(100, Eval("this[\"x\"];"));
        }
    }
}