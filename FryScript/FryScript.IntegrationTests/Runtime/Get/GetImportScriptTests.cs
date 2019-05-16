using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Get
{
    [TestClass]
    public class GetImportScriptTests : IntegrationTestBase
    {
        [TestMethod]
        public void Import_Script()
        {
            var expected = Get("scripts/importScript");

            var actual = Get("scripts/importingScript");

            Assert.AreEqual(expected, actual.importedScript);
        }

        [TestMethod]
        public void Import_Members_From_Script()
        {
            var expected = Get("scripts/importScript");

            var actual = Get("scripts/importingScript");

            Assert.AreEqual(expected.member1, actual.importedMember1);
            Assert.AreEqual(expected.member2, actual.importedMember2);
        }
    }
}
