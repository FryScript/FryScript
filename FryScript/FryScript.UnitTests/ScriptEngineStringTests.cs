using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptEngineStringTests
    {
        private ScriptEngine _scriptEngine;

        [TestInitialize]
        public void TestInitialize()
        {
            _scriptEngine = new ScriptEngine();
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.AreEqual("test" + "test2", Eval("\"test\" + \"test2\";"));
        }

        [TestMethod]
        public void EqualTest()
        {
            Assert.IsTrue(Eval("\"test\" == \"test\";"));
        }

        [TestMethod]
        public void NotEqualTest()
        {
            Assert.IsTrue(Eval("\"test\" != \"test2\";"));
        }

        [TestMethod]
        public void ToStringTest()
        {
            Assert.AreEqual("test", Eval("\"test\".toString();"));
            Assert.IsFalse(Eval("\"false\".toBool();"));
        }

        [TestMethod]
        public void ToBoolTest()
        {
            Assert.IsTrue(Eval("\"true\".toBool();"));
            Assert.IsFalse(Eval("\"false\".toBool();"));
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void ToBoolInvalidTest()
        {
            Eval("\"invalid\".toBool();");
        }

        [TestMethod]
        public void ToIntTest()
        {
            Assert.AreEqual(100, Eval("\"100\".toInt();"));
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void ToIntInvalidTest()
        {
            Eval("\"invalid\".toInt();");
        }

        [TestMethod]
        public void ToFloatTest()
        {
            Assert.AreEqual(10.5f, Eval("\"10.5\".toFloat();"));
        }

        [TestMethod]
        [ExpectedException(typeof (FryScriptException))]
        public void ToFloatInvalidTest()
        {
            Eval("\"invalid\".toFloat();");
        }

        [TestMethod]
        [Ignore]

        public void DefaultPrimitiveStringTest()
        {
            //var obj = _scriptEngine.Get("[string]");
            //Assert.AreEqual(string.Empty, ScriptObject.GetTarget(obj));
        }

        [TestMethod]
        [Ignore]
        public void NewPrimitiveStringTest()
        {
            var obj = Eval("@import \"[string]\" as string; new string();");
            Assert.AreEqual(string.Empty, obj);
        }

        private dynamic Eval(string script)
        {
            return _scriptEngine.Eval(script);
        }
    }
}
