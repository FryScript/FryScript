using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class AssignmentTests : IntegrationTestBase
    {
        [TestMethod]
        public void Multiple_Assignment()
        {
            Eval("var obj1; var obj2;");

            Eval("obj1 = obj2 = false;");

            Assert.IsFalse(Eval("obj1;"));
            Assert.IsFalse(Eval("obj2;"));
        }

        [TestMethod]
        public void Multiple_Index_Assignment()
        {
            Eval("var obj = {};");
            Eval("obj[\"member1\"] = obj[\"member2\"] = \"test\";");

            Assert.AreEqual("test", Eval("obj[\"member1\"];"));
            Assert.AreEqual("test", Eval("obj[\"member2\"];"));
        }
    }
}