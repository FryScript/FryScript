using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Get
{
    [TestClass]
    public class GetPathResolutionTests : IntegrationTestBase
    {
        [TestMethod]
        public void Get_Resolves_Script_With_Same_Name_But_Different_Paths_From_Multiple_Locations()
        {
            dynamic obj = ScriptRuntime.Get("Scripts/ResolutionScripts/SubFolder/inheriting");

            Assert.AreNotEqual(obj.toImport, obj.toImport2);
        }

        [TestMethod]
        public void Get_Resolves_Script_From_Higher_Directory()
        {
            dynamic toResolve = ScriptRuntime.Get("Scripts/ResolutionScripts/toResolve");
            dynamic resolveHigher = ScriptRuntime.Get("Scripts/ResolutionScripts/SubFolder/resolveHigher");
            var imported = resolveHigher.toResolve as ScriptImport;
            Assert.AreEqual(toResolve, imported.Target);
        }
    }
}
