using FryScript.Compilation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FryScript.UnitTests
{

    [TestClass]
    public class ScriptEngineScriptsTest
    {
        [ScriptableType("importType")]
        public class ImportType { }

        private ScriptEngine _scriptEngine;
        private ScriptRuntime _scriptRuntime;

        [TestInitialize]
        public void TestInitialize()
        {
            _scriptEngine = new ScriptEngine();
            _scriptRuntime = new ScriptRuntime();
        }

        [TestMethod]
        public void LocateAndCompileTest()
        {
            dynamic obj = _scriptRuntime.Get("scripts/simpleImport");
            Assert.IsTrue(obj.member);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Get_Script_With_Circular_Import()
        {
            _scriptRuntime.Get("scripts/circularImport1");
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void Get_Script_With_Circular_Extend()
        {
            _scriptRuntime.Get("scripts/circularExtend1");
        }

        [TestMethod]
        public void Get_Script_With_Correct_Extend()
        {
            var baseScript = _scriptRuntime.Get("scripts/baseScript");
            var extendingScript = _scriptRuntime.Get("scripts/extendingScript");

            dynamic dExtendingScript = extendingScript;

            Assert.AreEqual("Base script", dExtendingScript.name);
            Assert.AreEqual("My name is Base script", dExtendingScript.getName());
            Assert.IsTrue(baseScript.GetType() != extendingScript.GetType());
            Assert.IsTrue(baseScript.GetType().IsAssignableFrom(extendingScript.GetType()));
        }

        //[TestMethod]
        //public void NestedImportTest()
        //{
        //    dynamic nested3 = _scriptEngine.Get("scripts/nestedImport3");
        //    dynamic nested2 = _scriptEngine.Get("scripts/nestedImport2");
        //    dynamic nested1 = _scriptEngine.Get("scripts/nestedImport1");

        //    Assert.AreEqual(nested2, nested3.nestedImport2);
        //    Assert.AreEqual(nested1, nested3.nestedImport1);

        //    Assert.AreEqual(nested1, nested2.nestedImport1);
        //}

        //[TestMethod]
        //public void ProtoImportTest()
        //{
        //    dynamic protoImport = _scriptEngine.Get("scripts/protoImport");

        //    Assert.AreEqual(true, protoImport.importedMember);
        //}

        [TestMethod]
        public void Get_Imported_Type()
        {
            _scriptRuntime.Import(typeof(ImportType));

            var importType = _scriptRuntime.Get("importType");

            Assert.IsNotNull(importType);
        }

        [TestMethod]
        public void NestedExceptionTest()
        {
            try
            {
                _scriptRuntime.Get("scripts/errorHandling1");
            }
            catch(FryScriptException ex)
            {
                var count = 2;
                dynamic data = ex.ScriptData;
                Exception currentEx = ex;
                while(data != null)
                {
                    var errorMessage = "Error " + count;
                    Assert.AreEqual(errorMessage, currentEx.Message);
                    Assert.AreEqual(errorMessage, data.message);
                    count--;
                    data = data.innerObject;
                    currentEx = currentEx.InnerException;
                }
            }
        }

        [TestMethod]
        public void ImportNamedMembersTest()
        {
            dynamic imports = _scriptRuntime.Get("scripts/importNamedMembers");
            dynamic exports = _scriptRuntime.Get("scripts/exportMembers");

            Assert.AreEqual(exports.member2, imports.importedMember2);
            Assert.AreEqual(exports.member3, imports.importedMember3);

        }
    }
}
