using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class ConditionalEqualTest : IntegrationTestBase
    {
        [TestMethod]
        public void Conditional_Equal_Variable()
        {
            Eval("var x = null;");
            Eval("x ?= true;");
            Assert.IsTrue(Eval("x;"));
        }

        [TestMethod]
        public void Conditional_Equal_Member()
        {
            Eval("this.x = null;");
            Eval("this.x ?= \"test\";");
            Assert.AreEqual("test", Eval("this.x;"));
        }

        [TestMethod]
        public void Conditional_Equal_Index()
        {
            Eval("this[\"x\"] = null;");
            Eval("this[\"x\"]  ?= 500;");
            Assert.AreEqual(500, Eval("this[\"x\"];"));
        }

        [TestMethod]
        public void Condition_Equal_Varaiable_Does_Not_Assign()
        {
            Eval("var x = 10;");
            Eval("x ?= true;");
            Assert.AreEqual(10, Eval("x;"));
        }

        [TestMethod]
        public void Conditional_Equal_Member_Does_Not_Assign()
        {
            Eval("this.x = \"test\";");
            Eval("this.x ?= false;");
            Assert.AreEqual("test", Eval("this.x;"));
        }

         [TestMethod]
        public void Conditional_Equal_Index_Does_Not_Assign()
        {
            Eval("this[\"x\"] = 90;");
            Eval("this[\"x\"]  ?= 900;");
            Assert.AreEqual(90, Eval("this[\"x\"];"));
        }
    }
}