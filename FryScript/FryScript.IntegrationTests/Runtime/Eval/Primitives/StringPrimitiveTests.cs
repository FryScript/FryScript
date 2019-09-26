using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Primitives
{
    [TestClass]
    public class StringPrimitiveTests : IntegrationTestBase
    {
        [TestMethod]
        public void New_String()
        {
            Eval("@import \"string\" as string;");

            var result = Eval("new string(\"Test\");");

            Assert.AreEqual("Test", result);
        }
    }
}