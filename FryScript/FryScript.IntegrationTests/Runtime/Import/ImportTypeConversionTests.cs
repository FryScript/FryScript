using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
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

        [ScriptableType("no-sub-class", IgnoreTypeFactory = true)]
        public class ImportableNoSubClass : IScriptObject
        {
            public ObjectCore ObjectCore => new ObjectCore();

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
        }

        [TestMethod]
        public void Imported_Type_Is_Not_Sub_Class_Of_Base()
        {
            ScriptRuntime.Import<ImportableNoSubClass>();

            IScriptObject importable = ScriptRuntime.Get("no-sub-class");

            Assert.AreEqual(importable.GetType(), typeof(ImportableNoSubClass));
        }
    }
}
