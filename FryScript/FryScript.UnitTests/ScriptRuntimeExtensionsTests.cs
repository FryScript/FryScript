using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptRuntimeExtensionsTests
    {
        public enum TestEnum
        {
            Test1, Test2, Test3
        }

        private IScriptRuntime _runtime;
        private IScriptObject _obj;
        private string _enumName;

        [TestInitialize]
        public void TestInitialize()
        {
            _runtime = Substitute.For<IScriptRuntime>();
            _obj = Substitute.For<IScriptObject>();
            _enumName = "enum";
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImportEnum_Null_Source()
        {
            ScriptRuntimeExtensions.ImportEnum<TestEnum>(null, _enumName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ImportEnum_Type_Is_Not_An_Enum()
        {
            _runtime.ImportEnum<object>(_enumName);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ImportEnum_Invalid_Name(string name)
        {
            _runtime.ImportEnum<TestEnum>(name);
        }

        [TestMethod]
        public void ImportEnum_Success()
        {
            dynamic result = _runtime.ImportEnum<TestEnum>(_enumName);

            _runtime.Received().Import(_enumName, Arg.Any<IScriptObject>());
            Assert.AreEqual(TestEnum.Test1, result.Test1);
            Assert.AreEqual(TestEnum.Test2, result.Test2);
            Assert.AreEqual(TestEnum.Test3, result.Test3);
        }
    }
}
