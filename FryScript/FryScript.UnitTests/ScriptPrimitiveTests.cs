using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FryScript.UnitTests
{
    using Binders;
    using System.Runtime.CompilerServices;

    [TestClass]
    public class ScriptPrimitiveTests
    {
        private ScriptPrimitive<int> _scriptPrimitive;
        private dynamic _dynamicPrimitive;

        [TestInitialize]
        public void TestInitialize()
        {
            _scriptPrimitive = new ScriptPrimitive<int>();
            _dynamicPrimitive = _scriptPrimitive;
        }

        [TestMethod]
        public void GetMemberTest()
        {
            var result = _dynamicPrimitive.toString;
            Assert.IsTrue(result is ScriptFunction);
        }

        [TestMethod]
        [ExpectedException(typeof(FryScriptException))]
        public void SetMemberTest()
        {
            _dynamicPrimitive.invalid = 100;
        }

        [TestMethod]
        public void GetMemberInvoke()
        {
            var result = _dynamicPrimitive.toString();
            Assert.AreEqual("0", result);
        }

        [TestMethod]
        public void Add()
        {
            var result = _dynamicPrimitive + 100;
            Assert.AreEqual(100, result);
        }

        [TestMethod]
        public void Subtract()
        {
            var result = _dynamicPrimitive - 100;
            Assert.AreEqual(-100, result);
        }

        [TestMethod]
        public void Multiply()
        {
            var result = _dynamicPrimitive * 100;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Divide()
        {
            var result = _dynamicPrimitive / 10;
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void Convert()
        {
            var convert = CallSite<Func<CallSite, object, float>>.Create(BinderCache.Current.ConvertBinder(typeof(float)));
            float result = convert.Target(convert, _dynamicPrimitive);

            Assert.AreEqual(typeof(float), result.GetType());
        }
    }
}
