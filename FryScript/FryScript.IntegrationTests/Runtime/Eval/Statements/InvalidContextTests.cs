using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval.Statements
{
    [TestClass]
    public class InvalidContextTests : IntegrationTestBase
    {
        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Continue_Invalid_Outside_Loop()
        {
            Eval("continue;");
        }
    }
}