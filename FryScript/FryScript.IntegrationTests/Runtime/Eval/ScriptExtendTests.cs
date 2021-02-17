using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class EvalScriptExtendTests : IntegrationTestBase
    {
        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Extend_Throws_Exception()
        {
            Eval("@extend \"scripts/noScript\";");
        }

        [TestMethod]
        public void Extend_Array()
        {
            Eval("@import \"Scripts/extendArray\" as arrayExtend;");

            var result = Eval("new arrayExtend(10);");

            Assert.AreEqual("Value 0", result[0]);
            Assert.AreEqual("Value 1", result[1]);
            Assert.AreEqual("Value 2", result[2]);
            Assert.AreEqual("Value 3", result[3]);
            Assert.AreEqual("Value 4", result[4]);
            Assert.AreEqual("Value 5", result[5]);
            Assert.AreEqual("Value 6", result[6]);
            Assert.AreEqual("Value 7", result[7]);
            Assert.AreEqual("Value 8", result[8]);
            Assert.AreEqual("Value 9", result[9]);

        }
    }
}
