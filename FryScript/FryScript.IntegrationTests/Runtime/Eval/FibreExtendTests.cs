using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class FibreExtendTests : IntegrationTestBase
    {
        [TestMethod]
        public void Fibre_Extend()
        {
            Eval("var f = fibre() => { yield 1; };");
            var result = Eval("var fc = f();") as ScriptFibreContext;

            Assert.AreEqual(1, result.Resume());
            Assert.IsTrue(result.Completed);

            Eval("f extend fibre() => { yield 2; };");
            result = Eval("var fc = f();") as ScriptFibreContext;

            Assert.AreEqual(2, result.Resume());
            Assert.IsTrue(result.Completed);
        }

        [TestMethod]
        public void Fibre_Extend_Base()
        {
            Eval("var f = fibre() => { yield 1; };");
            Eval("f extend fibre() => { yield await base(); yield 2; };");

            var result = Eval("var fc = f();") as ScriptFibreContext;

            result.Resume();

            Assert.AreEqual(1, result.Resume());
            Assert.AreEqual(2, result.Resume());
            Assert.IsTrue(result.Completed);
        }
    }
}