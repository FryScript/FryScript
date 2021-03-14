using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Dynamic;
using System.Linq.Expressions;

namespace FryScript.IntegrationTests.Runtime.Import
{
    [TestClass]
    public class ImportTypeConversionTests : IntegrationTestBase
    {
        [ScriptableType("importable")]
        public class Importable
        {
        }

        [ScriptableType("no-sub-class", IgnoreTypeFactory = true)]
        public class ImportableNoSubClass : IScriptObject
        {
            public ObjectCore ObjectCore { get; } = new ObjectCore();

            public DynamicMetaObject GetMetaObject(Expression parameter)
            {
                return new MetaScriptObject(parameter, BindingRestrictions.Empty, this);
            }
        }

        [TestMethod]
        public void Imported_Type_Is_Sub_Class_Of_Base()
        {
            ScriptRuntime.Import<Importable>();

            var importable = ScriptRuntime.Get("importable");

            Assert.IsTrue(typeof(Importable).IsAssignableFrom(importable.GetType()));
            Assert.AreNotEqual(importable.GetType(), typeof(Importable));
            Assert.AreEqual(new Uri("runtime://importable.fry"), importable.ObjectCore.Builder.Uri);
        }

        [TestMethod]
        public void Imported_Type_Is_Not_Sub_Class_Of_Base()
        {
            ScriptRuntime.Import<ImportableNoSubClass>();

            IScriptObject importable = ScriptRuntime.Get("no-sub-class");

            Assert.AreEqual(importable.GetType(), typeof(ImportableNoSubClass));
            Assert.AreEqual(new Uri("runtime://no-sub-class.fry"), importable.ObjectCore.Builder.Uri);
        }

        [TestMethod]
        public void Import_Instance_Is_Unmodified()
        {
            var instance = new ScriptObject();

            var imported = ScriptRuntime.Import("instance", instance);

            Assert.AreEqual(instance, imported);

            Assert.AreEqual(new Uri("runtime://instance.fry"), imported.ObjectCore.Builder.Uri);
        }
    }
}
