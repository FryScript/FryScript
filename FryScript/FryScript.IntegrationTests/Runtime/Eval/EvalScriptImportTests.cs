using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
