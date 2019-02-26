using FryScript.HostInterop;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ObjectRegistryTests
    {
        [ScriptableType("importable")]
        public class Importable
        {

        }

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

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Import_By_Type_With_No_Script_Name()
        {
            _objectRegistry.Import(typeof(object));
        }

        [TestMethod]
        public void Import_By_Type_Adds_Obj()
        {
            _objectRegistry.Import(typeof(Importable));

            Assert.IsTrue(_objectRegistry.TryGetObject("importable", out IScriptObject obj));
            Assert.IsTrue(typeof(Importable).GetTypeInfo().IsAssignableFrom(obj.GetType()));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Import_By_Type_Name_Already_Exists()
        {
            _objectRegistry.Import(typeof(Importable));
            _objectRegistry.Import(typeof(Importable));
        }
    }
}
