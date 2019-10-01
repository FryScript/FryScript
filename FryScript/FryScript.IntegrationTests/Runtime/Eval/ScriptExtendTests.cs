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
    }
}
