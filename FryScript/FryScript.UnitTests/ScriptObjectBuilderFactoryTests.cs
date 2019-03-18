using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptObjectBuilderFactoryTests
    {
        private ScriptObjectBuilderFactory _factory;

        [TestInitialize]
        public void TestIntialize()
        {
            _factory = new ScriptObjectBuilderFactory();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Type()
        {
            _factory.Create(null, o => o, new Uri("test:///name"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Ctor()
        {
            _factory.Create(typeof(object), null, new Uri("test:///name"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Create_Null_Uri()
        {
            _factory.Create(typeof(object), o => o, null);
        }

        [TestMethod]
        public void Create_Success()
        {
            Func<IScriptObject, object> ctor = o => o;
            var uri = new Uri("test:///name");

            var result = _factory.Create(typeof(ScriptObject), ctor, uri);
            var obj = result.Build();

            Assert.AreEqual(uri, result.Uri);
            Assert.IsTrue(obj is ScriptObject);
        }
    }
}
