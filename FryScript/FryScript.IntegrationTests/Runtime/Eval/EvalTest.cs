using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class EvalTests : IntegrationTestBase
    {
        [TestMethod]
        public void Eval_Expression()
        {
            var result = Eval("100;");

            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void Eval_Expression_Persists_Eval_State()
        {
            Eval("var x = 100;");

            var result = ScriptRuntime.Eval("x;");

            Assert.AreEqual(100, result);
        }
    }
}
