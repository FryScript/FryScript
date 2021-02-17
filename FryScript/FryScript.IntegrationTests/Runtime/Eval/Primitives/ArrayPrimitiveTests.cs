using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Primitives
{
    [TestClass]
    public class ArrayPrimitiveTests : IntegrationTestBase
    {
        [TestMethod]
        public void New_Array()
        {
            Eval("@import \"array\" as array;");

            var result = Eval("new array(10);");

            Assert.AreEqual(10, result.count);
        }
    }
}