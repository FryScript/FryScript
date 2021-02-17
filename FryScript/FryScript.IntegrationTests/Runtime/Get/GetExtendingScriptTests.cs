using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Get
{
    [TestClass]
    public class GetExtendingScriptTests : IntegrationTestBase
    {
        [ScriptableType("hostType")]
        public class HostType
        {
            [ScriptableProperty("name")]
            public string Name => "Host Type";
        }

        [TestMethod]
        public void Get_Extending_Script_Test()
        {
            var result = Get("Scripts/extendingScript");

            Assert.AreEqual("Base Script was extended!", result.name);
        }

        [TestMethod]
        public void Get_Extended_Host_Type()
        {
            ScriptRuntime.Import<HostType>();

            var result = Get("Scripts/extendingHost");

            Assert.AreEqual("Host Type", result.name);
            Assert.IsTrue(typeof(HostType).IsAssignableFrom((result as object).GetType()));
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Get_Multiple_Extend_Statements()
        {
            ScriptRuntime.Get("Scripts/extendingMultiple");
        }
    }
}
