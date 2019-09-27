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

            var result = Eval("new float();");

            Assert.AreEqual(0.0, result);
        }
    }
}