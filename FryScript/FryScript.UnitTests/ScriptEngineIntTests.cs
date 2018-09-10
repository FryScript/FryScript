using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FryScript.UnitTests
{
    [TestClass]
    public class ScriptEngineIntTests
    {
        [ScriptableType("target")]
        public class Target : IScriptable
        {
            public dynamic Script { get; set; }

            [ScriptableProperty("value")]
            public float Value { get; set; }

            [ScriptableProperty("nullable")]
            public float? Nullable { get; set; }
        }

        private ScriptEngine _scriptEngine;
        private Target _target;
        private dynamic _dynamicTarget;

        [TestInitialize]
        public void TestInitialize()
        {
            _scriptEngine = new ScriptEngine();
            _scriptEngine.Import<Target>();
            _target = _scriptEngine.New<Target>("target");
            _dynamicTarget = _target.Script;
        }

        [TestMethod]
        public void AddTest()
        {
            Assert.AreEqual(5 + 5, Eval("5 + 5;"));
        }

        [TestMethod]
        public void SubtractTest()
        {
            Assert.AreEqual(5 - 2, Eval("5 - 2;"));
        }

        [TestMethod]
        public void MultiplyTest()
        {
            Assert.AreEqual(5*5, Eval("5 * 5;"));
        }

        [TestMethod]
        public void DivideTest()
        {
            Assert.AreEqual(6/3, Eval("6 / 3;"));
        }

        [TestMethod]
        public void ModuloTest()
        {
            Assert.AreEqual(567 % 294, Eval("567%294;"));
        }

        [TestMethod]
        public void EqualTest()
        {
            Assert.IsTrue(Eval("2 == 2;"));
        }

        [TestMethod]
        public void GreaterThanTest()
        {
            Assert.IsTrue(Eval("3 > 2;"));
        }

        [TestMethod]
        public void LessThanTest()
        {
            Assert.IsTrue(Eval("5 < 10;"));
        }

        [TestMethod]
        public void GreaterThanOrEqualTest()
        {
            Assert.IsTrue(Eval("3 >= 1;"));
        }

        [TestMethod]
        public void LessThanOrEqualTest()
        {
            Assert.IsTrue(Eval("2 <= 10;"));
        }

        [TestMethod]
        public void AddFloatTest()
        {
            Assert.AreEqual(5 + 5.5f, Eval("5 + 5.5;"));
        }

        [TestMethod]
        public void SubtractFloatTest()
        {
            Assert.AreEqual(5 - 2.5f, Eval("5 - 2.5;"));
        }

        [TestMethod]
        public void MultiplyFloatTest()
        {
            Assert.AreEqual(5*0.5f, Eval("5 * 0.5;"));
        }

        [TestMethod]
        public void DivideFloatTest()
        {
            Assert.AreEqual(10/0.1f, Eval("10 / 0.1;"));
        }

        [TestMethod]
        public void EqualFloatTest()
        {
            Assert.IsTrue(Eval("2 == 2.0;"));
        }

        [TestMethod]
        public void GreaterThanFloatTest()
        {
            Assert.IsTrue(Eval("3 > 2.0;"));
        }

        [TestMethod]
        public void LessThanFloatTest()
        {
            Assert.IsTrue(Eval("5 < 10.0;"));
        }

        [TestMethod]
        public void GreaterThanOrEqualFloatTest()
        {
            Assert.IsTrue(Eval("3 >= 1.0;"));
        }

        [TestMethod]
        public void LessThanOrEqualFloatTest()
        {
            Assert.IsTrue(Eval("2 <= 10.0;"));
        }

        [TestMethod]
        public void ToStringTest()
        {
            Assert.IsTrue(Eval("100.toString();").GetType() == typeof(string));
            Assert.AreEqual("100", Eval("100.toString();"));
        }

        [TestMethod]
        public void ToBoolTest()
        {
            Assert.IsTrue(Eval("0.toBool();").GetType() == typeof(bool));
            Assert.IsFalse(Eval("0.toBool();"));
            Assert.IsTrue(Eval("1.toBool();"));
        }

        [TestMethod]
        public void ToFloatTest()
        {
            Assert.IsTrue(Eval("100.toFloat();").GetType() == typeof(float));
            Assert.AreEqual(100f, Eval("100.toFloat();"));
        }

        [TestMethod]
        public void ToIntTest()
        {
            Assert.AreEqual(typeof(int), Eval("100.toInt();").GetType());
        }

        [TestMethod]
        public void DefaultPrimitiveIntTest()
        {
            var obj = _scriptEngine.Get("[int]");
            Assert.AreEqual(default(int), ScriptObject.GetTarget(obj));
        }

        [TestMethod]
        public void NewPrimitiveIntTest()
        {
            var obj = Eval("@import \"[int]\" as int; new int();");
            Assert.AreEqual(0, obj);
        }

        [TestMethod]
        public void IntToFloatImplicitTest()
        {
            _dynamicTarget.value = 10;
            Assert.AreEqual(10f, _target.Value);
        }

        [TestMethod]
        public void IntToNullableFloatImplicitTest()
        {
            _dynamicTarget.nullable = 100;
            Assert.AreEqual(new float?(100), _target.Nullable);
        }

        private dynamic Eval(string script)
        {
            return _scriptEngine.Eval(script);
        }
    }
}
