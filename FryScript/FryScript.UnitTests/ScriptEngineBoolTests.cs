using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptEngineBoolTests
    {
        private ScriptEngine _scriptEngine;

        [TestInitialize]
        public void TestInitialize()
        {
            _scriptEngine = new ScriptEngine();
        }

        [TestMethod]
        public void EqualTest()
        {
            Assert.IsTrue(Eval("true == true;"));
        }

        [TestMethod]
        public void NotEqualTest()
        {
            Assert.IsTrue(Eval("true != false;"));
        }

        [TestMethod]
        public void ToStringTest()
        {
            Assert.AreEqual("true", Eval("true.toString();"));
            Assert.AreEqual("false", Eval("false.toString();"));
        }

        [TestMethod]
        public void ToBoolTest()
        {
            Assert.AreEqual(true, Eval("true.toBool();"));
            Assert.AreEqual(false, Eval("false.toBool();"));
        }

        [TestMethod]
        public void ToIntTest()
        {
            Assert.AreEqual(1, Eval("true.toInt();"));
            Assert.AreEqual(0, Eval("false.toInt();"));
        }

        [TestMethod]
        public void ToFloatTest()
        {
            Assert.AreEqual(1f, Eval("true.toFloat();"));
            Assert.AreEqual(0f, Eval("false.toFloat();"));
        }

        [TestMethod]
        public void DefaultPrimitiveBooleanTest()
        {
            var obj = _scriptEngine.Get("[bool]");
            Assert.AreEqual(default(bool), ScriptObject.GetTarget(obj));
        }

        [TestMethod]
        public void NewPrimitiveBoolTest()
        {
            var obj = Eval("@import \"[bool]\" as bool; new bool();");
            Assert.AreEqual(false, obj);
        }

        private dynamic Eval(string script)
        {
            return _scriptEngine.Eval(script);
        }
    }
}
