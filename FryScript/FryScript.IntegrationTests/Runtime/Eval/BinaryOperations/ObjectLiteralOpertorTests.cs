using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class ObjectLiteralOperatorTests : IntegrationTestBase
    {
        [TestMethod]
        public void Object_Equal()
        {
            Eval("var obj = {};");
            Eval("var obj2 = {};");

            Assert.IsFalse(Eval("obj1 == obj2;"));
        }

        [TestMethod]
        public void Object_Not_Equal()
        {
            Eval("var obj = {};");
            Eval("var obj2 = {};");

            Assert.IsTrue(Eval("obj1 != obj2;"));
        }
    }
}