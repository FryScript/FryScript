using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests
{
    [TestClass]
    public class RuntimeUriTests
    {
        [ScriptableType("uri")]
        public class UriType
        {
        }

        [TestMethod]
        public void ScriptObjectUri_Value()
        {
            Assert.AreEqual(new Uri("runtime://object.fry"), RuntimeUri.ScriptObjectUri);
        }

        [TestMethod]
        public void ScriptArrayUri_Value()
        {
            Assert.AreEqual(new Uri("runtime://array.fry"), RuntimeUri.ScriptArrayUri);
        }

        [TestMethod]
        public void ScriptFunctionUri_Value()
        {
            Assert.AreEqual(new Uri("runtime://function.fry"), RuntimeUri.ScriptFunctionUri);
        }

        [TestMethod]
        public void ScriptErrorUri_Value()
        {
            Assert.AreEqual(new Uri("runtime://error.fry"), RuntimeUri.ScriptErrorUri);
        }

        [TestMethod]
        public void ScriptFibreUri_Value()
        {
            Assert.AreEqual(new Uri("runtime://fibre.fry"), RuntimeUri.ScriptFibreUri);
        }

        [TestMethod]
        public void ScriptFibreContextUri_Value()
        {
            Assert.AreEqual(new Uri("runtime://fibre-context.fry"), RuntimeUri.ScriptFibreContextUri);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetRuntimeUri_By_Type_Null_Type()
        {
            RuntimeUri.GetRuntimeUri(null as Type);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetRuntimeUri_By_Type_Missing_ScriptableType_Attribute()
        {
            RuntimeUri.GetRuntimeUri(typeof(object));
        }

        [TestMethod]
        public void GetRuntimeUri_By_Type_Success()
        {
            var result = RuntimeUri.GetRuntimeUri(typeof(UriType));

            Assert.AreEqual(new Uri("runtime://uri.fry"), result);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("  ")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetRuntimeUri_By_String_Invalid_Name(string name)
        {
            RuntimeUri.GetRuntimeUri(name);
        }

        [TestMethod]
        public void GetRuntimeUri_By_String_Success()
        {
            var result = RuntimeUri.GetRuntimeUri("test");

            Assert.AreEqual(new Uri("runtime://test.fry"), result);
        }
    }
}