using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Primitives
{
    [TestClass]
    public class Int32PrimitiveTests : IntegrationTestBase
    {
        [TestMethod]
        public void New_Int32()
        {
            Eval("@import \"int32\" as int;");

            var result = Eval("new int(100);");

            Assert.AreEqual(100, result);
        }
    }
}