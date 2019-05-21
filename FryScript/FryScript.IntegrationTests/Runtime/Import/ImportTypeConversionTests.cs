using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace FryScript.IntegrationTests.Runtime.Import
{
    [TestClass]
    public class ImportTypeConversionTests : IntegrationTestBase
    {
        [ScriptableType("importable")]
        public class Importable
        {
        }

        [TestMethod]
        public void Import_Type_Converts_To_Base_Type()
        {
            ScriptRuntime.Import<Importable>();

            var importable = ScriptRuntime.Get("importable");

            Assert.IsTrue(typeof(Importable).IsAssignableFrom(importable.GetType()));
        }
    }
}
