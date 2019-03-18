using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptObjectBuilderTests
    {
        private ScriptObjectBuilder<ScriptObject> _builder;

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Ctor_Func()
        {
            _builder = new ScriptObjectBuilder<ScriptObject>(null, new Uri("test:///file"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_Null_Uri()
        {
            _builder = new ScriptObjectBuilder<ScriptObject>(o => o, null);
        }

        [TestMethod]
        public void Build_Success()
        {
            var uri = new Uri("test:///file");
            var ctor = new Func<IScriptObject, object>(o =>
            {
                dynamic d = o;
                d.prop = "Property";

                return o;
            });

            _builder = new ScriptObjectBuilder<ScriptObject>(ctor, uri);

            var result = _builder.Build();

            Assert.AreEqual("Property", ((dynamic)result).prop);
            Assert.AreEqual(_builder, result.ObjectCore.Builder);
            Assert.AreEqual(uri, result.ObjectCore.Builder.Uri);
        }
    }
}
