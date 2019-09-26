using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptEngineFloatTests
    {
        [ScriptableType("target")]
        public class Target : IScriptable
        {
            public dynamic Script { get; set; }

            [ScriptableProperty("value")]
            public int Value { get; set; }

            [ScriptableProperty("nullable")]
            public int? Nullable { get; set; }
        }

        private ScriptEngine _scriptEngine;
        //private Target _target;
        //private dynamic _dynamicTarget;

        [TestInitialize]
        public void TestInitialize()
        {
            _scriptEngine = new ScriptEngine();
            _scriptEngine.Import<Target>();
            //_target = _scriptEngine.New<Target>("target");
            //_dynamicTarget = _target.Script;
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.AreEqual(5.5f + 5.5f, Eval("5.5 + 5.5;"));
        }

        [TestMethod]
        public void SubtractTest()
        {
            Assert.AreEqual(5.5f - 2.6f, Eval("5.5 - 2.6;"));
        }

        [TestMethod]
        public void MultiplyTest()
        {
            Assert.AreEqual(5.25f * 5.6f, Eval("5.25 * 5.6;"));
        }

        [TestMethod]
        public void DivideTest()
        {
            Assert.AreEqual(6.6f / 3.7f, Eval("6.6 / 3.7;"));
        }

        [TestMethod]
        public void ModuloTest()
        {
            Assert.AreEqual(6.78f%23.9, Eval("6.78 % 23.9;"));
        }

        [TestMethod]
        public void EqualTest()
        {
            Assert.IsTrue(Eval("2.45 == 2.45;"));
        }

        [TestMethod]
        public void GreaterThanTest()
        {
            Assert.IsTrue(Eval("3.78 > 2.25;"));
        }

        [TestMethod]
        public void LessThanTest()
        {
            Assert.IsTrue(Eval("5.8 < 10.5;"));
        }

        [TestMethod]
        public void GreaterThanOrEqualTest()
        {
            Assert.IsTrue(Eval("3.9 >= 1.5;"));
        }

        [TestMethod]
        public void LessThanOrEqualTest()
        {
            Assert.IsTrue(Eval("2.5 <= 10.1;"));
        }

        [TestMethod]
        public void AddIntTest()
        {
            Assert.AreEqual(5.5f + 5, Eval("5.5 + 5;"));
        }

        [TestMethod]
        public void SubtractIntTest()
        {
            Assert.AreEqual(5.5f - 2, Eval("5.5 - 2;"));
        }

        [TestMethod]
        public void MultiplyIntTest()
        {
            Assert.AreEqual(5.25f * 5, Eval("5.25 * 5;"));
        }

        [TestMethod]
        public void DivideIntTest()
        {
            Assert.AreEqual(6.6f / 3, Eval("6.6 / 3;"));
        }

        [TestMethod]
        public void EqualIntTest()
        {
            Assert.IsTrue(Eval("2.45 == 2.45;"));
        }

        [TestMethod]
        public void GreaterThanIntTest()
        {
            Assert.IsTrue(Eval("3.78 > 2.25;"));
        }

        [TestMethod]
        public void LessThanIntTest()
        {
            Assert.IsTrue(Eval("5.8 < 10.5;"));
        }

        [TestMethod]
        public void GreaterThanOrEqualIntTest()
        {
            Assert.IsTrue(Eval("3.9 >= 1.5;"));
        }

        [TestMethod]
        public void LessThanOrEqualIntTest()
        {
            Assert.IsTrue(Eval("2.5 <= 10.1;"));
        }

        [TestMethod]
        public void ToStringTest()
        {
            Assert.AreEqual("1.5", Eval("1.5.toString();"));
        }

        [TestMethod]
        public void ToBoolTest()
        {
            Assert.IsTrue(Eval("1.0.toBool();"));
            Assert.IsFalse(Eval("0.0.toBool();"));
        }

        [TestMethod]
        public void ToFloatTest()
        {
            Assert.AreEqual(100.4f, Eval("100.4.toFloat();"));
        }

        [TestMethod]
        public void ToIntTest()
        {
            Assert.AreEqual(100, Eval("100.4.toInt();"));
        }

        [TestMethod]
        public void DefaultPrimitiveFloatTest()
        {
            var obj = _scriptEngine.Get("[float]") as ScriptPrimitive<bool>;
            Assert.AreEqual(default(float), obj.Target);
        }

        [TestMethod]
        public void NewPrimitiveFloatTest()
        {
            var obj = Eval("@import \"[float]\" as float; new float();");
            Assert.AreEqual(0f, obj);
        }

        private dynamic Eval(string script)
        {
            return _scriptEngine.Eval(script);
        }
    }
}
