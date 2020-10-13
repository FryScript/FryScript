using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Primitives
{
    [TestClass]
    public class FibreContextTests : IntegrationTestBase
    {
        [TestMethod]
        public void New_Fibre_Context()
        {
            Eval("@import \"fibre-context\" as fc;");

            var result = Eval("new fc();");
            var fc = Eval("fc;");

            Assert.IsInstanceOfType(result, typeof(ScriptFibreContext));
            Assert.AreNotEqual(fc, result);
        }
    }
}