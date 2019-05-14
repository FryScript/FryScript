using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptRuntimeExtensionsTests
    {
        private IScriptRuntime _runtime;
        private IScriptObject _obj;

        [TestInitialize]
        public void TestInitialize()
        {
            _runtime = Substitute.For<IScriptRuntime>();
            _obj = Substitute.For<IScriptObject>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Import_Null_Source()
        {
            ScriptRuntimeExtensions.Import<object>(null);
        }

        [TestMethod]
        public void Import_Success()
        {
            _runtime.Import(typeof(object)).Returns(_obj);

            var result = _runtime.Import<object>();

            Assert.AreEqual(_obj, result);
        }
    }
}
