using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ObjectRegistryTests
    {
        private ObjectRegistry _objectRegistry;
        private IScriptObject _scriptObject;

        [TestInitialize]
        public void TestInitialize()
        {
            _objectRegistry = new ObjectRegistry();
            _scriptObject = Substitute.For<IScriptObject>();
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Import_By_Name_Invalid_Name(string name)
        {
            _objectRegistry.Import(name, _scriptObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Import_By_Name_Null_Obj()
        {
            _objectRegistry.Import("test", null);
        }

        [TestMethod]
        public void Import_By_Name_Adds_Obj()
        {
            _objectRegistry.Import("test", _scriptObject);

            Assert.IsTrue(_objectRegistry.TryGetObject("test", out IScriptObject obj));
            Assert.AreEqual(_scriptObject, obj);    
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Import_By_Name_Throws_On_Duplicate_Name()
        {
            _objectRegistry.Import("test", _scriptObject);
            _objectRegistry.Import("test", _scriptObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Import_By_Type_InvalidType()
        {
            _objectRegistry.Import(null as Type);
        }
    }
}
