﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.IntegrationTests.Runtime.Get
{
    [TestClass]
    public class GetImportScriptTests : IntegrationTestBase
    {
        [TestMethod]
        public void Import_Script()
        {
            var expected = Get("Scripts/importScript");

            var actual = Get("Scripts/importingScript");

            Assert.AreEqual(expected, actual.importedScript);
        }

        [TestMethod]
        public void Import_Members_From_Script()
        {
            var expected = Get("Scripts/importScript");

            var actual = Get("Scripts/importingScript");

            Assert.AreEqual(expected.member1, actual.importedMember1);
            Assert.AreEqual(expected.member2, actual.importedMember2);
        }

        [TestMethod]
        public void Import_Alias_Members_From_Script()
        {
            var expected = Get("Scripts/importScript");

            var actual = Get("Scripts/importingScript");

            Assert.AreEqual(expected.member1, actual.aliasedMember1);
            Assert.AreEqual(expected.member2, actual.aliasedMember2);
        }
    }
}
