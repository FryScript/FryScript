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

            var result = Eval("new fc();") as ScriptFibreContext;
            var fc = Eval("fc;") as ScriptFibreContext;

            Assert.IsInstanceOfType(result, typeof(ScriptFibreContext));
            Assert.AreNotEqual(fc, result);
        }

        [TestMethod]
        public void Fibre_Context_ToString()
        {
            Eval("@import \"fibre-context\" as fc;");

            var result = Eval("fc.toString();");

            Assert.AreEqual("fibre-context", result);
        }
    }
}