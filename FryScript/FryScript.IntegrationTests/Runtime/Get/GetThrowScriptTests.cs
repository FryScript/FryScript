using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Get
{
    [TestClass]
    public class GetThrowScriptTests : IntegrationTestBase
    {
        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void Get_Script_Throws_Exception()
        {
            ScriptRuntime.Get("scripts/throwScript");
        }
    }
}
