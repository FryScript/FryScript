using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class FunctionExtendTests : IntegrationTestBase
    {
        [TestMethod]
        public void Function_Extend()
        {
            Eval("func = () => \"not extended\";");

            Assert.AreEqual("not extended", Eval("func();"));

            Eval("func extend () => \"extended!\";");

            Assert.AreEqual("extended!", Eval("func();"));
        }

        [TestMethod]
        public void Function_Extend_Base()
        {
            Eval("func = name => \"Hello \" + name;");

            Assert.AreEqual("Hello Test", Eval("func(\"Test\");"));

            Eval(@"
            func extend name => {
                var firstPart = base(name);
                return firstPart + "", I was extended!"";
            };");

            Assert.AreEqual("Hello Test, I was extended!", Eval("func(\"Test\");"));
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void Function_Extend_Non_Function()
        {
            Eval("nonFunc = 100;");
            Eval("nonFunc extend () => {};");
        }
    }
}