using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Primitives
{
    [TestClass]
    public class ScriptFibreTests : IntegrationTestBase
    {
        [TestMethod]
        public void New_Fibre_Context()
        {
            Eval("@import \"fibre\" as f;");

            var result = Eval("new f();") as ScriptFibre;
            var f = Eval("f;") as ScriptFibre;

            Assert.IsInstanceOfType(result, typeof(ScriptFibre));
            Assert.AreNotEqual(f, result);
        }

        [TestMethod]
        public void Fibre_ToString()
        {
            Eval("@import \"fibre\" as f;");

            var result = Eval("f.toString();");

            Assert.AreEqual("fibre", result);
        }
    }
}