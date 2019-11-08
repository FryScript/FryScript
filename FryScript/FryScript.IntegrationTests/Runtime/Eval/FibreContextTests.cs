using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class FibreContextTests : IntegrationTestBase
    {
        [TestMethod]
        public void Invoked_Fibre_Returns_Fibre_Context()
        {
            Eval("var f = fibre() => {};");

            var result = Eval("f();");

            Assert.IsInstanceOfType(result, typeof(ScriptFibreContext));
        }

        [TestMethod]
        public void Yield()
        {
            Eval(@"
            var f = fibre () => {
                yield 1;
                yield 2;
                yield 3;
            };

            var fc = f();

            var yield1 = fc.resume();
            var yield2 = fc.resume();
            var yield3 = fc.resume();
            ");

            Assert.AreEqual(1, Eval("yield1;"));
            Assert.AreEqual(2, Eval("yield2;"));
            Assert.AreEqual(3, Eval("yield3;"));
        }

        [TestMethod]
        public void Yield_States()
        {
            Eval(@"
            ");

            Assert.Fail();
        }
    }
}