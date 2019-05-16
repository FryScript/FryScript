using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Eval
{
    [TestClass]
    public class EvalScriptImportTests : IntegrationTestBase
    {
        [TestMethod]
        public void Import_Script()
        {
            Eval("@import \"scripts/importScript\" as importScript;");

            var result = Eval("importScript;");

            Assert.AreEqual("Import Script", result.name);
        }

        [TestMethod]
        public void Import_Members_From_Script()
        {
            Eval(@"@import member1, member2 from ""scripts/importScript"";");
            var result = Eval("{member1: member1, member2: member2};");

            var importScript = Get("scripts/importScript");

            Assert.AreEqual(importScript.member1, result.member1);
            Assert.AreEqual(importScript.member2, result.member2);
        }

        [TestMethod]
        public void Import_Alias_Members_From_Script()
        {
            Eval(@"@import member1 as alias1, member2 as alias2 from ""scripts/importScript"";");
            var result = Eval("{member1: alias1, member2: alias2};");

            var importScript = Get("scripts/importScript");

            Assert.AreEqual(importScript.member1, result.member1);
            Assert.AreEqual(importScript.member2, result.member2);
        }
    }
}
