using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Primitives
{
    [TestClass]
    public class FloatPrimitiveTests : IntegrationTestBase
    {
        [TestMethod]
        public void New_Float()
        {
            Eval("@import \"single\" as float;");

            var result = Eval("new float(100.5);");

            Assert.AreEqual(100.5, result);
        }
    }
}