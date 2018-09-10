using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptableExtensionsTests
    {
        [ScriptableType("scriptable")]
        public class Scriptable : IScriptable
        {
            public dynamic Script { get; set; }
        }

        private Scriptable _scriptable;

        [TestInitialize]
        public void TestInitialize()
        {
            _scriptable = new Scriptable();
        }

        [TestMethod]
        public void BindTest()
        {
            _scriptable.Bind();

            var s = new Scriptable();
            s.Bind();

            Assert.IsNotNull(_scriptable.Script);
            Assert.AreEqual(_scriptable, ScriptObject.GetTarget((ScriptObject)_scriptable.Script));
        }
    }
}
