using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.BinaryOperations
{
    [TestClass]
    public class BooleanOperatorTests : IntegrationTestBase
    {
        [TestMethod]
        public void Boolean_Equal()
        {
            var result = Eval("true == true;");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Boolean_Not_Equal()
        {
            var result = Eval("true == false;");

            Assert.IsFalse(result);
        }
    }
}