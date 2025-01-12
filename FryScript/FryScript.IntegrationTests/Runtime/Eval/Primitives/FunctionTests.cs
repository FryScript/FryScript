using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Primitives
{
    [TestClass]
    public class FunctionTests : IntegrationTestBase
    {
        [TestMethod]
        public void New_Function()
        {
            Eval("@import \"function\" as func;");

            var result = Eval("new func();") as ScriptFunction;
            var func = Eval("func;") as ScriptFunction;

            Assert.IsInstanceOfType(result, typeof(ScriptFunction));
            Assert.AreNotEqual(func, result);
        }

        [TestMethod]
        public void Function_ToString()
        {
            Eval("@import \"function\" as func;");

            var result = Eval("func.toString();");

            Assert.AreEqual("function", result);
        }
    }
}