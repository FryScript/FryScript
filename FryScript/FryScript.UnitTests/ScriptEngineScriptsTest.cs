using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests
{
    using Compilation;
    using FryScript.Compilation;

    [TestClass]
    public class ScriptEngineScriptsTest
    {
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
            dynamic obj = _scriptEngine.Get("scripts/simpleImport");
            Assert.IsTrue(obj.member);
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void CircularImportTest()
        {
            _scriptRuntime.Get("scripts/circularImport1");
        }

        [TestMethod]
        [ExpectedException(typeof(CompilerException))]
        public void CircularExtendTest()
        {
            _scriptEngine.Get("scripts/circularExtend1");
        }

        [TestMethod]
        public void NestedImportTest()
        {
            dynamic nested3 = _scriptEngine.Get("scripts/nestedImport3");
            dynamic nested2 = _scriptEngine.Get("scripts/nestedImport2");
            dynamic nested1 = _scriptEngine.Get("scripts/nestedImport1");

            Assert.AreEqual(nested2, nested3.nestedImport2);
            Assert.AreEqual(nested1, nested3.nestedImport1);

            Assert.AreEqual(nested1, nested2.nestedImport1);
        }

        [TestMethod]
        public void ProtoImportTest()
        {
            dynamic protoImport = _scriptEngine.Get("scripts/protoImport");

            Assert.AreEqual(true, protoImport.importedMember);
        }

        [TestMethod]
        public void NestedExceptionTest()
        {
            try
            {
                _scriptEngine.Get("scripts/errorHandling1");
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
        public void ImportAllMembersTest()
        {
            dynamic imports = _scriptEngine.Get("scripts/importAllMembers");
            dynamic exports = _scriptEngine.Get("scripts/exportMembers");

            Assert.AreEqual(exports.member1, imports.importedMember1);
            Assert.AreEqual(exports.member2, imports.importedMember2);
        }

        [TestMethod]
        public void ImportNamedMembersTest()
        {
            dynamic imports = _scriptEngine.Get("scripts/importNamedMembers");
            dynamic exports = _scriptEngine.Get("scripts/exportMembers");

            Assert.AreEqual(exports.member2, imports.importedMember2);
            Assert.AreEqual(exports.member3, imports.importedMember3);

        }
    }
}
