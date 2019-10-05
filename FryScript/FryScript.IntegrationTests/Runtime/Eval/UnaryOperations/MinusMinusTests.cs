using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.UnaryOperations
{
    [TestClass]
    public class MinusMinusTests : IntegrationTestBase
    {
        [TestMethod]
        public void Prefix_Variable_Minus_Minus()
        {
            Eval("var x = 1;");
            Assert.AreEqual(0, Eval("--x;"));
            Assert.AreEqual(0, Eval("x;"));
        }

        [TestMethod]
        public void Prefix_Member_Minus_Minus()
        {
            Eval("this.x = 5;");
            Assert.AreEqual(4, Eval("--this.x;"));
            Assert.AreEqual(4, Eval("this.x;"));
        }

        [TestMethod]
        public void Prefix_Index_Minus_Minus()
        {
            Eval("this[\"test\"] = 10;");
            Assert.AreEqual(9, Eval("--this[\"test\"];"));
            Assert.AreEqual(9, Eval("this[\"test\"];"));
        }

        [TestMethod]
        public void Suffix_Variable_Minus_Minus()
        {
            Eval("var x = 90;");
            Assert.AreEqual(90, Eval("x--;"));
            Assert.AreEqual(89, Eval("x;"));
        }


        [TestMethod]
        public void Suffix_Member_Minus_Minus()
        {
            Eval("this.x = 3;");
            Assert.AreEqual(3, Eval("this.x--;"));
            Assert.AreEqual(2, Eval("this.x;"));
        }


        [TestMethod]
        public void Suffix_Index_Minus_Minus()
        {
            Eval("this[\"x\"] = 66;");
            Assert.AreEqual(66, Eval("this[\"x\"]--;"));
            Assert.AreEqual(65, Eval("this[\"x\"];"));
        }
    }
}